// --------------------------------
// <copyright file="RapidActionButton.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework.UI
{
    using System.Globalization;

    /// <summary>Implements RapidActionButton class</summary>
    public class RapidActionButton : Element
    {
        /// <summary>Gets the HTML code of a radio button</summary>
        public override string Html
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture, 
                    @"<div class=""col-sm-{1}""><span class=""btn btn-light col-xs-12 col-sm-12"" style=""height:30px;"" id=""{0}"">...</span></div>", 
                    this.Id, 
                    this.Expand);
            }
        }
    }
}
