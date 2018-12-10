// --------------------------------
// <copyright file="CustomerActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for CustomerActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class CustomerActions : WebService
{
    public CustomerActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(string description, int companyId, int userId)
    {
        var res = new Customer { Description = description, CompanyId = companyId }.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(Customer oldCustomer, Customer newCustomer, int userId)
    {
        var res = newCustomer.Update(userId);
        if (res.Success)
        {
            string extraData = newCustomer.Differences(oldCustomer);
            if (!string.IsNullOrEmpty(extraData))
            {
                res = ActivityLog.Customer(Convert.ToInt32(newCustomer.Id), userId, newCustomer.CompanyId, CustomerLogActions.Update, extraData);
            }

            HttpContext.Current.Session["Company"] = new Company(newCustomer.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int customerId, int companyId, int userId)
    {
        var res = new Customer { Id = customerId, CompanyId = companyId }.Delete(userId);
        if (res.Success)
        {
            HttpContext.Current.Session["Company"] = new Company(companyId);
        }

        return res;
    }
}