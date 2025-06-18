//using BookReviewApp.Api.Models.App.TuProyecto.API.Models.App;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using TuProyecto.API.Models.Identity;

//namespace BookReviewApp.Api.Models.App
//{
//    public class Review
//    {
//    }
//}


using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookReviewApp.Api.Models.App;
//using BookReviewApp.API.Models.App.Identity;
using BookReviewApp.API.Models.Identity; // Para la relación con AppUser

namespace BookReviewApp.API.Models.App
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)] // Valida que el rating esté entre 1 y 5, como el CHECK de SQL
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        // El valor por defecto (GETDATE()) se configurará en el DbContext
        public DateTime CreatedAt { get; set; }

        // --- Relaciones (Foreign Keys) ---

        // 1. Clave foránea hacia Book
        [Required]
        public int BookId { get; set; }

        // Propiedad de navegación hacia el libro reseñado
        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        // 2. Clave foránea hacia AppUser (AspNetUsers)
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Propiedad de navegación hacia el usuario que escribió la reseña
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
}