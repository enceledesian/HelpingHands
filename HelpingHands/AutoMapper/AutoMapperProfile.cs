using AutoMapper;
using HelpingHands.Models;
using HelpingHands.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpingHands.AutoMapper
{
    public class AutoMapperProfile : Profile    
    {
        public AutoMapperProfile()
        {
            CreateMap<DonationRequirement, RequirementViewModel>();
        }

    }
}
