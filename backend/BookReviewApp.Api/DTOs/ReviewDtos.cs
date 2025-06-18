using System.ComponentModel.DataAnnotations;

namespace BookReviewApp.DTOs;

// Para crear una nueva reseña
public record CreateReviewDto(
    [Required] int BookId,
    [Required][Range(1, 5)] int Rating,
    [Required] string Comment
);

// Para actualizar una reseña existente
public record UpdateReviewDto(
    [Required][Range(1, 5)] int Rating,
    [Required] string Comment
);

// Para mostrar información de una reseña
public record ReviewDto(
    int Id,
    int Rating,
    string Comment,
    DateTime CreatedAt,
    string Username,
    string? UserProfilePictureUrl
);
//namespace BookReviewApp.Api.DTOs
//{
//    public class ReviewDtos
//    {
//    }
//}
