// --------------------------------
// <copyright file="FormTextArea.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormTextArea
    {
        public string Label { get; set; }
        public int Rows { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ColumnsSpan { get; set; }
        public int ColumnsSpanLabel { get; set; }
        public bool Embedded { get; set; }
        public string ToolTip { get; set; }
        public bool TitleLabel { get; set; }
        public bool? GrantToWrite { get; set; }
        public int MaxLength { get; set; }
        

        public string Render
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    if (this.TitleLabel)
                    {
                        label = string.Format(
                            CultureInfo.CurrentCulture, @"<h4 id=""{1}Label"">{0}</h4>", 
                            this.Label, 
                            this.Name);
                    }
                    else
                    {
                        label = string.Format(CultureInfo.CurrentCulture, @"<div class=""for-group""><label class=""col-sm-{1}"" id=""{2}Label"">{0}</label></div>", this.Label, this.ColumnsSpan > 0 ? this.ColumnsSpanLabel : 12, this.Name);
                    }
                }

                string maxLength = "250";
                if (this.MaxLength > 0)
                {
                    maxLength = MaxLength.ToString();
                }

                string pattern = @" {0}
                                   {5}
                                       <div class=""col-sm-{4}"">
                                           <textarea rows=""{1}""{7} class=""form-control col-xs-12 col-sm-12"" maxlength=""{8}"" id=""{2}"">{3}</textarea>
                                       </div>
                                   {6}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    label,
                    this.Rows > 0 ? this.Rows : 5,
                    this.Name,
                    this.Value,
                    this.ColumnsSpan > 0 ? this.ColumnsSpan : 12,
                    this.Embedded ? string.Empty : "<div class=\"form-group\">",
                    this.Embedded ? string.Empty : "</div>",
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " readonly=\"readonly\"" : string.Empty,
                    maxLength);
            }
        }
    }
}