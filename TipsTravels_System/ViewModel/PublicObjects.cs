using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TipsTravels_System.ViewModel
{
    public static class PublicObjects
    {
        public static byte[] Image { get; set; }
        public static byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}