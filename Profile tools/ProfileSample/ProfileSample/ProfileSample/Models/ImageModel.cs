﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileSample.Models
{
    public class ImageModel
    {
        public string Name { get; set; }

        public byte[] Data { get; set; } 
    }
}