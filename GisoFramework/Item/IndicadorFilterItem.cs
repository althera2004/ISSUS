﻿// --------------------------------
// <copyright file="IndicadorFilterItem.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;

    public class IndicadorFilterItem
    {
        public Indicador Indicador { get; set; }

        public Process Proceso { get; set; }

        public Objetivo Objetivo { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string ProcessResponsible { get; set; }

        public string ObjetivoResponsible { get; set; }

        public int Status { get; set; }

        public static bool operator ==(IndicadorFilterItem filter1, IndicadorFilterItem filter2)
        {
            if (filter1 == null)
            {
                return filter2 == null;
            }

            if (filter2 == null)
            {
                return false;
            }

            return filter1.Equals(filter2);
        }

        public static bool operator !=(IndicadorFilterItem filter1, IndicadorFilterItem filter2)
        {
            if (filter1 == null)
            {
                return filter2 != null;
            }

            if (filter2 == null)
            {
                return true;
            }

            return !filter1.Equals(filter2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IndicadorFilterItem))
            {
                return false;
            }

            return this.Equals((IndicadorFilterItem)obj);
        }

        public bool Equals(IndicadorFilterItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Indicador.Id != other.Indicador.Id)
            {
                return false;
            }

            return this.Indicador.Id == other.Indicador.Id;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}