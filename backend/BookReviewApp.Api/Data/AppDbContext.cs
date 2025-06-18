//using BookReviewApp.Api.Models.App.TuProyecto.API.Models.App;
//using BookReviewApp.API.Models.App;
//using System.Collections.Generic;
//using System.Reflection.Emit;
//using TuProyecto.API.Models.App;
//using TuProyecto.API.Models.Identity;

//namespace BookReviewApp.Api.Data
//{
//    public class AppDbContext
//    {
//    }
//}

using BookReviewApp.Api.Models.App;
using BookReviewApp.API.Models.App;
using BookReviewApp.API.Models.Identity;
//using BookReviewApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApp.API.Data
{
    // Heredamos de IdentityDbContext y le pasamos nuestro usuario personalizado AppUser.
    // Esto configura automáticamente todas las tablas de Identity por nosotros.
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para tus modelos personalizados
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        // Add this line
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Es CRUCIAL llamar al método base para que Identity se configure correctamente.
            base.OnModelCreating(builder);

            // Map Identity tables to custom names
            //builder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");          
            
            builder.Entity<IdentityRole>().ToTable("AspNetRoles");
            builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

            // Configuraciones adicionales usando Fluent API (más potente que las anotaciones)

            // Configuración de la relación para Review
            builder.Entity<Review>(entity =>
            {
                // Un Libro tiene muchas Reseñas, y cada Reseña pertenece a un Libro.
                // Si se borra un Libro, se borran sus reseñas (OnDelete.Cascade es el default aquí).
                entity.HasOne(r => r.Book)
                      .WithMany(b => b.Reviews)
                      .HasForeignKey(r => r.BookId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Un Usuario tiene muchas Reseñas, y cada Reseña pertenece a un Usuario.
                entity.HasOne(r => r.User)
                      .WithMany(u => u.Reviews)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Configurar el valor por defecto para CreatedAt
                entity.Property(r => r.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configuración para Book
            builder.Entity<Book>(entity =>
            {
                // Un Libro pertenece a una Categoría. Una Categoría tiene muchos libros.
                entity.HasOne(b => b.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(b => b.CategoryId);

                // Crear índices como en tu script SQL para mejorar el rendimiento
                entity.HasIndex(b => b.Title);
                entity.HasIndex(b => b.Author);
            });

            // Seed Data (Datos de ejemplo)
            // Se puede hacer aquí o en un archivo separado para más orden.
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Ficción" },
                new Category { Id = 2, Name = "Ciencia Ficción" },
                new Category { Id = 3, Name = "Misterio" },
                new Category { Id = 4, Name = "Fantasía" },
                new Category { Id = 5, Name = "No Ficción" },
                new Category { Id = 6, Name = "Biografía" }
            );

            builder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Cien años de soledad",
                    Author = "Gabriel García Márquez",
                    Summary = "La historia de la familia Buendía a lo largo de siete generaciones en el pueblo ficticio de Macondo.",
                    CategoryId = 1,
                    CoverImageUrl = "https://images.penguinrandomhouse.com/cover/9780307474728"
                },
                new Book
                {
                    Id = 2,
                    Title = "Dune",
                    Author = "Frank Herbert",
                    Summary = "La historia de Paul Atreides, un joven brillante destinado a un gran propósito, que debe viajar al planeta más peligroso del universo para asegurar el futuro de su familia y su pueblo.",
                    CategoryId = 2,
                    CoverImageUrl = "https://images.penguinrandomhouse.com/cover/9780441013593"
                },
                new Book
                {
                    Id = 3,
                    Title = "El código Da Vinci",
                    Author = "Dan Brown",
                    Summary = "El experto en simbología Robert Langdon se ve envuelto en una búsqueda del tesoro a través de Europa para descubrir un antiguo secreto protegido por una sociedad secreta.",
                    CategoryId = 3,
                    CoverImageUrl = "https://images.penguinrandomhouse.com/cover/9780307474278"
                },
                new Book
                {
                    Id = 4,
                    Title = "El Señor de los Anillos",
                    Author = "J.R.R. Tolkien",
                    Summary = "Un hobbit llamado Frodo Bolsón se embarca en una peligrosa misión para destruir un poderoso anillo y salvar a la Tierra Media de la oscuridad.",
                    CategoryId = 4,
                    CoverImageUrl = "https://images.penguinrandomhouse.com/cover/9780618640157"
                }
            );
        }

    }
}