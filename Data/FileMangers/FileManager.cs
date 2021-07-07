using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.FileMangers
{
    public class FileManager : IFileManager
    {
        private string _path;

        public FileManager(IConfiguration config)
        {
            // get path and make it compatable with current system
            _path = Path.Combine( config["Paths:Images"] );

            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }

        public async Task<string> ImageSave(IFormFile image)
        {
            string imageName = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss");
            string imageType = image.FileName.Substring(
                                        image.FileName.LastIndexOf(".")
                                        );
            string imageFullPath = Path.Combine(_path, $"{imageName}{imageType}");

            using(var fileStrem = new FileStream(imageFullPath, FileMode.Create))
            {
                await image.CopyToAsync(fileStrem);
            }
            return $"{imageName}{imageType}";
        }


        public FileStream ImageStream(string name)
        {
            var imageFullPath = Path.Combine(_path, name);

            return new FileStream(imageFullPath, FileMode.Open, FileAccess.Read);
        }

        public void ImageDelete(string name)
        {
            var imageFullPath = Path.Combine(_path, name);

            File.Delete(imageFullPath);
        }
    }
}
