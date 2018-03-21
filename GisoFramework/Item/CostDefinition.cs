// --------------------------------
// <copyright file="CostDefinition.cs" company="Sbrinna">
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

    public class CostDefinition : BaseItem
    {
        public static CostDefinition Empty
        {
            get
            {
                return new CostDefinition
                {
                    Id = -1,
                    Description = string.Empty,
                    Amount = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    CompanyId = Company.Empty.Id
                };
            }
        }

        public decimal Amount { get; set; }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = @"{{
                        ""Id"":{0},
                        ""Description"":""{1}"",
                        ""Amount"":{2},
                        ""ModifiedBy"":{3},
                        ""ModifiedOn"":{4:dd/MM/yyyy},
                        ""Active"":{5},
                        ""Deletable"":{6}
                    }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Amount,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"{{""Id"":{0},""Description"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                     CultureInfo.InvariantCulture,
                     @"<a href=""CostDefinitionView.aspx?id={0}"" title=""{2} {1}"">{1}</a>",
                     this.Id,
                     this.Description,
                     ((Dictionary<string, string>)HttpContext.Current.Session["Dictionary"])["Common_Edit"]);
            }
        }

        public static CostDefinition GetById(long costDefinitionId, int companyId)
        {
            var costs = GetByCompany(companyId);
            if (costs.Any(cd => cd.Id == costDefinitionId))
            {
                return costs.First(cd => cd.Id == costDefinitionId);
            }

            return CostDefinition.Empty;
        }

        public static ReadOnlyCollection<CostDefinition> GetActive(int companyId)
        {
            return new ReadOnlyCollection<CostDefinition>(GetByCompany(companyId).Where(cd => cd.Active).ToList());
        }

        public static string GetByCompanyJson(int companyId)
        {
            var res = new StringBuilder("[");
            var costs = GetByCompany(companyId);
            bool first = true;
            foreach (var cost in costs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(cost.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<CostDefinition> GetByCompany(int companyId)
        {
            /* CREATE PROCEDURE Customer_GetByCompany
             *   @CompanyId int */
            var res = new List<CostDefinition>();
            using (var cmd = new SqlCommand("CostDefinition_GetAll"))
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
                                var newCost = new CostDefinition
                                {
                                    Id = rdr.GetInt64(ColumnsCostDefinitionGet.Id),
                                    Description = rdr.GetString(ColumnsCostDefinitionGet.Description),
                                    Amount = rdr.GetDecimal(ColumnsCostDefinitionGet.Amount),
                                    CreatedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsCostDefinitionGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsCostDefinitionGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsCostDefinitionGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser
                                    {
                                        Id = rdr.GetInt32(ColumnsCostDefinitionGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsCostDefinitionGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsCostDefinitionGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsCostDefinitionGet.Active),
                                    CanBeDeleted = true,
                                    CompanyId = rdr.GetInt32(ColumnsCostDefinitionGet.CompanyId)
                                };

                                newCost.ModifiedBy.Employee = Employee.GetByUserId(newCost.ModifiedBy.Id);
                                res.Add(newCost);
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

            return new ReadOnlyCollection<CostDefinition>(res);
        }

        public static ActionResult Inactive(long costDefinitionId, int companyId, int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [issususer].[CostDefinition_Deactivate]
             *   @CostDefinitionId bigint,
             *   @CompanyId int,
             *   @ApplicationUserId bigint */
            string source = string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"CostDefinition::Inactive({0},{1},{2})",
                costDefinitionId,
                companyId,
                applicationUserId);
            using (var cmd = new SqlCommand("CostDefinition_Deactivate"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Parameters.Add(DataParameter.Input("@CostDefinitionId", costDefinitionId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                        res.SetFail(ex);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                        res.SetFail(ex);
                    }
                    catch (NotImplementedException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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

        /// <summary>Gets a row of list departament</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">List of user's grants</param>
        /// <returns>Code HTML for department row</returns>        
        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            bool grantCostDefinition = UserGrant.HasWriteGrant(grants, ApplicationGrant.CostDefinition);

            string iconRename = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""CostDefinitionUpdate({0});""><i class=""icon-edit bigger-120""></i></span>", this.Id, Tools.SetTooltip(this.Description), Tools.JsonCompliant(dictionary["Common_Edit"]));

            string iconDelete = string.Empty;

            if (grantCostDefinition)
            {
                string deleteAction = string.Format(CultureInfo.GetCultureInfo("en-us"), "CostDefinitionDelete('{0}', {1});", Tools.JsonCompliant(this.Description), this.Id);
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{1} {2}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteAction, Tools.JsonCompliant(dictionary["Common_Delete"]), Tools.SetTooltip(this.Description));
            }

            string import = string.Format(CultureInfo.InvariantCulture, "{0:#,##0.00}", this.Amount).Replace(".", ",");
            return string.Format(CultureInfo.GetCultureInfo("en-us"), @"<tr id=""{0}""><td>{1}</td><td class=""hidden-480"" align=""right"" style=""padding-right:8px;width:200px;"">{2}</td><td style=""width:89px"">{3} {4}</td></tr>", this.Id, this.Link, import, iconRename, iconDelete);
        }

        public ActionResult Insert(int applicationUserId)
        {
            /* CREATE PROCEDURE CostDefinition_Insert
             *   @CostDefinitionId bigint output,
             *   @CompanyId int,
             *   @Description nvarchar(100),
             *   @Amount decimal(18,3),
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CostDefinition_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    try
                    {
                        cmd.Parameters.Add(DataParameter.OutputLong("@CostDefinitionId"));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = (long)cmd.Parameters["@CostDefinitionId"].Value;
                        res.SetSuccess(this.Id);
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "CostDefinition::Insert({0})", this.Id));
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

        public ActionResult Update(int applicationUserId)
        {
            /* CREATE PROCEDURE CostDefinition_Update
             *   @CostDefinitionId bigint,
             *   @CompanyId int,
             *   @Description nvarchar(100),
             *   @Amount decimal(18,3),
             *   @ApplicationUserId bigint */
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("CostDefinition_Update"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CostDefinitionId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 100));
                        cmd.Parameters.Add(DataParameter.Input("@Amount", this.Amount));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "CostDefinition::Update({0})", this.Id));
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