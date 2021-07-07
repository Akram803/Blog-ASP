using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{ 
    public class Repository : IRepository
    {
        private AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        // create
        public void AddPost(Post post)
        {
            if (post != null)
                _context.Posts.AddAsync(post);
            else 
                throw new NullReferenceException();
        }
        // read
        public async Task< List<Post> > GetAllPosts()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetPost(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        
        public void UpdatePost(Post post)
        {
            _context.Posts.Update(post);
        }
        
        public async Task RemovePost(Post post)
        {
            _context.Posts.Remove(post);
        }


        public async Task<bool> SaveChanges()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
