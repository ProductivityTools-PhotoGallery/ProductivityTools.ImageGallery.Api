using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductivityTools.PhotoGallery.Api.Model;
using ProductivityTools.PhotoGallery.CoreObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
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

        //replace from core objects
        private const string MetadataName = ".photo.json";
        private string GetPhotoMetadataPath(string directory)
        {
            var photoMetadataPath = Path.Join(directory, MetadataName);
            return photoMetadataPath;
        }

        [HttpGet]
        [Route("Get")]
        public List<ImageItem> Get([FromQuery(Name = "Name")] string name)
        {

            Func<string, int, string> getPath = (file, size) => $"{ApiAddress}Images/Image1?gallery={name.Replace(" ", "%20")}&width={size}&name={Path.GetFileName(file)}";

            DateTime now = DateTime.Now;
            Console.WriteLine("XXXXXXXXXXXXX -Start-XXXXXXXXXXXXXX");

            var galleryDirectory = Path.Join(OriginalPhotoBasePath, name);
            var photoMetadataPath = GetPhotoMetadataPath(galleryDirectory);
            var result = new List<ImageItem>();

            if (System.IO.File.Exists(photoMetadataPath))
            {
                var joson = System.IO.File.ReadAllText(photoMetadataPath);
                var gallery = JsonSerializer.Deserialize<Gallery>(joson);

                foreach (var photo in gallery.ImageList)
                {
                    string imagePath = getPath(photo.Name, gallery.ImageSizes[3]);
                    List<ImageItem> srcSet = gallery.ImageSizes.Select(x =>
                    {
                        var ratio = photo.Width / x;
                        var result = new ImageItem() { Height = photo.Height / ratio, Width = x, src = string.Format($"{getPath(photo.Name, x)}") };
                        return result;
                    }
                    ).ToList();
                    List<string> sizes = new List<string> { "10vw" };
                    result.Add(new ImageItem
                    {
                        src = imagePath,
                        Width = photo.Width,
                        Height = photo.Height,
                        srcSet = srcSet,
                        //  sizes = sizes
                    }); ;
                }


            }
            else
            {
                throw new Exception("not processed");
            }


            //List<int> thumbNailSizes = new List<int> { 500, 800, 1024, 1600 };

            //var directory = Path.Join(OriginalPhotoBasePath, name);
            //string[] files = Directory.GetFiles(directory, "*jpg");



            //foreach (string file in files)
            //{

            //    //Bitmap img = new Bitmap(file);

            //    //var imageHeight = img.Height;
            //    //var imageWidth = img.Width;

            //    string imagePath = getPath(file, thumbNailSizes[0]);
            //    List<string> srcSet = thumbNailSizes.Select(x => string.Format($"{getPath(file, x)} {x}w")).ToList();
            //    List<string> sizes = new List<string> { "(min-width: 480px) 50vw,(min-width: 1024px) 33.3vw,100vw" };
            //    result.Add(new ImageItem
            //    {
            //        src = imagePath,
            //        //Width = imageWidth,
            //        //Height = imageHeight,
            //        srcSet = srcSet,
            //        sizes = sizes
            //    }); ;
            //}
            Console.WriteLine(DateTime.Now - now);

            Console.WriteLine("XXXXXXXXXXXXX -End-XXXXXXXXXXXXXX");
            return result;
        }
    }
}
