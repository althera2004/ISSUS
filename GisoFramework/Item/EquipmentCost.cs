// --------------------------------
// <copyright file="EquipmentCost.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Text;
    using GisoFramework.DataAccess;

    public class EquipmentCost
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public decimal CI { get; set; }
        public decimal CE { get; set; }
        public decimal VI { get; set; }
        public decimal VE { get; set; }
        public decimal MI { get; set; }
        public decimal ME { get; set; }
        public decimal RI { get; set; }
        public decimal RE { get; set; }
        public decimal Total
        {
            get
            {
                return CI + CE + VI + VE + MI + ME + RI + RE;
            }
        }

        public static string JsonList(ReadOnlyCollection<EquipmentCost> list)
        {
            var res = new StringBuilder("[");
            bool first = true;
            foreach(var cost in list)
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

        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{""Id"":{0}, ""D"":""{1}"", ""CI"":{2}, ""CE"":{3}, ""VI"":{4}, ""VE"":{5}, ""MI"":{6}, ""ME"":{7}, ""RI"":{8}, ""RE"":{9}, ""T"":{10}, ""A"":{11}}}",
                    this.Id,
                    this.Description,
                    this.CI,
                    this.CE,
                    this.VI,
                    this.VE,
                    this.MI,
                    this.ME,
                    this.RI,
                    this.RE,
                    this.Total,
                    this.Active ? Constant.JavaScriptTrue : Constant.JavaScriptTrue);
            }
        }

        public static ReadOnlyCollection<EquipmentCost> Filter(string from, string to, string filter, int companyId)
        {
            var fromDate = Tools.DateFromTextYYYYMMDD(from);
            var toDate = Tools.DateFromTextYYYYMMDD(to);
            var CI = filter.IndexOf("|CI") != -1;
            var CE = filter.IndexOf("|CE") != -1;
            var VI = filter.IndexOf("|VI") != -1;
            var VE = filter.IndexOf("|VE") != -1;
            var MI = filter.IndexOf("|MI") != -1;
            var ME = filter.IndexOf("|ME") != -1;
            var RI = filter.IndexOf("|RI") != -1;
            var RE = filter.IndexOf("|RE") != -1;
            var AC = filter.IndexOf("|AC") != -1;
            var IN = filter.IndexOf("|IN") != -1;

            var data = new List<EquipmentCost>();
            using (var cmd = new SqlCommand("Equipment_GetCosts2"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@From", fromDate));
                    cmd.Parameters.Add(DataParameter.Input("@to", toDate));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                data.Add(new EquipmentCost
                                {
                                    Id = rdr.GetInt64(8),
                                    Description = rdr.GetString(9),
                                    Active = rdr.GetBoolean(10),
                                    CI = CI ? rdr.GetDecimal(0) : 0,
                                    CE = CE ? rdr.GetDecimal(1) : 0,
                                    VI = VI ? rdr.GetDecimal(2) : 0,
                                    VE = VE ? rdr.GetDecimal(3) : 0,
                                    MI = MI ? rdr.GetDecimal(4) : 0,
                                    ME = ME ? rdr.GetDecimal(5) : 0,
                                    RI = RI ? rdr.GetDecimal(6) : 0,
                                    RE = RE ? rdr.GetDecimal(7) : 0
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

            return new ReadOnlyCollection<EquipmentCost>(data);
        }
    }
}