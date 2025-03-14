using Microsoft.AspNetCore.Identity;


namespace Task_Manager.Models
{
	public class ApplicationUser:IdentityUser
	{
		
		public string FirstName { get; set; }


		public string LastName { get; set; }


        // ✅ Navigation property for related tasks
        public ICollection<Task> Tasks { get; set; }
    }
}
