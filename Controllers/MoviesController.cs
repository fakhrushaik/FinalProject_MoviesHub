using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesHub.Data;
using MoviesHub.Models;

public class MoviesController : Controller
{
    private readonly AppDbContext _db;
    public MoviesController(AppDbContext db) => _db = db;

    // q: search, genre: exact, year: exact, ratingMin: 1-10,
    // sort: title|year|rating  dir: asc|desc
    public async Task<IActionResult> Index(
        string? q, string? genre, int? year, int? ratingMin,
        string sort = "rating", string dir = "desc")
    {
        // Dropdown data
        ViewBag.Genres = await _db.Movies
            .Where(m => m.Genre != null && m.Genre != "")
            .Select(m => m.Genre!).Distinct().OrderBy(g => g).ToListAsync();

        ViewBag.Years = await _db.Movies
            .Select(m => m.Year).Distinct().OrderByDescending(y => y).ToListAsync();

        var query = _db.Movies.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            var q2 = q.Trim();
            query = query.Where(m => m.Title.Contains(q2) || (m.Director ?? "").Contains(q2));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            var g = genre.Trim().ToLower();
            query = query.Where(m => (m.Genre ?? "").ToLower() == g);
        }

        if (year.HasValue)
            query = query.Where(m => m.Year == year.Value);

        if (ratingMin.HasValue)
            query = query.Where(m => m.Rating >= ratingMin.Value);

        // Sorting
        bool desc = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);
        query = (sort?.ToLower()) switch
        {
            "title" => desc ? query.OrderByDescending(m => m.Title) : query.OrderBy(m => m.Title),
            "year" => desc ? query.OrderByDescending(m => m.Year) : query.OrderBy(m => m.Year),
            _ => desc ? query.OrderByDescending(m => m.Rating) : query.OrderBy(m => m.Rating) // default rating
        };

        var list = await query.ToListAsync();

        // keep selections for the view
        ViewBag.CurrentSearch = q;
        ViewBag.CurrentGenre = genre;
        ViewBag.CurrentYear = year;
        ViewBag.CurrentRatingMin = ratingMin;
        ViewBag.CurrentSort = sort;
        ViewBag.CurrentDir = desc ? "desc" : "asc";

        return View(list);
    }

    // GET: Movies/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    // GET: Movies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Movies/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Director,Year,Genre,RuntimeMins,Rating,Description,PosterUrl,IsTopPick")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            _db.Add(movie);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Movies/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _db.Movies.FindAsync(id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    // POST: Movies/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Director,Year,Genre,RuntimeMins,Rating,Description,PosterUrl,IsTopPick")] Movie movie)
    {
        if (id != movie.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(movie);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: Movies/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
            return NotFound();

        return View(movie);
    }

    // POST: Movies/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _db.Movies.FindAsync(id);
        if (movie != null)
        {
            _db.Movies.Remove(movie);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int id)
    {
        return _db.Movies.Any(e => e.Id == id);
    }
}
