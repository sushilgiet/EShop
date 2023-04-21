﻿using System.IO;
using System;

namespace EShop.UI.Models
{
    public class BlobInformation
    {
        public Uri? BlobUri { get; set; }
        public string BlobName
        {
            get
            {
                return BlobUri!.Segments[BlobUri.Segments.Length - 1];
            }
        }
        public string BlobNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(BlobName);
            }
        }
        public int Id { get; set; }
    }
}
