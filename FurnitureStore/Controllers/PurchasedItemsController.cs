using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FurnitureStore.Models;

namespace FurnitureStore.Controllers
{
    public class PurchasedItemsController : Controller
    {
        private readonly furnitureStoreContext _context;

        public PurchasedItemsController(furnitureStoreContext context)
        {
            _context = context;
        }

        // GET: PurchasedItems
        public async Task<IActionResult> Index()
        {
            var furnitureStoreContext = _context.PurchasedItems.Include(p => p.Items).ThenInclude(i => i.Images);
            return View(await furnitureStoreContext.ToListAsync());
        }

        // GET: PurchasedItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasedItem = await _context.PurchasedItems
                .Include(p => p.Items)
                .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(m => m.PurchasedItems_ID == id);
            if (purchasedItem == null)
            {
                return NotFound();
            }

            return View(purchasedItem);
        }

        // GET: PurchasedItems/Create
        public IActionResult Create()
        {
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description");
            return View();
        }

        // POST: PurchasedItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchasedItems_ID,DatePurchased,UserId,Items_ID")] PurchasedItem purchasedItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchasedItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", purchasedItem.Items_ID);
            return View(purchasedItem);
        }

        // GET: PurchasedItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasedItem = await _context.PurchasedItems.FindAsync(id);
            if (purchasedItem == null)
            {
                return NotFound();
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", purchasedItem.Items_ID);
            return View(purchasedItem);
        }

        // POST: PurchasedItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchasedItems_ID,DatePurchased,UserId,Items_ID")] PurchasedItem purchasedItem)
        {
            if (id != purchasedItem.PurchasedItems_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchasedItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchasedItemExists(purchasedItem.PurchasedItems_ID))
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
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", purchasedItem.Items_ID);
            return View(purchasedItem);
        }

        // GET: PurchasedItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchasedItem = await _context.PurchasedItems
                .Include(p => p.Items)
                .FirstOrDefaultAsync(m => m.PurchasedItems_ID == id);
            if (purchasedItem == null)
            {
                return NotFound();
            }

            return View(purchasedItem);
        }

        // POST: PurchasedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchasedItem = await _context.PurchasedItems.FindAsync(id);
            if (purchasedItem != null)
            {
                _context.PurchasedItems.Remove(purchasedItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchasedItemExists(int id)
        {
            return _context.PurchasedItems.Any(e => e.PurchasedItems_ID == id);
        }
    }
}
