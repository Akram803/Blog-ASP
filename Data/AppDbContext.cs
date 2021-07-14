using Blog.Models;
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

        // ========  default is Cascading ============
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //modelBuilder.Entity<Category>()
        //    .HasMany(c => c.Posts)
        //    .WithOne(p => p.Category)
        //    .HasForeignKey(p => p.CategoryId)
        //    .OnDelete(DeleteBehavior.SetNull);

        //modelBuilder.Entity<Post>()
        //    .HasMany(p => p.MainComments)
        //    .WithOne(mc => mc.Post)
        //    .HasForeignKey(mc => mc.PostId)
        //    .OnDelete(DeleteBehavior.Cascade);

        //}

    }
}
