using calorieCounter_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace calorieCounter_backend.Data;

public class DataContext(IConfiguration config) : DbContext
{
    public virtual DbSet<User> User { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<Recipe> Recipe { get; set; }
    public virtual DbSet<RecipeProduct> RecipeProduct { get; set; }
    public virtual DbSet<UserEntry> UserEntry { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        optionsBuilder.UseSqlServer(config.GetConnectionString("AzureSQL"),
            o =>
            {
                o.EnableRetryOnFailure();
                o.CommandTimeout(180000);
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("calorieCounter");

        // User entity
        modelBuilder.Entity<User>()
            .HasKey(u => u.Email);

        // Product entity
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Owner)
            .WithMany()
            .HasForeignKey(p => p.OwnerEmail)
            .OnDelete(DeleteBehavior.Cascade);

        // Recipe entity
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Owner)
            .WithMany()
            .HasForeignKey(r => r.OwnerEmail)
            .OnDelete(DeleteBehavior.Cascade);

        // RecipeProduct entity (many-to-many relationship between Recipe and Product)
        modelBuilder.Entity<RecipeProduct>()
            .HasKey(rp => new { rp.RecipeId, rp.ProductId });

        modelBuilder.Entity<RecipeProduct>()
            .HasOne(rp => rp.Recipe)
            .WithMany(r => r.RecipeProducts)
            .HasForeignKey(rp => rp.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecipeProduct>()
            .HasOne(rp => rp.Product)
            .WithMany()
            .HasForeignKey(rp => rp.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        // UserEntry entity
        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.User)
            .WithMany()
            .HasForeignKey(ue => ue.UserEmail)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.Product)
            .WithMany()
            .HasForeignKey(ue => ue.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.Recipe)
            .WithMany()
            .HasForeignKey(ue => ue.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}