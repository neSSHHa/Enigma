using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataManipulation
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasMany(x => x.Lectures).WithMany(x => x.Students);
            modelBuilder.Entity<Lecture>()
      .HasOne(l => l.Lecturer)
      .WithMany() // Assuming a Lecturer can have many Lectures
      .HasForeignKey(l => l.LecturerId)
              .OnDelete(DeleteBehavior.NoAction); // 👈 Add this line
            
            base.OnModelCreating(modelBuilder);
        }

        // tables

        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<proofOfPayment> Pop { get; set; }
    }
}
