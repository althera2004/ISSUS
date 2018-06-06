// --------------------------------
// <copyright file="CompanyAddress.cs" company="Sbrinna">
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
    using GisoFramework.Item.Binding;

    /// <summary>Implements CompanyAddress class</summary>
    public class CompanyAddress
    {
        /// <summary>Initializes a new instance of the CompanyAddress class.</summary>
        public CompanyAddress()
        {
        }

        #region Properties
        /// <summary>Gets an empty address</summary>
        public static CompanyAddress Empty
        {
            get
            {
                return new CompanyAddress
                {
                    Id = -1,
                    Address = string.Empty,
                    City = string.Empty,
                    Company = Company.EmptySimple,
                    Country = string.Empty,
                    Email = string.Empty,
                    Fax = string.Empty
                };
            }
        }

        /// <summary>Gets a JSON structure of address</summary>
        public string Json
        {
            get
            {
                string pattern = @"
                        {{
                            ""Id"":{0},
                            ""CompanId"":{1},
                            ""Address"":""{2}"",
                            ""PostalCode"":""{3}"",
                            ""City"":""{4}"",
                            ""Province"":""{5}"",
                            ""Country"":""{6}"",
                            ""Phone"":""{7}"",
                            ""Mobile"":""{8}"",
                            ""Fax"":""{9}"",
                            ""Email"":""{10}""
                        }}";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Company.Id,
                    this.Address,
                    this.PostalCode,
                    this.City,
                    this.Province,
                    this.Country,
                    this.Phone,
                    this.Mobile,
                    this.Fax,
                    this.Email);
            }
        }

        /// <summary>Gets the HTML code for a option tag in select address</summary>
        public string SelectOption
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "<option value=\"{0}\" {3}>{1}, {2}</option>",
                    this.Id,
                    this.Address,
                    this.City,
                    this.Id == this.Company.DefaultAddress.Id ? "selected=\"selected\"" : string.Empty);
            }
        }

        /// <summary>Gets or sets the identitifer of company's address</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the company of address</summary>
        public Company Company { get; set; }

        /// <summary>Gets or sets the street address of company's address</summary>
        public string Address { get; set; }

        /// <summary>Gets or sets the postal code of company's address</summary>
        public string PostalCode { get; set; }

        /// <summary>Gets or sets the city of company's address</summary>
        public string City { get; set; }

        /// <summary>Gets or sets the province of company's address</summary>
        public string Province { get; set; }

        /// <summary>Gets or sets the country of company's address</summary>
        public string Country { get; set; }

        /// <summary>Gets or sets the phone of company's address</summary>
        public string Phone { get; set; }

        /// <summary>Gets or sets the mobile of company's address</summary>
        public string Mobile { get; set; }

        /// <summary>Gets or sets the fax of company's address</summary>
        public string Fax { get; set; }

        /// <summary>Gets or sets the email of company's address</summary>
        public string Email { get; set; }

        /// <summary>Gets or sets the notes of company's address</summary>
        public string Notes { get; set; }

        /// <summary>Gets or sets a value indicating whether if this address is the default address of company</summary>
        public bool DefaultAddress { get; set; }

        /// <summary>Gets a text with the complete address components</summary>
        public string Description
        {
            get
            {
                var res = new StringBuilder();
                bool first = true;
                if (!string.IsNullOrEmpty(this.Address))
                {
                    res.Append(this.Address);
                    first = false;
                }

                if (!string.IsNullOrEmpty(this.City))
                {
                    if (!first)
                    {
                        res.Append(", ");
                    }

                    res.Append(this.City);
                    first = false;
                }

                if (!string.IsNullOrEmpty(this.PostalCode))
                {
                    if (!first)
                    {
                        res.Append(", ");
                    }

                    res.Append(this.PostalCode);
                    first = false;
                }

                if (!string.IsNullOrEmpty(this.Province))
                {
                    if (!first)
                    {
                        res.Append(", ");
                    }

                    res.Append(this.Province);
                    first = false;
                }

                if (!string.IsNullOrEmpty(this.Country))
                {
                    if (!first)
                    {
                        res.Append(", ");
                    }

                    res.Append(this.Country);
                    first = false;
                }

                return res.ToString();
            }
        }
        #endregion

        /// <summary>Gets a descriptive text with the differences between tow company address</summary>
        /// <param name="address1">First address</param>
        /// <param name="address2">Second address</param>
        /// <returns>Descriptive text with the differences between tow company address</returns>
        public static string Differences(CompanyAddress address1, CompanyAddress address2)
        {
            if (address1 == null || address2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            if (address1.Address != address2.Address)
            {
                res.Append("Address:").Append(address2.Address).Append(", ");
            }

            if (address1.PostalCode != address2.PostalCode)
            {
                res.Append("PostalCode:").Append(address2.PostalCode).Append(", ");
            }

            if (address1.City != address2.City)
            {
                res.Append("City:").Append(address2.City).Append(", ");
            }

            if (address1.Province != address2.Province)
            {
                res.Append("Province:").Append(address2.Province).Append(", ");
            }

            if (address1.Country != address2.Country)
            {
                res.Append("Country:").Append(address2.Country).Append(", ");
            }

            if (address1.Phone != address2.Phone)
            {
                res.Append("Phone:").Append(address2.Phone).Append(", ");
            }

            if (address1.Mobile != address2.Mobile)
            {
                res.Append("Mobile:").Append(address2.Mobile).Append(";");
            }

            if (address1.Fax != address2.Fax)
            {
                res.Append("Fax:").Append(address2.Fax).Append(", ");
            }

            if (address1.Email != address2.Email)
            {
                res.Append("Email:").Append(address2.Email).Append(", ");
            }

            return res.ToString();
        }

        /// <summary>Delete an address in data base</summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="addressId">Address identifier</param>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public static ActionResult Delete(int companyId, int addressId, int userId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Company_DeleteAddress"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                        cmd.Parameters.Add("@AddressId", SqlDbType.Int);
                        cmd.Parameters.Add("@UserId", SqlDbType.Int);
                        cmd.Parameters["@CompanyId"].Value = companyId;
                        cmd.Parameters["@AddressId"].Value = addressId;
                        cmd.Parameters["@UserId"].Value = userId;
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "CompanyAddress::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "CompanyAddress::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "CompanyAddress::Delete", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
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

        /// <summary>Obtain all address of a company</summary>
        /// <param name="company">Compnay to search in</param>
        /// <returns>List of address</returns>
        public static ReadOnlyCollection<CompanyAddress> GetAddressByCompanyId(Company company)
        {
            if (company == null)
            {
                return new ReadOnlyCollection<CompanyAddress>(new List<CompanyAddress>());                
            }

            string source = string.Format(
                CultureInfo.InvariantCulture,
                "CompanyAddress::GetAddressByCompanyId({0})",
                company.Id);

            var res = new List<CompanyAddress>();
            using (var cmd = new SqlCommand("Company_GetAdress"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = company.Id;
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new CompanyAddress
                                {
                                    Id = rdr.GetInt32(ColumnsCompanyAddressGet.Id),
                                    Company = company,
                                    Address = rdr.GetString(ColumnsCompanyAddressGet.Address),
                                    PostalCode = rdr.GetString(ColumnsCompanyAddressGet.PostalCode),
                                    City = rdr.GetString(ColumnsCompanyAddressGet.City),
                                    Province = rdr.GetString(ColumnsCompanyAddressGet.Province),
                                    Country = rdr.GetString(ColumnsCompanyAddressGet.Country),
                                    Phone = rdr.GetString(ColumnsCompanyAddressGet.Phone),
                                    Mobile = rdr.GetString(ColumnsCompanyAddressGet.Mobile),
                                    Fax = rdr.GetString(ColumnsCompanyAddressGet.Fax),
                                    Email = rdr.GetString(ColumnsCompanyAddressGet.Email),
                                    Notes = rdr.GetString(ColumnsCompanyAddressGet.Notes),
                                    DefaultAddress = rdr.GetInt32(ColumnsCompanyAddressGet.DefaultAddress) == 1
                                });
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, source);
                    }
                    catch (InvalidCastException ex)
                    {
                        ExceptionManager.Trace(ex, source);
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

            return new ReadOnlyCollection<CompanyAddress>(res);
        }

        /// <summary>Insert an address into data base</summary>
        /// <param name="address">Address to insert</param>
        /// <param name="userId">Identifier of user that perfomrs the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult Insert(CompanyAddress address, int userId)
        {
            var res = ActionResult.NoAction;
            if (address == null)
            {
                return res;
            }

            /* CREATE PROCEDURE CompanyAddress_Insert 
             * @CompanyAddressId int out,
             * @CompanyId int,
             * @Address nvarchar(50),
             * @PostalCode nvarchar(10),
             * @City nvarchar(50),
             * @Province nvarchar(50),
             * @Country nvarchar(15),
             * @Phone nvarchar(15),
             * @Mobile nvarchar(15),
             * @Email nvarchar(10),
             * @Notes text,
             * @Fax nvarchar(15),
             * @UserId int*/

            using (var cmd = new SqlCommand("CompanyAddress_Insert"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        int companyId = 0;
                        if (address.Company != null)
                        {
                            companyId = address.Company.Id;
                        }

                        cmd.Parameters.Add(DataParameter.OutputInt("@CompanyAddressId"));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@Address", address.Address, 100));
                        cmd.Parameters.Add(DataParameter.Input("@PostalCode", address.PostalCode, 10));
                        cmd.Parameters.Add(DataParameter.Input("@City", address.City, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Province", address.Province, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Country", address.Country, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", address.Phone, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", address.Mobile, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Email", address.Email, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Notes", address.Notes));
                        cmd.Parameters.Add(DataParameter.Input("@Fax", address.Fax, 15));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.Success = true;
                        res.MessageError = cmd.Parameters["@CompanyAddressId"].Value.ToString();
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

        /// <summary>Render the HTML code for a row to address table</summary>
        /// <param name="dictionary">Dictionary for fixed labels</param>
        /// <returns>HTML code</returns>
        public string ListRow(Dictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                return string.Empty;
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                @"<tr id=""Address{0:##0}""><td style=""font-weight:{2}"">{1}{3}</td><td align=""center"">{4}</td><td align=""center""><div style=""visibility:{7};""><span class=""btn btn-xs btn-success"" onclick=""SetDefaultAddress({0});"" title=""{5}""><i class=""icon-star bigger-120""></i></span>&nbsp;<span class=""btn btn-xs btn-danger"" onclick=""CompanyAddressDelete({0});"" title=""{6}""><i class=""icon-trash bigger-120""></i></span></div></td></tr>", 
                this.Id,
                this.Description, 
                this.DefaultAddress ? "bold" : "normal", 
                this.DefaultAddress ? "</strong>" : string.Empty, 
                this.DefaultAddress ? dictionary["Common_Yes"] : dictionary["Common_No"], 
                dictionary["Item_CompanyAddress_ToolTip_SetAsPrincipal"], 
                dictionary["Common_Delete"], 
                this.DefaultAddress ? "hidden" : "visible");
        }

        /// <summary>Update an address in data base</summary>
        /// <param name="userId">Identifier of user that performs the actions</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE CompanyAddress_Update
             * @CompanyAddressId int,
             * @CompanyId int,
             * @Address nvarchar(50),
             * @PostalCode nvarchar(10),
             * @City nvarchar(50),
             * @Province nvarchar(50),
             * @Country nvarchar(15),
             * @Phone nvarchar(15),
             * @Mobile nvarchar(15),
             * @Email nvarchar(15),
             * @Fax nvarchar(15),
             * @User int */
            using (var cmd = new SqlCommand("CompanyAddress_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyAddressId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Company.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Address", this.Address, 100));
                        cmd.Parameters.Add(DataParameter.Input("@PostalCode", this.PostalCode, 10));
                        cmd.Parameters.Add(DataParameter.Input("@City", this.City, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Province", this.Province, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Country", this.Country));
                        cmd.Parameters.Add(DataParameter.Input("@Phone", this.Phone, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Mobile", this.Mobile, 15));
                        cmd.Parameters.Add(DataParameter.Input("@Email", this.Email, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Fax", this.Fax, 15));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.Success = true;
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