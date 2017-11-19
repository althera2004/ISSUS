// --------------------------------
// <copyright file="BaseItem.cs" company="Sbrinna">
//     Copyright (c) Sbrinna. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@sbrinna.com</author>
// --------------------------------
namespace GisoFramework.Item
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using GisoFramework.Activity;
    using System.Globalization;

    /// <summary>
    /// Implements BaseItem class
    /// </summary>
    public abstract class BaseItem
    {
        /// <summary>
        /// Gets or sets the item identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the item's company identifier
        /// </summary>
        public int CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the item description
        /// </summary>
        [DifferenciableAttribute]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the employee that creates item
        /// </summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the date of creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the employee that makes the last modification
        /// </summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the last modification date of item
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item can be deleted
        /// </summary>
        public bool CanBeDeleted { get; set; }
        
        /// <summary>
        /// Gets a JSON structure of item data
        /// </summary>        
        public abstract string Json { get; }

        /// <summary>
        /// Gets a JSON structure of item key/value 
        /// </summary>        
        public abstract string JsonKeyValue { get; }

        /// <summary>
        /// Gets the HTML code for a link to Item page
        /// </summary>
        public abstract string Link { get; }

        /// <summary>
        /// Gets the differences compared with anhoter item
        /// </summary>
        /// <param name="item1">Item to compare</param>
        /// <returns>String with the differences compared with anhoter item</returns>
        public string Differences(BaseItem item1)
        {
            string source = "Differences(BaseItem)";
            StringBuilder res = new StringBuilder();
            List<Variance> variances = new List<Variance>();
            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var propertyAttributes = property.GetCustomAttributes(true);

                if (propertyAttributes.Contains(new DifferenciableAttribute(true)))
                {
                    Variance v = new Variance();
                    v.PropertyName = property.Name;
                    v.NewValue = property.GetValue(this, null);
                    v.OldValue = property.GetValue(item1, null);

                    if (v.NewValue == null && v.OldValue != null)
                    {
                        variances.Add(v);
                    }
                    else if (v.NewValue != null && v.OldValue == null)
                    {
                        variances.Add(v);
                    }
                    else if (v.NewValue == null && v.OldValue == null)
                    {
                    }
                    else if (v.OldValue.GetType().FullName.StartsWith("Giso", StringComparison.Ordinal))
                    {
                        string va = v.NewValue.GetType().GetProperty("Id").GetValue(v.NewValue, null).ToString();
                        string vb = v.OldValue.GetType().GetProperty("Id").GetValue(v.OldValue, null).ToString();
                        if (va != vb)
                        {
                            variances.Add(v);
                        }
                    }
                    else if (!v.NewValue.Equals(v.OldValue))
                    {
                        variances.Add(v);
                    }
                }
            }

            bool first = true;
            foreach (Variance v in variances)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    res.Append("|");
                }

                try
                {
                    if (v.OldValue == null)
                    {
                        //// string newValue = v.NewValue.GetType().GetProperty("Id").GetValue(v.NewValue, null).ToString();
                        string newValue = v.NewValue.ToString();
                        res.Append(v.PropertyName).Append("::null-->").Append(newValue);
                    }
                    else if(v.OldValue.GetType().Name.ToUpperInvariant().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        if (v.NewValue == null)
                        {
                            res.Append(v.PropertyName).Append(@"::""""-->null");
                        }
                        else
                        {
                            res.Append(v.PropertyName).AppendFormat(CultureInfo.InvariantCulture, @"::""""-->{0}", v.NewValue);
                        }
                    }
                    else if (v.NewValue == null)
                    {
                        string oldValue = v.OldValue.GetType().GetProperty("Id").GetValue(v.OldValue, null).ToString();
                        res.Append(v.PropertyName).Append("::").Append(oldValue).Append("-->null");
                    }
                    else if (v.OldValue.GetType().FullName.StartsWith("Giso", StringComparison.Ordinal))
                    {
                        string oldValue = v.OldValue.GetType().GetProperty("Id").GetValue(v.OldValue, null).ToString();
                        string newValue = v.NewValue.GetType().GetProperty("Id").GetValue(v.NewValue, null).ToString();
                        res.Append(v.PropertyName).Append("::").Append(oldValue).Append("-->").Append(newValue);
                    }
                    else
                    {
                        res.Append(v.PropertyName).Append("::").Append(v.OldValue).Append("-->").Append(v.NewValue);
                    }
                }
                catch (SqlException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (ArgumentException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (FormatException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NullReferenceException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
                catch (NotSupportedException ex)
                {
                    ExceptionManager.Trace(ex, source);
                }
            }

            return res.ToString();
        }
    }
}
