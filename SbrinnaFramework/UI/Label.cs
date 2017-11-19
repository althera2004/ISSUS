// --------------------------------
// <copyright file="Label.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>
    /// Implements Label class
    /// </summary>
    public class Label : Element
    {
        public bool Right { get; set; }

        public string Text { get; set; }

        public override string Html
        {
            get
            {
                if (this.Expand == 0)
                {
                    this.Expand = 1;
                }

                string pattern = "<label class=\"col-sm-{0} control-label{1}\" id=\"{2}\">{3}</label>";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Expand,
                    this.Right ? " no-padding-right" : string.Empty,
                    this.Id,
                    this.Text);
            }
        }
    }
}
