// -----------------------------------------------------------------------
// <copyright file="DataParameter.cs" company="Sbrinna">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace GisoFramework.DataAccess
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
using GisoFramework.Item;

    /// <summary>
    /// Create a SqlParameter
    /// </summary>
    public static class DataParameter
    {
        /// <summary>
        /// Gets a input SqlParameter with null value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter InputNull(string name)
        {
            SqlParameter res = new SqlParameter(SetName(name), SqlDbType.NVarChar);
            res.Direction = ParameterDirection.Input;
            res.Value = DBNull.Value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with string value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">string value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, string value)
        {
            SqlParameter res = new SqlParameter(SetName(name), SqlDbType.NVarChar);
            res.Direction = ParameterDirection.Input;
            if (string.IsNullOrEmpty(value))
            {
                res.Value = string.Empty;
            }
            else
            {
                res.Value = value;
            }

            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with string value with length limit
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">String value</param>
        /// <param name="length">Limit length of string value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, string value, int length)
        {
            SqlParameter res = new SqlParameter(SetName(name), SqlDbType.NVarChar);
            res.Direction = ParameterDirection.Input;
            res.Value = Tools.LimitedText(value, length);
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with integer value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, int value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Int);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with nullable integer value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, int? value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Int);
            res.Direction = ParameterDirection.Input;

            if (value.HasValue)
            {
                res.Value = value.Value;
            }
            else
            {
                res.Value = DBNull.Value;
            }

            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with nullable integer value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, decimal? value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Decimal);
            res.Direction = ParameterDirection.Input;

            if (value.HasValue)
            {
                res.Value = value.Value;
            }
            else
            {
                res.Value = DBNull.Value;
            }

            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with float value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Float value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, float value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.BigInt);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with long value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Long value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, long value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.BigInt);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with decimal value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, decimal value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Decimal);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with boolean value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Boolean value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, bool value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Bit);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with datetime value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, DateTime value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.DateTime);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with nullable DateTime value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, DateTime? value)
        {
            if (!value.HasValue)
            {
                return InputNull(name);
            }

            return Input(name, value.Value);
        }

        /// <summary>
        /// Gets a input SqlParameter with text value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Text value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter InputText(string name, string value)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Text);
            res.Direction = ParameterDirection.Input;
            res.Value = value;
            return res;
        }

        /// <summary>
        /// Gets a input SqlParameter with BaseItem value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="item">BaseItem value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, BaseItem item)
        {
            if (item == null)
            {
                return InputNull(name);
            }

            return Input(name, item.Id);
        }

        /// <summary>
        /// Gets a output SqlParameter with string value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="size">Size of returned value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputString(string name, int size)
        {
            SqlParameter res = new SqlParameter(SetName(name), SqlDbType.NVarChar, size);
            res.Direction = ParameterDirection.Output;
            res.Value = DBNull.Value;
            return res;
        }

        /// <summary>
        /// Gets a output SqlParameter with intger value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputInt(string name)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.Int);
            res.Direction = ParameterDirection.Output;
            res.Value = DBNull.Value;
            return res;
        }

        /// <summary>
        /// Gets a output SqlParameter with long value
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputLong(string name)
        {
            SqlParameter res = new SqlParameter(name, SqlDbType.BigInt);
            res.Direction = ParameterDirection.Output;
            res.Value = DBNull.Value;
            return res;
        }

        /// <summary>
        /// Determines if parameter name starts with "@", else adds "@" before name
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <returns>normalized parameter name</returns>
        private static string SetName(string name)
        {
            if (!name.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                name = "@" + name;
            }

            return name;
        }
    }
}
