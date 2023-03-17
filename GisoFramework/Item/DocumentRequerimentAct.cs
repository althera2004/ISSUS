// --------------------------------
// <copyright file="DocumentRequerimentAct.cs" company="OpenFramework">
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

    /// <summary>Implements DocumentRequerimentAct class</summary>
    public class DocumentRequerimentAct : BaseItem
    {
        public static DocumentRequerimentAct Empty
        {
            get
            {
                return new DocumentRequerimentAct
                {
                    Id = -1,
                    DocumentRequerimentDefinitionId = -1,
                    DocumentId = -1,
                    Responsible = Employee.Empty
                };
            }
        }

        public long DocumentRequerimentDefinitionId { get; set; }

        public long DocumentId { get; set; }

        public DateTime Date { get; set; }

        public string Observations { get; set; }

        public Employee Responsible { get; set; }

        public decimal? Cost { get; set; }

        public DateTime Expiration { get; set; }

        public override string Link
        {
            get { return string.Empty; }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""Description"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Id", this.Id)).Append(",");
                res.Append(Tools.JsonPair("DocumentId", this.DocumentId)).Append(",");
                res.Append(Tools.JsonPair("DocumentRequerimentDefinitionId", this.DocumentRequerimentDefinitionId)).Append(",");
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Description", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Observations", this.Observations)).Append(",");
                res.Append(Tools.JsonPair("Expiration", this.Expiration)).Append(",");
                res.Append(Tools.JsonPair("Date", this.Date)).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append(",");
                res.Append(Tools.JsonPair("Active", this.Active)).Append(",");

                if (this.Responsible == null)
                {
                    res.Append("\"Responsible\":null}");
                }
                else
                {
                    res.Append("\"Responsible\":").Append(this.Responsible.JsonKeyValue).Append("}");
                }

                return res.ToString();
            }
        }

        public static string JsonList(long equipmentId, int companyId)
        {
            var res = new StringBuilder("[");
            var maintenance = GetByCompany(equipmentId, companyId);
            bool first = true;
            foreach (var item in maintenance)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(item.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<DocumentRequerimentAct> GetByCompany(long documentId, int companyId)
        {
            /* CREATE PROCEDURE DocumentRequerimentAct_ByDocumentId
             *   @DocumentId bigint */
            var res = new List<DocumentRequerimentAct>();
            using (var cmd = new SqlCommand("DocumentRequerimentAct_ByDocumentId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", documentId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newDocumentRequerimentAct = new DocumentRequerimentAct
                                {
                                    Id = rdr.GetInt64(ColumnsDocumentRequerimentActGet.Id),
                                    CompanyId = companyId,
                                    DocumentId = rdr.GetInt64(ColumnsDocumentRequerimentActGet.DocumentId),
                                    Description = rdr.GetString(ColumnsDocumentRequerimentActGet.Requeriment),
                                    Observations = rdr.GetString(ColumnsDocumentRequerimentActGet.Observations),
                                    Date = rdr.GetDateTime(ColumnsDocumentRequerimentActGet.Date),
                                    DocumentRequerimentDefinitionId = rdr.GetInt64(ColumnsDocumentRequerimentActGet.DocumentRequerimentDefinitionId),
                                    Expiration = rdr.GetDateTime(ColumnsDocumentRequerimentActGet.Vto),
                                    Active = true,
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsDocumentRequerimentActGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsDocumentRequerimentActGet.Name),
                                        LastName = rdr.GetString(ColumnsDocumentRequerimentActGet.LastName)
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = -1,
                                        UserName = string.Empty
                                    },
                                    ModifiedOn = DateTime.Now
                                };

                                if (!rdr.IsDBNull(ColumnsDocumentRequerimentActGet.Cost))
                                {
                                    newDocumentRequerimentAct.Cost = rdr.GetDecimal(ColumnsDocumentRequerimentActGet.Cost);
                                }

                                res.Add(newDocumentRequerimentAct);
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

            return new ReadOnlyCollection<DocumentRequerimentAct>(res);
        }

        public static string Differences(DocumentRequerimentAct item1, DocumentRequerimentAct item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            if (item1.Description != item2.Description)
            {
                res.Append("Operation:").Append(item2.Description).Append("; ");
            }

            if (item1.Observations != item2.Observations)
            {
                res.Append("Observations:").Append(item2.Observations).Append("; ");
            }

            if (item1.Date != item2.Date)
            {
                res.Append("Date:").Append(item2.Date).Append("; ");
            }

            if (item1.Expiration != item2.Expiration)
            {
                res.Append("Vto:").Append(item2.Expiration).Append("; ");
            }

            if (item1.Cost != item2.Cost)
            {
                res.Append("Cost:").Append(item2.Cost).Append("; ");
            }

            if (item1.Responsible.Id != item2.Responsible.Id)
            {
                res.Append("Responsible:").Append(item2.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }

        public static ActionResult Delete(int documentRequerimentActId, int userId, int companyId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"EquipmentMaintenanceAct::Delete Id:{0} User:{1} Company:{2}", documentRequerimentActId, userId, companyId);
            /* CREATE PROCEDURE DocumentRequerimentAct_Inactive
             *   @DocumentRequerimentActId bigint,
             *   @ApplicactionUserId bigint */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("DocumentRequerimentAct_Inactive"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@ApplicactionUserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentRequerimentActId", documentRequerimentActId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
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

        public ActionResult Save(int userId)
        {
            if(this.Id > 0)
            {
                return this.Update(userId);
            }
            else
            {
                return this.Insert(userId);
            }
        }

        private ActionResult Insert(int userId)
        {
            /* CREATE PROCEDURE DocumentRequerimentAct_Insert
             *   @DocumentRequerimentActId bigint output,
             *   @DocumentRequerimentDefinitionId bigint,
             *   @DocumentId bigint,
             *   @CompanyId bigint,
             *   @Date datetime,
             *   @Observations text,
             *   @Cost numeric(18,3),
             *   @Vto date,
             *   @ResponsibleId int,
             *   @ApplicactionUserId bigint */
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("DocumentRequerimentAct_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@DocumentRequerimentActId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.DocumentId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentRequerimentDefinitionId", this.DocumentRequerimentDefinitionId));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@DocumentRequerimentActId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.Success = true;
                        result.MessageError = this.Id.ToString(CultureInfo.InvariantCulture);
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentAct::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "DocumentRequerimentAct", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        private ActionResult Update(int userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"DocumentRequerimentAct::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId);
            /* CREATE PROCEDURE DocumentRequerimentAct_Update
             *   @DocumentRequerimentActId bigint,
             *   @Date datetime,
             *   @Observations text,
             *   @Cost numeric(18,3),
             *   @Vto date,
             *   @ResponsibleId int,
             *   @ApplicactionUserId bigint */
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("DocumentRequerimentAct_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DocumentRequerimentActId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Observations", this.Observations));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@Vto", this.Expiration));
                        cmd.Parameters.Add(DataParameter.Input("@Responsible", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
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
    }
}