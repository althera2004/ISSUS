// -----------------------------------------------------------------------
// <copyright file="ProviderCost.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.Item
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    using System.Web;

    /// <summary>
    /// Implements ProviderIncidentActions class
    /// </summary>
    public class ProviderIncidentActions
    {
        public Provider Provider { get; set; }
        public Incident Incident { get; set; }
        public IncidentAction Action { get; set; }
        public int Relation { get; set; }
        public int Status { get; set; }

        public string Row(Dictionary<string,string> dictionary,  ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantIncident = UserGrant.HasWriteGrant(grants, ApplicationGrant.Incident);
            bool grantAction = UserGrant.HasWriteGrant(grants, ApplicationGrant.IncidentActions);

            string itemName = string.Empty;
            string status = string.Empty;
            string description = string.Empty;
            string associated = string.Empty;

             switch (this.Status)
                {
                    case 1: status = dictionary["Item_Incident_Status1"]; break;
                    case 2: status = dictionary["Item_Incident_Status2"]; break;
                    case 3: status = dictionary["Item_Incident_Status3"]; break;
                    case 4: status = dictionary["Item_Incident_Status4"]; break;
                    default: status = "undefined"; break;
                }

            if (this.Incident != null && this.Relation==0 || this.Relation == 1)
            {
                itemName = dictionary["Item_Incident"];
                description = grantIncident ? this.Incident.Link : this.Incident.Description;
                if (this.Relation == 1)
                {
                    associated = dictionary["Item_IncidentAction"] + ": " + (grantAction ? this.Action.Link : this.Action.Description);
                }
            }
            else 
            {
                itemName = dictionary["Item_IncidentAction"];
                description = grantAction ? this.Action.Link : this.Action.Description;
                if (this.Relation == 2)
                {
                    associated = dictionary["Item_Incident"] + ": " + (grantIncident ? this.Incident.Link : this.Incident.Description);
                }
            }

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                @"<tr>
                    <td>{0}</td>
                    <td>{1}</td>
                    <td>{2}</td>
                    <td>{3}</td>
                  </tr>",
                        itemName,
                        description,
                        status,
                        associated);

        }

        public static ReadOnlyCollection<ProviderIncidentActions> ByProvider(Provider provider)
        {
            var res = new List<ProviderIncidentActions>();
            if (provider == null)
            {
                return new ReadOnlyCollection<ProviderIncidentActions>(res);
            }

            using (var cmd = new SqlCommand("Provider_GetIncidentActions"))
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
                        var newItem = new ProviderIncidentActions
                        {
                            Provider = provider
                        };

                        string itemType = rdr.GetString(ColumnsProviderIncidentActionsGet.ItemType);
                        switch(itemType)
                        {
                            case "Incident":
                                newItem.Incident = new Incident
                                {
                                    Id = rdr.GetInt64(ColumnsProviderIncidentActionsGet.Id),
                                    Description = rdr.GetString(ColumnsProviderIncidentActionsGet.Description),
                                    Code = rdr.GetInt64(ColumnsProviderIncidentActionsGet.IncidentCode)
                                };
                                break;
                            case "Action":
                                newItem.Action = new IncidentAction
                                {
                                    Id = rdr.GetInt64(ColumnsProviderIncidentActionsGet.Id),
                                    Description = rdr.GetString(ColumnsProviderIncidentActionsGet.Description),
                                    Number = rdr.GetInt64(ColumnsProviderIncidentActionsGet.ActionCode)
                                };
                                break;
                        }

                        newItem.Status = 0;
                        if (!rdr.IsDBNull(ColumnsProviderIncidentActionsGet.ClosedOn))
                        {
                            newItem.Status = 4;
                        }
                        else if (!rdr.IsDBNull(ColumnsProviderIncidentActionsGet.ActionsOn))
                        {
                            newItem.Status = 3;
                        }
                        else if (!rdr.IsDBNull(ColumnsProviderIncidentActionsGet.CausesOn))
                        {
                            newItem.Status = 2;
                        }
                        else if (!rdr.IsDBNull(ColumnsProviderIncidentActionsGet.WhatHappenedOn))
                        {
                            newItem.Status = 1;
                        }

                        newItem.Relation = 0;
                        if (rdr.GetInt64(ColumnsProviderIncidentActionsGet.AssociatedId) != 0)
                        {
                            if (newItem.Action != null)
                            {
                                newItem.Incident = new Incident
                                {
                                    Id = rdr.GetInt64(ColumnsProviderIncidentActionsGet.AssociatedId),
                                    Description = rdr.GetString(ColumnsProviderIncidentActionsGet.AssociatedDescription),
                                    Code = rdr.GetInt64(ColumnsProviderIncidentActionsGet.IncidentCode)
                                };

                                newItem.Relation = 2;
                            }
                            else
                            {

                                newItem.Action = new IncidentAction
                                {
                                    Id = rdr.GetInt64(ColumnsProviderIncidentActionsGet.AssociatedId),
                                    Description = rdr.GetString(ColumnsProviderIncidentActionsGet.AssociatedDescription),
                                    Number = rdr.GetInt64(ColumnsProviderIncidentActionsGet.ActionCode)
                                };
                                newItem.Relation = 1;
                            }
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

            return new ReadOnlyCollection<ProviderIncidentActions>(res);
        }
    }
}