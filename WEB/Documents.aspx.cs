// --------------------------------
// <copyright file="Documents.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework.Item;
using GisoFramework;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class Documents : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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

    public string Filter
    {
        get
        {
            if (this.Session["DocumentFilter"] == null)
            {
                return "AI|-1|-1";
            }

            return this.Session["DocumentFilter"].ToString().ToUpperInvariant();
        }
    }

    public string DocumentsJson
    {
        get
        {
            return Document.GetAllJson(this.company.Id);
        }
    }

    public string CategoriesJson
    {
        get
        {
            return DocumentCategory.GetAllJson(this.company.Id);
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
        this.company = (Company)Session["Company"];
        this.user = (ApplicationUser)Session["User"];
        this.master = this.Master as Giso;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master.AddBreadCrumb("Item_Documents");
        this.master.Titulo = "Item_Documents";
        this.RenderDocumentData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Document_Button_New", "DocumentView.aspx");
        }
    }

    private void RenderDocumentData()
    {
        var sea = new StringBuilder(@"""""");
        var documents = Document.GetByCompany((Company)Session["Company"]);
        foreach (var document in documents)
        {
            if (document.Code.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@",'{0}',", document.Code));
            }
            else
            {
                sea.Append(string.Format(@",""{0}"",", document.Code));
            }

            if (document.Description.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}',", document.Description));
            }
            else
            {
                sea.Append(string.Format(@"""{0}"",", document.Description));
            }

            if (document.Location.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}'", document.Location));
            }
            else
            {
                sea.Append(string.Format(@"""{0}""", document.Location));
            }
        }
        
        this.master.SearcheableItems = sea.ToString();
    }
}