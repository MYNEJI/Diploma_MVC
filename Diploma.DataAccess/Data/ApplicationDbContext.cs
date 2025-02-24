using Diploma.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diploma.DataAccess.Data
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Computer courses", DisplayOrder = 1 },
				new Category { Id = 2, Name = "Foreighn Languages", DisplayOrder = 2 },
				new Category { Id = 3, Name = "Musical courses", DisplayOrder = 3 }
				);
			modelBuilder.Entity<Subject>().HasData(
				new Subject
				{
					Id = 1,
					Title = "Fortune of Time",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SWD9999001",
					ListPrice = 99,
					Price = 90,
					Price50 = 85,
					Price100 = 80,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 2,
					Title = "Dark Skies",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "CAW777777701",
					ListPrice = 40,
					Price = 30,
					Price50 = 25,
					Price100 = 20,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 3,
					Title = "Vanish in the Sunset",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "RITO5555501",
					ListPrice = 55,
					Price = 50,
					Price50 = 40,
					Price100 = 35,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 4,
					Title = "Cotton Candy",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "WS3333333301",
					ListPrice = 70,
					Price = 65,
					Price50 = 60,
					Price100 = 55,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 5,
					Title = "Rock in the Ocean",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SOTJ1111111101",
					ListPrice = 30,
					Price = 27,
					Price50 = 25,
					Price100 = 20,
					CategoryId = 3,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 6,
					Title = "Leaves and Wonders",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "FOT000000001",
					ListPrice = 25,
					Price = 23,
					Price50 = 22,
					Price100 = 20,
					CategoryId = 1,
					ImageUrl = ""
				}
				);
		}
	}
}