// --------------------------------
// <copyright file="ObjetivoFilterItem.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    public class ObjetivoFilterItem
    {
        public Objetivo Objetivo { get; set; }

        public static bool operator ==(ObjetivoFilterItem filter1, ObjetivoFilterItem filter2)
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

        public static bool operator !=(ObjetivoFilterItem filter1, ObjetivoFilterItem filter2)
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
            if (!(obj is IndicadorFilterItem))
            {
                return false;
            }

            return this.Equals((ObjetivoFilterItem)obj);
        }

        public bool Equals(ObjetivoFilterItem other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Objetivo.Id != other.Objetivo.Id)
            {
                return false;
            }

            return this.Objetivo.Id == other.Objetivo.Id;
        }
    }
}