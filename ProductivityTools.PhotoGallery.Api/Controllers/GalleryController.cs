using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductivityTools.PhotoGallery.Api.Model;
using ProductivityTools.PhotoGallery.PhotoProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
            Console.WriteLine("XXXXXXXXXXXXX -Start-XXXXXXXXXXXXXX");
            List<int> thumbNailSizes = new List<int> { 500, 800, 1024, 1600 };
           
            var result = new List<ImageItem>();
            var directory = Path.Join(OriginalPhotoBasePath, name);
            thumbNailSizes.ForEach(x => ValidateThumbNails(directory, x));
            string[] files = Directory.GetFiles(directory, "*jpg");

            Func<string, int, string> getPath = (file, size) => $"{ApiAddress}Images/Image1?gallery={name}&height={size}&name={Path.GetFileName(file)}";

            foreach (string file in files)
            {

                Bitmap img = new Bitmap(file);

                var imageHeight = img.Height;
                var imageWidth = img.Width;

                string imagePath = getPath(file, thumbNailSizes[0]);
                List<string> srcSet = thumbNailSizes.Select(x => string.Format($"{getPath(file, x)} {x}w")).ToList();
                List<string> sizes = new List<string> { "(min-width: 480px) 50vw,(min-width: 1024px) 33.3vw,100vw" };
                result.Add(new ImageItem {
                    src = imagePath,
                    Width = imageWidth,
                    Height = imageHeight,
                    srcSet = srcSet,
                    sizes = sizes
                }); ;
            }
            return result;
            Console.WriteLine("XXXXXXXXXXXXX -End-XXXXXXXXXXXXXX");
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
                var directory=Path.GetDirectoryName(thumbNailFileName);
                var fileName=Path.GetFileName(thumbNailFileName);
                var thumbNailFileNameWithSize = Path.Join(directory, size.ToString(), fileName);
                if (System.IO.File.Exists(thumbNailFileNameWithSize) == false)
                {
                    ResizePhotograph(pathFileName, thumbNailFileNameWithSize, size);
                }
            }
            return false;
        }
    }
}
