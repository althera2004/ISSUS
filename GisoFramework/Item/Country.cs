// --------------------------------
// <copyright file="Country.cs" company="Sbrinna">
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
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;

    /// <summary>Implements Country class</summary>
    public class Country
    {
        /// <summary> Gets or sets the identifier of country</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the description of country</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets a value indicating whether if country is selected in company</summary>
        public bool Selected { get; set; }

        /// <summary>Gets or sets a value indicating whether if country can be delete</summary>
        public bool CanBeDelete { get; set; }

        /// <summary>Gets the HTML code for a selected country tag</summary>
        public string SelectedTag
        {
            get
            {
                string pattern = @"
                    <div class=""col-sm-3"" id=""CT{0}"">
                        <input type=""checkbox"" id=""{0}"" onclick=""{2}(this);"" />
                        <img src=""assets/flags/{0}.png"" />
                        <span>{1}</span>
                    </div>";

                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Description,
                    this.CanBeDelete ? "SelectCountryDelete" : "SelectCountryNoDelete");
            }
        }

        /// <summary>Gets the HTML code for a available country tag</summary>
        public string AvailableTag
        {
            get
            {
                string pattern = @"<div class=""col-sm-3"" id=""CT{0}""><input type=""checkbox"" id=""{0}"" onclick=""SelectCountry(this);"" /><img src=""assets/flags/{0}.png"" /><span id=""name"">{1}</span></div>";
                return string.Format(
                    CultureInfo.InvariantCulture,
                    pattern,
                    this.Id,
                    this.Description);
            }
        }

        /// <summary>Add a country to compnay's selected countries</summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult CompanyAddCountry(int countryId, int companyId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Company_SetCountry"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CountryId", countryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Discard a country of company's selected countries</summary>
        /// <param name="countryId">Country identifier</param>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Result of action</returns>
        public static ActionResult CompanyDiscardCountry(int countryId, int companyId)
        {
            var res = ActionResult.NoAction;
            using (var cmd = new SqlCommand("Company_UnSetCountry"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CountryId", countryId));
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
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

        /// <summary>Obtain all countries of the company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>A list of countries</returns>
        public static ReadOnlyCollection<Country> GetAll(int companyId)
        {
            var res = new List<Country>();
            using (var cmd = new SqlCommand("Countries_GetAll"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new Country()
                                {
                                    Id = rdr.GetInt32(0),
                                    Description = rdr.GetString(1),
                                    Selected = rdr.GetInt32(2) == 1,
                                    CanBeDelete = rdr.GetInt32(3) == 1
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

            return new ReadOnlyCollection<Country>(res);
        }
    }
}