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
    [Route("Lists")]
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lists
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            List<Models.List> data = await _context.List
                .Include(l => l.Items)
                .ToListAsync();

            return View(data);
        }

        // GET: Lists/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
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
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] List list)
        {
            if (!ModelState.IsValid)
            {
                // As the model is not valid we want to show the form again with the current values.
                // The problem is that we can't do this with a redirect, which I normally use in Laravel.
                return Create();
            }

            _context.Add(list);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Lists/5/Edit
        [HttpGet("{id:int}/Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _context.List.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }
            return View(list);
        }

        // POST: Lists/5/Edit
        [HttpPost("{id:int}/Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] List list)
        {
            if (id != list.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                // As the model is not valid we want to show the form again with the current values.
                // The problem is that we can't do this with a redirect, which I normally use in Laravel.
                return await Edit(id);
            }

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
    }
}
