// --------------------------------
// <copyright file="EmployeesView.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------

using System;
using System.Collections.Generic;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Item;
using System.Globalization;
using System.Text;
using GisoFramework.Activity;
using System.Collections.ObjectModel;
using SbrinnaCoreFramework.UI;
using System.IO;

public partial class EmployeesView : Page
{
    /// <summary>
    /// Master of page
    /// </summary>
    private Giso master;

    /// <summary>
    /// Company of session
    /// </summary>
    private Company company;

    /// <summary>
    /// Application user logged in session
    /// </summary>
    private ApplicationUser user;

    /// <summary>
    /// Dictionary for fixed labels
    /// </summary>
    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Indicates if employee is active
    /// </summary>
    private bool active;

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
    /// Gets or sets if user show help in interface
    /// </summary>
    public bool ShowHelp
    {
        get
        {
            return this.user.ShowHelp;
        }
    }

    public string SelectedTab { get; set; }

    public bool Active
    {
        get
        {
            return this.active;
        }
    }

    TabBar tabBar = new TabBar() { Id = "EmployeeTabBar" };

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
            if (this.employee.Id > 0)
            {
                if (this.employee.EmployeeSkills.AcademicValid.HasValue)
                {
                    return this.employee.EmployeeSkills.AcademicValid.Value ? "true" : "false";
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
            if (this.employee.Id > 0)
            {
                if (this.employee.EmployeeSkills.AbilityValid.HasValue)
                {
                    return this.employee.EmployeeSkills.AbilityValid.Value ? "true" : "false";
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
            if (this.employee.Id > 0)
            {
                if (this.employee.EmployeeSkills.SpecificValid.HasValue)
                {
                    return this.employee.EmployeeSkills.SpecificValid.Value ? "true" : "false";
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
            if (this.employee.Id > 0)
            {
                if (this.employee.EmployeeSkills.WorkExperienceValid.HasValue)
                {
                    return this.employee.EmployeeSkills.WorkExperienceValid.Value ? "true" : "false";
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
    private Employee employee;
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
            if (this.employee.EmployeeSkills.AcademicValid.HasValue)
            {
                return this.employee.EmployeeSkills.AcademicValid.Value ? this.dictionary["Common_Yes"] : this.dictionary["Common_No"];
            }

            return this.dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionSpecificValid
    {
        get
        {
            if (this.employee.EmployeeSkills.SpecificValid.HasValue)
            {
                return this.employee.EmployeeSkills.SpecificValid.Value ? this.dictionary["Common_Yes"] : this.dictionary["Common_No"];
            }

            return this.dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionWorkExperienceValid
    {
        get
        {
            if (this.employee.EmployeeSkills.WorkExperienceValid.HasValue)
            {
                return this.employee.EmployeeSkills.WorkExperienceValid.Value ? this.dictionary["Common_Yes"] : this.dictionary["Common_No"];
            }

            return this.dictionary["Item_Employee_Skills_Status_Undefined"];
        }
    }

    public string JobPositionHabilityValid
    {
        get
        {
            if (this.employee.EmployeeSkills.AbilityValid.HasValue)
            {
                return this.employee.EmployeeSkills.AbilityValid.Value ? this.dictionary["Common_Yes"] : this.dictionary["Common_No"];
            }

            return this.dictionary["Item_Employee_Skills_Status_Undefined"];
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

            return this.formFooter.Render(this.dictionary);
        }
    }

    public string FormFooterLearning
    {
        get
        {
            return this.formFooterLearning.Render(this.dictionary);
        }
    }

    public string FormFooterInternalLearning
    {
        get
        {
            return this.formFooterInternalLearning.Render(this.dictionary);
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
            return this.employeeUserId.ToString(CultureInfo.GetCultureInfo("en-us"));
        }
    }

    public string GroupsJson
    {
        get
        {
            if (this.employee.User == null)
            {
                return "[]";
            }

            return this.employee.User.GroupsJson;
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

    public Employee Employee
    {
        get
        {
            return this.employee;
        }
    }

    public string EmployeeSkills
    {
        get
        {
            return this.employee.EmployeeSkills.Json;
        }
    }

    /// <summary>
    /// Gets the identifier of employee showed in page
    /// </summary>
    public string EmployeeJson
    {
        get
        {
            return this.employee.Json;
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

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        this.active = true;
        if (Session["User"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            if (this.Request.QueryString["id"] == null)
            {
                this.Response.Redirect("NoAccesible.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                int test = 0;
                if (!int.TryParse(this.Request.QueryString["id"].ToString(), out test))
                {
                    this.Response.Redirect("NoAccesible.aspx", true);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }

            this.Go();
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
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
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        string label = this.employeeId == -1 ? "Item_Employee_Button_New" : "Item_Employee_Title_EmployeeData";
        this.master = this.Master as Giso;
        this.master.AdminPage = true;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Employees", "EmployeesList.aspx", false);
        this.master.AddBreadCrumb(label);
        this.master.TitleInvariant = true;
        this.master.Titulo = this.dictionary["Item_Employee_Title_EmployeeData"];

        if (employeeId > 0)
        {
            bool grantDelete = UserGrant.HasWriteGrant(this.user.Grants, ApplicationGrant.Employee);
            bool grantJobPositionView = UserGrant.HasReadGrant(this.user.Grants, ApplicationGrant.JobPosition);
            bool grantDepartmentsView = UserGrant.HasReadGrant(this.user.Grants, ApplicationGrant.Department);

            this.employee = new Employee(this.employeeId, true);
            if (this.employee.CompanyId != this.company.Id)
            {
                this.Response.Redirect("NoAccesible.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                this.employee = Employee.Empty;
            }

            if (this.employee.DisabledDate.HasValue)
            {
                this.active = false;
            }


            label = string.Format(CultureInfo.InvariantCulture, "{0}: <strong>{1}</strong>", this.dictionary["Item_Employee"], employee.FullName);

            this.master.Titulo = label;

            this.departmentsEmployeeJson = new StringBuilder("[").Append(Environment.NewLine);
            this.departmentsEmployeeJson.Append(Environment.NewLine).Append("\t]");

            this.jobPositionAcademic = new StringBuilder();
            this.jobPositionSpecific = new StringBuilder();
            this.jobPositionWorkExperience = new StringBuilder();
            this.jobPositionHability = new StringBuilder();

            StringBuilder tableJobAsignements = new StringBuilder();
            this.jobpositionEmployeeJson = new StringBuilder("[");
            bool firstJobPosition = true;
            this.employee.ObtainJobPositionsHistoric();
            List<long> JobPositionAdded = new List<long>();
            foreach (JobPositionAsigment jobAsignement in this.employee.JobPositionAssignment)
            {
                if (!JobPositionAdded.Contains(jobAsignement.JobPosition.Id) || true)
                {
                    JobPositionAdded.Add(jobAsignement.JobPosition.Id);
                    JobPosition actualJobPosition = new JobPosition(jobAsignement.JobPosition.Id, this.company.Id);

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

                    tableJobAsignements.Append(jobAsignement.TableRow(this.dictionary, grantDelete, grantJobPositionView, grantDepartmentsView));
                }
            }

            this.jobpositionEmployeeJson.Append("]");
            this.TableExperiencia.Text = tableJobAsignements.ToString();

            this.companyUserNames = new StringBuilder();
            bool firstUserName = true;
            foreach (KeyValuePair<string, int> userName in ApplicationUser.CompanyUserNames(this.company.Id))
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

            StringBuilder tableAssistance = new StringBuilder();
            this.employee.ObtainLearningAssistance();
            foreach (LearningAssistance assistance in this.employee.LearningAssistance)
            {
                tableAssistance.Append(assistance.TableRowProfile(this.dictionary));
            }

            this.TableLearningAssistance.Text = tableAssistance.ToString();

            this.LtTrazas.Text = ActivityTrace.RenderTraceTableForItem(this.employeeId, TargetType.Employee);

            this.employee.ObtainEmployeeSkills();

            this.formFooter.ModifiedBy = this.employee.ModifiedBy.Description;
            this.formFooter.ModifiedOn = this.employee.ModifiedOn;

            if (this.employee.EmployeeSkills != null && this.employee.EmployeeSkills.ModifiedBy != null)
            {
                this.formFooterLearning.ModifiedBy = this.employee.EmployeeSkills.ModifiedBy.Description;
                this.formFooterLearning.ModifiedOn = this.employee.EmployeeSkills.ModifiedOn;
            }

            if (this.employee.User == null)
            {
                this.employee.User = ApplicationUser.GetByEmployee(this.employee.Id, this.employee.CompanyId);
                this.employeeUserId = this.employee.User.Id;
                this.userName = this.employee.User.UserName;
            }

            this.RenderDocuments();
        }
        else
        {
            this.employee = Employee.Empty;
            this.departmentsEmployeeJson = new StringBuilder("[]");
            this.jobpositionEmployeeJson = new StringBuilder("[]");
            this.employeeUserId = -1;
            this.userName = string.Empty;
            this.companyUserNames = new StringBuilder();
            this.employee.Address = EmployeeAddress.Empty;
        }

        this.RenderCountries();
        if (this.active)
        {
            this.formFooter.AddButton(new UIButton() { Id = "BtnAnular", Icon = "icon-ban-circle", Text = this.dictionary["Item_Employee_Btn_Inactive"], Action = "danger" });
            this.formFooter.AddButton(new UIButton() { Id = "BtnSave", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
        }
        else
        {
            this.formFooter.AddButton(new UIButton() { Id = "BtnRestore", Icon = "icon-undo", Text = this.dictionary["Item_Employee_Button_Restore"], Action = "primary" });
            //this.formFooter.AddButton(new UIButton() { Id = "BtnRestore", Icon = "icon-ok", Text = this.dictionary["Item_Employee_Button_Restore"], Action = "success" });
        }

        this.formFooter.AddButton(new UIButton() { Id = "BtnCancel", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });

        if (this.active)
        {
            this.formFooterInternalLearning.ModifiedBy = this.formFooter.ModifiedBy;
            this.formFooterInternalLearning.ModifiedOn = this.formFooter.ModifiedOn;
            this.formFooterLearning.AddButton(new UIButton() { Id = "BtnSaveFormacion", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterLearning.AddButton(new UIButton() { Id = "BtnCancelFormacion", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
            this.formFooterInternalLearning.AddButton(new UIButton() { Id = "BtnSaveInternalLearning", Icon = "icon-ok", Text = this.dictionary["Common_Accept"], Action = "success" });
            this.formFooterInternalLearning.AddButton(new UIButton() { Id = "BtnCancelInternalLearning", Icon = "icon-undo", Text = this.dictionary["Common_Cancel"] });
        }

        this.SelectedTab = "home";
        if (this.Request.QueryString["Tab"] != null)
        {
            SelectedTab = this.Request.QueryString["Tab"].ToString().Trim().ToLowerInvariant();
        }

        this.tabBar.AddTab(new Tab() { Id = "home", Selected = SelectedTab == "home", Active = true, Label = this.dictionary["Item_Employee_Tab_Principal"], Available = true });
        this.tabBar.AddTab(new Tab() { Id = "formacion", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId <1, Label = this.Dictionary["Item_Employee_Tab_Learning"] });
        this.tabBar.AddTab(new Tab() { Id = "formacionInterna", Selected = SelectedTab == "formacioninterna", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId < 1, Label = this.Dictionary["Item_Employee_Tab_InternalLearning"] });
        this.tabBar.AddTab(new Tab() { Id = "uploadFiles", Selected = SelectedTab == "uploadfiles", Available = true, Active = this.employeeId > 0, Hidden = this.employeeId < 1, Label = this.Dictionary["Item_Employee_Tab_UploadFiles"] });
        //// this.tabBar.AddTab(new Tab() { Id = "trazas", Available = this.user.HasTraceGrant() && this.employeeId > 0, Active = this.employeeId > 0, Label = this.dictionary["Item_Employee_Tab_Traces"] });
    }

    private void RenderDocuments()
    {
        this.LtDocumentsList.Text = string.Empty;
        this.LtDocuments.Text = string.Empty;

        ReadOnlyCollection<UploadFile> files = UploadFile.GetByItem(5, this.employeeId, this.company.Id);
        StringBuilder res = new StringBuilder();
        StringBuilder resList = new StringBuilder();
        int contCells = 0;
        ReadOnlyCollection<string> extensions = ToolsFile.ExtensionToShow;
        foreach (UploadFile file in files)
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

    /// <summary>
    /// Generates JSON list of selected countries for the company
    /// </summary>
    private void RenderCountries()
    {
        StringBuilder res = new StringBuilder();
        string countryCompare = this.employee.Id < 0 ? this.dictionary["Common_None"] : this.employee.Address.Country;
        res.Append(string.Format(@"{{""text"":""{0}"",""value"":0,""selected"":{1},""description"":""{0}""}}", this.dictionary["Common_None"], countryCompare == this.dictionary["Common_None"] ? "true" : "false"));
        /*foreach (Country country in this.company.Countries)
        {
            res.Append(string.Format(@",{{""Id"":{2},""text"":""{0}"",""value"":{2},""selected"":{1},""description"":""{0}"",""imageSrc"":""assets/flags/{2}.png""}}", country.Description, country.Description == countryCompare ? "true" : "false", country.Id));
        }*/

        if (this.user.Language == "es")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Alemania", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brasil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Cataluña", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "España", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "Francia", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italia", "Italia", 5);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
        }

        if (this.user.Language == "ca")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Alemanya", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brasil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalunya", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Espanya", "España", 1);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Euskal Herria", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "França", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italia", "Italia", 5);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
        }

        if (this.user.Language == "en")
        {
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Andorra" ? "true" : "false", "Andorra", "Andorra", 4);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Argentina" ? "true" : "false", "Argentina", "Argentina", 18);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Australia" ? "true" : "false", "Australia", "Australia", 44);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Euskal Herria" ? "true" : "false", "Basque Country", "Euskal Herria", 46);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Brasil" ? "true" : "false", "Brazil", "Brasil", 20);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Cataluña" ? "true" : "false", "Catalonia", "Cataluña", 45);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Francia" ? "true" : "false", "France", "Francia", 7);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Alemania" ? "true" : "false", "Germany", "Alemania", 8);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Italia" ? "true" : "false", "Italy", "Italia", 5);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "Portugal" ? "true" : "false", "Portugal", "Portugal", 2);
            res.AppendFormat(@",{{""text"": ""{1}"", ""value"": ""{3}"", ""selected"": {0}, ""description"": ""{2}"", ""imageSrc"": ""assets/flags/{3}.png""}}", countryCompare == "España" ? "true" : "false", "Spain", "España", 1);
        }

        this.countryData = res.ToString();
        this.LtCountries.Text = res.ToString();
    }
}