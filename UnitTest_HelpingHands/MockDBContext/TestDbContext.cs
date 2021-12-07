using HelpingHands.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest_HelpingHands.MockDBContext
{
    public class TestDbContext
    {
        public static AppDbContext GetTestDbContext()
        {
            // Create db context options specifying in memory database
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            //Use this to instantiate the db context
            return new AppDbContext(options);

        }
    }
}
