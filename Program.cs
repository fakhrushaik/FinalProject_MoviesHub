using MoviesHub.Models;
using Microsoft.EntityFrameworkCore;
using MoviesHub.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

// Seed initial data on first run
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();                
    await SampleMovieSeeder.RunAsync(db);          
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
