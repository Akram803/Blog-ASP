using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.FileMangers
{
    public interface IFileManager
    {
        public Task<string> ImageSave(IFormFile image);
        public FileStream ImageStream(string name);
        public void ImageDelete(string name);
    }
}
