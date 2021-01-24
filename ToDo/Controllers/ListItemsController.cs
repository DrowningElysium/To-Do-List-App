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
    [Route("Lists/{listId:int}/Items")]
    public class ListItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lists/1/Create
        [HttpGet("Create")]
        public async Task<IActionResult> Create(int listId)
        {
            ViewData["List"] = await _context.List
                .FindAsync(listId);
            return View();
        }

        // POST: Lists/1/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int listId, [Bind("ListId,Name")] Item item)
        {
            if (!ModelState.IsValid)
            {
                // As the model is not valid we want to show the form again with the current values.
                // The problem is that we can't do this with a redirect, which I normally use in Laravel.
                return await Create(listId);
            }

            _context.Add(item);
            await _context.SaveChangesAsync();

            // Hardcoded redirect as RedirectToAction doesn't seem to understand my URL scheme
            return Redirect($"/Lists/{listId}");
        }

        // GET: Lists/1/Items/5/Edit
        [HttpGet("{id:int}/Edit")]
        public async Task<IActionResult> Edit(int listId, int id)
        {
            var item = await _context.Item
                .Include(i => i.List)
                .Where(i => i.Id == id)
                .FirstAsync();

            if (item == null || item.ListId != listId)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Lists/1/Items/5/Edit
        [HttpPost("{id:int}/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int listId, int id, [Bind("Id,ListId,Name")] Item item)
        {
            if (!ModelState.IsValid)
            {
                // As the model is not valid we want to show the form again with the current values.
                // The problem is that we can't do this with a redirect, which I normally use in Laravel.
                return await Edit(listId, id);
            }

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

            // Hardcoded redirect as RedirectToAction doesn't seem to understand my URL scheme
            return Redirect($"/Lists/{listId}");
        }

        // GET: Lists/1/Items/5/Complete
        [HttpGet("/{id:int}/Complete")]
        public async Task<IActionResult> Complete(int listId, int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item == null || item.ListId != listId)
            {
                return NotFound();
            }

            item.CompletionDate = DateTime.UtcNow;
            _context.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "ListsController", new { id = listId });
        }

        // POST: Lists/1/Items/5/Delete
        [HttpPost("{id:int}/Delete"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int listId, int id)
        {
            Item item = await _context.Item
                .FindAsync(id);

            if (item == null || item.ListId != listId)
            {
                return NotFound();
            }

            List list = await _context.List
                .Include(l => l.Items)
                .Where(l => l.Id == listId)
                .FirstAsync();

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            // If last item of list, delete list.
            if (list.Items.Count < 1) { 
            
                _context.List.Remove(list);
            }

            // Commit DB changes.
            await _context.SaveChangesAsync();

            // Hardcoded redirect as RedirectToAction doesn't seem to understand my URL scheme
            return Redirect($"/Lists/{listId}");
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
