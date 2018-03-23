// --------------------------------
// <copyright file="Department.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements department class</summary>
    public class Department : BaseItem
    {
        #region Fields
        /// <summary>
        /// Indicates if department is active
        /// </summary>
        private bool deleted;

        /// <summary>
        /// Inidicates if has job postion assigned
        /// </summary>
        private bool jobPositionAssigned;

        /// <summary>
        /// Employees of department
        /// </summary>
        private List<Employee> employees;

        /// <summary>
        /// List on job positions
        /// </summary>
        private List<JobPosition> jobPositions;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Department class.
        /// </summary>
        public Department()
        {
            this.employees = new List<Employee>();
        }

        /// <summary>
        /// Initializes a new instance of the Department class.
        /// Data of department is searched into database based on department identifier
        /// </summary>
        /// <param name="id">Department identififer</param>
        /// <param name="companyId">Company identifier</param>
        public Department(long id, int companyId)
        {
            this.employees = new List<Employee>();
            using (var cmd = new SqlCommand("Department_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@Id", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            bool first = true;
                            while (rdr.Read())
                            {
                                if (first)
                                {
                                    first = false;
                                    this.Id = id;
                                    this.CompanyId = companyId;
                                    this.Description = rdr.GetString(ColumnsDepartmentGetById.DepartmentDescription);
                                    this.ModifiedOn = rdr.GetDateTime(ColumnsDepartmentGetById.ModifiedOn);
                                    this.ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsDepartmentGetById.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsDepartmentGetById.ModifiedByUserName)
                                    };

                                    this.ModifiedBy.Employee = Employee.GetByUserId(this.ModifiedBy.Id);
                                }
                            }
                        }

                        this.ObtainEmployees();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Department({0},{1})", id, companyId));
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }

        #region Properties
        /// <summary>
        /// Gets an empty object of Department class.
        /// </summary>
        public static Department Empty
        {
            get
            {
                return new Department() 
                { 
                    Id = -1, 
                    CompanyId = -1, 
                    Description = string.Empty,
                    deleted = false,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        /// <summary>
        /// Gets an colection employees
        /// Conseguimos una colección de empleados
        /// </summary>
        public ReadOnlyCollection<Employee> Employees
        {
            get
            {
                if (this.employees == null)
                {
                    this.employees = new List<Employee>();
                }

                return new ReadOnlyCollection<Employee>(this.employees);
            }
        }

        /// <summary>
        /// Gets the job postions of department
        /// </summary>
        public ReadOnlyCollection<JobPosition> JobPositions
        {
            get
            {
                if (this.jobPositions == null)
                {
                    this.jobPositions = new List<JobPosition>();
                }

                return new ReadOnlyCollection<JobPosition>(this.jobPositions);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if department is deleted
        /// </summary>
        public bool Deleted
        {
            get
            {
                return this.deleted;
            }

            set
            {
                this.deleted = value;
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether if departent has job positions assigned
        /// </summary>
        public bool JobPositionAssigned
        {
            get
            {
                return this.jobPositionAssigned;
            }

            set
            {
                this.jobPositionAssigned = value;
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"
                    {{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""Active"": {3},
                        ""Deletable"": {4}
                    }}";

                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.CompanyId,
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>
        /// Gets the HTML code to render a table row of department for profile
        /// </summary>
        public string TableRowProfileSimple
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<tr><td>{0}</td></tr>", this.Description);
            }
        }

        /// <summary>
        /// Gets the HTML code to render a table row of department for the employee profile
        /// </summary>
        public string TableRowEmployee
        {
            get
            {
                string pattern = @"<tr>
                         <td><a href=""#"">{0}</a></td>
                         <td>
                             <div class=""visible-md visible-lg hidden-sm hidden-xs btn-group"">
                                 <span class=""btn btn-xs btn-danger"" onclick=""DepartmentDesassociation({1},'{0}');"">
                                    <i class=""icon-trash bigger-120""></i>
                                 </span>
                             </div>
                         </td>                                                                 
                     </tr>";
                return string.Format(CultureInfo.GetCultureInfo("en-us"), pattern, this.Description, this.Id);
            }
        }

        /// <summary>
        /// Gets a row reference 
        /// </summary>
        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<a href=""DepartmentView.aspx?id={0}"" title=""{2} {1}"">{1}</a>",
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }
        #endregion

        /// <summary>
        /// Extract from data base a departmen based on identifier
        /// </summary>
        /// <param name="id">Department identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A department based on identifier</returns>
        public static Department GetById(long id, int companyId)
        {
            return new Department(id, companyId);
        }

        /// <summary>
        /// Creates a JSON structure with the departmens of a company
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON structure of a departments list</returns>
        public static string GetByCompanyJsonList(int companyId)
        {
            var departments = ByCompany(companyId);
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var department in departments)
            {
                if (!department.deleted)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(department.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Creates a JSON structure with the departmens of a company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON structure of a departments list</returns>
        public static string GetByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var department in ByCompany(companyId))
            {
                if (!department.deleted)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(department.JsonKeyValue);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Delete a departent in database</summary>
        /// <param name="departmentId">Department identifier</param>
        /// <param name="reason">Reason for delete</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int departmentId, string reason, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Department_Delete
             * @DepartmentId int,
             * @CompanyId int,
             * @Reason nvarchar(200),
             * @UserId int */
            using (var cmd = new SqlCommand("Department_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", departmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", Tools.LimitedText(reason, 200)));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>Get a row delete Departament</summary>
        /// <param name="companyId">Type of companyId</param>
        /// <param name="departmentId">Type of depatamentId</param>
        /// <returns>Restult of action</returns>
        public static ActionResult Delete(int companyId, int departmentId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, @"Department::Delete {0}", departmentId);
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("Department_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", departmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result = ActivityLog.Department(departmentId, Convert.ToInt32(HttpContext.Current.Session["UserId"], CultureInfo.GetCultureInfo("en-us")), companyId, DepartmentLogActions.Delete, departmentId.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>Get a row GetByCompany</summary>
        /// <param name="companyId">Type companyId</param>
        /// <returns>Return of action</returns>
        public static ReadOnlyCollection<Department> ByCompany(int companyId)
        {
            var res = new List<Department>();
            using (var cmd = new SqlCommand("Departments_GetByCompany"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            var newDepartment = Department.Empty;
                            while (rdr.Read())
                            {
                                if (newDepartment.Id != rdr.GetInt32(0))
                                {
                                    newDepartment = new Department()
                                    {
                                        Id = rdr.GetInt32(0),
                                        CompanyId = companyId,
                                        Description = rdr.GetString(1),
                                        deleted = rdr.GetBoolean(3),
                                        employees = new List<Employee>(),
                                        jobPositionAssigned = rdr.GetInt32(4) == 1
                                    };

                                    newDepartment.ObtainEmployees();
                                    res.Add(newDepartment);
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, "Deparment::GetByCompany", companyId.ToString(CultureInfo.InvariantCulture));
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return new ReadOnlyCollection<Department>(res);
        }

        /// <summary>Obtain the employess of department</summary>
        public void ObtainEmployees()
        {
            this.jobPositions = new List<JobPosition>();
            this.employees = new List<Employee>();
            using (var cmd = new SqlCommand("Department_GetEmployess"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newJobPosition = new JobPosition()
                                {
                                    Id = rdr.GetInt32(0),
                                    CompanyId = this.CompanyId,
                                    Description = rdr.GetString(1)
                                };

                                if (!rdr.IsDBNull(2))
                                {
                                    var responsible = new JobPosition()
                                    {
                                        Id = rdr.GetInt32(2),
                                        CompanyId = this.CompanyId,
                                        Description = rdr.GetString(3)
                                    };
                                    newJobPosition.Responsible = responsible;
                                }

                                bool jobPositionExists = false;
                                foreach (var jobPosition in this.jobPositions)
                                {
                                    if (jobPosition.Id == newJobPosition.Id)
                                    {
                                        jobPositionExists = true;
                                        break;
                                    }
                                }

                                if (!jobPositionExists)
                                {
                                    this.jobPositions.Add(newJobPosition);
                                }

                                if (!rdr.IsDBNull(4))
                                {
                                    var newEmployee = new Employee()
                                    {
                                        Id = rdr.GetInt32(4),
                                        CompanyId = this.CompanyId,
                                        Name = rdr.GetString(5),
                                        LastName = rdr.GetString(6),
                                        Nif = rdr.GetString(7),
                                        Email = rdr.GetString(8),
                                        Phone = rdr.GetString(9)
                                    };

                                    bool employeeExists = false;
                                    foreach (var employee in this.employees)
                                    {
                                        if (employee.Id == newEmployee.Id)
                                        {
                                            employeeExists = true;
                                            break;
                                        }
                                    }

                                    if (!employeeExists)
                                    {
                                        this.employees.Add(newEmployee);
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a row of list departament
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">List of user's grants</param>
        /// <returns>Code HTML for department row</returns>        
        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            bool grantDepartment = UserGrant.HasWriteGrant(grants, ApplicationGrant.Department);
            bool grantDepartmentDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Department);
            bool grantJobPosition = UserGrant.HasWriteGrant(grants, ApplicationGrant.JobPosition);
            bool grantEmployee = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);

            string employeesList = string.Empty;
            bool first = true;
            foreach (Employee employee in this.employees)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    employeesList += ", ";
                }

                if (grantEmployee)
                {
                    employeesList += employee.Link;
                }
                else
                {
                    employeesList += employee.FullName;
                }
            }

            string jobPositionsList = string.Empty;
            first = true;
            foreach (JobPosition jobPosition in this.jobPositions)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    jobPositionsList += ", ";
                }

                if (grantJobPosition)
                {
                    jobPositionsList += jobPosition.Link;
                }
                else
                {
                    jobPositionsList += jobPosition.Description;
                }
            }

            string iconRenameSymbol = grantDepartment ? "icon-edit" : "icon-eye-open";
            string iconRename = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""DepartmentUpdate({0},'{1}');"">                 <i class=""{3} bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), Tools.JsonCompliant(dictionary["Common_Edit"]), iconRenameSymbol);

            string iconDelete = string.Empty;
            if (grantDepartmentDelete)
            {
                string deleteAction = string.Empty;
                if (this.employees.Count == 0 && this.jobPositions.Count == 0)
                {
                    deleteAction = string.Format(CultureInfo.GetCultureInfo("en-us"), "DepartmentDelete({0},'{1}');", this.Id, this.Description);
                }
                else if (this.jobPositions.Count == 0)
                {
                    deleteAction = string.Format(CultureInfo.GetCultureInfo("en-us"), "alertUI('{0}');", Tools.JsonCompliant(dictionary["Item_Departmennt_ErrorMessage_HasEmployees"]));
                }
                else if (this.employees.Count > 0)
                {
                    deleteAction = string.Format(CultureInfo.GetCultureInfo("en-us"), "alertUI('{0}');", Tools.JsonCompliant(dictionary["Item_Departmennt_ErrorMessage_HasEmployees"]));
                }
                else
                {
                    deleteAction = string.Format(CultureInfo.GetCultureInfo("en-us"), "alertUI('{0}');", Tools.JsonCompliant(dictionary["Item_JobPosition_ErrorMessage_HasJobPositionLinked"]));
                }
                
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{1} {2}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteAction, Tools.JsonCompliant(dictionary["Common_Delete"]), Tools.SetTooltip(this.Description));
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr id=""{5}""><td style=""width:300px;padding-left:4px;"">{0}</td><td style=""width:400px;padding-left:4px;""class=""hidden-480"">({6})&nbsp;{7}</td><td class=""hidden-480"">({1})&nbsp;{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td></tr>",
                this.Link,
                this.employees.Count,
                employeesList,
                iconRename,
                iconDelete,
                this.Id,
                this.jobPositions.Count,
                jobPositionsList);
        }

        /// <summary>
        /// Gets the HTML code for render a tag of employe
        /// </summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <returns>HTML code</returns>
        public string EmployeeListTag(Dictionary<string, string> dictionary, int employeeId)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string pattern = @"<span class=""tag"" style=""cursor:pointer;"" id=""{0}"" title=""{4} {1}""><span onclick=""GoDepartment({0});"">{1}</span><button type=""button"" class=""close"" title=""{2}"" onclick=""DepartmentDesassociation({0}, '{1}',{3});"">×</button></span>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Id,
                this.Description,
                dictionary["Common_Delete"],
                employeeId,
                dictionary["Common_Edit"]);
        }

        /// <summary>
        /// Render a alert tag for alerts dropdown on principal header
        /// </summary>
        /// <param name="dictionary">Dicitionary for fixed labels</param>
        /// <returns>HTML code</returns>
        public string AlertTag(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string pattern = @"<li>
                                    <a href=""DepartmentView.aspx?id={0}"">
                                        <div class=""clearfix"">
                                            <span class=""pull-left"">
                                                <i class=""btn btn-xs no-hover btn-success icon-group""></i>{1}
                                            </span>
                                            <span class=""pull-right badge badge-info"">{2}</span>
                                        </div>
                                    </a>
                                </li>";
            return string.Format(CultureInfo.InvariantCulture, pattern, this.Id, this.Description, dictionary["Item_Department_Message_WithoutEmployees"]);
        }

        /// <summary>
        /// Get a row inset Departament
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Department_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.OutputLong("@DepartmentId"));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt64(cmd.Parameters["@DepartmentId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    result.SetSuccess(this.Id);
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "Department::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Department::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get a row DepartamentUpdate
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            ActionResult result = new ActionResult() { Success = false, MessageError = "No action" };
            using (SqlCommand cmd = new SqlCommand("Department_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = this.CompanyId;

                    cmd.Parameters.Add("@Description", SqlDbType.Text);
                    cmd.Parameters["@Description"].Value = this.Description;

                    cmd.Parameters.Add("@DepartmentId", SqlDbType.Int);
                    cmd.Parameters["@DepartmentId"].Value = this.Id;

                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@UserId"].Value = userId;

                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.Success = true;
                    result.MessageError = string.Empty;
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "Department::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Department::Update", string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds an employee into department
        /// </summary>
        /// <param name="employee">Employee to add</param>
        public void AddEmployee(Employee employee)
        {
            if (employee != null)
            {
                if (this.employees == null)
                {
                    this.employees = new List<Employee>();
                }

                this.employees.Add(employee);
            }
        }
    }
}
