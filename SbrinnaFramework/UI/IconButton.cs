// --------------------------------
// <copyright file="IconButton.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>
    /// Implements IconButton class
    /// </summary>
    public class IconButton : Element
    {
        public string Title { get; set; }

        public string Icon { get; set; }

        public string Appearance { get; set; }

        public string Action { get; set; }

        public override string Html
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("es-es"),
                    @"<span title=""{0}"" class=""btn btn-xs btn-{1}"" onclick=""{2}"">
                    <i class=""icon-{3} bigger-120""></i>
                </span>",
                            this.Title,
                            this.Appearance,
                            this.Action,
                            this.Icon);
            }
        }
    }
}
