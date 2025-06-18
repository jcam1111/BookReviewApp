using System.ComponentModel.DataAnnotations;

namespace BookReviewApp.DTOs;

// Para mostrar el perfil del usuario
public record ProfileDto(
    string Username,
    string Email,
    string? ProfilePictureUrl
);

// Para actualizar el perfil del usuario
public record UpdateProfileDto(
    [Required][EmailAddress] string Email,
    string? ProfilePictureUrl
);
//namespace BookReviewApp.Api.DTOs
//{
//    public class ProfileDtos
//    {
//    }
//}
