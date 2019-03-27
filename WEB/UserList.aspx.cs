// --------------------------------
// <copyright file="UserList.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_UserName"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_EmployeeName"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem { Id = "th3", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_User_List_Header_Email"], Sortable = true, Filterable = true });
    }

    private void RenderUserData()
    {
        var active = new StringBuilder();
        var sea = new StringBuilder();
        var searchedItem = new List<string>();
        bool first = true;
        var users =  ApplicationUser.CompanyUsers(this.company.Id);
        int contData = 0;

        foreach (var userItem in users.Where(u => u.PrimaryUser == true))
        {
            var row = string.Empty;
            if (dictionary == null)
            {
                dictionary = Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.User);
            bool grantDelete = UserGrant.HasDeleteGrant(this.user.Grants, ApplicationGrant.User);

            string employeeLink = userItem.Employee != null ? userItem.Employee.Link : string.Empty;

            string iconDelete = string.Empty;

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""UserUpdate({0},'{2}');""><i class=""icon-eye-open bigger-120""></i></span>",
                userItem.Id,
                dictionary["Common_View"],
                userItem.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""UserUpdate({0},'{2}');""><i class=""icon-edit bigger-120""></i></span>",
                userItem.Id,
                dictionary["Common_Edit"],
                userItem.Description);
            }

            string iconAdmin = iconAdmin = "<i class=\"icon-star\" style=\"color:#428bca;\" title=" + dictionary["User_PrimaryUser"] + "></i>";


            string pattern = @"<tr><td style=""width:40px;"">{5}</td><td>{0}</td><td style=""width:300px;"">{1}</td><td style=""width:300px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td></tr>";
            row = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                userItem.Link,
                employeeLink,
                userItem.Email,
                iconEdit,
                string.Empty,
                iconAdmin);


            active.Append(row);

            if (!searchedItem.Contains(userItem.UserName))
            {
                searchedItem.Add(userItem.UserName);
            }

            if (!searchedItem.Contains(userItem.Email))
            {
                searchedItem.Add(userItem.Email);
            }

            if (!searchedItem.Contains(userItem.Employee.FullName))
            {
                searchedItem.Add(userItem.Employee.FullName);
            }

            contData++;
        }

        foreach (var userItem in users.Where(u=>u.PrimaryUser == false))
        {
            active.Append(userItem.ListRow(this.dictionary, this.user.Grants));

            if (!searchedItem.Contains(userItem.UserName))
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