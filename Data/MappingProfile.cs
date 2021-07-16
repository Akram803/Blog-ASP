using AutoMapper;
using Blog.Models;
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
            CreateMap<RegisterViewModel, AppUser>();
                //.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email)) ;

            CreateMap<PostViewModel, Post>()
                .ForMember(p => p.Image, opt => opt.MapFrom(vm => vm.ImageName));
            CreateMap<Post, PostViewModel>()
                .ForMember(vm => vm.ImageName, opt => opt.MapFrom(p => p.Image));
        }

    }
}
