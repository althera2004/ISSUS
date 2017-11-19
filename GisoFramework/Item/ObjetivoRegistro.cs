// --------------------------------
// <copyright file="ObjetivoRegistro.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    using System.Text;
    using System.Web;

    /// <summary>Implements ObjetivoRegistro class</summary>
    public class ObjetivoRegistro : BaseItem
    {
        /// <summary>
        /// Gets an empty instance of ObjetivoRegistro
        /// </summary>
        public static ObjetivoRegistro Empty
        {
            get
            {
                return new ObjetivoRegistro()
                {
                    Id = 0,
                    ObjetivoId = 0,
                    Date = DateTime.Now,
                    Value = 0,
                    Comments = string.Empty,
                    Responsible = Employee.EmptySimple,
                    CreatedBy = ApplicationUser.Empty,
                    CreatedOn = DateTime.Now,
                    ModifiedBy = ApplicationUser.Empty,
                    ModifiedOn = DateTime.Now,
                    Active = false
                };
            }
        }

        [DifferenciableAttribute]
        public int ObjetivoId { get; set; }

        [DifferenciableAttribute]
        public DateTime Date { get; set; }

        [DifferenciableAttribute]
        public string MetaComparer { get; set; }

        [DifferenciableAttribute]
        public decimal? Meta { get; set; }

        [DifferenciableAttribute]
        public decimal Value { get; set; }

        [DifferenciableAttribute]
        public string Comments { get; set; }

        [DifferenciableAttribute]
        public Employee Responsible { get; set; }

        public override string JsonKeyValue
        {
            get
            {
                return "{}";
            }
        }

        public override string Link
        {
            get
            {
                return string.Empty;
            }
        }

        public override string Json
        {
            get
            {
                Dictionary<string, string> dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string objetivoRegistroPattern = @"{{
                        ""Id"":{0},
                        ""ObjetivoId"":{1},
                        ""CompanyId"":{2},
                        ""Date"":""{3:dd/MM/yyyy}"",
                        {4},
                        ""MetaComparer"":""{5}"",
                        ""Value"":{6:#0.000},
                        ""Comments"":""{7}"",
                        ""Responsible"":{8},
                        ""ModifiedBy"":{9},
                        ""ModifiedOn"":""{10:dd/MM/yyyy}"",
                        ""Active"":{11}}}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    objetivoRegistroPattern,
                    this.Id,
                    this.CompanyId,
                    this.ObjetivoId,
                    this.Date,
                    Tools.JsonPair("Meta", this.Meta),
                    ComparerLabelSign(this.MetaComparer, dictionary),
                    this.Value,
                    Tools.JsonCompliant(this.Comments),
                    this.Responsible.JsonKeyValue,
                    this.ModifiedBy.JsonKeyValue,
                    this.ModifiedOn,
                    this.Active ? "true" : "false");
            }
        }

        public static string GetByObjetivoJson(int objetivoId, int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (ObjetivoRegistro registro in GetByObjetivo(objetivoId, companyId))
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

        public static ReadOnlyCollection<ObjetivoRegistro> GetByObjetivo(int objetivoId, int companyId)
        {
            List<ObjetivoRegistro> res = new List<ObjetivoRegistro>();
            /* CREATE PROCEDURE ObjetivoRegistro_GetByObjetivo
             *   @CompanyId int,
             *   @ObjetivoId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_GetByObjetivo"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ObjetivoRegistro registro = new ObjetivoRegistro()
                        {
                            Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsObjetivoRegistroGet.CompanyId),
                            Date = rdr.GetDateTime(ColumnsObjetivoRegistroGet.Fecha),
                            Value = rdr.GetDecimal(ColumnsObjetivoRegistroGet.Valor),
                            MetaComparer = rdr.GetString(ColumnsObjetivoRegistroGet.MetaComparer),
                            Comments = rdr.GetString(ColumnsObjetivoRegistroGet.Comentario),
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.ResponsableId),
                                Name = rdr.GetString(ColumnsObjetivoRegistroGet.ResponsableFirstName),
                                LastName = rdr.GetString(ColumnsObjetivoRegistroGet.ResponsableLastName)
                            },
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsObjetivoRegistroGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsObjetivoRegistroGet.CreatedOn),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsObjetivoRegistroGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsObjetivoRegistroGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsObjetivoRegistroGet.Active)
                        };

                        if (!rdr.IsDBNull(ColumnsObjetivoRegistroGet.Meta))
                        {
                            registro.Meta = rdr.GetDecimal(ColumnsObjetivoRegistroGet.Meta);
                        }

                        res.Add(registro);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<ObjetivoRegistro>(res);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns></returns>
        public static ReadOnlyCollection<ObjetivoRegistro> GetList(int companyId)
        {
            List<ObjetivoRegistro> res = new List<ObjetivoRegistro>();
            /* CREATE PROCEDURE ObjetivoRegistro_GetAll
             *   @CompanyId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_GetAll"))
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
                        ObjetivoRegistro registro = new  ObjetivoRegistro()
                        {
                            Id = rdr.GetInt64(ColumnsObjetivoRegistroGet.Id),
                            CompanyId = rdr.GetInt32(ColumnsObjetivoRegistroGet.CompanyId),
                            Date = rdr.GetDateTime(ColumnsObjetivoRegistroGet.Fecha),
                            Meta = rdr.GetDecimal(ColumnsObjetivoRegistroGet.Meta),
                            MetaComparer = rdr.GetString(ColumnsObjetivoRegistroGet.MetaComparer),
                            Value = rdr.GetDecimal(ColumnsObjetivoRegistroGet.Valor),
                            Comments = rdr.GetString(ColumnsObjetivoRegistroGet.Comentario),
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.ResponsableId),
                                Name = rdr.GetString(ColumnsObjetivoRegistroGet.ResponsableFirstName),
                                LastName = rdr.GetString(ColumnsObjetivoRegistroGet.ResponsableLastName)
                            },
                            CreatedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.CreatedBy),
                                UserName = rdr.GetString(ColumnsObjetivoRegistroGet.CreatedByName)
                            },
                            CreatedOn = rdr.GetDateTime(ColumnsObjetivoRegistroGet.CreatedOn),
                            ModifiedBy =  new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsObjetivoRegistroGet.ModifiedBy),
                                UserName = rdr.GetString(ColumnsObjetivoRegistroGet.ModifiedByName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsObjetivoRegistroGet.ModifiedOn),
                            Active = rdr.GetBoolean(ColumnsObjetivoRegistroGet.Active)
                        };
                        

                        if (!rdr.IsDBNull(ColumnsObjetivoRegistroGet.Meta))
                        {
                            registro.Meta = rdr.GetDecimal(ColumnsObjetivoRegistroGet.Meta);
                        }

                        res.Add(registro);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                catch (InvalidCastException ex)
                {
                    ExceptionManager.Trace(ex, string.Format(CultureInfo.GetCultureInfo("en-us"), "Equipment::GetList({0})", companyId));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return new ReadOnlyCollection<ObjetivoRegistro>(res);
        }

        public static ActionResult Inactivate(int objetivoRegistroId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"ObjetivoRegistro::Inactivate({0}, {1})",
                objetivoRegistroId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[ObjetivoRegistro_Inactivate]
             *   @ObjetivoRegsitroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_Inactivate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoRegistroId", objetivoRegistroId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoRegistroId);
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

        public static ActionResult Activate(int objetivoRegistroId, int companyId, int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"ObjetivoRegistro::Activate({0}, {1})",
                objetivoRegistroId,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[ObjetivoRegistro_Activate]
             *   @ObjetivoRegsitroId int,
             *   @CompanyId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_Activate"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoRegistroId", objetivoRegistroId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));

                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess(objetivoRegistroId);
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

        private ActionResult Insert(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"ObjetivoRegistro::Insert({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[ObjetivoRegistro_Insert]
             *   @ObjetivoRegsitroId int output,
             *   @CompanyId int,
             *   @ObjetivoId int,
             *   @Fecha datetime,
             *   @Valor decimal (18,6),
             *   @Comentari nvarchar(500),
             *   @ResponsableId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_Insert"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.OutputInt("@ObjetivoRegistroId"));
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.ObjetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Fecha", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Valor", this.Value));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@Comentari", this.Comments, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@ObjetivoRegistroId"].Value.ToString());
                        res.SetSuccess(this.Id);
                        res.ReturnValue = this.Json;
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

        private ActionResult Update(int applicationUserId)
        {
            string source = string.Format(
                CultureInfo.InvariantCulture,
                @"ObjetivoRegistro::Update({0}, {1})",
                this.Id,
                applicationUserId);
            ActionResult res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[ObjetivoRegistro_Update]
             *   @ObjetivoRegsitroId int,
             *   @CompanyId int,
             *   @ObjetivoId int,
             *   @Fecha datetime,
             *   @Valor decimal (18,6),
             *   @Comentari nvarchar(500),
             *   @ResponsableId int,
             *   @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("ObjetivoRegistro_Update"))
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                    {
                        cmd.Connection = cnn;
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoRegistroId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.ObjetivoId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                        cmd.Parameters.Add(DataParameter.Input("@Fecha", this.Date));
                        cmd.Parameters.Add(DataParameter.Input("@Valor", this.Value));
                        cmd.Parameters.Add(DataParameter.Input("@MetaComparer", this.MetaComparer, 10));
                        cmd.Parameters.Add(DataParameter.Input("@Meta", this.Meta));
                        cmd.Parameters.Add(DataParameter.Input("@Comentari", this.Comments, 500));
                        cmd.Parameters.Add(DataParameter.Input("@ResponsibleId", this.Responsible));
                        cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        this.Id = Convert.ToInt32(cmd.Parameters["@ObjetivoRegistroId"].Value.ToString());
                        res.SetSuccess(this.Id);
                        res.ReturnValue = this.Json;
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

        public ActionResult Save(int applicationUserId)
        {
            if (this.Id > 0)
            {
                return this.Update(applicationUserId);
            }

            return this.Insert(applicationUserId);
        }

        public static string ComparerLabel(string comparerValue, Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }


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
            if(dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

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
    }
}
