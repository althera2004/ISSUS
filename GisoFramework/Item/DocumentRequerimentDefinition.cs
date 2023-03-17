// --------------------------------
// <copyright file="EquipmentMaintenanceDefinition.cs" company="OpenFramework">
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

    /// <summary>Implements DocumentRequerimentDefinition class</summary>
    public class DocumentRequerimentDefinition : BaseItem
    {
        public long DocumentId { get; set; }

        public int Periodicity { get; set; }

        public string Actuacio { get; set; }

        public decimal? Cost { get; set; }

        public Employee Responsible { get; set; }

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
                res.Append(Tools.JsonPair("CompanyId", this.CompanyId)).Append(",");
                res.Append(Tools.JsonPair("Requeriment", this.Description)).Append(",");
                res.Append(Tools.JsonPair("Periodicity", this.Periodicity)).Append(",");
                res.Append(Tools.JsonPair("Actuacio", this.Actuacio)).Append(",");
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

        public static string JsonList(int documentId, int companyId)
        {
            var res = new StringBuilder("[");
            var definitions = ByDocumentId(documentId, companyId);
            bool first = true;
            foreach (var item in definitions)
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

        /*public string Row(Dictionary<string, string> dictionary, bool grantToWrite)
        {
            string maintenanceType = this.MaintenanceType == 0 ? dictionary["Common_Internal"] : dictionary["Common_External"];
            string iconEdit = string.Empty;
            string iconDelete = string.Empty;
            string iconAct = string.Empty;

            if (grantToWrite)
            {
                iconEdit = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-info"" title=""{0}"" onclick=""EquipmentMaintenanceEdit({1});""><i class=""icon-edit bigger-120""></i></span>",
                    dictionary["Common_Edit"],
                    this.Id);
                iconDelete = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-danger"" title=""{0}"" onclick=""EquipmentMaintenanceDelete({1});""><i class=""icon-trash bigger-120""></i></span>",
                    dictionary["Common_Delete"],
                    this.Id);
                iconAct = string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<span class=""btn btn-xs btn-success"" title=""{0}"" onclick=""EquipmentMaintenanceEdit({1});""><i class=""icon-star bigger-120""></i></span>",
                    dictionary["Item_EquipmentMaintenance_Button_Register"],
                    this.Id);
            }

            StringBuilder res = new StringBuilder("<tr>");
            res.Append("<td>").Append(this.Operation).Append("</td><td>");
            res.Append(maintenanceType).Append("</td><td align=\"right\">");
            res.Append(this.Periodicity).Append("</td><td>");
            res.Append(this.Accessories).Append("</td><td align=\"right\">");
            res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#0.00}", this.Cost)).Append("</td><td>");
            res.Append(iconEdit).Append("&nbsp;").Append(iconDelete).Append("&nbsp;").Append(iconAct).Append("</td></tr>");
            return res.ToString();
        }*/

        public static ReadOnlyCollection<DocumentRequerimentDefinition> ByDocumentId(int equipmentId, int companyId)
        {
            /*CREATE PROCEDURE DocumentRequeriment_GetByDocumentId
             *   @DocumentId bigint,
             *   @CompanyId int */
            var res = new List<DocumentRequerimentDefinition>();
            using (var cmd = new SqlCommand("DocumentRequeriment_GetByDocumentId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", equipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newDocumentRequerimentDefinition = new DocumentRequerimentDefinition
                                {
                                    Id = rdr.GetInt64(ColumnsDocumentRequerimentDefinitionGet.Id),
                                    CompanyId = rdr.GetInt32(ColumnsDocumentRequerimentDefinitionGet.CompanyId),
                                    DocumentId = rdr.GetInt64(ColumnsDocumentRequerimentDefinitionGet.DocumentId),
                                    Description = rdr.GetString(ColumnsDocumentRequerimentDefinitionGet.Requeriment),
                                    Actuacio = rdr.GetString(ColumnsDocumentRequerimentDefinitionGet.Actuacio),
                                    Periodicity = rdr.GetInt32(ColumnsDocumentRequerimentDefinitionGet.Periodicity),
                                    Active = rdr.GetBoolean(ColumnsDocumentRequerimentDefinitionGet.Active),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsDocumentRequerimentDefinitionGet.ResponsableId),
                                        Name = string.Empty,
                                        LastName = string.Empty
                                    },
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsDocumentRequerimentDefinitionGet.ModifiedByUserId),
                                        UserName = string.Empty
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsDocumentRequerimentDefinitionGet.ModifiedOn)
                                };

                                if (!rdr.IsDBNull(ColumnsDocumentRequerimentDefinitionGet.Cost))
                                {
                                    newDocumentRequerimentDefinition.Cost = rdr.GetDecimal(ColumnsDocumentRequerimentDefinitionGet.Cost);
                                }

                                res.Add(newDocumentRequerimentDefinition);
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

            return new ReadOnlyCollection<DocumentRequerimentDefinition>(res);
        }

        public static ActionResult Delete(int documentRequeriemtnDefinitionId, int userId, int companyId)
        {
            var result = new ActionResult() { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("DocumentRequerimentDefinition_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentRequerimentDefinitionId", documentRequeriemtnDefinitionId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"DocumentRequerimentDefinition::Delete Id:{0} User:{1} Company:{2}", documentRequeriemtnDefinitionId, userId, companyId));
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"DocumentRequerimentDefinition::Delete Id:{0} User:{1} Company:{2}", documentRequeriemtnDefinitionId, userId, companyId));
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"DocumentRequerimentDefinition::Delete Id:{0} User:{1} Company:{2}", documentRequeriemtnDefinitionId, userId, companyId));
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"DocumentRequerimentDefinition::Delete Id:{0} User:{1} Company:{2}", documentRequeriemtnDefinitionId, userId, companyId));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), @"DocumentRequerimentDefinition::Delete Id:{0} User:{1} Company:{2}", documentRequeriemtnDefinitionId, userId, companyId));
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

        /*
        public override string Differences(BaseItem item)
        {
            if(item==null)
            {
                return string.Empty;
            }

            EquipmentMaintenanceDefinition item1 = item as EquipmentMaintenanceDefinition;
            StringBuilder res = new StringBuilder();
            if (item1.Description != this.Description)
            {
                res.Append("Operation:").Append(this.Description).Append("; ");
            }
            if (item1.MaintenanceType != this.MaintenanceType)
            {
                res.Append("MaintenanceType:").Append(this.MaintenanceType).Append("; ");
            }
            if (item1.Periodicity != this.Periodicity)
            {
                res.Append("Periodicity:").Append(this.Periodicity).Append("; ");
            }
            if (item1.Accessories != this.Accessories)
            {
                res.Append("Accessories:").Append(this.Accessories).Append("; ");
            }
            if (item1.Cost != this.Cost)
            {
                res.Append("Cost:").Append(this.Cost).Append("; ");
            }

            if (item1.Provider != null || this.Provider != null)
            {
                if (item1.Provider == null)
                {
                    res.Append("Provider:").Append(this.Provider.Id).Append("; ");
                }
                else if (this.Provider == null)
                {
                    res.Append("Provider:null; ");
                }
                else if (item1.Provider.Id != this.Provider.Id)
                {
                    res.Append("Provider:").Append(this.Provider.Id).Append("; ");
                }
            }

            if (item1.Responsible.Id != this.Responsible.Id)
            {
                res.Append("Responsible:").Append(this.Responsible.Id).Append("; ");
            }

            return res.ToString();
        }
*/

        public ActionResult Save(int userId)
        {
            if(this.Id > 0)
            {
                return this.Update(string.Empty, userId);
            }
            else
            {
                return this.Insert(userId);
            }
        }

        private ActionResult Insert(int userId)
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Id:{0} - Name:{1}", this.Id, this.Description);
            /* CREATE PROCEDURE EquipmentMaintenance_Insert
             *   @EquipmentMaintenanceId bigint output,
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @Operation nvarchar(50),
             *   @EquipmentMaintenanceType int,
             *   @Periodicity int,
             *   @Accessories nvarchar(50),
             *   @Cost numeric(18,3),
             *   @ProviderId bigint,
             *   @ResponsableId int,
             *   @UserId int	*/
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("DocumentRequerimentDefinition_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@DocumentRequerimentDefinitionId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.DocumentId));
                        cmd.Parameters.Add(DataParameter.Input("@Requeriment", this.Description, 200));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Actuacio", this.Actuacio ?? string.Empty));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));

                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@DocumentRequerimentDefinitionId"].Value, CultureInfo.InvariantCulture);
                        result.Success = true;
                        result.ReturnValue = this.Id;
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentDefinitionId::Insert", source);
                    }
                    catch (FormatException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentDefinition::Insert", source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentDefinition::Insert", source);
                    }
                    catch (ArgumentException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentDefinition::Insert", source);
                    }
                    catch (NullReferenceException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "DocumentRequerimentDefinition::Insert", source);
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

        private ActionResult Update(string differences, int userId)
        {
            string source = string.Format(CultureInfo.InvariantCulture, @"DocumentRequrimentDefinition::Update Id:{0} User:{1} Company:{2}", this.Id, userId, this.CompanyId);
            /* CREATE PROCEDURE DocumentRequerimentDefinition_Update
             *   @DocumentRequerimentDefinition bigint,
             *   @CompanyId int,
             *   @DocumentId bigint,
             *   @Requeriment nvarchar(200),
             *   @Periodicity int,
             *   @Actuacio nvarchar(50),
             *   @Cost numeric(18,3),
             *   @ResponsableId int,
             *   @UserId int */
            var result = new ActionResult { Success = false, MessageError = "No action" };
            using (var cmd = new SqlCommand("DocumentRequerimentDefinition_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@DocumentRequerimentDefinition", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@DocumentId", this.DocumentId));
                        cmd.Parameters.Add(DataParameter.Input("@Requeriment", this.Description, 200));
                        cmd.Parameters.Add(DataParameter.Input("@Periodicity", this.Periodicity));
                        cmd.Parameters.Add(DataParameter.Input("@Actuacio", this.Actuacio));
                        cmd.Parameters.Add(DataParameter.Input("@Cost", this.Cost));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsableId", this.Responsible.Id));
                        //cmd.Parameters.Add(DataParameter.Input("@Differences", differences));
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