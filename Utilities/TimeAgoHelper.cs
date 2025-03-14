using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;
namespace Task_Manager.Utilities


{
    public static class TimeAgoHelper
    {
        public static string TimeAgo(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.Seconds} seconds ago";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{timeSpan.Days} days ago";
            if (timeSpan.TotalDays < 30)
                return $"{timeSpan.Days / 7} weeks ago";
            if (timeSpan.TotalDays < 365)   
                return $"{timeSpan.Days / 30} months ago";

            return $"{timeSpan.Days / 365} years ago";
        }
        public static string TimeUntil(DateTime dueDate)
        {
            TimeSpan timeSpan = dueDate - DateTime.Now.ToUniversalTime();

            if (timeSpan.TotalSeconds < 0)
            {
                return "Overdue";
            }

            if (timeSpan.TotalSeconds < 60)
                return $"{timeSpan.Seconds} seconds left";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.Minutes} minutes left";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.Hours} hours left";
            if (timeSpan.TotalDays < 7)
                return $"{timeSpan.Days} days left";
            if (timeSpan.TotalDays < 30)
                return $"{timeSpan.Days / 7} weeks left";
            if (timeSpan.TotalDays < 365)
                return $"{timeSpan.Days / 30} months left";

            return $"{timeSpan.Days / 365} years left";
        }

       
            public static async System.Threading.Tasks.Task UpdateOverdueTasksAsync(ApplicationDbContext context, string userId)
            {

            // Get Asia/Kathmandu timezone
            var kathmanduTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kathmandu");
            var localKathmanduTime = TimeZoneInfo.ConvertTime(DateTime.Now, kathmanduTimeZone);

            var utcTime = localKathmanduTime.ToUniversalTime();
            await context.Tasks
                    .Where(t => t.UserId == userId
                             && t.Status != Task_Manager.Models.TaskStatus.Completed
                             && t.DueDate < DateTime.UtcNow)
                           .ExecuteUpdateAsync(setters => setters.SetProperty(t => t.Status, Task_Manager.Models.TaskStatus.Overdue));

            }
        
    }
}
