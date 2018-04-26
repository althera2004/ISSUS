// --------------------------------
// <copyright file="LeftMenuOption.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    using System.Globalization;
    using System.Web;

    /// <summary>Implements a class of options of left menu</summary>
    public static class LeftMenuOption
    {
        /// <summary>Render a option in the left menu</summary>
        /// <param name="text">Label of option</param>
        /// <param name="link">Url of option</param>
        /// <param name="current">The current url of page</param>
        /// <param name="icon">Icon of option</param>
        /// <returns>The html code to render option in left menu</returns>
        public static string Render(string text, string link, bool current, string icon)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = string.Empty;
            }

            if (string.IsNullOrEmpty(link))
            {
                link = string.Empty;
            }

            if (string.IsNullOrEmpty(icon))
            {
                icon = string.Empty;
            }

            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            if (HttpContext.Current.Request.Url.LocalPath != "/")
            {
                string baseUrl = actualUrl.Replace(HttpContext.Current.Request.Url.LocalPath.Substring(1), string.Empty);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    actualUrl = "/" + actualUrl.Replace(baseUrl, string.Empty);
                }
            }
            
            current = link.ToUpperInvariant() == actualUrl.ToUpperInvariant();

            string currentText = current ? " class=\"active\"" : string.Empty;
            return string.Format(
                CultureInfo.InvariantCulture,
                @"<li {2}><a href=""{1}""><i class=""{3}""></i><span class=""menu-text""> {0} </span></a></li>",
                text,
                link,
                currentText,
                icon);
        }
    }
}