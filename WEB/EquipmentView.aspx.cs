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
using System.Linq;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;
using SbrinnaCoreFramework;
using System.IO;

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

    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    private string launch;

    public string Launch
    {
        get
        {
            return this.launch;
        }
    }

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
            return EquipmentCalibrationAct.JsonList(this.equipmentId, this.company.Id);
        }
    }

    public string OperationId { get; set; }

    public string EquipmentVerificationActList
    {
        get
        {
            return EquipmentVerificationAct.JsonList(this.equipmentId, this.company.Id);
        }
    }

    public string EquipmentRepairList
    {
        get
        {
            return EquipmentRepair.JsonList(this.equipmentId, this.company.Id);
        }
    }

    public string EquipmentMaintenanceDefinitionList
    {
        get
        {
            return EquipmentMaintenanceDefinition.JsonList(this.equipmentId, this.company.Id);
        }
    }

    public string EquipmentMaintenanceActList
    {
        get
        {
            return EquipmentMaintenanceAct.JsonList(this.equipmentId, this.company.Id);
        }
    }

    public string EquipmentScaleDivisionList
    {
        get
        {
            return EquipmentScaleDivision.JsonList("EquipmentScaleDivision", EquipmentScaleDivision.GetByCompany(this.company));
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

            return this.formFooter.Render(this.dictionary);
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

            return this.formFooterCalibration.Render(this.dictionary);
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

            return this.formFooterVerification.Render(this.dictionary);
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

            return this.formFooterMaintenance.Render(this.dictionary);
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

            return this.formFooterRepair.Render(this.dictionary);
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

            return this.formFooterRecords.Render(this.dictionary);
        }
    }

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
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

    TabBar tabBar = new TabBar() { Id = "EquipmentTabBar" };

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
            return Provider.GetByCompanyJson(this.company.Id);
        }
    }

    public string CustomersJson
    {
        get
        {
            return Customer.GetByCompanyJson(this.company.Id);
        }
    }

    public string EmployeesJson
    {
        get
        {
            return Employee.CompanyListJson(this.company.Id);
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
        if (this.Request.QueryString["id"] != null)
        {
            this.equipmentId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
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
        this.company = (Company)Session["company"];
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Equipments", "EquipmentList.aspx", false);
        this.master.AddBreadCrumb("Item_Equipment_Tab_Details");

        if (this.equipmentId > 0)
        {
            this.Session["EquipmentId"] = this.equipmentId;
            this.Equipment = Equipment.GetById(this.equipmentId, this.company);
            if (this.Equipment.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.Equipment = Equipment.Empty;
            }

            this.master.TitleInvariant = true;
            this.master.Titulo = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Equipment"], this.Equipment.Code + " - " + this.Equipment.Description);

            this.formFooter.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Equipment.ModifiedOn;
            this.formFooter.AddButton(new UIButton() { Id = "BtnPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton() { Id = "BtnRestaurar", Icon = "icon-undo", Text = this.dictionary["Item_Equipment_Btn_Restaurar"], Action = "primary" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Equipment_Btn_Anular"], Action = "danger" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success", ColumnsSpan = 12 });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"], ColumnsSpan = 12 });

            this.formFooterCalibration.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterCalibration.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterCalibration.AddButton(new UIButton() { Id = "BtnCalibrationPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterCalibration.AddButton(new UIButton() { Id = "BtnCalibrationSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterCalibration.AddButton(new UIButton() { Id = "BtnCalibrationCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooterVerification.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterVerification.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterVerification.AddButton(new UIButton() { Id = "BtnVerificationPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterVerification.AddButton(new UIButton() { Id = "BtnVerificationSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterVerification.AddButton(new UIButton() { Id = "BtnVerificationCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooterMaintenance.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterMaintenance.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterMaintenance.AddButton(new UIButton() { Id = "BtnMaintenancePrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterMaintenance.AddButton(new UIButton() { Id = "BtnMaintenanceSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterMaintenance.AddButton(new UIButton() { Id = "BtnMaintenanceCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooterRepair.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterRepair.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterRepair.AddButton(new UIButton() { Id = "BtnRepairPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterRepair.AddButton(new UIButton() { Id = "BtnRepairSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterRepair.AddButton(new UIButton() { Id = "BtnRepairCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.formFooterRecords.ModifiedBy = this.Equipment.ModifiedBy.Description;
            this.formFooterRecords.ModifiedOn = this.Equipment.ModifiedOn;
            //this.formFooterRecords.AddButton(new UIButton() { Id = "BtnRecordsPrint", Icon = "icon-file-pdf", Text = this.dictionary["Common_PrintPdf"], Action = "success", ColumnsSpan = 12 });
            //this.formFooterRecords.AddButton(new UIButton() { Id = "BtnRecordsSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterRecords.AddButton(new UIButton() { Id = "BtnRecordsCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

            this.master.ItemCode = string.Format("{0} - {1}", this.Equipment.Code, this.Equipment.Description);
            this.RenderDocuments();
        }
        else
        {
            this.master.Titulo = "Item_Equipment_Button_New";
            this.Equipment = Equipment.Empty;
            this.formFooter.ModifiedBy = this.dictionary["Common_New"];
            this.formFooter.ModifiedOn = DateTime.Now;
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }

        this.SelectedTab = "home";
        if (this.Request.QueryString["Tab"] != null)
        {
            SelectedTab = this.Request.QueryString["Tab"].ToString().Trim().ToLowerInvariant();
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Selected = SelectedTab == "home", Active = true, Label = this.dictionary["Item_Equipment_Tab_Basic"], Available = true });
        this.tabBar.AddTab(new Tab() { Id = "calibracion", Selected = SelectedTab == "calibracion", Available = true, Active = true, Label = this.dictionary["Item_Equipment_Tab_Calibration"] });
        this.tabBar.AddTab(new Tab() { Id = "verificacion", Selected = SelectedTab == "verificacion", Available = true, Active = true, Label = this.dictionary["Item_Equipment_Tab_Verification"] });
        this.tabBar.AddTab(new Tab() { Id = "mantenimiento", Selected = SelectedTab == "mantenimiento", Available = true, Active = true, Label = this.dictionary["Item_Equipment_Tab_Maintenance"] });
        this.tabBar.AddTab(new Tab() { Id = "reparaciones", Available = this.equipmentId > 0, Active = true, Label = this.dictionary["Item_Equipment_Tab_Repair"] });
        this.tabBar.AddTab(new Tab() { Id = "registros", Available = this.equipmentId > 0, Active = true, Label = this.dictionary["Item_Equipment_Tab_Records"] });
        this.tabBar.AddTab(new Tab() { Id = "uploadFiles", Selected = SelectedTab == "uploadfiles", Available = true, Active = this.equipmentId > 0, Hidden = this.equipmentId < 1, Label = this.Dictionary["Item_Equipment_Tab_UploadFiles"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Available = this.user.HasTraceGrant() && this.equipmentId > 0, Active = this.equipmentId > 0, Label = this.dictionary["Item_Equipment_Tab_Traces"] });

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
                string type = this.Request.QueryString["Type"].ToString();
                this.launch = "ShowDialogEquipmentVerificacionPopup(" + (type == "I" ? "-1" : "-2") + ");";
            }
        }

        if (SelectedTab.Equals("calibracion", StringComparison.OrdinalIgnoreCase))
        {
            if (this.Request.QueryString["Type"] != null)
            {
                string type = this.Request.QueryString["Type"].ToString();
                this.launch = "ShowDialogNewCalibrationPopup(" + (type == "I" ? "-1" : "-2") + ");";
            }
        }

        if(SelectedTab.Equals("tabuploadfiles", StringComparison.OrdinalIgnoreCase))
        {
            this.launch = @"$(""#TabuploadFiles a"").click();";
        }
    }

    private void RenderForm()
    {
        this.grantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Equipment);

        this.RenderTabBasicData();

        #region TabCalibration
        #region Internal
        this.CalibrationInternalTxtOperation = new FormText()
        {
            Name = "TxtCalibrationInternalOperation",
            ColumnSpan = 9,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Operation"],
            MaximumLength = 100,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Operation"],
            Value = this.Equipment.InternalCalibration.Description,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtPeriodicity = new FormTextInteger()
        {
            Name = "TxtCalibrationInternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Periodicity"],
            MaximumLength = 5,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Periodicity"],
            Value = this.Equipment.InternalCalibration.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtCalibrationInternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00######}", this.Equipment.InternalCalibration.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalCalibration.Uncertainty),
            Required = true,
            Numeric = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtRange = new FormText()
        {
            Name = "TxtCalibrationInternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Range"],
            MaximumLength = 50,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Range"],
            Value = this.Equipment.InternalCalibration.Range,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtPattern = new FormText()
        {
            Name = "TxtCalibrationInternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Pattern"],
            MaximumLength = 50,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Pattern"],
            Value = this.Equipment.InternalCalibration.Pattern,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtCost = new FormTextDecimal()
        {
            Name = "TxtCalibrationInternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Cost"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Cost"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.InternalCalibration.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.InternalCalibration.Cost),
            Numeric = true,
            // ISSUS-18
            Required = false,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationInternalTxtNotes = new FormTextArea()
        {
            Name = "TxtCalibrationInternalNotes",
            Rows = 5,
            Value = this.Equipment.InternalCalibration.Notes,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Notes"]
        };

        this.CalibrationInternalCmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationInternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.company.Employees)
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
        this.CalibrationExternalTxtOperation = new FormText()
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
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtPeriodicity = new FormTextInteger()
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
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtCalibrationExternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Uncertainty"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00####}", this.Equipment.ExternalCalibration.Uncertainty),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalCalibration.Uncertainty),
            Required = true,
            Numeric = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtRange = new FormText()
        {
            Name = "TxtCalibrationExternalRange",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Range"],
            MaximumLength = 50,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Range"],
            Value = this.Equipment.ExternalCalibration.Range,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtPattern = new FormText()
        {
            Name = "TxtCalibrationExternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Pattern"],
            MaximumLength = 50,
            Required = true,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Pattern"],
            Value = this.Equipment.ExternalCalibration.Pattern
        };

        this.CalibrationExternalTxtCost = new FormTextDecimal()
        {
            Name = "TxtCalibrationExternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Cost"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_Equipment_Field_Calibration_Cost"],
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.ExternalCalibration.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalCalibration.Cost),
            // ISSUS-18
            Required = false,
            Numeric = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.CalibrationExternalTxtNotes = new FormTextArea()
        {
            Name = "TxtCalibrationExternalNotes",
            Rows = 5,
            Value = this.Equipment.ExternalCalibration.Notes,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Notes"],
        };

        this.CalibrationExternalCmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Responsible"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationExternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        this.CalibrationExternalCmbProvider = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Equipment_Field_Calibration_Provider"],
            ColumnsSpan = 9,
            Name = "CmbCalibrationExternalProvider",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.company.Employees)
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

                this.CalibrationExternalCmbResponsible.AddOption(new FormSelectOption()
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

        foreach (Provider provider in Provider.GetByCompany(this.company.Id))
        {
            if (provider.Active)
            {
                this.CalibrationExternalCmbProvider.AddOption(new FormSelectOption()
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
            Label = this.dictionary["Item_EquipmentVerification_Field_Operation"],
            MaximumLength = 100,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Operation"],
            Value = this.Equipment.InternalVerification.Description,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.VerificationInternalTxtPeriodicity = new FormTextInteger()
        {
            Name = "TxtVerificationInternalPeriodicity",
            ColumnSpan = 2,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_EquipmentVerification_Field_Periodicity"],
            MaximumLength = 5,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Periodicity"],
            Value = this.Equipment.InternalVerification.Periodicity.ToString(),
            Required = true,
            Numeric = true,
            IsInteger = true,
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.VerificationInternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtVerificationInternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Uncertainty"],
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
            Label = this.dictionary["Item_EquipmentVerification_Field_Range"],
            MaximumLength = 50,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Range"],
            Value = this.Equipment.InternalVerification.Range
        };

        this.VerificationInternalTxtPattern = new FormText()
        {
            Name = "TxtVerificationInternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Patrón",
            MaximumLength = 50,
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
            RequiredMessage = this.dictionary["Common_Required"],
            Nullable = true
        };

        this.VerificationInternalTxtNotes = new FormTextArea()
        {
            Name = "TxtVerificationInternalNotes",
            Rows = 5,
            Value = this.Equipment.InternalVerification.Notes,
            Label = "Notas"
        };

        this.VerificationInternalCmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 9,
            Name = "CmbVerificationInternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.company.Employees.OrderBy(e => e.FullName))
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
            RequiredMessage = this.dictionary["Common_Required"]
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
            RequiredMessage = this.dictionary["Common_Required"]
        };

        this.VerificationExternalTxtUncertainty = new FormTextFreeDecimal()
        {
            Name = "TxtVerificationExternalUncertainty",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = this.dictionary["Item_EquipmentVerification_Field_Uncertainty"],
            MaximumLength = 8,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Uncertainty"],
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
            Label = this.dictionary["Item_EquipmentVerification_Field_Range"],
            MaximumLength = 50,
            Placeholder = this.dictionary["Item_EquipmentVerification_Field_Range"],
            Value = this.Equipment.ExternalVerification.Range
        };

        this.VerificationExternalTxtPattern = new FormText()
        {
            Name = "TxtVerificationExternalPattern",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Patrón",
            MaximumLength = 50,
            Placeholder = "Patrón",
            Value = this.Equipment.ExternalVerification.Pattern
        };

        this.VerificationExternalTxtCost = new FormTextDecimal()
        {
            Name = "TxtVerificationExternalCost",
            ColumnSpan = 3,
            ColumnSpanLabel = 3,
            GrantToWrite = this.grantToWrite,
            Label = "Coste",
            MaximumLength = 8,
            Placeholder = "Coste",
            //Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Equipment.ExternalVerification.Cost),
            Value = GisoFramework.Tools.NumberFormat(this.Equipment.ExternalVerification.Cost),
            // ISSUS-18
            Required = false,
            Numeric = true,
            RequiredMessage = this.dictionary["Common_Required"],
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
            Label = this.dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 9,
            Name = "CmbVerificationExternalResponsible",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.company.Employees.OrderBy(e => e.FullName))
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
            Text = this.dictionary["Item_Equipment_Maintenance_Button_AddConfiguration"]
        };

        this.MaintenanceNewAct = new UIButton()
        {
            Action = "success",
            Icon = "icon-plus-sign",
            Id = "BtnNewMaintainmentAct",
            Text = this.dictionary["Item_Equipment_Maintenance_Button_AddAct"]
        };
        #endregion

    }

    private void RenderTabBasicData()
    {
        this.TxtCode = new FormText();
        this.TxtCode.ColumnSpanLabel = 1;
        this.TxtCode.Label = this.dictionary["Item_Equipment_Field_Code_Label"];
        this.TxtCode.ColumnSpan = 3;
        this.TxtCode.Placeholder = this.dictionary["Item_Equipment_Field_Code_Placeholder"];
        this.TxtCode.Duplicated = true;
        this.TxtCode.DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"];
        this.TxtCode.Required = true;
        this.TxtCode.RequiredMessage = this.dictionary["Common_Required"];
        this.TxtCode.GrantToWrite = this.grantToWrite;
        this.TxtCode.Name = "TxtCode";
        this.TxtCode.Value = this.Equipment.Code;
        this.TxtCode.MaximumLength = 10;

        this.TxtDescription = new FormText();
        this.TxtDescription.ColumnSpanLabel = 1;
        this.TxtDescription.Label = this.dictionary["Item_Equipment_Field_Description_Label"];
        this.TxtDescription.ColumnSpan = 6;
        this.TxtDescription.Placeholder = this.dictionary["Item_Equipment_Field_Description_Placeholder"];
        this.TxtDescription.Duplicated = true;
        this.TxtDescription.DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"];
        this.TxtDescription.Required = true;
        this.TxtDescription.RequiredMessage = this.dictionary["Common_Required"];
        this.TxtDescription.GrantToWrite = this.grantToWrite;
        this.TxtDescription.Name = "TxtDescription";
        this.TxtDescription.Value = this.Equipment.Description;
        this.TxtDescription.MaximumLength = 150;

        this.TxtTradeMark = new FormText();
        this.TxtTradeMark.ColumnSpanLabel = 1;
        this.TxtTradeMark.Label = this.dictionary["Item_Equipment_Field_TradeMark_Label"];
        this.TxtTradeMark.ColumnSpan = 3;
        this.TxtTradeMark.Placeholder = this.dictionary["Item_Equipment_Field_TradeMark_PlaceHolder"];
        this.TxtTradeMark.GrantToWrite = this.grantToWrite;
        this.TxtTradeMark.Name = "TxtTradeMark";
        this.TxtTradeMark.Value = this.Equipment.Trademark;
        this.TxtTradeMark.MaximumLength = 50;

        this.TxtModel = new FormText();
        this.TxtModel.ColumnSpanLabel = 1;
        this.TxtModel.Label = this.dictionary["Item_Equipment_Field_Model_Label"];
        this.TxtModel.ColumnSpan = 6;
        this.TxtModel.Placeholder = this.dictionary["Item_Equipment_Field_Model_PlaceHolder"];
        this.TxtModel.Required = true;
        this.TxtModel.RequiredMessage = this.dictionary["Common_Required"];
        this.TxtModel.GrantToWrite = this.grantToWrite;
        this.TxtModel.Name = "TxtModel";
        this.TxtModel.Value = this.Equipment.Model;
        this.TxtModel.MaximumLength = 50;

        this.TxtSerialNumber = new FormText();
        this.TxtSerialNumber.ColumnSpanLabel = 1;
        this.TxtSerialNumber.Label = this.dictionary["Item_Equipment_Field_SerialNumber_Label"];
        this.TxtSerialNumber.ColumnSpan = 3;
        this.TxtSerialNumber.Placeholder = this.dictionary["Item_Equipment_Field_SerialNumber_PlaceHolder"];
        this.TxtSerialNumber.GrantToWrite = this.grantToWrite;
        this.TxtSerialNumber.Name = "TxtSerialNumber";
        this.TxtSerialNumber.Value = this.Equipment.SerialNumber;
        this.TxtSerialNumber.MaximumLength = 50;

        this.TxtLocation = new FormText();
        this.TxtLocation.ColumnSpanLabel = 1;
        this.TxtLocation.Label = this.dictionary["Item_Equipment_Field_Location_Label"];
        this.TxtLocation.ColumnSpan = 6;
        this.TxtLocation.Placeholder = this.dictionary["Item_Equipment_Field_Location_PlaceHolder"];
        this.TxtLocation.Required = true;
        this.TxtLocation.RequiredMessage = this.dictionary["Common_Required"];
        this.TxtLocation.GrantToWrite = this.grantToWrite;
        this.TxtLocation.Name = "TxtLocation";
        this.TxtLocation.Value = this.Equipment.Location;
        this.TxtLocation.MaximumLength = 100;

        this.TxtMeasureRange = new FormTextDecimal();
        this.TxtMeasureRange.ColumnSpanLabel = 1;
        this.TxtMeasureRange.Label = this.dictionary["Item_Equipment_Field_Measure_Range_Label"];
        this.TxtMeasureRange.ColumnSpan = 2;
        this.TxtMeasureRange.Placeholder = this.dictionary["Item_Equipment_Field_Measure_Range_PlaceHolder"];
        this.TxtMeasureRange.GrantToWrite = this.grantToWrite;
        this.TxtMeasureRange.Name = "TxtMeasureRange";
        this.TxtMeasureRange.Value = this.Equipment.MeasureRange;
        this.TxtMeasureRange.MaximumLength = 10;
        this.TxtMeasureRange.Numeric = false;

        this.TxtScaleDivision = new FormTextFreeDecimal();
        this.TxtScaleDivision.ColumnSpanLabel = 1;
        this.TxtScaleDivision.Label = this.dictionary["Item_Equipment_Field_ScaleDivision_Label"];
        this.TxtScaleDivision.ColumnSpan = 2;
        this.TxtScaleDivision.Placeholder = this.dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"];
        this.TxtScaleDivision.GrantToWrite = this.grantToWrite;
        this.TxtScaleDivision.Name = "TxtScaleDivision";
        this.TxtScaleDivision.Value = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.0000}", this.Equipment.ScaleDivisionValue);
        this.TxtScaleDivision.MaximumLength = 10;
        this.TxtScaleDivision.Numeric = true;

        this.BarScaleDivisionType = new FormBar();
        this.BarScaleDivisionType.ColumnSpan = 3;
        this.BarScaleDivisionType.Name = "EquipmentScaleDivision";
        this.BarScaleDivisionType.GrantToWrite = this.grantToWrite;
        this.BarScaleDivisionType.GrantToEdit = this.grantToWrite;
        this.BarScaleDivisionType.Value = this.Equipment.MeasureUnit == null ? string.Empty : string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}", this.Equipment.MeasureUnit.Id); this.BarScaleDivisionType.ValueName = "TxtEquipmentScaleDivision";
        this.BarScaleDivisionType.ButtonBar = "EquipmentScaleDivision";
        this.BarScaleDivisionType.BarToolTip = "Editar división de escala";

        this.EquipmentScaleDivisionBarPopups = new BarPopup()
        {
            Id = "EquipmentScaleDivision",
            DeleteMessage = this.dictionary["Item_Equipment_PopupTitle_DeleteScaleDivision"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
            Description = this.dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"],
            FieldName = this.dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"],
            BarTitle = this.dictionary["Item_Equipment_Field_ScaleDivision_PlaceHolder"]
        };

        this.ProviderBarPopups = new BarPopup()
        {
            Id = "Provider",
            DeleteMessage = this.dictionary["Common_DeleteMessage"],
            BarWidth = 600,
            UpdateWidth = 600,
            DeleteWidth = 600,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            Duplicated = true,
            DuplicatedMessage = this.dictionary["Common_Error_NameAlreadyExists"],
            Description = "Proveedor",
            FieldName = this.dictionary["Common_Name"],
            BarTitle = this.Dictionary["Item_Providers"]
        };

        this.CmbResponsible = new FormSelect()
        {
            ColumnsSpanLabel = 2,
            Label = this.dictionary["Item_Equipment_Field_Responsible_Label"],
            ColumnsSpan = 8,
            Name = "CmbResponsible",
            ChangeEvent = "CmbResponsibleChanged();",
            GrantToWrite = this.grantToWrite,
            Required = true,
            RequiredMessage = this.dictionary["Common_Required"],
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" }
        };

        foreach (Employee e in this.company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsible.AddOption(new FormSelectOption()
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName,
                    Selected = e.Id == this.Equipment.Responsible.Id
                });
            }
        }

        this.TxtNotes = new FormTextArea()
        {
            Rows = 5,
            Value = this.Equipment.Notes,
            Name = "TxtNotes",
            MaxLength = 500
        };

        this.TxtObservations = new FormTextArea()
        {
            Label = this.dictionary["Item_Equipment_Field_Observations_Label"],
            Rows = 5,
            Value = this.Equipment.Observations,
            Name = "TxtObservations",
            MaxLength = 2000
        };

        this.TxtStartDate = new FormDatePicker()
        {
            Id = "TxtStartDate",
            Label = this.dictionary["Item_Equipment_Field_StartDate"],
            ColumnsSpanLabel = 1,
            ColumnsSpan = 3,
            Value = this.Equipment.StartDate
        };

        this.ImgEquipment = new ImageSelector()
        {
            Name = "Equipment",
            ImageName = this.equipmentId > 0 ? @"images\equipments\" + this.Equipment.Image : @"images\equipments\noimage.jpg",
            Width = 150,
            Height = 150,
            Label = this.dictionary["Item_Equipment_Field_Image_Label"]
        };

        this.status0.Checked = this.Equipment.IsCalibration;
        this.status1.Checked = this.Equipment.IsVerification;
        this.status2.Checked = this.Equipment.IsMaintenance;

        StringBuilder responsibleData = new StringBuilder();
        foreach (Employee employee in this.company.Employees.OrderBy(e => e.FullName))
        {
            responsibleData.Append(string.Format(@"<option value=""{0}""{1}>{2}</option>", employee.Id, employee.Id == this.Equipment.Responsible.Id ? " selected=\"selected\"" : string.Empty, employee.FullName));
        }

        this.ResponsibleData = responsibleData.ToString();



        this.CmbResponsibleClose = new FormSelect()
        {
            ColumnsSpanLabel = 3,
            Label = this.dictionary["Item_Equipment_FieldLabel_EndResponsible"],
            ColumnsSpan = 9,
            Name = "CmbEndResponsible",
            GrantToWrite = this.user.HasGrantToWrite(ApplicationGrant.Objetivo),
            DefaultOption = new FormSelectOption() { Text = this.dictionary["Common_SelectAll"], Value = "0" },
            RequiredMessage = this.dictionary["Common_Required"],
            Required = true
        };

        long endResponsibleId = -1;
        if (this.Equipment.EndResponsible != null)
        {
            endResponsibleId = this.Equipment.EndResponsible.Id;
        }

        foreach (Employee e in this.company.Employees.OrderBy(e => e.FullName))
        {
            if (e.Active && e.DisabledDate == null)
            {
                this.CmbResponsibleClose.AddOption(new FormSelectOption()
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

        ReadOnlyCollection<UploadFile> files = UploadFile.GetByItem(11, this.equipmentId, this.company.Id);
        StringBuilder res = new StringBuilder();
        StringBuilder resList = new StringBuilder();
        int contCells = 0;
        ReadOnlyCollection<string> extensions = ToolsFile.ExtensionToShow;
        foreach (UploadFile file in files)
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