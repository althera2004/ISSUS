// --------------------------------
// <copyright file="Customer.cs" company="Sbrinna">
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
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Customer : BaseItem
    {
        public static Customer Empty
        {
            get
            {
                return new Customer()
                {
                    Id = 0,
                    Description = string.Empty,
                    ModifiedBy = ApplicationUser.Empty
                };
            }
        }

        /// <summary>Gets an identifier/description json item</summary>
        public override string JsonKeyValue
        {
            get
            {
                string description = this.Description;
                if (string.IsNullOrEmpty(description))
                {
                    description = string.Empty;
                }

                return string.Format(CultureInfo.GetCultureInfo("en-us"), @"{{""Id"":{0},""Description"":""{1}""}}", this.Id, description.Replace("\"", "\\\""));
            }
        }

        /// <summary>Gets the structure json item</summary>
        public override string Json
        {
            get
            {
                string pattern = 
                    @"{{
                        ""Id"": {0},
                        ""Description"": ""{1}"",
                        ""Active"": {2},
                        ""Deletable"": {3},
                        ""CompanyId"": {4}
                      }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    this.Id,
                    this.Description.Replace("\"", "\\\""),
                    this.Active ? "true" : "false",
                    this.CanBeDeleted ? "true" : "false",
                    this.CompanyId);
            }
        }

        public override string Link
        {
            get
            {
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    @"<a href=""CustomersView.aspx?id={0}"">{1}</a>",
                    this.Id,
                    this.Description);
            }
        }

        /*public override string Differences(BaseItem item1)
        {
            Customer c1 = item1 as Customer;

            if (c1.Description != this.Description)
            {
                return string.Format(CultureInfo.GetCultureInfo("en-us"), "Description{0}", this.Description);
            }

            return string.Empty;
        }*/

        public static ReadOnlyCollection<Customer> GetByCompany(int companyId)
        {
            /* CREATE PROCEDURE Customer_GetByCompany
             *   @CompanyId int */
            List<Customer> res = new List<Customer>();
            using (SqlCommand cmd = new SqlCommand("Customer_GetByCompany"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        bool deletable = true;
                        if (rdr.GetInt32(ColumnsCustomerGetByCompany.InIncident) == 1)
                        {
                            deletable = false;
                        }

                        if (rdr.GetInt32(ColumnsCustomerGetByCompany.InActionIncident) == 1)
                        {
                            deletable = false;
                        }

                        res.Add(new Customer()
                        {
                            Id = rdr.GetInt64(ColumnsCustomerGetByCompany.Id),
                            CompanyId = rdr.GetInt32(ColumnsCustomerGetByCompany.CompanyId),
                            Description = rdr.GetString(ColumnsCustomerGetByCompany.Description),
                            Active = rdr.GetBoolean(ColumnsCustomerGetByCompany.Active),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsCustomerGetBy.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsCustomerGetByCompany.ModifiedByUserName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsCustomerGetBy.ModifiedOn),
                            CanBeDeleted = deletable
                        });
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

            return new ReadOnlyCollection<Customer>(res);
        }

        public static string GetByCompanyJson(int companyId)
        {
            StringBuilder res = new StringBuilder("[");
            bool first = true;
            foreach (Customer customer in GetByCompany(companyId))
            {
                if (customer.Active)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        res.Append(",");
                    }

                    res.Append(customer.Json);
                }
            }

            res.Append("]");
            return res.ToString();
        }

        public static Customer GetById(long id, int companyId)
        {
            Customer res = new Customer();
            using (SqlCommand cmd = new SqlCommand("Customer_GetById"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    cmd.Connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        rdr.Read();
                        res =  new Customer()
                        {
                            Id = rdr.GetInt64(ColumnsCustomerGetBy.Id),
                            CompanyId = rdr.GetInt32(ColumnsCustomerGetBy.CompanyId),
                            Description = rdr.GetString(ColumnsCustomerGetBy.Description),
                            Active = rdr.GetBoolean(ColumnsCustomerGetBy.Active),
                            ModifiedBy = new ApplicationUser()
                            {
                                Id = rdr.GetInt32(ColumnsCustomerGetBy.ModifiedByUserId),
                                UserName = rdr.GetString(ColumnsCustomerGetBy.ModifiedByUserName)
                            },
                            ModifiedOn = rdr.GetDateTime(ColumnsCustomerGetBy.ModifiedOn)
                        };

                        res.ModifiedBy.Employee = Employee.GetByUserId(res.ModifiedBy.Id);
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

            return res;
        }

        public string ListRow(Dictionary<string, string> dictionary, ReadOnlyCollection<UserGrant> grants)
        {
            if (grants == null)
            {
                return string.Empty;
            }

            if (dictionary == null)
            {
                dictionary = HttpContext.Current.Session["Dictionary"] as Dictionary<string, string>;
            }

            bool grantWrite = UserGrant.HasWriteGrant(grants, ApplicationGrant.Customer);
            bool grantDelete = UserGrant.HasDeleteGrant(grants, ApplicationGrant.Customer);

            string iconDelete = string.Empty;
            if (grantDelete)
            {
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "CustomerDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconDelete = string.Format(
                    CultureInfo.InvariantCulture,
                    @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>",
                    deleteFunction,
                    Tools.LiteralQuote(Tools.JsonCompliant(this.Description)),
                    Tools.JsonCompliant(dictionary["Common_Delete"]));

            }

            string iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='CustomersView.aspx?id={0}';""><i class=""icon-eye-open bigger-120""></i></span>",
                this.Id,
                dictionary["Common_View"],
                this.Description);

            if (grantWrite)
            {
                iconEdit = string.Format(
                CultureInfo.InvariantCulture,
                @"<span title=""{1} '{2}'"" class=""btn btn-xs btn-info"" onclick=""document.location='CustomersView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>",
                this.Id,
                dictionary["Common_Edit"],
                this.Description);
            }

            /*if (grantWrite)
            {
                string validDescription = Tools.LiteralQuote(Tools.JsonCompliant(this.Description));
                string deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "CustomerDelete({0},'{1}');", this.Id, this.Description);
                if (!this.CanBeDeleted)
                {
                    deleteFunction = string.Format(CultureInfo.GetCultureInfo("en-us"), "warningInfoUI('{0}', null, 400);", dictionary["Common_Warning_Undelete"]);
                }

                iconEdit = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-info"" onclick=""document.location='CustomersView.aspx?id={0}';""><i class=""icon-edit bigger-120""></i></span>", this.Id, validDescription, Tools.JsonCompliant(dictionary["Common_Edit"]));
                iconDelete = string.Format(CultureInfo.GetCultureInfo("en-us"), @"<span title=""{2} {1}"" class=""btn btn-xs btn-danger"" onclick=""{0}""><i class=""icon-trash bigger-120""></i></span>", deleteFunction, validDescription, Tools.JsonCompliant(dictionary["Common_Delete"]));
            }*/

            string pattenr = @"<tr><td>{0}</td><td style=""width:90px;"">{1}&nbsp;{2}</td></tr>";
            return string.Format(
                CultureInfo.GetCultureInfo("en-us"),
                pattenr,
                this.Link,
                iconEdit,
                iconDelete);
        }

        public ActionResult Insert(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Customer_Insert"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.OutputInt("@CustomerId"));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    this.Id = Convert.ToInt32(cmd.Parameters["@CustomerId"].Value, CultureInfo.GetCultureInfo("en-us"));
                    result.SetSuccess(this.Id.ToString(CultureInfo.InvariantCulture));
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "Customer::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Customer::Insert", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        public ActionResult Update(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Customer_Update"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@Description", this.Description));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "Customer::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Customer::Update", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }

        public ActionResult Delete(int userId)
        {
            ActionResult result = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Customer_Delete"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CustomerId", this.Id));
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.CompanyId));
                    cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    result.SetSuccess();
                }
                catch (SqlException ex)
                {
                    result.SetFail(ex);
                    ExceptionManager.Trace(ex, "Customer::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Customer::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "Id:{0} - Name{1}", this.Id, this.Description));
                }
                finally
                {
                    if (cmd.Connection.State != ConnectionState.Closed)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return result;
        }
    }
}
