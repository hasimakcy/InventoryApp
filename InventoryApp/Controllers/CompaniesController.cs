using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventoryApp.Data;
using InventoryApp.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InventoryApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly InventoryAppContext _context;

        public CompaniesController(InventoryAppContext context)
        {
            _context = context;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {

            


            return _context.Company != null ? 
                          View(await _context.Company.ToListAsync()) :
                          Problem("Entity set 'InventoryAppContext.Company'  is null.");
            

           
        }

        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Company == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }
            var companyy = await _context.Company.FindAsync(id);
            var companies = _context.Company.Include(c => c.Inventories).ToList().Where(x => x.Id == company.Id);

            double TotalPriceWithoutTax = companies.Sum(company => company.Inventories.Sum(inventory => inventory.PriceWithoutTax));
            double TotalTaxOfProducts = companies.Sum(company => company.Inventories.Sum(inventory => inventory.Tax));
            double TotalPriveWithTaxProducts = TotalPriceWithoutTax + TotalPriceWithoutTax * TotalTaxOfProducts;
            int NumberOfProduct = companies.Sum(company => company.Inventories.Count());

            ViewData["TotalPriceWithoutTax"] = TotalPriceWithoutTax;
            ViewData["TotalTaxOfProducts"] = TotalTaxOfProducts;
            ViewData["TotalPriveWithTaxProducts"] = TotalPriveWithTaxProducts;
            ViewData["NumberOfProduct"] = NumberOfProduct;

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreationDate,PriceWithTax")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Company == null)
            {
                return NotFound();
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreationDate")] Company company)
            //,PriceWithTax
        {
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Company == null)
            {
                return NotFound();
            }

            var company = await _context.Company
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Company == null)
            {
                return Problem("Entity set 'InventoryAppContext.Company'  is null.");
            }
            var company = await _context.Company.FindAsync(id);

            var companies = _context.Company.Include(c => c.Inventories).ToList().Where(x => x.Id == company.Id);
            var doluMu = companies.Sum(x => x.Inventories.Count());
            

            if (company != null && doluMu == 0)
            {
                _context.Company.Remove(company);
            }
            else
            {
                TempData["Message"] = "Cannot be deleted because inventory is full";
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
          return (_context.Company?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
