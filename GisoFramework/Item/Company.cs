// --------------------------------
// <copyright file="Company.cs" company="Sbrinna">
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
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using GisoFramework.Activity;
    using GisoFramework.DataAccess;
    using GisoFramework.Item.Binding;
    
    /// <summary>Implements Company class</summary>
    public class Company
    {
        /// <summary> Company departments </summary>
        private Collection<Department> departments;

        /// <summary>Company's employees</summary>
        private Collection<Employee> employees;

        /// <summary>List of company's addresses</summary>
        private Collection<CompanyAddress> addresses;

        /// <summary>Compnay's default address</summary>
        private CompanyAddress defaultAddress;

        /// <summary> Selected countries for the company </summary>
        private List<Country> countries;

        /// <summary>Initializes a new instance of the Company class</summary>
        public Company()
        {
        }

        /// <summary>Initializes a new instance of the Company class.
        /// Company data is searched on database based in company identifier
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        public Company(int companyId)
        {
            this.Id = -1;
            this.MailContact = string.Empty;
            this.Web = string.Empty;
            this.SubscriptionEnd = DateTime.Now;
            this.SubscriptionStart = DateTime.Now;
            this.Name = string.Empty;
            this.Language = "es";
            this.DiskQuote = 0;

            string source = string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId);
            using (var cmd = new SqlCommand("Company_GetById"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyId", SqlDbType.Int);
                    cmd.Parameters["@CompanyId"].Value = companyId;

                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                rdr.Read();
                                this.Id = rdr.GetInt32(0);
                                this.Name = rdr[1].ToString();
                                this.MailContact = string.Empty;
                                this.Web = string.Empty;
                                this.SubscriptionStart = rdr.GetDateTime(2);
                                this.SubscriptionEnd = rdr.GetDateTime(3);
                                this.Language = Convert.ToString(rdr[4], CultureInfo.InvariantCulture);
                                this.FiscalNumber = rdr[5].ToString();
                                this.Code = rdr[6].ToString();
                                this.DiskQuote = rdr.GetInt64(8);
                                this.Agreement = rdr.GetBoolean(9);
                            }

                            this.departments = Company.GetDepartments(this.Id);
                            this.addresses = CompanyAddress.GetAddressByCompanyId(this);
                            foreach (var address in this.addresses)
                            {
                                if (address.DefaultAddress)
                                {
                                    this.defaultAddress = address;
                                    break;
                                }
                            }

                            this.ObtainEmployees();
                        }
                    }
                    catch (SqlException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        this.Id = -1;
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionEnd = DateTime.Now;
                        this.SubscriptionStart = DateTime.Now;
                        this.Name = string.Empty;
                    }
                    catch (FormatException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        this.Id = -1;
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionEnd = DateTime.Now;
                        this.SubscriptionStart = DateTime.Now;
                        this.Name = string.Empty;
                    }
                    catch (NullReferenceException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InstalledUICulture, "cto::Company({0})", companyId));
                        this.Id = -1;
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionEnd = DateTime.Now;
                        this.SubscriptionStart = DateTime.Now;
                        this.Name = string.Empty;
                    }
                    catch (ArgumentNullException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
                        this.Id = -1;
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionEnd = DateTime.Now;
                        this.SubscriptionStart = DateTime.Now;
                        this.Name = string.Empty;
                    }
                    catch (ArgumentException ex)
                    {
                        ExceptionManager.Trace(ex, string.Format(CultureInfo.InvariantCulture, "cto::Company({0})", companyId));
                        this.Id = -1;
                        this.MailContact = string.Empty;
                        this.Web = string.Empty;
                        this.SubscriptionEnd = DateTime.Now;
                        this.SubscriptionStart = DateTime.Now;
                        this.Name = string.Empty;
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

            this.countries = new List<Country>();
            using (var cmdCountries = new SqlCommand("Company_GetCountries"))
            {
                using (var ccCountry = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ToString()))
                {
                    cmdCountries.Connection = ccCountry;
                    cmdCountries.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmdCountries.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmdCountries.Connection.Open();
                        using (var rdrCountries = cmdCountries.ExecuteReader())
                        {
                            while (rdrCountries.Read())
                            {
                                this.countries.Add(new Country() { Id = rdrCountries.GetInt32(0), Description = rdrCountries.GetString(1) });
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
                    finally
                    {
                        if (cmdCountries.Connection.State != ConnectionState.Closed)
                        {
                            cmdCountries.Connection.Close();
                        }
                    }
                }
            }
        }
        
        /// <summary>Gets an empty company</summary>
        public static Company Empty
        {
            get
            {
                return new Company
                {
                    Id = -1,
                    addresses = new Collection<CompanyAddress>(),
                    Code = string.Empty,
                    defaultAddress = CompanyAddress.Empty,
                    departments = new Collection<Department>(),
                    employees = new Collection<Employee>(),
                    Language = string.Empty,
                    MailContact = string.Empty,
                    Name = string.Empty,
                    FiscalNumber = string.Empty,
                    Web = string.Empty
                };
            }
        }

        /// <summary>Gets an empty company</summary>
        public static Company EmptySimple
        {
            get
            {
                return new Company
                {
                    Id = -1,
                    Code = string.Empty,
                    departments = new Collection<Department>(),
                    employees = new Collection<Employee>(),
                    Language = string.Empty,
                    MailContact = string.Empty,
                    Name = string.Empty,
                    FiscalNumber = string.Empty,
                    Web = string.Empty
                };
            }
        }

        /// <summary>Gets or sets company's disk quote</summary>
        public long DiskQuote { get; set; }

        /// <summary>Gets or sets a valute indicating whether agreement document is accepted</summary>
        public bool Agreement { get; set; }

        #region Properties
        /// <summary>Gets a JSON key/value stucture of the company</summary>
        public string JsonKeyValue
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture, @"{{""Id"":{0},""Value"":""{1}""}}",
                    this.Id, 
                    Tools.JsonCompliant(this.Name));
            }
        }

        /// <summary>Gets or sets the company's logo</summary>
        public string Logo { get; set; }

        /// <summary>Gets a list of countries available for the company</summary>
        public ReadOnlyCollection<Country> Countries
        {
            get
            {
                if (this.countries == null)
                {
                    this.countries = new List<Country>();
                }

                return new ReadOnlyCollection<Country>(this.countries);
            }
        }

        /// <summary>Gets or sets the company identifier</summary>
        public int Id { get; set; }

        /// <summary>Gets or sets the code of company</summary>
        public string Code { get; set; }

        /// <summary>Gets or sets the name of company</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the date of starting subscription</summary>
        public DateTime SubscriptionStart { get; set; }

        /// <summary>Gets or sets the date of finishing subscription</summary>
        public DateTime SubscriptionEnd { get; set; }

        /// <summary>Gets or sets de email contacto of company</summary>
        public string MailContact { get; set; }

        /// <summary>Gets or sets de web address of company</summary>
        public string Web { get; set; }

        /// <summary>Gets or sets the default language of company</summary>
        public string Language { get; set; }

        /// <summary>Gets or sets the NIF of company</summary>
        public string FiscalNumber { get; set; }

        /// <summary>Gets a list of compnay's departments</summary>
        public ReadOnlyCollection<Department> Departments
        {
            get
            {
                return new ReadOnlyCollection<Department>(this.departments);
            }
        }

        /// <summary>Gets a list of company's employees</summary>
        public ReadOnlyCollection<Employee> Employees
        {
            get
            {
                return new ReadOnlyCollection<Employee>(this.employees);
            }
        }

        /// <summary>Gets a list of company's employees with user associated</summary>
        public ReadOnlyCollection<Employee> EmployessWithUser
        {
            get
            {
                return new ReadOnlyCollection<Employee>(this.employees.Where(e => e.HasUserAssigned == true && e.Active == true && e.DisabledDate == null).ToList());
            }
        }

        /// <summary>Gets a list of company's employees without user associated</summary>
        public ReadOnlyCollection<Employee> EmployessWithoutUser
        {
            get
            {
                return new ReadOnlyCollection<Employee>(this.employees.Where(e => e.HasUserAssigned == false && e.Active == true && e.DisabledDate == null).ToList());
            }
        }

        /// <summary>Gets a list of company's addresses</summary>
        public ReadOnlyCollection<CompanyAddress> Addresses
        {
            get
            {
                return new ReadOnlyCollection<CompanyAddress>(this.addresses);
            }
        }

        /// <summary>Gets or sets the default address of company</summary>
        public CompanyAddress DefaultAddress
        {
            get
            {
                return this.defaultAddress;
            }

            set
            {
                this.defaultAddress = value;
            }
        }
        #endregion

        /// <summary>Obtain the departments of a company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>List of departments</returns>
        public static Collection<Department> GetDepartments(int companyId)
        {
            string source = string.Format(CultureInfo.InstalledUICulture, "GetDepartments({0})", companyId);
            var res = new Collection<Department>();
            using (var cmd = new SqlCommand("Company_GetDepartments"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                    try
                    {
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                res.Add(new Department()
                                {
                                    CompanyId = companyId,
                                    Id = rdr.GetInt32(0),
                                    Description = rdr.GetString(1)
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

        /// <summary>Gets a descriptive text with the differences between two companies</summary>
        /// <param name="item1">First company to compare</param>
        /// <param name="item2">Second company to capmpare</param>
        /// <returns>The description of differences between two companies</returns>
        public static string Differences(Company item1, Company item2)
        {
            if (item1 == null || item2 == null)
            {
                return string.Empty;
            }

            var res = new StringBuilder();
            bool first = true;

            if (item1.Name != item2.Name)
            {
                res.Append("Name:").Append(item2.Name);
                first = false;
            }

            if (item1.FiscalNumber != item2.FiscalNumber)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Nif:").Append(item2.FiscalNumber);
                first = false;
            }

            if (item1.Language != item2.Language)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("Language:").Append(item2.Language);
                first = false;
            }

            if (item1.defaultAddress.Id != item2.defaultAddress.Id)
            {
                if (!first)
                {
                    res.Append(",");
                }

                res.Append("defaultAddress:").Append(item2.defaultAddress.Address).Append(",").Append(item2.defaultAddress.City);
            }

            return res.ToString();
        }

        /// <summary>Set the default address of a company</summary>
        /// <param name="companyId">Compnay identifier</param>
        /// <param name="addressId">Address identifier</param>
        /// <param name="userId">Identifier of user that peforms the action</param>
        /// <returns>Result of action</returns>
        public static ActionResult SetDefaultAddress(int companyId, int addressId, int userId)
        {
            var res = ActionResult.NoAction;
            /* ALTER PROCEDURE [dbo].[Company_SetDefaultAddress]
             * @CompanyId int,
             * @AddressId int,
             * @UserId int */
            using (var cmd = new SqlCommand("Company_SetDefaultAddress"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", companyId));
                        cmd.Parameters.Add(DataParameter.Input("@AddressId", addressId));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.SetSuccess();
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
                    }
                    catch (NullReferenceException ex)
                    {
                        res.SetFail(ex);
                        ExceptionManager.Trace(ex, "Company::SetDefaultAddress", string.Format(CultureInfo.GetCultureInfo("en-us"), "CompanyId:{0},AddressId{1},UserId{2}", companyId, addressId, userId));
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

        /// <summary>Generates a JSON strcuture of company</summary>
        /// <param name="company">Compnay to extract data</param>
        /// <returns>JSON structure</returns>
        public static string Json(Company company)
        {
            if (company == null)
            {
                return "{}";
            }

            var res = new StringBuilder("{").Append(Environment.NewLine);
            res.Append("\t\t\"Id\":").Append(company.Id).Append(",").Append(Environment.NewLine);
            res.Append("\t\t\"Name\":\"").Append(company.Name).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"Nif\":\"").Append(company.FiscalNumber).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"MailContact\":\"").Append(company.MailContact).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"Web\":\"").Append(company.Web).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"SubscriptionStart\":\"").Append(company.SubscriptionStart.ToShortDateString()).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"SubscriptionEnd\":\"").Append(company.SubscriptionEnd.ToShortDateString()).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"Language\":\"").Append(company.Language).Append("\",").Append(Environment.NewLine);
            res.Append("\t\t\"Departments\":").Append(Environment.NewLine);
            res.Append("\t\t[");
            bool firstDepartment = true;
            foreach (var department in company.departments)
            {
                if (firstDepartment)
                {
                    firstDepartment = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(Environment.NewLine).Append("\t\t\t").Append(department.Json);
            }

            res.Append(Environment.NewLine).Append("\t\t],").Append(Environment.NewLine);
            res.Append("\t\t\"Employees\":").Append(Environment.NewLine);
            res.Append("\t\t[");
            bool firstEmployee = true;

            company.ObtainEmployees();
            foreach (var employee in company.employees)
            {
                if (firstEmployee)
                {
                    firstEmployee = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(employee.Json);
            }

            res.Append(Environment.NewLine).Append("\t\t]");

            if (company.defaultAddress != null)
            {
                res.Append(",").Append(Environment.NewLine).Append("\t\t\"DefaultAddress\":").Append(company.defaultAddress.Json);
            }

            bool firstCountry = true;
            res.Append(",").Append(Environment.NewLine).Append("\t\t\"Countries\":").Append(Environment.NewLine).Append("\t\t[");
            foreach (var country in company.Countries)
            {
                if (firstCountry)
                {
                    firstCountry = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(string.Format(CultureInfo.GetCultureInfo("en-us"), @"{2}                      {{""Id"":{0}, ""Name"":""{1}""}}", country.Id, Tools.JsonCompliant(country.Description), Environment.NewLine));
            }

            res.Append(Environment.NewLine).Append("\t\t]");
            res.Append(Environment.NewLine).Append("\t}");
            return res.ToString();
        }

        /// <summary>Get a compnay from data base by code</summary>
        /// <param name="code">Company's code</param>
        /// <returns>Company object</returns>
        public static int GetByCode(string code)
        {
            int res = 0;
            using (var cmd = new SqlCommand("Company_GetByCode"))
            {
                cmd.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.Text);
                cmd.Parameters["@CompanyCode"].Value = code;
                try
                {
                    cmd.Connection.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            res = rdr.GetInt32(0);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, "Company::GetByCode", code);
                    res = 0;
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

        /// <summary>Gets log of company</summary>
        /// <param name="companyId">Company identifier</param>
        /// <returns>Filename of company's logo</returns>
        public static string GetLogoFileName(int companyId)
        {
            string res = "NoImage.jpg";
            string path = HttpContext.Current.Request.PhysicalApplicationPath;
            if (!path.EndsWith("\\"))
            {
                path += string.Format(CultureInfo.InvariantCulture, @"\", path);
            }

            path = string.Format(CultureInfo.InvariantCulture, @"{0}\images\Logos\", path);
            string pattern = string.Format(CultureInfo.InvariantCulture, "{0}.*", companyId);
            var last = new DateTime(1900, 1, 1);
            var files = Directory.GetFiles(path, pattern);
            foreach (string file in files)
            {
                var info = new FileInfo(file);
                var created = info.LastWriteTime;
                if (created > last)
                {
                    last = created;
                    res = file;
                }
            }

            res = Path.GetFileName(res);
            return res;
        }

        /// <summary>Avoid company addresses</summary>
        public void AvoidAddress()
        {
            this.addresses = new Collection<CompanyAddress>(new List<CompanyAddress>());
        }

        /// <summary>Add an address in company addresses</summary>
        /// <param name="address">Address to add</param>
        public void AddAddress(CompanyAddress address)
        {
            if (this.addresses == null)
            {
                this.addresses = new Collection<CompanyAddress>(new List<CompanyAddress>());
            }

            this.addresses.Add(address);
        }

        /// <summary>Update company in data base</summary>
        /// <param name="userId">Identifier of user that performs the action</param>
        /// <returns>Result of action</returns>
        public ActionResult Update(int userId)
        {
            var res = ActionResult.NoAction;
            /* CREATE PROCEDURE Company_Update
             *   @CompanyId int,
             *   @Name nvarchar(50),
             *   @Nif nvarchar(15),
             *   @DefaultAddress int,
             *   @Language nvarchar(2),
             *   @UserId int */
            using (var cmd = new SqlCommand("Company_Update"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Name", this.Name, 50));
                        cmd.Parameters.Add(DataParameter.Input("@Nif", this.FiscalNumber, 15));
                        cmd.Parameters.Add(DataParameter.Input("@DefaultAddress", this.defaultAddress.Id));
                        cmd.Parameters.Add(DataParameter.Input("@Language", this.Language, 2));
                        cmd.Parameters.Add(DataParameter.Input("@UserId", userId));
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        res.Success = true;
                        res.MessageError = string.Empty;
                    }
                    catch (SqlException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (FormatException ex)
                    {
                        res.SetFail(ex);
                    }
                    catch (NullReferenceException ex)
                    {
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
            }

            return res;
        }

        /// <summary>Obtain the employees of company</summary>
        public void ObtainEmployees()
        {
            var source = string.Format(CultureInfo.InvariantCulture, "Company::ObtainEmployees() . CompanyId:{0}", this.Id);
            this.employees = new Collection<Employee>();
            using (var cmd = new SqlCommand("Company_GetEmployees"))
            {
                using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString))
                {
                    cmd.Connection = cnn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.Add(DataParameter.Input("@CompanyId", this.Id));
                        cmd.Connection.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var newEmployee = new Employee()
                                {
                                    Id = rdr.GetInt32(ColumnsCompanyGetEmployees.Id),
                                    CompanyId = this.Id,
                                    Name = rdr.GetString(ColumnsCompanyGetEmployees.Name),
                                    LastName = rdr.GetString(ColumnsCompanyGetEmployees.LastName),
                                    Active = rdr.GetBoolean(ColumnsCompanyGetEmployees.Active),
                                    Nif = rdr.GetString(ColumnsCompanyGetEmployees.Nif),
                                    Email = rdr.GetString(ColumnsCompanyGetEmployees.Email),
                                    Phone = rdr.GetString(ColumnsCompanyGetEmployees.Phone),
                                    Address = new EmployeeAddress()
                                    {
                                        Address = rdr.GetString(ColumnsCompanyGetEmployees.Address),
                                        PostalCode = rdr.GetString(ColumnsCompanyGetEmployees.PostalCode),
                                        City = rdr.GetString(ColumnsCompanyGetEmployees.City),
                                        Province = rdr.GetString(ColumnsCompanyGetEmployees.Province),
                                        Country = rdr.GetString(ColumnsCompanyGetEmployees.Country)
                                    },
                                    HasUserAssigned = rdr.GetInt32(ColumnsCompanyGetEmployees.HasUserAssigned) == 1
                                };

                                if (!rdr.IsDBNull(ColumnsCompanyGetEmployees.EndDate))
                                {
                                    newEmployee.DisabledDate = rdr.GetDateTime(ColumnsCompanyGetEmployees.EndDate);
                                }

                                this.employees.Add(newEmployee);
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
                    finally
                    {
                        if (cmd.Connection.State != ConnectionState.Closed)
                        {
                            cmd.Connection.Close();
                        }
                    }
                }
            }
        }
    }
}