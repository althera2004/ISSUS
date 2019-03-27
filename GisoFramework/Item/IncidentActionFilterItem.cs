// --------------------------------
// <copyright file="IncidentActionFilterItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;

    /// <summary>Implements IncidentActionFilterItem class</summary>
    public class IncidentActionFilterItem
    {
        /// <summary>Gets or sets action identifier</summary>
        public long IncidentActionId { get; set; }

        /// <summary>Gets or sets open date</summary>
        public DateTime? OpenDate { get; set; }

        /// <summary>Gets or sets close date</summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>Gets or sets implementation date</summary>
        public DateTime? ImplementationDate { get; set; }

        /// <summary>Gets or sets action type</summary>
        public int ActionType { get; set; }

        /// <summary>Gets or sets reporter type</summary>
        public int ReporterType { get; set; }

        /// <summary>Gets or sets origin</summary>
        public int Origin { get; set; }

        /// <summary>Gets or sets origin text</summary>
        public string OriginText { get; set; }

        /// <summary>Gets or sets status</summary>
        public int Status { get; set; }

        /// <summary>Gets or sets description</summary>
        public string Description { get; set; }

        /// <summary>Gets or sets number</summary>
        public string Number { get; set; }

        /// <summary>Gets or sets incident</summary>
        public Incident Incident { get; set; }

        /// <summary>Gets or sets ammount</summary>
        public decimal Amount { get; set; }

        /// <summary>Implements equals operator</summary>
        /// <param name="filter1">First filter to compare</param>
        /// <param name="filter2">Second filter to compare</param>
        /// <returns>Equals between tow objects</returns>
        public static bool operator ==(IncidentActionFilterItem filter1, IncidentActionFilterItem filter2)
        {
            if (filter1 == null && filter2 == null)
            {
                return true;
            }

            if (filter1 != null && filter2 == null)
            {
                return false;
            }

            if (filter1 == null && filter2 != null)
            {
                return false;
            }

            return filter1.Equals(filter2);
        }

        /// <summary>Implements not equals operator</summary>
        /// <param name="filter1">First filter to compare</param>
        /// <param name="filter2">Second filter to compare</param>
        /// <returns>Not equals between tow objects</returns>
        public static bool operator !=(IncidentActionFilterItem filter1, IncidentActionFilterItem filter2)
        {
            if (filter1 == null && filter2 == null)
            {
                return false;
            }

            if (filter1 == null)
            {
                return true;
            }

            if (filter2 == null)
            {
                return true;
            }

            return !filter1.Equals(filter2);
        }

        /// <summary>Overrides equals function</summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Equals function</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is IncidentActionFilterItem))
            {
                return false;
            }

            return this.Equals((IncidentActionFilterItem)obj);
        }

        /// <summary>Overrides equals function</summary>
        /// <param name="other">Filter to compare</param>
        /// <returns>Equals function</returns>
        public bool Equals(IncidentActionFilterItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.IncidentActionId != other.IncidentActionId)
            {
                return false;
            }

            return this.IncidentActionId == other.IncidentActionId;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}