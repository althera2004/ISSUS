// --------------------------------
// <copyright file="UserGrant.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Web;

    /// <summary>Implments user grants</summary>
    public class UserGrant
    {
        /// <summary>Gets or sets the user idetifier</summary>
        public int UserId { get; set; }

        /// <summary>Gets or sets the item that is affected by grant</summary>
        public ApplicationGrant Item { get; set; }

        /// <summary>Gets or sets a value indicating whether if has read grant</summary>
        public bool GrantToRead { get; set; }

        /// <summary>Gets or sets a value indicating whether if has write grant</summary>
        public bool GrantToWrite { get; set; }

        /// <summary>Gets or sets a value indicating whether if has delete grant</summary>
        public bool GrantToDelete { get; set; }

        /// <summary>Gets or sets the usear that creates grant</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date of grant creationd</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the usear that modifies grant</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the date of last grant modification</summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>Determine if user has read grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="grant">Grant to examine</param>
        /// <returns>Return if user has read grant</returns>
        public static bool HasReadGrant(ReadOnlyCollection<UserGrant> grants, ApplicationGrant grant)
        {
            if (grants == null || grant == null)
            {
                return false;
            }

            foreach (var g in grants)
            {
                if (grant.Code == g.Item.Code)
                {
                    return g.GrantToRead;
                }
            }

            return false;
        }

        /// <summary>Determine if user has write grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="grant">Grant to examine</param>
        /// <returns>Return if user has write grant</returns>
        public static bool HasWriteGrant(ReadOnlyCollection<UserGrant> grants, ApplicationGrant grant)
        {
            if (grants == null || grant == null)
            {
                return false;
            }

            foreach (UserGrant g in grants)
            {
                if (grant.Code == g.Item.Code)
                {
                    return g.GrantToWrite;
                }
            }

            return false;
        }

        /// <summary>Determine if user has delete grant</summary>
        /// <param name="grants">Collection of user grants</param>
        /// <param name="grant">Grant to examine</param>
        /// <returns>Return if user has delete grant</returns>
        public static bool HasDeleteGrant(ReadOnlyCollection<UserGrant> grants, ApplicationGrant grant)
        {
            if (grants == null || grant == null)
            {
                return false;
            }

            foreach (UserGrant g in grants)
            {
                if (grant.Code == g.Item.Code)
                {
                    return g.GrantToDelete;
                }
            }

            return false;
        }

        /// <summary>Render HTML code to show a row with the grant edition</summary>
        /// <returns>HTML code to show a row with the grant edition</returns>
        public string Render()
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            string label = string.Empty;
            if (dictionary.ContainsKey(this.Item.Description))
            {
                label = dictionary[this.Item.Description];
            }
            else
            {
                if (dictionary.ContainsKey("Item_" + this.Item.Description))
                {
                    label = dictionary["Item_" + this.Item.Description];
                }
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"<tr><td>{0}</td><td align=""center""><input type=""checkbox"" id=""CheckboxRead{1}"" onclick=""GrantChanged('R',{1},this);"" class=""CBR"" {2}/></td><td align=""center""><input type=""checkbox"" id=""CheckboxWrite{1}"" onclick=""GrantChanged('W',{1},this);"" class=""CBW"" {3}/></td></tr>",
                label,
                this.Item.Code,
                this.GrantToRead ? " checked=\"checked\"" : string.Empty,
                this.GrantToWrite ? " checked=\"checked\"" : string.Empty);
        }
    }
}