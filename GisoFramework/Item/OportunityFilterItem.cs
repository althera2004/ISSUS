// --------------------------------
// <copyright file="OportunityFilterItem.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    public class OportunityFilterItem
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public long OportunityId { get; set; }

        public string Number { get; set; }

        public Rules Rule { get; set; }

        public Process Process { get; set; }

        public int Result { get; set; }

        public DateTime? AnulateDate { get; set; }

        public DateTime OpenDate { get; set; }
    }
}