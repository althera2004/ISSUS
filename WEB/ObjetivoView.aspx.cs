// --------------------------------
// <copyright file="ObjetivoView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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
using System.IO;

public partial class ObjetivoView : Page
{
    /// <summary>Gets a random value to prevents static cache files/summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private TabBar tabBar = new TabBar() { Id = "EquipmentTabBar" };

    public string ItemData
    {
        get
        {
            return this.Objetivo.Json;
        }
    }

    public string Periodicities
    {
        get
        {
            return Indicador.GetPeriodicityByCompany(this.company.Id);
        }
    }

    public string IndicadorName
    {
        get
        {
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId.Value > 0)
            {
                Indicador indicador = Indicador.GetById(this.Objetivo.IndicatorId.Value, this.company.Id);
                return indicador.Description;
            }

            return string.Empty;
        }
    }

    public string IndicadoresObjetivo
    {
        get
        {
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId.Value > 0)
            {
                Indicador indicador = Indicador.GetById(this.Objetivo.IndicatorId.Value, this.company.Id);
                return indicador.Json;
            }

            return "null";
        }
    }

    public string Objetivos
    {
        get
        {
            return Objetivo.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string Employees
    {
        get
        {
            return Employee.CompanyListJson(this.company.Id);
        }
    }

    private FormFooter formFooter;

    public string FormFooter
    {
        get
        {
            if (this.formFooter == null)
            {
                return string.Empty;
            }

            return this.formFooter.Render(this.dictionary);
        }
    }

    /// <summary>
    /// Gets or sets if user show help in interface
    /// </summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
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

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public string Registros
    {
        get
        {
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId.Value > 0)
            {
                return IndicadorRegistro.GetByIndicadorJson(this.Objetivo.IndicatorId.Value, this.company.Id);
            }

            return ObjetivoRegistro.GetByObjetivoJson(this.objetivoId, this.company.Id);
        }
    }

    public FormSelect CmbResponsibleRecord { get; set; }

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

    private int objetivoId;

    private string ObjetivoId
    {
        get
        {
            return this.objetivoId.ToString();
        }
    }

    private string returnScript;

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
        }
    }
    public Objetivo Objetivo { get; set; }


    public FormDatePicker TxtFechaCierrePrevista { get; set; }
    public FormDatePicker TxtFechaCierreReal { get; set; }
    public FormTextArea TxtNotes { get; set; }
    public FormTextArea TxtRecursos { get; set; }
    public FormTextArea TxtMetodologia { get; set; }
    public FormSelect CmbIndicador { get; set; }
    public FormSelect CmbResponsible { get; set; }
    public FormSelect CmbResponsibleClose { get; set; }

    private void Go()
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.objetivoId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'ObjetivoList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Objetivos", "ObjetivoList.aspx", false);
        this.master.AddBreadCrumb("Item_Objetivo");
        this.master.Titulo = "Item_Objetivo";
        this.formFooter = new FormFooter();

        if (this.objetivoId > 0)
        {
            this.Session["EquipmentId"] = this.objetivoId;
            this.Objetivo = Objetivo.GetById(this.objetivoId, this.company.Id);
            if (this.Objetivo.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.Objetivo = Objetivo.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Objetivo"], this.Objetivo.Name);

            this.formFooter.ModifiedBy = this.Objetivo.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Objetivo.ModifiedOn;
            this.formFooter.AddButton(new UIButton() { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.dictionary["Item_Objetivo_Btn_Restaurar"], Action = "primary" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Objetivo_Btn_Anular"], Action = "danger" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Save"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }
        else
        {
            this.master.AddBreadCrumb("Item_Objetivo");
            this.master.Titulo = "Item_Objetivo_New_Label";
            this.Objetivo = Objetivo.Empty;
            this.formFooter.ModifiedBy = this.dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_Objetivo_TabBasic"], Available = true });
        this.tabBar.AddTab(new Tab() { Id = "records", Available = true, Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_TabRecords"] });
        this.tabBar.AddTab(new Tab() { Id = "graphics", Available = true, Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_TabGraphics"] });
        

        this.RenderForm();
    }

    private void RenderForm()
    {
        this.TxtRecursos = new FormTextArea()
        {
            Value = this.Objetivo.Resources,
            Label = this.dictionary["Item_Objetivo_FieldLabel_Resources"],
            Name = "TxtResources",
            Rows = 3,
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo)
        };

        this.TxtMetodologia = new FormTextArea()
        {
            Value = this.Objetivo.Methodology,
            Label = this.dictionary["Item_Objetivo_FieldLabel_Methodology"],
            Name = "TxtMethodology",
            Rows = 3,
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo)
        };

        this.TxtNotes = new FormTextArea()
        {
            Value = this.Objetivo.Notes,
            Label = this.dictionary["Item_Objetivo_FieldLabel_Notes"],
            Name = "TxtNotes",
            Rows = 3,
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo)
        };

        this.CmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Objetivo_FieldLabel_Responsible"],
            ColumnsSpan = 3,
            Name = "CmbResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbResponsibleClose = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Objetivo_FieldLabel_CloseResponsible"],
            ColumnsSpan = 9,
            Name = "CmbEndResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo),
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }, 
            RequiredMessage = this.dictionary["Common_Required"],
            Required = true
        };

        this.CmbResponsibleRecord = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_IndicatorRecord_FieldLabel_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbResponsibleRecord",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo), 
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" },
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CmbIndicador = new FormSelect()
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Objetivo_FieldLabel_Indicator"],
            ColumnsSpan = 3,
            Name = "CmbIndicador",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo),
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Indicador indicador in Indicador.GetByCompany(this.company.Id))
        {
            if (indicador.Active || indicador.Id == this.Objetivo.IndicatorId)
            {
                this.CmbIndicador.AddOption(new FormSelectOption()
                {
                    Value = indicador.Id.ToString(),
                    Text = indicador.Description,
                    Selected = indicador.Id == this.Objetivo.IndicatorId
                });
            }
        }

        foreach (Employee e in this.company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsible.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Objetivo.Responsible.Id
                });

                this.CmbResponsibleClose.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Objetivo.EndResponsible.Id
                });

                this.CmbResponsibleRecord.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = false
                });
            }
        }

        this.TxtFechaCierrePrevista = new FormDatePicker()
        {
            Id = "TxtFechaCierrePrevista",
            Value = this.Objetivo.PreviewEndDate,
            ColumnsSpan = 4,
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_Objetivo_FieldLabel_ClosePreviewDate"]
        };

        this.TxtFechaCierreReal = new FormDatePicker()
        {
            Id = "TxtFechaCierreReal",
            Value = this.Objetivo.EndDate,
            ColumnsSpan = 4,
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_Objetivo_FieldLabel_CloseRealDate"]
        };
    }
}