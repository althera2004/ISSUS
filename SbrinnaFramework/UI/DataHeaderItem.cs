// --------------------------------
// <copyright file="DataHeaderItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary></summary>
    public class UIDataHeaderItem
    {
        public string Text { get; set; }
        public bool Sortable { get; set; }
        public bool Filterable { get; set; }
        public bool HiddenMobile { get; set; }
        public string Class { get; set; }
        public int Width { get; set; }
        public string Id { get; set; }
        public string HeaderId { get; set; }
        public string DataId { get; set; }

        public string Render
        {
            get
            {
                string cssClass = this.Class;
                if (this.Filterable)
                {
                    if (!string.IsNullOrEmpty(cssClass))
                    {
                        cssClass += " ";
                    }

                    cssClass += "search";
                }

                if (this.HiddenMobile)
                {
                    if (!string.IsNullOrEmpty(cssClass))
                    {
                        cssClass += " ";
                    }

                    cssClass += "hidden-480";
                }

                string style = string.Empty;

                string sortAction = string.Empty;
                if(this.Sortable)
                {
                    sortAction = string.Format(CultureInfo.InvariantCulture, "onclick=\"Sort(this,'{0}');\" ", this.DataId);
                    style = "cursor:pointer;";
                    if (!string.IsNullOrEmpty(cssClass))
                    {
                        cssClass += " ";
                    }

                    cssClass += "sort";
                }

                if(this.Width>0)
                {
                    style+= string.Format(CultureInfo.InvariantCulture,"width:{0}px;", this.Width);
                }

                string pattern = @"<th {1}id=""{2}"" class=""{3}"" style=""{4}"">{0}</th>";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Text,
                    sortAction,
                    this.Id,
                    cssClass,
                    style);
            }
        }
    }
}