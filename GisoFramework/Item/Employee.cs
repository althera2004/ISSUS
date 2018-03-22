// --------------------------------
// <copyright file="Employee.cs" company="Sbrinna">
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
    using System.Linq;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements Employee class.</summary>
    public class Employee : BaseItem
    {
        /// <summary>
        /// Value for separtor values
        /// </summary>
        private const string Separator = "|";

        #region Fields
        /// <summary>List of learning assitance of employee</summary>
        private List<LearningAssistance> learningAssistance;

        /// <summary>Address of employee</summary>
        private EmployeeAddress address;

        /// <summary>Notes added to employee</summary>
        private string notes;

        /// <summary>Indentifier of application user of employee</summary>
        private int userId;

        /// <summary>Date of disabled</summary>
        private DateTime? disabledDate;

        /// <summary>Actual job position</summary>
        private JobPosition jobPosition;

        /// <summary>Departments where employee is assigned</summary>
        private List<Department> departments;
        
        /// <summary>Job positions of employee</summary>
        private List<JobPosition> jobPositions;

        /// <summary>The skills of employee</summary>
        private EmployeeSkills employeeSkills;
        #endregion

        /// <summary>Initializes a new instance of the Employee class.</summary>
        public Employee()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee" /> class.
        /// Searching it into database by employee's identifier
        /// </summary>
        /// <param name="employeeId">Identifier of employee</param>
        /// <param name="complete">Indicates if obtain the complete data</param>
        public Employee(long employeeId, bool complete)
        {
            this.departments = new List<Department>();
            this.jobPositions = new List<JobPosition>();
            using (var cmd = new SqlCommand("Employee_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            bool first = true;
                            while (rdr.Read())
                            {
                                if (first)
                                {
                                    first = false;
                                    this.Id = rdr.GetInt32(ColumnsEmployeeGetById.Id);
                                    this.CompanyId = rdr.GetInt32(ColumnsEmployeeGetById.CompanyId);
                                    this.Name = rdr.GetString(ColumnsEmployeeGetById.Name);
                                    this.LastName = rdr.GetString(ColumnsEmployeeGetById.LastName);
                                    this.Email = rdr.GetString(ColumnsEmployeeGetById.Email);
                                    this.Phone = rdr.GetString(ColumnsEmployeeGetById.Phone);
                                    this.Nif = rdr.GetString(ColumnsEmployeeGetById.Nif);
                                    this.notes = rdr.GetString(ColumnsEmployeeGetById.Notes);
                                    this.address = new EmployeeAddress()
                                    {
                                        Address = rdr.GetString(ColumnsEmployeeGetById.Address),
                                        PostalCode = rdr.GetString(ColumnsEmployeeGetById.PostalCode),
                                        City = rdr.GetString(ColumnsEmployeeGetById.City),
                                        Province = rdr.GetString(ColumnsEmployeeGetById.Province),
                                        Country = rdr.GetString(ColumnsEmployeeGetById.Country)
                                    };

                                    if (!rdr.IsDBNull(ColumnsEmployeeGetById.ModifiedByUserId))
                                    {
                                        this.ModifiedBy = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt32(ColumnsEmployeeGetById.ModifiedByUserId),
                                            UserName = rdr.GetString(ColumnsEmployeeGetById.ModifiedByUserName)
                                        };
                                    }

                                    if (!rdr.IsDBNull(ColumnsEmployeeGetById.ModifiedOn))
                                    {
                                        this.ModifiedOn = rdr.GetDateTime(ColumnsEmployeeGetById.ModifiedOn);
                                    }

                                    if (!rdr.IsDBNull(ColumnsEmployeeGetById.InactivationDate))
                                    {
                                        this.disabledDate = rdr.GetDateTime(ColumnsEmployeeGetById.InactivationDate);
                                    }
                                }

                                this.ModifiedBy.Employee = Employee.GetByUserId(this.ModifiedBy.Id);
                            }
                        }
                        if (complete)
                        {
                            this.ObtainJobPositionsHistoric();
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employee({0},{1})", this.Id, complete));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employee({0},{1})", this.Id, complete));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employee({0},{1})", this.Id, complete));
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
        /// <summary>Gets an empty employee with basic data</summary>
        public static Employee EmptySimple
        {
            get
            {
                return new Employee
                {
                    Id = -1,
                    Name = string.Empty,
                    LastName = string.Empty,
                    CompanyId = -1,
                    address = EmployeeAddress.Empty
                };
            }
        }

        /// <summary>Gets an empty employee</summary>
        public static Employee Empty
        {
            get
            {
                return new Employee
                {
                    Id = -1,
                    Name = string.Empty,
                    LastName = string.Empty,
                    CompanyId = -1,
                    jobPositions = new List<JobPosition>(),
                    Email = string.Empty,
                    employeeSkills = EmployeeSkills.Empty,
                    jobPosition = JobPosition.Empty,
                    learningAssistance = new List<LearningAssistance>(),
                    Nif = string.Empty,
                    notes = string.Empty,
                    Phone = string.Empty,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        public string EmployeeActions
        {
            get
            {
                var res = new StringBuilder("[");
                /* CREATE PROCEDURE Employee_GetActions
                 *   @EmployeeId bigint,
                 *   @CompanyId int */
                using (var cmd = new SqlCommand())
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Employee_GetActions";
                        try
                        {
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Id));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                bool first = true;
                                while (rdr.Read())
                                {
                                    if (first)
                                    {
                                        first = false;
                                    }
                                    else
                                    {
                                        res.Append(",");
                                    }

                                    res.Append(Environment.NewLine);
                                    res.Append("\t{");
                                    res.Append("\"AssignationType\":\"").Append(Tools.JsonCompliant(rdr.GetString(0))).Append("\",");
                                    res.Append("\"ItemTypePage\":").Append(rdr.GetInt64(1)).Append(",");
                                    res.Append("\"ItemId\":").Append(rdr.GetInt64(2)).Append(",");
                                    res.Append("\"Description\":\"").Append(Tools.JsonCompliant(rdr.GetString(3))).Append("\"");
                                    res.Append("}");
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

                res.Append("]");
                return res.ToString();
            }
        }

        public bool HasActions
        {
            get
            {
                bool res = false;
                /* CREATE PROCEDURE Employee_GetActions
                 *   @EmployeeId bigint,
                 *   @CompanyId int */
                using (var cmd = new SqlCommand())
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Employee_GetActions";
                        try
                        {
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Id));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                res = rdr.HasRows;
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

                return res;
            }
        }

        public bool HasUserAssigned { get; set; }

        public bool HasActionAssigned { get; set; }

        /// <summary>Gets or sets the employee's skills</summary>
        public EmployeeSkills EmployeeSkills
        {
            get
            {
                return this.employeeSkills;
            }

            set
            {
                this.employeeSkills = value;
            }
        }

        /// <summary>Gets Learning assistance list</summary>
        public ReadOnlyCollection<LearningAssistance> LearningAssistance
        {
            get
            {
                if (this.learningAssistance == null)
                {
                    this.learningAssistance = new List<LearningAssistance>();
                }

                return new ReadOnlyCollection<LearningAssistance>(this.learningAssistance);
            }
        }

        /// <summary>Gets or sets employee's job position</summary>
        public JobPosition JobPosition
        {
            get
            {
                return this.jobPosition;
            }

            set
            {
                this.jobPosition = value;
            }
        }

        /// <summary>Gets or sets notes added to employee</summary>
        public string Notes
        {
            get
            {
                return this.notes;
            }

            set
            {
                this.notes = value;
            }
        }

        /// <summary>Gets or sets application user assigned to employee</summary>
        public int UserId
        {
            get
            {
                return this.userId;
            }

            set
            {
                this.userId = value;
            }
        }

        /// <summary>Gets or sets the name of employee</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the last name of employee</summary>
        public string LastName { get; set; }

        /// <summary>Gets or sets the identification number of employee</summary>
        public string Nif { get; set; }

        /// <summary>Gets or sets the email of employee</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets the phone of employee, string.Empty if employee hasn't phone</summary>
        public string Phone { get; set; }

        /// <summary>Gets or sets the date of employee deactivation</summary>
        public DateTime? DisabledDate
        {
            get
            {
                return this.disabledDate;
            }

            set
            {
                this.disabledDate = value;
            }
        }

        /// <summary>Gets or sets the address of employee</summary>
        public EmployeeAddress Address
        {
            get
            {
                return this.address;
            }

            set
            {
                this.address = value;
            }
        }

        /// <summary>Gets the job positions of employee</summary>
        public ReadOnlyCollection<JobPosition> JobPositionsList
        {
            get
            {
                return new ReadOnlyCollection<JobPosition>(this.jobPositions);
            }
        }

        /// <summary>Gets the HTML code to a employee row in the job position profile</summary>
        public string JobPositionListRow
        {
            get
            {
                var user = (ApplicationUser)HttpContext.Current.Session["User"];
                string text = this.FullName;
                if (user.HasGrantToRead(ApplicationGrant.Employee))
                {
                    text = this.Link;
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"<tr><td>{0}</td><td class=""hidden-480"">{1}</td><td>{2}</td><td class=""hidden-480"">{3}</td></tr>",
                    text,
                    this.Nif,
                    this.Email,
                    this.Phone);
            }
        }

        /// <summary>
        /// Gets the full name of employee.
        /// Employe name is showed as coloquial name format
        /// </summary>
        public string FullName
        {
            get
            {
                string res = string.Empty;
                if (!string.IsNullOrEmpty(this.Name))
                {
                    res = this.Name;
                }

                if (!string.IsNullOrEmpty(this.LastName))
                {
                    if (!string.IsNullOrEmpty(res))
                    {
                        res += " ";
                    }

                    res += this.LastName;
                }

                return res;
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                string activeValue = this.Active ? "true" : "false";
                if (this.Active && this.disabledDate.HasValue)
                {
                    activeValue = "false";
                }

                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0},""Value"":""{1}"", ""Active"":{2}}}", this.Id, this.FullName, activeValue);
            }
        }

        public string JsonCombo
        {
            get
            {
                string endDate = "null";
                if (this.disabledDate.HasValue)
                {
                    endDate = string.Format(CultureInfo.InvariantCulture, "{0:dd/MM/yyyy}", this.disabledDate.Value);
                }

                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""FullName"":""{2}"",""HasUserAssigned"":{3},""Active"":{4},""DisabledDate"":{5}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.FullName),
                    this.HasUserAssigned ? "true" : "false",
                    this.Active ? "true" : "false",
                    endDate);
            }
        }

        public string JsonSimple
        {
            get
            {
                return string.Format(
                       CultureInfo.InvariantCulture,
                       @"{{""Id"":{0},""CompanyId"":{1},""Name"":""{2}"",""LastName"":""{3}""}}",
                       this.Id,
                       this.CompanyId,
                       Tools.JsonCompliant(this.Name),
                       Tools.JsonCompliant(this.LastName));
            }
        }

        public static string JsonList(ReadOnlyCollection<Employee> list)
        {
            if(list == null)
            {
                return Tools.EmptyJsonList;
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach(Employee employee in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(employee.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                if (this.address == null)
                {
                    this.address = EmployeeAddress.Empty;
                }

                if (this.jobPositions == null)
                {
                    this.jobPositions = new List<JobPosition>();
                }

                var res = new StringBuilder();
                res.Append(Environment.NewLine);
                res.Append("\t\t\t{").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Id\":").Append(this.Id).Append(",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"CompanyId\":").Append(this.CompanyId).Append(",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Name\":\"").Append(this.Name).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"LastName\":\"").Append(this.LastName).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"FullName\":\"").Append(this.FullName).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Nif\":\"").Append(this.Nif).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Email\":\"").Append(this.Email).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Phone\":\"").Append(this.Phone).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Address\":").Append(this.address.Json).Append(",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Notes\":\"").Append(Tools.JsonCompliant(this.notes)).Append("\",").Append(Environment.NewLine);
                res.Append("\t\t\t\t").Append(Tools.JsonPair("DisabledDate", this.disabledDate)).Append(",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"Active\":").Append(this.Active ? "true" : "false").Append(",").Append(Environment.NewLine);
                res.Append("\t\t\t\t\"JobPositions\":[");
                bool first = true;
                foreach (JobPosition jobPositionItem in this.jobPositions)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(jobPositionItem.Id);
                }

                res.Append("]");
                if (this.User != null)
                {
                    if (this.User.Id > 0)
                    {
                        res.Append(",\"User\":").Append(this.User.Json);
                    }
                }

                res.Append(Environment.NewLine).Append("\t\t\t}");
                return res.ToString();
            }
        }

        /// <summary>Gets the HTML code for the link to Employee view page</summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "<a href=\"EmployeesView.aspx?id={0}\" title=\"{2} {1}\">{1}</a>", this.Id, this.FullName, ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets a list of job position assginmets of employee</summary>
        public Collection<JobPositionAsigment> JobPositionAssignment { get; private set; }

        /// <summary>Gets or sets the application user linked to employee</summary>
        public ApplicationUser User { get; set; }

        public ReadOnlyCollection<Department> Departments
        {
            get
            {
                if (this.departments == null)
                {
                    this.departments = new List<Department>();
                }

                return new ReadOnlyCollection<Department>(this.departments);
            }
        }
        #endregion

        public static ReadOnlyCollection<Employee> WithoutUser(int companyId)
        {
            /* CREATE PROCEDURE Employee_WithoutUser
             *   @CompanyId int */
            var res = new List<Employee>();
            using (var cmd = new SqlCommand("Employee_WithoutUser"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new Employee()
                                {
                                    Id = rdr.GetInt32(0),
                                    Name = rdr.GetString(1),
                                    LastName = rdr.GetString(2)
                                });
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

            return new ReadOnlyCollection<Employee>(res);
        }

        public static Employee GetByUserId(int userId)
        {
            var res = Employee.Empty;
            using (var cmd = new SqlCommand("Employee_GetByUserId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsEmployeeGetByUserId.Id),
                                    Name = rdr.GetString(ColumnsEmployeeGetByUserId.Name),
                                    LastName = rdr.GetString(ColumnsEmployeeGetByUserId.LastName),
                                    Email = rdr.GetString(ColumnsEmployeeGetByUserId.Email),
                                    Nif = rdr.GetString(ColumnsEmployeeGetByUserId.Nif),
                                    Phone = rdr.GetString(ColumnsEmployeeGetByUserId.Phone),
                                    address = new EmployeeAddress()
                                    {
                                        Address = rdr.GetString(ColumnsEmployeeGetByUserId.Address),
                                        City = rdr.GetString(ColumnsEmployeeGetByUserId.City),
                                        Country = rdr.GetString(ColumnsEmployeeGetByUserId.Country),
                                        PostalCode = rdr.GetString(ColumnsEmployeeGetByUserId.PostalCode),
                                        Province = rdr.GetString(ColumnsEmployeeGetByUserId.Province)
                                    }
                                };
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

            return res;
        }

        public static ReadOnlyCollection<Employee> GetByCompany(int companyId)
        {
            /* CREATE PROCEDURE Employee_GetByCompany
             *   @CompanyId int */
            var res = new List<Employee>();
            using (var cmd = new SqlCommand("Employee_GetByCompany"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                bool exists = false;
                                long employeeId = rdr.GetInt32(ColumnsEmployeeGetByCompany.Id);
                                foreach (Employee employee in res)
                                {
                                    if (employee.Id == employeeId)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    var newEmployee = new Employee()
                                    {
                                        Id = employeeId,
                                        Name = rdr.GetString(ColumnsEmployeeGetByCompany.Name),
                                        LastName = rdr.GetString(ColumnsEmployeeGetByCompany.LastName),
                                        Email = rdr.GetString(ColumnsEmployeeGetByCompany.Email),
                                        Phone = rdr.GetString(ColumnsEmployeeGetByCompany.Phone),
                                        Nif = rdr.GetString(ColumnsEmployeeGetByCompany.Nif),
                                        Active = rdr.GetBoolean(ColumnsEmployeeGetByCompany.Active)
                                    };

                                    if (!rdr.IsDBNull(ColumnsEmployeeGetByCompany.FechaBaja))
                                    {
                                        newEmployee.disabledDate = rdr.GetDateTime(ColumnsEmployeeGetByCompany.FechaBaja);
                                    }

                                    res.Add(newEmployee);
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

            return new ReadOnlyCollection<Employee>(res);
        }

        public static ReadOnlyCollection<Employee> GetByCompanyWithUser(int companyId)
        {
            /* CREATE PROCEDURE Employee_GetByCompany
             *   @CompanyId int */
            var res = new List<Employee>();
            using (var cmd = new SqlCommand("Employee_GetByCompany"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                bool exists = false;
                                long employeeId = rdr.GetInt32(ColumnsEmployeeGetByCompany.Id);
                                foreach (var employee in res)
                                {
                                    if (employee.Id == employeeId)
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    var newEmployee = new Employee()
                                    {
                                        Id = employeeId,
                                        Name = rdr.GetString(ColumnsEmployeeGetByCompany.Name),
                                        LastName = rdr.GetString(ColumnsEmployeeGetByCompany.LastName),
                                        Email = rdr.GetString(ColumnsEmployeeGetByCompany.Email),
                                        Phone = rdr.GetString(ColumnsEmployeeGetByCompany.Phone),
                                        Nif = rdr.GetString(ColumnsEmployeeGetByCompany.Nif),
                                        Active = rdr.GetBoolean(ColumnsEmployeeGetByCompany.Active)
                                    };

                                    if (!rdr.IsDBNull(ColumnsEmployeeGetByCompany.FechaBaja))
                                    {
                                        newEmployee.disabledDate = rdr.GetDateTime(ColumnsEmployeeGetByCompany.FechaBaja);
                                    }

                                    res.Add(newEmployee);
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

            return new ReadOnlyCollection<Employee>(res);
        }

        public static string GetByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var employee in GetByCompany(companyId).OrderBy(e => e.FullName))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(employee.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Restore a employee</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult Restore(int employeeId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_Restore
             * @EmployeeId int,
             * @CompanyId int,
             * @UserId int */
            using (var cmd = new SqlCommand("Employee_Restore"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserID", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Disable a employee</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <param name="endDate">Date of action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Disable(long employeeId, int companyId, int userId, DateTime endDate)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_Disable
             * @EmployeeId bigint,
             * @CompanyId int,
             * @UserId int,
             * @FechaBaja date */
            using (var cmd = new SqlCommand("Employee_Disable"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserID", userId));
                        cmd.Parameters.Add(DataParameter.Input("@FechaBaja", endDate));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Gets a json structure of an employee</summary>
        /// <param name="employee">Employee to extract data</param>
        /// <returns>JSON structure</returns>
        public static string ToJson(Employee employee)
        {
            if (employee == null)
            {
                return Tools.EmptyJsonObject;
            }

            string pattern = @"
                {{
                    ""Id"":{0},
                    ""CompanyId"":{1},
                    ""Name"":""{2}"",
                    ""Nif"":""{5}"",
                    ""Email"":""{3}"",
                    ""UserId"":{4},
                    ""Address"":{{{6}}}
                }}";

            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                employee.Id,
                employee.CompanyId,
                employee.FullName,
                employee.Email,
                employee.userId,
                employee.Nif,
                employee.address.Json);
        }

        /// <summary>Unlink from a department</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="departmentId">Department identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult DisassociateToDepartment(int companyId, int employeeId, int departmentId)
        {
            var result = new ActionResult();
            /* CREATE PROCEDURE Employee_DesassociateToDepartment
             * @EmployeeId int,
             * @DepartmentId int,
             * @CompanyId int */
            using (var cmd = new SqlCommand("Employee_DesassociateToDepartment"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                        cmd.Parameters.Add("@DepartmentId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@EmployeeId"].Value = employeeId;
                        cmd.Parameters["@DepartmentId"].Value = departmentId;
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        ActivityLog.Employee(employeeId, Convert.ToInt32(HttpContext.Current.Session["UserId"], CultureInfo.GetCultureInfo("en-us")), companyId, EmployeeLogActions.DisassociateDepartment, string.Format(CultureInfo.GetCultureInfo("en-us"), "DepartmentId:{0}", departmentId));
                        result.Success = true;
                        result.MessageError = string.Empty;
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

        /// <summary>Link to a department</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="departmentId">Department identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult AssociateToDepartment(int companyId, int employeeId, int departmentId)
        {
            var result = new ActionResult();
            /* CREATE PROCEDURE Employee_AssociateToDepartment
             * @EmployeeId int,
             * @DepartmentId int,
             * @CompanyId int */
            using (var cmd = new SqlCommand("Employee_AssociateToDepartment"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                        cmd.Parameters.Add("@DepartmentId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@EmployeeId"].Value = employeeId;
                        cmd.Parameters["@DepartmentId"].Value = departmentId;
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        ActivityLog.Employee(employeeId, Convert.ToInt32(HttpContext.Current.Session["UserId"], CultureInfo.GetCultureInfo("en-us")), companyId, EmployeeLogActions.AssociateToDepartment, string.Format(CultureInfo.InstalledUICulture, "DepartmentId:{0}", departmentId));
                        result.Success = true;
                        result.MessageError = string.Empty;
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

        /// <summary>Delete an employee</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="reason">Reason to delete</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user taht performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int employeeId, string reason, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_Delete
             * @EmployeeId int,
             * @CompanyId int,
             * @Reason nvarchar(200),
             * @UserId int */
            using (var cmd = new SqlCommand("Employee_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Reason", reason, 200));
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

        /// <summary>Assignate job position to an employee</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="jobPositionId">Job position identifier</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user taht performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult AssignateJobPosition(int employeeId, long jobPositionId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_AsignateJobPosition
             * @EmployeeId int,
             * @JobPosition bigint,
             * @CompanyId int,
             * @UserId int */
            using (var cmd = new SqlCommand("Employee_AsignateJobPosition"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", jobPositionId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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

        /// <summary>Unassignate job position to an employee</summary>
        /// <param name="employeeId">Employee identifier</param>
        /// <param name="jobPositionId">Job position identifier</param>
        /// <param name="date">Date of effective unassignation</param>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="userId">Identifier of user taht performs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult UnassignateJobPosition(int employeeId, long jobPositionId,DateTime date, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Employee_UnasignateJobPosition"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", jobPositionId));
                        cmd.Parameters.Add(DataParameter.Input("@Date", date));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
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

        public static ActionResult DeleteJobPosition(int employeeId, int jobPositionId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_DeleteJobPosition
             *   @EmployeeId bigint,
             *   @JobPositionId bigint */
            using (var cmd = new SqlCommand("Employee_DeleteJobPosition"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", Convert.ToInt64(employeeId)));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", Convert.ToInt64(jobPositionId)));
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

        public static string CompanyListJson(Company company)
        {
            if(company == null)
            {
                return Tools.EmptyJsonList;
            }

            return CompanyListJson(company.Id);
        }

        public static string CompanyListJson(int companyId)
        {
            var res = new StringBuilder("[");
            var employees = GetList(companyId);
            bool first = true;
            foreach (var employee in employees.OrderBy(e => e.FullName))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(employee.JsonCombo);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Employee> GetList(int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, "Compnay::ObtainEmployees() . CompanyId:{0}", companyId);
            var res = new List<Employee>();
            using (var cmd = new SqlCommand("Company_GetEmployees"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newEmployee = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsCompanyGetEmployees.Id),
                                    CompanyId = companyId,
                                    Name = rdr.GetString(ColumnsCompanyGetEmployees.Name),
                                    LastName = rdr.GetString(ColumnsCompanyGetEmployees.LastName),
                                    Active = rdr.GetBoolean(ColumnsCompanyGetEmployees.Active),
                                    Nif = rdr.GetString(ColumnsCompanyGetEmployees.Nif),
                                    Email = rdr.GetString(ColumnsCompanyGetEmployees.Email),
                                    jobPositions = new List<JobPosition>(),
                                    departments = new List<Department>(),
                                    HasUserAssigned = rdr.GetInt32(ColumnsCompanyGetEmployees.HasUserAssigned) == 1,
                                    HasActionAssigned = rdr.GetInt32(ColumnsCompanyGetEmployees.HasActionAssignated) == 1
                                };

                                if (!rdr.IsDBNull(ColumnsCompanyGetEmployees.EndDate))
                                {
                                    newEmployee.disabledDate = rdr.GetDateTime(ColumnsCompanyGetEmployees.EndDate);
                                }

                                newEmployee.GetJobPositions();
                                res.Add(newEmployee);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
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

            return new ReadOnlyCollection<Employee>(res);
        }

        /// <summary>Render a tag for employee</summary>
        /// <param name="dictionary">Dictionary for fices labels</param>
        /// <param name="admin">Indicates is session user is admin</param>
        /// <returns>HTML code for employee tag</returns>
        public string LearningTag(Dictionary<string, string> dictionary, bool admin)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string link = string.Empty;
            if (admin)
            {
                string.Format(
                    CultureInfo.InvariantCulture,
                    @" onclick=""document.location='EmployeesView.aspx?id={0}'"";",
                    this.Id);
            }

            string cursor = admin ? @" style=""cursor:pointer;""" : string.Empty;
            string pattern = @"<span class=""tag"" _title=""{3}""{4} id=""{0}""{2}>{1}</span>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                this.Id,
                Tools.Resume(this.FullName, 40),
                link,
                dictionary["Common_Edit"],
                cursor);
        }

        /// <summary>Render the HTML code for a department row for employe profile</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="departmentId">Department identifier</param>
        /// <returns>HTML code</returns>
        public string DepartmentListRow(Dictionary<string, string> dictionary, long departmentId)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string deleteAction = "EmployeeDelete";
            if (this.jobPositions == null)
            {
                this.jobPositions = new List<JobPosition>();
            }

            foreach (var jobPositionItem in this.jobPositions)
            {
                if (jobPositionItem != null)
                {
                    if (jobPositionItem.Department.Id == departmentId)
                    {
                        deleteAction = "NoDelete";
                        break;
                    }
                }
            }

            string iconRename = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-edit bigger-120""></i></span>", this.Id, this.FullName, dictionary["Common_Edit"]);
            string iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", this.Id, this.FullName, dictionary["Common_Delete"], deleteAction);
            iconDelete = string.Empty;
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<tr><td>{0}</td><td class=""hidden-480"">{1}</td><td class=""hidden-480"">{2}</td><td class=""hidden-480"">{3}</td><td>{4} {5}</td></tr>", this.Link, this.Nif, this.Email, this.Phone, iconRename, iconDelete);
        }

        /// <summary>Render the HTML code for a row in inactive employees list</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">Grants of user</param>
        /// <returns>HTML code</returns>
        public string ListRowInactive(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);
            string iconRestore = string.Format(CultureInfo.InvariantCulture, @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""Restore({0},'{1}');""><i class=""icon-undo bigger-120""></i></span>", this.Id, this.FullName, dictionary["Item_Employee_Button_Restore"]);
            string pattern = @" <tr><td>{0}</td><td style=""width:120px;"">{1}</td><td style=""width:300px;"">{2}</td><td style=""width:90px;"">{3:dd/MM/yyyy}</td><td style=""width:90px;"">{4}</td></tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Link,
                this.Nif,
                this.Email,
                this.disabledDate,
                grantWrite ? iconRestore : string.Empty);
        }

        /// <summary>Obtain the employee's skills from data base</summary>
        public void ObtainEmployeeSkills()
        {
            this.employeeSkills = new EmployeeSkills(this.Id, this.CompanyId);
            if (this.employeeSkills.Id == 0)
            {
                this.employeeSkills = EmployeeSkills.Empty;
            }
        }

        public void GetJobPositions()
        {
            /* CREATE PROCEDURE Employee_GetJobPositions
             *   @EmployeeId bigint,
             *   @CompanyId int */
            this.jobPositions = new List<JobPosition>();
            this.departments = new List<Department>();
            using (var cmd = new SqlCommand("Employee_GetJobPositions"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (rdr.IsDBNull(1))
                                {
                                    var item = new JobPosition()
                                    {
                                        Id = rdr.GetInt32(2),
                                        Description = rdr.GetString(4),
                                        Department = new Department
                                        {
                                            Id = rdr.GetInt32(3),
                                            Description = rdr.GetString(9)
                                        }
                                    };

                                    bool found = false;
                                    foreach (var department in this.departments)
                                    {
                                        if (department.Id == item.Department.Id)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (!found)
                                    {
                                        this.departments.Add(item.Department);
                                    }

                                    this.jobPositions.Add(item);
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, "Employee::GetJosPoisitions");
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }

                        cmd.Connection.Dispose();
                    }
                }
            }
        }

        /// <summary>Render the HTML code for a row in active employees list</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">User grants</param>
        /// <returns>HTML code</returns>
        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantEmployee = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);
            bool grantEmployeeDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Employee);
            bool grantDepartment = UserGrant.HasWriteGrant(grants, ApplicationGrant.Department);
            bool grantJobPosition = UserGrant.HasWriteGrant(grants, ApplicationGrant.JobPosition);

            string iconDelete = string.Empty;
            if (grantEmployeeDelete)
            {
                string deleteFunction = string.Format(CultureInfo.InvariantCulture, "ProviderDelete({0},'{1}');", this.Id, this.Description);
                string deleteAction = this.HasActions ? "EmployeeDeleteAlert" : "EmployeeDelete";
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>",
                    this.Id,
                    this.FullName,
                    dictionary["Common_Delete"],
                    deleteAction);
            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantEmployee)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            /*
            string deleteAction = this.HasActions ? "EmployeeDeleteAlert" : "EmployeeDelete";
            string iconEdit = grantEmployee ? string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-edit bigger-120""></i></span>", this.Id, this.FullName, dictionary["Common_Edit"]) : string.Empty;
            string iconDelete = grantEmployeeDelete ? string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", 
                this.Id, 
                this.FullName, 
                dictionary["Common_Delete"], 
                deleteAction) : @"<span class=""btn btn-xs btn-danger"" onclick=""NoDelete();""><i class=""icon-trash bigger-120""></i></span>";
            */

            bool firstDepartment = true;

            var departmentsList = new StringBuilder();
            if (this.departments != null)
            {
                foreach (var deparment in this.departments)
                {
                    if (firstDepartment)
                    {
                        firstDepartment = false;
                    }
                    else
                    {
                        departmentsList.Append(", ");
                    }

                    departmentsList.Append(grantDepartment ? deparment.Link : deparment.Description);
                }
            }

            var cargosList = new StringBuilder();
            bool firstJobPosition = true;
            foreach (var jobPositionItem in this.jobPositions)
            {
                if (firstJobPosition)
                {
                    firstJobPosition = false;
                }
                else
                {
                    cargosList.Append(", ");
                }

                cargosList.Append(grantJobPosition ? jobPositionItem.Link : jobPositionItem.Description);
            }

            string pattern = @"<tr><td>{0}</td><td style=""width:250px;"">{1}</td><td style=""width:250px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</td></tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                grantEmployee ? this.Link : this.FullName,
                cargosList,
                departmentsList,
                iconEdit,
                iconDelete);
        }

        /// <summary>Render the HTML code for a row in active employees list</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">User grants</param>
        /// <returns>HTML code</returns>
        public string JsonListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantEmployee = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);
            bool grantEmployeeDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Employee);
            bool grantDepartment = UserGrant.HasWriteGrant(grants, ApplicationGrant.Department);
            bool grantJobPosition = UserGrant.HasWriteGrant(grants, ApplicationGrant.JobPosition);

            string iconDelete = string.Empty;
            if (grantEmployeeDelete)
            {
                string deleteFunction = string.Format(CultureInfo.InvariantCulture, "ProviderDelete({0},'{1}');", this.Id, this.Description);
                string deleteAction = this.HasActions ? "EmployeeDeleteAlert" : "EmployeeDelete";
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{3}({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>",
                    this.Id,
                    this.FullName,
                    dictionary["Common_Delete"],
                    deleteAction);
            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantEmployee)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""EmployeeUpdate({0},'{1}');""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            bool firstDepartment = true;
            var departmentsList = new StringBuilder();
            if (this.departments != null)
            {
                foreach (var deparment in this.departments)
                {
                    if (firstDepartment)
                    {
                        firstDepartment = false;
                    }
                    else
                    {
                        departmentsList.Append(", ");
                    }

                    departmentsList.Append(grantDepartment ? deparment.Link : deparment.Description);
                }
            }

            var cargosList = new StringBuilder();
            bool firstJobPosition = true;
            foreach (var jobPositionItem in this.jobPositions)
            {
                if (firstJobPosition)
                {
                    firstJobPosition = false;
                }
                else
                {
                    cargosList.Append(", ");
                }

                cargosList.Append(grantJobPosition ? jobPositionItem.Link : jobPositionItem.Description);
            }

            string pattern = @"{{""Id"":{0},""Link"":""{1}"",""FullName"":""{7}"",""Cargos"":""{2}"",""Departamentos"":""{3}"",""Editable"":{4},""Deletable"":{5},""HasActions"":{8}, ""Baja"":{6}}}";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                this.Id,
                Tools.JsonCompliant(grantEmployee ? this.Link : this.FullName),
                Tools.JsonCompliant(cargosList.ToString()),
                Tools.JsonCompliant(departmentsList.ToString()),
                grantEmployee ? "true" : "false",
                grantEmployeeDelete ? "true" : "false",
                this.disabledDate.HasValue ? "true" : "false",
                Tools.JsonCompliant(this.FullName),
                this.HasActionAssigned ? "true" : "false");
        }

        /// <summary>Obtain the assistance of employee in company learning</summary>
        public void ObtainLearningAssistance()
        {
            this.learningAssistance = new List<LearningAssistance>();
            using (var cmd = new SqlCommand("Employee_GetLearningAssitance"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                        cmd.Parameters["@EmployeeId"].Value = this.Id;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newJobPosition = JobPosition.Empty;

                                // Hay que asegurarse que exista el cargo
                                if (!rdr.IsDBNull(ColumnsGetLearningAssistance.JobPositionId))
                                {
                                    newJobPosition = new JobPosition
                                    {
                                        Id = rdr.GetInt32(ColumnsGetLearningAssistance.JobPositionId),
                                        Description = rdr.GetString(ColumnsGetLearningAssistance.JobPositionDescription)
                                    };
                                }

                                var newAssistance = new LearningAssistance
                                {
                                    Id = rdr.GetInt32(ColumnsGetLearningAssistance.AssistanceId),
                                    CompanyId = this.CompanyId,
                                    Date = rdr.GetDateTime(ColumnsGetLearningAssistance.EstimatedDate),
                                    Learning = new Learning
                                    {
                                        Id = rdr.GetInt32(ColumnsGetLearningAssistance.LearningId),
                                        Description = rdr.GetString(ColumnsGetLearningAssistance.LearningDescription),
                                        Status = rdr.GetInt32(ColumnsGetLearningAssistance.LearningStatus)
                                    },
                                    Employee = this,
                                    JobPosition = newJobPosition
                                };

                                if (!rdr.IsDBNull(ColumnsGetLearningAssistance.LearningFinishDate))
                                {
                                    newAssistance.Learning.RealFinish = rdr.GetDateTime(ColumnsGetLearningAssistance.LearningFinishDate);
                                }

                                if (!rdr.IsDBNull(ColumnsGetLearningAssistance.Completed))
                                {
                                    newAssistance.Completed = rdr.GetBoolean(ColumnsGetLearningAssistance.Completed);
                                }

                                if (!rdr.IsDBNull(ColumnsGetLearningAssistance.Success))
                                {
                                    newAssistance.Success = rdr.GetBoolean(ColumnsGetLearningAssistance.Success);
                                }

                                this.learningAssistance.Add(newAssistance);
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

        /// <summary>Obtain the historial job position assignations</summary>
        public void ObtainJobPositionsHistoric()
        {
            string source = string.Format(CultureInfo.GetCultureInfo("en-us"), "GetJobPositions({0})", this.Id);
            this.JobPositionAssignment = new Collection<JobPositionAsigment>();
            /* ALTER PROCEDURE Employee_GetJobPositionHistoric
             * @EmployeeId int,
             * @CompanyId int */
            using (var cmd = new SqlCommand("Employee_GetJobPositionHistoric"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@EmployeeId", SqlDbType.Int);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters["@EmployeeId"].Value = this.Id;
                        cmd.Parameters["@CompanyId"].Value = this.CompanyId;
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newJobPositionAsigment = new JobPositionAsigment
                                {
                                    Employee = this,
                                    StartDate = rdr.GetDateTime(ColumnsEmployeeJobPosition.StartDate),
                                    JobPosition = new JobPosition
                                    {
                                        Id = rdr.GetInt32(ColumnsEmployeeJobPosition.JobPositionId),
                                        Description = rdr.GetString(ColumnsEmployeeJobPosition.JobPositionDescription),
                                        Department = new Department
                                        {
                                            Id = rdr.GetInt32(ColumnsEmployeeJobPosition.DepartmentId),
                                            Description = rdr.GetString(ColumnsEmployeeJobPosition.DepartmentName)
                                        },
                                        Active = rdr.GetBoolean(ColumnsEmployeeJobPosition.Active),
                                        CompanyId = this.CompanyId
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsEmployeeJobPosition.JobPositionResponsibleId))
                                {
                                    var responsible = new JobPosition
                                    {
                                        Id = rdr.GetInt32(ColumnsEmployeeJobPosition.JobPositionResponsibleId),
                                        Description = rdr.GetString(ColumnsEmployeeJobPosition.JobPositionResponsibleFullName)
                                    };

                                    newJobPositionAsigment.JobPosition.Responsible = responsible;
                                }

                                if (!rdr.IsDBNull(ColumnsEmployeeJobPosition.EndDate))
                                {
                                    newJobPositionAsigment.EndDate = rdr.GetDateTime(ColumnsEmployeeJobPosition.EndDate);
                                }
                                else
                                {
                                    this.jobPosition = new JobPosition { Id = rdr.GetInt32(ColumnsEmployeeJobPosition.JobPositionId), Description = rdr.GetString(ColumnsEmployeeJobPosition.JobPositionDescription) };
                                }

                                this.JobPositionAssignment.Add(newJobPositionAsigment);
                            }
                        }
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
                    catch (InvalidCastException ex)
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
        }

        /// <summary>Insert employee data in database</summary>
        /// <param name="userActionId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userActionId)
        {
            var actionResult = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_Insert
             * @EmployeeId int out,
             * @CompanyId int,
             * @Name nvarchar(50),
             * @LastName nvarchar(50),
             * @Email nvarchar(50),
             * @Phone nvarchar(50),
             * @NIF nvarchar(15),
             * @Address nvarchar(50),
             * @PostalCode nvarchar(10),
             * @City nvarchar(50),
             * @Province nvarchar(50),
             * @Country nvarchar(50),
             * @Notes text,
             * @UserId int,
             * @Password nvarchar(6) out */
            using (var cmd = new SqlCommand("Employee_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        string userName = ApplicationUser.SetNewUserName(this.Name.Substring(0, 1) + this.LastName.Split(' ')[0].Trim(), this.CompanyId);
                        cmd.Parameters.Add(DataParameter.OutputInt("@EmployeeId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@NIF", this.Nif, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Address", this.address.Address, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@PostalCode", this.address.PostalCode, 10));
                        cmd.Parameters.Add(DataParameter.Input("@City", this.address.City, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Province", this.address.Province, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Country", this.address.Country, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.notes));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userActionId));
                        cmd.Parameters.Add(DataParameter.Input("@UserName", userName));
                        cmd.Parameters.Add(DataParameter.InputNull("@Password"));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@EmployeeId"].Value, CultureInfo.InvariantCulture);
                        string pass = cmd.Parameters["@Password"].ToString().Trim();
                        actionResult.SetSuccess(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}{2}{1}", this.Id.ToString(CultureInfo.InvariantCulture), pass, Separator));
                    }
                    catch (SqlException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (NullReferenceException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (FormatException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (NotSupportedException ex)
                    {
                        actionResult.SetFail(ex.Message);
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

            return actionResult;
        }

        /// <summary>Update employee data in database</summary>
        /// <param name="userActionId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userActionId)
        {
            var actionResult = ActionResult.NoAction;
            /* CREATE PROCEDURE EmployeeUpdate
             * @EmployeeId int,
             * @CompanyId int,
             * @Name nvarchar(50),
             * @LastName nvarchar(50),
             * @Email nvarchar(50),
             * @Phone nvarchar(50),
             * @NIF nvarchar(15),
             * @Address nvarchar(50),
             * @PostalCode nvarchar(10),
             * @City nvarchar(50),
             * @Province nvarchar(50),
             * @Country nvarchar(50),
             * @Notes text,
             * @ModifiedBy int */
            using (var cmd = new SqlCommand("Employee_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@LastName", this.LastName, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@NIF", this.Nif, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Address", this.address.Address, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@PostalCode", this.address.PostalCode, 10));
                        cmd.Parameters.Add(DataParameter.Input("@City", this.address.City, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Province", this.address.Province, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Country", this.address.Country, DataParameter.DefaultTextLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", this.notes));
                        cmd.Parameters.Add(DataParameter.Input("@ModifiedBy", userActionId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();

                        if (this.disabledDate.HasValue)
                        {
                            Disable(this.Id, this.CompanyId, userActionId, this.disabledDate.Value);
                        }

                        actionResult.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (NullReferenceException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (FormatException ex)
                    {
                        actionResult.SetFail(ex.Message);
                    }
                    catch (NotSupportedException ex)
                    {
                        actionResult.SetFail(ex.Message);
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

            return actionResult;
        }

        public ActionResult SetUser()
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Employee_SetUser
             *   @EmployeeId bigint,
             *   @UserId bigint,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("Employee_SetUser"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", this.userId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
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
    }
}