// --------------------------------
// <copyright file="IndicadorHistorico.cs" company="OpenFramework">
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

    public class IndicadorHistorico : BaseItem
    {
        public int IndicadorId { get; set; }

        public DateTime Date { get; set; }

        public Employee Employee { get; set; }

        public string Reason { get; set; }

        public override string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{
                        ""Id"":{0},
                        ""IndicadorId"":{1},
                        ""Date"":""{2:dd/MM/yyyy}"",
                        ""Reason"":""{3}"",
                        ""Employee"":{4}
                    }}",
                    this.Id,
                    this.IndicadorId,
                    this.Date,
                    Tools.JsonCompliant(this.Reason),
                    this.Employee.JsonKeyValue);
            }
        }

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public static string ByIndicadorIdJsonList(int indicadorId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var historico in ByIndicadorId(indicadorId))
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(historico.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<IndicadorHistorico> ByIndicadorId(int indicadorId)
        {
            var res = new List<IndicadorHistorico>();
            using (var cmd = new SqlCommand("IndicadorHistorico_GetByIndicadorId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@IndicadorId", indicadorId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new IndicadorHistorico
                                {
                                    Id = Convert.ToInt32(rdr.GetInt64(0)),
                                    IndicadorId = rdr.GetInt32(1),
                                    Date = rdr.GetDateTime(2),
                                    Reason = rdr.GetString(3),
                                    Employee = new Employee()
                                    {
                                        Id = rdr.GetInt32(4),
                                        Name = rdr.GetString(5),
                                        LastName = rdr.GetString(6)
                                    }
                                });
                            }
                        }
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

            return new ReadOnlyCollection<IndicadorHistorico>(res);
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE IndicadorHistorico_Insert
             *   @Id int output,
             *   @IndicadorId int,
             *   @CompanyId int,
             *   @ActionDate datetime,
             *   @Reason nvarchar(500),
             *   @EmployeeId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("IndicadorHistorico_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@IndicadorId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@ActionDelete", this.Date));
                    cmd.Parameters.Add(DataParameter.Input("@Reason", this.Reason, 500));
                    cmd.Parameters.Add(DataParameter.Input("@EmployeeId", this.Employee.Id));
                    cmd.Parameters.Add(DataParameter.Input("@ApplicationUserId", applicationUserId));
                    try
                    {
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        var id = Convert.ToInt32(cmd.Parameters["@Id"].Value);
                        res.SetSuccess(id);
                    }
                    catch (Exception ex)
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