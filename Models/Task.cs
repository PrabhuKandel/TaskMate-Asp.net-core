using System.ComponentModel.DataAnnotations;

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
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Auto-set creation date

		public DateTime? DueDate { get; set; } // Nullable: Task may not have a deadline

	
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
}
