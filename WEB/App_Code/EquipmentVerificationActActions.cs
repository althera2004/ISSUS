// --------------------------------
// <copyright file="EquipmentVerificationActActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentVerificationAct</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentVerificationActActions : WebService
{
    public EquipmentVerificationActActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentVerificationAct equipmentVerificationAct, int userId)
    {
        var res = equipmentVerificationAct.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentVerificationAct.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentVerificationAct equipmentVerificationAct, int userId)
    {
        return equipmentVerificationAct.Update(string.Empty, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentVerificationActId, int companyId, int userId)
    {
        return EquipmentVerificationAct.Delete(equipmentVerificationActId, userId, companyId);
    }
}