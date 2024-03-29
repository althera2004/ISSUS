﻿// --------------------------------
// <copyright file="Provider.cs" company="OpenFramework">
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
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements Provider class</summary>
    public class Provider : BaseItem
    {
        public static Provider Empty
        {
            get
            {
                return new Provider()
                {
                    Id = 0,
                    Description = string.Empty,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                string description = this.Description;
                if (string.IsNullOrEmpty(description))
                {
                    description = string.Empty;
                }

                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0}, ""Description"":""{1}""}}", this.Id, description.Replace("\"", "\\\""));
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
                        ""Active"": {2},
                        ""Deletable"": {3},
                        ""CompanyId"": {4}
                      }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.Description.Replace("\"", "\\\""),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false",
                    this.CompanyId);
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<a href=""ProvidersView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Description);
            }
        }

        public static Provider ById(long id, int companyId)
        {
            var res = new Provider();
            using (var cmd = new SqlCommand("Provider_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                bool deletable = true;
                                if (rdr.GetInt32(ColumnsProviderGetByCompany.CalibrationAct) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.CalibrationDefinition) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.MaintenanceAct) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.MaintenanceDefinition) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.EquipmentRepair) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.Incident) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.IncidentAction) == 1)
                                {
                                    deletable = false;
                                }

                                res = new Provider()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderGetByCompany.Id),
                                    CompanyId = rdr.GetInt32(ColumnsProviderGetByCompany.CompanyId),
                                    Description = rdr.GetString(ColumnsProviderGetByCompany.Description),
                                    Active = rdr.GetBoolean(ColumnsProviderGetByCompany.Active),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderGetByCompany.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsProviderGetByCompany.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsProviderGetByCompany.ModifiedOn),
                                    CanBeDeleted = deletable
                                };

                                res.ModifiedBy.Employee = Employee.ByUserId(res.ModifiedBy.Id);
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

        public static ReadOnlyCollection<Provider> ByCompany(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            var res = new List<Provider>();
            using (var cmd = new SqlCommand("Provider_GetByCompany"))
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
                                bool deletable = true;
                                if (rdr.GetInt32(ColumnsProviderGetByCompany.CalibrationAct) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.CalibrationDefinition) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.MaintenanceAct) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.MaintenanceDefinition) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.EquipmentRepair) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.Incident) == 1)
                                {
                                    deletable = false;
                                }

                                if (rdr.GetInt32(ColumnsProviderGetByCompany.IncidentAction) == 1)
                                {
                                    deletable = false;
                                }

                                res.Add(new Provider()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderGetByCompany.Id),
                                    CompanyId = rdr.GetInt32(ColumnsProviderGetByCompany.CompanyId),
                                    Description = rdr.GetString(ColumnsProviderGetByCompany.Description),
                                    Active = rdr.GetBoolean(ColumnsProviderGetByCompany.Active),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderGetByCompany.ModifiedByUserId),
                                        UserName = rdr.GetString(ColumnsProviderGetByCompany.ModifiedByUserName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsProviderGetByCompany.ModifiedOn),
                                    CanBeDeleted = deletable
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

            return new ReadOnlyCollection<Provider>(res);
        }

        public static string ByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var provider in ByCompany(companyId))
            {
                if (provider.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(provider.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Provider);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Provider);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "ProviderDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", 
                    deleteFunction, 
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ProvidersView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='ProvidersView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            /*if (grantWrite)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "ProviderDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconEdit = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""document.location='ProvidersView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>", this.Id, Tools.LiteralQuote(Tools.JsonCompliant(this.Description)), Tools.JsonCompliant(dictionary["Common_Edit"]));
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteFunction, Tools.LiteralQuote(Tools.JsonCompliant(this.Description)), Tools.JsonCompliant(dictionary["Common_Delete"]));
            }*/

            string pattenr = @"<tr><td>{0}</td><td style=""width:90px;"">{1}&nbsp;{2}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattenr,
                this.Link,
                iconEdit,
                iconDelete);
        }

        public ActionResult Insert(int userId)
        {
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Provider_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.OutputInt("@ProviderId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@ProviderId"].Value, CultureInfo.GetCultureInfo("en-us"));
                        result.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "Provider::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "Provider::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        public ActionResult Update(int userId)
        {
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Provider_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "Provider::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "Provider::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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

        public ActionResult Delete(int userId)
        {
            var result = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Provider_Delete"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@ProviderId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        result.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        result.SetFail(ex);
                        ExceptionManager.Trace(ex, "Provider::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, "Provider::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
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