using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductivityTools.ImageGallery.Api.Model;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ProductivityTools.ImageGallery.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : ControllerBase
    {
        private string ApiAddress = @"https://localhost:5001/api/";

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
        [HttpGet]
        [Route("Get")]
        public List<ImageItem> Get([FromQuery(Name = "Name")] string name,
            [FromQuery(Name = "Height")] int height)
        {
            var result = new List<ImageItem>();
            string[] files = Directory.GetFiles(Path.Join(BasePath, name), "*jpg");
            foreach (string file in files)
            {

                Bitmap img = new Bitmap(file);

                var imageHeight = img.Height;
                var imageWidth = img.Width;

                string imagePath = $"{ApiAddress}Images/Image3?gallery={name}&name={Path.GetFileName(file)}&height={height}";
                string imagePathThumbnail = $"{ApiAddress}Images/Image2?gallery={name}&name={Path.GetFileName(file)}&height=100";
                result.Add(new ImageItem { Original = imagePath, Width = imageWidth, Height = imageHeight, Thumbnail = imagePathThumbnail });
            }
            return result;
        }
    }
}
