// --------------------------------
// <copyright file="TrazasList.aspx.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI;
using GisoFramework;
using GisoFramework.Activity;
using GisoFramework.Item;
using SbrinnaCoreFramework;

/// <summary>
/// Implements Trace list page
/// </summary>
public partial class TrazasList : Page
{
    /// <summary> Master of page</summary>
    private Giso master;

    /// <summary>
    /// Application user logged in session
    /// </summary>
    private ApplicationUser user;

    /// <summary>Company of session</summary>
    private Company company;

    /// <summary>Dictionary for fixed labels</summary>
    private Dictionary<string, string> dictionary;

    /// <summary>
    /// Gets dictionary for fixed labels
    /// </summary>
    public Dictionary<string, string> Dictionary
    {
        get
        {
            return this.dictionary;
        }
    }

    /// <summary>
    /// Page's load event
    /// </summary>
    /// <param name="sender">Loaded page</param>
    /// <param name="e">Event's arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null || this.Session["UniqueSessionId"] == null)
        {
            this.Response.Redirect("Default.aspx", true);
            Context.ApplicationInstance.CompleteRequest();
        }
        else
        {
            this.user = this.Session["User"] as ApplicationUser;
            Guid token = new Guid(this.Session["UniqueSessionId"].ToString());
            if (!UniqueSession.Exists(token, this.user.Id))
            {
                this.Response.Redirect("MultipleSession.aspx", true);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                this.Go();
            }
        }
    }

    /// <summary>
    /// Begin page running after session validations
    /// </summary>
    private void Go()
    {
        this.company = Session["Company"] as Company;
        this.dictionary = Session["Dictionary"] as Dictionary<string, string>;
        this.master = this.Master as Giso;
        string serverPath = this.Request.Url.AbsoluteUri.Replace(this.Request.RawUrl.Substring(1), string.Empty);
        this.master.AddBreadCrumb("Item_Traces");
        this.master.Titulo = "Item_Traces";

        ReadOnlyCollection<ActivityTrace> activity24h = ActivityLog.GetActivity24H(this.company.Id);
        StringBuilder activi24HText = new StringBuilder();
        StringBuilder tracesDocument = new StringBuilder();
        StringBuilder tracesEmployee = new StringBuilder();
        StringBuilder tracesDepartment = new StringBuilder();
        StringBuilder tracesProccess = new StringBuilder();
        StringBuilder tracesJobPosition = new StringBuilder();
        StringBuilder tracesCompany = new StringBuilder();
        StringBuilder tracesLearning = new StringBuilder();
        StringBuilder tracesUser = new StringBuilder();
        StringBuilder tracesOtros = new StringBuilder();
        StringBuilder tracesLearningAssistant = new StringBuilder();

        foreach (ActivityTrace activity in activity24h)
        {
            string row = activity.TableTracesRow;
            switch (activity.Target)
            {
                case "Department":
                    tracesDepartment.Append(row);
                    break;
                case "Document":
                    tracesDocument.Append(row);
                    break;
                case "Employee":
                    tracesEmployee.Append(row);
                    break;
                case "Process":
                    tracesProccess.Append(row);
                    break;
                case "Job position":
                    tracesJobPosition.Append(row);
                    break;
                case "Company":
                    tracesCompany.Append(row);
                    break;
                case "Learning":
                    tracesLearning.Append(row);
                    break;
                case "Learning assistant":
                    tracesLearningAssistant.Append(row);
                    break;
                case "User":
                    tracesLearning.Append(row);
                    break;
                default:
                    tracesOtros.Append(activity.TableRow);
                    break;
            }

            this.LtTraceProccess.Text = tracesProccess.ToString();
            this.LtTracesDocument.Text = tracesDocument.ToString();
            this.LtTracesDepartment.Text = tracesDepartment.ToString();
            this.LtTracesLearning.Text = tracesLearning.ToString();
            this.LtTracesEmployee.Text = tracesEmployee.ToString();
            this.LtTracesJobPosition.Text = tracesJobPosition.ToString();
            this.LtTracesUser.Text = tracesUser.ToString();
            this.LtTracesOtros.Text = tracesOtros.ToString();
            this.LtTracesLearningAssistant.Text = tracesLearningAssistant.ToString();
        }
    }
}