using Microsoft.EntityFrameworkCore;
using Numbridle.Models;
using System.Reflection.Emit;

namespace Numbridle.Data;

public class GameDbContext(
    DbContextOptions<GameDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<NumberMeaning> NumberMeanings => Set<NumberMeaning>();
    public DbSet<NumberDiscovery> NumberDiscoveries => Set<NumberDiscovery>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NumberMeaning>()
            .HasIndex(x => x.Combination)
            .IsUnique();

        modelBuilder.Entity<NumberMeaning>()
            .Property(x => x.Points)
            .HasDefaultValue(0L);

        modelBuilder.Entity<Player>()
            .Property(x => x.TotalScore)
            .HasDefaultValue(0L);

        modelBuilder.Entity<NumberDiscovery>()
            .HasIndex(x => new { x.PlayerId, x.NumberMeaningId })
            .IsUnique();

        modelBuilder.Entity<NumberDiscovery>()
            .HasOne(x => x.Player)
            .WithMany(x => x.Discoveries)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NumberDiscovery>()
            .HasOne(x => x.NumberMeaning)
            .WithMany(x => x.Discoveries)
            .HasForeignKey(x => x.NumberMeaningId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PlayerTraitDiscovery>()
    .HasIndex(x => new { x.PlayerId, x.TraitKey })
    .IsUnique();

        modelBuilder.Entity<PlayerTraitDiscovery>()
            .Property(x => x.PointsAwarded)
            .HasDefaultValue(0L);

        modelBuilder.Entity<PlayerTraitDiscovery>()
            .HasOne(x => x.Player)
            .WithMany()
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}