using Microsoft.EntityFrameworkCore;

namespace Task_Manager.Models
{
	public class ApplicationDbContext:DbContext
	{

		// The constructor to configure the connection string
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		// DbSet properties for each model/entity
		public DbSet<Task> Tasks { get; set; }
	}
}
