// --------------------------------
// <copyright file="CustomerActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for CustomerActions
/// </summary>
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
        ActionResult res = new Customer() { Description = description, CompanyId = companyId }.Insert(userId);

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
        ActionResult res = ActionResult.NoAction;                
        res = newCustomer.Update(userId);

        if (res.Success)
        {
            string extraData = newCustomer.Differences(oldCustomer);
            if (!string.IsNullOrEmpty(extraData))
            {
                res = ActivityLog.Customer(Convert.ToInt32(newCustomer.Id), userId, newCustomer.CompanyId, CustomerLogActions.Update, extraData);
            }

            Company companySession = new Company(newCustomer.CompanyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    /*
    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(long customerId, string description, int userId, int companyId)
    {
        return new Customer() { Id = customerId, Description = description, CompanyId = companyId }.Update(userId);
    }
    */

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int CustomerId, int companyId, int userId)
    {
        ActionResult res = new Customer() { Id = CustomerId, CompanyId = companyId }.Delete(userId);

        if (res.Success)
        {
            Company companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }
}

