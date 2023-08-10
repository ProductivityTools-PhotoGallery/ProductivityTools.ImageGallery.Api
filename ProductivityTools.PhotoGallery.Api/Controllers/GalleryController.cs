using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductivityTools.PhotoGallery.Api.Model;
using ProductivityTools.PhotoGallery.PhotoProcessing;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

namespace ProductivityTools.PhotoGallery.Api.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GalleryController : BaseController
    {
        private string ApiAddress = @"https://localhost:5001/api/";

        //private string BasePath = @"d:\PhotoGallery\";


        public GalleryController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        [Route("List")]
        public List<GalleryItem> List(int height)
        {
            var result = new List<GalleryItem>();
            string[] directories = Directory.GetDirectories(OriginalPhotoBasePath);
            foreach (string file in directories)
            {
                result.Add(new GalleryItem { Name = file.Replace(OriginalPhotoBasePath, "") });
            }
            return result;
        }
        [HttpGet]
        [Route("Get")]
        public List<ImageItem> Get([FromQuery(Name = "Name")] string name,
            [FromQuery(Name = "Height")] int height)
        {
            int thumbnailsize = 100;
            var result = new List<ImageItem>();
            var directory = Path.Join(OriginalPhotoBasePath, name);
            ValidateThumbNails(directory, thumbnailsize);
            string[] files = Directory.GetFiles(directory, "*jpg");

            foreach (string file in files)
            {

                Bitmap img = new Bitmap(file);

                var imageHeight = img.Height;
                var imageWidth = img.Width;

                string imagePath = $"{ApiAddress}Images/Image1?gallery={name}&name={Path.GetFileName(file)}";
                string imagePathThumbnail = $"{ApiAddress}Images/Image1?gallery={name}&name={Path.GetFileName(file)}&height={thumbnailsize}";
                result.Add(new ImageItem { src = imagePath, Width = imageWidth, Height = imageHeight, Thumbnail = imagePathThumbnail });
            }
            return result;
        }

        private void ResizePhotograph(string sourceFile, string destinationFile, int targetSize)
        {
            new PhotoProcessingService().ConvertImage(sourceFile, destinationFile, targetSize);
        }

        private bool ValidateThumbNails(string path, int size)
        {
            var thumbNailDirectory = path.Replace(OriginalPhotoBasePath, ThumbnailsPhotoBasePath);
            thumbNailDirectory = Path.Join(thumbNailDirectory, size.ToString());
            bool exists = Directory.Exists(thumbNailDirectory);
            if (!exists)
            {
                Directory.CreateDirectory(thumbNailDirectory);
            }
            string[] files = Directory.GetFiles(path, "*jpg");
            foreach (var file in files)
            {
                var pathFileName = Path.GetFullPath(file);
                var thumbNailFileName = pathFileName.Replace(OriginalPhotoBasePath, ThumbnailsPhotoBasePath);

                if (System.IO.File.Exists(thumbNailFileName) == false)
                {
                    ResizePhotograph(pathFileName, thumbNailFileName, size);
                }
            }
            return false;
        }
    }
}
