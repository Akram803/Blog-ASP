using Blog.Models.Comment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        [DisplayName("Title")]
        [Required]
        public string Title { get; set; } = "";
        [DisplayName("what do you think")] 
        [Required]
        public string Body { get; set; } = "";
        public string Image { get; set; } = "";

        public string Tags { get; set; } = "";
        public string Description { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now ;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        public List<MainComment> MainComments { get; set; }
    } 
}
