// --------------------------------
// <copyright file="EquipmentCalibrationActActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for EquipmentCalibrationActActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentCalibrationActActions : WebService
{
    public EquipmentCalibrationActActions ()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentCalibrationAct equipmentCalibrationAct, int userId)
    {
        var res = equipmentCalibrationAct.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentCalibrationAct.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentCalibrationAct equipmentCalibrationAct, int userId)
    {
        return equipmentCalibrationAct.Update(string.Empty, userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentCalibrationActId, int companyId, int userId)
    {
        return EquipmentCalibrationAct.Delete(equipmentCalibrationActId, userId, companyId);
    }
}