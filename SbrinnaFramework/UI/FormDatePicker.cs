// --------------------------------
// <copyright file="FormDatePicker.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>Implements FormDatePicker control</summary>
    public class FormDatePicker
    {
        /// <summary>Gets or sets control identifier</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets the number of columns for control</summary>
        public int ColumnsSpan { get; set; }

        /// <summary>Gets or sets the number of columns for label control</summary>
        public int ColumnsSpanLabel { get; set; }

        /// <summary>Gets or sets text of label control</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets value to show</summary>
        public DateTime? Value { get; set; }

        /// <summary>Gets or sets a value indicating whether user grants</summary>
        public bool? GrantToWrite { get; set; }

        /// <summary>Gets or sets a value indicating whether if field is required</summary>
        public bool Required { get; set; }

        /// <summary>Gets HTML code of control</summary>
        public string Render
        {
            get
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string labelSpan = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    labelSpan = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<label id=""{1}Label"" class=""col-sm-{2} control-label no-padding-right"">{0}</label>",
                        this.Label,
                        this.Id,
                        this.ColumnsSpanLabel);
                }

                string malformedLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}DateMalformed"" style=""display:none;"">{1}</span>", this.Id, dictionary["Common_Error_DateMalformed"]);
                string requiredLabel = string.Format(CultureInfo.InvariantCulture, @"<span class=""ErrorMessage"" id=""{0}DateRequired"" style=""display:none;"">{1}</span>", this.Id, dictionary["Common_Required"]);

                string pattern = @"{0}
                      <div class=""col-sm-{1}"">
                        <div class=""row"">
                            <div class=""col-xs-12 col-sm-12 tooltip-info"" id=""{2}Div"">
                                <div class=""input-group"">
                                    <input{5} class=""form-control date-picker"" id=""{2}"" type=""text"" data-date-format=""dd/mm/yyyy"" maxlength=""10"" value=""{3}"">
                                    <span{6} id=""{2}Btn"" class=""input-group-addon"" onclick=""document.getElementById('{2}').focus();"">
                                        <i class=""icon-calendar bigger-110""></i>
                                    </span>
                                </div>
                                {4}
                                {7}
                            </div>
                        </div>
                    </div>";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    labelSpan,
                    this.ColumnsSpan,
                    this.Id,
                    this.Value.HasValue ? string.Format("{0:dd/MM/yyyy}", this.Value.Value) : string.Empty,
                    malformedLabel,
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " readonly=\"readonly\"" : string.Empty,
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " style=\"display:none;\"" : string.Empty,
                    this.Required ? requiredLabel : string.Empty);
            }
        }
    }
}