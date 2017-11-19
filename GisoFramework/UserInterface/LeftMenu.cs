// -----------------------------------------------------------------------
// <copyright file="LeftMenu.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace GisoFramework.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Implements a class of application user
    /// </summary>
    public class LeftMenu
    {
        /// <summary>
        /// Dictionary to translate interface texts
        /// </summary>
        private Dictionary<string, string> dictionary;

        /// <summary>
        /// Application user logged
        /// </summary>
        private ApplicationUser user;

        /// <summary>
        /// Initializes a new instance of the LeftMenu class
        /// </summary>
        /// <param name="user">User logged</param>
        /// <param name="dictionary">Dictionary to translate interface texts</param>
        public LeftMenu(ApplicationUser user, Dictionary<string, string> dictionary)
        {
            this.dictionary = dictionary;
            this.user = user;
        }

        /// <summary>
        /// Render the html code of the left menu
        /// </summary>
        /// <returns>The html code of the left menu</returns>
        /*public string Render(bool selected)
        {
            StringBuilder res = new StringBuilder(Environment.NewLine);

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Company))
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Cuadro de mandos"], "/DashBoard.aspx", false, "icon-dashboard"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Company))
            {
                res.Append(LeftMenuOption.RenderAdmin(this.dictionary, this.user, selected));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Process))
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Procesos"], "/ProcesosList.aspx", false, "icon-gear"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Documents))
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Documentos"], "/Documents.aspx", false, "icon-book"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Learning))
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Formación"], "/FormacionList.aspx", false, "icon-edit"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Equipos) || true)
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Equipos"], "/EquipmentList.aspx", false, "icon-edit"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Incidence) || true)
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Incidencias"], "IncidentList.aspx", false, "icon-edit"));
            }

            if (this.user.Groups.Contains(ApplicationLogOn.SecurityGroup.Actions) || true)
            {
                res.Append(LeftMenuOption.Render(this.dictionary["Acciones"], "ActionList.aspx", false, "icon-edit"));
            }

            res.Append(Environment.NewLine);
            return res.ToString();
        }*/
    }
}
