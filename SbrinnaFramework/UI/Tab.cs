// --------------------------------
// <copyright file="Tab.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>
    /// Implements Tab class
    /// </summary>
    public class Tab
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public bool Selected { get; set; }

        public bool Active { get; set; }

        public bool Available { get; set; }

        public bool Hidden { get; set; }

        public string Render
        {
            get
            {
                if (this.Available)
                {
                    string pattern = @"<li class=""{1}"" id=""Tab{0}""{4}>
                           <a {2}href=""#{0}"">{3}</a>
                          </li>";
                    return string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        pattern,
                        this.Id,
                        this.Selected ? "active" : string.Empty,
                        this.Active ? "data-toggle=\"tab\" " : string.Empty,
                        this.Label,
                        this.Hidden ? " style=\"display:none;\"" : string.Empty);
                }

                return string.Empty;
            }
        }
    }
}
