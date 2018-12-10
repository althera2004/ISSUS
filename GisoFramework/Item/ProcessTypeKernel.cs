// --------------------------------
// <copyright file="ProcessTypeKernel.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
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
