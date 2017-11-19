// --------------------------------
// <copyright file="ProviderActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for ProviderActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class ProviderActions : WebService {

    public ProviderActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(string description, int companyId, int userId)
    {
        ActionResult res = new Provider() { Description = description, CompanyId = companyId }.Insert(userId);

        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(int providerId, string description, int companyId, int userId)
    {
        ActionResult res = new Provider() { Id = providerId, Description = description, CompanyId = companyId }.Update(userId);

        if (res.Success)
        {
            Company companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int ProviderId, int companyId, int userId)
    {
        ActionResult res = new Provider() { Id = ProviderId, CompanyId = companyId }.Delete(userId);

        if (res.Success)
        {
            Company companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }    
}
