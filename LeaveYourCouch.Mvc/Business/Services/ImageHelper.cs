using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace LeaveYourCouch.Mvc.Business.Services
{
    internal class ImageHelper: IImageHelper
    {
        /// <summary>
        /// Resize an image keeping its aspect ratio (cropping may occur).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
         Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }

        Dictionary<string, ImageFormat> _formats= new Dictionary<string, ImageFormat>
        {
            {"jpeg",ImageFormat.Jpeg},
            {"jpg",ImageFormat.Jpeg},
            {"png",ImageFormat.Png},
        };

        public byte[] ToRatioImageDisplay(string picturepath,int width=200, int height=200)
        {
            byte[] result;

            string ext = Path.GetExtension(picturepath).ToLower().Replace(".", "");
            ImageFormat format = _formats[ext];
            using (var source = Image.FromFile(picturepath))
            {
                var resultRatio = ResizeImageKeepAspectRatio(source, width, height);
                using (var ms = new MemoryStream())
                {
                    resultRatio.Save(ms, format);
                    result = ms.ToArray();
                }
            }
            //var source = Image.FromFile(picturepath);
            


            
            return result;
        }

        public void SaveUploadedImage(HttpPostedFileBase profilePicutre, string imagePath)
        {
            profilePicutre.SaveAs(imagePath);
        }
    }

    public interface IImageHelper
    {
        byte[] ToRatioImageDisplay(string picturepath,int width=200, int height=200);
        void SaveUploadedImage(HttpPostedFileBase viewmodelProfilePictureUpload, string imagePath);
    }
}