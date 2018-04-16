using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using System.Collections.ObjectModel;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;
using System.Globalization;
using System.IO;

public partial class ProcesosView : Page
{
    #region Fields
    private ApplicationUser user;

    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private int processId;
    private Process process;

    private FormFooter formFooter;

    /// <summary>Company of session</summary>
    private Company company;

    private string procesosListJson;

    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string FormFooter
    {
        get
        {
            return this.formFooter.Render(this.dictionary);
        }
    }

    public string ProcesosListJson
    {
        get
        {
            return this.procesosListJson;
        }
    }
    #endregion

    #region Properties
    public Process Proceso
    {
        get
        {
            return this.process;
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

    /// <summary>
    /// Gets a value indicating if user has Admin privileges in Company
    /// </summary>
    public bool Admin
    {
        get
        {
            return this.user.Admin;
        }
    }

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
    #endregion

    public string TxtName
    {
        get
        {
            return new FormText()
            {
                Name = "TxtName",
                Value = this.Proceso.Description,
                ColumnSpan = 11,
                Placeholder = this.dictionary["Common_Name"],
                Required = true,
                RequiredMessage = this.dictionary["Common_Required"],
                Duplicated = true,
                DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
                GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Provider),
                MaximumLength = 100
            }.Render;
        }
    }

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
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else if (!int.TryParse(this.Request.QueryString["id"], out test))
            {
                this.Response.Redirect("NoAccesible.aspx", true);
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
        this.company = Session["company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        if (this.Request.QueryString["id"] != null)
        {
            this.processId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        string label = this.processId == -1 ? "Item_Process_Button_New" : "Item_Process_Button_Edit";
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Processes", "ProcesosList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.Titulo = label;


        this.formFooter = new FormFooter();
        this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary[ "Common_Accept"], Action = "success" });
        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        this.process = Process.Empty;
        if (processId > 0)
        {
            this.process = new Process(this.processId, this.company.Id);

            if (this.process.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.process = Process.Empty;
            }

            this.formFooter.ModifiedBy = this.process.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.process.ModifiedOn;
            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format("{0}: <strong>{1}</strong>", this.dictionary["Item_Process"], this.process.Description);
            this.RenderIndicatorsData();
            this.RenderDocuments();
        }
        
        this.RenderProcesosData();

        if (!IsPostBack)
        {
            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.processId, TargetType.Process);
        }
    }

    private void RenderIndicatorsData()
    {
        var res = new StringBuilder();
        var indicators = Indicador.ByProcessId(this.processId, this.company.Id);
        foreach (var indicadtor in indicators)
        {
            res.Append(indicadtor.ListRowProcessTab(this.dictionary, this.user.Grants));
        }

        this.IndicatorsData.Text = res.ToString();
    }

    private void RenderProcesosData()
    {
        var procesos = Process.ByCompany(this.company.Id);
        var res = new StringBuilder();
        bool first = true;
        foreach (var process in procesos)
        {
            if(first)
            {
                first=false;
            }
            else
            {
                res.Append(",");
            }

            res.Append(string.Format(@"{{""Id"":{0},""Description"":""{1}""}}", process.Id, process.Description.Replace("\"", "\\\"")));
        }

        this.procesosListJson = res.ToString();
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(9, this.processId, this.company.Id);
        var res = new StringBuilder();
        var resList = new StringBuilder();
        int contCells = 0;
        var extensions = ToolsFile.ExtensionToShow;
        foreach (var file in files)
        {
            decimal finalSize = ToolsFile.FormatSize((decimal)file.Size);
            string fileShowed = string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description;
            if (fileShowed.Length > 15)
            {
                fileShowed = fileShowed.Substring(0, 15) + "...";
            }

            string viewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<div class=""col-sm-2 btn-success"" onclick=""ShowPDF('{0}');""><i class=""icon-eye-open bigger-120""></i></div>",
                file.FileName
                );

            string listViewButton = string.Format(
                CultureInfo.InvariantCulture,
                @"<span class=""btn btn-xs btn-success"" onclick=""ShowPDF('{0}');"">
                            <i class=""icon-eye-open bigger-120""></i>
                        </span>",
                file.FileName);

            var fileExtension = Path.GetExtension(file.FileName);

            if (!extensions.Contains(fileExtension))
            {
                viewButton = "<div class=\"col-sm-2\">&nbsp;</div>";
                listViewButton = "<span style=\"margin-left:30px;\">&nbsp;</span>";
            }

            res.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<div id=""{0}"" class=""col-sm-3 document-container"">
                        <div class=""col-sm-6"">&nbsp</div>
                        {10}
                        <div class=""col-sm-2 btn-info""><a class=""icon-download bigger-120"" href=""/DOCS/{3}/{4}"" target=""_blank"" style=""color:#fff;""></a></div>
                        <div class=""col-sm-2 btn-danger"" onclick=""DeleteUploadFile({0},'{1}');""><i class=""icon-trash bigger-120""></i></div>
                        <div class=""col-sm-12 iconfile"" style=""max-width: 100%;"">
                            <div class=""col-sm-4""><img src=""/images/FileIcons/{2}.png"" /></div>
                            <div class=""col-sm-8 document-name"">
                                <strong title=""{1}"">{9}</strong><br />
                                {7}: {5:dd/MM/yyyy}
                                {8}: {6:#,##0.00} MB
                            </div>
                        </div>
                    </div>",
                    file.Id,
                    string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                    file.Extension,
                    this.company.Id,
                    file.FileName,
                    file.CreatedOn,
                    finalSize,
                    this.Dictionary["Item_Attachment_Header_CreateDate"],
                    this.dictionary["Item_Attachment_Header_Size"],
                    fileShowed,
                    viewButton);

            resList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr id=""tr{2}"">
                    <td>{1}</td>
                    <td align=""center"" style=""width:90px;"">{4:dd/MM/yyyy}</td>
                    <td align=""right"" style=""width:120px;"">{5:#,##0.00} MB</td>
                    <td style=""width:150px;"">
                        {6}
                        <span class=""btn btn-xs btn-info"">
                            <a class=""icon-download bigger-120"" href=""/DOCS/{3}/{0}"" target=""_blank"" style=""color:#fff;""></a>
                        </span>
                        <span class=""btn btn-xs btn-danger"" onclick=""DeleteUploadFile({2},'{1}');"">
                            <i class=""icon-trash bigger-120""></i>
                        </span>
                    </td>
                </tr>",
                file.FileName,
                string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                file.Id,
                this.company.Id,
                file.CreatedOn,
                finalSize,
                listViewButton);

            contCells++;
            if (contCells == 4)
            {
                contCells = 0;
                res.Append("<div style=\"clear:both\">&nbsp;</div>");
            }
        }

        this.LtDocuments.Text = res.ToString();
        this.LtDocumentsList.Text = resList.ToString();
    }
}