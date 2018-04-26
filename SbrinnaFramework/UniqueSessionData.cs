﻿// --------------------------------
// <copyright file="UniqueSessionData.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
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
    }
}
