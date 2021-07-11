using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Comment
{
    public class MainComment : Comment
    {
        [Required]
        public int PostId { get; set; }
        public List<SubComment> SubComments { get; set; } 
    }
}
