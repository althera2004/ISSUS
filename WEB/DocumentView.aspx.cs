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
    #region Fields
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private FormFooter formFooter;

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }
    private int documentId;
    private Document documento;
    #endregion

    #region Properties

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public bool Trace
    {
        get
        {
            return this.user.HasGrantToRead(ApplicationGrant.Trace);
        }
    }

    /// <summary>Gets a value indicating if user has Admin privileges in Company</summary>
    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

    public string DocumentAttachActual { get; set; }

    public string CompanyId
    {
        get
        {
            return this.company.Id.ToString(CultureInfo.GetCultureInfo("en-us"));
        }
    }

    public string UserId
    {
        get
        {
            return this.master.UserId;
        }
    }

    public string UserName
    {
        get
        {
            return this.master.UserName;
        }
    }

    public string CategoriasJson
    {
        get
        {
            var categories = DocumentCategory.GetByCompany(this.company.Id);
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
            return DocumentAttach.JsonList(new ReadOnlyCollection<DocumentAttach>(DocumentAttach.GetByDocument(this.documentId, this.company.Id).Where(d => d.Active == true).ToList()));
        }
    }

    public string ProcedenciasJson
    {
        get
        {
            var origins = DocumentOrigin.GetByCompany(this.company.Id);
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
            if (this.documentId == -1)
            {
                return "{}";
            }

            return this.documento.Json;
        }
    }

    public int DocumentId
    {
        get
        {
            return this.documentId;
        }
    }

    public DocumentVersion LastVersion
    {
        get
        {
            return this.documento.LastVersion;
        }
    }

    public string CompanyDocuments
    {
        get
        {
            var res = new StringBuilder();
            bool first = true;
            foreach (var document in Document.GetByCompany(this.company))
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
    #endregion

    /// <summary>Page's load event</summary>
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
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
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
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.user = Session["User"] as ApplicationUser;

        if (this.Request.QueryString["id"] != null)
        {
            this.documentId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        string label = this.documentId == -1 ? "Item_Document_Button_New" : "Item_Document_Tab_Details";
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Documents", "Documents.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;
        this.formFooter = new FormFooter();

        if (this.documentId != -1)
        {
            this.documento = Document.GetById(this.documentId, this.company.Id);
            if (this.documento.Id == 0)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.documento = new Document();
            }

            this.formFooter.ModifiedBy = this.documento.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.documento.ModifiedOn;

            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Document"], this.documento.Description);
            this.master.TitleInvariant = true;
            this.master.Titulo = label;
        }
        else
        {
            this.documento = new Document();
        }

        this.RenderHistorico();
        this.FillCmbConservacion();

        if (!this.IsPostBack)
        {
            this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
            this.FillCmbConservacion();
            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.documentId, TargetType.Document);
        }

        this.formFooter.AddButton(new UIButton() { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.dictionary["Item_Document_Btn_Restaurar"], Action = "primary", Hidden = !this.documento.EndDate.HasValue });
        this.formFooter.AddButton(new UIButton() { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Document_Btn_Anular"], Action = "danger", Hidden = this.documento.EndDate.HasValue });
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
    }

    /// <summary>Generates HTML code for historical changes table</summary>
    private void RenderHistorico()
    {
        this.DocumentAttachActual = "null";
        var firstDate = DateTime.Now;
        var rows = new List<DocumentVersionRow>();
        var attachs = DocumentAttach.GetByDocument(this.documentId, this.company.Id).Where(d=>d.Active == true).ToList();
        foreach (var version in this.documento.Versions)
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

                if (attach.Version == this.LastVersion.Version)
                {
                    this.DocumentAttachActual = attach.Json;
                }
            }

            rows.Add(new DocumentVersionRow
            {
                Id = this.documentId,
                DocumentId = documentId,
                Version = version.Version,
                Date = version.Date,
                Attach = fileName,
                Extension = extension,
                Reason = version.Reason,
                AprovedBy = version.UserCreateName,
                CompanyId = this.company.Id
            });
        }

        var res = new StringBuilder();
        foreach(var row in rows.OrderByDescending(r=>r.Version).ThenByDescending(r=>r.Date))
        {
            firstDate = row.Date;
            string rowText = row.Render(this.dictionary, this.user.Grants);

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
        this.FirstVersionDate = string.Format(CultureInfo.GetCultureInfo("en-us"), "new Date({0:yyyy}, {0:MM} - 1, {0:dd})", firstDate);
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
            this.documento.ConservationType == 1 ? "selected=\"selected\"" : string.Empty, 
            this.documento.ConservationType == 2 ? "selected=\"selected\"" : string.Empty,
            this.documento.ConservationType == 3 ? "selected=\"selected\"" : string.Empty);
    }
}