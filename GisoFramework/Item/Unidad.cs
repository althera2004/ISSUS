

namespace GisoFramework.Item
{
    using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
    using System.Collections.ObjectModel;
    using GisoFramework.Item.Binding;
    using GisoFramework.Activity;
    using System.Data.SqlClient;
    using GisoFramework.DataAccess;
    using System.Configuration;
    using System.Data;
    using System.Web;
    public class Unidad : BaseItem
    {
        public static Unidad Empty
        {
            get
            {
                return new Unidad()
                {
                    Id = -1,
                    Description = string.Empty,
                    CompanyId = -1,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        public override string Json
        {
            get {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""Description"":""{2}"",""CreatedBy"":{3},""CreatedOn"":""{4:dd/MM/yyyy}"",""ModifiedBy"":{5},""ModifiedOn"":""{6:dd/MM/yyyy}"",""Active"":{7}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Description),
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.Active ? "true" : "false");
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""CompanyId"":{1},""Description"":""{2}"", ""Active"":{3},""Deletable"":{4}}}",
                    this.Id,
                    this.CompanyId,
                    Tools.JsonCompliant(this.Description),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false");
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, @"<a href=""UnidadView.aspx?id={0}"">{1}</a>", this.Id, this.Description);
            }
        }

        public static string GetByCompanyJsonList(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach(Unidad unidad in GetAll(companyId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(unidad.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<Unidad> GetAll(Company company)
        {
            if(company == null)
            {
                return new ReadOnlyCollection<Unidad>(new List<Unidad>());
            }

            return GetAll(company.Id);
        }

        public static ReadOnlyCollection<Unidad> GetActive(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            List<Unidad> res = new List<Unidad>();
            using (SqlCommand cmd = new SqlCommand("Unidad_GetActive"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Unidad objetivo = new Unidad()
                        {
                            Id = rdr.GetInt32(ColumnsUnidadGet.Id),
                            Description = rdr.GetString(ColumnsUnidadGet.Description),
                            CompanyId = companyId,
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsUnidadGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsUnidadGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsUnidadGet.CreatedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsUnidadGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsUnidadGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsUnidadGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsUnidadGet.Active),
                            CanBeDeleted = rdr.GetInt32(ColumnsUnidadGet.Deletable) == 1
                        };

                        res.Add(objetivo);
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

            return new ReadOnlyCollection<Unidad>(res);
        }

        public static ReadOnlyCollection<Unidad> GetAll(int companyId)
        {
            /* CREATE PROCEDURE Provider_GetByCompany
             *   @CompanyId int */
            List<Unidad> res = new List<Unidad>();
            using (SqlCommand cmd = new SqlCommand("Unidad_GetAll"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Unidad objetivo = new Unidad()
                        {
                            Id = rdr.GetInt32(ColumnsUnidadGet.Id),
                            Description = rdr.GetString(ColumnsUnidadGet.Description),
                            CompanyId = companyId,
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsUnidadGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsUnidadGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsUnidadGet.CreatedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsUnidadGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsUnidadGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsUnidadGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsUnidadGet.Active),
                            CanBeDeleted = rdr.GetInt32(ColumnsUnidadGet.Deletable) == 1
                        };

                        res.Add(objetivo);
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

            return new ReadOnlyCollection<Unidad>(res);
        }

        public static Unidad GetById(int objetivoId, int companyId)
        {
            /* CREATE PROCEDURE Unidad_GetByCompany
             *   @UnidadId int */
            Unidad res = Unidad.Empty;
            using (SqlCommand cmd = new SqlCommand("Unidad_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@UnidadId", objetivoId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res.Id = rdr.GetInt32(ColumnsUnidadGet.Id);
                        res.Description = rdr.GetString(ColumnsUnidadGet.Description);
                        res.CompanyId = companyId;
                        res.CreatedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsUnidadGet.CreatedBy),
                            UserName = rdr.GetString(ColumnsUnidadGet.CreatedByName)
                        };
                        res.CreatedOn = rdr.GetDateTime(ColumnsUnidadGet.CreatedOn);
                        res.ModifiedBy = new ApplicationUser()
                        {
                            Id = rdr.GetInt32(ColumnsUnidadGet.ModifiedBy),
                            UserName = rdr.GetString(ColumnsUnidadGet.ModifiedByName)
                        };
                        res.ModifiedOn = rdr.GetDateTime(ColumnsUnidadGet.ModifiedOn);
                        res.Active = rdr.GetBoolean(ColumnsUnidadGet.Active);
                        res.CanBeDeleted = rdr.GetInt32(ColumnsUnidadGet.Deletable) == 1;
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

        public static string ListJson(ReadOnlyCollection<Unidad> list)
        {
            if (list == null)
            {
                return "[]";
            }

            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Unidad unidad in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(unidad.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static string ListJsonKeyValue(ReadOnlyCollection<Unidad> list)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Unidad unidad in list)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(unidad.JsonKeyValue);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ActionResult Activate(int unidadId, int compnayId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Unidad::Activate({0}, {1})",
                unidadId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Activate]
             *   @UnidadId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Unidad_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@UnidadId", unidadId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", compnayId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(unidadId);
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
            return res;
        }

        public static ActionResult Inactivate(int unidadId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Unidad::Inactivate({0}, {1})",
                unidadId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Objetivo_Inactivate]
             *   @UnidadId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Unidad_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@UnidadId", unidadId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(unidadId);
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
            return res;
        }

        public ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Unidad::Unidad_Insert({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Unidad_Insert]
             *   @UnidadId int,
             *   @Description nvarchar(50),
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Unidad_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@UnidadId"));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@UnidadId"].Value.ToString());
                        this.CreatedBy = ApplicationUser.Empty;
                        this.ModifiedBy = ApplicationUser.Empty;
                        this.Active = true;
                        res.SetSuccess(this.Id);
                        res.ReturnValue = this.Json;
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
            return res;
        }

        public ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"Unidad::Unidad_Update({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Unidad_Update]
             *   @UnidadId int,
             *   @CompanyId int,
             *   @Description nvarchar(50),
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("Unidad_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@UnidadId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Description", this.Description, 50));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@UnidadId"].Value.ToString());
                        this.CreatedBy = ApplicationUser.Empty;
                        this.ModifiedBy = ApplicationUser.Empty;
                        this.Active = true;
                        res.SetSuccess(this.Id);
                        res.ReturnValue = this.Json;
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
            return res;
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

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Unidad);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Unidad);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "UnidadDelete({0},'{1}');", this.Id, this.Description);
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
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='UnidadView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='UnidadView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string pattenr = @"<tr><td>{0}</td><td style=""width:90px;"">{1}&nbsp;{2}</td></tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattenr,
                this.Link,
                iconEdit,
                iconDelete);
        }
    }
}
