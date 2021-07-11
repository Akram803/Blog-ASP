using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Models.Comment
{
    public class SubComment : Comment
    {
        [Required]
        public int MainCommentId { get; set; }
    }
}
