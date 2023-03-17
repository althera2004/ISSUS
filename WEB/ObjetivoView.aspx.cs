// --------------------------------
// <copyright file="ObjetivoView.aspx.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class ObjetivoView : Page
{
    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary> Master of page</summary>
    private Giso master;    

    private TabBar tabBar = new TabBar { Id = "EquipmentTabBar" };

    public Company Company { get; private set; }

    public ApplicationUser ApplicationUser { get; private set; }

    public int ActionsOpen { get; private set; }

    public string Historic
    {
        get
        {
            return ObjetivoHistorico.ByObjetivoIdJsonList(this.objetivoId);
        }
    }

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
            return Indicador.PeriodicityByCompany(this.Company.Id);
        }
    }

    public string IndicadorName
    {
        get
        {
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId > 0)
            {
                var indicador = Indicador.ById(this.Objetivo.IndicatorId.Value, this.Company.Id);
                return GisoFramework.Tools.JsonCompliant(indicador.Description);
            }

            return string.Empty;
        }
    }

    public string ActionsList
    {
        get
        {
            return IncidentAction.ByObjetivoIdJsonList(this.objetivoId, this.Company.Id, this.Dictionary);
        }
    }

    public string IndicadoresObjetivo
    {
        get
        {
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId > 0)
            {
                var indicador = Indicador.ById(this.Objetivo.IndicatorId.Value, this.Company.Id);
                return indicador.Json;
            }

            return "null";
        }
    }

    public string Objetivos
    {
        get
        {
            return Objetivo.ByCompanyJsonList(this.Company.Id);
        }
    }

    public string Employees
    {
        get
        {
            return Employee.CompanyListJson(this.Company.Id);
        }
    }

    /// <summary>Gets or sets if user show help in interface</summary>
    public bool ShowHelp
    {
        get
        {
            return this.ApplicationUser.ShowHelp;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

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
            if (this.Objetivo.IndicatorId.HasValue && this.Objetivo.IndicatorId > 0)
            {
                return IndicadorRegistro.ByIndicadorJson(this.Objetivo.IndicatorId.Value, this.Company.Id);
            }

            return ObjetivoRegistro.GetByObjetivoJson(this.objetivoId, this.Company.Id);
        }
    }

    public FormSelect CmbResponsibleRecord { get; set; }

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

    private int objetivoId;

    private string ObjetivoId
    {
        get
        {
            return this.objetivoId.ToString();
        }
    }

    public string ReturnScript { get; private set; }

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
            this.objetivoId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.ReturnScript = "document.location = 'ObjetivoList.aspx';";
        }
        else
        {
            this.ReturnScript = "document.location = referrer;";
        }

        this.ApplicationUser = (ApplicationUser)Session["User"];
        this.Company = (Company)Session["company"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Objetivos", "ObjetivoList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_Objetivo_Detail");
        this.master.Titulo = "Item_Objetivo_Detail";
        this.master.formFooter = new FormFooter();

        this.ActionsOpen = 0;
        if (this.objetivoId > 0)
        {
            this.Session["EquipmentId"] = this.objetivoId;
            this.Objetivo = Objetivo.ById(this.objetivoId, this.Company.Id);
            if (this.Objetivo.CompanyId != this.Company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.NotLeaft);
                Context.ApplicationInstance.CompleteRequest();
                this.Objetivo = Objetivo.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Objetivo_Header_Name"], this.Objetivo.Name);

            //this.formFooter.ModifiedBy = this.Objetivo.ModifiedBy.Description;
            //this.formFooter.ModifiedOn = this.Objetivo.ModifiedOn;
            this.master.ModifiedBy = this.Objetivo.ModifiedBy.Description;
            this.master.ModifiedOn = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.Objetivo.ModifiedOn);

            this.master.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-print", Text = this.Dictionary["Common_Print"], Action = "info" });
            this.master.formFooter.AddButton(new UIButton { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.Dictionary["Item_Objetivo_Btn_Restaurar"], Action = "primary" });
            this.master.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.Dictionary["Item_Objetivo_Btn_Anular"], Action = "danger" });
            this.master.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Save"], Action = "success" });
        }
        else
        {
            //this.master.AddBreadCrumb("Item_Objetivo");
            this.master.Titulo = "Item_Objetivo_Detail";
            this.Objetivo = Objetivo.Empty;
            this.master.ModifiedBy = Dictionary["Common_New"];
            this.master.ModifiedOn = "-";
            //this.formFooter.ModifiedBy = this.Dictionary["Common_New"];
            //this.formFooter.ModifiedOn = DateTime.Now;
            this.master.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        }

        this.master.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.Dictionary["Item_Objetivo_TabBasic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "actions", Available = this.objetivoId > 0 && this.ApplicationUser.HasGrantToRead(ApplicationGrant.IncidentActions), Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_TabActions"] });
        this.tabBar.AddTab(new Tab { Id = "records", Available = this.objetivoId > 0, Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_Tab_Records"] });
        this.tabBar.AddTab(new Tab { Id = "graphics", Available = this.objetivoId > 0, Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_TabGraphics"] });
        this.tabBar.AddTab(new Tab { Id = "historic", Available = this.objetivoId > 0 && this.ApplicationUser.HasGrantToRead(ApplicationGrant.IncidentActions), Active = this.objetivoId > 0, Hidden = this.objetivoId < 1, Label = this.Dictionary["Item_Objetivo_TabHistoric"] });

        this.RenderForm();
    }

    private void RenderForm()
    {
        var grantToWrite = this.ApplicationUser.HasGrantToWrite(ApplicationGrant.Objetivo);
        var defaultComboOption = FormSelectOption.DefaultOption(this.Dictionary);
        this.TxtRecursos = new FormTextArea
        {
            Value = this.Objetivo.Resources,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_Resources"],
            Name = "TxtResources",
            Rows = 3,
            GrantToWrite = grantToWrite
        };

        this.TxtMetodologia = new FormTextArea
        {
            Value = this.Objetivo.Methodology,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_Methodology"],
            Name = "TxtMethodology",
            Rows = 3,
            GrantToWrite = grantToWrite
        };

        this.TxtNotes = new FormTextArea
        {
            Value = this.Objetivo.Notes,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_Notes"],
            Name = "TxtNotes",
            Rows = 3,
            GrantToWrite = grantToWrite
        };

        this.CmbResponsible = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_Responsible"],
            ColumnsSpan = 3,
            Name = "CmbResponsible",
            GrantToWrite = grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultComboOption
        };

        this.CmbResponsibleClose = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_CloseResponsible"],
            ColumnsSpan = 9,
            Name = "CmbEndResponsible",
            GrantToWrite = grantToWrite,
            DefaultOption = defaultComboOption, 
            RequiredMessage = this.Dictionary["Common_Required"],
            Required = true
        };

        this.CmbResponsibleRecord = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_IndicatorRecord_FieldLabel_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbResponsibleRecord",
            GrantToWrite = grantToWrite, 
            DefaultOption = defaultComboOption,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CmbIndicador = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_Indicator"],
            ColumnsSpan = 3,
            Name = "CmbIndicador",
            GrantToWrite = grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = defaultComboOption
        };

        foreach (var indicador in Indicador.ByCompany(this.Company.Id))
        {
            if (indicador.Active || indicador.Id == this.Objetivo.IndicatorId)
            {
                this.CmbIndicador.AddOption(new FormSelectOption
                {
                    Value = indicador.Id.ToString(),
                    Text = indicador.Description,
                    Selected = indicador.Id == this.Objetivo.IndicatorId
                });
            }
        }

        foreach (var e in this.Company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsible.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Objetivo.Responsible.Id
                });

                this.CmbResponsibleClose.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Objetivo.EndResponsible.Id
                });

                this.CmbResponsibleRecord.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = false
                });
            }
        }

        this.TxtFechaCierrePrevista = new FormDatePicker
        {
            Id = "TxtFechaCierrePrevista",
            Value = this.Objetivo.PreviewEndDate,
            ColumnsSpan = Constant.ColumnSpan4,
            ColumnsSpanLabel = Constant.ColumnSpan2,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_ClosePreviewDate"]
        };

        this.TxtFechaCierreReal = new FormDatePicker
        {
            Id = "TxtFechaCierreReal",
            Value = this.Objetivo.EndDate,
            ColumnsSpan = Constant.ColumnSpan4,
            ColumnsSpanLabel = Constant.ColumnSpan2,
            Label = this.Dictionary["Item_Objetivo_FieldLabel_CloseRealDate"]
        };
    }
}