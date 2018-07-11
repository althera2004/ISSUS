// --------------------------------
// <copyright file="EquipmentView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;

public partial class EquipmentView : Page
{
    /// <summary>Gets a random value to prevents static cache files/summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    public string LimitInitialDate
    {
        get
        {
            var records = EquipmentRecord.GetFilter(this.equipmentId, this.Company.Id, true, true, true, true, true, true, true, true, null, null);

            if (records.Count > 0)
            {
                var record = records.OrderBy(r => r.Date).First();
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{0:dd/MM/yyyy}",
                    record.Date);
            }

            return Constant.NowText;
        }
    }

    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    public Company Company { get; private set; }

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    public string Launch { get; private set; }

    private bool grantToWrite;

    public string ResponsibleData { get; set; }

    public string SelectedTab { get; set; }

    public long CalibrationProviderId
    {
        get
        {
            if (this.Equipment.ExternalCalibration != null)
            {
                if (this.Equipment.ExternalCalibration.Provider != null)
                {
                    return this.Equipment.ExternalCalibration.Provider.Id;
                }
            }

            return 0;
        }
    }

    public string ExternalCalibrationResponsible
    {
        get
        {
            return this.CalibrationExternalCmbResponsible.Render;
        }
    }

    public long VerificationProviderId
    {
        get
        {
            if (this.Equipment.ExternalVerification != null)
            {
                if (this.Equipment.ExternalVerification.Provider != null)
                {
                    return this.Equipment.ExternalVerification.Provider.Id;
                }
            }

            return 0;
        }
    }

    public string EquipmentCalibrationActList
    {
        get
        {
            return EquipmentCalibrationAct.JsonList(this.equipmentId, this.Company.Id);
        }
    }

    public string OperationId { get; set; }

    public string EquipmentVerificationActList
    {
        get
        {
            return EquipmentVerificationAct.JsonList(this.equipmentId, this.Company.Id);
        }
    }

    public string EquipmentRepairList
    {
        get
        {
            return EquipmentRepair.JsonList(this.equipmentId, this.Company.Id);
        }
    }

    public string EquipmentMaintenanceDefinitionList
    {
        get
        {
            return EquipmentMaintenanceDefinition.JsonList(this.equipmentId, this.Company.Id);
        }
    }

    public string EquipmentMaintenanceActList
    {
        get
        {
            return EquipmentMaintenanceAct.JsonList(this.equipmentId, this.Company.Id);
        }
    }

    public string EquipmentScaleDivisionList
    {
        get
        {
            return EquipmentScaleDivision.JsonList("EquipmentScaleDivision", EquipmentScaleDivision.GetByCompany(this.Company));
        }
    }

    public string EquipmentScaleDivisionSelected
    {
        get
        {
            if (this.equipmentId > 0)
            {
                if (this.Equipment.MeasureUnit != null)
                {
                    return this.Equipment.MeasureUnit.Id.ToString();
                }
                else
                {
                    return "0";
                }
            }

            return "0";
        }
    }

    public BarPopup EquipmentScaleDivisionBarPopups { get; set; }

    public BarPopup ProviderBarPopups { get; set; }

    /// <summary>Gets or sets if user show help in interface</summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    private string returnScript;

    private long equipmentId;

    private string EquipmentId
    {
        get
        {
            return this.equipmentId.ToString();
        }
    }

    public Equipment Equipment { get; set; }


    private FormFooter formFooter;
    private FormFooter formFooterCalibration;
    private FormFooter formFooterVerification;
    private FormFooter formFooterMaintenance;
    private FormFooter formFooterRepair;
    private FormFooter formFooterRecords;

    public string FormFooter
    {
        get
        {
            if (this.formFooter == null)
            {
                return string.Empty;
            }

            return this.formFooter.Render(this.Dictionary);
        }
    }

    public string FormFooterCalibration
    {
        get
        {
            if (this.formFooterCalibration == null)
            {
                return string.Empty;
            }

            return this.formFooterCalibration.Render(this.Dictionary);
        }
    }

    public string FormFooterVerification
    {
        get
        {
            if (this.formFooterVerification == null)
            {
                return string.Empty;
            }

            return this.formFooterVerification.Render(this.Dictionary);
        }
    }

    public string FormFooterMaintenance
    {
        get
        {
            if (this.formFooterMaintenance == null)
            {
                return string.Empty;
            }

            return this.formFooterMaintenance.Render(this.Dictionary);
        }
    }

    public string FormFooterRepair
    {
        get
        {
            if (this.formFooterRepair == null)
            {
                return string.Empty;
            }

            return this.formFooterRepair.Render(this.Dictionary);
        }
    }

    public string FormFooterRecords
    {
        get
        {
            if (this.formFooterRecords == null)
            {
                return string.Empty;
            }

            return this.formFooterRecords.Render(this.Dictionary);
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
    public Dictionary<string, string> Dictionary { get; private set; }

    TabBar tabBar = new TabBar { Id = "EquipmentTabBar" };

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public string ProvidersJson
    {
        get
        {
            return Provider.ByCompanyJson(this.Company.Id);
        }
    }

    public string CustomersJson
    {
        get
        {
            return Customer.ByCompanyJson(this.Company.Id);
        }
    }

    public string EmployeesJson
    {
        get
        {
            return Employee.CompanyListJson(this.Company.Id);
            //return Employee.GetByCompanyJson(this.company.Id);
        }
    }

    public string GrantToWrite
    {
        get
        {
            return this.grantToWrite ? "true" : "false";
        }
    }

    #region Fields Datos básicos
    public FormText TxtCode { get; set; }
    public FormText TxtDescription { get; set; }
    public FormText TxtTradeMark { get; set; }
    public FormText TxtModel { get; set; }
    public FormText TxtSerialNumber { get; set; }
    public FormText TxtLocation { get; set; }
    public FormTextDecimal TxtMeasureRange { get; set; }
    public FormTextFreeDecimal TxtScaleDivision { get; set; }
    public FormBar BarScaleDivisionType { get; set; }
    public FormSelect CmbResponsible { get; set; }
    public FormTextArea TxtNotes { get; set; }
    public FormTextArea TxtObservations { get; set; }
    public FormDatePicker TxtStartDate { get; set; }
    public ImageSelector ImgEquipment { get; set; }
    #endregion

    #region Fields Calibration
    public FormText CalibrationInternalTxtOperation { get; set; }
    public FormText CalibrationExternalTxtOperation { get; set; }
    public FormTextInteger CalibrationInternalTxtPeriodicity { get; set; }
    public FormTextInteger CalibrationExternalTxtPeriodicity { get; set; }
    public FormTextFreeDecimal CalibrationInternalTxtUncertainty { get; set; }
    public FormTextFreeDecimal CalibrationExternalTxtUncertainty { get; set; }
    public FormText CalibrationInternalTxtRange { get; set; }
    public FormText CalibrationExternalTxtRange { get; set; }
    public FormText CalibrationExternalTxtPattern { get; set; }
    public FormText CalibrationInternalTxtPattern { get; set; }
    public FormTextDecimal CalibrationExternalTxtCost { get; set; }
    public FormTextDecimal CalibrationInternalTxtCost { get; set; }
    public FormTextArea CalibrationExternalTxtNotes { get; set; }
    public FormTextArea CalibrationInternalTxtNotes { get; set; }
    public FormSelect CalibrationExternalCmbResponsible { get; set; }
    public FormSelect CalibrationInternalCmbResponsible { get; set; }
    public FormSelect CalibrationExternalCmbProvider { get; set; }
    #endregion

    #region Fields Verification
    public FormText VerificationInternalTxtOperation { get; set; }
    public FormText VerificationExternalTxtOperation { get; set; }
    public FormTextInteger VerificationInternalTxtPeriodicity { get; set; }
    public FormTextInteger VerificationExternalTxtPeriodicity { get; set; }
    public FormTextFreeDecimal VerificationInternalTxtUncertainty { get; set; }
    public FormTextFreeDecimal VerificationExternalTxtUncertainty { get; set; }
    public FormText VerificationInternalTxtRange { get; set; }
    public FormText VerificationExternalTxtRange { get; set; }
    public FormText VerificationExternalTxtPattern { get; set; }
    public FormText VerificationInternalTxtPattern { get; set; }
    public FormTextDecimal VerificationExternalTxtCost { get; set; }
    public FormTextDecimal VerificationInternalTxtCost { get; set; }
    public FormTextArea VerificationExternalTxtNotes { get; set; }
    public FormTextArea VerificationInternalTxtNotes { get; set; }
    public FormSelect VerificationExternalCmbResponsible { get; set; }
    public FormSelect VerificationInternalCmbResponsible { get; set; }
    #endregion

    #region Fields Maintenance
    public UIButton MaintenanceNewConfiguration { get; set; }
    public UIButton MaintenanceNewAct { get; set; }
    #endregion

    public FormSelect CmbResponsibleClose { get; set; }


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
            this.user = this.Session["User"] as ApplicationUser;
            var token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
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

            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.equipmentId = Convert.ToInt32(this.Request.QueryString["id"]);
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'EquipmentList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.formFooter = new FormFooter();
        this.formFooterCalibration = new FormFooter();
        this.formFooterVerification = new FormFooter();
        this.formFooterMaintenance = new FormFooter();
        this.formFooterRepair = new FormFooter();
        this.formFooterRecords = new FormFooter();
        this.Equipment = new Equipment();

        this.user = (ApplicationUser)Session["User"];
        this.Company = (Company)Session["company"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Equipments", "EquipmentList.aspx", Constant.NotLeaft);
        this.master.AddBreadCrumb("Item_Equipment_Tab_Details");

        if (this.equipmentId > 0)
        {
            this.Session["EquipmentId"] = this.equipmentId;
            this.Equipment = Equipment.ById(this.equipmentId, this.Company);
            if (this.Equipment.CompanyId != this.Company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                Context.ApplicationInstance.CompleteRequest();
                this.Equipment = Equipment.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Equipment"], this.Equipment.Code + " - " + this.Equipment.Description);

            this.formFooter.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Equipment.ModifiedOn;
            this.formFooter.AddButton(new UIButton { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.Dictionary["Item_Equipment_Btn_Restaurar"], Action = "primary" });
            this.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.Dictionary["Item_Equipment_Btn_Anular"], Action = "danger" });
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"], ColumnsSpan = 12 });

            this.formFooterCalibration.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterCalibration.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterCalibration.AddButton(new UIButton { Id = "BtnCalibrationPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterCalibration.AddButton(new UIButton { Id = "BtnCalibrationSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterCalibration.AddButton(new UIButton { Id = "BtnCalibrationCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

            this.formFooterVerification.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterVerification.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterVerification.AddButton(new UIButton { Id = "BtnVerificationPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterVerification.AddButton(new UIButton { Id = "BtnVerificationSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterVerification.AddButton(new UIButton { Id = "BtnVerificationCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

            this.formFooterMaintenance.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterMaintenance.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterMaintenance.AddButton(new UIButton { Id = "BtnMaintenancePrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterMaintenance.AddButton(new UIButton { Id = "BtnMaintenanceSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterMaintenance.AddButton(new UIButton { Id = "BtnMaintenanceCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

            this.formFooterRepair.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterRepair.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterRepair.AddButton(new UIButton { Id = "BtnRepairPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterRepair.AddButton(new UIButton { Id = "BtnRepairSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterRepair.AddButton(new UIButton { Id = "BtnRepairCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

            this.formFooterRecords.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterRecords.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterRecords.AddButton(new UIButton { Id = "BtnRecordsPrint", Icon = "icon-file-pdf", Text = this.Dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterRecords.AddButton(new UIButton { Id = "BtnRecordsSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterRecords.AddButton(new UIButton { Id = "BtnRecordsCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

            this.master.ItemCode = string.Format("{0} - {1}", this.Equipment.Code, this.Equipment.Description);
            this.RenderDocuments();
        }
        else
        {
            this.master.Titulo = "Item_Equipment_Button_New";
            this.Equipment = Equipment.Empty;
            this.formFooter.ModifiedBy = this.Dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
        }

        this.SelectedTab = "home";
        if (this.Request.QueryString["Tab"] != null)
        {
            SelectedTab = this.Request.QueryString["Tab"].Trim().ToLowerInvariant();
        }

        this.tabBar.AddTab(new Tab { Id = "home", Selected = SelectedTab == "home", Active = true, Label = this.Dictionary["Item_Equipment_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "calibracion", Selected = SelectedTab == "calibracion", Available = true, Active = true, Label = this.Dictionary["Item_Equipment_Tab_Calibration"] });
        this.tabBar.AddTab(new Tab { Id = "verificacion", Selected = SelectedTab == "verificacion", Available = true, Active = true, Label = this.Dictionary["Item_Equipment_Tab_Verification"] });
        this.tabBar.AddTab(new Tab { Id = "mantenimiento", Selected = SelectedTab == "mantenimiento", Available = true, Active = true, Label = this.Dictionary["Item_Equipment_Tab_Maintenance"] });
        this.tabBar.AddTab(new Tab { Id = "reparaciones", Available = this.equipmentId > 0, Active = true, Label = this.Dictionary["Item_Equipment_Tab_Repair"] });
        this.tabBar.AddTab(new Tab { Id = "registros", Available = this.equipmentId > 0, Active = true, Label = this.Dictionary["Item_Equipment_Tab_Records"] });
        this.tabBar.AddTab(new Tab { Id = "uploadFiles", Selected = SelectedTab == "uploadfiles", Available = true, Active = this.equipmentId > 0, Hidden = this.equipmentId < 1, Label = this.Dictionary["Item_Equipment_Tab_UploadFiles"] });
        //// this.tabBar.AddTab(new Tab { Id = "trazas", Available = this.user.HasTraceGrant() && this.equipmentId > 0, Active = this.equipmentId > 0, Label = this.dictionary["Item_Equipment_Tab_Traces"] });

        this.RenderForm();

        if (this.Request.QueryString["OperationId"] != null)
        {
            this.OperationId = this.Request.QueryString["OperationId"].Trim();
        }
        else
        {
            this.OperationId = string.Empty;
        }

        if (SelectedTab.Equals("verificacion",StringComparison.OrdinalIgnoreCase))
        {
            if (this.Request.QueryString["Type"] != null)
            {
                string type = this.Request.QueryString["Type"];
                this.Launch = "ShowDialogEquipmentVerificacionPopup(" + (type == "I" ? "-1" : "-2") + ");";
            }
        }

        if (SelectedTab.Equals("calibracion", StringComparison.OrdinalIgnoreCase))
        {
            if (this.Request.QueryString["Type"] != null)
            {
                string type = this.Request.QueryString["Type"];
                this.Launch = "ShowDialogNewCalibrationPopup(" + (type == "I" ? "-1" : "-2") + ");";
            }
        }

        if (SelectedTab.Equals("mantenimiento", StringComparison.OrdinalIgnoreCase))
        {
            if (this.Request.QueryString["Action"] != null)
            {
                string actionId = this.Request.QueryString["Action"];
                this.Launch = "mantenimientoLaunchId = " + actionId + ";EquipmentMaintenanceDefinitionRegister(null);";
            }
        }

        if (SelectedTab.Equals("tabuploadfiles", StringComparison.OrdinalIgnoreCase))
        {
            this.Launch = @"$(""#TabuploadFiles a"").click();";
        }
    }

    private void RenderForm()
    {
        this.grantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Equipment);

        this.RenderTabBasicData();

        #region TabCalibration
        #region Internal
        this.CalibrationInternalTxtOperation = new FormText
        {
            Name = "TxtCalibrationInternalOperation",
            ColumnSpan = 9,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Operation"],
            MaximumLength = 100,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Operation"],
            Value = this.Equipment.InternalCalibration.Description,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtPeriodicity = new FormTextInteger
        {
            Name = "TxtCalibrationInternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Periodicity"],
            MaximumLength = 5,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Periodicity"],
            Value = this.Equipment.InternalCalibration.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtUncertainty = new FormTextFreeDecimal
        {
            Name = "TxtCalibrationInternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00######}", this.Equipment.InternalCalibration.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalCalibration.Uncertainty),
            Required = true,
            Numeric = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtRange = new FormText
        {
            Name = "TxtCalibrationInternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Range"],
            MaximumLength = 50,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Range"],
            Value = this.Equipment.InternalCalibration.Range,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtPattern = new FormText
        {
            Name = "TxtCalibrationInternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Pattern"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Pattern"],
            Value = this.Equipment.InternalCalibration.Pattern,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtCost = new FormTextDecimal
        {
            Name = "TxtCalibrationInternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Cost"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Cost"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.InternalCalibration.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalCalibration.Cost),
            Numeric = true,
            // ISSUS-18
            Required = false,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtNotes = new FormTextArea
        {
            Name = "TxtCalibrationInternalNotes",
            Rows = 5,
            Value = this.Equipment.InternalCalibration.Notes,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Notes"]
        };

        this.CalibrationInternalCmbResponsible = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationInternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption { Text = this.Dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (var e in this.Company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                long responsibleId = -1;
                if (this.Equipment.InternalCalibration != null)
                {
                    if (this.Equipment.InternalCalibration.Responsible != null)
                    {
                        responsibleId = this.Equipment.InternalCalibration.Responsible.Id;
                    }
                }

                this.CalibrationInternalCmbResponsible.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == responsibleId
                });
            }
        }
        #endregion
        #region External
        this.CalibrationExternalTxtOperation = new FormText
        {
            Name = "TxtCalibrationExternalOperation",
            ColumnSpan = 9,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Operación",
            MaximumLength = 100,
            Placeholder = "Operación",
            Value = this.Equipment.ExternalCalibration.Description,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtPeriodicity = new FormTextInteger
        {
            Name = "TxtCalibrationExternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Periodicidad",
            MaximumLength = 5,
            Placeholder = "Periodicidad",
            Value = this.Equipment.ExternalCalibration.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtUncertainty = new FormTextFreeDecimal
        {
            Name = "TxtCalibrationExternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00####}", this.Equipment.ExternalCalibration.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalCalibration.Uncertainty),
            Required = true,
            Numeric = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtRange = new FormText
        {
            Name = "TxtCalibrationExternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Range"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Range"],
            Value = this.Equipment.ExternalCalibration.Range,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtPattern = new FormText
        {
            Name = "TxtCalibrationExternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Pattern"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Required = true,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Pattern"],
            Value = this.Equipment.ExternalCalibration.Pattern
        };

        this.CalibrationExternalTxtCost = new FormTextDecimal
        {
            Name = "TxtCalibrationExternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Cost"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_Equipment_Field_Calibration_Cost"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.ExternalCalibration.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalCalibration.Cost),
            // ISSUS-18
            Required = false,
            Numeric = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtNotes = new FormTextArea
        {
            Name = "TxtCalibrationExternalNotes",
            Rows = 5,
            Value = this.Equipment.ExternalCalibration.Notes,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Notes"],
        };

        this.CalibrationExternalCmbResponsible = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationExternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.Dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CalibrationExternalCmbProvider = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_Field_Calibration_Provider"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationExternalProvider",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption { Text = this.Dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (var e in this.Company.Employees)
        {
            if (e.Active && e.DisabledDate == null)
            {
                long responsibleId = -1;
                if (this.Equipment.ExternalCalibration != null)
                {
                    if (this.Equipment.ExternalCalibration.Responsible != null)
                    {
                        responsibleId = this.Equipment.ExternalCalibration.Responsible.Id;
                    }
                }

                this.CalibrationExternalCmbResponsible.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == responsibleId
                });
            }
        }

        long providerId = -1;
        if (this.Equipment.ExternalCalibration != null)
        {
            if (this.Equipment.ExternalCalibration.Provider != null)
            {
                providerId = this.Equipment.ExternalCalibration.Provider.Id;
            }
        }

        foreach (var provider in Provider.ByCompany(this.Company.Id))
        {
            if (provider.Active)
            {
                this.CalibrationExternalCmbProvider.AddOption(new FormSelectOption
                {
                    Value = provider.Id.ToString(),
                    Text = provider.Description,
                    Selected = provider.Id == providerId
                });
            }
        }
        #endregion
        #endregion

        #region TabVerification
        #region Internal
        this.VerificationInternalTxtOperation = new FormText()
        {
            Name = "TxtVerificationInternalOperation",
            ColumnSpan = 9,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Operation"],
            MaximumLength = 100,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Operation"],
            Value = this.Equipment.InternalVerification.Description,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.VerificationInternalTxtPeriodicity = new FormTextInteger()
        {
            Name = "TxtVerificationInternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Periodicity"],
            MaximumLength = 5,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Periodicity"],
            Value = this.Equipment.InternalVerification.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.VerificationInternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtVerificationInternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00####}", this.Equipment.InternalVerification.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalVerification.Uncertainty),
            Numeric = true,
            Nullable = true
        };

        this.VerificationInternalTxtRange = new FormText()
        {
            Name = "TxtVerificationInternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Range"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Range"],
            Value = this.Equipment.InternalVerification.Range
        };

        this.VerificationInternalTxtPattern = new FormText()
        {
            Name = "TxtVerificationInternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Patrón",
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = "Patrón",
            Value = this.Equipment.InternalVerification.Pattern
        };

        this.VerificationInternalTxtCost = new FormTextDecimal()
        {
            Name = "TxtVerificationInternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Coste",
            MaximumLength = 8,
            Placeholder = "Coste",
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.InternalVerification.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalVerification.Cost),
            Numeric = true,
            // ISSUS-18
            Required = false,
            RequiredMessage = this.Dictionary["Common_Required"],
            Nullable = true
        };

        this.VerificationInternalTxtNotes = new FormTextArea()
        {
            Name = "TxtVerificationInternalNotes",
            Rows = 5,
            Value = this.Equipment.InternalVerification.Notes,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Notes"]
        };

        this.VerificationInternalCmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 9,
            Name = "CmbVerificationInternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.Dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.Company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                long responsibleId = -1;
                if (this.Equipment.InternalVerification != null)
                {
                    if (this.Equipment.InternalVerification.Responsible != null)
                    {
                        responsibleId = this.Equipment.InternalVerification.Responsible.Id;
                    }
                }

                this.VerificationInternalCmbResponsible.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == responsibleId
                });
            }
        }
        #endregion
        #region External
        this.VerificationExternalTxtOperation = new FormText()
        {
            Name = "TxtVerificationExternalOperation",
            ColumnSpan = 9,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Operación",
            MaximumLength = 100,
            Placeholder = "Operación",
            Value = this.Equipment.ExternalVerification.Description,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.VerificationExternalTxtPeriodicity = new FormTextInteger()
        {
            Name = "TxtVerificationExternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Periodicidad",
            MaximumLength = 5,
            Placeholder = "Periodicidad",
            Value = this.Equipment.ExternalVerification.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.Dictionary["Common_Required"]
        };

        this.VerificationExternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtVerificationExternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00####}", this.Equipment.ExternalVerification.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalVerification.Uncertainty),
            Numeric = true,
            Nullable = true
        };

        this.VerificationExternalTxtRange = new FormText()
        {
            Name = "TxtVerificationExternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Range"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Range"],
            Value = this.Equipment.ExternalVerification.Range
        };

        this.VerificationExternalTxtPattern = new FormText()
        {
            Name = "TxtVerificationExternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Pattern"],
            MaximumLength = Constant.DefaultDatabaseVarChar,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Pattern"],
            Value = this.Equipment.ExternalVerification.Pattern
        };

        this.VerificationExternalTxtCost = new FormTextDecimal()
        {
            Name = "TxtVerificationExternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.Dictionary["Item_EquipmentVerification_Field_Cost"],
            MaximumLength = 8,
            Placeholder = this.Dictionary["Item_EquipmentVerification_Field_Cost"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.ExternalVerification.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalVerification.Cost),
            // ISSUS-18
            Required = false,
            Numeric = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Nullable = true
        };

        this.VerificationExternalTxtNotes = new FormTextArea()
        {
            Name = "TxtVerificationExternalNotes",
            Rows = 5,
            Value = this.Equipment.ExternalVerification.Notes,
            Label = "Notas"
        };

        this.VerificationExternalCmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 9,
            Name = "CmbVerificationExternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.Dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.Company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                long responsibleId = -1;
                if (this.Equipment.ExternalVerification != null)
                {
                    if (this.Equipment.ExternalVerification.Responsible != null)
                    {
                        responsibleId = this.Equipment.ExternalVerification.Responsible.Id;
                    }
                }

                this.VerificationExternalCmbResponsible.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == responsibleId
                });
            }
        }
        #endregion
        #endregion

        #region TabMaintenance
        this.MaintenanceNewConfiguration = new UIButton()
        {
            Action = "success",
            Icon = "icon-plus-sign",
            Id = "BtnNewMaintainment",
            Text = this.Dictionary["Item_Equipment_Maintenance_Button_AddConfiguration"]
        };

        this.MaintenanceNewAct = new UIButton()
        {
            Action = "success",
            Icon = "icon-plus-sign",
            Id = "BtnNewMaintainmentAct",
            Text = this.Dictionary["Item_Equipment_Maintenance_Button_AddAct"]
        };
        #endregion

    }

    private void RenderTabBasicData()
    {
        this.TxtCode = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_Code_Label"],
            ColumnSpan = 3,
            Placeholder = this.Dictionary["Item_Equipment_Field_Code_Placeholder"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtCode",
            Value = this.Equipment.Code,
            MaximumLength = 10
        };

        this.TxtDescription = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_Description_Label"],
            ColumnSpan = 6,
            Placeholder = this.Dictionary["Item_Equipment_Field_Description_Placeholder"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtDescription",
            Value = this.Equipment.Description,
            MaximumLength = 150
        };

        this.TxtTradeMark = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_TradeMark_Label"],
            ColumnSpan = 3,
            Placeholder = this.Dictionary["Item_Equipment_Field_TradeMark_PlaceHolder"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtTradeMark",
            Value = this.Equipment.Trademark,
            MaximumLength = 50
        };

        this.TxtModel = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_Model_Label"],
            ColumnSpan = 6,
            Placeholder = this.Dictionary["Item_Equipment_Field_Model_PlaceHolder"],
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtModel",
            Value = this.Equipment.Model,
            MaximumLength = 50
        };

        this.TxtSerialNumber = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_SerialNumber_Label"],
            ColumnSpan = 3,
            Placeholder = this.Dictionary["Item_Equipment_Field_SerialNumber_PlaceHolder"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtSerialNumber",
            Value = this.Equipment.SerialNumber,
            MaximumLength = 50
        };

        this.TxtLocation = new FormText
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_Location_Label"],
            ColumnSpan = 6,
            Placeholder = this.Dictionary["Item_Equipment_Field_Location_PlaceHolder"],
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtLocation",
            Value = this.Equipment.Location,
            MaximumLength = 100
        };

        this.TxtMeasureRange = new FormTextDecimal
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_Measure_Range_Label"],
            ColumnSpan = 2,
            Placeholder = this.Dictionary["Item_Equipment_Field_Measure_Range_PlaceHolder"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtMeasureRange",
            Value = this.Equipment.MeasureRange,
            MaximumLength = 10,
            Numeric = false
        };

        this.TxtScaleDivision = new FormTextFreeDecimal
        {
            ColumnSpanLabel = 1,
            Label = this.Dictionary["Item_Equipment_Field_ScaleDivision_Label"],
            ColumnSpan = 2,
            Placeholder = this.Dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"],
            GrantToWrite = this.grantToWrite,
            Name = "TxtScaleDivision",
            Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.0000}", this.Equipment.ScaleDivisionValue),
            MaximumLength = 10,
            Numeric = true
        };

        this.BarScaleDivisionType = new FormBar
        {
            ColumnSpan = 3,
            Name = "EquipmentScaleDivision",
            GrantToWrite = this.grantToWrite,
            GrantToEdit = this.grantToWrite,
            Value = this.Equipment.MeasureUnit == null ? string.Empty : string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}", this.Equipment.MeasureUnit.Id),
            ValueName = "TxtEquipmentScaleDivision",
            ButtonBar = "EquipmentScaleDivision",
            BarToolTip = "Editar división de escala"
        };

        this.EquipmentScaleDivisionBarPopups = new BarPopup
        {
            Id = "EquipmentScaleDivision",
            DeleteMessage = this.Dictionary["Item_Equipment_PopupTitle_DeleteScaleDivision"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Description = this.Dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"],
            FieldName = this.Dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"],
            BarTitle = this.Dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"]
        };

        this.ProviderBarPopups = new BarPopup
        {
            Id = "Provider",
            DeleteMessage = this.Dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.Dictionary["Common_Error_NameAlreadyExists"],
            Description = this.Dictionary["Item_Provider"],
            FieldName = this.Dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Providers"]
        };

        this.CmbResponsible = new FormSelect
        {
            ColumnsSpanLabel = Constant.ColumnSpan2,
            Label = this.Dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 3,
            Name = "CmbResponsible",
            ChangeEvent = "CmbResponsibleChanged();",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.Dictionary["Common_Required"],
            DefaultOption = new FormSelectOption
            {
                Text = this.Dictionary["Common_SelectAll"],
                Value = "0"
            }
        };

        foreach (var e in this.Company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsible.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Equipment.Responsible.Id
                });
            }
        }

        this.TxtNotes = new FormTextArea
        {
            Rows = 5,
            Value = this.Equipment.Notes,
            Name = "TxtNotes",
            MaxLength = 500
        };

        this.TxtObservations = new FormTextArea
        {
            Label = this.Dictionary["Item_Equipment_Field_Observations_Label"],
            Rows = 5,
            Value = this.Equipment.Observations,
            Name = "TxtObservations",
            MaxLength = 2000
        };

        this.TxtStartDate = new FormDatePicker
        {
            Id = "TxtStartDate",
            Label = this.Dictionary["Item_Equipment_Field_StartDate"],
            ColumnsSpanLabel = 1,
            ColumnsSpan = 3,
            Value = this.Equipment.StartDate
        };

        this.ImgEquipment = new ImageSelector
        {
            Name = "Equipment",
            ImageName = this.equipmentId > 0 ? @"images\equipments\" + this.Equipment.Image : @"images\equipments\noimage.jpg",
            Width = 150,
            Height = 150,
            Label = this.Dictionary["Item_Equipment_Field_Image_Label"]
        };

        this.status0.Checked = this.Equipment.IsCalibration;
        this.status1.Checked = this.Equipment.IsVerification;
        this.status2.Checked = this.Equipment.IsMaintenance;

        var responsibleData = new StringBuilder();
        foreach (var employee in this.Company.Employees.OrderBy(e => e.FullName))
        {
            responsibleData.Append(string.Format(@"<option value=""{0}""{1}>{2}</option>", employee.Id, employee.Id == this.Equipment.Responsible.Id ? " selected=\"selected\"" : string.Empty, employee.FullName));
        }

        this.ResponsibleData = responsibleData.ToString();

        this.CmbResponsibleClose = new FormSelect
        {
            ColumnsSpanLabel = 3,
            Label = this.Dictionary["Item_Equipment_FieldLabel_EndResponsible"],
            ColumnsSpan = 9,
            Name = "CmbEndResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo),
            DefaultOption = new FormSelectOption
            {
                Text = this.Dictionary["Common_SelectAll"],
                Value = "0"
            },
            RequiredMessage = this.Dictionary["Common_Required"],
            Required = true
        };

        long endResponsibleId = -1;
        if (this.Equipment.EndResponsible != null)
        {
            endResponsibleId = this.Equipment.EndResponsible.Id;
        }

        foreach (var e in this.Company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsibleClose.AddOption(new FormSelectOption
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == endResponsibleId
                });
            }
        }
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(ItemValues.Equipment, this.equipmentId, this.Company.Id);
        var res = new StringBuilder();
        var resList = new StringBuilder();
        int contCells = 0;
        var extensions = ToolsFile.ExtensionToShow;
        foreach (var file in files)
        {
            decimal finalSize = ToolsFile.FormatSize((decimal)file.Size);
            string fileShowed = string.IsNullOrEmpty(file.Description) ? file.FileName.Split('_')[2] : file.Description;
            if (fileShowed.Length > 25)
            {
                fileShowed = fileShowed.Substring(0, 25) + "...";
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
                            <div class=""col-sm-12"" style=""margin-bottom:8px;""><strong title=""{1}"">{9}</strong></div>
                            <div class=""col-sm-4""><img src=""/images/FileIcons/{2}.png"" /></div>
                            <div class=""col-sm-8 document-name"" style=""font-size:12px;"">   
                                {7}: <strong>{5:dd/MM/yyyy}</strong><br />
                                {8}: <strong>{6:#,##0.00} MB</strong>
                            </div>
                            <div class=""col-sm-12""></div>
                        </div>
                    </div>",
                    file.Id,
                    string.IsNullOrEmpty(file.Description) ? file.FileName : file.Description,
                    file.Extension,
                    this.Company.Id,
                    file.FileName,
                    file.CreatedOn,
                    finalSize,
                    this.Dictionary["Item_Attachment_Header_CreateDate"],
                    this.Dictionary["Item_Attachment_Header_Size"],
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
                this.Company.Id,
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