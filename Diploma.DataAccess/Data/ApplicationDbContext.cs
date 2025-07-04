﻿using Diploma.Models;
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
		public DbSet<Company> Companies { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<CourseEnrollmentRequest> CourseEnrollmentRequests { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<Weekday> Weekdays { get; set; }
		public DbSet<Group> Groups { get; set; }
		public DbSet<GroupStudent> GroupStudents { get; set; }
		public DbSet<GroupWeekday> GroupWeekdays { get; set; }
		public DbSet<SubjectTeacher> SubjectTeachers { get; set; }
		public DbSet<FileResource> FileResources { get; set; }
		public DbSet<ChatMessage> ChatMessages { get; set; }
		public DbSet<Classroom> Classrooms { get; set; }
		public DbSet<Lesson> Lessons { get; set; }
		public DbSet<Attendance> Attendances { get; set; }
		public DbSet<Subscription> Subscriptions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Group>()
				.HasOne(g => g.SubjectTeacher)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Attendance>()
				.HasOne(g => g.Lesson)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Group>()
				.HasOne(g => g.Classroom)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict);


			modelBuilder.Entity<Category>().HasData(
					new Category { Id = 1, Name = "Computer courses", DisplayOrder = 1 },
					new Category { Id = 2, Name = "Foreighn Languages", DisplayOrder = 2 },
					new Category { Id = 3, Name = "Musical courses", DisplayOrder = 3 }
					);

			modelBuilder.Entity<Classroom>().HasData(
					new Classroom { Id = 1, RoomName = "English room" },
					new Classroom { Id = 2, RoomName = "Music room" },
					new Classroom { Id = 3, RoomName = "German room" }
					);

			modelBuilder.Entity<Weekday>().HasData(
				new Weekday { Id = 1, Day = "Monday" },
				new Weekday { Id = 2, Day = "Tuesday" },
				new Weekday { Id = 3, Day = "Wednesday" },
				new Weekday { Id = 4, Day = "Thursday" },
				new Weekday { Id = 5, Day = "Friday" },
				new Weekday { Id = 6, Day = "Saturday" }
				);

			modelBuilder.Entity<Company>().HasData(
				new Company
				{
					Id = 1,
					Name = "Tech Solution",
					StreetAddress = "123 Tech St",
					City = "Tech City",
					PostalCode = "12121",
					State = "IL",
					PhoneNumber = "6669990000"
				},
				new Company
				{
					Id = 2,
					Name = "Vivid Books",
					StreetAddress = "999 Vid St",
					City = "Vid City",
					PostalCode = "66666",
					State = "IL",
					PhoneNumber = "7779990000"
				},
				new Company
				{
					Id = 3,
					Name = "Readers Club",
					StreetAddress = "999 Main St",
					City = "Lala land",
					PostalCode = "99999",
					State = "NY",
					PhoneNumber = "1113335555"
				});

			modelBuilder.Entity<Subject>().HasData(
				new Subject
				{
					Id = 1,
					Title = "Fortune of Time",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SWD9999001",
					Price = 90,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 2,
					Title = "Dark Skies",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "CAW777777701",
					Price = 30,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 3,
					Title = "Vanish in the Sunset",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "RITO5555501",
					Price = 50,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 4,
					Title = "Cotton Candy",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "WS3333333301",
					Price = 65,
					CategoryId = 1,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 5,
					Title = "Rock in the Ocean",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "SOTJ1111111101",
					Price = 27,
					CategoryId = 3,
					ImageUrl = ""
				},
				new Subject
				{
					Id = 6,
					Title = "Leaves and Wonders",
					Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
					ISBN = "FOT000000001",
					Price = 23,
					CategoryId = 1,
					ImageUrl = ""
				}
				);
		}
	}
}