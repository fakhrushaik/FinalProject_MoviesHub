using Microsoft.EntityFrameworkCore;
using MoviesHub.Models;
using System.Collections.Generic;

namespace MoviesHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies => Set<Movie>();
    }
}
