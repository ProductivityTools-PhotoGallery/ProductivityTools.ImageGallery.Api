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
using Microsoft.Extensions.Configuration;
using ProductivityTools.PhotoGallery.Api.Model;

namespace ProductivityTools.PhotoGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : BaseController
    {
        //private string BasePath = @"d:\Trash\Images\";
        private string ApiAddress = @"https://localhost:5001/api/";


        public ImagesController(IConfiguration configuration) : base(configuration)
        {
        }


        ////https://localhost:5001/api/Images/List
        //[HttpGet]
        //[Route("List")]
        //public List<ImageItem> List(int height)
        //{
        //    var result = new List<ImageItem>();
        //    string[] files = Directory.GetFiles(OriginalPhotoBasePath, "*jpg");
        //    foreach (string file in files)
        //    {
        //        string imagePath = $"{ApiAddress}Images/Image4?name={Path.GetFileName(file)}";
        //        string imagePathThumbnail = $"{ApiAddress}Images/Image4?name={Path.GetFileName(file)}";
        //        result.Add(new ImageItem { Original = imagePath, Thumbnail = imagePathThumbnail });
        //    }
        //    return result;
        //}

        //public Image GetReducedImage(int height, Stream resourceImage)
        //{
        //    try
        //    {
        //        Image image = Image.FromStream(resourceImage);
        //        var newWidth = image.Width * height / image.Height;
        //        Image thumb = image.GetThumbnailImage(newWidth, height, () => false, IntPtr.Zero);

        //        return thumb;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        //: 'The process cannot access the file 'D:\PhotoGallery\2017.03.12 Zell am Ziller Narty - Copy\2017.03.12_09.28.56.jpg' because it is being used by another process.'

        //https://localhost:5001/api/Images/Image?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image1")]
        public IActionResult Get(string gallery, string name, int? height)
        {

            //PhysicalFileResult result = PhysicalFile(path, "image/jpg");

            if (height.HasValue)
            {
                string path = Path.Join(ThumbnailsPhotoBasePath, height.Value.ToString(), gallery, name);
                FileStream file = new FileStream(path, FileMode.Open);
                PhysicalFileResult result = PhysicalFile(path, "image/jpg");
                return result;
            }
            else
            {
                string path = Path.Join(OriginalPhotoBasePath, gallery, name);
                FileStream file = new FileStream(path, FileMode.Open);
                PhysicalFileResult result = PhysicalFile(path, "image/jpg");
                return result;
            }
        }

        //jak rakieta
        //https://localhost:5001/api/Images/Image2?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image2")]
        public IActionResult Get2(string gallery, string name)
        {
            string path = Path.Join(OriginalPhotoBasePath, gallery, name);
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            Image newImage = Image.FromStream(file);
            MemoryStream s = new MemoryStream();
            newImage.Save(s, ImageFormat.Jpeg);
            file.Close();

            return File(s.ToArray(), "image/jpg");
        }

        //private static object o = new object();

        //public static Image ResizeImage(Image srcImage, int height)
        //{
        //    var start = DateTime.Now;

        //    var b = new Bitmap(srcImage.Width * height / srcImage.Height, height);
        //    lock (o)
        //    {
        //        using (var g = Graphics.FromImage((Image)b))
        //        {
        //            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //            g.DrawImage(srcImage, 0, 0, b.Width, b.Height);
        //        }
        //    }
        //    var x = DateTime.Now.Subtract(start);
        //    Console.WriteLine($"{x}");
        //    return b;

        //}

        //jak rakieta
        //https://localhost:5001/api/Images/Image2?name=IMGP0001.JPG
        [HttpGet]
        [Route("Image3")]
        public async Task<IActionResult> Get3(string gallery, string name)
        {

            string path = Path.Join(OriginalPhotoBasePath, gallery, name);
            byte[] result;

            using (FileStream SourceStream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                result = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            }
            return File(result.ToArray(), "image/jpg");

            //MemoryStream s = new MemoryStream();
            /////var image = await Image.FromStream(result);
            //lock (o)
            //{
            //    MemoryStream ms = new MemoryStream(result, 0, result.Length);
            //    ms.Position = 0; // this is important
            //    var returnImage = Image.FromStream(ms, true);
            //    var newImage = ResizeImage(returnImage, height);

            //    // FileStream file = new FileStream(path, FileMode.Open);
            //    // Image newImage = GetReducedImage(height, file);

            //    newImage.Save(s, ImageFormat.Jpeg);
            //}
            //return File(s.ToArray(), "image/jpg");
        }

        [Route("Image4")]
        public IActionResult Image3(string gallery, string name)
        {
            string filename = Path.Join(OriginalPhotoBasePath, gallery, name);

            string ext = System.IO.Path.GetExtension(filename).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            //get the mimetype of the file
            string mimeType = regKey.GetValue("Content Type").ToString();

            return PhysicalFile(filename, mimeType);

        }

    }
}