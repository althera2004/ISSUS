// --------------------------------
// <copyright file="DocumentActions.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GISOWeb
{
    using System;
    using System.Web.Script.Services;
    using System.Web.Services;
    using GisoFramework.Activity;
    using GisoFramework.Item;

    /// <summary>
    /// Asynchronous actions for documents
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class DocumentActions : WebService
    {
        /// <summary>
        /// Initializes a new instance of the DocumentActions class.
        /// </summary>
        public DocumentActions()
        {
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult Restore(int documentId, int companyId, int userId)
        {
            return Document.Restore(documentId, companyId, userId);
        }

        /// <summary>
        /// Asynchronous action to call category insert
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="description">Category description</param>
        /// <param name="companyId">Identifier of category's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult CategoryInsert(int categoryId, string description, int companyId, int userId)
        {
            DocumentCategory newCategory = new DocumentCategory()
            {
                Id = categoryId,
                Description = description,
                CompanyId = companyId,
                Editable = true
            };

            ActionResult res = newCategory.Insert();
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call category update
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="description">Category description</param>
        /// <param name="companyId">Identifier of category's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult CategoryUpdate(int categoryId, string description, int companyId, int userId)
        {
            DocumentCategory newCategory = new DocumentCategory()
            {
                Id = categoryId,
                Description = description,
                CompanyId = companyId,
                Editable = true
            };

            ActionResult res = newCategory.Update();
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call category delete
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="description">Category description</param>
        /// <param name="companyId">Identifier of category's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult CategoryDelete(int categoryId, string description, int companyId, int userId)
        {
            ActionResult res = DocumentCategory.Delete(categoryId, companyId);
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call source insert
        /// </summary>
        /// <param name="procedenciaId">Source identifier</param>
        /// <param name="description">Source description</param>
        /// <param name="companyId">Identifier of source's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ProcedenciaInsert(int procedenciaId, string description, int companyId, int userId)
        {
            DocumentOrigin newProcedencia = new DocumentOrigin()
            {
                CompanyId = companyId,
                Description = description,
                Editable = true
            };

            ActionResult res = newProcedencia.Insert();
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call source update
        /// </summary>
        /// <param name="procedenciaId">Source identifier</param>
        /// <param name="description">Source description</param>
        /// <param name="companyId">Identifier of source's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ProcedenciaUpdate(int procedenciaId, string description, int companyId, int userId)
        {
            DocumentOrigin newProcedencia = new DocumentOrigin()
            {
                Id = procedenciaId,
                CompanyId = companyId,
                Description = description,
                Editable = true
            };

            ActionResult res = newProcedencia.Update();
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call source delete
        /// </summary>
        /// <param name="procedenciaId">Source identifier</param>
        /// <param name="description">Source description</param>
        /// <param name="companyId">Identifier of source's company</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult ProcedenciaDelete(int procedenciaId, string description, int companyId, int userId)
        {
            ActionResult res = DocumentOrigin.Delete(procedenciaId, companyId);
            if (res.Success)
            {
                Session["Company"] = new Company(companyId);
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call document update
        /// </summary>
        /// <param name="oldDocument">Old document data</param>
        /// <param name="newDocument">New document data</param>
        /// <param name="reason">Reason of change</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult Update(Document oldDocument, Document newDocument, string reason, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            string extraData = string.Empty;
            if (oldDocument != null)
            {
                extraData = newDocument.Differences(oldDocument);
            }

            res = newDocument.Update(userId);
            if (res.Success)
            {
                res = ActivityLog.Document(Convert.ToInt32(newDocument.Id), userId, newDocument.CompanyId, DocumentLogAction.Update, extraData);
            }

            //int lastVersion = newDocument.LastVersionNumber();
            //res = Document.Versioned(Convert.ToInt32(newDocument.Id), userId, newDocument.Company.Id, lastVersion + 1, reason);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult Versioned(int documentId, int userId, int companyId, int version, string reason)
        {
            return Document.Versioned(documentId, userId, companyId, version + 1, reason);
        }

        /// <summary>
        /// Asynchronous action to call document insert
        /// </summary>
        /// <param name="newDocument">Document to insert</param>
        /// <param name="reason">Reason of insert</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult Insert(Document newDocument, string reason, int version, int userId)
        {
            ActionResult res = ActionResult.NoAction;
            res = newDocument.Insert(userId, version);
            if (res.Success)
            {
                res = ActivityLog.Document(Convert.ToInt32(newDocument.Id), userId, newDocument.CompanyId, DocumentLogAction.Create, newDocument.Json);
                //res = Document.Versioned(Convert.ToInt32(newDocument.Id), userId, newDocument.Company.Id, version, "Document insert");
                if (res.Success)
                {
                    res.SetSuccess(Convert.ToInt32(newDocument.Id));
                }
            }

            return res;
        }

        /// <summary>
        /// Asynchronous action to call document delete
        /// </summary>
        /// <param name="documentId">Document identifier</param>
        /// <param name="companyId">Document's company identifier</param>
        /// <param name="userId">Identifier of user of action</param>
        /// <returns>Result of action</returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public ActionResult DocumentDelete(int documentId, int companyId, int userId, string reason)
        {
            return Document.Delete(documentId, companyId, userId, reason);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public string SetFilter(string filter)
        {
            Session["DocumentFilter"] = filter.ToUpperInvariant();
            return "OK";
        }
    }
}