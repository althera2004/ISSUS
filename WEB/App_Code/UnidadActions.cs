// --------------------------------
// <copyright file="UnidadActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón -  jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for UnidadActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class UnidadActions : WebService {

    public UnidadActions ()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(string description, int companyId, int userId)
    {
        var res = new Unidad { Description = description, CompanyId = companyId }.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(int unidadId, string description, int companyId, int userId)
    {
        var res = new Unidad { Id = unidadId, Description = description, CompanyId = companyId }.Update(userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int unidadId, int companyId, int userId)
    {
        var res = Unidad.Inactivate(unidadId, companyId, userId);
        if (res.Success)
        {
            var companySession = new Company(companyId);
            HttpContext.Current.Session["Company"] = companySession;
        }

        return res;
    }
}