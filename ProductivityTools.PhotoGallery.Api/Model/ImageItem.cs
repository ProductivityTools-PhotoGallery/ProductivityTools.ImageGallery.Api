﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityTools.PhotoGallery.Api.Model
{
    public class ImageItem
    {
        public string src { get; set; }
        public List<string> srcSet { get; set; }

        public string sizes { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
