using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using BookReviewApp.Data;
using BookReviewApp.API.Models.Identity;
using BookReviewApp.API.Data;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/books
    [HttpGet]
    public async Task<IActionResult> GetBooks(
        [FromQuery] string? title,
        [FromQuery] string? author,
        [FromQuery] int? categoryId)
    {
        var query = _context.Books.Include(b => b.Category).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(b => b.Title.Contains(title));
        }
        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(b => b.Author.Contains(author));
        }
        if (categoryId.HasValue)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }

        var books = await query.Select(b => new
        {
            b.Id,
            b.Title,
            b.Author,
            Category = b.Category.Name,
            b.CoverImageUrl
        }).ToListAsync();

        return Ok(books);
    }

    // GET: api/books/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
            .Select(b => new
            {
                b.Id,
                b.Title,
                b.Author,
                b.Summary,
                Category = b.Category.Name,
                b.CoverImageUrl,
                Reviews = b.Reviews.Select(r => new
                {
                    r.Id,
                    r.Rating,
                    r.Comment,
                    r.CreatedAt,
                    User = new { r.User.UserName, r.User.ProfilePictureUrl }
                }).OrderByDescending(r => r.CreatedAt)
                 .ToList() // ✅ this makes it work
            })
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }
}

//using Microsoft.AspNetCore.Mvc;

//namespace BookReviewApp.Api.Controllers
//{
//    public class BooksController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
