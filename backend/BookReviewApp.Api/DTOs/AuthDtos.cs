using System.ComponentModel.DataAnnotations;

namespace BookReviewApp.DTOs;

// Para el registro de un nuevo usuario
public record RegisterDto(
    [Required] string Username,
    [Required][EmailAddress] string Email,
    [Required] string Password
);

// Para el inicio de sesión
public record LoginDto(
    [Required] string Username,
    [Required] string Password
);

// Respuesta enviada tras un login/registro exitoso
public record AuthResponseDto(
    string UserId,
    string Username,
    string Email,
    string Token
);

//namespace BookReviewApp.Api.DTOs
//{
//    public class AuthDtos
//    {
//    }
//}
