// --------------------------------
// <copyright file="ImageSelector.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Web;

    /// <summary>
    /// Implements ImageSelector class
    /// </summary>
    public class ImageSelector
    {
        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Label { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static string SizeJson(string imageName, int width, int height)
        {
            string path = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
            if (!path.EndsWith("\\", StringComparison.Ordinal))
            {
                path = string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", path, imageName);
            }
            else
            {
                path = string.Format(CultureInfo.CurrentCulture, @"{0}{1}", path, imageName);
            }

            Size size = ImageHelper.GetDimensions(path);
            decimal fileWidth = Convert.ToDecimal(size.Width);
            decimal fileHeight = Convert.ToDecimal(size.Height);
            decimal fileRatio = fileWidth / fileHeight;
            decimal imageWidth = Convert.ToDecimal(width);
            decimal imageHeight = Convert.ToDecimal(height);

            if (imageWidth > imageHeight)
            {
                if (imageWidth > fileWidth) { fileWidth = imageWidth; }
                else { imageWidth = fileWidth; }
            }
            else
            {
                if (imageHeight > fileHeight) { }
            }

            if (fileRatio > 0)
            {
                imageHeight = imageHeight / fileRatio;
            }
            else
            {
                imageWidth = imageWidth / fileRatio;
            }

            width = Convert.ToInt32(imageWidth);
            height = Convert.ToInt32(imageHeight);

            if (height > 30)
            {
                width = Convert.ToInt32((decimal)(width * 30) / (decimal)height);
                height = 30;
            }

            return string.Format(
                CultureInfo.CurrentCulture,
                @"{{""Width"":""{0}"",""Height"":""{1}""}}",
                width > 0 ? string.Format("{0}px", width) : "100%",
                height > 0 ? string.Format("{0}px", height) : "100%");
        }

        public string Render
        {
            get
            {
                string path = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
                if (!path.EndsWith("\\", StringComparison.Ordinal))
                {
                    path = string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", path, this.ImageName);
                }
                else
                {
                    path = string.Format(CultureInfo.CurrentCulture, @"{0}{1}", path, this.ImageName);
                }

                if (File.Exists(path))
                {

                    Size size = ImageHelper.GetDimensions(path);
                    decimal fileWidth = Convert.ToDecimal(size.Width);
                    decimal fileHeight = Convert.ToDecimal(size.Height);
                    decimal fileRatio = fileWidth / fileHeight;
                    decimal imageWidth = Convert.ToDecimal(this.Width);
                    decimal imageHeight = Convert.ToDecimal(this.Height);

                    if (imageWidth > imageHeight)
                    {
                        if (imageWidth > fileWidth) { fileWidth = imageWidth; }
                        else { imageWidth = fileWidth; }
                    }
                    else
                    {
                        if (imageHeight > fileHeight) { }
                    }

                    if (fileRatio > 0)
                    {
                        imageHeight = imageHeight / fileRatio;
                    }
                    else
                    {
                        imageWidth = imageWidth / fileRatio;
                    }

                    this.Width = Convert.ToInt32(imageWidth);
                    this.Height = Convert.ToInt32(imageHeight);
                }

                return string.Format(
                    CultureInfo.CurrentCulture,
                    @"<div class=""col-sm-12"">
                                <img id=""{0}Img"" src=""{1}?ac={5}"" alt="""" style=""width:{2};height:{3};"" class=""ItemImage"" />
                            </div>
                            <div class=""col-sm-12"" style=""margin-top:8px;"">
                                <button class=""btn btn-success"" type=""button"" id=""Btn{0}ChangeImage"">
                                    <i class=""icon-refresh bigger-110""></i>
                                    {4}
                                </button>
                                <!--<button class=""col-sm-6 btn btn-danger"" type=""button"" id=""Btn{0}DeleteImage"">
                                    <i class=""icon-trash bigger-110""></i>-->
                                </button>
                            </div>",
                    this.Name,
                    this.ImageName,
                    this.Width > 0 ? string.Format("{0}px", this.Width) : "100%",
                    this.Height > 0 ? string.Format("{0}px", this.Height) : "100%",
                    this.Label,
                    Guid.NewGuid());
            }
        }
    }
}