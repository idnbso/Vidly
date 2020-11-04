using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MoviesViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
    }
}