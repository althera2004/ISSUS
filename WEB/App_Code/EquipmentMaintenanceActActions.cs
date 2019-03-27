// --------------------------------
// <copyright file="EquipmentMaintenanceActActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for EquipmentMaintenanceActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentMaintenanceActActions : WebService {

    public EquipmentMaintenanceActActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentMaintenanceAct equipmentMaintenanceAct, int userId)
    {
        var res = equipmentMaintenanceAct.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentMaintenanceAct.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentMaintenanceAct equipmentMaintenanceAct, int userId)
    {
        return equipmentMaintenanceAct.Update(string.Empty, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentMaintenanceActId, int companyId, int userId)
    {
        return EquipmentMaintenanceAct.Delete(equipmentMaintenanceActId, userId, companyId);
    }
}