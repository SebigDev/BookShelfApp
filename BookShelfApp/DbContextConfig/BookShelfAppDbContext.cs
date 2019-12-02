using BookShelfApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfApp.DbContextConfig
{
    public class BookShelfAppDbContext : DbContext
    {
        public BookShelfAppDbContext(DbContextOptions options)
            : base(options) { }


        public DbSet<User> User { get; set; }
        
    }
}
