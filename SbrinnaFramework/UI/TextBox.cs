// --------------------------------
// <copyright file="TextBox.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;
    using System.Text;

    /// <summary>
    /// Implements TextBox class
    /// </summary>
    public class TextBox : Element
    {
        public string Placeholder { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public bool Required { get; set; }

        public int MinimumLength { get; set; }

        public int MaximumLength { get; set; }

        public string Label { get; set; }

        public bool RapidActionButton { get; set; }

        public bool ReadOnly { get; set; }

        public override string Html
        {
            get
            {
                if (this.Expand == 0)
                {
                    this.Expand = 1;
                }

                StringBuilder res = new StringBuilder();
                if (!string.IsNullOrEmpty(this.Label))
                {
                    res.Append(new Label() { Id = this.Id + "Label", Expand = 2, Text = this.Label, Right = true }.Html);
                }

                string maxLength = string.Empty;
                if (this.MaximumLength > 0)
                {
                    maxLength = string.Format(CultureInfo.CurrentCulture, " maxlength=\"{0}\"", this.MaximumLength);
                }

                string readOnly = string.Empty;
                if (this.ReadOnly)
                {
                    readOnly = " readonly=\"readonly\"";
                }

                res.Append("<div class=\"col-sm-").Append(this.Expand).Append("\">");
                res.Append("<input type=\"text\" id=\"").Append(this.Id).Append(" placeholder=\"").Append(this.Placeholder).Append("\" class=\"col-xs-12 col-sm-12\" value=\"").Append(this.Text).Append("\"").Append(maxLength).Append(readOnly).Append(" onblur=\"this.value = this.value.trim();\">");
                if (this.Required)
                {
                    res.Append("<span class=\"ErrorMessage\" id=\"").Append(this.Id).Append("ErrorRequired\" style=\"display:none;\">Common_Required</span>");
                }
                res.Append("</div>");

                if (this.RapidActionButton)
                {
                    res.Append(new RapidActionButton() { Id = this.Id.Replace("Txt", "Btn"), Expand = 1 }.Html);
                }

                return res.ToString();
            }
        }
    }
}
