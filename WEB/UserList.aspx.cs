// --------------------------------
// <copyright file="UserList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class UserList : Page
{
    /// <summary>Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string UserLogin { get { return this.user.UserName; } }
    public string UserName { get { return this.user.UserName; } }

    /// <summary> Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public UIDataHeader DataHeader { get; set; }

    public new string User
    {
        get
        {
            return this.user.Json;
        }
    }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString();
        }
    }

    public string CompanyData
    {
        get
        {
            return this.company.Json;
        }
    }

    public Company Company
    {
        get { return this.company; }
    }

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
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        this.master.AddBreadCrumb("Item_Users");
        this.master.Titulo = "Item_Users";
        this.user = new ApplicationUser(Convert.ToInt32(Session["UserId"]));
        this.company = (Company)Session["Company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);

        this.RenderUserData();

        if (this.user.HasGrantToWrite(ApplicationGrant.User))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_User_Button_New", "UserView.aspx");
        }

        this.DataHeader = new UIDataHeader { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_UserName"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_EmployeeName"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_Email"], Sortable = true, Filterable = true });
    }

    private void RenderUserData()
    {
        var active = new StringBuilder();
        var sea = new StringBuilder();
        var searchedItem = new List<string>();
        bool first = true;
        var users =  ApplicationUser.CompanyUsers(this.company.Id);
        int contData = 0;
        foreach (var userItem in users)
        {
            active.Append(userItem.ListRow(this.dictionary, this.user.Grants));
            if(!searchedItem.Contains(userItem.UserName))
            {
                searchedItem.Add(userItem.UserName);
            }

            if (!searchedItem.Contains(userItem.Email))
            {
                searchedItem.Add(userItem.Email);
            }

            if(!searchedItem.Contains(userItem.Employee.FullName))
            {
                searchedItem.Add(userItem.Employee.FullName);
            }

            contData++;
        }

        searchedItem.Sort();
        foreach(string s1 in searchedItem)
        {
            if (!string.IsNullOrEmpty(s1))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sea.Append(",");
                }

                sea.AppendFormat(CultureInfo.InvariantCulture, @"""{0}""", s1);
            }
        }

        this.UsersData.Text = active.ToString();
        this.UsersDataTotal.Text = contData.ToString();
        this.master.SearcheableItems = sea.ToString();
    }
}