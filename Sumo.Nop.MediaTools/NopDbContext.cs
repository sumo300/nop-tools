using System.Data.Entity;
using Sumo.Nop.MediaTools.Models;

namespace Sumo.Nop.MediaTools
{
    public class NopDbContext : DbContext
    {
        public NopDbContext()
            : base("name=Nop")
        {
            Database.SetInitializer<NopDbContext>(null);
            Database.CommandTimeout = 18000; // 5 hours
        }

        public virtual DbSet<Picture> Pictures { get; set; }

        public virtual DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}