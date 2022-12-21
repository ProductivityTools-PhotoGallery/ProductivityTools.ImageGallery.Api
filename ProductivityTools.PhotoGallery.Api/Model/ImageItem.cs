using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityTools.PhotoGallery.Api.Model
{
    public class ImageItem
    {
        public string Original { get; set; }
        public string Thumbnail { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
