// --------------------------------
// <copyright file="EquipmentScaleDivisionActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentScaleDivision</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentScaleDivisionActions : WebService {

    public EquipmentScaleDivisionActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(string description, int companyId, int userId)
    {
        var res = new EquipmentScaleDivision
        {
            Description = description,
            CompanyId = companyId
        }.Insert(userId);

        if (res.Success)
        {
            Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(int EquipmentScaleDivisionId, string description, int companyId, int userId)
    {
        var res = new EquipmentScaleDivision
        {
            Id = EquipmentScaleDivisionId,
            Description = description,
            CompanyId = companyId
        }.Update(userId);
        if (res.Success)
        {
            HttpContext.Current.Session["Company"] = new Company(companyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int EquipmentScaleDivisionId, int companyId, int userId)
    {
        var res = EquipmentScaleDivision.Delete(EquipmentScaleDivisionId, userId, companyId);
        if (res.Success)
        {
            HttpContext.Current.Session["Company"] = new Company(companyId);
        }

        return res;
    }    
}