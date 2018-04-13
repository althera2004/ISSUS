// --------------------------------
// <copyright file="ObjetivoHistorico.cs" company="Sbrinna">
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
    using System.Text;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;

    public class ObjetivoHistorico : BaseItem
    {
        public int ObjetivoId { get; set; }

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
                        ""ObjetivoId"":{1},
                        ""Date"":""{2:dd/MM/yyyy}"",
                        ""Reason"":""{3}"",
                        ""Employee"":{4}
                    }}",
                    this.Id,
                    this.ObjetivoId,
                    this.Date,
                    Tools.JsonCompliant(this.Reason),
                    this.Employee.JsonKeyValue);
            }
        }

        public override string JsonKeyValue => throw new NotImplementedException();

        public override string Link => throw new NotImplementedException();

        public static string ByObjetivoIdJsonList(int objetivoId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach (var historico in ByObjetivoId(objetivoId))
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

        public static ReadOnlyCollection<ObjetivoHistorico> ByObjetivoId(int objetivoId)
        {
            var res = new List<ObjetivoHistorico>();
            using (var cmd = new SqlCommand("ObjetivoHistorico_GetByObjetivoId"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", objetivoId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new ObjetivoHistorico
                                {
                                    Id = Convert.ToInt32(rdr.GetInt64(0)),
                                    ObjetivoId = rdr.GetInt32(1),
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

            return new ReadOnlyCollection<ObjetivoHistorico>(res);
        }

        public ActionResult Insert(int applicationUserId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE ObjetivoHistorico_Insert
             *   @Id int output,
             *   @ObjetivoId int,
             *   @CompanyId int,
             *   @ActionDate datetime,
             *   @Reason nvarchar(500),
             *   @EmployeeId int,
             *   @ApplicationUserId int */
            using (var cmd = new SqlCommand("ObjetivoHistorico_Insert"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.Parameters.Add(DataParameter.OutputInt("@Id"));
                    cmd.Parameters.Add(DataParameter.Input("@ObjetivoId", this.Id));
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