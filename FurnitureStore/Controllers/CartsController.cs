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
    public class CartsController : Controller
    {
        private readonly furnitureStoreContext _context;

        public CartsController(furnitureStoreContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var furnitureStoreContext = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Images);
            return View(await furnitureStoreContext.ToListAsync());
        }


        public async Task<IActionResult> AddToCart(int itemId)
        {
            try
            {
                var existingCartItem = await _context.Carts
                    .FirstOrDefaultAsync(c => c.Items_ID == itemId);

                // Get the last Cart_id and increment it
                int lastCartId = await _context.Carts
                    .OrderByDescending(c => c.Cart_id)
                    .Select(c => c.Cart_id)
                    .FirstOrDefaultAsync();

                int newCartId = lastCartId + 1;

                // Create new cart item with manually incremented ID
                var cartItem = new Cart
                {
                    Cart_id = newCartId, // Manually set the incremented ID
                    Items_ID = itemId,
                    // Add other properties if your Cart model has them
                };

                _context.Carts.Add(cartItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false});
            }
        }


        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(m => m.Cart_id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cart_id,UserId,Items_ID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", cart.Items_ID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", cart.Items_ID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Cart_id,UserId,Items_ID")] Cart cart)
        {
            if (id != cart.Cart_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Cart_id))
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
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", cart.Items_ID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(m => m.Cart_id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Carts/ProcessOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder()
        {
            var cartItems = await _context.Carts.ToListAsync();
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction(nameof(Index));
            }

            int lastPurchasedId = await _context.PurchasedItems
                .OrderByDescending(p => p.PurchasedItems_ID)
                .Select(p => p.PurchasedItems_ID)
                .FirstOrDefaultAsync();

            foreach (var item in cartItems)
            {
                lastPurchasedId++;
                var purchasedItem = new PurchasedItem
                {
                    PurchasedItems_ID = lastPurchasedId,
                    Items_ID = item.Items_ID,
                    DatePurchased = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.PurchasedItems.Add(purchasedItem);
            }

            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Cart_id == id);
        }
    }
}
