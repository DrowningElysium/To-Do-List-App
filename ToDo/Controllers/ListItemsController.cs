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
    [Route("Lists/{listId:int}/Items/[action]")]
    public class ListItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ListItems
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Item.Include(i => i.list);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ListItems/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int listId, int id)
        {
            var item = await _context.Item
                .Include(i => i.list)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: ListItems/Create
        [HttpGet]
        public string Create(int listId)
        {
            return "works";
            //ViewData["ListId"] = new SelectList(_context.List, "Id", "Name");
            //return View();
        }

        // POST: ListItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int listId, [Bind("Id,ListId,Name,CompletionDate")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", item.ListId);
            return View(item);
        }

        // GET: ListItems/5/Edit
        [HttpGet("{id:int}/Edit")]
        public async Task<IActionResult> Edit(int listId, int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", item.ListId);
            return View(item);
        }

        // POST: ListItems/5/Edit
        [HttpPost("{id:int}/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int listId, int id, [Bind("Id,ListId,Name,CompletionDate")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["ListId"] = new SelectList(_context.List, "Id", "Name", item.ListId);
            return View(item);
        }

        // GET: Lists/1/Items/5/Complete
        [HttpGet("{id:int}/Complete")]
        public async Task<IActionResult> Complete(int listId, int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null || item.ListId != listId)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "ListsController", new { listId = listId });
        }

        // GET: ListItems/5/Delete
        [HttpGet("{id:int}/Delete")]
        public async Task<string> Delete(int listId, int id)
        {
            return listId + ", " + id;
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var item = await _context.Item
            //    .Include(i => i.list)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (item == null)
            //{
            //    return NotFound();
            //}

            //return View(item);
        }

        // POST: ListItems/5/Delete
        [HttpPost("{id:int}/Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int listId, int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
