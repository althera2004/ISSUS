// --------------------------------
// <copyright file="IncidentFilterItem.cs" company="Sbrinna">
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
    /// Implements IncidentFilterItem class
    /// </summary>
    public class IncidentFilterItem
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int Origin { get; set; }

        public string OriginText { get; set; }

        public IncidentAction Action { get; set; }

        public Department Department { get; set; }

        public Provider Provider { get; set; }

        public Customer Customer { get; set; }

        public DateTime? Open { get; set; }

        public DateTime? Close { get; set; }

        public decimal Amount { get; set; }

        public static bool operator ==(IncidentFilterItem filter1, IncidentFilterItem filter2)
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

        public static bool operator !=(IncidentFilterItem filter1, IncidentFilterItem filter2)
        {
            if(filter1==null && filter2 == null)
            {
                return false;
            }

            if(filter1==null)
            {
                return true;
            }

            if(filter2==null)
            {
                return true;
            }

            return !filter1.Equals(filter2);
        }  

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is IncidentFilterItem))
            {
                return false;
            }

            return this.Equals(obj as IncidentFilterItem);
        }

        public bool Equals(IncidentFilterItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Id != other.Id)
            {
                return false;
            }

            return this.Id == other.Id;
        }
    }
}
