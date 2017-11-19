// --------------------------------
// <copyright file="AlertTag.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AlertTag
    {
        public long ItemId { get; set; }

        public string ItemDescription { get; set; }

        public string ItemTypeName { get; set; }

        public string AlertDescription { get; set; }

        public string Item { get; set; }

        public string Render(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string pattern = @"<li>
                                    <a href=""DepartmentView.aspx?id={0}"">
                                        <div class=""clearfix"">
                                            <span class=""pull-left"">
                                                <i class=""btn btn-xs no-hover btn-success icon-group""></i>{1}
                                            </span>
                                            <span class=""pull-right badge badge-info"">{2}</span>
                                        </div>
                                    </a>
                                </li>";
            return string.Format(CultureInfo.GetCultureInfo("en-us"), pattern, this.ItemId, this.ItemDescription, dictionary[this.AlertDescription]);
        }
    }
}
