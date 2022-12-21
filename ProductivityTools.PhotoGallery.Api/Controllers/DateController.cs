using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ProductivityTools.PhotoGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateController : ControllerBase
    {
        //https://localhost:5001/api/Images/List
        [HttpGet]
        [Route("Get")]
        public string Get()
        {
            return DateTime.Now.ToString();
        }
    }
}
