// --------------------------------
// <copyright file="EmployeeAddress.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System.Globalization;
    using System.Text;

    /// <summary>Implements EmployeeAddress class</summary>
    public class EmployeeAddress
    {
        #region Fields
        /// <summary>
        /// Street address
        /// </summary>
        private string address;

        /// <summary>
        /// Postal code
        /// </summary>
        private string postalCode;

        /// <summary>
        /// City of address
        /// </summary>
        private string city;

        /// <summary>
        /// Province of address
        /// </summary>
        private string province;

        /// <summary>
        /// Country of address
        /// </summary>
        private string country;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a empty address
        /// </summary>
        public static EmployeeAddress Empty
        {
            get
            {
                return new EmployeeAddress()
                {
                    address = string.Empty,
                    postalCode = string.Empty,
                    city = string.Empty,
                    province = string.Empty,
                    country = string.Empty
                };
            }
        }

        /// <summary>
        /// Gets or sets street address
        /// </summary>
        public string Address
        {
            get 
            {
                return this.address; 
            }

            set 
            { 
                this.address = value; 
            }
        }

        /// <summary>
        /// Gets or sets postal code
        /// </summary>
        public string PostalCode
        {
            get 
            {
                return this.postalCode;
            }

            set 
            {
                this.postalCode = value;
            }
        }

        /// <summary>
        /// Gets or sets city of address
        /// </summary>
        public string City
        {
            get
            {
                return this.city;
            }

            set
            {
                this.city = value;
            }
        }

        /// <summary>
        /// Gets or sets province of address
        /// </summary>
        public string Province
        {
            get 
            {
                return this.province;
            }

            set 
            {
                this.province = value;
            }
        }

        /// <summary>
        /// Gets or sets country of address
        /// </summary>
        public string Country
        {
            get
            {
                return this.country; 
            }

            set 
            {
                this.country = value; 
            }
        }

        /// <summary>
        /// Gets a JSON structure of address
        /// </summary>
        public string Json
        {
            get
            {
                string pattern = @"
                                {{
                                    ""Address"":""{0}"",
                                    ""PostalCode"":""{1}"",
                                    ""City"":""{2}"",
                                    ""Province"":""{3}"",
                                    ""Country"":{4}
                                }}";
                return string.Format(
                    CultureInfo.GetCultureInfo("en-us"),
                    pattern,
                    Tools.JsonCompliant(this.address),
                    this.postalCode,
                    Tools.JsonCompliant(this.city),
                    Tools.JsonCompliant(this.province),
                    string.IsNullOrEmpty(this.country) ? "0" : Tools.JsonCompliant(this.country));
            }
        }
        #endregion

        /// <summary>
        /// Generates a representative string of address's data
        /// </summary>
        /// <returns>Representative string of address's data</returns>
        public override string ToString()
        {
            StringBuilder res = new StringBuilder();

            if (string.IsNullOrEmpty(this.address) && string.IsNullOrEmpty(this.city) && string.IsNullOrEmpty(this.postalCode) && string.IsNullOrEmpty(this.province))
            {
                return string.Empty;
            }

            res.Append(this.address).Append(", ").Append(this.city).Append(" - ").Append(this.postalCode).Append(", ").Append(this.province).Append(" (").Append(this.country).Append(")");
            return res.ToString();
        }
    }
}
