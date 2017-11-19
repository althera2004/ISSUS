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
                ColumnSpan = 7,
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
            int test = 0;
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
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
            else if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
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

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.user = Session["User"] as ApplicationUser;
        this.company = Session["company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;

        if (this.Request.QueryString["id"] != null)
        {
            this.processId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
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

        }
        
        this.RenderProcesosData();

        if (!IsPostBack)
        {
            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.processId, TargetType.Process);
        }
    }

    private void RenderIndicatorsData()
    {
        StringBuilder res = new StringBuilder();
        ReadOnlyCollection<Indicador> indicators = Indicador.GetByProcess(this.processId, this.company.Id);
        foreach (Indicador indicadtor in indicators)
        {
            res.Append(indicadtor.ListRowProcessTab(this.dictionary, this.user.Grants));
        }

        this.IndicatorsData.Text = res.ToString();
    }

    private void RenderProcesosData()
    {
        ReadOnlyCollection<Process> procesos = Process.GetByCompany(this.company.Id);
        StringBuilder res = new StringBuilder();
        bool first = true;
        foreach (Process process in procesos)
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
}