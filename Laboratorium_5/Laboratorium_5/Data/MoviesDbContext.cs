using Laboratorium_5.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Laboratorium_5.Data
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public MoviesDbContext(DbContextOptions options) :
        base(options)
        {
        }
    }

}
