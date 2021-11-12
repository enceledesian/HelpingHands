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
    /// <summary>
    /// Controller
    /// </summary>
    
    public class VolunteerController : Controller
    {
        private readonly AppDbContext _dbContext;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public VolunteerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Get Volunteer List
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var volunteerList = await _dbContext.Volunteer.Where(x=>x.IsActive == true).ToListAsync();
            ViewBag.toalCount = volunteerList.Count;
            return View(volunteerList);
        }
        /// <summary>
        /// Volunteer
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create()
        {
            return View();
        }
        /// <summary>
        /// Add Volunteer Details
        /// </summary>
        /// <param name="volunteer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Volunteer volunteer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                volunteer.IsActive = true;
                _dbContext.Volunteer.Add(volunteer);
                await _dbContext.SaveChangesAsync();
               
                ModelState.Clear();
                ViewBag.Message = "Thank You For Become A Volunteer";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something Went Wrong ! Try Again";
                return View();
            }
            
        }
        [Authorize]
        /// <summary>
        /// Delete Volunteer
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

                Volunteer volunteer = await _dbContext.Volunteer.FindAsync(id);

                if (volunteer != null)
                {
                    volunteer.IsActive = false;
                    _dbContext.Volunteer.Update(volunteer);
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
        [Authorize]
        /// <summary>
        /// Get Volunteer Details
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

                var volunteerDetails = await _dbContext.Volunteer.FirstOrDefaultAsync(m => m.VolunteerId == id);

                if (volunteerDetails == null)
                {
                    return NotFound();
                }
                return View(volunteerDetails);
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }
        /// <summary>
        /// Update Volunteer Details
        /// </summary>
        /// <param name="volunteer"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Volunteer volunteer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var volunteerDetails = await _dbContext.Volunteer.AsNoTracking().FirstOrDefaultAsync(m => m.VolunteerId == volunteer.VolunteerId);
                if (volunteerDetails != null)
                {
                    volunteer.IsActive = volunteerDetails.IsActive;
                    _dbContext.Volunteer.Update(volunteer);
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
    }
}
