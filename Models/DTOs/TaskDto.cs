using Task_Manager.Utilities;

namespace Task_Manager.Models.DTOs
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedTimeAgo => TimeAgoHelper.TimeAgo(CreatedAt);
        public string DueDateUntil => TimeAgoHelper.TimeUntil(DueDate);
    }
}
