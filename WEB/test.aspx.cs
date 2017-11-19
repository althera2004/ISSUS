// --------------------------------
// <copyright file="test.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using SbrinnaCoreFramework.UI;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using GisoFramework.Item;

public partial class test : Page
{
    UIDataHeader dataheader;

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string cns = ConfigurationManager.ConnectionStrings["cns"].ConnectionString;
        this.ltCns.Text = "Connection string: " + cns + "<br />";

        SqlConnection cnn = new SqlConnection(cns);
        try
        {
            cnn.Open();
            this.ltCnn.Text = "Conexión sql: OK";

            this.ltCnn.Text += "<br />Application Path:" + this.Request.PhysicalApplicationPath;
            this.ltCnn.Text += "<br />Root Path:" + this.Server.MapPath("~");

        }
        catch (Exception ex)
        {
            this.ltCnn.Text = "Conexión sql:" + ex.Message;
        }
        finally
        {
            if (cnn.State != ConnectionState.Closed)
            {
                cnn.Close();
            }
            cnn.Dispose();
        }

        Employee em = new Employee();
    }
}