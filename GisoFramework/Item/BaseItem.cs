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
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using GisoFramework.Activity;

    /// <summary>Implements BaseItem class</summary>
    public abstract class BaseItem
    {
        /// <summary>Gets or sets the item identifier</summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the item's company identifier</summary>
        public int CompanyId { get; set; }

        /// <summary>Gets or sets the item description</summary>
        [DifferenciableAttribute]
        public string Description { get; set; }

        /// <summary>Gets or sets the employee that creates item</summary>
        public ApplicationUser CreatedBy { get; set; }

        /// <summary>Gets or sets the date of creation</summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>Gets or sets the employee that makes the last modification</summary>
        public ApplicationUser ModifiedBy { get; set; }

        /// <summary>Gets or sets the last modification date of item</summary>
        public DateTime ModifiedOn { get; set; }

        public DateTime? Date { get; set; }

        /// <summary>Gets or sets a value indicating whether item is active</summary>
        public bool Active { get; set; }

        /// <summary>Gets or sets a value indicating whether item can be deleted</summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>Gets a JSON structure of item data</summary>        
        public abstract string Json { get; }

        /// <summary>Gets a JSON structure of item key/value</summary>        
        public abstract string JsonKeyValue { get; }

        /// <summary>Gets the HTML code for a link to Item page</summary>
        public abstract string Link { get; }

        /// <summary>Gets the differences compared with anhoter item</summary>
        /// <param name="item1">Item to compare</param>
        /// <returns>String with the differences compared with anhoter item</returns>
        public string Differences(BaseItem item1)
        {
            string source = "Differences(BaseItem)";
            var res = new StringBuilder();
            var variances = new List<Variance>();
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                var propertyAttributes = property.GetCustomAttributes(true);

                if (propertyAttributes.Contains(new DifferenciableAttribute(true)))
                {
                    var variance = new Variance
                    {
                        PropertyName = property.Name,
                        NewValue = property.GetValue(this, null),
                        OldValue = property.GetValue(item1, null)
                    };

                    if (variance.NewValue == null && variance.OldValue == null)
                    {
                        continue;
                    }

                    if (variance.NewValue == null && variance.OldValue != null)
                    {
                        variances.Add(variance);
                    }
                    else if (variance.NewValue != null && variance.OldValue == null)
                    {
                        variances.Add(variance);
                    }
                    else if (variance.OldValue.GetType().FullName.StartsWith("Giso", StringComparison.Ordinal))
                    {
                        string va = variance.NewValue.GetType().GetProperty("Id").GetValue(variance.NewValue, null).ToString();
                        string vb = variance.OldValue.GetType().GetProperty("Id").GetValue(variance.OldValue, null).ToString();
                        if (va != vb)
                        {
                            variances.Add(variance);
                        }
                    }
                    else if (!variance.NewValue.Equals(variance.OldValue))
                    {
                        variances.Add(variance);
                    }
                }
            }

            bool first = true;
            foreach (var variance in variances)
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
                    if (variance.OldValue == null && variance.NewValue == null)
                    {
                        continue;
                    }

                    if (variance.OldValue == null && variance.NewValue != null)
                    {
                        string newValue = variance.NewValue.ToString();
                        res.Append(variance.PropertyName).Append("::null-->").Append(newValue);
                    }
                    else if (variance.OldValue.GetType().Name.ToUpperInvariant().Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        if (variance.NewValue == null)
                        {
                            res.Append(variance.PropertyName).Append(@"::""""-->null");
                        }
                        else
                        {
                            res.Append(variance.PropertyName).AppendFormat(CultureInfo.InvariantCulture, @"::""""-->{0}", variance.NewValue);
                        }
                    }
                    else if (variance.NewValue == null)
                    {
                        string oldValue = variance.OldValue.GetType().GetProperty("Id").GetValue(variance.OldValue, null).ToString();
                        res.Append(variance.PropertyName).Append("::").Append(oldValue).Append("-->null");
                    }
                    else if (variance.OldValue.GetType().FullName.StartsWith("Giso", StringComparison.Ordinal))
                    {
                        string oldValue = variance.OldValue.GetType().GetProperty("Id").GetValue(variance.OldValue, null).ToString();
                        string newValue = variance.NewValue.GetType().GetProperty("Id").GetValue(variance.NewValue, null).ToString();
                        res.Append(variance.PropertyName).Append("::").Append(oldValue).Append("-->").Append(newValue);
                    }
                    else
                    {
                        res.Append(variance.PropertyName).Append("::").Append(variance.OldValue).Append("-->").Append(variance.NewValue);
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