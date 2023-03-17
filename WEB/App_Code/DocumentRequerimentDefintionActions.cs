// --------------------------------
// <copyright file="DocumentRequerimentDefintionActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>
/// Descripción breve de DocumentRequerimentDefintionActions
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DocumentRequerimentDefintionActions : WebService
{
    public DocumentRequerimentDefintionActions()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public ActionResult Save(DocumentRequerimentDefinition documentRequerimentDefinition, int userId)
    {
        return documentRequerimentDefinition.Save(userId);
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
    public ActionResult Delete(int documentRequerimentDefinitionId, int companyId, int userId)
    {
        return DocumentRequerimentDefinition.Delete(documentRequerimentDefinitionId, userId, companyId);
    }
}
