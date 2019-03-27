// --------------------------------
// <copyright file="EmployeeAddress.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System.Globalization;
    using System.Text;

    /// <summary>Implements EmployeeAddress class</summary>
    public class EmployeeAddress
    {
        /// <summary>Gets a empty address</summary>
        public static EmployeeAddress Empty
        {
            get
            {
                return new EmployeeAddress
                {
                    Address = string.Empty,
                    PostalCode = string.Empty,
                    City = string.Empty,
                    Province = string.Empty,
                    Country = string.Empty
                };
            }
        }

        /// <summary>Gets or sets street address</summary>
        public string Address { get; set; }

        /// <summary>Gets or sets postal code</summary>
        public string PostalCode { get; set; }

        /// <summary>Gets or sets city of address</summary>
        public string City { get; set; }

        /// <summary>Gets or sets province of address</summary>
        public string Province { get; set; }

        /// <summary>Gets or sets country of address</summary>
        public string Country { get; set; }

        /// <summary>Gets a JSON structure of address</summary>
        public string Json
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    @"{{ ""Address"":""{0}"", ""PostalCode"":""{1}"", ""City"":""{2}"", ""Province"":""{3}"", ""Country"":{4} }}",
                    Tools.JsonCompliant(this.Address),
                    this.PostalCode,
                    Tools.JsonCompliant(this.City),
                    Tools.JsonCompliant(this.Province),
                    string.IsNullOrEmpty(this.Country) ? "0" : Tools.JsonCompliant(this.Country));
            }
        }

        /// <summary>Generates a representative string of address's data</summary>
        /// <returns>Representative string of address's data</returns>
        public override string ToString()
        {
            var res = new StringBuilder();
            if (string.IsNullOrEmpty(this.Address) && string.IsNullOrEmpty(this.City) && string.IsNullOrEmpty(this.PostalCode) && string.IsNullOrEmpty(this.Province))
            {
                return string.Empty;
            }

            res.Append(this.Address).Append(", ").Append(this.City).Append(" - ").Append(this.PostalCode).Append(", ").Append(this.Province).Append(" (").Append(this.Country).Append(")");
            return res.ToString();
        }
    }
}