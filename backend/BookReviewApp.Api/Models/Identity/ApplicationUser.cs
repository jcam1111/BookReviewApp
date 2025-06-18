//namespace BookReviewApp.Api.Models.Identity;
//{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using System.Collections.Generic;
using BookReviewApp.API.Models.App; // Asegúrate de que el namespace sea correcto

namespace BookReviewApp.API.Models.Identity
    {
        // Heredamos de IdentityUser para obtener todos los campos base
        // (UserName, Email, PasswordHash, etc.)
        public class ApplicationUser : IdentityUser
        {
            // 1. Campo personalizado que añadiste en tu script SQL
            public string? ProfilePictureUrl { get; set; }

            // 2. Propiedad de navegación: Un usuario puede escribir muchas reseñas.
            // Esto le dice a EF Core que hay una relación uno-a-muchos.
            public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        }
    }
//}
