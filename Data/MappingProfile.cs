using AutoMapper;
using Blog.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel,IdentityUser >()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email)) ;
        }

    }
}
