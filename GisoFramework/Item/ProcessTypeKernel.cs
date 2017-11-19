// --------------------------------
// <copyright file="ProcessTypeKernel.cs" company="Sbrinna">
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
    /// Implements ProcessTypeKernel class
    /// </summary>
    public static class ProcessTypeKernel
    {
        /// <summary>0 - Undefined</summary>
        public static readonly int None = 0;

        /// <summary>1 - Principal</summary>
        public static readonly int Principal = 1;

        /// <summary>2 - Support</summary>
        public static readonly int Support = 2;

        /// <summary>3 - Strategic</summary>
        public static readonly int Strategic = 3;
    }
}
