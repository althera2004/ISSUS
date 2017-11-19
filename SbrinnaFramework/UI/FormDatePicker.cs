// --------------------------------
// <copyright file="FormDatePicker.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Implements FormDatePicker control
    /// </summary>
    public class FormDatePicker
    {
        public string Id { get; set; }
        public int ColumnsSpan { get; set; }
        public int ColumnsSpanLabel { get; set; }
        public string Label { get; set; }
        public DateTime? Value { get; set; }
        public bool? GrantToWrite { get; set; }

        public string Render
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string labelSpan = string.Empty;
                if (!string.IsNullOrEmpty(this.Label))
                {
                    labelSpan = string.Format(
                        CultureInfo.InvariantCulture,
                        @"<label id=""{1}Label"" class=""col-sm-{2}"">{0}</label>",
                        this.Label,
                        this.Id,
                        this.ColumnsSpanLabel);
                }

                string malformedLabel = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span class=""ErrorMessage"" id=""{0}DateMalformed"" style=""display:none;"">{1}</span>", this.Id, dictionary["Common_Error_DateMalformed"]);

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0}
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
                            </div>
                        </div>
                    </div>",
                    labelSpan,
                    ColumnsSpan,
                    this.Id,
                    this.Value.HasValue ? string.Format("{0:dd/MM/yyyy}", this.Value.Value) : string.Empty,
                    malformedLabel,
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " readonly=\"readonly\"" : string.Empty,
                    (this.GrantToWrite.HasValue && this.GrantToWrite.Value == false) ? " style=\"display:none;\"" : string.Empty);
            }
        }
    }
}
