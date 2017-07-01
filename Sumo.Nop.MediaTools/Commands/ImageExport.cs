using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ManyConsole;
using Sumo.Nop.MediaTools.Common;
using Sumo.Nop.MediaTools.Models;

// ReSharper disable UnusedMember.Global

namespace Sumo.Nop.MediaTools.Commands {
    public class ImageExport : ConsoleCommand {
        private readonly ImageExportOptions _options;

        public ImageExport() {
            IsCommand("ImageExport", $"Exports images from nopCommerce v{Assembly.GetEntryAssembly().GetName().Version} safely.");

            _options = new ImageExportOptions();

            HasRequiredOption<string>("output|o=", "Output directory", o => _options.OutputDirectory = o);
            HasOption("update|u", "Enable updates after export", u => _options.IsUpdateEnabled = true);
            HasOption<int>("store|s=", "Store ID", s => _options.StoreId = s);
        }

        public override int Run(string[] remainingArguments) {
            var watch = Stopwatch.StartNew();
            var outDir = _options.OutputDirectory;

            try {
                using (var db = new NopDbContext()) {
                    var pictures = db.Pictures.AsNoTracking();
                    var imageHashes = SavePicturesToDisk(pictures, outDir);
                    ValidatePictureHashes(imageHashes);

                    if (_options.IsUpdateEnabled) RemovePicturesFromDb(db);
                }

                Console.Write("ImageExport complete!");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            watch.Stop();
            var t = TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds);
            var timeElapsed = $"{t.Hours:D2}h:{t.Minutes:D2}m:{t.Seconds:D2}s:{t.Milliseconds:D3}ms";
            Console.WriteLine();
            Console.WriteLine($"Execution time: {timeElapsed}");

            // Do something
            return 0;
        }

        private void RemovePicturesFromDb(NopDbContext db) {
            var spin = new ConsoleSpinner();
            Console.Write("Removing pictures... ");

            var pictures = db.Pictures;
            var pictureCount = pictures.Count();

            // Clear pictures in DB
            // Work with only 100 pictures at a time from the DB
            for (var i = 0; i < pictureCount; i = i + 100) {
                spin.Turn();
                var pictureBatch = pictures.OrderBy(p => p.Id).Skip(i).Take(100);

                foreach (var picture in pictureBatch) picture.PictureBinary = new byte[0];

                db.SaveChanges();
            }

            // Turn off StoreInDB setting
            Console.Write("Turning off StoreInDB setting...");
            var setting = db.Settings.SingleOrDefault(s => s.StoreId == _options.StoreId && s.Name == "Media.Images.StoreInDB");
            if (setting == null) return;

            setting.Value = false.ToString();
            db.SaveChanges();
        }

        private void ValidatePictureHashes(Dictionary<int, PictureHash> pictureHashes) {
            var spin = new ConsoleSpinner();
            Console.Write("Validating image hashes... ");

            // Verify we've written all images we said we would
            foreach (var ph in pictureHashes) {
                spin.Turn();
                var fileHash = GetHash(File.ReadAllBytes(ph.Value.Path));

                if (fileHash.Equals(pictureHashes[ph.Key].Hash)) continue;

                throw new Exception($"Hash validation failed.  Delete {ph.Value.Path} and re-run ImageExport command.");
            }
        }

        private Dictionary<int, PictureHash> SavePicturesToDisk(IQueryable<Picture> pictures, string outDir) {
            var pictureCount = pictures.Count();

            // Set up a dictionary to store image hashes for verification prior to removal from DB later
            var imageHashes = new Dictionary<int, PictureHash>(pictureCount);

            // Work with only 100 pictures at a time from the DB
            for (var i = 0; i < pictureCount; i = i + 100) {
                var pictureBatch = pictures.OrderBy(p => p.Id).Skip(i).Take(100);

                foreach (var picture in pictureBatch) {
                    // Skip if no picture
                    if (picture.PictureBinary == null || picture.PictureBinary.Equals(new byte[0])) continue;

                    var ext = GetFileExtensionFromMimeType(picture.MimeType);
                    var fileName = $"{picture.Id:0000000}_0.{ext}";
                    var filePath = Path.Combine(outDir, fileName);

                    if (!File.Exists(filePath)) {
                        Console.WriteLine($"Writing image\t{fileName} - {picture.SeoFilename}");
                        File.WriteAllBytes(filePath, picture.PictureBinary);
                    }
                    else {
                        Console.WriteLine($"Skipping image\t{fileName} - {picture.SeoFilename}");
                    }

                    // Store a hash for later
                    imageHashes.Add(picture.Id, new PictureHash {Path = filePath, Hash = GetHash(picture.PictureBinary)});
                }
            }

            return imageHashes;
        }

        private string GetHash(byte[] picturePictureBinary) {
            using (var sha1 = new SHA1CryptoServiceProvider()) {
                return Convert.ToBase64String(sha1.ComputeHash(picturePictureBinary));
            }
        }

        private string GetFileExtensionFromMimeType(string mimeType) {
            if (mimeType == null)
                return null;

            var parts = mimeType.Split('/');
            var lastPart = parts[parts.Length - 1];
            switch (lastPart) {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }
    }
}