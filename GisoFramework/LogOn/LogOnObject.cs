// --------------------------------
// <copyright file="LogOnObject.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.LogOn
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using GisoFramework.LogOn;

    /// <summary>
    /// Implements LogOnObject class
    /// </summary>
    public class LogOnObject
    {
        /// <summary>
        /// Gets or sets the membership of user in security groups
        /// </summary>
        private List<ApplicationLogOn.SecurityGroup> membership;

        /// <summary>
        /// Gets or sets user identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the result of log on action
        /// </summary>
        public ApplicationLogOn.LogOnResult Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can access to multiple companies
        /// </summary>
        public bool MultipleCompany { get; set; }

        /// <summary>
        /// Gets or sets de identifier of compnay to log in
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if user must change password
        /// </summary>
        public bool MustResetPassword { get; set; }

        /// <summary>
        /// Gets the membership of user in security groups
        /// </summary>
        public ReadOnlyCollection<ApplicationLogOn.SecurityGroup> Membership
        {
            get
            {
                if (this.membership == null)
                {
                    this.membership = new List<ApplicationLogOn.SecurityGroup>();
                }

                return new ReadOnlyCollection<ApplicationLogOn.SecurityGroup>(this.membership);
            }
        }

        /// <summary>
        /// Compare object with another
        /// </summary>
        /// <param name="logOnObject1">Object from compare</param>
        /// <param name="logOnObject2">Object to compare</param>
        /// <returns>Indicates if objects are equals</returns>
        public static bool operator ==(LogOnObject logOnObject1, LogOnObject logOnObject2)
        {
            if (logOnObject1 == null && logOnObject2 == null)
            {
                return true;
            }

            if (logOnObject1 == null)
            {
                return false;
            }

            if (logOnObject2 == null)
            {
                return false;
            }

            return logOnObject1.Equals(logOnObject2);
        }

        /// <summary>
        /// Compare object with another
        /// </summary>
        /// <param name="logOnObject1">Object from compare</param>
        /// <param name="logOnObject2">Object to compare</param>
        /// <returns>Indicates if objects are different</returns>
        public static bool operator !=(LogOnObject logOnObject1, LogOnObject logOnObject2)
        {
            if (logOnObject1 == null && logOnObject2 == null)
            {
                return false;
            }

            if (logOnObject1 == null)
            {
                return true;
            }

            if (logOnObject2 == null)
            {
                return true;
            }

            return !logOnObject1.Equals(logOnObject2);
        }

        /// <summary>
        /// Compare object with another
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns>Indicates if objects are equals</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is LogOnObject))
            {
                return false;
            }

            return this.Equals((LogOnObject)obj);
        }

        /// <summary>
        /// Compare object with another
        /// </summary>
        /// <param name="other">Object to compare</param>
        /// <returns>Indicates if objects are equals</returns>
        public bool Equals(LogOnObject other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.Result != other.Result)
            {
                return false;
            }

            return this.Result == other.Result;
        }

        /// <summary>
        /// Get the hash code of object
        /// </summary>
        /// <returns>Hash code of object</returns>
        public override int GetHashCode()
        {
            int hash = 17;

            hash = hash * (23 + this.Id.GetHashCode());
            hash = hash * (23 + this.UserName.GetHashCode());
            return hash;
        }        
    }
}
