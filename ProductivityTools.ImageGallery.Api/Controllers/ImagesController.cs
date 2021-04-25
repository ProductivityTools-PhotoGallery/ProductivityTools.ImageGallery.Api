using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
    public class ImagesController : ControllerBase
    {
        private string BasePath = @"d:\Photographs\Processed\zdjeciaDone\2009.12.30 Sylwester\";
        private string ApiAddress = @"https://localhost:5001/api/";

        //https://localhost:5001/api/Images/List
        [HttpGet]
        [Route("List")]
        public List<ImageItem> List()
        {
            var result = new List<ImageItem>();
            string[] files = Directory.GetFiles(BasePath);
            foreach (string file in files)
            {
                string imagePath = $"{ApiAddress}Images/Image?name={Path.GetFileName(file)}";
                result.Add(new ImageItem { Original = imagePath, Thumbnail = imagePath });
            }
            return result;
        }

        public Image GetReducedImage(int width, int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                
                return thumb;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        //https://localhost:5001/api/Images/Image?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image")]
        public IActionResult Get(string name)
        {
            string path = Path.Join(BasePath, name);
            //PhysicalFileResult result = PhysicalFile(path, "image/jpg");
            FileStream file = new FileStream(path, FileMode.Open);
            PhysicalFileResult result = PhysicalFile(path, "image/jpg");
            return result;
        }

        //https://localhost:5001/api/Images/Image2?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image2")]
        public IActionResult Get2(string name, int size)
        {
            string path = Path.Join(BasePath, name);
            //PhysicalFileResult result = PhysicalFile(path, "image/jpg");
            FileStream file = new FileStream(path, FileMode.Open);
            
            //Stream stream = ProductImage.OpenReadStream();

            Image newImage = GetReducedImage(32, 32, file);
            MemoryStream s = new MemoryStream();
            newImage.Save(s, ImageFormat.Jpeg);
            file.Close();

            return File(s.ToArray(), "image/jpg");
        }
    }
}