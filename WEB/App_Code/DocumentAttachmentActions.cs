// --------------------------------
// <copyright file="DocumentAttachmentActions.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
using System.Web.Script.Services;
using System.Web.Services;
using GisoFramework.Activity;
using GisoFramework.Item;

/// <summary>Summary description for DocumentAttachmentActions</summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
public class DocumentAttachmentActions : WebService {

    /// <summary>Initializes a new instance of the DocumentAttachActions class</summary>
    public DocumentAttachmentActions ()
    {
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public ActionResult Delete(long attachId, int companyId)
    {
        return DocumentAttach.Delete(attachId, companyId);
    }    
}