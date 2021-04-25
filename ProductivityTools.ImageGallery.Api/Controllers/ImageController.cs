using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductivityTools.ImageGallery.Api.Model;

namespace ProductivityTools.ImageGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private string BasePath = @"d:\.PawelPC\Photographs\Processed\zdjeciaDone\2009.12.30 Sylwester\";

        [HttpGet]
        [Route("List")]
        public List<ImageItem> List()
        {
            var result = new List<ImageItem>();
            string[] files = Directory.GetFiles(BasePath);
            foreach (string file in files)
            {
                result.Add(new ImageItem { Original = file, Thumbnail = file });
            }
            return result;
        }

        [HttpGet]
        [Route("Image")]
        public IActionResult Get(string name)
        {
            string path = Path.Join(BasePath, name);
            PhysicalFileResult result = PhysicalFile(path, "image/jpg");
            return result;
        }
    }
}