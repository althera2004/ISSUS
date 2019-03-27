// --------------------------------
// <copyright file="EquipmentCalibrationDefinitionActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentCalibrationDefinition</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentCalibrationDefinitionActions : WebService
{
    public EquipmentCalibrationDefinitionActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentCalibrationDefinition equipmentCalibrationDefinition, int userId)
    {
        var res = equipmentCalibrationDefinition.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentCalibrationDefinition.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentCalibrationDefinition equipmentCalibrationDefinition, int userId)
    {
        return equipmentCalibrationDefinition.Update(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentCalibrationDefinitionId, int companyId, int userId)
    {
        return new EquipmentCalibrationDefinition
        {
            Id = equipmentCalibrationDefinitionId,
            CompanyId = companyId
        }.Delete(userId);
    }
}
