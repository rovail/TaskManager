﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManager.Common.Models;

namespace TaskManager.Client.Models.Extensions
{
    public static class CommonModelExtension
    {
        public static BitmapImage LoadImage(this CommonModel model)
        {
            if (model.Photo == null || model.Photo.Length == 0)
                return new BitmapImage();

            var image = new BitmapImage();
            using (var memSm = new MemoryStream(model.Photo))
            {
                memSm.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memSm;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
