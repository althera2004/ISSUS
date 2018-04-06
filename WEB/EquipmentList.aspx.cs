// --------------------------------
// <copyright file="EquipmentList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

/// <summary>Implements equipments list page</summary>
public partial class EquipmentList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    public string EquipmentsJson
    {
        get
        {
            return Equipment.GetListJson(this.company.Id);
        }
    }

    public string Filter
    {
        get
        {
            if(this.Session["EquipmentFilter"] == null)
            {
                return "CVM|0";
            }

            return this.Session["EquipmentFilter"].ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

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
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.company = this.Session["Company"] as Company;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Equipments");
        this.master.Titulo = "Item_Equipments";

        if (this.user.HasGrantToWrite(ApplicationGrant.Equipment))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Equipment_Button_New", "EquipmentView.aspx");
        }

        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Equipment_Header_Code"], Sortable = true, Filterable = true, HiddenMobile = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Equipment_Header_Description"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Equipment_Header_Location"], Sortable = true, HiddenMobile = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th3", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Equipment_Header_Responsible"], Sortable = true, HiddenMobile = true });

        bool grantWrite = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Equipment);
        bool grantDelete = UserGrant.HasDeleteGrant(this.user.Grants, ApplicationGrant.Equipment);
        bool grantEmployee = UserGrant.HasReadGrant(this.user.Grants, ApplicationGrant.Employee);

        //this.EquipmentData.Text = Equipment.List(this.company, grantWrite, grantDelete, grantEmployee, this.dictionary);

        bool first = true;
        var equipments = Equipment.GetList(this.company);
        var searchList = new List<string>();
        foreach (var equipment in equipments)
        {
            if (!searchList.Contains(equipment.Code))
            {
                searchList.Add(equipment.Code);
            }

            if (!searchList.Contains(equipment.Description))
            {
                searchList.Contains(equipment.Description);
            }
        }

        searchList.Sort();
        var sea = new StringBuilder();
        foreach (string item in searchList)
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

        this.master.SearcheableItems = sea.ToString();
        //this.EquipmentDataTotal.Text = equipments.Count.ToString();
    }
}