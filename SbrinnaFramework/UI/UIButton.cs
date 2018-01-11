// --------------------------------
// <copyright file="UIButton.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Implements Button control
    /// </summary>
    public class UIButton : Element
    {
        /// <summary>
        /// Gets an standard button to save item
        /// </summary>
        public static UIButton FormSaveButton
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                return new UIButton
                {
                    Id = "BtnSave",
                    Icon = "icon-ok",
                    Text = dictionary["Common_Accept"],
                    Action = "success"
                };
            }
        }

        /// <summary>Gets an standard button to cancel item edition</summary>
        public static UIButton FormCancelButton
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                return new UIButton
                {
                    Id = "BtnCancel",
                    Icon = "icon-undo",
                    Text = dictionary["Common_Cancel"]
                };
            }
        }

        /// <summary>Gets an standard button to back navigation</summary>
        public static UIButton FormBackButton
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                return new UIButton
                {
                    Id = "BtnCancel",
                    Icon = "icon-undo",
                    Text = dictionary["Common_Back"]
                };
            }
        }

        /// <summary>
        /// Gets or sets the button text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the button icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets or sets a color that represents the action
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets a JavaScript function call
        /// </summary>
        public string EventClick { get; set; }

        /// <summary>
        /// Gets or sets the column span of button
        /// </summary>
        public int ColumnsSpan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether button is hidden
        /// </summary>
        public bool Hidden { get; set; }

        /// <summary>Gets HTML code for button control</summary>
        public string Render
        {
            get
            {
                string colSm = string.Empty;
                if (this.ColumnsSpan != 0)
                {
                    colSm = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"col-sm{0} ",
                        this.ColumnsSpan);
                }

                string pattern = @"<button class=""{5}btn btn-{3}"" {6} type=""button"" id=""{1}""{4}><i class=""{2} bigger-110""></i>{0}</button>";
                return string.Format(
                    CultureInfo.GetCultureInfo("es-es"),
                    pattern,
                    this.Text,
                    this.Id,
                    this.Icon,
                    this.Action,
                    string.IsNullOrEmpty(this.EventClick) ? string.Empty : " onclick=\"" + this.EventClick + "\"",
                    colSm,
                    this.Hidden ? " style=\"display:none;\"" : string.Empty);
            }
        }

        /// <summary>
        /// Gets HTML code to show button as icon
        /// </summary>
        public string RenderAsIcon
        {
            get
            {
                string colSm = string.Empty;
                if (this.ColumnsSpan != 0)
                {
                    colSm = string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        @"col-sm{0} ",
                        this.ColumnsSpan);
                }

                string pattern = @"<button class=""{5}btn btn-{3}"" type=""button"" id=""{1}""{4}><i class=""{2} bigger-110""></i>{0}</button>";
                return string.Format(
                    CultureInfo.GetCultureInfo("es-es"),
                    pattern,
                    this.Text,
                    this.Id,
                    this.Icon,
                    this.Action,
                    string.IsNullOrEmpty(this.EventClick) ? string.Empty : " onclick=\"" + this.EventClick + "\"",
                    colSm);
            }
        }

        /// <summary>
        /// Gets HTML for a new item button
        /// </summary>
        /// <param name="label">Text of button</param>
        /// <param name="link">Link to item edition page</param>
        /// <returns>HTML for a new item button</returns>
        public static UIButton NewItemButton(string label, string link)
        {
            Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            return new UIButton
            {
                Text = dictionary[label],
                Action = "success",
                Icon = "icon-plus",
                Id = "BtnNewItem",
                EventClick = string.Format(@"document.location='{0}?id=-1';", link)
            };
        }

        /// <summary>
        /// Gets HTML for a new item button
        /// </summary>
        /// <param name="label">Text of button</param>
        /// <param name="link">Link to item edition page</param>
        /// <param name="icon">Icon of button</param>
        /// <returns>HTML for a new item button</returns>
        public static UIButton NewItemButton(string label, string link, string icon)
        {
            Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            return new UIButton
            {
                Text = dictionary[label],
                Action = "success",
                Icon = icon,
                Id = "BtnNewItem",
                EventClick = string.Format(@"document.location='{0}?id=-1';", link)
            };
        }        
    }
}
