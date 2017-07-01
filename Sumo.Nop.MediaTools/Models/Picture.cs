using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global

namespace Sumo.Nop.MediaTools.Models
{
    [Table("Picture")]
    public partial class Picture
    {
        [Key]
        public int Id { get; set; }

        public byte[] PictureBinary { get; set; }

        [Required]
        [StringLength(40)]
        public string MimeType { get; set; }

        public bool IsNew { get; set; }

        [StringLength(300)]
        public string SeoFilename { get; set; }

        public string TitleAttribute { get; set; }

        public string AltAttribute { get; set; }
    }
}
