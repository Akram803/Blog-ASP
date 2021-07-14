using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Comment
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now ;

        public string UserId { get; set; }
        public AppUser User { get; set; }

    }
}
