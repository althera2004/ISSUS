// -----------------------------------------------------------------------
// <copyright file="JobPosition.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
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

    /// <summary>
    /// Implements JobPosition class.
    /// </summary>
    public class JobPosition : BaseItem
    {
        #region Fields

        /// <summary>
        /// Department of job position
        /// </summary>
        private Department department;

        /// <summary>
        /// Responsabiliteis of job position
        /// </summary>
        private string responsibilities;

        /// <summary>
        /// Notes of job position
        /// </summary>
        private string notes;

        /// <summary>
        /// Academic skills for job position
        /// </summary>
        private string academicSkills;

        /// <summary>
        /// Specific skills for job position
        /// </summary>
        private string specificSkills;

        /// <summary>
        /// Work experience required to job position
        /// </summary>
        private string workExperience;

        /// <summary>
        /// Habilities required to job position
        /// </summary>
        private string abilities;

        /// <summary>
        /// Job position responsible of job position
        /// </summary>
        private JobPosition responsible;
        #endregion

        /// <summary>
        /// Initializes a new instance of the JobPosition class.
        /// </summary>
        public JobPosition()
        {
            this.Id = -1;
            this.department = Department.Empty;
            this.ModifiedBy = ApplicationUser.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the JobPosition class.
        /// The data of job position is searched into data base based on job position and company identifiers
        /// </summary>
        /// <param name="jobPositionId">Job position identifier</param>
        /// <param name="companyId">Company identifier</param>
        public JobPosition(long jobPositionId, int companyId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("JobPosition_GetById"))
                {
                    cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@Id", jobPositionId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            this.Id = rdr.GetInt32(ColumnsJobPositionGetById.Id);
                            this.Description = rdr.GetString(ColumnsJobPositionGetById.Description);
                            this.responsibilities = rdr.GetString(ColumnsJobPositionGetById.Responsibilities);
                            this.notes = rdr.GetString(ColumnsJobPositionGetById.Notes);
                            this.academicSkills = rdr.GetString(ColumnsJobPositionGetById.AcademicSkills);
                            this.specificSkills = rdr.GetString(ColumnsJobPositionGetById.SpecificSkills);
                            this.workExperience = rdr.GetString(ColumnsJobPositionGetById.ProfessionalExperience);
                            this.abilities = rdr.GetString(ColumnsJobPositionGetById.Skills);
                            this.CompanyId = companyId;
                            this.ModifiedOn = rdr.GetDateTime(ColumnsJobPositionGetById.ModifiedOn);

                            this.department = new Department()
                            {
                                Id = rdr.GetInt32(ColumnsJobPositionGetById.DepartmentId),
                                CompanyId = companyId,
                                Description = rdr.GetString(ColumnsJobPositionGetById.DepartmentName)
                            };

                            this.ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsJobPositionGetById.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsJobPositionGetById.ModifiedByUserName)
                            };

                            if (rdr.GetInt32(ColumnsJobPositionGetById.ResponsibleId) != 0)
                            {
                                this.responsible = new JobPosition()
                                {
                                    Id = rdr.GetInt32(ColumnsJobPositionGetById.ResponsibleId),
                                    Description = rdr.GetString(ColumnsJobPositionGetById.ResponsibleName)
                                };
                            }

                            this.ModifiedBy.Employee = Employee.GetByUserId(this.ModifiedBy.Id);
                        }
                    }

                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
            catch (ArgumentNullException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
            catch (ArgumentException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
            catch (InvalidCastException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition.ctro({0}.{1})", jobPositionId, companyId));
            }
        }

        #region Properties
        /// <summary>
        /// Gets an empty job position object
        /// </summary>
        public static JobPosition Empty
        {
            get
            {
                return new JobPosition()
                {
                    Id = 0,
                    CompanyId = 0,
                    academicSkills = string.Empty,
                    department = Department.Empty,
                    Description = string.Empty,
                    abilities = string.Empty,
                    ModifiedBy = null,
                    responsible = JobPosition.EmptySimple
                };
            }
        }

        /// <summary>
        /// Gets an empty simple job position object
        /// </summary>
        public static JobPosition EmptySimple
        {
            get
            {
                return new JobPosition()
                {
                    Id = 0,
                    Description = string.Empty
                };
            }
        }

        /// <summary>
        /// Gets or sets the job position responsible of the job position
        /// </summary>
        public JobPosition Responsible
        {
            get
            {
                return this.responsible;
            }

            set
            {
                this.responsible = value;
            }
        }

        /// <summary>
        /// Gets or sets de job position departemnt linked
        /// </summary>
        public Department Department
        {
            get 
            {
                return this.department; 
            }

            set
            {
                this.department = value;
            }
        }

        /// <summary>
        /// Gets or sets a text description for resposabilities of job position
        /// </summary>
        public string Responsibilities
        {
            get
            {
                return this.responsibilities; 
            }

            set 
            {
                this.responsibilities = value;
            }
        }

        /// <summary>
        /// Gets or sets a text for the notes of job position
        /// </summary>
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

        /// <summary>
        /// Gets or sets a text for the academics skills of job position
        /// </summary>
        public string AcademicSkills
        {
            get 
            {
                return this.academicSkills;
            }

            set
            {
                this.academicSkills = value; 
            }
        }

        /// <summary>
        /// Gets or sets a text for the specific skills of job position
        /// </summary>
        public string SpecificSkills
        {
            get
            {
                return this.specificSkills; 
            }

            set 
            {
                this.specificSkills = value;
            }
        }

        /// <summary>
        /// Gets or sets a text for the work experience required for the job position
        /// </summary>
        public string WorkExperience
        {
            get 
            {
                return this.workExperience; 
            }

            set 
            {
                this.workExperience = value;
            }
        }

        /// <summary>
        /// Gets or sets a text for the habilities required for the job position
        /// </summary>
        public string Habilities
        {
            get 
            {
                return this.abilities; 
            }

            set
            { 
                this.abilities = value;
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                if (this.Id == 0)
                {
                    return "null";
                }

                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, this.Description.Replace("\"", "\\\""));
            }
        }

        /// <summary>
        /// Gets a simple Json structure of job position
        /// </summary>
        public string JsonSimple
        {
            get
            {
                string pattern = @"
    {{
        ""Id"": {0},
        ""Description"": ""{1}"",
        ""Department"":{{""Id"": {2}, ""Name"": ""{4}""}},
        ""Responsible"": {3}
    }}";
                if (this.Id > 0)
                {
                    return string.Format(
                        CultureInfo.GetCultureInfo("en-us"),
                        pattern,
                        this.Id,
                        this.Description.Replace("\"", "\\\""),
                        this.department.Id,
                        this.responsible.JsonKeyValue,
                        this.department.Description.Replace("\"", "\\\""));
                }

                return @"{
                    ""Id"":0,
                    ""Description"":"""",
                    ""Department"":{""Id"":0}
                    }";
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"{{
                    ""Id"": {0},
                    ""Description"": ""{1}"",
                    ""CompanyId"": {2},
                    ""Responsibilities"": ""{3}"",
                    ""Notes"": ""{4}"",
                    ""AcademicSkills"": ""{5}"",
                    ""SpecificSkills"": ""{6}"",
                    ""WorkExperience"": ""{7}"",
                    ""Abilities"": ""{8}"",
                    ""Department"": {9},
                    ""Responsible"": {10}
                    }}";
                if (this.Id > 0)
                {
                    return string.Format(
                        CultureInfo.InvariantCulture,
                        pattern,
                        this.Id,
                        this.Description.Replace("\"", "\\\""),
                        this.CompanyId,
                        Tools.JsonCompliant(this.responsibilities),
                        Tools.JsonCompliant(this.notes),
                        Tools.JsonCompliant(this.academicSkills),
                        Tools.JsonCompliant(this.specificSkills),
                        Tools.JsonCompliant(this.workExperience),
                        Tools.JsonCompliant(this.abilities),
                        this.department.JsonKeyValue,
                        this.responsible == null ? "null" : this.responsible.JsonKeyValue);
                }

                return @"{
                    ""Id"": 0,
                    ""Description"": """",
                    ""CompanyId"": 0,
                    ""Responsibilities"": """",
                    ""Notes"": """",
                    ""AcademicSkills"": """",
                    ""SpecificSkills"": """",
                    ""WorkExperience"": """",
                    ""Habilities"": """",
                    ""Employee"": { ""Id"": 0 },
                    ""Department"": { ""Id"": 0 },
                    ""Responsable"": { ""Id"": 0 }
                    }";
            }
        }

        /// <summary>
        /// Gets a HTML code for the link to the job position profile page
        /// </summary>
        public override string Link
        {
            get
            {
                string pattern = @"<a href=""CargosView.aspx?id={0}"" title=""{2} {1}"">{1}</a>";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether job positions has employees in historial
        /// </summary>
        public bool HasEmployeesHistorical
        {
            get
            {
                int res = 0;
                /* CREATE PROCEDURE JobPosition_GetEmployeesHistorical
                 * @JobPositionId int,
                 * @CompanyId int */

                using (var cmd = new SqlCommand("JobPosition_GetEmployeesHistorical"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                        {
                            cmd.Connection = cnn;
                            cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.Id));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    res = rdr.GetInt32(0);
                                }
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }

                return res != 0;
            }
        }

        /// <summary>
        /// Gets employees linked to job position
        /// </summary>
        public ReadOnlyCollection<Employee> Employees
        {
            get
            {
                List<Employee> res = new List<Employee>();
                /* CREATE PROCEDURE JobPosition_GetEmployees
                 * @JobPositionId int,
                 * @CompanyId int */
                using (SqlCommand cmd = new SqlCommand("JobPosition_GetEmployees"))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString());
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsJobPositionGetEmployees.EmployeeId),
                                    Name = rdr.GetString(ColumnsJobPositionGetEmployees.EmployeeName),
                                    LastName = rdr.GetString(ColumnsJobPositionGetEmployees.EmployeeLastName),
                                    Nif = rdr.GetString(ColumnsJobPositionGetEmployees.EmployeeNif),
                                    Email = rdr.GetString(ColumnsJobPositionGetEmployees.EmployeeEmail),
                                    Phone = rdr.GetString(ColumnsJobPositionGetEmployees.EmployeePhone),
                                    CompanyId = this.CompanyId
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Employees - Id:{0}", this.Id));
                    }
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }

                return new ReadOnlyCollection<Employee>(res);
            }
        }

        /// <summary>
        /// Gets a descriptive text with the differences between two job position objects
        /// </summary>
        /// <param name="jobPosition1">First job position object</param>
        /// <param name="jobPosition2">Second job position object</param>
        /// <returns>Descriptive text with the differences between two job position objects</returns>
        public static string Differences(JobPosition jobPosition1, JobPosition jobPosition2)
        {
            if (jobPosition1 == null || jobPosition2 == null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();
            bool first = true;

            if (jobPosition1.Description != jobPosition2.Description)
            {
                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "description:{0}", jobPosition2.Description));
                first = false;
            }

            if (jobPosition1.responsible != null && jobPosition2.responsible != null && jobPosition2.responsible.Id != jobPosition1.responsible.Id)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "responsible{0}", jobPosition2.responsible.Id));
                first = false;
            }
            
            if (jobPosition1.responsible == null && jobPosition2.responsible != null)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "responsible{0}", jobPosition2.responsible.Id));
                first = false;
            }

            if (jobPosition1.responsible != null)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                string resposibleId = "null";
                if (jobPosition2.responsible != null)
                {
                    resposibleId = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0}", jobPosition2.responsible.Id);
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "responsible{0}", resposibleId));
                first = false;
            }
            
            if (jobPosition1.department.Id != jobPosition2.department.Id)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "department:{0}", jobPosition2.department.Id));
                first = false;
            }

            if (jobPosition1.responsibilities != jobPosition2.responsibilities)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "responsibilities:{0}", jobPosition2.responsibilities));
                first = false;
            }

            if (jobPosition1.notes != jobPosition2.notes)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "notes:{0}", jobPosition2.notes));
                first = false;
            }

            if (jobPosition1.academicSkills != jobPosition2.academicSkills)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "academicSkills:{0}", jobPosition2.academicSkills));
                first = false;
            }

            if (jobPosition1.specificSkills != jobPosition2.specificSkills)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "specificSkills:{0}", jobPosition2.specificSkills));
                first = false;
            }

            if (jobPosition1.abilities != jobPosition2.abilities)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "Habilities:{0}", jobPosition2.abilities));
                first = false;
            }

            if (jobPosition1.workExperience != jobPosition2.workExperience)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "Experience:{0}", jobPosition2.workExperience));
                first = false;
            }

            return res.ToString();
        }

        /// <summary>Obtains a list of job positions by company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List in JSON format of job positions</returns>
        public static string ByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var jobPosition in JobsPositionByCompany(companyId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(jobPosition.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Gets all job positions of company
        /// </summary>
        /// <param name="company">Company to search in</param>
        /// <returns>A list of job positions</returns>
        public static ReadOnlyCollection<JobPosition> JobsPositionByCompany(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<JobPosition>(new List<JobPosition>());
            }

            return JobsPositionByCompany(company.Id);
        }

        /// <summary>Gets all job positions of company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of job positions</returns>
        public static ReadOnlyCollection<JobPosition> JobsPositionByCompany(int companyId)
        {
            var res = new List<JobPosition>();
            try
            {
                using (var cmd = new SqlCommand("JobPosition_GetByCompany"))
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new JobPosition()
                                {
                                    Id = rdr.GetInt32(ColumnsJobPositionGetAll.Id),
                                    Description = rdr.GetString(ColumnsJobPositionGetAll.Description),
                                    responsibilities = rdr.GetString(ColumnsJobPositionGetAll.Responsibilities),
                                    notes = rdr.GetString(ColumnsJobPositionGetAll.Notes),
                                    academicSkills = rdr.GetString(ColumnsJobPositionGetAll.AcademicSkills),
                                    specificSkills = rdr.GetString(ColumnsJobPositionGetAll.SpecificSkills),
                                    workExperience = rdr.GetString(ColumnsJobPositionGetAll.WorkExperience),
                                    abilities = rdr.GetString(ColumnsJobPositionGetAll.Abilities),
                                    department = new Department()
                                    {
                                        Id = rdr.GetInt32(ColumnsJobPositionGetAll.DepartmentId),
                                        Description = rdr.GetString(ColumnsJobPositionGetAll.DepartmentName),
                                        Deleted = rdr.GetBoolean(ColumnsJobPositionGetAll.DepartmentDeleted),
                                        CompanyId = companyId
                                    },
                                    responsible = new JobPosition()
                                    {
                                        Id = rdr.GetInt32(ColumnsJobPositionGetAll.ResponsibleId),
                                        Description = rdr.GetString(ColumnsJobPositionGetAll.ResponsibleFullName),
                                        CompanyId = companyId
                                    },
                                    CompanyId = companyId
                                });
                            }

                            cmd.Connection.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition::JobsPositionByCompany({0})", companyId));
            }
            catch (FormatException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition::JobsPositionByCompany({0})", companyId));
            }
            catch (NullReferenceException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition::JobsPositionByCompany({0})", companyId));
            }
            catch (ArgumentNullException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition::JobsPositionByCompany({0})", companyId));
            }
            catch (ArgumentException ex)
            {
                ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "JobPosition::JobsPositionByCompany({0})", companyId));
            }

            return new ReadOnlyCollection<JobPosition>(res);
        }

        /// <summary>Unlinks the specified employee identifier from Job position.</summary>
        /// <param name="employeeId">The employee identifier.</param>
        /// <param name="jobPositionId">The job position identifier.</param>
        /// <returns></returns>
        public static ActionResult Unlink(int employeeId, int jobPositionId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE JobPosition_Unlink
             *  @EmployeeId int,
             *  @JobPositionId int */
            using (var cmd = new SqlCommand("JobPosition_Unlink"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@EmployeeId", employeeId));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", jobPositionId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                }
                catch (SqlException ex)
                {
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    res.SetFail(ex);
                }
                catch (NotSupportedException ex)
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

            return res;
        }

        /// <summary>
        /// Delete a job position in data base
        /// </summary>
        /// <param name="jobPositionId">Job position identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="reason">Reason to delete</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int jobPositionId, int companyId, int userId, string reason)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE JobPosition_Delete
             * @JobPositionId bigint,
             * @CompanyId int,
             * @Extradata nvarchar(200),
             * @UserId int */
            using (var cmd = new SqlCommand("JobPosition_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@JobPositionId", SqlDbType.BigInt);
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters.Add("@ExtraData", SqlDbType.NVarChar);
                        cmd.Parameters["@JobPositionId"].Value = Convert.ToInt64(jobPositionId);
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Parameters["@ExtraData"].Value = Tools.LimitedText(reason, 200);
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
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

        /// <summary>Render HTML code in order to get a row into the job positions table</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grantToWritePositon">Indicates if has write permission for job positions</param>
        /// <param name="grantToReadDepartments">Indicates if has read permission for departments</param>
        /// <returns>Html code</returns>
        public string TableRow(Dictionary<string, string> dictionary, bool grantToWritePositon, bool grantToReadDepartments)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string iconEdit = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""document.location='CargosView.aspx?id={0}';""><i class=""{3} bigger-120""></i></span>", 
                this.Id, 
                this.Description, 
                dictionary["Common_Edit"],
                grantToWritePositon ? "icon-edit" : "icon-eye-open");

            string iconDelete = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""JobPositionDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", 
                this.Id, 
                this.Description, 
                dictionary["Common_Delete"]);

            if (this.Employees != null && this.Employees.Count > 0)
            {
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-danger"" onclick=""alertUI('{0}');""><i class=""icon-trash bigger-120""></i></span>", dictionary["Item_JobPosition_ErrorMessage_HasEmployees"], dictionary["Common_Delete"], this.Description);
            }
            else if (this.HasEmployeesHistorical)
            {
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-danger"" onclick=""alertUI('{0}');""><i class=""icon-trash bigger-120""></i></span>", dictionary["Item_JobPosition_ErrorMessage_HasEmployeesHistoric"], dictionary["Common_Delete"], this.Description);
            }

            if (!grantToWritePositon)
            {
                iconDelete = string.Empty;
            }

            string responsibleName = this.responsible.Description;
            if (string.IsNullOrEmpty(responsibleName))
            {
                responsibleName = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span style=""color:#000"">{0}</span>", dictionary["Item_JobPosition_Message_WithoutResponsible"]);
            }
            else
            {
                responsibleName = this.responsible.Link;
            }

            string departmentName = string.Empty;
            if (this.department.Deleted)
            {
                departmentName = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<a href=""#"" style=""color:#f00"" onclick=""alert('{1}');"">{0}</a>", this.department.Description, dictionary["Item_Department_Status_Deleted"]);
            }
            else
            {
                departmentName = grantToReadDepartments ? this.department.Link : this.department.Description;
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr><td>{0}</td><td style=""width:400px;"">{1}</td><td style=""width:400px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</tr>",
                this.Link,
                responsibleName,
                departmentName,
                iconEdit,
                iconDelete);
        }

        /// <summary>Render HTML code to get a row for the job position table of employee profile</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>HTML code</returns>
        public string EmployeeRow(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            string iconRename = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""JobPositionUpdate({0});""><i class=""icon-edit bigger-120""></i></span>",
                this.Id, 
                this.Description,
                dictionary["Common_Edit"]);

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr><td>{0}</td><td>{1}</td></tr>",
                this.Link,
                iconRename);
        }

        /// <summary>Update job position data in data base</summary>
        /// <param name="userId">Identifier of user that updates job position</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE JobPosition_Update
             * @JobPositionId bigint,
             * @CompanyId int,
             * @ResponsableId int,
             * @DepartmentId int,
             * @Description nvarchar(50),
             * @Responsabilidades text,
             * @Notas text,
             * @FormacionAcademicaDeseada text,
             * @FormacionEspecificaDesdeada text,
             * @ExperienciaLaboralDeseada text,
             * @HabilidadesDeseadas text,
             * @ModifiedBy int */
            using (var cmd = new SqlCommand("JobPosition_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.department.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.responsible));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Responsabilidades", this.responsibilities, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Notas", this.notes, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionAcademicaDeseada", this.academicSkills, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionEspecificaDesdeada", this.specificSkills, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ExperienciaLaboralDeseada", this.workExperience, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@HabilidadesDeseadas", this.abilities, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ModifiedBy", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
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

        /// <summary>Insert a job position into database</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE JobPosition_Insert
             * @JobPositionId bigint out,
             * @CompanyId int,
             * @ResponsableId int,
             * @DepartmentId int,
             * @Description nvarchar(50),
             * @Responsabilidades text,
             * @Notas text,
             * @FormacionAcademicaDeseada text,
             * @FormacionEspecificaDeseada text,
             * @ExperienciaLaboralDeseada text,
             * @HabilidadesDeseadas text,
             * @UserId int */
            using (var cmd = new SqlCommand("JobPosition_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputLong("@JobPositionId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.department.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.responsible));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Responsabilidades", this.responsibilities, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@Notas", this.notes, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionAcademicaDeseada", this.academicSkills, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionEspecificaDeseada", this.specificSkills, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@ExperienciaLaboralDeseada", this.workExperience, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@HabilidadesDeseadas", this.abilities, 2000));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@JobPositionId"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
                        res.SetSuccess(this.Id.ToString(CultureInfo.GetCultureInfo("en-us")));
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NotSupportedException ex)
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