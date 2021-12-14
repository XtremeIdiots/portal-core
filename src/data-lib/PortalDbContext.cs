using Microsoft.EntityFrameworkCore;
using XtremeIdiots.Portal.DataLib.Models;

namespace XtremeIdiots.Portal.DataLib
{
    public partial class PortalDbContext : DbContext
    {
        public PortalDbContext()
        {
        }

        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FirstSeen).HasColumnType("datetime");

                entity.Property(e => e.GameType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Guid).HasMaxLength(50);

                entity.Property(e => e.IpAddress).HasMaxLength(50);

                entity.Property(e => e.LastSeen).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}