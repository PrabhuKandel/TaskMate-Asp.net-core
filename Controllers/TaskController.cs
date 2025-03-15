using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Task_Manager.Models.DTOs;
using Task_Manager.Utilities;

namespace Task_Manager.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task<IActionResult> Index(Task_Manager.Models.TaskStatus ? status, Task_Manager.Models. TaskPriority? priority, string? dueDate, string? search)
        {
            String userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return Unauthorized();
            }
            await TimeAgoHelper.UpdateOverdueTasksAsync(_context, userId);

            var tasks = _context.Tasks.AsQueryable();
             tasks =    _context.Tasks.Where(t => t.UserId == userId);
            // Apply Status Filter
            if (status.HasValue)
            {
                tasks = tasks.Where(t => t.Status == status.Value);
            }

            // Apply Priority Filter
            if (priority.HasValue)
            {
                tasks = tasks.Where(t => t.Priority == priority.Value);
            }

            // Apply Due Date Sorting
            if (!string.IsNullOrEmpty(dueDate))
            {
                tasks = dueDate.ToLower() == "asc"
                    ? tasks.OrderBy(t => t.DueDate)
                    : tasks.OrderByDescending(t => t.DueDate);
            }
            // Apply Search Filter (Search by Task Name)
            if (!string.IsNullOrEmpty(search))
            {
                tasks = tasks.Where(t => t.Name.Contains(search));  // This filters by task name
            }

            var taskDTOs = await tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate, // Format due date as needed
                CreatedAt = t.CreatedAt

            }).ToListAsync();

           if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
           {
                return PartialView("_TaskListPartial", taskDTOs); 
            }
            return View(taskDTOs);

        }

   
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            task.CreatedAt = task.CreatedAt.ToLocalTime();
            task.DueDate = task.DueDate.ToLocalTime();
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

    
        public IActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Task_Manager.Models.Task task)
        {
         
            task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


 
            task.CreatedAt = task.CreatedAt.ToUniversalTime(); 
            task.DueDate = task.DueDate.ToUniversalTime();     


            ModelState.Remove(nameof(task.UserId));
            if (ModelState.IsValid)
            {
               

                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == user.Id);
            task.CreatedAt = task.CreatedAt.ToLocalTime();
            task.DueDate = task.DueDate.ToLocalTime();
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Task_Manager.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            task.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            task.CreatedAt = task.CreatedAt.ToUniversalTime();  
            task.DueDate = task.DueDate.ToUniversalTime();    


            if (!ModelState.IsValid)
            {
                return View(task);
            }
        
               
                _context.Update(task);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
            
    
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int taskId, bool isChecked)
        {
     

            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
            {
                return NotFound(new { success = false, message = "Task not found" });
            }
            if(isChecked)
            {
                task.Status = Models.TaskStatus.Completed;
            }
            else
            {
                task.Status = task.DueDate < DateTime.Now.ToUniversalTime() ? Models.TaskStatus.Overdue : Models.TaskStatus.Pending;
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true, status = task.Status });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int taskId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);
            if (task == null)
            {
                return Json(new { success = false, message = "Task not found or you do not have permission to delete it." });
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Task deleted successfully" });
        }





        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
