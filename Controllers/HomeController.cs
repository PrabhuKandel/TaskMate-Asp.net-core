using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Task_Manager.Models;

namespace Task_Manager.Controllers
{
    public class HomeController : Controller
    {

		private readonly ApplicationDbContext _context;

		public HomeController( ApplicationDbContext context)
        {
       
            _context = context;
        }

			public async Task<IActionResult> Index()

			{


				var allTasks = await _context.Tasks.ToListAsync();

				var totalTasks =await _context.Tasks.CountAsync(); //toal tasks
				var pendingTasks = await _context.Tasks.CountAsync(t => t.Status == Task_Manager.Models.TaskStatus.Pending); // Pending tasks
				var completedTasks =await  _context.Tasks.CountAsync(t => t.Status == Task_Manager.Models.TaskStatus.Completed);
				ViewBag.TotalTasks = totalTasks;
				ViewBag.PendingTasks = pendingTasks;
				ViewBag.CompletedTasks = completedTasks;

			return View();

			}

       
		}
}
