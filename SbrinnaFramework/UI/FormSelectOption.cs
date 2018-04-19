// --------------------------------
// <copyright file="FormSelectOption.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormSelectOption
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }

        public static FormSelectOption DefaultOption(Dictionary<string, string> dictionary)
        {
            return new FormSelectOption
            {
                Text = dictionary["Common_SelectAll"],
                Value = "0"
            };
        }

        public string Render
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "<option{0}{2}>{1}</option>",
                    string.IsNullOrEmpty(this.Value) ? string.Empty : string.Format(@" value=""{0}""", this.Value),
                    this.Text,
                    this.Selected ? " selected=\"selected\"" : string.Empty
                    );
            }
        }
    }
}
