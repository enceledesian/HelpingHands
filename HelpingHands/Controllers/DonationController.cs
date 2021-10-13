using AutoMapper;
using HelpingHands.Models;
using HelpingHands.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.Controllers
{
    /// <summary>
    /// Controller
    /// </summary>
    
    public class DonationController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public DonationController(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult >Create()
        {
            try
            {
                List<SelectListItem> categories = _dbContext.MeterialItems.AsNoTracking().Where(x=>x.IsActive == true)
                    .OrderBy(n => n.MererialItemName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.MaterialItemId.ToString(),
                            Text = n.MererialItemName
                        }).ToList();
                ViewBag.CategoryList = categories;
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return View();
        }
        /// <summary>
        /// Create Donation Requirement
        /// </summary>
        /// <param name="donationRequirementView"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DonationRequirementViewModel donationRequirementView, IFormFile file, IFormCollection form)
        {
            try
            {
                DonationRequirement donationRequirement = new DonationRequirement();
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (file != null && file.Length > 0)
                {
                    string fileName = await UploadedProfileImage(file);
                    donationRequirement.Photo = fileName;
                }

                donationRequirement.FirstName = donationRequirementView.FirstName;
                donationRequirement.LastName = donationRequirementView.LastName;
                donationRequirement.Age = donationRequirementView.Age;
                donationRequirement.MaterialItemId = Convert.ToInt32(form["CategoryId"].ToString());
                donationRequirement.Description = donationRequirementView.Description;
                donationRequirement.Address = donationRequirementView.Address;
                donationRequirement.DeliveryDate = donationRequirementView.DeliveryDate;
                donationRequirement.DeliveryStatus = donationRequirementView.DeliveryStatus;
                _dbContext.DonationRequirement.Add(donationRequirement);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }

        }
        /// <summary>
        /// Get Donation Requirement Info
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<RequirementViewModel> list = new List<RequirementViewModel>();
            try
            {
                var donationRequirement = await _dbContext.DonationRequirement.ToListAsync();
                ViewBag.toalCount = donationRequirement.Count;
                
                if(donationRequirement.Any())
                {
                    foreach (DonationRequirement item in donationRequirement)
                    {
                        RequirementViewModel requirementViewModel = new RequirementViewModel();
                        requirementViewModel = _mapper.Map<RequirementViewModel>(item);
                        var categoryName = await _dbContext.MeterialItems.Where(x => x.MaterialItemId == item.MaterialItemId).Select(x=>x.MererialItemName).FirstOrDefaultAsync();
                        requirementViewModel.CategoryName = categoryName;
                        list.Add(requirementViewModel);
                    }
                }
                return View(list);
            }
            catch (Exception ex)
            {
                return View();
            }

        }
        /// <summary>
        /// Delete Donation Requirement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                DonationRequirement donationRequirement = await _dbContext.DonationRequirement.FindAsync(id);

                if (donationRequirement != null)
                {
                    _dbContext.DonationRequirement.Remove(donationRequirement);
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
        /// <summary>
        /// Upload Profile Image
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task<string> UploadedProfileImage(IFormFile file)
        {
            string uniqueFileName = string.Empty;
            try
            {
                if (file != null)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                        var fileExtension = Path.GetExtension(fileName);
                        uniqueFileName = String.Concat(myUniqueFileName, fileExtension);

                        var filepath =
                new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{uniqueFileName}";

                        using (FileStream fs = System.IO.File.Create(filepath))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return uniqueFileName;
        }
        /// <summary>
        /// Get Donation Requirement Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var donationRequirementDetails = await _dbContext.DonationRequirement.FirstOrDefaultAsync(m => m.DonationRequirementId == id);

                if (donationRequirementDetails == null)
                {
                    return NotFound();
                }

                List<SelectListItem> categories = _dbContext.MeterialItems.AsNoTracking().Where(x => x.IsActive == true)
                    .OrderBy(n => n.MererialItemName)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.MaterialItemId.ToString(),
                            Text = n.MererialItemName
                        }).ToList();
                ViewBag.CategoryList = categories;

                return View(donationRequirementDetails);
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }

        }
        /// <summary>
        /// Update Donation Requirement Details
        /// </summary>
        /// <param name="donationRequirementView"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(DonationRequirement donationRequirementView)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var donationRequirementDetails = await _dbContext.DonationRequirement.AsNoTracking().FirstOrDefaultAsync(m => m.DonationRequirementId == donationRequirementView.DonationRequirementId);
                if (donationRequirementDetails != null)
                {
                    donationRequirementView.Photo = donationRequirementDetails.Photo;
                    _dbContext.DonationRequirement.Update(donationRequirementView);
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
