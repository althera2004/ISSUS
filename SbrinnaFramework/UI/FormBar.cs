// --------------------------------
// <copyright file="FormBar.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>
    /// Implements FormBar control
    /// </summary>
    public class FormBar
    {
        public int ColumnSpan { get; set; }

        public string Name { get; set; }

        public string ValueName { get; set; }

        public string ButtonBar { get; set; }

        public string BarToolTip { get; set; }

        public string Value { get; set; }

        public bool Required { get; set; }

        public bool ReadOnly { get; set; }

        public bool GrantToWrite { get; set; }

        public string RequiredMessage { get; set; }

        public bool GrantToEdit { get; set; }

        /// <summary>
        /// Return the HTML code for FormBar component
        /// </summary>
        public string Render
        {
            get
            {
                if (this.GrantToWrite)
                {
                    string requiredLabel = string.Empty;
                    if (this.Required)
                    {
                        requiredLabel = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span class=""ErrorMessage"" id=""{0}ErrorRequired"" style=""display:none;"">{1}</span>", this.Name, this.RequiredMessage);
                    }

                    string buttonBar = string.Empty;
                    if (this.GrantToEdit)
                    {
                        buttonBar = string.Format(
                            CultureInfo.GetCultureInfo("en-us"),
                            @"<div class=""col-sm-1""><span class=""btn btn-light"" style=""height:30px;"" id=""Btn{0}BAR"" title=""{1}"">...</span></div>",
                            this.ButtonBar,
                            this.BarToolTip);
                    }

                    string pattern = @"<div class=""col-sm-{1}"" id=""DivCmb{0}"" style=""height:35px !important;"">
                            <select id=""Cmb{0}"" class=""col-xs-12 col-sm-12 tooltip-info"" onchange=""Cmb{0}Changed();""></select>     
                            <input style=""display:none;"" type=""text"" id=""{3}"" />
                            {2}                                                           
                        </div>
                        {4}";

                    return string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        pattern,
                        this.Name,
                        this.ColumnSpan,
                        requiredLabel,
                        this.ValueName,
                        buttonBar);
                }
                else
                {
                    return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<label class=""col-sm-{1}"">{0}</label>", this.Value, this.ColumnSpan);
                }
            }
        }

        public static bool operator ==(FormBar formBar1, FormBar formBar2)
        {
            if (formBar1 == null)
            {
                return false;
            }

            if (formBar1 != null && formBar2 == null)
            {
                return false;
            }

            return formBar1.Equals(formBar2);
        }

        public static bool operator !=(FormBar formBar1, FormBar formBar2)
        {
            if(formBar1 == null)
            {
                return false;
            }
            
            if (formBar1 != null && formBar2 == null)
            {
                return true;
            }
            return !formBar1.Equals(formBar2);
        }  

        public override bool Equals(object obj)
        {
            if (!(obj is FormBar))
            {
                return false;
            }

            return this.Equals((FormBar)obj);
        }

        public bool Equals(FormBar other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Name != other.Name)
            {
                return false;
            }

            return this.Name == other.Name;
        }

        /// <summary>
        /// Get the hash code of object
        /// </summary>
        /// <returns>Hash code of object</returns>
        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * (23 + this.Name.GetHashCode());
            hash = hash * (23 + this.Value.GetHashCode());
            return hash;
        }   
    }
}
