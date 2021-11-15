using HelpingHands.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Category View
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var categoryList = await _dbContext.MeterialItems.Where(x => x.IsActive == true).ToListAsync();
            ViewBag.toalCount = categoryList.Count;
            return View(categoryList);
        }
        /// <summary>
        /// Category Create View
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            return View();
        }
        /// <summary>
        /// Add Category
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(MeterialItems model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                model.IsActive = true;
                _dbContext.MeterialItems.Add(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }

        }
        [Authorize]
        /// <summary>
        /// Get Category Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var categoryDetails = await _dbContext.MeterialItems.FirstOrDefaultAsync(m => m.MaterialItemId == id);

                if (categoryDetails == null)
                {
                    return NotFound();
                }
                return View(categoryDetails);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }
            
        }
        /// <summary>
        /// Update Category Details
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(MeterialItems category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var categoryDetails = await _dbContext.MeterialItems.AsNoTracking().FirstOrDefaultAsync(m => m.MaterialItemId == category.MaterialItemId);
                if (categoryDetails != null)
                {
                    category.IsActive = categoryDetails.IsActive;
                    _dbContext.MeterialItems.Update(category);
                    await _dbContext.SaveChangesAsync();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }


        }
        [Authorize]
        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                MeterialItems category = await _dbContext.MeterialItems.FindAsync(id);

                if (category != null)
                {
                    category.IsActive = false;
                    _dbContext.MeterialItems.Update(category);
                    await _dbContext.SaveChangesAsync();
                }
                ViewBag.DeleteMessage = "Delete Record Successfully";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }

        }
    }
}
