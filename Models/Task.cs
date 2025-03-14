using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task_Manager.Models
{
	public class Task
	{
		[Key]  // Marks as Primary Key
		public int Id { get; set; }

		[Required(ErrorMessage = "Task name is required")]
		[StringLength(100, ErrorMessage = "Task name cannot exceed 100 characters")]
		public string Name { get; set; }

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Status is required")]
		[EnumDataType(typeof(TaskStatus))]
		public TaskStatus Status { get; set; } = TaskStatus.Pending; // Default: Pending

		[Required(ErrorMessage = "Priority is required")]
		[EnumDataType(typeof(TaskPriority))]
		public TaskPriority Priority { get; set; } = TaskPriority.Medium; // Default: Medium

		[Required]
		public DateTime CreatedAt { get; set; } 

        [Required]
        [DueDateGreaterThanCreatedAt(ErrorMessage = "Due date must be greater than created date.")]
        public DateTime DueDate { get; set; } 

        // ✅ Foreign Key to ApplicationUser
        [Required]
        public string UserId { get; set; } // Stores the User ID from IdentityUser

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser? User { get; set; }  // ✅ Navigation Property


    }

	public enum TaskStatus
	{
		Pending,
		Completed,
		Overdue
	}

	public enum TaskPriority
	{
		Low,
		Medium,
		High
		
	}
    public class DueDateGreaterThanCreatedAt : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var task = validationContext.ObjectInstance as Task;  // Access the full object (Task)

            if (task != null)
            {
                // Check if DueDate is greater than CreatedAt
                if (task.DueDate <= task.CreatedAt)
                {
                   
                    // Return an error if validation fails
                    return new ValidationResult("Due date must be greater than created date.");
                }
            }

            // If no validation errors, return Success
            return ValidationResult.Success;
        }
    }



}
