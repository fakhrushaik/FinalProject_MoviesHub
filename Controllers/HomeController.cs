using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesHub.Data;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public IActionResult Index() => View();
    public IActionResult AboutMovies() => View();
    public IActionResult Genres() => View();
    public IActionResult AboutMe() => View();

    // Explicit About page for Anthony Than
    public IActionResult AnthonyThan() => View();

    public async Task<IActionResult> TopPicks()
    {
        var picks = await _db.Movies
            .AsNoTracking()
            .Where(m => m.IsTopPick) 
            .OrderByDescending(m => m.Rating) 
            .ThenBy(m => m.Title)
            .Take(12)
            .ToListAsync();

        return View(picks);
    }
}
