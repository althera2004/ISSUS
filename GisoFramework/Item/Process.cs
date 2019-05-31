// --------------------------------
// <copyright file="Process.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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

    /// <summary>Implements Process class</summary>
    public class Process : BaseItem
    {
        /// <summary>Initializes a new instance of the Process class.</summary>
        public Process()
        {
            this.JobPosition = JobPosition.Empty;
            this.ModifiedBy = ApplicationUser.Empty;
        }

        /// <summary>Initializes a new instance of the Process class. Search the data proccess on database</summary>
        /// <param name="id">Proccess identifier</param>
        /// <param name="companyId">Process's company identifier</param>
        public Process(long id, int companyId)
        {
            /* CREATE PROCEDURE Process_GetById
             * @Id int,
             * @CompanyId int */
            using (var cmd = new SqlCommand("Process_GetById"))
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
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            this.Id = id;
                            this.CompanyId = companyId;
                            this.ProcessType = rdr.GetInt32(ColumnsProcessGetById.Type);
                            this.Start = rdr.GetString(ColumnsProcessGetById.Start);
                            this.Work = rdr.GetString(ColumnsProcessGetById.Work);
                            this.End = rdr.GetString(ColumnsProcessGetById.End);
                            this.JobPosition = new JobPosition(Convert.ToInt32(rdr.GetInt64(ColumnsProcessGetById.JobPositionId)), companyId);
                            this.ModifiedOn = rdr.GetDateTime(ColumnsProcessGetById.ModifiedOn);
                            this.Description = rdr.GetString(ColumnsProcessGetById.Description);
                            this.Active = rdr.GetBoolean(ColumnsProcessGetById.Active);
                            this.CanBeDeleted = rdr.GetInt32(ColumnsProcessGetById.Deletable) == 1;
                            this.ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsProcessGetById.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsProcessGetById.ModifiedByUserName)
                            };

                            this.ModifiedBy.Employee = Employee.ByUserId(this.ModifiedBy.Id);

                            if (!rdr.IsDBNull(ColumnsProcessGetById.DisabledBy))
                            {
                                this.DisabledBy = new ApplicationUser
                                {
                                    Id = rdr.GetInt32(ColumnsProcessGetById.DisabledBy),
                                    UserName = rdr.GetString(ColumnsProcessGetById.DisabledByUserName)
                                };
                            }

                            if (!rdr.IsDBNull(ColumnsProcessGetById.DisabledOn))
                            {
                                this.DisabledOn = rdr.GetDateTime(ColumnsProcessGetById.DisabledOn);
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

        /// <summary>Gets a empty process</summary>
        public static Process Empty
        {
            get
            {
                return new Process
                {
                    Description = string.Empty,
                    ModifiedBy = ApplicationUser.Empty,
                    JobPosition = new JobPosition
                    {
                        Id = -1,
                        Description = string.Empty
                    }
                };
            }
        }

        /// <summary>Gets or sets the type of proccess</summary>
        public int ProcessType { get; set; }

        /// <summary>Gets or sets the text for the work phase</summary>
        public string Work { get; set; }

        /// <summary>Gets or sets the text for the start phase</summary>
        public string Start { get; set; }

        /// <summary>Gets or sets the text for the finish phase</summary>
        public string End { get; set; }

        /// <summary>Gets or sets the user that disables process</summary>
        public ApplicationUser DisabledBy { get; set; }

        /// <summary>Gets or sets the date of disable process</summary>
        public DateTime? DisabledOn { get; set; }

        /// <summary>Gets or sets the job position linked to process</summary>
        public JobPosition JobPosition { get; set; }

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
                    this.ProcessType,
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string disabledOn = Constant.JavaScriptNull;
                if (this.DisabledOn.HasValue)
                {
                    disabledOn = string.Format(CultureInfo.InvariantCulture, @"""{0:dd/MM/yyyy}""", this.DisabledOn.Value);
                }

                ApplicationUser disabledByUser = ApplicationUser.Empty;
                if(this.DisabledBy!=null && this.DisabledBy.Id > 0)
                {
                    disabledByUser = this.DisabledBy;
                }

                string disabledBy = disabledByUser.JsonKeyValue;

                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""CompanyId"":{2},
                        ""JobPosition"":{{""Id"":{3}}},
                        ""ProcessType"":{4},
                        ""Start"":""{5}"",
                        ""Work"":""{6}"",
                        ""End"":""{7}"",
                        ""Disabledby"": {10},
                        ""DisabledOn"": {11},
                        ""Active"":{8},
                        ""Deletable"":{9}
                    }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Description.Replace("\"", "\\\""),
                    this.CompanyId,
                    this.JobPosition.Id,
                    this.ProcessType,
                    Tools.JsonCompliant(this.Start),
                    Tools.JsonCompliant(this.Work),
                    Tools.JsonCompliant(this.End),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false",
                    disabledBy,
                    disabledOn);
            }
        }

        /// <summary>Gets a hyper link to process profile page</summary>
        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""ProcesosView.aspx?id={0}"" title=""{1}"">{1}</a>", this.Id, this.Description);
            }
        }
        
        /*public override string Differences(BaseItem item1)
        {
            Process c1 = item1 as Process;
            if (c1.Description != this.Description)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "Description{0}", this.Description);
            }

            return string.Empty;
        }*/

        /// <summary>Gets a JSON list of compnay's process</summary>
        /// <param name="companyId">Identifier of company</param>
        /// <returns>JSON list of compnay's process</returns>
        public static string GetByCompanyJsonList(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var process in ByCompany(companyId))
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

        /// <summary>Deactive process</summary>
        /// <param name="processId">Process identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Action result</returns>
        public static ActionResult Delete(int processId, int companyId, int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Process_Desactive
             * @Id int out,
             * @CompanyId int,
             * @UserId int */
            using (var cmd = new SqlCommand("Process_Desactive"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.Input("@Id", processId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess();

                    if (res.Success)
                    {
                        Tools.DeleteAttachs(companyId, "Processes", processId);
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

            return res;
        }

        /// <summary>Creates a JSON structure with the processes of a company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>JSON structure of a departments list</returns>
        public static string ByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var process in ByCompany(companyId))
            {
                if (!process.CanBeDeleted)
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
            }

            res.Append("]");
            return res.ToString();
        }

        /// <summary>Gets a descriptive string with the differences between two process</summary>
        /// <param name="other">Second process to compare</param>
        /// <returns>A descriptive string</returns>
        public string Differences(Process other)
        {
            if (this == null || other == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;
            if (this.Description != other.Description)
            {
                res.Append("Description:").Append(other.Description);
                first = false;
            }

            if (this.JobPosition.Id != other.JobPosition.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("JobPosition:").Append(other.JobPosition.Id);
                first = false;
            }

            if (this.ProcessType != other.ProcessType)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("ProcessType:").Append(other.ProcessType);
            }

            if (this.Start != other.Start)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Start:").Append(other.Start);
                first = false;
            }

            if (this.Work != other.Work)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Work:").Append(other.Work);
                first = false;
            }

            if (this.End != other.End)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("End:").Append(other.End);
                first = false;
            }

            return res.ToString();
        }

        /// <summary>Obtain all process of a company</summary>
        /// <param name="companyId">Compnay identifier</param>
        /// <returns>A list of process</returns>
        public static ReadOnlyCollection<Process> ByCompany(int companyId)
        {
            var res = new List<Process>();
            using (var cmd = new SqlCommand("Get_Procesos"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = companyId;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            long lastProcessId = 0;
                            while (rdr.Read())
                            {
                                long actualProcessId = rdr.GetInt64(0);
                                if (actualProcessId != lastProcessId)
                                {
                                    lastProcessId = actualProcessId;
                                    var newProcess = new Process
                                    {
                                        Id = actualProcessId,
                                        CompanyId = companyId,
                                        ProcessType = rdr.GetInt32(2),
                                        Start = rdr.GetString(3),
                                        Work = rdr.GetString(4),
                                        End = rdr.GetString(5),
                                        Description = rdr.GetString(6),
                                        JobPosition = JobPosition.Empty,
                                        Active = true,
                                        CanBeDeleted = rdr.GetBoolean(9)
                                    };

                                    if (!rdr.IsDBNull(7))
                                    {
                                        newProcess.JobPosition = new JobPosition
                                        {
                                            Id = rdr.GetInt32(7),
                                            Description = rdr.GetString(8),
                                            CompanyId = companyId
                                        };
                                    }

                                    if (!rdr.IsDBNull(10))
                                    {
                                        newProcess.DisabledBy = new ApplicationUser
                                        {
                                            Id = rdr.GetInt32(10),
                                            UserName = rdr.GetString(11)
                                        };
                                    }

                                    if (!rdr.IsDBNull(12))
                                    {
                                        newProcess.DisabledOn = rdr.GetDateTime(12);
                                    }

                                    res.Add(newProcess);
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

                return new ReadOnlyCollection<Process>(res);
            }
        }

        /// <summary>Gets a string to identificate process</summary>
        /// <returns>String to identificate process</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}] {1}", this.Id, this.Description);
        }

        /// <summary>Insert the process into data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Insert(int userId)
        {
            var res = ActionResult.NoAction;
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
            using (var cmd = new SqlCommand("Process_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.JobPosition.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Type", this.ProcessType));
                        cmd.Parameters.Add(DataParameter.Input("@Start", this.Start, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Work", this.Work, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@End", this.End, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(cmd.Parameters["@Id"].Value.ToString());
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString(), CultureInfo.InvariantCulture);
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
            }

            return res;
        }

        /// <summary>Update the process in data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
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
            using (var cmd = new SqlCommand("Process_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 150));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@JobPositionId", this.JobPosition.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Type", this.ProcessType));
                        cmd.Parameters.Add(DataParameter.Input("@Start", this.Start, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@Work", this.Work, Constant.MaximumTextAreaLength));
                        cmd.Parameters.Add(DataParameter.Input("@End", this.End, Constant.MaximumTextAreaLength));
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
            }

            return res;
        }
    }
}