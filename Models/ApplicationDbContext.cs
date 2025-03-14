using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Task_Manager.Models
{
	public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
	{

		// The constructor to configure the connection string
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		// DbSet properties for each model/entity
		public DbSet<Task> Tasks { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

            // ✅ Explicitly defining the relationship
            builder.Entity<Task>()
                .HasOne(t => t.User)  // One User
                .WithMany(u => u.Tasks)  // Many Tasks
                .HasForeignKey(t => t.UserId)  // Foreign Key
                .OnDelete(DeleteBehavior.Cascade); // If a user is deleted, delete their tasks

            // Customizing the ApplicationUser class
            builder.Entity<ApplicationUser>(entity =>
			{
				entity.Property(e => e.FirstName)
					.HasMaxLength(100)  // Setting max length for FirstName
					.IsRequired();      // Setting FirstName as required

				entity.Property(e => e.LastName)
					.HasMaxLength(100)  // Setting max length for LastName
					.IsRequired();      // Setting LastName as required
			});
		}
	}
}
