// --------------------------------
// <copyright file="IncidentActionFilterItem.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implements IncidentActionFilterItem class
    /// </summary>
    public class IncidentActionFilterItem
    {
        public long IncidentActionId { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? ImplementationDate { get; set; }

        public int ActionType { get; set; }

        public int ReporterType { get; set; }

        public int Origin { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }

        public string Number { get; set; }

        public Incident Incident { get; set; }

        public decimal Amount { get; set; }

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

        public override bool Equals(object obj)
        {
            if (!(obj is IncidentActionFilterItem))
            {
                return false;
            }

            return this.Equals((IncidentActionFilterItem)obj);
        }

        public bool Equals(IncidentActionFilterItem other)
        {
            if(other==null)
            {
                return false;
            }

            if (this.IncidentActionId != other.IncidentActionId)
            {
                return false;
            }

            return this.IncidentActionId == other.IncidentActionId;
        }
    }
}
