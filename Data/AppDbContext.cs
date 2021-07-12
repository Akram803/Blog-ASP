﻿using Blog.Models;
using Blog.Models.Comment;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; } 
        public DbSet<MainComment> MainComments { get; set; } 
        public DbSet<SubComment> SubComments { get; set; } 
        public DbSet<Category> Categories { get; set; }
    }
}
