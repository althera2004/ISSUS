// --------------------------------
// <copyright file="DocumentRequerimentActActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Descripción breve de DocumentRequerimentActActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DocumentRequerimentActActions : WebService
{
    public DocumentRequerimentActActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult Save(DocumentRequerimentAct documentRequerimentAct, int userId)
    {
        return documentRequerimentAct.Save(userId);
    }

    /*[WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Update(DocumentRequerimentDefinition newDocumentRequerimentDefinition, DocumentRequerimentDefinition oldDocumentRequerimentDefinition, int userId)
    {
        string differences = newDocumentRequerimentDefinition.Differences(oldDocumentRequerimentDefinition);
        return newDocumentRequerimentDefinition.Update(differences, userId);
    }*/

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult Delete(int documentRequerimentActId, int companyId, int userId)
    {
        return DocumentRequerimentAct.Delete(documentRequerimentActId, userId, companyId);
    }
}