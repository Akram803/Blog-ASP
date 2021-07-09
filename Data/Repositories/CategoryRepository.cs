using Blog.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public class CategoryRepository : RepositoryBase
    {
        public CategoryRepository(AppDbContext context)
            :base(context)
        {

        }


        public async Task<List<Category>> Getall()
        {
            return await _context.Categories.ToListAsync();
        }

        public void Add(Category category)
        {
            if (category != null)
                _context.Categories.Add(category);
            else
                throw new NullReferenceException();
        }

        public async Task Remove(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(cat);
        }

    }
}
