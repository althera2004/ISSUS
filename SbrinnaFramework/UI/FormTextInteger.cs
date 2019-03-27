// --------------------------------
// <copyright file="FormTextInteger.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    public class FormTextInteger : FormText
    {
        public override string Render
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

                return string.Format(CultureInfo.GetCultureInfo("en-us"),
                    @"{7}
                        <div class=""col-sm-{2}"">                                                                                                                            
                            <input type=""text""{8} id=""{0}"" placeholder=""{3}"" class=""col-xs-12 col-sm-12 tooltip-info integerFormated"" value=""{1}"" {4} onblur=""this.value=$.trim(this.value);"" />
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
                             (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " readonly=\"readonly\"" : string.Empty);

            }
        }
    }
}