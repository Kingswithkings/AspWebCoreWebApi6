using AspWebCoreWebApi6.Data;
using AspWebCoreWebApi6.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspWebCoreWebApi6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _dbContext;

        public MoviesController(MovieContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            var movies = await _dbContext.Movies.ToListAsync();

            if (movies == null || movies.Count == 0)
            {
                // If no movies are found, return a 404 Not Found response.
                return NotFound();
            }

            // If movies are found, return them as the result.
            return movies;
        }

        // GET: api/Movies/5
        // Retrieve a specific movie by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                // If the specified movie ID is not found, return a 404 Not Found response.
                return NotFound();
            }

            // If the movie is found, return it as the result.
            return movie;
        }

        // POST: api/Movies
        // Create a new movie.
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            // Return the newly created movie with a 201 Created status and a link to its resource.
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT: api/Movies/5
        // Update a movie by its ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                // If the provided ID does not match the movie's ID, return a Bad Request response.
                return BadRequest();
            }

            _dbContext.Entry(movie).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the update fails due to concurrency, check if the movie still exists.
                if (!MovieExists(id))
                {
                    // If the movie doesn't exist, return a 404 Not Found response.
                    return NotFound();
                }
                else
                {
                    // If the error is due to reasons other than non-existence, rethrow the exception.
                    throw;
                }
            }

            // If the update is successful, return a No Content response.
            return NoContent();
        }

        // Helper method to check if a movie with a specific ID exists.
        private bool MovieExists(long id)
        {
            return _dbContext.Movies.Any(e => e.Id == id);
        }
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _dbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                // If the specified movie ID is not found, return a 404 Not Found response.
                return NotFound();
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();

            // If the deletion is successful, return a No Content response.
            return NoContent();
        }
    }
}
