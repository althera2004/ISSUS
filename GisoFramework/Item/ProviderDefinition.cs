// --------------------------------
// <copyright file="ProviderDefinition.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
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
    /// Implements ProviderDefinition class
    /// </summary>
    public class ProviderDefinition
    {
        public Provider Provider { get; set; }

        public Employee Responsible { get; set; }

        public Equipment Equipment { get; set; }

        public decimal? Cost { get; set; }

        public string DefinitionType { get; set; }

        public string Description { get; set; }

        public int Periodicity { get; set; }

        public bool Active { get; set; }

        public static ReadOnlyCollection<ProviderDefinition> GetByProvider(Provider provider)
        {
            List<ProviderDefinition> res = new List<ProviderDefinition>();
            if (provider == null)
            {
                return new ReadOnlyCollection<ProviderDefinition>(res);
            }

            using (SqlCommand cmd = new SqlCommand("Provider_GetDefinitions"))
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
                        ProviderDefinition newItem = new ProviderDefinition()
                        {
                            Provider = provider,
                            Responsible = new Employee()
                            {
                                Id = rdr.GetInt32(ColumnsProviderGetDefinitions.Responsible),
                                Name = rdr.GetString(ColumnsProviderGetDefinitions.ResponsibleName),
                                LastName = rdr.GetString(ColumnsProviderGetDefinitions.ResponsibleLastName)
                            },
                            Equipment = new Equipment()
                            {
                                Id = rdr.GetInt64(ColumnsProviderGetDefinitions.EquipmentId),
                                Description = rdr.GetString(ColumnsProviderGetDefinitions.EquipmentDescription),
                                Code = rdr.GetString(ColumnsProviderGetDefinitions.EquipmentCode)
                            },
                            Active = rdr.GetBoolean(ColumnsProviderGetDefinitions.Active),
                            
                            Description = rdr.GetString(ColumnsProviderGetDefinitions.Operation),
                            Periodicity = rdr.GetInt32(ColumnsProviderGetDefinitions.Periodicity),
                            DefinitionType = rdr.GetString(ColumnsProviderGetDefinitions.Item)
                        };

                        if (!rdr.IsDBNull(ColumnsProviderGetDefinitions.Cost))
                        {
                            newItem.Cost = rdr.GetDecimal(ColumnsProviderGetDefinitions.Cost);
                        }
                        else
                        {
                            newItem.Cost = null;
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

            return new ReadOnlyCollection<ProviderDefinition>(res);
        }

        /*public string Row(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            bool grantEquipment = UserGrant.HasWriteGrant(grants, ApplicationGrant.Equipment);
            bool grantEmployee = UserGrant.HasWriteGrant(grants, ApplicationGrant.Employee);

            string pattern = @"<tr>
                    <td>{0}</td>
                    <td>{1}</td>
                    <td>{2}</td>
                    <td align=""right"">{3}</td>
                    <td>{4}</td>
                    <td align=""right"">{5}</td>
                  </tr>";

            return string.Format(
                CultureInfo.InvariantCulture,
                pattern,
                grantEquipment ? this.Equipment.Link : this.Equipment.FullName,
                this.DefinitionType,
                this.Description,
                this.Periodicity,
                grantEmployee ? this.Responsible.Link : this.Responsible.FullName,
                string.Format(CultureInfo.GetCultureInfo("es-es"), "{0:#,##0.00}", this.Cost));
        }*/
    }
}
