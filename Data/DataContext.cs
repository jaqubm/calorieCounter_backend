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
            .HasKey(u => u.Id); // Set Id as the primary key

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email) // Ensure Email is unique
            .IsUnique();

        // Product entity
        modelBuilder.Entity<Product>()
            .HasKey(p => p.Id); // Set Id as the primary key

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.OwnerId) // Updated to OwnerId
            .OnDelete(DeleteBehavior.Cascade);

        // Recipe entity
        modelBuilder.Entity<Recipe>()
            .HasKey(r => r.Id); // Set Id as the primary key

        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.Owner)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.OwnerId) // Updated to OwnerId
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
            .WithMany(p => p.RecipeProducts)
            .HasForeignKey(rp => rp.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        // UserEntry entity
        modelBuilder.Entity<UserEntry>()
            .HasKey(ue => ue.Id); // Set Id as the primary key

        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserEntries)
            .HasForeignKey(ue => ue.UserId) // Updated to UserId
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.Product)
            .WithMany(p => p.UserEntries)
            .HasForeignKey(ue => ue.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<UserEntry>()
            .HasOne(ue => ue.Recipe)
            .WithMany(r => r.UserEntries)
            .HasForeignKey(ue => ue.RecipeId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}