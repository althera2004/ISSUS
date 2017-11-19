// --------------------------------
// <copyright file="FormFooter.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Text;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FormFooter
    {
        private List<UIButton> buttons;
        public string ModifiedBy { get; set; }

        /// <summary>Gets or sets the date and time of last modification of item</summary>
        public DateTime ModifiedOn { get; set; }

        public FormFooter()
        {
            this.buttons = new List<UIButton>();
        }

        public FormFooter(Dictionary<string, string> dictionary, bool grantToEdit)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            this.ModifiedBy = dictionary["Common_New"];
            this.ModifiedOn = DateTime.Now;
            this.buttons = new List<UIButton>();
            if (grantToEdit)
            {
                this.buttons.Add(UIButton.FormSaveButton);
                this.buttons.Add(UIButton.FormCancelButton);
            }
            else
            {
                this.buttons.Add(UIButton.FormBackButton);
            }
        }

        public FormFooter(string modifiedBy, DateTime modifiedOn, bool grantToEdit)
        {
            this.ModifiedBy = modifiedBy;
            this.ModifiedOn = modifiedOn;
            this.buttons = new List<UIButton>();
            if (grantToEdit)
            {
                this.buttons.Add(UIButton.FormSaveButton);
                this.buttons.Add(UIButton.FormCancelButton);
            }
            else
            {
                this.buttons.Add(UIButton.FormBackButton);
            }
        }

        public ReadOnlyCollection<UIButton> Buttons
        {
            get
            {
                if (this.buttons == null)
                {
                    this.buttons = new List<UIButton>();
                }

                return new ReadOnlyCollection<UIButton>(this.buttons);
            }
        }

        public void AddButton(UIButton button)
        {
            if (this.buttons == null)
            {
                this.buttons = new List<UIButton>();
            }

            this.buttons.Add(button);
        }

        public string Render(Dictionary<string, string> dictionary)
        {
            if(dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            StringBuilder buttonsHtml = new StringBuilder();
            StringBuilder iconsHtml = new StringBuilder();
            bool first = true;
            if (this.buttons != null)
            {
                foreach (UIButton button in this.buttons)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        buttonsHtml.Append("&nbsp;&nbsp;");
                        iconsHtml.Append("&nbsp;");
                    }

                    buttonsHtml.Append(button.Render);
                    iconsHtml.Append(button.RenderAsIcon);
                }
            }

            string modifiedBy = string.Empty;
            if (!string.IsNullOrEmpty(this.ModifiedBy))
            {
                modifiedBy = string.Format(
                    CultureInfo.GetCultureInfo("es-es"),
                    "{0}:&nbsp;<strong>{1}</strong><br /><i>{2:dd/MM/yyyy}</i>",
                    dictionary["Common_ModifiedBy"],
                    this.ModifiedBy,
                    this.ModifiedOn);
            }

            return string.Format(
                CultureInfo.GetCultureInfo("es-es"),
                    @"
                    <!-- Form footer -->
                    <div id=""oldFormFooter"" class=""clearfix form-actions"" style=""margin-bottom:50px;"">
                        <div style=""float:left;"" id=""ItemStatus"">
                            {0}
                        </div>
                        <div style=""float:right;"" id=""ItemButtons"">
                            {1}
                        </div>
                    </div>
                    <!-- Form footer -->",
                    modifiedBy,
                    buttonsHtml);
        }
    }
}
