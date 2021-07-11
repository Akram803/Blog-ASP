using Blog.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }


        [Required]
        public int CatrgoryId{ get; set; }

        public Category Category { get; set; }

        public string ImageName { get; set; } = "";
        public IFormFile ImageObj { get; set; }
    }
}
