// --------------------------------
// <copyright file="EmployeesView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework.UI;

public partial class EmployeesView : Page
{
    /// <summary>Master of page</summary>
    private Giso master;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Application user logged in session</summary>
    private ApplicationUser user;

    /// <summary>Gets a random value to prevents static cache files</summary>
    public string AntiCache
    {
        get
        {
            return Guid.NewGuid().ToString().ToUpperInvariant();
        }
    }

    /// <summary>Gets or sets if user show help in interface</summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string JobsPositionCompany
    {
        get
        {
            return JobPosition.ByCompanyJson(this.company.Id);
        }
    }

    public string SelectedTab { get; set; }

    public bool Active { get; private set; }

    TabBar tabBar = new TabBar { Id = "EmployeeTabBar" };

    public string TabBar
    {
        get
        {
            return this.tabBar.Render;
        }
    }

    public string SkillAcademicValid
    {
        get
        {
            if (this.Employee.Id > 0)
            {
                if (this.Employee.EmployeeSkills.AcademicValid.HasValue)
                {
                    return this.Employee.EmployeeSkills.AcademicValid.Value ? "true" : "false";
                }
                else
                {
                    return "null";
                }
            }

            return "null";
        }
    }

    public string SkillHabilityValid
    {
        get
        {
            if (this.Employee.Id > 0)
            {
                if (this.Employee.EmployeeSkills.AbilityValid.HasValue)
                {
                    return this.Employee.EmployeeSkills.AbilityValid.Value ? "true" : "false";
                }
                else
                {
                    return "null";
                }
            }

            return "null";
        }
    }

    public string SkillSpecificValid
    {
        get
        {
            if (this.Employee.Id > 0)
            {
                if (this.Employee.EmployeeSkills.SpecificValid.HasValue)
                {
                    return this.Employee.EmployeeSkills.SpecificValid.Value ? "true" : "false";
                }
                else
                {
                    return "null";
                }
            }

            return "null";
        }
    }

    public string SkillWorkExperienceValid
    {
        get
        {
            if (this.Employee.Id > 0)
            {
                if (this.Employee.EmployeeSkills.WorkExperienceValid.HasValue)
                {
                    return this.Employee.EmployeeSkills.WorkExperienceValid.Value ? "true" : "false";
                }
                else
                {
                    return "null";
                }
            }

            return "null";
        }
    }

    private int employeeId;
    private string userName;
    private string returnScript;

    private StringBuilder jobPositionAcademic;
    private StringBuilder jobPositionSpecific;
    private StringBuilder jobPositionWorkExperience;
    private StringBuilder jobPositionHability;

    public string JobPositionAcademic { get { return this.jobPositionAcademic.ToString(); } }
    public string JobPositionSpecific { get { return this.jobPositionSpecific.ToString(); } }
    public string JobPositionWorkExperience { get { return this.jobPositionWorkExperience.ToString(); } }
    public string JobPositionHability { get { return this.jobPositionHability.ToString(); } }

