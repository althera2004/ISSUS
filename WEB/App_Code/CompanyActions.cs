// --------------------------------
// <copyright file="CompanyActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GISOWeb
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using GisoFramework.Activity;
    using GisoFramework.Item;

    /// <summary>
    /// Summary description for CompanyActions
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class CompanyActions : WebService
    {
        public CompanyActions()
        {
        }

        [WebMethod]
        [ScriptMethod]
        public Company GetById(int companyId)
        {
            return new Company(companyId);
        }

        [WebMethod]
        [ScriptMethod]
        public ActionResult ChangeLogo(Stream x)
        {
            return ActionResult.NoAction;
            /*HttpContext postedContext = HttpContext.Current;
            //File Collection that was submitted with posted data
            HttpFileCollection Files = postedContext.Request.Files;*/
        }

        [WebMethod]
        [ScriptMethod]
        public string GetDepartments(int companyId)
        {
            var departments = new Company(companyId).Departments;
            bool first = true;
            var res = new StringBuilder("[");

            foreach (Department department in departments)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append(",");
                }

                res.Append(department.Json);
            }

            res.Append("]");
            return res.ToString();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult SelectCountries(string countries, int companyId)
        {
            var res = ActionResult.NoAction;
            var countriesList = countries.Split('|');
            string error = string.Empty;
            foreach (string id in countriesList)
            {
                if (string.IsNullOrEmpty(id))
                {
                    continue;
                }

                int countryId = Convert.ToInt32(id);
                var resTemporal = Country.CompanyAddCountry(countryId, companyId);
                if (!resTemporal.Success)
                {
                    error += resTemporal.MessageError;
                }
            }

            if (string.IsNullOrEmpty(error))
            {
                res.SetSuccess();
                var companySession = new Company(companyId);
                HttpContext.Current.Session["Company"] = companySession;
            }
            else
            {
                res.SetFail(error);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult DiscardCountries(string countries, int companyId)
        {
            var res = ActionResult.NoAction;
            var countriesList = countries.Split('|');
            string error = string.Empty;
            foreach (string id in countriesList)
            {
                if (string.IsNullOrEmpty(id))
                {
                    continue;
                }
                int countryId = Convert.ToInt32(id);
                var resTemporal = Country.CompanyDiscardCountry(countryId, companyId);
                if (!resTemporal.Success)
                {
                    error += resTemporal.MessageError;
                }

            }

            if (string.IsNullOrEmpty(error))
            {
                res.SetSuccess();
                var companySession = new Company(companyId);
                HttpContext.Current.Session["Company"] = companySession;
            }
            else
            {
                res.SetFail(error);
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult Save(Company oldCompany, Company newCompany, int userId)
        {
            var res = ActionResult.NoAction;
            string extradata = oldCompany.Differences(newCompany);
            if (!string.IsNullOrEmpty(extradata))
            {
                res = newCompany.Update(userId);
                if (res.Success)
                {
                    res = ActivityLog.Company(newCompany.Id, userId, newCompany.Id, CompanyLogActions.Update, extradata);
                    Session["Company"] = new Company(newCompany.Id);
                }
            }

            if (res.MessageError == "No action")
            {
                res.SetSuccess();
            }

            if (res.Success)
            {
                var companySession = new Company(newCompany.Id);
                HttpContext.Current.Session["Company"] = companySession;
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult SaveAddress(CompanyAddress address, int userId)
        {
            var res = CompanyAddress.Insert(address, userId);
            int addressId = -1;
            if (res.Success)
            {
                addressId = Convert.ToInt32(res.MessageError);
                res = Company.SetDefaultAddress(address.Company.Id, addressId, userId);
                if (res.Success)
                {
                    res = ActivityLog.Company(address.Company.Id, userId, address.Company.Id, CompanyLogActions.NewCompanyAddress, string.Empty);
                    if (res.Success)
                    {
                        res.MessageError = addressId.ToString();
                        Session["Company"] = new Company(address.Company.Id);
                    }
                }
            }

            if (res.Success)
            {
                var companySession = new Company(address.Company.Id);
                HttpContext.Current.Session["Company"] = companySession;
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult UpdateAddress(CompanyAddress oldAddress, CompanyAddress newAddress, int userId)
        {
            var res = ActionResult.NoAction;
            string extradata = CompanyAddress.Differences(oldAddress, newAddress);
            if (!string.IsNullOrEmpty(extradata))
            {
                res = newAddress.Update(userId);
                if (res.Success)
                {
                    res = ActivityLog.Company(newAddress.Company.Id, userId, newAddress.Company.Id, CompanyLogActions.Update, extradata);
                }
            }
            else
            {
                res.SetSuccess();
            }

            if (res.Success)
            {
                var companySession = new Company(newAddress.Company.Id);
                HttpContext.Current.Session["Company"] = companySession;
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult SetDefaultAddress(int companyId, int addressId, int userId)
        {
            var res = Company.SetDefaultAddress(companyId, addressId, userId);
            if (res.Success)
            {
                var company = (Company)Session["company"];
                foreach (CompanyAddress address in company.Addresses)
                {
                    if (address.Id == addressId)
                    {
                        company.DefaultAddress = address;
                        break;
                    }
                }

                if (res.Success)
                {
                    var companySession = new Company(companyId);
                    HttpContext.Current.Session["Company"] = companySession;
                }

                res = ActivityLog.Company(companyId, userId, companyId, CompanyLogActions.SetDefaultAddress, string.Format("CompanyId:{0},AddressId:{1},UserId:{2}", companyId, addressId, userId));
                res.SetSuccess(addressId.ToString(CultureInfo.GetCultureInfo("en-us")));
            }

            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult DeleteAddress(int companyId, int addressId, int userId)
        {
            var res = CompanyAddress.Delete(companyId, addressId, userId);
            if (res.Success)
            {
                var companySession = new Company(companyId);
                HttpContext.Current.Session["Company"] = companySession;
            }

            return res;
        }
    }
}