// -----------------------------------------------------------------------
// <copyright file="JobPosition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
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

    /// <summary>Implements JobPosition class.</summary>
    public class JobPosition : BaseItem
    {
        /// <summary>Initializes a new instance of the JobPosition class.</summary>
        public JobPosition()
        {
            this.Id = -1;
            this.Department = Department.Empty;
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
            string source = string.Format(CultureInfo.InvariantCulture, "JobPosition.ctro({0}.{1})", jobPositionId, companyId);
            try
            {
                using (var cmd = new SqlCommand("JobPosition_GetById"))
                {
                    using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                    {
                        try
                        {
                            cmd.Connection = cnn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(DataParameter.Input("@Id", jobPositionId));
                            cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                            cmd.Connection.Open();
                            using (var rdr = cmd.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    rdr.Read();
                                    this.Id = rdr.GetInt32(ColumnsJobPositionGetById.Id);
                                    this.Description = rdr.GetString(ColumnsJobPositionGetById.Description);
                                    this.Responsibilities = rdr.GetString(ColumnsJobPositionGetById.Responsibilities);
                                    this.Notes = rdr.GetString(ColumnsJobPositionGetById.Notes);
                                    this.AcademicSkills = rdr.GetString(ColumnsJobPositionGetById.AcademicSkills);
                                    this.SpecificSkills = rdr.GetString(ColumnsJobPositionGetById.SpecificSkills);
                                    this.WorkExperience = rdr.GetString(ColumnsJobPositionGetById.ProfessionalExperience);
                                    this.Habilities = rdr.GetString(ColumnsJobPositionGetById.Skills);
                                    this.CompanyId = companyId;
                                    this.ModifiedOn = rdr.GetDateTime(ColumnsJobPositionGetById.ModifiedOn);

                                    this.Department = new Department
                                    {
                                        Id = rdr.GetInt32(ColumnsJobPositionGetById.DepartmentId),
                                        CompanyId = companyId,
                                        Description = rdr.GetString(ColumnsJobPositionGetById.DepartmentName)
                                    };

                                    this.ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsJobPositionGetById.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsJobPositionGetById.ModifiedByUserName)
                                    };

                                    if (rdr.GetInt32(ColumnsJobPositionGetById.ResponsibleId) != 0)
                                    {
                                        this.Responsible = new JobPosition
                                        {
                                            Id = rdr.GetInt32(ColumnsJobPositionGetById.ResponsibleId),
                                            Description = rdr.GetString(ColumnsJobPositionGetById.ResponsibleName)
                                        };
                                    }

                                    this.ModifiedBy.Employee = Employee.ByUserId(this.ModifiedBy.Id);
                                }
                            }
                        }
                        finally
                        {
                            if(cmd.Connection.State != ConnectionState.Closed)
                            {
                                cmd.Connection.Close();
                            }
                        }
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
        }

        /// <summary>Gets an empty job position object</summary>
        public static JobPosition Empty
        {
            get
            {
                return new JobPosition
                {
                    Id = 0,
                    CompanyId = 0,
                    AcademicSkills = string.Empty,
                    Department = Department.Empty,
                    Description = string.Empty,
                    Habilities = string.Empty,
                    ModifiedBy = null,
                    Responsible = JobPosition.EmptySimple
                };
            }
        }

        /// <summary>Gets an empty simple job position object</summary>
        public static JobPosition EmptySimple
        {
            get
            {
                return new JobPosition
                {
                    Id = 0,
                    Description = string.Empty
                };
            }
        }

        /// <summary>Gets or sets the job position responsible of the job position</summary>
        public JobPosition Responsible { get; set; }

        /// <summary>Gets or sets de job position departemnt linked</summary>
        public Department Department { get; set; }

        /// <summary>Gets or sets a text description for resposabilities of job position</summary>
        public string Responsibilities { get; set; }

        /// <summary>Gets or sets a text for the notes of job position</summary>
        public string Notes { get; set; }

        /// <summary>Gets or sets a text for the academics skills of job position</summary>
        public string AcademicSkills { get; set; }

        /// <summary>Gets or sets a text for the specific skills of job position</summary>
        public string SpecificSkills { get; set; }

        /// <summary>Gets or sets a text for the work experience required for the job position</summary>
        public string WorkExperience { get; set; }

        /// <summary>Gets or sets a text for the habilities required for the job position</summary>
        public string Habilities { get; set; }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                if (this.Id == 0)
                {
                    return "null";
                }

                return string.Format(CultureInfo.InvariantCulture, @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, this.Description.Replace("\"", "\\\""));
            }
        }

        /// <summary>Gets a simple Json structure of job position</summary>
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
                        this.Department.Id,
                        this.Responsible.JsonKeyValue,
                        this.Department.Description.Replace("\"", "\\\""));
                }

                return @"{""Id"":0 ,""Description"":"""", ""Department"":{""Id"":0}}";
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
                        Tools.JsonCompliant(this.Responsibilities),
                        Tools.JsonCompliant(this.Notes),
                        Tools.JsonCompliant(this.AcademicSkills),
                        Tools.JsonCompliant(this.SpecificSkills),
                        Tools.JsonCompliant(this.WorkExperience),
                        Tools.JsonCompliant(this.Habilities),
                        this.Department.JsonKeyValue,
                        this.Responsible == null ? "null" : this.Responsible.JsonKeyValue);
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

        /// <summary>Gets a HTML code for the link to the job position profile page</summary>
        public override string Link
        {
            get
            {
                string pattern = @"<a href=""CargosView.aspx?id={0}"" title=""{2} {1}"">{1}</a>";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Description,
                    ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        /// <summary>Gets a value indicating whether job positions has employees in historial</summary>
        public bool HasEmployeesHistorical
        {
            get
            {
                var source = string.Format(CultureInfo.InvariantCulture, "Employees - Id:{0}", this.Id);
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

                return res != 0;
            }
        }

        /// <summary>Gets employees linked to job position</summary>
        public ReadOnlyCollection<Employee> Employees
        {
            get
            {
                string source = string.Format(CultureInfo.InvariantCulture, "Employees - Id:{0}", this.Id);
                var res = new List<Employee>();
                /* CREATE PROCEDURE JobPosition_GetEmployees
                 * @JobPositionId int,
                 * @CompanyId int */
                using (var cmd = new SqlCommand("JobPosition_GetEmployees"))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
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
                                    res.Add(new Employee
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

                return new ReadOnlyCollection<Employee>(res);
            }
        }

        /// <summary>Gets a descriptive text with the differences between two job position objects</summary>
        /// <param name="jobPosition1">First job position object</param>
        /// <param name="jobPosition2">Second job position object</param>
        /// <returns>Descriptive text with the differences between two job position objects</returns>
        public static string Differences(JobPosition jobPosition1, JobPosition jobPosition2)
        {
            if (jobPosition1 == null || jobPosition2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;

            if (jobPosition1.Description != jobPosition2.Description)
            {
                res.Append(string.Format(CultureInfo.InvariantCulture, "description:{0}", jobPosition2.Description));
                first = false;
            }

            if (jobPosition1.Responsible != null && jobPosition2.Responsible != null && jobPosition2.Responsible.Id != jobPosition1.Responsible.Id)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "responsible{0}", jobPosition2.Responsible.Id));
                first = false;
            }
            
            if (jobPosition1.Responsible == null && jobPosition2.Responsible != null)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "responsible{0}", jobPosition2.Responsible.Id));
                first = false;
            }

            if (jobPosition1.Responsible != null)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                string resposibleId = "null";
                if (jobPosition2.Responsible != null)
                {
                    resposibleId = string.Format(CultureInfo.InvariantCulture, "{0}", jobPosition2.Responsible.Id);
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "responsible{0}", resposibleId));
                first = false;
            }
            
            if (jobPosition1.Department.Id != jobPosition2.Department.Id)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "department:{0}", jobPosition2.Department.Id));
                first = false;
            }

            if (jobPosition1.Responsibilities != jobPosition2.Responsibilities)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "responsibilities:{0}", jobPosition2.Responsibilities));
                first = false;
            }

            if (jobPosition1.Notes != jobPosition2.Notes)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "notes:{0}", jobPosition2.Notes));
                first = false;
            }

            if (jobPosition1.AcademicSkills != jobPosition2.AcademicSkills)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "academicSkills:{0}", jobPosition2.AcademicSkills));
                first = false;
            }

            if (jobPosition1.SpecificSkills != jobPosition2.SpecificSkills)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "specificSkills:{0}", jobPosition2.SpecificSkills));
                first = false;
            }

            if (jobPosition1.Habilities != jobPosition2.Habilities)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "Habilities:{0}", jobPosition2.Habilities));
                first = false;
            }

            if (jobPosition1.WorkExperience != jobPosition2.WorkExperience)
            {
                if (!first)
                {
                    res.Append(", ");
                }

                res.Append(string.Format(CultureInfo.InvariantCulture, "Experience:{0}", jobPosition2.WorkExperience));
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

        /// <summary>Gets all job positions of company</summary>
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
                                res.Add(new JobPosition
                                {
                                    Id = rdr.GetInt32(ColumnsJobPositionGetAll.Id),
                                    Description = rdr.GetString(ColumnsJobPositionGetAll.Description),
                                    Responsibilities = rdr.GetString(ColumnsJobPositionGetAll.Responsibilities),
                                    Notes = rdr.GetString(ColumnsJobPositionGetAll.Notes),
                                    AcademicSkills = rdr.GetString(ColumnsJobPositionGetAll.AcademicSkills),
                                    SpecificSkills = rdr.GetString(ColumnsJobPositionGetAll.SpecificSkills),
                                    WorkExperience = rdr.GetString(ColumnsJobPositionGetAll.WorkExperience),
                                    Habilities = rdr.GetString(ColumnsJobPositionGetAll.Abilities),
                                    Department = new Department
                                    {
                                        Id = rdr.GetInt32(ColumnsJobPositionGetAll.DepartmentId),
                                        Description = rdr.GetString(ColumnsJobPositionGetAll.DepartmentName),
                                        Deleted = rdr.GetBoolean(ColumnsJobPositionGetAll.DepartmentDeleted),
                                        CompanyId = companyId
                                    },
                                    Responsible = new JobPosition
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

        /// <summary>Delete a job position in data base</summary>
        /// <param name="jobPositionId">Job position identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs action</param>
        /// <param name="reason">Reason to delete</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int jobPositionId, int companyId, int userId, string reason)
        {
            var res = ActionResult.NoAction;
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
                CultureInfo.InvariantCulture,
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-info"" onclick=""document.location='CargosView.aspx?id={0}';""><i class=""{3} bigger-120""></i></span>", 
                this.Id, 
                this.Description, 
                dictionary["Common_Edit"],
                grantToWritePositon ? "icon-edit" : "icon-eye-open");

            string iconDelete = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{2} '{1}'"" class=""btn btn-xs btn-danger"" onclick=""JobPositionDelete({0},'{1}');""><i class=""icon-trash bigger-120""></i></span>", 
                this.Id, 
                this.Description, 
                dictionary["Common_Delete"]);

            if (this.Employees != null && this.Employees.Count > 0)
            {
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture, 
                    @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-danger"" onclick=""alertUI('{0}');""><i class=""icon-trash bigger-120""></i></span>", 
                    dictionary["Item_JobPosition_ErrorMessage_HasEmployees"], 
                    dictionary["Common_Delete"], 
                    this.Description);
            }
            else if (this.HasEmployeesHistorical)
            {
                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-danger"" onclick=""alertUI('{0}');""><i class=""icon-trash bigger-120""></i></span>", 
                    dictionary["Item_JobPosition_ErrorMessage_HasEmployeesHistoric"], 
                    dictionary["Common_Delete"], 
                    this.Description);
            }

            if (!grantToWritePositon)
            {
                iconDelete = string.Empty;
            }

            string responsibleName = this.Responsible.Description;
            if (string.IsNullOrEmpty(responsibleName))
            {
                responsibleName = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span style=""color:#000"">{0}</span>", 
                    dictionary["Item_JobPosition_Message_WithoutResponsible"]);
            }
            else
            {
                responsibleName = this.Responsible.Link;
            }

            string departmentName = string.Empty;
            if (this.Department.Deleted)
            {
                departmentName = string.Format(
                    CultureInfo.InvariantCulture, 
                    @"<a href=""#"" style=""color:#f00"" onclick=""alert('{1}');"">{0}</a>", 
                    this.Department.Description, 
                    dictionary["Item_Department_Status_Deleted"]);
            }
            else
            {
                departmentName = grantToReadDepartments ? this.Department.Link : this.Department.Description;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"<tr><td>{0}</td><td style=""width:250px;"">{1}</td><td style=""width:250px;"">{2}</td><td style=""width:90px;"">{3}&nbsp;{4}</tr>",
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
                CultureInfo.InvariantCulture,
                @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""JobPositionUpdate({0});""><i class=""icon-edit bigger-120""></i></span>",
                this.Id, 
                this.Description,
                dictionary["Common_Edit"]);

            return string.Format(
                CultureInfo.InvariantCulture,
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
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Department.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Responsabilidades", this.Responsibilities, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notas", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionAcademicaDeseada", this.AcademicSkills, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionEspecificaDesdeada", this.SpecificSkills, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@ExperienciaLaboralDeseada", this.WorkExperience, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@HabilidadesDeseadas", this.Habilities, Constant.MaximumTextAreaLength));
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
                        cmd.Parameters.Add(DataParameter.Input("@DepartmentId", this.Department.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Responsabilidades", this.Responsibilities, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Notas", this.Notes, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionAcademicaDeseada", this.AcademicSkills, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@FormacionEspecificaDeseada", this.SpecificSkills, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@ExperienciaLaboralDeseada", this.WorkExperience, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@HabilidadesDeseadas", this.Habilities, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@JobPositionId"].Value.ToString(), CultureInfo.InvariantCulture);
                        res.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
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