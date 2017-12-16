// --------------------------------
// <copyright file="PreInitSession.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using GisoFramework;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

/// <summary>
/// Implements PreInitSession page
/// </summary>
public partial class PreInitSession : Page
{
    /// <summary>Gets or sets user identififer/summary>
    public string UserId { get; set; }

    /// <summary>Gets or sets user password</summary>
    public string CompanyId { get; set; }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string password = this.Request.Form["Password"].ToString();
        this.UserId = this.Request.Form["UserId"].ToString();
        this.CompanyId = this.Request.Form["CompanyId"].ToString();
        ApplicationUser.SetPassword(Convert.ToInt32(this.UserId), password);
        /*string query = "UPDATE ApplicationUser SET Password = '" + Password + "', MustResetPassword = 0 where Id = " + UserId;
        using (SqlCommand cmd = new SqlCommand(query))
        {
            cmd.CommandType = CommandType.Text;
            cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }*/
    }
}