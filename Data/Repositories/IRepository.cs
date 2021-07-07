using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repositories
{
    public interface IRepository
    {
        // C R U D

        // create 
        public void AddPost(Post post);
        // read 
        public Task<Post> GetPost(int id);
        public Task<List<Post>> GetAllPosts();
        // update
        public void UpdatePost(Post post);
        // delete
        public Task RemovePost(Post post);


        public Task<bool> SaveChanges();
    }
}
