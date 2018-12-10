//-----------------------------------------------------------------------
// <copyright file="CargosList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;
using System.Globalization;

/// <summary>Implements a class for the "CargosList" page</summary>
public partial class CargosList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public UIDataHeader DataHeader { get; set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string GraphRows { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else
            {
                this.Go();
            }
        }

        Context.ApplicationInstance.CompleteRequest();
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.JobPosition))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_JobPositions");
        this.master.Titulo = "Item_JobPositions";
        this.RenderJobPositionData();

        if (this.user.HasGrantToWrite(ApplicationGrant.JobPosition))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_JobPosition_Button_New", "CargosView.aspx");
        }
    }

    /// <summary>Generates the HTML code to show JobPosition list</summary>
    private void RenderJobPositionData()
    {
        var graphData = new StringBuilder("[");

        var res = new StringBuilder();
        var sea = new StringBuilder();
        var searchItems = new List<string>();
        var cargos = JobPosition.JobsPositionByCompany((Company)Session["Company"]).OrderBy(c => c.Responsible.Id);
        int contData = 0;
        bool firstGraph = true;
        foreach (var cargo in cargos)
        {
            res.Append(cargo.TableRow(this.Dictionary, this.user.HasGrantToWrite(ApplicationGrant.JobPosition), this.user.HasGrantToRead(ApplicationGrant.Department)));
            if (!searchItems.Contains(cargo.Description))
            {
                searchItems.Add(cargo.Description);
                contData++;
            }

            if (firstGraph)
            {
                firstGraph = false;
            }
            else
            {
                graphData.Append(",");
            }

            graphData.AppendFormat(
                CultureInfo.InvariantCulture,
                @"[{{""v"": ""{0}"", ""f"": ""{1}<div style='color:#333; font-style:italic;'>{2}</div>""}},""{3}"", ""{4}""]{5}",
                cargo.Id,
                cargo.Description,
                cargo.Department.Description,
                cargo.Responsible.Id == 0 ? string.Empty : cargo.Responsible.Id.ToString(),
                cargo.Description,
                Environment.NewLine);
        }

        graphData.Append("]");
        this.GraphRows = graphData.ToString();
        this.CargosDataTotal.Text = contData.ToString();

        searchItems.Sort();
        bool first = true;
        foreach (var item in searchItems)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sea.Append(",");
            }

            if (item.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}'", item));
            }
            else
            {
                sea.Append(string.Format(@"""{0}""", item));
            }
        }

        this.CargosData.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
    }
}