using HelpingHands.Controllers;
using HelpingHands.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnitTest_HelpingHands.MockDBContext;
using Xunit;

namespace UnitTest_HelpingHands
{
    public class VolunteerUnitTest
    {
        [Fact(DisplayName = "Get all the Volunteers")]
        public void Index_ReturnsAViewResult_WithAListOfVolunteer()
        {
            // Arrange
            using (var context = TestDbContext.GetTestDbContext())
            using (var controller = new VolunteerController(context))
            {
                AddTestData(context);
                // Act
                ViewResult result = controller.Index();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(3, result.ViewData["toalCount"]);
            }
        }
        /// <summary>
        /// This method used to Add new volunteer with bad request
        /// </summary>
        /// <returns></returns>
        [Fact(DisplayName ="Add New Volunteer with Bad Request")]
        public async Task Add_Volunteer_WhenModelStateIsInvalid()
        {
            // Arrange 
            var newVolunteer = new Volunteer { VolunteerId = 1, FirstName = "Jay", LastName = "Jain", Gender = "Male", Age = 23, MobileNumber = "1234567890",IsActive = true };
            using (var context = TestDbContext.GetTestDbContext())
            using (var volunteercontroller = new VolunteerController(context))
            {
                // Act
                volunteercontroller.ModelState.AddModelError("EmailId", "Required");
                var volunteerBadrequest = await volunteercontroller.Create(newVolunteer);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(volunteerBadrequest);
                Assert.IsType<SerializableError>(badRequestResult.Value);
            }
            
        }
        /// <summary>  
        ///  This method used to to add new volunteer  
        /// </summary>  
        [Fact(DisplayName = "Add New Volunteer")]
        public async Task Add_Volunteer_WhenModelStateIsValid()
        {
            // Arrange  
            var newVolunteer = new Volunteer { VolunteerId = 1, FirstName = "Jay" , EmailId="JayJain@yahoo.com", LastName="Jain"  ,Gender="Male",Age=23,MobileNumber="1234567890" };

            using (var context = TestDbContext.GetTestDbContext())
            using (var volunteercontroller = new VolunteerController(context))
            {
                var volunteerresult = await volunteercontroller.Create(newVolunteer) as ViewResult;

                Assert.NotNull(volunteerresult);
                Assert.Equal("Thank You For Become A Volunteer", volunteerresult.ViewData["Message"]);
            }
            
        }
        /// <summary>
        /// Add Volunteer Test Data
        /// </summary>
        /// <param name="context"></param>
        private static void AddTestData(AppDbContext context)
        {
            var testVolunteer1 = new Volunteer
            {
                VolunteerId = 11,
                FirstName = "Jhon",
                LastName = "Dezy",
                EmailId="JohnD@yahoo.com",
                Gender="Male",
                Age=45,
                MobileNumber="6756565656",
                IsActive = true
            };

            context.Volunteer.Add(testVolunteer1);

            var testVolunteer2 = new Volunteer
            {
                VolunteerId = 12,
                FirstName = "Prince",
                LastName = "Khan",
                EmailId = "PrinceK@yahoo.com",
                Gender = "Male",
                Age = 35,
                MobileNumber = "2323334444",
                IsActive = true
            };

            context.Volunteer.Add(testVolunteer2);

            var testVolunteer3 = new Volunteer
            {
                VolunteerId = 13,
                FirstName = "Mohib",
                LastName = "Shah",
                EmailId = "MohibS@yahoo.com",
                Gender = "Male",
                Age = 25,
                MobileNumber = "5454545454",
                IsActive = true
            };

            context.Volunteer.Add(testVolunteer3);

            context.SaveChanges();
        }

    }
}
