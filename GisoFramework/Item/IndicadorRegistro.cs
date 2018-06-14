// --------------------------------
// <copyright file="IndicadorRegistro.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using GisoFramework.Activity;
using GisoFramework.DataAccess;
using GisoFramework.Item.Binding;
using System.Text;

namespace GisoFramework.Item
{
    public class IndicadorRegistro : BaseItem
    {
        [DifferenciableAttribute]
        public Indicador Indicador { get; set; }

        [DifferenciableAttribute]
        public string MetaComparer { get; set; }

        [DifferenciableAttribute]
        public string AlarmaComparer { get; set; }

        [DifferenciableAttribute]
        public decimal Meta { get; set; }

        [DifferenciableAttribute]
        public decimal? Alarma { get; set; }

        [DifferenciableAttribute]
        public decimal Value { get; set; }

        [DifferenciableAttribute]
        public DateTime Date { get; set; }

        [DifferenciableAttribute]
        public string Comments { get; set; }

        [DifferenciableAttribute]
        public Employee Responsible { get; set; }

        public static IndicadorRegistro Empty
        {
            get
            {
                return new IndicadorRegistro()
                {
                    Id = 0,
                    Description = string.Empty,
                    Indicador = Indicador.Empty,
                    Meta = 0,
                    Alarma = null,
                    Value = 0,
                    Date = DateTime.Now,
                    Comments = string.Empty,
                    Responsible = Employee.Empty,
                    CompanyId = 0,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false,
                    CanBeDeleted = true
                };
            }
        }

        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public override string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},""Name"":""{1}""}}",
                    this.Id,
                    Tools.JsonCompliant(this.Description));
            }
        }

        public override string Json
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0},
                    ""Description"":""{1}"",
                    ""Indicador"":{2},
                    ""MetaComparer"":""{14}"",
                    {3},
                    ""AlarmaComparer"":""{15}"",
                    {4},
                    ""Value"":{5},
                    ""Date"":""{6:dd/MM/yyyy}"",
                    ""Comments"":""{7}"",
                    ""Responsible"":{8},
                    ""CreatedBy"":{9},
                    ""CreatedOn"":""{10:dd/MM/yyyy}"",
                    ""ModifiedBy"":{11},
                    ""ModifiedOn"":""{12:dd/MM/yyyy}"",
                    ""Active"":{13}
                    }}",
                    this.Id,
                    Tools.JsonCompliant(this.Description),
                    this.Indicador.JsonKeyValue,
                    Tools.JsonPair("Meta", this.Meta),
                    Tools.JsonPair("Alarma", this.Alarma),
                    this.Value,
                    this.Date,
                    Tools.JsonCompliant(this.Comments),
                    this.Responsible.JsonKeyValue,
                    this.CreatedBy.JsonKeyValue,
                    this.CreatedOn,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true" : "false",
                    ComparerLabelSign(this.MetaComparer, dictionary),
                    ComparerLabelSign(this.AlarmaComparer, dictionary));
            }
        }

        public static string ByIndicadorJson(int indicadorId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach(IndicadorRegistro registro in ByIndicadorId(indicadorId, companyId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(registro.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IndicadorRegistro> ByIndicadorId(int indicadorId, int companyId)
        {
            /* CREATE PROCEDURE [dbo].[IndicadorRegistro_GetByIndicadorId]
             *   @IndicadorId int,
             *   @CompanyId int */
            var res = new List<IndicadorRegistro>();
            using (var cmd = new SqlCommand("IndicadorRegistro_GetByIndicadorId"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                IndicadorRegistro registro = new IndicadorRegistro()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.Id),
                                    Indicador = new Indicador()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.IndicadorId),
                                        Description = rdr.GetString(ColumnsIndicadorRegistroGet.IndicadorDescripcion)
                                    },
                                    MetaComparer = rdr.GetString(ColumnsIndicadorRegistroGet.MetaComparer),
                                    Meta = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Meta),
                                    AlarmaComparer = rdr.GetString(ColumnsIndicadorRegistroGet.MetaAlarm),
                                    Value = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Value),
                                    Date = rdr.GetDateTime(ColumnsIndicadorRegistroGet.Date),
                                    Comments = rdr.GetString(ColumnsIndicadorRegistroGet.Comments),
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsIndicadorRegistroGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsIndicadorRegistroGet.ResponsibleLastName),
                                        UserId = rdr.GetInt32(ColumnsIndicadorRegistroGet.EmployeeUserId),
                                        User = new ApplicationUser()
                                        {
                                            Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.EmployeeUserId),
                                            UserName = rdr.GetString(ColumnsIndicadorRegistroGet.EmployeeUserName)
                                        }
                                    },
                                    CreatedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.CreatedBy),
                                        UserName = rdr.GetString(ColumnsIndicadorRegistroGet.CreatedByName)
                                    },
                                    CreatedOn = rdr.GetDateTime(ColumnsIndicadorRegistroGet.CreatedOn),
                                    ModifiedBy = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.ModifiedBy),
                                        UserName = rdr.GetString(ColumnsIndicadorRegistroGet.ModifiedByName)
                                    },
                                    ModifiedOn = rdr.GetDateTime(ColumnsIndicadorRegistroGet.ModifiedOn),
                                    Active = rdr.GetBoolean(ColumnsIndicadorRegistroGet.Active)
                                };

                                if (!rdr.IsDBNull(ColumnsIndicadorRegistroGet.Alarm))
                                {
                                    registro.Alarma = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Alarm);
                                }

                                res.Add(registro);
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

            return new ReadOnlyCollection<IndicadorRegistro>(res);
        }

        public static string ComparerLabel(string comparerValue, Dictionary<string, string> dictionary)
        {
            switch (comparerValue.ToUpperInvariant())
            {
                case "EQ": return dictionary["Common_Comparer_eq"];
                case "GT": return dictionary["Common_Comparer_gt"];
                case "EQGT": return dictionary["Common_Comparer_eqgt"];
                case "LT": return dictionary["Common_Comparer_lt"];
                case "EQLT": return dictionary["Common_Comparer_eqlt"];
            }

            return string.Empty;
        }

        public static string ComparerLabelSign(string comparerValue, Dictionary<string, string> dictionary)
        {
            if (string.IsNullOrEmpty(comparerValue))
            {
                return string.Empty;
            }

            switch (comparerValue.ToUpperInvariant())
            {
                case "EQ": return dictionary["Common_ComparerSign_eq"];
                case "GT": return dictionary["Common_ComparerSign_gt"];
                case "EQGT": return dictionary["Common_ComparerSign_eqgt"];
                case "LT": return dictionary["Common_ComparerSign_lt"];
                case "EQLT": return dictionary["Common_ComparerSign_eqlt"];
            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicadorId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public static IndicadorRegistro ById(long indicadorId, int companyId)
        {
            /* CREATE PROCEDURE IndicadorRegistro_GetById
             *   @IndicadorRegistroId int,
             *   @CompanyId int */
            var res = IndicadorRegistro.Empty;
            using (var cmd = new SqlCommand("IndicadorRegistro_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorRegistroId", indicadorId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                res.Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.Id);
                                res.Indicador = new Indicador()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.IndicadorId),
                                    Description = rdr.GetString(ColumnsIndicadorRegistroGet.IndicadorDescripcion)
                                };
                                res.MetaComparer = rdr.GetString(ColumnsIndicadorRegistroGet.MetaComparer);
                                res.Meta = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Meta);
                                res.AlarmaComparer = rdr.GetString(ColumnsIndicadorRegistroGet.MetaAlarm);
                                res.Value = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Value);
                                res.Comments = rdr.GetString(ColumnsIndicadorRegistroGet.Comments);
                                res.Date = rdr.GetDateTime(ColumnsIndicadorRegistroGet.Date);
                                res.Responsible = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.ResponsibleId),
                                    Name = rdr.GetString(ColumnsIndicadorRegistroGet.ResponsibleName),
                                    LastName = rdr.GetString(ColumnsIndicadorRegistroGet.ResponsibleLastName),
                                    UserId = rdr.GetInt32(ColumnsIndicadorRegistroGet.EmployeeUserId),
                                    User = new ApplicationUser()
                                    {
                                        Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.EmployeeUserId),
                                        UserName = rdr.GetString(ColumnsIndicadorRegistroGet.EmployeeUserName)
                                    }
                                };
                                res.CreatedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.CreatedBy),
                                    UserName = rdr.GetString(ColumnsIndicadorRegistroGet.CreatedByName)
                                };
                                res.CreatedOn = rdr.GetDateTime(ColumnsIndicadorRegistroGet.CreatedOn);
                                res.ModifiedBy = new ApplicationUser()
                                {
                                    Id = rdr.GetInt32(ColumnsIndicadorRegistroGet.ModifiedBy),
                                    UserName = rdr.GetString(ColumnsIndicadorRegistroGet.ModifiedByName)
                                };
                                res.ModifiedOn = rdr.GetDateTime(ColumnsIndicadorRegistroGet.ModifiedOn);
                                res.Active = rdr.GetBoolean(ColumnsIndicadorRegistroGet.Active);

                                if (!rdr.IsDBNull(ColumnsIndicadorRegistroGet.Alarm))
                                {
                                    res.Alarma = rdr.GetDecimal(ColumnsIndicadorRegistroGet.Alarm);
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

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Indicador);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Indicador);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "IndicadorDelete({0},'{1}');", this.Id, this.Description);
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
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""recordEdit({0});""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='IndicadorView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            string alarmaText = string.Empty;
            if (this.Alarma.HasValue)
            {
                alarmaText = string.Format(
                    CultureInfo.InvariantCulture,
                    "@{0:#0.00}",
                    this.Alarma.Value);
            }
            string pattenr = @"<tr><td align=""right"" style=""width:90px;"">{1}</td><td align=""center"" style=""width:90px;"">{2:dd/MM/yyyy}</td><td>{3}</td><td align=""right"" style=""width:90px;"">{4}</td><td align=""right"" style=""width:90px;"">{5}</td><td style=""width:150px;"">{6}</td><td style=""width:90px;"">{7}&nbsp;{8}</td></tr>";
            return string.Format(
                CultureInfo.InvariantCulture,
                pattenr,
                this.Indicador.Link,
                this.Value,
                this.Date,
                this.Comments,
                this.Meta,
                alarmaText,
                this.Responsible.FullName,
                iconEdit,
                iconDelete);
        }

        public static ActionResult Activate(int indicadorRegistroId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"indicadorRegistro::Activate({0}, {1})",
                indicadorRegistroId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorRegistro_Activate
             *   @IndicadorRegistroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("IndicadorRegistro_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorRegistroId", indicadorRegistroId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorRegistroId);
                    }
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
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (InvalidCastException ex)
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
            return res;
        }

        public static ActionResult Inactivate(int indicadorRegistroId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"IndicadorRegistro::Inactivate({0}, {1})",
                indicadorRegistroId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorRegistro_Inactivate
             *   @IndicadorRegistroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("IndicadorRegistro_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorRegistroId", indicadorRegistroId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(indicadorRegistroId);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    res.SetFail(ex);
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex);
                }
                catch (InvalidCastException ex)
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
            return res;
        }

        private ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"IndicadorRegistro::IndicadorRegistro_Insert({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorRegistro_Insert
             *   @Id int output,
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @Date datetime,
             *   @MetaComparer nvarchar(10),
             *   @Meta decimal(18,6),
             *   @AlarmComparer nvarchar(10),
             *   @Alarm decimal (18,6),
             *   @Value decimal (18,6),
             *   @ResponsibleId int,
             *   @Comments nvarchar(500),
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("IndicadorRegistro_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                        cmd.Parameters.Add(DataParameter.Input("@IndicadorId", this.Indicador.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@AlarmComparer", this.AlarmaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Alarm", this.Alarma));
                        cmd.Parameters.Add(DataParameter.Input("@Value", this.Value));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Comments", this.Comments, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());
                        res.SetSuccess();
                        IndicadorRegistro newRecord = IndicadorRegistro.ById(this.Id, this.CompanyId);
                        res.ReturnValue = newRecord.Json;
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, source);
                    res.SetFail(ex as Exception);
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

        private ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"IndicadorRegistro::IndicadorRegistro({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorRegistro_Update
             *   @Id int,
             *   @Date datetime,
             *   @Value decimal (18,6),
             *   @ResponsibleId int,
             *   @Comments nvarchar(500),
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("IndicadorRegistro_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@Id", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Date", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Value", this.Value));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Comments", this.Comments, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt64(cmd.Parameters["@Id"].Value.ToString());
                        res.SetSuccess();
                        IndicadorRegistro newRecord = IndicadorRegistro.ById(this.Id, this.CompanyId);
                        res.ReturnValue = newRecord.Json;
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

        public ActionResult Save(int applicationUserId)
        {
            if (this.Id > 0)
            {
                return this.Update(applicationUserId);
            }

            return this.Insert(applicationUserId);
        }
    }
}

