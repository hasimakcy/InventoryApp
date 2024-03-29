using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;

namespace InventoryApp.Controllers
{
    public class inventoriesController : Controller
    {
        private readonly InventoryAppContext _context;

        public inventoriesController(InventoryAppContext context)
        {
            _context = context;
        }

        // GET: inventories
        public async Task<IActionResult> Index()
        {
            var inventoryAppContext = _context.inventory.Include(i => i.CompanyName);
            return View(await inventoryAppContext.ToListAsync());
        }

        // GET: inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.CompanyName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: inventories/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name");


            return View();   
        }



        // POST: inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,PriceWithoutTax,Tax,PriceWithTax,Quantity,TotalPrice,CompanyId")] inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name", inventory.CompanyId);

            inventory.PriceWithTax = inventory.PriceWithoutTax + inventory.PriceWithoutTax * inventory.Tax;
            
            inventory.TotalPrice = inventory.PriceWithTax * inventory.Quantity;




            return View(inventory);
        }

        // GET: inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name", inventory.CompanyId);
            return View(inventory);
        }

        // POST: inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PriceWithoutTax,Tax,PriceWithTax,Quantity,TotalPrice,CompanyId")] inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!inventoryExists(inventory.Id))
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
            ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name", inventory.CompanyId);
            return View(inventory);
        }

        // GET: inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.inventory
                .Include(i => i.CompanyName)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.inventory == null)
            {
                return Problem("Entity set 'InventoryAppContext.inventory'  is null.");
            }
            var inventory = await _context.inventory.FindAsync(id);
            if (inventory != null)
            {
                _context.inventory.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool inventoryExists(int id)
        {
          return (_context.inventory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
