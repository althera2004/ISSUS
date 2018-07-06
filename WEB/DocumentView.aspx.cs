// --------------------------------
// <copyright file="DocumentView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public partial class DocumentView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    public ApplicationUser ApplicationUser { get; private set; }

    /// <summary>Company of session</summary>
    public Company Company { get; private set; }

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
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

    public new bool Trace
    {
        get
        {
            return this.ApplicationUser.HasGrantToRead(ApplicationGrant.Trace);
        }
    }

    public string DocumentAttachActual { get; set; }

    public string CategoriasJson
    {
        get
        {
            var categories = DocumentCategory.ByCompany(this.Company.Id);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var category in categories)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(category.Json);
            }

            return res.Append("]").ToString();
        }
    }

    public string Attachs
    {
        get
        {
            return DocumentAttach.JsonList(new ReadOnlyCollection<DocumentAttach>(DocumentAttach.ByDocument(this.DocumentId, this.Company.Id).Where(d => d.Active == true).ToList()));
        }
    }

    public string ProcedenciasJson
    {
        get
        {
            var origins = DocumentOrigin.ByCompany(this.Company.Id);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var origin in origins)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(origin.Json);
            }

            return res.Append("]").ToString();
        }
    }

    public string DocumentoJson
    {
        get
        {
            if (this.DocumentId == -1)
            {
                return "{}";
            }

            return this.Document.Json;
        }
    }

    public int DocumentId { get; private set; }

    public Document Document { get; private set; }

    public string CompanyDocuments
    {
        get
        {
            var res = new StringBuilder();
            bool first = true;
            foreach (var document in Document.GetByCompany(this.Company))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(document.JsonSimple);
            }

            return res.ToString();
        }
    }

    public string FirstVersionDate { get; set; }

    /// <summary>Gets the dictionary for interface texts</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
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
        }
        else
        {
            int test = 0;
            this.ApplicationUser = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.ApplicationUser.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
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
        this.Company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.ApplicationUser = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.DocumentId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        string label = this.DocumentId == -1 ? "Item_Document_Tab_Details" : "Item_Document_Tab_Details";
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Documents", "Documents.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
        this.formFooter = new FormFooter();

        if (this.DocumentId != -1)
        {
            this.Document = Document.ById(this.DocumentId, this.Company.Id);
            if (this.Document.Id == 0)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.Document = new Document();
            }

            this.formFooter.ModifiedBy = this.Document.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Document.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Document"], this.Document.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.Document = new Document();
        }

        this.RenderHistorico();
        this.FillCmbConservacion();

        if (!this.IsPostBack)
        {
            this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
            this.FillCmbConservacion();
            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.DocumentId, TargetType.Document);
        }

        if (this.DocumentId != -1)
        {
            this.formFooter.AddButton(new UIButton { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.dictionary["Item_Document_Btn_Restaurar"], Action = "primary", Hidden = !this.Document.EndDate.HasValue });
            this.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Document_Btn_Anular"], Action = "danger", Hidden = this.Document.EndDate.HasValue });
        }
        this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
    }

    /// <summary>Generates HTML code for historical changes table</summary>
    private void RenderHistorico()
    {
        this.DocumentAttachActual = "null";
        var firstDate = DateTime.Now;
        var rows = new List<DocumentVersionRow>();
        var attachs = DocumentAttach.ByDocument(this.DocumentId, this.Company.Id).Where(d=>d.Active == true).ToList();
        foreach (var version in this.Document.Versions)
        {
            string fileName = string.Empty;
            long documentId = 0;
            string extension = string.Empty;
            if(attachs.Any(f=>f.Version == version.Version))
            {
                var attach = attachs.First(f => f.Version == version.Version);
                fileName = attach.Description;
                documentId = attach.Id;
                extension = attach.Extension;

                if (attach.Version == this.Document.LastVersion.Version)
                {
                    this.DocumentAttachActual = attach.Json;
                }
            }

            rows.Add(new DocumentVersionRow
            {
                Id = this.DocumentId,
                DocumentId = documentId,
                Version = version.Version,
                Date = version.Date,
                Attach = fileName,
                Extension = extension,
                Reason = version.Reason,
                AprovedBy = version.UserCreateName,
                CompanyId = this.Company.Id
            });
        }

        var res = new StringBuilder();
        foreach(var row in rows.OrderByDescending(r=>r.Version).ThenByDescending(r=>r.Date))
        {
            firstDate = row.Date;
            string rowText = row.Render(this.dictionary, this.ApplicationUser.Grants);

            string extension = rowText.Replace("window.open(", "^"); ;
            if (extension.IndexOf("^") != -1)
            {
                extension = extension.Split('^')[1].Split('.')[1].Split('\'')[0];
                rowText = rowText.Replace("icon-edit bigger-120", "icon-download");

                if (extension == "jpg" || extension == "jpeg" || extension == "png" || extension == "gif")
                {
                }
                else
                {
                    rowText = rowText.Replace("btn-success\"", "btn-success\" style=\"display:none;\"");
                }
            }
            else
            {
                extension = string.Empty;
            }

            rowText = rowText.Replace("icon-edit bigger-120", "icon-download");

            res.Append("<!-- ").Append(extension).Append(" -->");

            res.Append(rowText);
        }

        this.LtHistorico.Text = res.ToString();
        this.FirstVersionDate = string.Format(CultureInfo.InvariantCulture, "new Date({0:yyyy}, {0:MM} - 1, {0:dd})", firstDate);
    }

    private void FillCmbConservacion()
    {
        string pattern = @"
                <option value=""0""></option>
                <option value=""1"" {3}>{0}</option>
                <option value=""2"" {4}>{1}</option>
                <option value=""3"" {5}>{2}</option>
            ";
        this.LtConservacion.Text = string.Format(
            CultureInfo.InvariantCulture,
            pattern,
            this.dictionary["Common_Years"],
            this.dictionary["Common_Months"],
            this.dictionary["Common_Days"],
            this.Document.ConservationType == 1 ? "selected=\"selected\"" : string.Empty, 
            this.Document.ConservationType == 2 ? "selected=\"selected\"" : string.Empty,
            this.Document.ConservationType == 3 ? "selected=\"selected\"" : string.Empty);
    }
}