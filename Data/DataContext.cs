using System;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
	public class DataContext : DbContext
	{
		public DataContext( DbContextOptions<DataContext> options ) : base (options)
		{
		}

        public DbSet<User>? users { get; set; }
        public DbSet<Car>? cars { get; set; }

    }
}

