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

    /// <summary>
    /// Gets dictionary for fixed labels
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Gets a random value to prevents static cache files
    /// </summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public UIDataHeader DataHeader { get; set; }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.user = (ApplicationUser)Session["User"];
        this.master = this.Master as Giso;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Documents");
        this.master.Titulo = "Item_Documents";
        this.RenderDocumentData();
        this.RenderDocumentInactiveData();

        if (this.user.HasGrantToWrite(ApplicationGrant.Department))
        {
            this.master.ButtonNewItem = UIButton.NewItemButton("Item_Document_Button_New", "DocumentView.aspx");
        }

        this.DataHeader = new UIDataHeader() { Id = "ListDataHeader", ActionsItem = 2 };
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th0", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Document_ListHeader_Name"], Sortable = true, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th1", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Document_ListHeader_Code"], Width = 120, Sortable = false, Filterable = true });
        this.DataHeader.AddItem(new UIDataHeaderItem() { Id = "th2", HeaderId = "ListDataHeader", DataId = "ListDataTable", Text = this.dictionary["Item_Document_ListHeader_Revision"], Width = 90, Sortable = false });
    }

    private void RenderDocumentData()
    {
        StringBuilder res = new StringBuilder();
        StringBuilder sea = new StringBuilder();
        bool first = true;
        ReadOnlyCollection<Document> documents = Document.GetByCompany((Company)Session["Company"]);

        bool grantWrite = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Document);
        bool grantDelete = UserGrant.HasDeleteGrant(this.user.Grants, ApplicationGrant.Document);

        int cont = 0;
        foreach (Document document in documents)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                sea.Append(",");
            }

            if (document.Code.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}',", document.Code));
            }
            else
            {
                sea.Append(string.Format(@"""{0}"",", document.Code));
            }

            if (document.Description.IndexOf("\"") != -1)
            {
                sea.Append(string.Format(@"'{0}'", document.Description));
            }
            else
            {
                sea.Append(string.Format(@"""{0}""", document.Description));
            }

            res.Append(document.ListRow(this.dictionary, grantWrite, grantDelete));
            cont++;
        }

        this.DocumentDataTotal.Text = cont.ToString();
        this.LtDocumentsActive.Text = res.ToString();
        this.master.SearcheableItems = sea.ToString();
    }

    private void RenderDocumentInactiveData()
    {
        StringBuilder res = new StringBuilder();
        ReadOnlyCollection<Document> documents = Document.GetByCompanyInactive((Company)Session["Company"]);

        bool grantWrite = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Document);
        bool grantDelete = UserGrant.HasDeleteGrant(this.user.Grants, ApplicationGrant.Document);

        foreach (Document document in documents)
        {
            res.Append(document.ListRowInactive(this.dictionary, grantWrite, grantDelete));
        }

        this.LtDocumentsInactive.Text = res.ToString();
        this.DocumentInactiveDataTotal.Text = documents.Count.ToString();
    }
}