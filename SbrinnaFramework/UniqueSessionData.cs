// --------------------------------
// <copyright file="UniqueSessionData.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace SbrinnaCoreFramework
{
    using System;

    public struct UniqueSessionData
    {
        public Guid Token { get; set; }
        public int UserId { get; set; }
        public string IP { get; set; }
        public DateTime LastConnection { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
