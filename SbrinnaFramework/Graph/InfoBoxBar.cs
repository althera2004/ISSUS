// --------------------------------
// <copyright file="InfoBoxBar.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.Graph
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;

    /// <summary></summary>
    public class InfoBoxBar
    {
        private List<int> data;

        public string Color { get; set; }

        public string Label { get; set; }

        public string Render
        {
            get
            {
                int total = 0;
                string dataList = string.Empty;
                bool first = true;
                foreach (int dataItem in this.Data)
                {
                    total += dataItem;
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        dataList += ",";
                    }

                    dataList += dataItem.ToString();
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<div class=""infobox infobox-{0}"">
                                            <div class=""infobox-chart"">
                                                <span class=""sparkline"" data-values=""{1}""><canvas width=""44"" height=""33"" style=""display: inline-block; width: 44px; height: 33px; vertical-align: top;""></canvas></span>
                                            </div>
                                            <div class=""infobox-data"">
                                                <span class=""infobox-data-number"">{2}</span>
                                                <div class=""infobox-content"">{3}</div>
                                            </div>
                                        </div>",
                    this.Color,
                    dataList,
                    total,
                    this.Label);
            }
        }

        public ReadOnlyCollection<int> Data
        {
            get
            {
                if (this.data == null)
                {
                    this.data = new List<int>();
                }

                return new ReadOnlyCollection<int>(this.data);
            }
        }

        public void AddData(int value)
        {
            if (this.data == null)
            {
                this.data = new List<int>();
            }

            this.data.Add(value);
        }
    }
}