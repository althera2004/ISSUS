// --------------------------------
// <copyright file="ApplicationItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>Implements ApplicationItem</summary>
    public class ApplicationItem
    {
        /// <summary>Gets or sets the application item identifier</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the description of application item</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets the icon of application item</summary>
        public string Icon { get; set; }

        /// <summary>Gets or sets the url of the application item</summary>
        public Uri Url { get; set; }

        /// <summary>Gets or sets a value indicating whether if item is container</summary>
        public bool Container { get; set; }

        /// <summary>Gets or sets the parent item of application item</summary>
        public int Parent { get; set; }

        /// <summary>Gets or sets the grants of users for an application item</summary>
        public UserGrant UserGrant { get; set; }

        /// <summary>Render the HTML code for a menu option of application item</summary>
        /// <returns>HTML code for a menu option of application item</returns>
        public string Render()
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            bool selected = false;
            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToUpperInvariant();

            if (this.Url != null)
            {
                selected = this.Url.AbsoluteUri.ToUpperInvariant() == actualUrl;
            }

            string pattern = @"<li{3} id=""menuoption-{4}"">
                                    <a href=""{1}"">
                                        <i class=""{2}""></i>
                                        {0}
                                    </a>
                                </li>";

            return string.Format(
                CultureInfo.InvariantCulture, 
                pattern,
                dictionary[this.Description],
                this.Url,
                this.Icon,
                selected ? " class=\"active\"" : string.Empty,
                this.Id);
        }
    }
}