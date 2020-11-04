using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _context.Dispose();
        }

        // GET: Movies
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Title = "New Movie",
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                Title = "Edit Movie",
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    var viewModel = new MovieFormViewModel(movie)
                    {
                        Genres = _context.Genres.ToList()
                    };

                    return View("MovieForm", viewModel);
                }

                if (movie.Id == 0)
                {
                    _context.Movies.Add(movie);
                }
                else
                {
                    var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);

                    movieInDb.Name = movie.Name;
                    movieInDb.Genre = movie.Genre;
                    movieInDb.ReleaseDate = movie.ReleaseDate;
                    movieInDb.DateAdded = movie.DateAdded;                    
                    movieInDb.NumberInStock = movie.NumberInStock;
                }

                _context.SaveChanges();

            }
            catch (DbEntityValidationException ex)
            {
                Console.WriteLine(ex);
            }

            return RedirectToAction("Index", "Movies");
        }

        [Route("Movies/Details/{id:int}")]
        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" },
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);
        }
    }
}