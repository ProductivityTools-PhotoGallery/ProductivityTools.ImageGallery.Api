using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ProductivityTools.PhotoGallery.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IConfiguration Configuration;

        protected string OriginalPhotoBasePath
        {
            get
            {
                var r = this.Configuration["OriginalPhotoBasePath"];
                return r;
            }
        }

        protected string ThumbnailsPhotoBasePath
        {
            get
            {
                var r = this.Configuration["ThumbnailsPhotoBasePath"];
                return r;
            }
        }

        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
