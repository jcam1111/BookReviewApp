//namespace BookReviewApp.Api.Models.App
//{
//    public class Book
//    {
//    }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookReviewApp.API.Models.App;

namespace BookReviewApp.Api.Models.App
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Summary { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        // --- Relaciones (Foreign Keys) ---

        // 1. Clave foránea hacia Category
        [Required]
        public int CategoryId { get; set; }

        // Propiedad de navegación hacia la categoría a la que pertenece el libro
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // 2. Propiedad de navegación: Un libro puede tener muchas reseñas.
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
