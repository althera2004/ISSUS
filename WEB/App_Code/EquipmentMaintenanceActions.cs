// --------------------------------
// <copyright file="EquipmentMaintenanceActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentMaintenanceActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentMaintenanceActions : WebService
{

    public EquipmentMaintenanceActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentMaintenanceDefinition equipmentMaintenance, int userId)
    {
        var res = equipmentMaintenance.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentMaintenance.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentMaintenanceDefinition newEquipmentMaintenanceDefinition, EquipmentMaintenanceDefinition oldEquipmentMaintenanceDefinition, int userId)
    {
        string differences = newEquipmentMaintenanceDefinition.Differences(oldEquipmentMaintenanceDefinition);
        return newEquipmentMaintenanceDefinition.Update(differences, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentMaintenanceDefinitionId, int companyId, int userId)
    {
        return EquipmentMaintenanceDefinition.Delete(equipmentMaintenanceDefinitionId, userId, companyId);

    }
}