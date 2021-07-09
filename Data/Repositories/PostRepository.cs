using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public class PostRepository : RepositoryBase
    {
        public PostRepository(AppDbContext context) 
            : base(context)
        {
        }


        public async Task Add(Post post)
        {
            if (post != null)
                await _context.Posts.AddAsync(post);
            else
                throw new NullReferenceException();
        }

        public async Task<List<Post>> GetAll()
        {
            return await _context.Posts.ToListAsync();
        }

        public async Task<Post> GetById(int id)
        {
            return await _context.Posts
                            .Include(p => p.MainComments)
                            .ThenInclude(mc => mc.SubComments)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }


        public void Update(Post post)
        {
            _context.Posts.Update(post);
        }

        public void Remove(Post post)
        {
            _context.Posts.Remove(post);
        }

    }
}
