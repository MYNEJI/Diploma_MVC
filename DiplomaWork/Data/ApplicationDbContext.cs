using DiplomaWork.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWork.Data
{
	public class ApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<Category>().HasData(
		//		new Category { Id = 1, Name = "Foreighn Languages", DisplayOrder = 1 },
		//		new Category { Id = 2, Name = "Musical courses", DisplayOrder = 2 },
		//		new Category { Id = 3, Name = "Computer courses", DisplayOrder = 3 }
		//		);
		//}
	}
}
