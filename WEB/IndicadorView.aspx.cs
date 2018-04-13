// --------------------------------
// <copyright file="IndicadorView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework;
using SbrinnaCoreFramework.UI;

public partial class IndicadorView : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private bool grantToWrite;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string Employees
    {
        get
        {
            return Employee.CompanyListJson(this.company.Id);
        }
    }

    public string UserLanguage
    {
        get
        {
            return this.user.Language;
        }
    }

    public string IndicadoresObjetivo
    {
        get
        {
            if (this.IndicadorId < 1)
            {
                return "[]";
            }

            return IndicadorObjetivo.JsonList(IndicadorObjetivo.GetByIndicador(this.IndicadorId, this.company.Id));
        }
    }

    public string Historic
    {
        get
        {
            return IndicadorHistorico.ByIndicadorIdJsonList(this.IndicadorId);
        }
    }

    public int IndicadorId { get; set; }

    public Indicador Indicador { get; set; }

    private TabBar tabBar = new TabBar { Id = "IndicadorTabBar" };

    public BarPopup UnitsBarPopups { get; set; }

    public string Registros
    {
        get
        {
            return IndicadorRegistro.GetByIndicadorJson(this.IndicadorId, this.company.Id);
        }
    }

    public string Procesos
    {
        get
        {
            return Process.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string ProcesosType
    {
        get
        {
            return ProcessType.GetByCompanyJsonList(this.company.Id, this.dictionary);
        }
    }

    public string Unidades
    {
        get
        {
            return Unidad.GetByCompanyJsonList(this.company.Id);
        }
    }

    #region IndicadorForm
    public FormText TxtDescription { get; set; }
    public FormTextInteger TxtPeriodicidad { get; set; }
    public FormTextDecimal TxtMeta { get; set; }
    public FormTextDecimal TxtAlarma { get; set; }
    public FormTextArea TxtCalculo { get; set; }
    public FormSelect CmbUnidad { get; set; }
    public FormSelect CmbMetaComparer { get; set; }
    public FormSelect CmbAlarmaComparer { get; set; }
    public FormSelect CmbEndResponsible { get; set; }
    public FormSelect CmbType { get; set; }
    public FormSelect CmbProcess { get; set; }
    public FormSelect CmbObjetivo { get; set; }
    public FormSelect CmbResponsible { get; set; }
    public FormSelect CmbResponsibleRecord { get; set; }
    public FormSelect CmbResponsibleAnularRecord { get; set; }
    public FormDatePicker EndDate { get; set; }
    #endregion

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    /// <summary>Gets or sets a value indicating wheter if user shows help in interface</summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    /// <summary>Gets or sets a value indicating wheter if user has administrative privileges</summary>
    public bool IsAdmin
    {
        get
        {
            return this.user.Admin;
        }
    }

    private string returnScript;
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

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    public string ProcessJson
    {
        get
        {
            return Process.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string ObjetivosJson
    {
        get
        {
            return Objetivo.GetByCompanyJsonList(this.company.Id);
        }
    }

    public string GrantToWrite
    {
        get
        {
            return this.grantToWrite ? "true" : "false";
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
        if (this.Request.QueryString["id"] != null)
        {
            this.IndicadorId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'IndicadorList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.formFooter = new FormFooter();

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Indicadores", "IndicadorList.aspx", false);
        this.grantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Indicador);

        if (this.IndicadorId > 0)
        {
            this.Indicador = Indicador.GetById(this.IndicadorId, this.company.Id);
            this.master.AddBreadCrumbInvariant(this.Indicador.Description);
            if (this.Indicador.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.Indicador = Indicador.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Indicador"], this.Indicador.Description);

            this.formFooter.ModifiedBy = this.Indicador.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Indicador.ModifiedOn;
            this.formFooter.AddButton(new UIButton { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.dictionary["Item_Objetivo_Btn_Restaurar"], Action = "primary" });
            this.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Indicador_Btn_Anular"], Action = "danger" });
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
            this.master.ItemCode = this.Indicador.Description;
        }
        else
        {
            this.master.AddBreadCrumb("Item_Indicador");
            this.master.Titulo = "Item_Indicador_New_Label";
            this.Indicador = Indicador.Empty;
            this.formFooter.ModifiedBy = this.dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }

        this.tabBar.AddTab(new Tab { Id = "home", Selected = true, Active = true, Label = this.dictionary["Item_Indicador_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "records", Available = true && this.IndicadorId > 0, Active = this.IndicadorId > 0, Label = this.dictionary["Item_Indicador_Tab_Records"] });
        this.tabBar.AddTab(new Tab { Id = "graphics", Available = true && this.IndicadorId > 0, Active = this.IndicadorId > 0, Label = this.dictionary["Item_Indicador_Tab_Graphics"] });
        this.tabBar.AddTab(new Tab { Id = "historic", Available = true, Active = this.IndicadorId > 0, Hidden = this.IndicadorId < 1, Label = this.Dictionary["Item_Indicador_TabHistoric"] });

        this.RenderForm();

        this.UnitsBarPopups = new BarPopup
        {
            Id = "Units",
            DeleteMessage = this.dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
            Description = this.dictionary["Item_Unidades"],
            FieldName = this.dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Providers"]
        };
    }

    public void RenderRegistrosData()
    {
        var res = new StringBuilder();
        foreach (var registro in IndicadorRegistro.GetByIndicador(this.IndicadorId, this.company.Id))
        {
            res.Append(registro.ListRow(this.dictionary, this.user.Grants));
        }

        this.LtRegistrosData.Text = res.ToString();
    }

    public void RenderForm()
    {
        this.TxtCalculo = new FormTextArea
        {
            Rows = 3,
            Value = this.Indicador.Calculo,
            Name = "TxtCalculo",
            Label = this.dictionary["Item_Indicador_Field_Calculo"],
            ColumnsSpan = 11,
            ColumnsSpanLabel = 1,
            MaxLength = 500,
            GrantToWrite = this.grantToWrite            
        };

        this.CmbUnidad = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Indicador_Field_Unidad"],
            ColumnsSpan = 2,
            Name = "CmdUnidad",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = "...", Value = "0" }
        };

        this.CmbMetaComparer = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Indicador_Field_Meta"],
            ColumnsSpan = 1,
            Name = "CmbMetaComparer",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = "..." , Value = "0" }
        };

        this.CmbAlarmaComparer = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Indicador_Field_Alarma"],
            ColumnsSpan = 1,
            Name = "CmbAlarmaComparer",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = "...", Value = "0" }
        };

        this.CmbResponsible = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Indicador_Field_Responsible"],
            ColumnsSpan = 3,
            Name = "CmbResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" },
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CmbResponsibleRecord = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_IndicatorRecord_FieldLabel_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbResponsibleRecord",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" },
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CmbResponsibleAnularRecord = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_IndicatorRecord_FieldLabel_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbResponsibleAnularRecord",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = this.dictionary["Common_SelectAll"], Value = "0" },
            RequiredMessage = this.dictionary["Common_Required"],
            Required = true
        };

        this.CmbEndResponsible = new FormSelect
        {
            ColumnsSpanLabel = 1,
            Label = this.dictionary["Item_Indicador_Field_EndResponsible"],
            ColumnsSpan = 3,
            Name = "CmbEndResponsible",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CmbType = new FormSelect
        {
            Label = this.dictionary["Item_Indicador_Label_Type"],
            ColumnsSpanLabel = 1,
            ColumnsSpan = 3,
            Name = "CmbType",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = this.dictionary["Common_All_Male_Plural"], Value = "0" }
        };

        this.CmbProcess = new FormSelect
        {
            Label = this.dictionary["Item_Indicador_Field_Process"],
            ColumnsSpanLabel = 1,
            ColumnsSpan = 3,
            Name = "CmbProcess",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = this.dictionary["Common_SelectOne"], Value = "0" }
        };

        this.CmbObjetivo = new FormSelect
        {
            Label = this.dictionary["Item_Indicador_Field_Objetivo"],
            ColumnsSpanLabel = 1,
            ColumnsSpan = 3,
            Name = "CmbObjetivo",
            GrantToWrite = this.grantToWrite,
            DefaultOption = new FormSelectOption { Text = this.dictionary["Common_SelectOne"], Value = "0" },
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.TxtDescription = new FormText
        {
            ColumnSpan = 10,
            ColumnSpanLabel = 1,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Indicador_Field_Name"],
            MaximumLength = 100,
            Name = "TxtDescription",
            Placeholder = this.Dictionary["Item_Indicador_Field_Name"],
            Value = this.Indicador.Description,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.TxtPeriodicidad = new FormTextInteger
        {
            ColumnSpan = 2,
            ColumnSpanLabel = 1,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Indicador_Field_Periodicity"],
            MaximumLength = 3,
            Name = "TxtPeriodicity",
            Placeholder = this.Dictionary["Item_Indicador_Field_Periodicity"],
            Value = this.Indicador.Periodicity.ToString(),
            Required = true
        };

        this.TxtMeta = new FormTextDecimal
        {
            ColumnSpan = 1,
            ColumnSpanLabel = 0,
            GrantToWrite = this.grantToWrite,
            Label = string.Empty,
            MaximumLength = 8,
            Name = "TxtMeta",
            Placeholder = this.Dictionary["Item_Indicador_Field_Meta"],
            Value = this.Indicador.Meta.ToString(),
            Required = true
        };

        this.TxtAlarma = new FormTextDecimal
        {
            ColumnSpan = 1,
            ColumnSpanLabel = 0,
            GrantToWrite = this.grantToWrite,
            Label = string.Empty,
            MaximumLength = 8,
            Name = "TxtAlarma",
            Placeholder = this.Dictionary["Item_Indicador_Field_Alarma"]
        };

        if (this.Indicador.Alarma.HasValue)
        {
            this.TxtAlarma.Value = this.Indicador.Alarma.ToString();
        }

        this.EndDate = new FormDatePicker
        {
            Id = "TxtEndDate",
            Label = this.dictionary["Item_Indicador_Field_EndDate"],
            ColumnsSpanLabel = 4,
            ColumnsSpan = 8,
            Value = this.Indicador.EndDate,
            GrantToWrite = this.grantToWrite
        };

        foreach (Unidad unidad in Unidad.GetActive(this.company.Id))
        {
            this.CmbUnidad.AddOption(new FormSelectOption() 
                { 
                    Text = unidad.Description, 
                    Value = unidad.Id.ToString(), 
                    Selected = unidad.Id == this.Indicador.Unidad.Id 
                });
        }

        //foreach (Objetivo objetivo in Objetivo.GetActive(this.company.Id))
        //{
        //    this.CmbObjetivo.AddOption(new FormSelectOption() 
        //        { 
        //            Text = objetivo.Description, 
        //            Value = objetivo.Id.ToString(), 
        //            Selected = objetivo.Id == this.Indicador.Objetivo.Id 
        //        });
        //}

        foreach (var proceso in Process.GetByCompany(this.company.Id).Where(p => p.Active == true))
        {
            this.CmbProcess.AddOption(new FormSelectOption()
                {
                    Value = proceso.Id.ToString(),
                    Text = proceso.Description,
                    Selected = proceso.Id == this.Indicador.Proceso.Id
                });
        }

        this.CmbType.AddOption(new FormSelectOption
        {
            Value = "1",
            Text = this.dictionary["Item_Indicador_Label_TypeObjetivo"],
            Selected = false
        });

        this.CmbType.AddOption(new FormSelectOption
        {
            Value = "2",
            Text = this.dictionary["Item_Indicador_Label_TypeProceso"],
            Selected = false
        });

        long responsibleId = 0;
        if (this.Indicador.Responsible != null)
        {
            responsibleId = this.Indicador.Responsible.Id;
        }

        foreach (var e in this.company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsible.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == responsibleId
                });

                this.CmbResponsibleRecord.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = false
                });

                this.CmbResponsibleAnularRecord.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = false
                });
            }
        }

        this.CmbMetaComparer.AddOption(new FormSelectOption { Value = "eq", Text = "=", Selected = this.Indicador.MetaComparer == "eq" });
        this.CmbMetaComparer.AddOption(new FormSelectOption { Value = "gt", Text = "&gt;", Selected = this.Indicador.MetaComparer == "gt" });
        this.CmbMetaComparer.AddOption(new FormSelectOption { Value = "eqgt", Text = "=&gt;", Selected = this.Indicador.MetaComparer == "eqgt" });
        this.CmbMetaComparer.AddOption(new FormSelectOption { Value = "lt", Text = "&lt;", Selected = this.Indicador.MetaComparer == "lt" });
        this.CmbMetaComparer.AddOption(new FormSelectOption { Value = "eqlt", Text = "&lt;=", Selected = this.Indicador.MetaComparer == "eqlt" });

        this.CmbAlarmaComparer.AddOption(new FormSelectOption { Value = "eq", Text = "=", Selected = this.Indicador.AlarmaComparer == "eq" });
        this.CmbAlarmaComparer.AddOption(new FormSelectOption { Value = "gt", Text = "&gt;", Selected = this.Indicador.AlarmaComparer == "gt" });
        this.CmbAlarmaComparer.AddOption(new FormSelectOption { Value = "eqgt", Text = "=&gt;", Selected = this.Indicador.AlarmaComparer == "eqgt" });
        this.CmbAlarmaComparer.AddOption(new FormSelectOption { Value = "lt", Text = "&lt;", Selected = this.Indicador.AlarmaComparer == "lt" });
        this.CmbAlarmaComparer.AddOption(new FormSelectOption { Value = "eqlt", Text = "&lt;=", Selected = this.Indicador.AlarmaComparer == "eqlt" });
    }
}