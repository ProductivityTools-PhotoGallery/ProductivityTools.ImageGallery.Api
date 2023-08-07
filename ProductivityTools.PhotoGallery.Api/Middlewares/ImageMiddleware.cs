//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Primitives;
//using System.IO;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace ProductivityTools.PhotoGallery.Api.Middlewares
//{
//    public class ImageMiddleware
//    {
//        private readonly RequestDelegate next;
//        private string BasePath
//        {
//            get
//            {
//                var r = "D:\\PhotoGallery\\";
//                return r;
//            }
//        }
//        public ImageMiddleware(RequestDelegate next)
//        {
//            this.next = next;
//        }

//        public async Task<IActionResult> Invoke(HttpContext ctx)
//        {
//            StringValues name = string.Empty;
//            ctx.Request.Query.TryGetValue("name", out name);
//            string path = Path.Join(BasePath, name);
//            //PhysicalFileResult result = PhysicalFile(path, "image/jpg");
//            FileStream file = new FileStream(path, FileMode.Open);

//            return new File(file, "image/jpeg");
//        }
//    }
//}
