//-----------------------------------------------------------------------
// <copyright file="CargosList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

/// <summary>Implements a class for the "CargosList" page</summary>
public partial class CargosList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public UIDataHeader DataHeader { get; set; }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

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
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

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
        var res = new StringBuilder();
        var sea = new StringBuilder();
        var searchItems = new List<string>();
        var cargos = JobPosition.JobsPositionByCompany((Company)Session["Company"]);
        int contData = 0;
        foreach (var cargo in cargos)
        {
            res.Append(cargo.TableRow(this.dictionary, this.user.HasGrantToWrite(ApplicationGrant.JobPosition), this.user.HasGrantToRead(ApplicationGrant.Department)));
            if(!searchItems.Contains(cargo.Description))
            {
                searchItems.Add(cargo.Description);
                contData++;
            }
        }

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