using AspWebCoreWebApi6.Model;
using Microsoft.EntityFrameworkCore;

namespace AspWebCoreWebApi6.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) 
            : base(options)
        {
        }
        public DbSet<Movie> Movies { get; set; } = null!;
    }
}
