using GisoFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace GisoFramework.Item
{
    public class ApartadoNorma
    {
        public long NormaId { get; set; }
        public string Apartado { get; set; }

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""R"":{0},""A"":""{1}""}}",
                    this.NormaId,
                    Tools.JsonCompliant(this.Apartado));
            }
        }

        public static string JsonList(long companyId)
        {
            var res = new StringBuilder("[");
            bool first = true;
            using(var cmd = new SqlCommand("ApartadosNorma_All"))
            {
                using(var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using(var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                if (first)
                                {
                                    first = false;
                                }
                                else
                                {
                                    res.Append(",");
                                }

                                res.AppendFormat(
                                    CultureInfo.InvariantCulture,
                                    @"{{""R"":{0},""A"":""{1}""}}",
                                    rdr.GetInt64(0),
                                    rdr.GetString(1));
                            }
                        }
                    }
                    finally
                    {
                        if(cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }

            res.Append("]");
            return res.ToString();
        }
    }
}
