using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Task_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Task_Manager.Controllers
{
    public class HomeController : Controller
    {

		private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController( ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
       
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            return RedirectToAction("Dashboard");
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()

        {
            var user = await _userManager.GetUserAsync(User);
              var userId = user.Id;

            var allTasks = await _context.Tasks.Where(t => t.UserId == userId) .ToListAsync();

            var totalTasks = await _context.Tasks. Where(t => t.UserId == userId).CountAsync(); //toal tasks
            var pendingTasks = await _context.Tasks.CountAsync(t => t.UserId == userId && t.Status == Task_Manager.Models.TaskStatus.Pending); // Pending tasks
            var completedTasks = await _context.Tasks.CountAsync(t => t.UserId == userId && t.Status == Task_Manager.Models.TaskStatus.Completed);
            ViewBag.TotalTasks = totalTasks;
            ViewBag.PendingTasks = pendingTasks;
            ViewBag.CompletedTasks = completedTasks;

            return View();

        }


    }
}
