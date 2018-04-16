// --------------------------------
// <copyright file="CustomerIncidentActions.cs" company="Sbrinna">
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

    /// <summary>Implements CustomerIncidentActions class</summary>
    public class CustomerIncidentActions
    {
        /// <summary>Gets or sets the customer thats report the action</summary>
        public Customer Customer { get; set; }

        /// <summary>Gets or sets the incident origin of action</summary>
        public Incident Incident { get; set; }

        /// <summary>Gets or sets an action related to customer</summary>
        public IncidentAction Action { get; set; }

        /// <summary>Gets or sets the relation</summary>
        public int Relation { get; set; }

        /// <summary>Gets or sets the status of action</summary>
        public int Status { get; set; }

        /// <summary>Extrant from data base the actions related to a customer</summary>
        /// <param name="customer">Customer of actions</param>
        /// <returns>A list of incident actions</returns>
        public static ReadOnlyCollection<CustomerIncidentActions> GetByCustomer(Customer customer)
        {
            if (customer == null)
            {
                return new ReadOnlyCollection<CustomerIncidentActions>(new List<CustomerIncidentActions>());
            }

            var res = new List<CustomerIncidentActions>();
            using (var cmd = new SqlCommand("Customer_GetIncidentActions"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CustomerId", customer.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", customer.CompanyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                CustomerIncidentActions newItem = new CustomerIncidentActions()
                                {
                                    Customer = customer
                                };

                                string itemType = rdr.GetString(ColumnsCustomerIncidentActionGet.ItemType);
                                switch (itemType)
                                {
                                    case "Incident":
                                        newItem.Incident = new Incident()
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomerIncidentActionGet.Id),
                                            Description = rdr.GetString(ColumnsCustomerIncidentActionGet.Description),
                                            Code = rdr.GetInt64(ColumnsCustomerIncidentActionGet.IncidentCode)
                                        };
                                        break;
                                    case "Action":
                                        newItem.Action = new IncidentAction()
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomerIncidentActionGet.Id),
                                            Description = rdr.GetString(ColumnsCustomerIncidentActionGet.Description),
                                            Number = rdr.GetInt64(ColumnsCustomerIncidentActionGet.ActionCode)
                                        };
                                        break;
                                }

                                newItem.Status = 0;
                                if (!rdr.IsDBNull(ColumnsCustomerIncidentActionGet.ClosedOn))
                                {
                                    newItem.Status = 4;
                                }
                                else if (!rdr.IsDBNull(ColumnsCustomerIncidentActionGet.ActionsOn))
                                {
                                    newItem.Status = 3;
                                }
                                else if (!rdr.IsDBNull(ColumnsCustomerIncidentActionGet.CausesOn))
                                {
                                    newItem.Status = 2;
                                }
                                else if (!rdr.IsDBNull(ColumnsCustomerIncidentActionGet.WhatHappenedOn))
                                {
                                    newItem.Status = 1;
                                }

                                newItem.Relation = 0;
                                if (rdr.GetInt64(ColumnsCustomerIncidentActionGet.AssociatedId) != 0)
                                {
                                    if (newItem.Action != null)
                                    {
                                        newItem.Incident = new Incident()
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomerIncidentActionGet.AssociatedId),
                                            Description = rdr.GetString(ColumnsCustomerIncidentActionGet.AssociatedDescription),
                                            Code = rdr.GetInt64(ColumnsCustomerIncidentActionGet.IncidentCode)
                                        };

                                        newItem.Relation = 2;
                                    }
                                    else
                                    {
                                        newItem.Action = new IncidentAction()
                                        {
                                            Id = rdr.GetInt64(ColumnsCustomerIncidentActionGet.AssociatedId),
                                            Description = rdr.GetString(ColumnsCustomerIncidentActionGet.AssociatedDescription),
                                            Number = rdr.GetInt64(ColumnsCustomerIncidentActionGet.ActionCode)
                                        };
                                        newItem.Relation = 1;
                                    }
                                }

                                res.Add(newItem);
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

            return new ReadOnlyCollection<CustomerIncidentActions>(res);
        }

        /// <summary>Render the HTML code for a row of customer actions list</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <param name="grants">User grants</param>
        /// <returns>HTML code for a row of customer actions list</returns>
        public string Row(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
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
                case 1:
                    status = dictionary["Item_Incident_Status1"]; 
                    break;
                case 2:
                    status = dictionary["Item_Incident_Status2"]; 
                    break;
                case 3:
                    status = dictionary["Item_Incident_Status3"]; 
                    break;
                case 4:
                    status = dictionary["Item_Incident_Status4"]; 
                    break;
                default:
                    status = "undefined"; 
                    break;
            }

            if (this.Incident != null && (this.Relation == 0 || this.Relation == 1))
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

            string pattern = @"<tr>
                    <td>{0}</td>
                    <td>{1}</td>
                    <td>{2}</td>
                    <td>{3}</td>
                  </tr>";

            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattern,
                itemName,
                description,
                status,
                associated);
        }
    }
}