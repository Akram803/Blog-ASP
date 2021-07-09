using Blog.Models.Comment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; } = "";
        [DisplayName("what do you think")]
        public string Body { get; set; } = "";
        public string Image { get; set; } = "";

        public string Tags { get; set; } = "";
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now ;

        public int CategoryId { get; set; }

        public List<MainComment> MainComments { get; set; }
    } 
}
