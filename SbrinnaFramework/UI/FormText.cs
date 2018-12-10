// --------------------------------
// <copyright file="FormText.cs" company="OpenFramework">
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
    public class FormText
    {
        public int ColumnSpanLabel { get; set; }
        public string Label { get; set; }
        public int ColumnSpan { get; set; }
        public string Name { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public bool Required { get; set; }
        public bool Duplicated { get; set; }
        public int MaximumLength { get; set; }
        public bool ReadOnly { get; set; }
        public bool? GrantToWrite { get; set; }
        public string RequiredMessage { get; set; }
        public string DuplicatedMessage { get; set; }
        public bool Numeric { get; set; }
        public string ToolTip { get; set; }
        public bool IsInteger { get; set; }
        public bool RightAlign { get; set; }

        public virtual string Render
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    string requiredMark = this.Required ? "<span style=\"color:#f00\">*</span>" : string.Empty;
                    label = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"<label id=""{2}Label"" class=""col-sm-{0}{4}"">{1}{3}</label>",
                        this.ColumnSpanLabel,
                        this.Label,
                        this.Name,
                        requiredMark,
                        this.RightAlign ? " control-label no-padding-right" : string.Empty);
                }

                if (this.Value == null)
                {
                    this.Value = string.Empty;
                }

                string requiredLabel = string.Empty;
                if (this.Required)
                {
                    requiredLabel = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span class=""ErrorMessage"" id=""{0}ErrorRequired"" style=""display:none;"">{1}</span>", this.Name, this.RequiredMessage);
                }

                string duplicatedLabel = string.Empty;
                if (this.Duplicated)
                {
                    duplicatedLabel = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span class=""ErrorMessage"" id=""{0}ErrorDuplicated"" style=""display:none;"">{1}</span>", this.Name, this.DuplicatedMessage);
                }

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{7}
                        <div class=""col-sm-{2}"">                                                                                                                            
                            <input type=""text""{10} id=""{0}"" placeholder=""{3}"" class=""col-xs-12 col-sm-12 tooltip-info{8}"" value={9}{1}{9} {4} onblur=""this.value=$.trim(this.value);"" />
                            {5}
                            {6}                                                           
                        </div>	",
                    this.Name,
                    this.Value,
                    this.ColumnSpan,
                    this.Placeholder,
                    this.MaximumLength > 0 ? string.Format(CultureInfo.GetCultureInfo("en-us"), @" maxlength=""{0}""", this.MaximumLength) : string.Empty,
                    requiredLabel,
                    duplicatedLabel,
                    label,
                    this.Numeric ? " money-bank" : string.Empty,
                    this.Value.IndexOf('"') != -1 ? "'" : "\"",
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " readonly=\"readonly\"" : string.Empty);
            }
        }
    }
}
