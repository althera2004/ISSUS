// --------------------------------
// <copyright file="EquipmentVerificationDefinitionActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for VerificationDefinitionActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class EquipmentVerificationDefinitionActions : WebService {

    public EquipmentVerificationDefinitionActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(EquipmentVerificationDefinition equipmentVerificationDefinition, int userId)
    {
        var res = equipmentVerificationDefinition.Insert(userId);
        if (res.Success)
        {
            Session["Company"] = new Company(equipmentVerificationDefinition.CompanyId);
        }

        return res;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(EquipmentVerificationDefinition equipmentVerificationDefinition, int userId)
    {
        return equipmentVerificationDefinition.Update(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(int equipmentVerificationDefinitionId, int companyId, int userId)
    {
        return new EquipmentVerificationDefinition
        {
            Id = equipmentVerificationDefinitionId,
            CompanyId = companyId
        }.Delete(userId);
    }
}