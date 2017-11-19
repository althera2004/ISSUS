// --------------------------------
// <copyright file="EquipmentVerificationDefinitionActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Summary description for VerificationDefinitionActions
/// </summary>
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
        ActionResult res = equipmentVerificationDefinition.Insert(userId);

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
        EquipmentVerificationDefinition victim = new EquipmentVerificationDefinition()
        {
            Id = equipmentVerificationDefinitionId,
            CompanyId = companyId
        };
        return victim.Delete(userId);
    }
}
