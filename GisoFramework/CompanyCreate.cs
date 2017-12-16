// --------------------------------
// <copyright file="CompanyCreate.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;

    /// <summary>
    /// Implementation of CompnayCreate class.
    /// </summary>
    public static class CompanyCreate
    {
        /// <summary>
        /// Char for separator
        /// </summary>
        private const string Separator = "|";

        /// <summary>
        /// Insert a new compnay in database
        /// </summary>
        /// <param name="companyName">Company name</param>
        /// <param name="companyCode">Company code</param>
        /// <param name="companyNif">Company nif</param>
        /// <param name="companyAddress">Company street address</param>
        /// <param name="companyPostalCode">Company postal code</param>
        /// <param name="companyCity">Company city</param>
        /// <param name="companyProvince">Company province</param>
        /// <param name="companyCountry">Company country</param>
        /// <param name="companyPhone">Company phone</param>
        /// <param name="companyMobile">Company mobile</param>
        /// <param name="companyFax">Company fax</param>
        /// <param name="companyEmail">Company email</param>
        /// <param name="userName">User name</param>
        /// <returns>Result of action</returns>
        public static ActionResult Create(string companyName, string companyCode, string companyNif, string companyAddress, string companyPostalCode, string companyCity, string companyProvince, string companyCountry, string companyPhone, string companyMobile, string companyFax, string userName, string companyEmail)
        {
            /* CREATE PROCEDURE Company_Create
             *   @CompanyId int out,
             *   @Login nvarchar(50) out,
             *   @Password nvarchar(50) out,
             *   @Name nvarchar(50),
             *   @Code nvarchar(10),
             *   @NIF nvarchar(15),
             *   @Address nvarchar(50),
             *   @PostalCode nvarchar(10),
             *   @City nvarchar(50),
             *   @Province nvarchar(50),
             *   @Country nvarchar(15),
             *   @Phone nvarchar(15),
             *   @Mobile nvarchar(15),
             *   @Email nvarchar(50),
             *   @Fax nvarchar(50),
             *   @EmployeeName nvarchar(50),
             *   @EmployeeLastName nvarchar(50),
             *   @EmployeeNif nvarchar(15),
             *   @EmployeePhone nvarchar(15),
             *   @UserName nvarchar(50),
             *   @EmployeeEmail nvarchar(50) */
            ActionResult res = ActionResult.NoAction;
            using (SqlCommand cmd = new SqlCommand("Company_Create"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cmd.Parameters.Add(DataParameter.OutputInt("@CompanyId"));
                    cmd.Parameters.Add(DataParameter.OutputString("@Login", 50));
                    cmd.Parameters.Add(DataParameter.OutputString("@Password", 50));
                    cmd.Parameters.Add(DataParameter.Input("@Name", companyName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Code", companyCode, 10));
                    cmd.Parameters.Add(DataParameter.Input("@NIF", companyNif, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Address", companyAddress, 50));
                    cmd.Parameters.Add(DataParameter.Input("@PostalCode", companyPostalCode, 10));
                    cmd.Parameters.Add(DataParameter.Input("@City", companyCity, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Province", companyProvince, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Country", companyCountry, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Phone", companyPhone, 15));
                    cmd.Parameters.Add(DataParameter.Input("@Mobile", companyMobile, 15));
                    cmd.Parameters.Add(DataParameter.Input("@UserName", userName, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Email", companyEmail, 50));
                    cmd.Parameters.Add(DataParameter.Input("@Fax", companyFax, 50));
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    res.SetSuccess(userName + Separator + companyEmail + Separator + cmd.Parameters["@Password"].Value.ToString());
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (ArgumentNullException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
                    res.SetFail(ex);
                }
                catch (InvalidOperationException ex)
                {
                    ExceptionManager.Trace(ex, "CreateCompany");
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

            return res;
        }
    }
}