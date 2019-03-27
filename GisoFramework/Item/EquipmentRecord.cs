// --------------------------------
// <copyright file="EquipmentRecord.cs" company="OpenFramework">
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
    using System.Text;
    using System.Web;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>Implements EquipmentRecord class</summary>
    public class EquipmentRecord
    {
        public DateTime Date { get; set; }

        public string Item { get; set; }

        public int RecordType { get; set; }

        public string RecordTypeText { get; set; }

        public string Operation { get; set; }

        public Employee Responsible { get; set; }

        public decimal? Cost { get; set; }

        public string Json
        {
            get
            {
                var dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
                string itemType = dictionary[this.Item + "-" + (this.RecordType == 0 ? "Int" : "Ext")];
                var res = new StringBuilder("{");
                res.Append(Tools.JsonPair("Date", this.Date)).Append(",");
                res.Append(Tools.JsonPair("Type", itemType)).Append(",");
                res.Append(Tools.JsonPair("Operation", this.Operation)).Append(",");
                res.Append("\"Responsible\":").Append(this.Responsible.JsonKeyValue).Append(",");
                res.Append(Tools.JsonPair("Cost", this.Cost)).Append("}");
                return res.ToString();
            }
        }

        public static string EquipmentRecordJsonList(long equipmentId, int companyId, bool calibrationInternal, bool calibrationExternal, bool verificationInternal, bool verificationExternal, bool maintenanceInternal, bool maintenanceExternal, bool repairInternal, bool repairExternal, DateTime? dateFrom, DateTime? dateTo)
        {
            var first = true;
            var records = GetFilter(equipmentId, companyId, calibrationInternal, calibrationExternal, verificationInternal, verificationExternal, maintenanceInternal, maintenanceExternal, repairInternal, repairExternal, dateFrom, dateTo);
            var res = new StringBuilder("[");
            foreach (var record in records)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(record.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        public static ReadOnlyCollection<EquipmentRecord> GetFilter(long equipmentId, int companyId, bool calibrationInternal, bool calibrationExternal, bool verificationInternal, bool verificationExternal, bool maintenanceInternal, bool maintenanceExternal, bool repairInternal, bool repairExternal, DateTime? dateFrom, DateTime? dateTo)
        {
            /* CREATE PROCEDURE Equipment_GetRecords
             *   @EquipmentId bigint,
             *   @CompanyId int,
             *   @CalibrationInt bit,
             *   @CalibrationExt bit,
             *   @VerificationInt bit,
             *   @VerificationExt bit,
             *   @MaintenanceInt bit,
             *   @MaintenanceExt bit,
             *   @RepairInt bit,
             *   @RepairExt bit,
             *   @DateFrom datetime,
             *   @DateTo datetime */
            var res = new List<EquipmentRecord>();
            using (var cmd = new SqlCommand("Equipment_GetRecords"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@EquipmentId", equipmentId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@CalibrationInt", calibrationInternal));
                        cmd.Parameters.Add(DataParameter.Input("@CalibrationExt", calibrationExternal));
                        cmd.Parameters.Add(DataParameter.Input("@VerificationInt", verificationInternal));
                        cmd.Parameters.Add(DataParameter.Input("@VerificationExt", verificationExternal));
                        cmd.Parameters.Add(DataParameter.Input("@MaintenanceInt", maintenanceInternal));
                        cmd.Parameters.Add(DataParameter.Input("@MaintenanceExt", maintenanceExternal));
                        cmd.Parameters.Add(DataParameter.Input("@RepairInt", repairInternal));
                        cmd.Parameters.Add(DataParameter.Input("@RepairExt", repairExternal));
                        if (dateFrom.HasValue)
                        {
                            cmd.Parameters.Add(DataParameter.Input("@DateFrom", dateFrom.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@DateFrom"));
                        }

                        if (dateTo.HasValue)
                        {
                            cmd.Parameters.Add(DataParameter.Input("@DateTo", dateTo.Value));
                        }
                        else
                        {
                            cmd.Parameters.Add(DataParameter.InputNull("@DateTo"));
                        }

                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var record = new EquipmentRecord
                                {
                                    Date = rdr.GetDateTime(ColumnsEquipmentRecordGet.Date),
                                    Item = rdr.GetString(ColumnsEquipmentRecordGet.Item),
                                    RecordType = rdr.GetInt32(ColumnsEquipmentRecordGet.Type),
                                    Operation = rdr.GetString(ColumnsEquipmentRecordGet.Operation),
                                    Responsible = new Employee
                                    {
                                        Id = rdr.GetInt32(ColumnsEquipmentRecordGet.ResponsibleId),
                                        Name = rdr.GetString(ColumnsEquipmentRecordGet.ResponsibleName),
                                        LastName = rdr.GetString(ColumnsEquipmentRecordGet.ResponsibleLastName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsEquipmentRecordGet.Cost))
                                {
                                    record.Cost = rdr.GetDecimal(ColumnsEquipmentRecordGet.Cost);
                                }
                                else
                                {
                                    record.Cost = null;
                                }

                                res.Add(record);
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

            HttpContext.Current.Session["EquipmentRecordsFilter"] = res;
            return new ReadOnlyCollection<EquipmentRecord>(res);
        }
    }
}