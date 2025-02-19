using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Task_Manager.Models;
using Microsoft.VisualBasic;

namespace Task_Manager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Task
        public async Task<IActionResult> Index(Task_Manager.Models.TaskStatus ? status, Task_Manager.Models. TaskPriority? priority, string? dueDate)
        {
            var tasks = _context.Tasks.AsQueryable();

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
           //return Json(new { success = true, data = tasks.ToList() });

           if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
           {
                return PartialView("_TaskListPartial", tasks.ToList()); // Return partial view for AJAX
            }
            return View(await tasks.OrderByDescending(t => t.CreatedAt).ToListAsync());

        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Status,Priority,CreatedAt,DueDate")] Task_Manager.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Status,Priority,CreatedAt,DueDate")] Task_Manager.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int taskId, Task_Manager.Models.TaskStatus status)
        {
            if (taskId <= 0)
            {
                return BadRequest(new { success = false, message = "Invalid task ID" });
            }

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound(new { success = false, message = "Task not found" });
            }

            task.Status = status;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Status updated successfully" });
        }


        [HttpPost] // Ensure it's a POST request
        public async Task<IActionResult> Delete(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return Json(new { success = false, message = "Task not found" });
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
