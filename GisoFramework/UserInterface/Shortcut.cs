// --------------------------------
// <copyright file="Shortcut.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.Item;

    /// <summary>
    /// Class that implements a class for menu's shortcuts
    /// </summary>
    public class Shortcut
    {
        /// <summary>ShortCut's identifier</summary>
        private int id;

        /// <summary>Shortcut's label</summary>
        private string label;

        /// <summary>Link address of shortcut</summary>
        private string link;

        /// <summary>Icon of shortcut</summary>
        private string icon;

        /// <summary>Gets or sets the shortCut's identifier</summary>
        public int Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>Gets or sets the shortcut's label text</summary>
        public string Label
        {
            get 
            { 
                return this.label; 
            }

            set 
            {
                this.label = value;
            }
        }

        /// <summary>Gets or sets the link address of shortcut</summary>
        public string Link
        {
            get 
            {
                return this.link; 
            }

            set 
            {
                this.link = value; 
            }
        }

        /// <summary>Gets or sets the icon of shortcut</summary>
        public string Icon
        {
            get 
            {
                return this.icon; 
            }

            set 
            {
                this.icon = value;
            }
        }

        /// <summary>
        /// Obtain the availables shorcuts actions by user
        /// </summary>
        /// <param name="applicationUserId">User identifier</param>
        /// <returns>List of shorcuts actions by user</returns>
        public static ReadOnlyCollection<Shortcut> Available(int applicationUserId)
        {
            List<Shortcut> res = new List<Shortcut>();
            using (SqlCommand cmd = new SqlCommand("ApplicationUser_GetShortcutAvailables"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = applicationUserId;
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        res.Add(new Shortcut()
                        {
                            id = rdr.GetInt32(0),
                            label = rdr.GetString(1),
                            link = rdr.GetString(2),
                            icon = rdr.GetString(3)
                        });
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

            return new ReadOnlyCollection<Shortcut>(res);
        }

        /// <summary>
        /// Render the HTML code for a shortcut on user's menu
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed text labels</param>
        /// <returns>HTML code for a shortcut on user's menu</returns>
        public string Selector(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<button class=""btn btn-info"" style=""height:32px;"" onclick=""alert('{0}');"" title=""{0}""><i class=""{1}""></i></button>", dictionary[this.label], this.icon);
        }

        /// <summary>
        /// Gets a Json structure of shortcut
        /// </summary>
        /// <param name="dictionary">Dictionary of fixed labels</param>
        /// <returns>Json structure of shortcut</returns>
        public string Json(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Label"":""{1}"",""Icon"":""{2}""}}", this.id, Tools.JsonCompliant(dictionary[this.label]), this.icon);
        }
    }
}
