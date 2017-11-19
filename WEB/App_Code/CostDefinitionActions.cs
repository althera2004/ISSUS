// --------------------------------
// <copyright file="CostDefinitionActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.LogOn;
using GisoFramework.Item;

/// <summary>
/// Summary description for CostDefinitionActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class CostDefinitionActions : WebService
{
    public CostDefinitionActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Insert(CostDefinition costDefinition, int userId)
    {
        return costDefinition.Insert(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(CostDefinition costDefinition, int userId)
    {
        return costDefinition.Update(userId);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Inactive(long costDefinitionId, int companyId, int userId)
    {
        return CostDefinition.Inactive(costDefinitionId, companyId, userId);
    }
}
