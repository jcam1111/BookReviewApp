//using System.ComponentModel.DataAnnotations;

//namespace BookReviewApp.Api.Models.App
//{
//    public class Category
//    {
//    }
//}


using BookReviewApp.Api.Models.App;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookReviewApp.API.Models.App
{
    public class Category
    {
        [Key] // Identifica la clave primaria
        public int Id { get; set; }

        [Required]
        [MaxLength(100)] // Coincide con NVARCHAR(100)
        public string Name { get; set; } = string.Empty;

        // Propiedad de navegación: Una categoría puede tener muchos libros.
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}