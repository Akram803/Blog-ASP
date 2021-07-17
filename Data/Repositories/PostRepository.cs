using Blog.Models;
using Blog.ViewModels;
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

        public async Task<PagedList<Post>> GetPosts(PagingParametersVM pagingParameters, int categoryId, string search)
        {
            var query = _context.Posts
                                .AsNoTracking()
                                .AsQueryable();

            if (categoryId > 0)
                query = query.Where(p => p.CategoryId == categoryId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p =>   EF.Functions.Like(p.Title, $"%{search}%")
                                        || EF.Functions.Like(p.Body, $"%{search}%")
                                        || EF.Functions.Like(p.Description, $"%{search}%"));

            // paging
            int count = await query.CountAsync();
            query = query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                    .Take(pagingParameters.PageSize);

            query = query.Include(p => p.User);

            return new PagedList<Post>(
                                await query.ToListAsync(),
                                count, 
                                pagingParameters.PageNumber, 
                                pagingParameters.PageSize
                            );
        }

        public async Task<PagedList<Post>> GetFeed(PagingParametersVM pagingParameters, string[] blogersId)
        {
            var query = _context.Posts
                                .AsNoTracking()
                                .AsQueryable();

            query.Where(p => 
                    blogersId.Any( id => id == p.UserId ) 
                );

            int count = await query.CountAsync();
            query = query
                    .OrderByDescending(p => p.CreatedAt)
                    .Skip((pagingParameters.PageNumber - 1) * pagingParameters.PageSize)
                    .Take(pagingParameters.PageSize);

            query = query.Include(p => p.User);
            return new PagedList<Post>(
                                await query.ToListAsync(),
                                count,
                                pagingParameters.PageNumber,
                                pagingParameters.PageSize
                            );
        }

        public async Task<Post> GetByIdFull(int id)
        {
            var post = await _context.Posts
                                .Include(p => p.Category)
                                .Include(p => p.User)
                                .Include(p => p.MainComments)
                                    .ThenInclude(mc => mc.User)
                                .Include(p => p.MainComments)
                                    .ThenInclude(mc => mc.SubComments )
                                        .ThenInclude(sc => sc.User)
                                .FirstOrDefaultAsync(p => p.Id == id);

            return post;


        }

        public async Task<Post> GetById(int id)
        {
            var post = await _context.Posts
                                .Include(p => p.Category)
                                .Include(p => p.User)
                                .FirstOrDefaultAsync(p => p.Id == id);

            return post;
        }

        public async Task<List<Post>> GetByUserId(string id)
        {
            return await _context.Posts
                            .Where(p => p.UserId == id)
                            .ToListAsync();
        }

        public async Task<Post> Check(int id)
        {
            return await _context.Posts
                     .AsNoTracking()
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
