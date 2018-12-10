// --------------------------------
// <copyright file="CustomersList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

/// <summary>Implements a class for the "CustomersList" page</summary>
public partial class CustomersList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    public UIDataHeader DataHeader { get; set; }

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
        this.user = Session["User"] as ApplicationUser;
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;

        // Security access control
        if (!this.user.HasGrantToRead(ApplicationGrant.Customer))
        {
            this.Response.Redirect("NoPrivileges.aspx", Constant.EndResponse);
            Context.ApplicationInstance.CompleteRequest();
        }

        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Customers");
        this.master.Titulo = "Item_Customers";
        this.RenderCustomersData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = new SbrinnaCoreFramework.UI.UIButton
            {
                Text = this.Dictionary["Item_Customer_Btn_New"],
                Action = "success",
                Icon = "icon-plus",
                Id = "BtnNewCustomer"
            };
        }

        this.DataHeader = new UIDataHeader { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.Dictionary["Item_Customer_ListHeader_Name"], Sortable = true, Filterable = true });
    }

    private void RenderCustomersData()
    {
        var res = new StringBuilder();
        var sea = new StringBuilder();
        var searchItems = new List<string>();
        bool first = true;
        int contData = 0;
        foreach (var customer in Customer.ByCompany(((Company)Session["Company"]).Id))
        {
            if (!customer.Active)
            {
                continue;
            }

            if (!searchItems.Contains(customer.Description))
            {
                searchItems.Add(customer.Description);
            }

            res.Append(customer.ListRow(this.Dictionary, this.user.Grants));
            contData++;
        }

        this.CustomerDataTotal.Text = contData.ToString();

        searchItems.Sort();
        foreach(string item in searchItems)
        {
            if(first)
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

        this.CustomerData.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
    }
}