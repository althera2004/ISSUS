// --------------------------------
// <copyright file="BashboardReports.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.DataAccess;
using GisoFramework.Item;
using SbrinnaCoreFramework;

public partial class Reports : Page
{
    /// <summary>Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString();
        }
    }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        Session["User"] = this.user;
        this.master = this.Master as Giso;
        this.company = Session["Company"] as Company;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master.AddBreadCrumb("Page_DashboarReports");
        this.master.Titulo = "Page_DashboarReports";
        this.ChartIncident();
        this.ChartIncidentAction();
    }

    public int ChartIncidentTotal { get; private set; }
    public decimal ChartIncident1 { get; private set; }
    public decimal ChartIncident2 { get; private set; }
    public decimal ChartIncident3 { get; private set; }
    public string ChartIncident4 { get; private set; }

    private void ChartIncident()
    {
        using (var cmd = new SqlCommand("ReportChart_Incident"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.company.Id));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            var total = rdr.GetInt32(0);
                            this.ChartIncidentTotal = total;
                            this.ChartIncident1 = (int)((decimal)rdr.GetInt32(1) / total * 100);
                            this.ChartIncident2 = (int)((decimal)rdr.GetInt32(2) / total * 100);
                            this.ChartIncident3 = (int)((decimal)rdr.GetInt32(3) / total * 100);
                            this.ChartIncident4 = string.Format(CultureInfo.InvariantCulture, "{0:#,###,###.00}", rdr.GetDecimal(4)).Replace(',','*').Replace('.',',').Replace('*','.');
                        }
                    }
                }
                finally
                {
                    if(cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }
    }

    public int ChartIncidentActionTotal { get; private set; }
    public decimal ChartIncidentAction1 { get; private set; }
    public decimal ChartIncidentAction2 { get; private set; }
    public decimal ChartIncidentAction3 { get; private set; }
    public decimal ChartIncidentAction4 { get; private set; }

    private void ChartIncidentAction()
    {
        using (var cmd = new SqlCommand("ReportChart_IncidentAction"))
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
            {
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.company.Id));
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            var total = rdr.GetInt32(0);
                            this.ChartIncidentActionTotal = total;
                            this.ChartIncidentAction1 = (int)((decimal)rdr.GetInt32(1) / total * 100);
                            this.ChartIncidentAction2 = (int)((decimal)rdr.GetInt32(2) / total * 100);
                            this.ChartIncidentAction3 = (int)((decimal)rdr.GetInt32(3) / total * 100);
                            this.ChartIncidentAction4 = (int)((decimal)rdr.GetInt32(4) / total * 100);
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
    }

    public string AsignedQuote
    {
        get
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", this.company.DiskQuote);
        }
    }

    public string DiskQuote
    {
        get
        {
            return UploadFile.GetQuota(this.company);
        }
    }
}