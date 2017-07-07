using Microsoft.EntityFrameworkCore;
using Sumo.Nop.MediaToolsCore.Commands;
using Sumo.Nop.MediaToolsCore.Models;

namespace Sumo.Nop.MediaToolsCore {
    public class NopDbContext : DbContext {
        public NopDbContext(DbContextOptions options) : base(options) { }

        //public NopDbContext()
        //    : base("name=Nop")
        //{
        //    Database.SetInitializer<NopDbContext>(null);
        //    Database.CommandTimeout = 18000; // 5 hours
        //}

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }
    }
}