using cloudcomputing.Efs.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudcomputing.Efs.Context
{
    public partial class DatabaseContext: DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {   
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Container> Containers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Container>().ToTable("Containers");
        }
    }
}