    public string JobPositionAcademicValid
    {
        get
        {
            if (this.Employee.EmployeeSkills.AcademicValid.HasValue)
            {
                return this.Employee.EmployeeSkills.AcademicValid.Value ? this.Dictionary["Common_Yes"] : this.Dictionary["Common_No"];
            }

            return this.Dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionSpecificValid
    {
        get
        {
            if (this.Employee.EmployeeSkills.SpecificValid.HasValue)
            {
                return this.Employee.EmployeeSkills.SpecificValid.Value ? this.Dictionary["Common_Yes"] : this.Dictionary["Common_No"];
            }

            return this.Dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionWorkExperienceValid
    {
        get
        {
            if (this.Employee.EmployeeSkills.WorkExperienceValid.HasValue)
            {
                return this.Employee.EmployeeSkills.WorkExperienceValid.Value ? this.Dictionary["Common_Yes"] : this.Dictionary["Common_No"];
            }

            return this.Dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionHabilityValid
    {
        get
        {
            if (this.Employee.EmployeeSkills.AbilityValid.HasValue)
            {
                return this.Employee.EmployeeSkills.AbilityValid.Value ? this.Dictionary["Common_Yes"] : this.Dictionary["Common_No"];
            }

            return this.Dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    private FormFooter formFooter;
    private FormFooter formFooterLearning;
    private FormFooter formFooterInternalLearning;

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

    public string FormFooterLearning
    {
        get
        {
            return this.formFooterLearning.Render(this.Dictionary);
        }
    }

    public string FormFooterInternalLearning
    {
        get
        {
            return this.formFooterInternalLearning.Render(this.Dictionary);
        }
    }

    public string ReturnScript
    {
        get
        {
            return this.returnScript;
        }
    }

    private StringBuilder departmentsEmployeeJson;
    private StringBuilder jobpositionEmployeeJson;
    private long employeeUserId;

    private StringBuilder companyUserNames;
    private string countryData;

    public string CountryData
    {
        get
        {
            return this.countryData;
        }
    }

    public string CompanyUserNames
    {
        get
        {
            return this.companyUserNames.ToString();
        }
    }

    public string UserName
    {
        get
        {
            return this.userName;
        }
    }

    public string EmployeeUserId
    {
        get
        {
            return this.employeeUserId.ToString(CultureInfo.InvariantCulture);
        }
    }

    public string GroupsJson
    {
        get
        {
            if (this.Employee.User == null)
            {
                return Constant.EmptyJsonList;
            }

            return this.Employee.User.GroupsJson;
        }
    }

    public int EmployeeId
    {
        get
        {
            return this.employeeId;
        }
    }

    public string JobPositionEmployeeJson
    {
        get
        {
            return this.jobpositionEmployeeJson.ToString();
        }
    }

    public string DepartmentsEmployeeJson
    {
        get
        {
            return this.departmentsEmployeeJson.ToString();
        }
    }

    public Employee Employee { get; private set; }

    public string EmployeeSkills
    {
        get
        {
            return this.Employee.EmployeeSkills.Json;
        }
    }

    /// <summary>Gets dictionary for fixed labels</summary>
    public Dictionary<string, string> Dictionary { get; private set; }

    /// <summary>Page's load event</summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Active = true;
        if (Session["User"] == null)
        {
             this.Response.Redirect("Default.aspx", Constant.EndResponse);
        }
        else
        {
            if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
            }
            else
            {
                int test = 0;
                if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
                {
                    this.Response.Redirect("NoAccesible.aspx", Constant.EndResponse);
                }
            }

            this.Go();
            Context.ApplicationInstance.CompleteRequest();
        }
    }

    /// <summary>Begin page running after session validations</summary>
    private void Go()
    {
        if (this.Request.QueryString["id"] != null)
        {
            this.employeeId = Convert.ToInt32(this.Request.QueryString["id"].ToString());
        }

        if (this.Request.QueryString["New"] != null)
        {
            this.returnScript = "document.location = 'EmployeesList.aspx';";
        }
        else
        {
            this.returnScript = "document.location = referrer;";
        }

        this.formFooter = new FormFooter();
        this.formFooterLearning = new FormFooter();
        this.formFooterInternalLearning = new FormFooter();

        this.user = (ApplicationUser)Session["User"];
        this.company = (Company)Session["company"];
        this.Dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = this.employeeId == -1 ? "Item_Employee_Button_New" : "Item_Employee_Title_EmployeeData";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Employees", "EmployeesList.aspx", false);
        this.master.AddBreadCrumb("Item_Employee_Title_EmployeeData");
        this.master.TitleInvariant = true;
        this.master.Titulo = this.Dictionary["Item_Employee_Title_EmployeeData"];

        if (employeeId > 0)
        {
            bool grantDelete = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Employee);
            bool grantJobPositionView = UserGrant.HasReadGrant(this.user.Grants, ApplicationGrant.JobPosition);
            bool grantDepartmentsView = UserGrant.HasReadGrant(this.user.Grants, ApplicationGrant.Department);

            this.Employee = new Employee(this.employeeId, true);
            if (this.Employee.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.Employee = Employee.Empty;
            }

            if (this.Employee.DisabledDate.HasValue)
            {
                this.Active = false;
            }


            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.Dictionary["Item_Employee"], Employee.FullName);

            this.master.Titulo = label;

            this.departmentsEmployeeJson = new StringBuilder("[").Append(Environment.NewLine);
            this.departmentsEmployeeJson.Append(Environment.NewLine).Append("\t]");

            this.jobPositionAcademic = new StringBuilder();
            this.jobPositionSpecific = new StringBuilder();
            this.jobPositionWorkExperience = new StringBuilder();
            this.jobPositionHability = new StringBuilder();

            var tableJobAsignements = new StringBuilder();
            this.jobpositionEmployeeJson = new StringBuilder("[");
            bool firstJobPosition = true;
            this.Employee.ObtainJobPositionsHistoric();
            var JobPositionAdded = new List<long>();
            foreach (var jobAsignement in this.Employee.JobPositionAssignment)
            {
                if (!JobPositionAdded.Contains(jobAsignement.JobPosition.Id) || true)
                {
                    JobPositionAdded.Add(jobAsignement.JobPosition.Id);
                    var actualJobPosition = new JobPosition(jobAsignement.JobPosition.Id, this.company.Id);

                    if (firstJobPosition)
                    {
                        firstJobPosition = false;
                    }
                    else
                    {
                        this.jobpositionEmployeeJson.Append(",");
                    }

                    this.jobpositionEmployeeJson.Append(jobAsignement.Json);

                    if (!string.IsNullOrEmpty(actualJobPosition.AcademicSkills))
                    {
                        if (this.jobPositionAcademic.ToString().IndexOf(actualJobPosition.AcademicSkills) == -1)
                        {
                            this.jobPositionAcademic.Append(actualJobPosition.AcademicSkills);
                            this.jobPositionAcademic.Append(Environment.NewLine);
                        }
                    }

                    if (!string.IsNullOrEmpty(actualJobPosition.SpecificSkills))
                    {
                        if (this.jobPositionSpecific.ToString().IndexOf(actualJobPosition.SpecificSkills) == -1)
                        {
                            this.jobPositionSpecific.Append(actualJobPosition.SpecificSkills);
                            this.jobPositionSpecific.Append(Environment.NewLine);
                        }
                    }

                    if (!string.IsNullOrEmpty(actualJobPosition.WorkExperience))
                    {
                        if (this.jobPositionWorkExperience.ToString().IndexOf(actualJobPosition.WorkExperience) == -1)
                        {
                            this.jobPositionWorkExperience.Append(actualJobPosition.WorkExperience);
                            this.jobPositionWorkExperience.Append(Environment.NewLine);
                        }
                    }

                    if (!string.IsNullOrEmpty(actualJobPosition.Habilities))
                    {
                        if (this.jobPositionHability.ToString().IndexOf(actualJobPosition.Habilities) == -1)
                        {
                            this.jobPositionHability.Append(actualJobPosition.Habilities);
                            this.jobPositionHability.Append(Environment.NewLine);
                        }
                    }

                    tableJobAsignements.Append(jobAsignement.TableRow(this.Dictionary, grantDelete, grantJobPositionView, grantDepartmentsView));
                }
            }

            this.jobpositionEmployeeJson.Append("]");
            this.TableExperiencia.Text = tableJobAsignements.ToString();

            this.companyUserNames = new StringBuilder();
            bool firstUserName = true;
            foreach (var userName in ApplicationUser.CompanyUserNames(this.company.Id))
            {
                if (firstUserName)
                {
                    firstUserName = false;
                }
                else
                {
                    this.companyUserNames.Append(",");
                }

                this.companyUserNames.Append(string.Format(@"{{""UserName"":""{0}"",""EmployeeId"":{1}}}", userName.Key, userName.Value));
            }

            var tableAssistance = new StringBuilder();
            this.Employee.ObtainLearningAssistance();
            foreach (LearningAssistance assistance in this.Employee.LearningAssistance)
            {
                tableAssistance.Append(assistance.TableRowProfile(this.Dictionary));
            }

            this.TableLearningAssistance.Text = tableAssistance.ToString();

            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.employeeId, TargetType.Employee);

            this.Employee.ObtainEmployeeSkills();

            this.formFooter.ModifiedBy = this.Employee.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.Employee.ModifiedOn;

            if (this.Employee.EmployeeSkills != null && this.Employee.EmployeeSkills.ModifiedBy != null)
            {
                this.formFooterLearning.ModifiedBy = this.Employee.EmployeeSkills.ModifiedBy.Description;
                this.formFooterLearning.ModifiedOn = this.Employee.EmployeeSkills.ModifiedOn;
            }

            if (this.Employee.User == null)
            {
                this.Employee.User = ApplicationUser.GetByEmployee(this.Employee.Id, this.Employee.CompanyId);
                this.employeeUserId = this.Employee.User.Id;
                this.userName = this.Employee.User.UserName;
            }

            this.RenderDocuments();
        }
        else
        {
            this.Employee = Employee.Empty;
            this.departmentsEmployeeJson = new StringBuilder(Constant.EmptyJsonList);
            this.jobpositionEmployeeJson = new StringBuilder(Constant.EmptyJsonList);
            this.employeeUserId = -1;
            this.userName = string.Empty;
            this.companyUserNames = new StringBuilder();
            this.Employee.Address = EmployeeAddress.Empty;
        }

        this.RenderCountries();
        if (this.Active)
        {
            if (employeeId > 0)
            {
                this.formFooter.AddButton(new UIButton { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.Dictionary["Item_Employee_Btn_Inactive"], Action = "danger" });
            }
            this.formFooter.AddButton(new UIButton { Id = "BtnSave", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
        }
        else
        {
            this.formFooter.AddButton(new UIButton { Id = "BtnRestore", Icon = "icon-undo", Text = this.Dictionary["Item_Employee_Button_Restore"], Action = "primary" });
            //this.formFooter.AddButton(new UIButton { Id = "BtnRestore", Icon = "icon-ok", Text = this.Dictionary["Item_Employee_Button_Restore"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton { Id = "BtnCancel", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });

        if (this.Active)
        {
            this.formFooterInternalLearning.ModifiedBy = this.formFooter.ModifiedBy;
            this.formFooterInternalLearning.ModifiedOn = this.formFooter.ModifiedOn;
            this.formFooterLearning.AddButton(new UIButton { Id = "BtnSaveFormacion", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterLearning.AddButton(new UIButton { Id = "BtnCancelFormacion", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
            this.formFooterInternalLearning.AddButton(new UIButton { Id = "BtnSaveInternalLearning", Icon = "icon-ok", Text = this.Dictionary["Common_Accept"], Action = "success" });
            this.formFooterInternalLearning.AddButton(new UIButton { Id = "BtnCancelInternalLearning", Icon = "icon-undo", Text = this.Dictionary["Common_Cancel"] });
        }

        this.SelectedTab = "home";
        if (this.Request.QueryString["Tab"] != null)
        {
            SelectedTab = this.Request.QueryString["Tab"].ToString().Trim().ToLowerInvariant();
        }

        this.tabBar.AddTab(new Tab { Id = "home", Selected = SelectedTab == "home", Active = true, Label = this.Dictionary["Item_Employee_Tab_Principal"], Available = true });
        this.tabBar.AddTab(new Tab { Id = "formacion", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId <1, Label = this.Dictionary["Item_Employee_Tab_Learning"] });
        this.tabBar.AddTab(new Tab { Id = "formacionInterna", Selected = SelectedTab == "formacioninterna", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId < 1, Label = this.Dictionary["Item_Employee_Tab_InternalLearning"] });
        this.tabBar.AddTab(new Tab { Id = "uploadFiles", Selected = SelectedTab == "uploadfiles", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId < 1, Label = this.Dictionary["Item_Employee_Tab_UploadFiles"] });
        //// this.tabBar.AddTab(new Tab { Id = "trazas", Available = this.user.HasTraceGrant() && this.employeeId > 0, Active = this.employeeId > 0, Label = this.dictionary["Item_Employee_Tab_Traces"] });
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        var files = UploadFile.GetByItem(ItemIdentifiers.Employee, this.employeeId, this.company.Id);
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
                    this.Dictionary["Item_Attachment_Header_Size"],
                    fileShowed,
                    viewButton);

            resList.AppendFormat(
                CultureInfo.InvariantCulture,
                @"<tr>
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

    /// <summary>Generates JSON list of selected countries for the company</summary>
    private void RenderCountries()
    {
        var res = new StringBuilder();
        string countryCompare = this.Employee.Id < 0 ? this.Dictionary["Common_None"] : this.Employee.Address.Country;
        res.Append(string.Format(@"{{""text"":""{0}"",""value"":0,""selected"":{1},""description"":""{0}""}}", this.Dictionary["Common_None"], countryCompare == this.Dictionary["Common_None"] ? "true" : "false"));

        switch (this.user.Language)
        {
            case "es":
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Cataluña", "Cataluña", 45);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "España", "España", 1);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "Francia", "Francia", 7);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Gran Bretaña", "Great Britain", 12);
                break;
            case "ca":
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalunya", "Cataluña", 45);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Espanya", "España", 1);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "França", "Francia", 7);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Gran Bretanya", "Gran Bretanya", 12);
                break;
            case "en":
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalonia", "Cataluña", 45);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Spain", "España", 1);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Basque Country", "Euskal Herria", 46);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "France", "Francia", 7);
                res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Great Britain" ? "true" : "false", "Great Britain", "Great Britain", 12);
                break;
        }

        this.countryData = res.ToString();
        this.LtCountries.Text = res.ToString();
    }
}