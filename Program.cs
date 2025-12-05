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
            // Top Picks - Classic & Modern Masterpieces
            new Movie 
            { 
                Title = "The Shawshank Redemption", 
                Director = "Frank Darabont", 
                Year = 1994, 
                Genre = "Drama", 
                RuntimeMins = 142, 
                Rating = 10, 
                IsTopPick = true,
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency."
            },
            new Movie 
            { 
                Title = "The Godfather", 
                Director = "Francis Ford Coppola", 
                Year = 1972, 
                Genre = "Crime", 
                RuntimeMins = 175, 
                Rating = 10, 
                IsTopPick = true,
                Description = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son."
            },
            new Movie 
            { 
                Title = "The Dark Knight", 
                Director = "Christopher Nolan", 
                Year = 2008, 
                Genre = "Action", 
                RuntimeMins = 152, 
                Rating = 9, 
                IsTopPick = true,
                Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests."
            },
            new Movie 
            { 
                Title = "Inception", 
                Director = "Christopher Nolan", 
                Year = 2010, 
                Genre = "Sci-Fi", 
                RuntimeMins = 148, 
                Rating = 9, 
                IsTopPick = true,
                Description = "A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO."
            },
            new Movie 
            { 
                Title = "Pulp Fiction", 
                Director = "Quentin Tarantino", 
                Year = 1994, 
                Genre = "Crime", 
                RuntimeMins = 154, 
                Rating = 9, 
                IsTopPick = true,
                Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption."
            },
            new Movie 
            { 
                Title = "Forrest Gump", 
                Director = "Robert Zemeckis", 
                Year = 1994, 
                Genre = "Drama", 
                RuntimeMins = 142, 
                Rating = 9, 
                IsTopPick = true,
                Description = "The presidencies of Kennedy and Johnson, the Vietnam War, and other historical events unfold from the perspective of an Alabama man with an IQ of 75."
            },
            new Movie 
            { 
                Title = "The Matrix", 
                Director = "Lana Wachowski, Lilly Wachowski", 
                Year = 1999, 
                Genre = "Sci-Fi", 
                RuntimeMins = 136, 
                Rating = 9, 
                IsTopPick = true,
                Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers."
            },
            new Movie 
            { 
                Title = "Goodfellas", 
                Director = "Martin Scorsese", 
                Year = 1990, 
                Genre = "Crime", 
                RuntimeMins = 146, 
                Rating = 9, 
                IsTopPick = true,
                Description = "The story of Henry Hill and his life in the mob, covering his relationship with his wife Karen Hill and his mob partners."
            },
            new Movie 
            { 
                Title = "Interstellar", 
                Director = "Christopher Nolan", 
                Year = 2014, 
                Genre = "Sci-Fi", 
                RuntimeMins = 169, 
                Rating = 9, 
                IsTopPick = true,
                Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival."
            },
            new Movie 
            { 
                Title = "Spirited Away", 
                Director = "Hayao Miyazaki", 
                Year = 2001, 
                Genre = "Animation", 
                RuntimeMins = 125, 
                Rating = 10, 
                IsTopPick = true,
                Description = "During her family's move to the suburbs, a sullen 10-year-old girl wanders into a world ruled by gods, witches, and spirits."
            },
            new Movie 
            { 
                Title = "Parasite", 
                Director = "Bong Joon Ho", 
                Year = 2019, 
                Genre = "Thriller", 
                RuntimeMins = 132, 
                Rating = 9, 
                IsTopPick = true,
                Description = "Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan."
            },
            new Movie 
            { 
                Title = "The Lord of the Rings: The Return of the King", 
                Director = "Peter Jackson", 
                Year = 2003, 
                Genre = "Fantasy", 
                RuntimeMins = 201, 
                Rating = 9, 
                IsTopPick = true,
                Description = "Gandalf and Aragorn lead the World of Men against Sauron's army to draw his gaze from Frodo and Sam as they approach Mount Doom with the One Ring."
            },

            // Great Action Movies
            new Movie 
            { 
                Title = "Mad Max: Fury Road", 
                Director = "George Miller", 
                Year = 2015, 
                Genre = "Action", 
                RuntimeMins = 120, 
                Rating = 8, 
                IsTopPick = false,
                Description = "In a post-apocalyptic wasteland, a woman rebels against a tyrannical ruler in search for her homeland with the aid of a group of female prisoners."
            },
            new Movie 
            { 
                Title = "John Wick", 
                Director = "Chad Stahelski", 
                Year = 2014, 
                Genre = "Action", 
                RuntimeMins = 101, 
                Rating = 8, 
                IsTopPick = false,
                Description = "An ex-hit-man comes out of retirement to track down the gangsters that killed his dog and took everything from him."
            },
            new Movie 
            { 
                Title = "Die Hard", 
                Director = "John McTiernan", 
                Year = 1988, 
                Genre = "Action", 
                RuntimeMins = 132, 
                Rating = 8, 
                IsTopPick = false,
                Description = "An NYPD officer tries to save his wife and several others taken hostage by German terrorists during a Christmas party at the Nakatomi Plaza in Los Angeles."
            },

            // Acclaimed Dramas
            new Movie 
            { 
                Title = "Schindler's List", 
                Director = "Steven Spielberg", 
                Year = 1993, 
                Genre = "Drama", 
                RuntimeMins = 195, 
                Rating = 9, 
                IsTopPick = false,
                Description = "In German-occupied Poland during World War II, industrialist Oskar Schindler gradually becomes concerned for his Jewish workforce after witnessing their persecution."
            },
            new Movie 
            { 
                Title = "12 Years a Slave", 
                Director = "Steve McQueen", 
                Year = 2013, 
                Genre = "Drama", 
                RuntimeMins = 134, 
                Rating = 8, 
                IsTopPick = false,
                Description = "In the antebellum United States, Solomon Northup, a free black man from upstate New York, is abducted and sold into slavery."
            },
            new Movie 
            { 
                Title = "The Green Mile", 
                Director = "Frank Darabont", 
                Year = 1999, 
                Genre = "Drama", 
                RuntimeMins = 189, 
                Rating = 8, 
                IsTopPick = false,
                Description = "The lives of guards on Death Row are affected by one of their charges: a black man accused of child murder and rape, yet who has a mysterious gift."
            },

            // Horror Classics
            new Movie 
            { 
                Title = "The Shining", 
                Director = "Stanley Kubrick", 
                Year = 1980, 
                Genre = "Horror", 
                RuntimeMins = 146, 
                Rating = 8, 
                IsTopPick = false,
                Description = "A family heads to an isolated hotel for the winter where a sinister presence influences the father into violence, while his psychic son sees horrific forebodings."
            },
            new Movie 
            { 
                Title = "Get Out", 
                Director = "Jordan Peele", 
                Year = 2017, 
                Genre = "Horror", 
                RuntimeMins = 104, 
                Rating = 8, 
                IsTopPick = false,
                Description = "A young African-American visits his white girlfriend's parents for the weekend, where his simmering uneasiness about their reception of him eventually reaches a boiling point."
            },
            new Movie 
            { 
                Title = "A Quiet Place", 
                Director = "John Krasinski", 
                Year = 2018, 
                Genre = "Horror", 
                RuntimeMins = 90, 
                Rating = 8, 
                IsTopPick = false,
                Description = "In a post-apocalyptic world, a family is forced to live in silence while hiding from monsters with ultra-sensitive hearing."
            },

            // Comedies
            new Movie 
            { 
                Title = "The Grand Budapest Hotel", 
                Director = "Wes Anderson", 
                Year = 2014, 
                Genre = "Comedy", 
                RuntimeMins = 99, 
                Rating = 8, 
                IsTopPick = false,
                Description = "A writer encounters the owner of an aging high-class hotel, who tells him of his early years serving as a lobby boy in the hotel's glorious years."
            },
            new Movie 
            { 
                Title = "Superbad", 
                Director = "Greg Mottola", 
                Year = 2007, 
                Genre = "Comedy", 
                RuntimeMins = 113, 
                Rating = 7, 
                IsTopPick = false,
                Description = "Two co-dependent high school seniors are forced to deal with separation anxiety after their plan to stage a booze-soaked party goes awry."
            },
            new Movie 
            { 
                Title = "The Big Lebowski", 
                Director = "Joel Coen, Ethan Coen", 
                Year = 1998, 
                Genre = "Comedy", 
                RuntimeMins = 117, 
                Rating = 8, 
                IsTopPick = false,
                Description = "Jeff 'The Dude' Lebowski, mistaken for a millionaire of the same name, seeks restitution for his ruined rug and enlists his bowling buddies to help get it."
            },

            // Animated Films
            new Movie 
            { 
                Title = "Toy Story", 
                Director = "John Lasseter", 
                Year = 1995, 
                Genre = "Animation", 
                RuntimeMins = 81, 
                Rating = 8, 
                IsTopPick = false,
                Description = "A cowboy doll is profoundly threatened and jealous when a new spaceman figure supplants him as top toy in a boy's room."
            },
            new Movie 
            { 
                Title = "WALL-E", 
                Director = "Andrew Stanton", 
                Year = 2008, 
                Genre = "Animation", 
                RuntimeMins = 98, 
                Rating = 8, 
                IsTopPick = false,
                Description = "In the distant future, a small waste-collecting robot inadvertently embarks on a space journey that will ultimately decide the fate of mankind."
            },
            new Movie 
            { 
                Title = "Spider-Man: Into the Spider-Verse", 
                Director = "Bob Persichetti, Peter Ramsey, Rodney Rothman", 
                Year = 2018, 
                Genre = "Animation", 
                RuntimeMins = 117, 
                Rating = 8, 
                IsTopPick = false,
                Description = "Teen Miles Morales becomes Spider-Man of his reality, crossing his path with five counterparts from other dimensions to stop a threat for all realities."
            },

            // Romance & Romance-Drama
            new Movie 
            { 
                Title = "Titanic", 
                Director = "James Cameron", 
                Year = 1997, 
                Genre = "Romance", 
                RuntimeMins = 194, 
                Rating = 8, 
                IsTopPick = false,
                Description = "A seventeen-year-old aristocrat falls in love with a kind but poor artist aboard the luxurious, ill-fated R.M.S. Titanic."
            },
            new Movie 
            { 
                Title = "La La Land", 
                Director = "Damien Chazelle", 
                Year = 2016, 
                Genre = "Romance", 
                RuntimeMins = 128, 
                Rating = 8, 
                IsTopPick = false,
                Description = "While navigating their careers in Los Angeles, a pianist and an actress fall in love while attempting to reconcile their aspirations for the future."
            },
            new Movie 
            { 
                Title = "Eternal Sunshine of the Spotless Mind", 
                Director = "Michel Gondry", 
                Year = 2004, 
                Genre = "Romance", 
                RuntimeMins = 108, 
                Rating = 8, 
                IsTopPick = false,
                Description = "When their relationship turns sour, a couple undergoes a medical procedure to have each other erased from their memories."
            },

            // Thrillers
            new Movie 
            { 
                Title = "Se7en", 
                Director = "David Fincher", 
                Year = 1995, 
                Genre = "Thriller", 
                RuntimeMins = 127, 
                Rating = 8, 
                IsTopPick = false,
                Description = "Two detectives, a rookie and a veteran, hunt a serial killer who uses the seven deadly sins as his motives."
            },
            new Movie 
            { 
                Title = "Gone Girl", 
                Director = "David Fincher", 
                Year = 2014, 
                Genre = "Thriller", 
                RuntimeMins = 149, 
                Rating = 8, 
                IsTopPick = false,
                Description = "With his wife's disappearance having become the focus of an intense media circus, a man sees the spotlight turned on him when it's suspected that he may not be innocent."
            },
            new Movie 
            { 
                Title = "Shutter Island", 
                Director = "Martin Scorsese", 
                Year = 2010, 
                Genre = "Thriller", 
                RuntimeMins = 138, 
                Rating = 8, 
                IsTopPick = false,
                Description = "In 1954, a U.S. Marshal investigates the disappearance of a murderer who escaped from a hospital for the criminally insane."
            },

            // More Recent Hits
            new Movie 
            { 
                Title = "Everything Everywhere All at Once", 
                Director = "Daniel Kwan, Daniel Scheinert", 
                Year = 2022, 
                Genre = "Sci-Fi", 
                RuntimeMins = 139, 
                Rating = 8, 
                IsTopPick = false,
                Description = "An aging Chinese immigrant is swept up in an insane adventure, where she alone can save the world by exploring other universes connecting with the lives she could have led."
            },
            new Movie 
            { 
                Title = "Dune", 
                Director = "Denis Villeneuve", 
                Year = 2021, 
                Genre = "Sci-Fi", 
                RuntimeMins = 155, 
                Rating = 8, 
                IsTopPick = false,
                Description = "Feature adaptation of Frank Herbert's science fiction novel about the son of a noble family entrusted with the protection of the most valuable asset in the galaxy."
            },
            new Movie 
            { 
                Title = "The Batman", 
                Director = "Matt Reeves", 
                Year = 2022, 
                Genre = "Action", 
                RuntimeMins = 176, 
                Rating = 8, 
                IsTopPick = false,
                Description = "When the Riddler, a sadistic serial killer, begins murdering key political figures in Gotham, Batman is forced to investigate the city's hidden corruption."
            },
            new Movie 
            { 
                Title = "Top Gun: Maverick", 
                Director = "Joseph Kosinski", 
                Year = 2022, 
                Genre = "Action", 
                RuntimeMins = 130, 
                Rating = 8, 
                IsTopPick = false,
                Description = "After thirty years, Maverick is still pushing the envelope as a top naval aviator, but must confront ghosts of his past when he leads TOP GUN's elite graduates on a mission."
            }
        );
        await db.SaveChangesAsync();
    }
}
