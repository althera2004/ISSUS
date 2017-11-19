// --------------------------------
// <copyright file="EquipmentRepairActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for EquipmentRepairActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentRepairActions : WebService {

    public EquipmentRepairActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentRepair equipmentRepair, int userId)
    {
        ActionResult res = equipmentRepair.Insert(userId);

        if (res.Success)
        {
            Session["Company"] = new Company(equipmentRepair.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentRepair equipmentRepair, int userId)
    {
        return equipmentRepair.Update(string.Empty, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentRepairId, int companyId, int userId)
    {
        return EquipmentRepair.Delete(equipmentRepairId, userId, companyId);
    }

}

