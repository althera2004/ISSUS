using GisoFramework.Activity;
using GisoFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GisoFramework.Item
{
    public class IndicadorObjetivo
    {
        public int IndicadorId { get; set; }
        public int ObjetivoId { get; set; }
        public int CompanyId { get; set; }
        public bool Active { get; set; }

        public static ActionResult Save(int indicadorId, int objetivoId, int companyId, int applicatioUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorObjetivo_Save
             * @ObjetivoId int,
             * @IndicadorId int,
             * @CompanyId int,
             * @ApplicationUserId int */
            using (var cmd = new SqlCommand("IndicadorObjetivo_Save"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@ApplicatoinUserId", applicatioUserId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "IndicadorObjetivo::Save()");
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        public static ActionResult Delete(int indicadorId, int objetivoId, int companyId, int applicatioUserId)
        {
            ActionResult res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorObjetivo_Delete
             * @ObjetivoId int,
             * @IndicadorId int,
             * @CompanyId int,
             * @ApplicationUserId int */
            using (SqlCommand cmd = new SqlCommand("IndicadorObjetivo_Delete"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                cmd.Parameters.Add(DataParameter.Input("@ApplicatoinUserId", applicatioUserId));
                using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "IndicadorObjetivo::Delete()");
                        res.SetFail(ex);
                    }
                    finally
                    {
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            return res;
        }

        public static ReadOnlyCollection<IndicadorObjetivo> ByIndicadorId(int indicadorId,int companyId)
        {
            var res = new List<IndicadorObjetivo>();
            /* CREATE PROCEDURE IndicadorObjetivo_GetByIndicadorId
             *   @IndicadorId int,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("IndicadorObjetivo_GetByIndicadorId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IndicadorObjetivo()
                                {
                                    IndicadorId = rdr.GetInt32(0),
                                    ObjetivoId = rdr.GetInt32(1),
                                    CompanyId = rdr.GetInt32(2),
                                    Active = rdr.GetBoolean(3)
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "IndicadorObjetivo::GetByObjetivo()");
                        res = new List<IndicadorObjetivo>();
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

            return new ReadOnlyCollection<IndicadorObjetivo>(res);
        }

        public static ReadOnlyCollection<IndicadorObjetivo> ByObjetivoId(int objetivoId, int companyId)
        {
            var res = new List<IndicadorObjetivo>();
            /* CREATE PROCEDURE IndicadorObjetivo_GetByObjetivoId
             *   @IndicadorId int,
             *   @CompanyId int */
            using (var cmd = new SqlCommand("IndicadorObjetivo_GetByObjetivoId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IndicadorObjetivo
                                {
                                    IndicadorId = rdr.GetInt32(0),
                                    ObjetivoId = rdr.GetInt32(1),
                                    CompanyId = rdr.GetInt32(2),
                                    Active = rdr.GetBoolean(3)
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Trace(ex, "IndicadorObjetivo::GetByObjetivo()");
                        res = new List<IndicadorObjetivo>();
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

            return new ReadOnlyCollection<IndicadorObjetivo>(res);
        }

        public static string JsonList(ReadOnlyCollection<IndicadorObjetivo> list)
        {
            if(list == null)
            {
                return "[]";
            }

            var res = new StringBuilder("[");
            bool first = true;
            foreach (var item in list)
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""IndicadorId"":{0},""ObjetivoId"":{1},""CompanyId"":{2},""Active"":{3}}}",
                    this.IndicadorId,
                    this.ObjetivoId,
                    this.CompanyId,
                    this.Active ? "true" : "false");
            }
        }
    }
}
