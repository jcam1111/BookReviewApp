using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using BookReviewApp.Data;
using BookReviewApp.API.Models.Identity;
using BookReviewApp.DTOs;
using BookReviewApp.API.Models.App;
using BookReviewApp.API.Data;

[Route("api/[controller]")]
[ApiController]
[Authorize] // ¡Todo este controlador requiere autenticación!
public class ReviewsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReviewsController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/reviews
    [HttpPost]
    public async Task<IActionResult> PostReview([FromBody] CreateReviewDto createReviewDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var bookExists = await _context.Books.AnyAsync(b => b.Id == createReviewDto.BookId);
        if (!bookExists)
        {
            return BadRequest(new { Message = "Book not found." });
        }

        var review = new Review
        {
            BookId = createReviewDto.BookId,
            Rating = createReviewDto.Rating,
            Comment = createReviewDto.Comment,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMyReview), new { id = review.Id }, review);
    }

    // --- EL MÉTODO QUE FALTA ---
    // GET: api/reviews/{id}
    // Este método permite obtener una reseña específica por su ID.
    // Es el que 'CreatedAtAction' necesita para construir la URL del nuevo recurso.
    [HttpGet("{id}", Name = "GetMyReview")] // Se le asigna un nombre a la ruta para que sea fácil de referenciar.
    public async Task<ActionResult<Review>> GetMyReview(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var review = await _context.Reviews
                                   .Where(r => r.Id == id && r.UserId == userId) // Asegurarse de que el usuario solo pueda ver sus propias reseñas
                                   .FirstOrDefaultAsync();

        if (review == null)
        {
            return NotFound(); // No se encontró la reseña o no pertenece al usuario
        }

        return Ok(review);
    }

    // PUT: api/reviews/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        // ¡Verificación de seguridad clave! Solo el autor puede editar.
        if (review.UserId != userId)
        {
            return Forbid();
        }

        review.Rating = updateReviewDto.Rating;
        review.Comment = updateReviewDto.Comment;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        // ¡Verificación de seguridad clave! Solo el autor puede borrar.
        if (review.UserId != userId)
        {
            return Forbid();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/reviews/myreviews
    [HttpGet("myreviews")]
    public async Task<ActionResult<IEnumerable<Review>>> GetMyReviews()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var reviews = await _context.Reviews
            .Where(r => r.UserId == userId)
            .Include(r => r.Book) // Incluir info del libro para dar contexto
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new {
                r.Id,
                r.Rating,
                r.Comment,
                r.CreatedAt,
                Book = new { r.Book.Id, r.Book.Title }
            })
            .ToListAsync();

        return Ok(reviews);
    }
}

//using Microsoft.AspNetCore.Mvc;

//namespace BookReviewApp.Api.Controllers
//{
//    public class ReviewsController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
