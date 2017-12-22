// --------------------------------
// <copyright file="Process.cs" company="Sbrinna">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>
    /// Implements Process class
    /// </summary>
    public class Process : BaseItem
    {
        #region Fields

        /// <summary>Identifier of proccess</summary>
        private int processType;

        /// <summary>Identifier of proccess</summary>
        private string work;

        /// <summary>Identifier of proccess</summary>
        private string start;

        /// <summary>Identifier of proccess</summary>
        private string end;

        /// <summary>Job position of process</summary>
        private JobPosition jobPosition;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Process class.
        /// </summary>
        public Process()
        {
            this.jobPosition = JobPosition.Empty;
            this.ModifiedBy = ApplicationUser.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Process class. Search the data proccess on database
        /// </summary>
        /// <param name="id">Proccess identifier</param>
        /// <param name="companyId">Process's company identifier</param>
        public Process(long id, int companyId)
        {
            /* CREATE PROCEDURE Process_GetById
             * @Id int,
             * @CompanyId int */

            using (SqlCommand cmd = new SqlCommand("Process_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Id", SqlDbType.Int);
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@Id"].Value = id;
                cmd.Parameters["@CompanyId"].Value = companyId;
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        this.Id = id;
                        this.CompanyId = companyId;
                        this.processType = rdr.GetInt32(ColumnsProcessGetById.Type);
                        this.start = rdr.GetString(ColumnsProcessGetById.Start);
                        this.work = rdr.GetString(ColumnsProcessGetById.Work);
                        this.end = rdr.GetString(ColumnsProcessGetById.End);
                        this.jobPosition = new JobPosition(Convert.ToInt32(rdr.GetInt64(ColumnsProcessGetById.JobPositionId)), companyId);
                        this.ModifiedOn = rdr.GetDateTime(ColumnsProcessGetById.ModifiedOn);
                        this.Description = rdr.GetString(ColumnsProcessGetById.Description);
                        this.Active = rdr.GetBoolean(ColumnsProcessGetById.Active);
                        this.CanBeDeleted = rdr.GetInt32(ColumnsProcessGetById.Deletable) == 1;
                        this.ModifiedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsProcessGetById.ModifiedByUserId),
                            UserName = rdr.GetString(ColumnsProcessGetById.ModifiedByUserName)
                        };

                        this.ModifiedBy.Employee = Employee.GetByUserId(this.ModifiedBy.Id);
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

        #region Properties
        /// <summary>
        /// Gets a empty process
        /// </summary>
        public static Process Empty
        {
            get
            {
                Process res = new Process()
                {
                    Description = string.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    jobPosition = new JobPosition() { Id = -1, Description = string.Empty }
                };

                return res;
            }
        }

        /// <summary>
        /// Gets or sets the type of proccess
        /// </summary>
        public int ProcessType
        {
            get
            {
                return this.processType;
            }

            set
            {
                this.processType = value;
            }
        }

        /// <summary>
        /// Gets or sets the text for the work phase
        /// </summary>
        public string Work
        {
            get
            {
                return this.work;
            }

            set
            {
                this.work = value;
            }
        }

        /// <summary>
        /// Gets or sets the text for the start phase
        /// </summary>
        public string Start
        {
            get
            {
                return this.start;
            }

            set
            {
                this.start = value;
            }
        }

        /// <summary>
        /// Gets or sets the text for the finish phase
        /// </summary>
        public string End
        {
            get
            {
                return this.end;
            }

            set
            {
                this.end = value;
            }
        }

        /// <summary>Gets or sets the job position linked to process</summary>
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

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0}, ""Description"":""{1}"", ""Active"":{2}, ""ProcessType"":{3}, ""Deletable"":{4}}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Active ? "true" : "false",
                    this.processType,
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""JobPosition"":{{""Id"":{3}}},
                        ""ProcessType"":{4},
                        ""Start"":""{5}"",
                        ""Work"":""{6}"",
                        ""End"":""{7}"",
                        ""Active"":{8},
                        ""Deletable"":{9}
                    }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.Description.Replace("\"", "\\\""),
                    this.CompanyId,
                    this.jobPosition.Id,
                    this.processType,
                    Tools.JsonCompliant(this.start),
                    Tools.JsonCompliant(this.work),
                    Tools.JsonCompliant(this.end),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>
        /// Gets a hyper link to process profile page
        /// </summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<a href=""ProcesosView.aspx?id={0}"" title=""{1}"">{1}</a>", this.Id, this.Description);
            }
        }
        #endregion
        
        /*public override string Differences(BaseItem item1)
        {
            Process c1 = item1 as Process;
            if (c1.Description != this.Description)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "Description{0}", this.Description);
            }

            return string.Empty;
        }*/

        /// <summary>
        /// Gets a JSON list of compnay's process
        /// </summary>
        /// <param name="companyId">Identifier of company</param>
        /// <returns>JSON list of compnay's process</returns>
        public static string GetByCompanyJsonList(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Process process in GetByCompany(companyId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(process.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>
        /// Deactive process
        /// </summary>
        /// <param name="processId">Process identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of users that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Deactive(int processId, int companyId, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Process_Desactive
             * @Id int out,
             * @CompanyId int,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Process_Desactive"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int);
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters.Add("@UserId", SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = processId;
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    cmd.Parameters["@UserId"].Value = userId;
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

            return res;
        }

        /// <summary>
        /// Gets a descriptive string with the differences between tow process
        /// </summary>
        /// <param name="process1">First process to compare</param>
        /// <param name="process2">Second process to compare</param>
        /// <returns>A descriptive string</returns>
        public static string Differences(Process process1, Process process2)
        {
            if (process1 == null || process2 == null)
            {
                return string.Empty;
            }

            StringBuilder res = new StringBuilder();
            bool first = true;
            if (process1.Description != process2.Description)
            {
                res.Append("Description:").Append(process2.Description);
                first = false;
            }

            if (process1.jobPosition.Id != process2.jobPosition.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("JobPosition:").Append(process2.jobPosition.Id);
                first = false;
            }

            if (process1.processType != process2.processType)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ProcessType:").Append(process2.processType);
            }

            if (process1.start != process2.start)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Start:").Append(process2.start);
                first = false;
            }

            if (process1.work != process2.work)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Work:").Append(process2.work);
                first = false;
            }

            if (process1.end != process2.end)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("End:").Append(process2.end);
                first = false;
            }

            return res.ToString();
        }

        /// <summary>
        /// Obtain all process of a company
        /// </summary>
        /// <param name="companyId">Compnay identifier</param>
        /// <returns>A list of process</returns>
        public static ReadOnlyCollection<Process> GetByCompany(int companyId)
        {
            List<Process> res = new List<Process>();

            /* CREATE PROCEDURE Get_ProcesosById
             * @Id int,
             * @CompanyId int */

            using (SqlCommand cmd = new SqlCommand("Get_Procesos"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                cmd.Parameters["@CompanyId"].Value = companyId;
                try
                {
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    long lastProcessId = 0;
                    while (rdr.Read())
                    {
                        long actualProcessId = rdr.GetInt64(0);
                        if (actualProcessId != lastProcessId)
                        {
                            lastProcessId = actualProcessId;
                            Process newProcess = new Process()
                            {
                                Id = actualProcessId,
                                CompanyId = companyId,
                                processType = rdr.GetInt32(2),
                                start = rdr.GetString(3),
                                work = rdr.GetString(4),
                                end = rdr.GetString(5),
                                Description = rdr.GetString(6),
                                jobPosition = JobPosition.Empty,
                                Active = true,
                                CanBeDeleted = rdr.GetBoolean(9)
                            };

                            if (!rdr.IsDBNull(7))
                            {
                                newProcess.jobPosition = new JobPosition()
                                {
                                    Id = rdr.GetInt32(7),
                                    Description = rdr.GetString(8)
                                };
                            }

                            res.Add(newProcess);
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

                return new ReadOnlyCollection<Process>(res);
            }
        }

        /// <summary>
        /// Gets a string to identificate process
        /// </summary>
        /// <returns>String to identificate process</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.GetCultureInfo("en-us"), "[{0}] {1}", this.Id, this.Description);
        }

        /// <summary>
        /// Insert the process into data base
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Process_Insert
             * @Id int out,
             * @CompanyId int,
             * @JobPositionId int,
             * @Description nvarchar(100),
             * @Type int,
             * @Start text,
             * @Work text,
             * @End text,
             * @UserId int */
            using (SqlCommand cmd = new SqlCommand("Process_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.jobPosition.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Type", this.processType));
                    cmd.Parameters.Add(DataParameter.Input("@Start", this.start, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@Work", this.work, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@End", this.end, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                    this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString(), CultureInfo.GetCultureInfo("en-us"));
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
        /// Update the process in data base
        /// </summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE Process_Update
             *  @Id int,
             *  @Description nvarchar(100),
             *  @CompanyId int,
             *  @JobPositionId int,
             *  @Type int,
             *  @Start text,
             *  @Work text,
             *  @End text,
             *  @UserId int */
            using (SqlCommand cmd = new SqlCommand("Process_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.jobPosition.Id));
                    cmd.Parameters.Add(DataParameter.Input("@Type", this.processType));
                    cmd.Parameters.Add(DataParameter.Input("@Start", this.start, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@Work", this.work, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@End", this.end, 2000));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
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
    }
}