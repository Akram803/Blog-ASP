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

        public void AddMainComment(MainComment comment)
        {
            if (comment != null)
                _context.MainComments.Add(comment);
            else
                throw new NullReferenceException();
        }

        public void AddSubComment(SubComment comment)
        {
            if (comment != null)
                _context.SubComments.Add(comment);
            else
                throw new NullReferenceException();
        }
    }
}
