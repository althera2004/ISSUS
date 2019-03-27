// --------------------------------
// <copyright file="InfoBox.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.Graph
{
    using System.Globalization;

    /// <summary></summary>
    public class InfoBox
    {
        public string Color { get; set; }

        public string Icon { get; set; }

        public string Data { get; set; }

        public string Label { get; set; }

        public bool ShowIncrement { get; set; }

        public string Increment { get; set; }

        public string Render
        {
            get
            {
                string increment = string.Empty;
                if (this.ShowIncrement)
                {
                    increment = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<div class=""stat stat-success"">{0}</div>", this.Increment);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<div class=""infobox infobox-{0}"">
                                            <div class=""infobox-icon"">
                                                <i class=""icon-{1}""></i>
                                            </div>
                                            <div class=""infobox-data"">
                                                <span class=""infobox-data-number"">{2}</span>
                                                <div class=""infobox-content"">{3}</div>
                                            </div>
                                            {4}
                                        </div>",
                                               this.Color,
                                               this.Icon,
                                               this.Data,
                                               this.Label,
                                               increment);
            }
        }
    }
}