// --------------------------------
// <copyright file="MenuOption.cs" company="Sbrinna">
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
    using System.Linq;
    using System.Text;
    using System.Web;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements MenuOption class</summary>
    public class MenuOption
    {
        /// <summary>Menu options</summary>
        private List<ApplicationItem> children;

        /// <summary>Gets or sets the option's item</summary>
        public ApplicationItem Item { get; set; }

        /// <summary>Gets option children</summary>
        public ReadOnlyCollection<ApplicationItem> Children
        {
            get
            {
                if (this.children == null)
                {
                    this.children = new List<ApplicationItem>();
                }

                return new ReadOnlyCollection<ApplicationItem>(this.children);
            }
        }  
 
        /// <summary>Render the HTML code for an menu option</summary>
        /// <param name="options">Menu options</param>
        /// <returns>HTML code for an menu option</returns>
        public static string RenderMenu(ReadOnlyCollection<MenuOption> options)
        {
            if (options == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            foreach (var option in options)
            {
                res.Append(option.Render());
            }

            return res.ToString();
        }

        /// <summary>Creates the menu structure for an user</summary>
        /// <param name="applicationUserId">User identifier</param>+
        /// <param name="admin">Indicates if user is administrator</param>
        /// <returns>Menu structure for an user</returns>
        public static ReadOnlyCollection<MenuOption> GetMenu(int applicationUserId, bool admin)
        {
            var res = new List<MenuOption>();

            /* ALTER PROCEDURE Application_GetMenu
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("Application_GetMenu"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var item = new ApplicationItem
                                {
                                    Id = rdr.GetInt32(ColumnsApplicationGetMenu.ItemId),
                                    Icon = rdr.GetString(ColumnsApplicationGetMenu.Icon),
                                    Description = rdr.GetString(ColumnsApplicationGetMenu.Description),
                                    Parent = rdr.GetInt32(ColumnsApplicationGetMenu.Parent),
                                    Container = rdr.GetBoolean(ColumnsApplicationGetMenu.Container)
                                };

                                if (!item.Container)
                                {
                                    item.UserGrant = new UserGrant
                                    {
                                        Item = ApplicationGrant.FromInteger(rdr.GetInt32(ColumnsApplicationGetMenu.ItemId)),
                                        UserId = applicationUserId,
                                        GrantToRead = rdr.GetInt32(ColumnsApplicationGetMenu.GrantToRead) == 1,
                                        GrantToWrite = rdr.GetInt32(ColumnsApplicationGetMenu.GrantToWrite) == 1,
                                        GrantToDelete = rdr.GetInt32(ColumnsApplicationGetMenu.GrantToDelete) == 1
                                    };
                                }

                                string url = rdr.GetString(ColumnsApplicationGetMenu.Url);
                                if (!string.IsNullOrEmpty(url))
                                {
                                    string temp = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                                    if (!temp.EndsWith("/", StringComparison.Ordinal))
                                    {
                                        temp += "/";
                                    }

                                    item.Url = new Uri(string.Format(
                                        CultureInfo.InvariantCulture,
                                        "{0}{1}",
                                        temp,
                                        url));
                                }

                                var option = new MenuOption
                                {
                                    Item = item,
                                    children = new List<ApplicationItem>()
                                };

                                if (item.Parent == 0)
                                {
                                    res.Add(option);
                                }
                                else
                                {
                                    foreach (var options in res)
                                    {
                                        if (options.Item.Id == item.Parent)
                                        {
                                            options.children.Add(item);
                                        }
                                    }
                                }
                            }
                        }

                        if (admin)
                        {
                            string temp = HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath, string.Empty);
                            if (!temp.EndsWith("/", StringComparison.Ordinal))
                            {
                                temp += "/";
                            }

                            var config = res.First(o => o.Item.Description == "Common_Configuration");
                            config.AddChild(new ApplicationItem
                            {
                                Id = 0,
                                Description = "Item_Backup",
                                Icon = "icon-gear",
                                Container = false,
                                Parent = 0,
                                Url = new Uri(string.Format(CultureInfo.InvariantCulture, "{0}BackUp.aspx", temp))
                            });
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }

                        cmd.Connection.Dispose();
                    }
                }
            }

            return new ReadOnlyCollection<MenuOption>(res);
        }

        /// <summary>Add child to menu</summary>
        /// <param name="item">Application item for menu option</param>
        public void AddChild(ApplicationItem item)
        {
            if (this.children == null)
            {
                this.children = new List<ApplicationItem>();
            }

            this.children.Add(item);
        }

        /// <summary>Render the HTML code for a menu</summary>
        /// <returns>HTML code for a menu</returns>
        public string Render()
        {
            if (this.Item.Container)
            {
                return this.RenderContainer();
            }

            if (this.Item.Parent == 0)
            {
                return this.RenderLevel0();
            }

            return string.Empty;
        }

        /// <summary>Render the HTML code for a menu container</summary>
        /// <returns>HTML code for a menu container</returns>
        public string RenderContainer()
        {
            if (this.children.Count == 0)
            {
                return string.Empty;
            }

            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var user = HttpContext.Current.Session["User"] as ApplicationUser;
            var res = new StringBuilder();
            bool selected = false;
            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToUpperInvariant();

            foreach (var option in this.Children)
            {
                res.Append(option.Render());
                if (option.Url != null && !selected)
                {
                    selected = option.Url.AbsoluteUri.ToUpperInvariant() == actualUrl;
                }
            }

            string pattern = @"<li class=""hsub{2}"">
                            <a href=""#"" class=""dropdown-toggle"">
                                <i class=""icon-cog""></i>
                                <span class=""menu-text""> {1} </span>
                                <b class=""arrow icon-angle-down""></b>
                            </a>
                            <ul class=""submenu nav-show"" style=""display:{3};"">
                                {0}
                            </ul>
                        </li>";

            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                res,
                dictionary[this.Item.Description],
                selected ? " open" : string.Empty,
                selected ? "block" : "none");
        }

        /// <summary>Render the HTML code for the level 0 of menu</summary>
        /// <returns>HTML code for a menu</returns>
        public string RenderLevel0()
        {
            var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            var actualUrl = HttpContext.Current.Request.Url.AbsoluteUri;
            if (HttpContext.Current.Request.Url.LocalPath != "/")
            {
                string baseUrl = actualUrl.Replace(HttpContext.Current.Request.Url.LocalPath.Substring(1), string.Empty);
                if (!string.IsNullOrEmpty(baseUrl))
                {
                    actualUrl = "/" + actualUrl.Replace(baseUrl, string.Empty);
                }
            }

            bool current = false;
            if (this.Item.Url != null)
            {
                current = this.Item.Url.AbsolutePath.ToUpperInvariant() == actualUrl.ToUpperInvariant();
            }

            string currentText = current ? " class=\"active\"" : string.Empty;
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<li {2} id=""menuoption-{4}""><a href=""{1}""><i class=""{3}""></i><span class=""menu-text""> {0} </span></a></li>",
                dictionary[this.Item.Description],
                this.Item.Url.AbsolutePath,
                currentText,
                this.Item.Icon,
                this.Item.Id);
        }
    }
}