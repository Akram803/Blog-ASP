using Blog.Models.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public class CommentRepository : RepositoryBase
    {
        public CommentRepository(AppDbContext context)
            :base(context)
        {

        }

        public async Task AddMainComment(MainComment comment)
        {
            if (comment != null)
                await _context.MainComments.AddAsync(comment);
            else
                throw new NullReferenceException();
        }

        public async Task AddSubComment(SubComment comment)
        {
            if (comment != null)
                await _context.SubComments.AddAsync(comment);
            else
                throw new NullReferenceException();
        }



        public async Task<MainComment> GetMainById(int id)
        {
            return await _context.MainComments.FindAsync(id);
        }

        public async Task<SubComment> GetSubById(int id)
        {
            return await _context.SubComments.FindAsync(id);
        }

        public void deleteMain(MainComment comment)
        {
            _context.MainComments.Remove(comment);
        }

        public void deleteSub(SubComment comment)
        {
            _context.SubComments.Remove(comment);
        }

    }
}
