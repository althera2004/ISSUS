// --------------------------------
// <copyright file="Shortcut.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Web;
    using GisoFramework.DataAccess;

    /// <summary>Class that implements a class for menu's shortcuts</summary>
    public class Shortcut
    {
        /// <summary>Gets or sets the shortcut's identifier</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the shortcut's label text</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets the link address of shortcut</summary>
        public string Link { get; set; }

        /// <summary>Gets or sets the icon of shortcut</summary>
        public string Icon { get; set; }

        /// <summary>Obtain the availables shorcuts actions by user</summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <returns>List of shorcuts actions by user</returns>
        public static ReadOnlyCollection<Shortcut> Available(int applicationUserId)
        {
            var res = new List<Shortcut>();
            using (var cmd = new SqlCommand("ApplicationUser_GetShortcutAvailables"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@UserId", applicationUserId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new Shortcut
                                {
                                    Id = rdr.GetInt32(0),
                                    Label = rdr.GetString(1),
                                    Link = rdr.GetString(2),
                                    Icon = rdr.GetString(3)
                                });
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Shortcut>(res);
        }

        /// <summary>Render the HTML code for a shortcut on user's menu</summary>
        /// <param name="dictionary">Dictionary for fixed text labels</param>
        /// <returns>HTML code for a shortcut on user's menu</returns>
        public string Selector(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"<button class=""btn btn-info"" style=""height:32px;"" onclick=""alert('{0}');"" title=""{0}""><i class=""{1}""></i></button>",
                dictionary[this.Label], 
                this.Icon);
        }

        /// <summary>Gets a Json structure of shortcut</summary>
        /// <param name="dictionary">Dictionary of fixed labels</param>
        /// <returns>Json structure of shortcut</returns>
        public string Json(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"{{""Id"":{0},""Label"":""{1}"",""Icon"":""{2}""}}",
                this.Id,
                Tools.JsonCompliant(dictionary[this.Label]), 
                this.Icon);
        }
    }
}