using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public List<ImageItem> List(int height)
        {
            var result = new List<ImageItem>();
            string[] files = Directory.GetFiles(BasePath, "*jpg");
            foreach (string file in files)
            {
                string imagePath = $"{ApiAddress}Images/Image3?name={Path.GetFileName(file)}&height={height}";
                string imagePathThumbnail = $"{ApiAddress}Images/Image3?name={Path.GetFileName(file)}&height=20";
                result.Add(new ImageItem { Original = imagePath, Thumbnail = imagePathThumbnail });
            }
            return result;
        }

        public Image GetReducedImage(int height, Stream resourceImage)
        {
            try
            {
                Image image = Image.FromStream(resourceImage);
                var newWidth = image.Width * height / image.Height;
                Image thumb = image.GetThumbnailImage(newWidth, height, () => false, IntPtr.Zero);

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
        public IActionResult Get2(string name, int height)
        {
            string path = Path.Join(BasePath, name);
            FileStream file = new FileStream(path, FileMode.Open);
            Image newImage = GetReducedImage(height, file);
            MemoryStream s = new MemoryStream();
            newImage.Save(s, ImageFormat.Jpeg);
            file.Close();

            return File(s.ToArray(), "image/jpg");
        }

        public static Image ResizeImage(Image srcImage, int height)
        {
            var b = new Bitmap(srcImage.Width * height / srcImage.Height, height);
            using (var g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(srcImage, 0, 0, b.Width, b.Height);
            }
            return b;
        }

        //https://localhost:5001/api/Images/Image2?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image3")]
        public IActionResult Get3(string name, int height)
        {
            string path = Path.Join(BasePath, name);
            var image = Image.FromFile(path);
            var newImage = ResizeImage(image, height);

            // FileStream file = new FileStream(path, FileMode.Open);
            // Image newImage = GetReducedImage(height, file);
            MemoryStream s = new MemoryStream();
            newImage.Save(s, ImageFormat.Jpeg);

            return File(s.ToArray(), "image/jpg");
        }
    }
}