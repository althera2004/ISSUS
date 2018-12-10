// --------------------------------
// <copyright file="BusinessRiskFilterItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    public class BusinessRiskFilterItem
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public long BusinessRiskId { get; set; }

        public DateTime? OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public DateTime? ImplementationDate { get; set; }

        public string Number { get; set; }

        public Rules Rule { get; set; }

        public Process Process { get; set; }

        public int FinalResult { get; set; }

        public int StartAction { get; set; }

        public int InitialResult { get; set; }

        public int FinalAction { get; set; }

        public bool Assumed { get; set; }

        public int Status { get; set; }
    }
}