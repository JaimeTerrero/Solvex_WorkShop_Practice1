using Application.DTOs;
using AutoMapper;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GeneralProfile
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDto, Product>();

            CreateMap<SignUp, User>();

            CreateMap<LogIn, User>();
        }
    }
}
