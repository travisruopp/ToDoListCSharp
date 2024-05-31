using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Context;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoDbContext _context;
        private readonly ILogger<ToDoController> logger;

        public ToDoController(ToDoDbContext context, ILogger<ToDoController> logger)
        {
            _context = context;
            this.logger = logger;
            logger.LogInformation("To Do Controller Started");
        }

        
        // GET: ToDo
        public async Task<IActionResult> Index(string priority, bool? status, string searchString, string category)
        {
            if (_context.ToDoModel == null)
            {
                return Problem("Entity set 'ToDoDbContext.ToDoModel' is null.");
            }

            var items = from t in _context.ToDoModel
                        select t;

            if (!string.IsNullOrEmpty(priority))
            {
                items = items.Where(s => s.Priority == priority);
            }

            if (status.HasValue)
            {
                items = items.Where(s => s.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Description.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(category))
            {
                items = items.Where(s => s.Category == category);
            }

            // Custom sorting by priority
            items = items.OrderByDescending(s => s.Priority == "High")
                         .ThenByDescending(s => s.Priority == "Medium")
                         .ThenByDescending(s => s.Priority == "Low")
                         .ThenBy(s => s.DueDate);
            return View(await items.ToListAsync());
        }
    

    // GET: ToDo/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            
                if (id == null || _context.ToDoModel == null)
                {
                    return NotFound();
                }

                var toDoModel = await _context.ToDoModel
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (toDoModel == null)
                {
                    return NotFound();
                }
            
            

            return View(toDoModel);
        }

        // GET: ToDo/Create
        public IActionResult Create()
        {
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" });
            ViewData["CategoryList"] = new SelectList(new[] { "Work", "Personal", "Shopping", "Others" });
            return View();
        }

        // POST: ToDo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Status,Priority,DueDate,Category")] ToDoModel toDoModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDoModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" });
            ViewData["CategoryList"] = new SelectList(new[] { "Work", "Personal", "Shopping", "Others" });
            return View(toDoModel);
        }

        // GET: ToDo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoModel == null)
            {
                return NotFound();
            }

            var toDoModel = await _context.ToDoModel.FindAsync(id);
            if (toDoModel == null)
            {
                return NotFound();
            }
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" });
            ViewData["CategoryList"] = new SelectList(new[] { "Work", "Personal", "Shopping", "Others" });
            return View(toDoModel);
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Status,Priority,DueDate,Category")] ToDoModel toDoModel)
        {
            if (id != toDoModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoModelExists(toDoModel.Id))
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
            ViewData["PriorityList"] = new SelectList(new[] { "High", "Medium", "Low" });
            ViewData["CategoryList"] = new SelectList(new[] { "Work", "Personal", "Shopping", "Others" });
            return View(toDoModel);
        }

        // GET: ToDo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoModel == null)
            {
                return NotFound();
            }

            var toDoModel = await _context.ToDoModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoModel == null)
            {
                return NotFound();
            }

            return View(toDoModel);
        }

        // POST: ToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoModel == null)
            {
                return Problem("Entity set 'ToDoDbContext.ToDoModel'  is null.");
            }
            var toDoModel = await _context.ToDoModel.FindAsync(id);
            if (toDoModel != null)
            {
                _context.ToDoModel.Remove(toDoModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoModelExists(int id)
        {
          return (_context.ToDoModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // POST: ToDo/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var toDoItem = await _context.ToDoModel.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            toDoItem.Status = !toDoItem.Status; // Toggle the status
            _context.Update(toDoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
