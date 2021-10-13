using AutoMapper;
using HelpingHands.Models;
using HelpingHands.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
     
    public class ContributorController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> userManager;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public ContributorController(AppDbContext dbContext, IMapper mapper, UserManager<IdentityUser> _userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            this.userManager = _userManager;
        }
        /// <summary>
        /// Get Donation Requirement Data
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            List<RequirementViewModel> list = new List<RequirementViewModel>();
            try
            {
                var donationRequirement = await _dbContext.DonationRequirement.ToListAsync();
                ViewBag.toalCount = donationRequirement.Count;

                if (donationRequirement.Any())
                {
                    foreach (DonationRequirement item in donationRequirement)
                    {
                        RequirementViewModel requirementViewModel = new RequirementViewModel();
                        requirementViewModel = _mapper.Map<RequirementViewModel>(item);
                        var categoryName = await _dbContext.MeterialItems.Where(x => x.MaterialItemId == item.MaterialItemId).Select(x => x.MererialItemName).FirstOrDefaultAsync();
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
        /// Get Donation Requirement Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                RequirementViewModel donationRequirementViewModel = null;
                DonationRequirement donationRequirement = await _dbContext.DonationRequirement.FindAsync(id);
                if (donationRequirement != null)
                {
                    donationRequirementViewModel = new RequirementViewModel();
                    donationRequirementViewModel = _mapper.Map<RequirementViewModel>(donationRequirement);
                }
                return View(donationRequirementViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Something went wrong";
                return View();
            }
            
        }
        /// <summary>
        /// Update Donation Details
        /// </summary>
        /// <param name="requirementViewModel"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Donate(RequirementViewModel requirementViewModel)
        {
            try
            {
                if (requirementViewModel != null)
                {
                    DonationRequirement donationRequirement = await _dbContext.DonationRequirement.FindAsync(requirementViewModel.DonationRequirementId);
                    if (donationRequirement != null)
                    {
                        donationRequirement.DeliveryStatus = "In Progress";
                        _dbContext.DonationRequirement.Update(donationRequirement);
                        await _dbContext.SaveChangesAsync();
                    }
                    // Update Contributor Details
                    ContributorDetails contributorDetails = new ContributorDetails();
                    IdentityUser applicationUser = await userManager.GetUserAsync(User);
                    string userId = applicationUser?.Id;
                    contributorDetails.UserId = userId;
                    contributorDetails.DispatchDate = DateTime.UtcNow;
                    contributorDetails.RecipientInfo = requirementViewModel.RecipientInfo;
                    _dbContext.ContributorDetails.Add(contributorDetails);
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
