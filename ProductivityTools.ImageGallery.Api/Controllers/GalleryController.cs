using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductivityTools.ImageGallery.Api.Model;
using System.Collections.Generic;
using System.IO;

namespace ProductivityTools.ImageGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private string BasePath = @"d:\Trash\Images\";

        [HttpGet]
        [Route("List")]
        public List<GalleryItem> List(int height)
        {
            var result = new List<GalleryItem>();
            string[] directories = Directory.GetDirectories(BasePath);
            foreach (string file in directories)
            {
                result.Add(new GalleryItem { Name = file.Replace(BasePath, "") });
            }
            return result;
        }
    }
}
