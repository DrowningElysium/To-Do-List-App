using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lists
        public async Task<IActionResult> Index()
        {
            List<Models.List> data = await _context.List
                .Include(l => l.Items)
                .ToListAsync();

            return View(data);
        }

        // GET: Lists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .Include(l => l.Items)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // GET: Lists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] List list)
        {
            if (ModelState.IsValid)
            {
                _context.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(list);
        }

        // GET: Lists/Edit/5
        public async Task<IActionResult> Edit(int? listId)
        {
            if (listId == null)
            {
                return NotFound();
            }

            var list = await _context.List.FindAsync(listId);
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        // POST: Lists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int listId, [Bind("Id,Name")] List list)
        {
            if (listId != list.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.Id))
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
            return View(list);
        }

        // GET: Lists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.List
                .FirstOrDefaultAsync(m => m.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            return View(list);
        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var list = await _context.List.FindAsync(id);
            _context.List.Remove(list);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListExists(int id)
        {
            return _context.List.Any(e => e.Id == id);
        }
    }
}
