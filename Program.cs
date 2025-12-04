using MoviesHub.Models;
using Microsoft.EntityFrameworkCore;
using MoviesHub.Data;
using System.Runtime.InteropServices;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Configure DbContext: use SQL Server on Windows (LocalDB) and SQLite on other platforms
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    else
    {
        // Use a local SQLite file for non-Windows environments (e.g., Linux dev container)
        var dbPath = Path.Combine(builder.Environment.ContentRootPath, "movieshub.db");
        options.UseSqlite($"Data Source={dbPath}");
    }
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed initial data on first run (attempt migrations, but continue if migrations can't be applied in this environment)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
        await SampleMovieSeeder.RunAsync(db);
    }
    catch (Exception ex)
    {
        // If migrations can't be applied (e.g., pending model changes or provider differences), fall back to EnsureCreated
        logger.LogWarning(ex, "Database Migrate failed; attempting EnsureCreated fallback.");
        try
        {
            db.Database.EnsureCreated();
            await SampleMovieSeeder.RunAsync(db);
        }
        catch (Exception inner)
        {
            logger.LogError(inner, "Database initialization failed. The app will continue, but database-backed features may be unavailable.");
        }
    }
}

app.Run();

// Seeder
static class SampleMovieSeeder
{
    public static async Task RunAsync(AppDbContext db)
    {
        if (await db.Movies.AnyAsync()) return;

        db.Movies.AddRange( 
            new MoviesHub.Models.Movie { Title = "Inception", /*...*/ Rating = 9, IsTopPick = true, /*...*/ }, 
            new MoviesHub.Models.Movie { Title = "Spirited Away", /*...*/ Rating = 10, IsTopPick = true, /*...*/ }, 
            new MoviesHub.Models.Movie { Title = "The Dark Knight", /*...*/ Rating = 9, IsTopPick = true, /*...*/ } 
        );
        await db.SaveChangesAsync();
    }
}
