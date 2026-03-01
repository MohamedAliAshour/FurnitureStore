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
    public class WishListsController : Controller
    {
        private readonly furnitureStoreContext _context;

        public WishListsController(furnitureStoreContext context)
        {
            _context = context;
        }

        // GET: WishLists
        public async Task<IActionResult> Index()
        {
            var furnitureStoreContext = _context.WishLists.Include(w => w.Items).ThenInclude(i => i.Images);
            return View(await furnitureStoreContext.ToListAsync());
        }

        // GET: WishLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists
                .Include(w => w.Items)
                .ThenInclude(i => i.Images)
                .FirstOrDefaultAsync(m => m.WishList_ID == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // GET: WishLists/Create
        public IActionResult Create()
        {
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description");
            return View();
        }

        // POST: WishLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WishList_ID,UserId,Items_ID")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", wishList.Items_ID);
            return View(wishList);
        }

        // GET: WishLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList == null)
            {
                return NotFound();
            }
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", wishList.Items_ID);
            return View(wishList);
        }

        // POST: WishLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WishList_ID,UserId,Items_ID")] WishList wishList)
        {
            if (id != wishList.WishList_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WishListExists(wishList.WishList_ID))
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
            ViewData["Items_ID"] = new SelectList(_context.Items, "Items_ID", "Description", wishList.Items_ID);
            return View(wishList);
        }

        // GET: WishLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wishList = await _context.WishLists
                .Include(w => w.Items)
                .FirstOrDefaultAsync(m => m.WishList_ID == id);
            if (wishList == null)
            {
                return NotFound();
            }

            return View(wishList);
        }

        // POST: WishLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList != null)
            {
                _context.WishLists.Remove(wishList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: WishLists/AddToWishList
        [HttpPost]
        public async Task<IActionResult> AddToWishList(int itemId)
        {
            try
            {
                // Check if item already exists in wishlist
                var existingWishListItem = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.Items_ID == itemId);

                if (existingWishListItem != null)
                {
                    return Json(new { success = false, message = "Item already in wishlist" });
                }

                // Get the last ID from the database and increment it
                var lastId = await _context.WishLists
                    .OrderByDescending(w => w.WishList_ID)
                    .Select(w => w.WishList_ID)
                    .FirstOrDefaultAsync();

                var newId = lastId + 1;

                // Create new wishlist entry with manual ID
                var wishListItem = new WishList
                {
                    WishList_ID = newId,
                    UserId = 1, // You might want to get this from the authenticated user
                    Items_ID = itemId
                };

                _context.WishLists.Add(wishListItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Item added to wishlist" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error adding item to wishlist" });
            }
        }

        // POST: WishLists/RemoveFromWishList
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(int itemId)
        {
            try
            {
                // Find the wishlist item by Items_ID
                var wishListItem = await _context.WishLists
                    .FirstOrDefaultAsync(w => w.Items_ID == itemId);

                if (wishListItem == null)
                {
                    return Json(new { success = false, message = "Item not found in wishlist" });
                }

                _context.WishLists.Remove(wishListItem);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Item removed from wishlist" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Error removing item from wishlist" });
            }
        }

        private bool WishListExists(int id)
        {
            return _context.WishLists.Any(e => e.WishList_ID == id);
        }
    }
}
