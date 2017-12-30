using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RecipeApi.Models
{
    public partial class recipeContext : DbContext
    {
        public virtual DbSet<Ingredient> Ingredient { get; set; }
        public virtual DbSet<Recipe> Recipe { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=MHIGGINS-LTP0\RECIPE;Database=recipe;Trusted_Connection=True");
                //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=recipe_localdb;Trusted_Connection=True;MultipleActiveResultSets=true");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(60);

                entity.Property(e => e.RecipeId).HasColumnName("RecipeID");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Ingredient)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK__Ingredien__Recip__5AEE82B9");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.NumberOfServings).HasMaxLength(50);
            });
        }
    }
}
