// -----------------------------------------------------------------------
// <copyright file="ProviderCost.cs" company="Microsoft">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Web;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>
    /// Implements ProviderCost class
    /// </summary>
    public class ProviderCost
    {
        public Provider Provider { get; set; }
        public Equipment Equipment { get; set; }
        public EquipmentCalibrationAct Calibration { get; set; }
        public EquipmentVerificationAct Verification { get; set; }
        public EquipmentMaintenanceAct Maintenance { get; set; }
        public EquipmentRepair Repair { get; set; }

        public string Row(Dictionary<string,string> dictionary,  ReadOnlyCollection<UserGrant> grants)
        {
            if(grants==null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantEquipment = UserGrant.HasWriteGrant(grants, ApplicationGrant.Equipment);
            bool grantEmployee = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);

            string itemName = string.Empty;
            string employee = string.Empty;
            DateTime date = new DateTime();
            decimal? amount = 0;
            string operation = string.Empty;

            if (this.Calibration != null)
            {
                itemName = dictionary["Item_EquipmentCalibration"];
                employee = grantEmployee ? this.Calibration.Responsible.Link : this.Calibration.Responsible.FullName;
                date = this.Calibration.Date;
                amount = this.Calibration.Cost;
                operation = this.Calibration.Description;
            }
            else if (this.Verification != null)
            {
                itemName = dictionary["Item_EquipmentVerification"];
                employee = grantEmployee ? this.Verification.Responsible.Link : this.Verification.Responsible.FullName;
                date = this.Verification.Date;
                amount = this.Verification.Cost;
                operation = this.Verification.Description;
            }
            else if (this.Maintenance != null)
            {
                itemName = dictionary["Item_EquipmentMaintenance"];
                employee = grantEmployee ? this.Maintenance.Responsible.Link : this.Maintenance.Responsible.FullName;
                date = this.Maintenance.Date;
                amount = this.Maintenance.Cost;
                operation = this.Maintenance.Description;
            }
            else if (this.Repair != null)
            {
                itemName = dictionary["Item_EquipmentRepair"];
                employee = grantEmployee ? this.Repair.Responsible.Link : this.Repair.Responsible.FullName;
                date = this.Repair.Date;
                amount = this.Repair.Cost;
                operation = this.Repair.Description;
            }

            string cost = string.Empty;
            if (amount.HasValue)
            {
                cost = string.Format(CultureInfo.GetCultureInfo("en-us"), "{0:#,##0.00}", amount).Replace(".", ",");
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr>
                    <td>{5}</td>
                    <td>{0}</td>
                    <td>{1}</td>
                    <td align=""center"">{2:dd/MM/yyyy}</td>
                    <td>{3}</td>
                    <td align=""right"">{4}</td>
                  </tr>",
                        itemName,
                        operation,
                        date,
                        employee,
                        cost,
                        grantEquipment ? this.Equipment.Link : this.Equipment.Description);

        }

        public static ReadOnlyCollection<ProviderCost> GetByProvider(Provider provider)
        {
            if (provider == null)
            {
                return new ReadOnlyCollection<ProviderCost>(new List<ProviderCost>());
            }

            List<ProviderCost> res = new List<ProviderCost>();
            using (SqlCommand cmd = new SqlCommand("Provider_GetCosts"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@ProviderId", provider.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", provider.CompanyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ProviderCost newItem = new ProviderCost()
                        {
                            Provider = provider,
                            Equipment = new Equipment()
                            {
                                Id = rdr.GetInt64(ColumnsProviderCostGet.EquipmentId),
                                Description = rdr.GetString(ColumnsProviderCostGet.EquipmentDescription),
                                Code = rdr.GetString(ColumnsProviderCostGet.EquipmentCode)
                            }
                        };

                        string itemType = rdr.GetString(ColumnsProviderCostGet.Item);
                        switch(itemType)
                        {
                            case "Calibration":
                                newItem.Calibration = new EquipmentCalibrationAct()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderCostGet.Id),
                                    Date = rdr.GetDateTime(ColumnsProviderCostGet.Date),
                                    Description = rdr.GetString(ColumnsProviderCostGet.Operation),
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderCostGet.Responsible),
                                        Name = rdr.GetString(ColumnsProviderCostGet.Name),
                                        LastName = rdr.GetString(ColumnsProviderCostGet.LastName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsProviderCostGet.Cost))
                                {
                                    newItem.Calibration.Cost = rdr.GetDecimal(ColumnsProviderCostGet.Cost);
                                }
                                else
                                {
                                    newItem.Calibration.Cost = null;
                                }

                                break;
                            case "Verification":
                                newItem.Verification = new EquipmentVerificationAct()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderCostGet.Id),
                                    Date = rdr.GetDateTime(ColumnsProviderCostGet.Date),
                                    Description = rdr.GetString(ColumnsProviderCostGet.Operation),
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderCostGet.Responsible),
                                        Name = rdr.GetString(ColumnsProviderCostGet.Name),
                                        LastName = rdr.GetString(ColumnsProviderCostGet.LastName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsProviderCostGet.Cost))
                                {
                                    newItem.Verification.Cost = rdr.GetDecimal(ColumnsProviderCostGet.Cost);
                                }
                                else
                                {
                                    newItem.Verification.Cost = null;
                                }

                                break;
                            case "Maintenance":
                                newItem.Maintenance = new EquipmentMaintenanceAct()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderCostGet.Id),
                                    Date = rdr.GetDateTime(ColumnsProviderCostGet.Date),
                                    Description = rdr.GetString(ColumnsProviderCostGet.Operation),
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderCostGet.Responsible),
                                        Name = rdr.GetString(ColumnsProviderCostGet.Name),
                                        LastName = rdr.GetString(ColumnsProviderCostGet.LastName)
                                    }
                                };

                                if(!rdr.IsDBNull(ColumnsProviderCostGet.Cost))
                                {
                                    newItem.Maintenance.Cost = rdr.GetDecimal(ColumnsProviderCostGet.Cost);
                                }
                                else
                                {
                                    newItem.Maintenance.Cost = null;
                                }

                                break;
                            case "Repair":
                                newItem.Repair = new EquipmentRepair()
                                {
                                    Id = rdr.GetInt64(ColumnsProviderCostGet.Id),
                                    Date = rdr.GetDateTime(ColumnsProviderCostGet.Date),
                                     Description = rdr.GetString(ColumnsProviderCostGet.Operation),
                                    Responsible = new Employee()
                                    {
                                        Id = rdr.GetInt32(ColumnsProviderCostGet.Responsible),
                                        Name = rdr.GetString(ColumnsProviderCostGet.Name),
                                        LastName = rdr.GetString(ColumnsProviderCostGet.LastName)
                                    }
                                };

                                if (!rdr.IsDBNull(ColumnsProviderCostGet.Cost))
                                {
                                    newItem.Repair.Cost = rdr.GetDecimal(ColumnsProviderCostGet.Cost);
                                }
                                else
                                {
                                    newItem.Repair.Cost = null;
                                }

                                break;
                        }

                        res.Add(newItem);
                    }
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }

                    cmd.Connection.Dispose();
                }
            }

            return new ReadOnlyCollection<ProviderCost>(res);
        }
    }
}
