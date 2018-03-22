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

    /// <summary>Create a SqlParameter</summary>
    public static class DataParameter
    {
        /// <summary>Default text length in database</summary>
        public const int DefaultTextLength = 50;

        /// <summary>Gets a input SqlParameter with null value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter InputNull(string name)
        {
            return new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a input SqlParameter with string value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">string value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, string value)
        {
            var res = new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input
            };

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

        /// <summary>Gets a input SqlParameter with string value with length limit</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">String value</param>
        /// <param name="length">Limit length of string value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, string value, int length)
        {
            return new SqlParameter(SetName(name), SqlDbType.NVarChar)
            {
                Direction = ParameterDirection.Input,
                Value = Tools.LimitedText(value, length)
            };
        }

        /// <summary>Gets a input SqlParameter with integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, int value)
        {
            return new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with nullable integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Integer nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, int? value)
        {
            var res = new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Input
            };

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

        /// <summary>Gets a input SqlParameter with nullable integer value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, decimal? value)
        {
            var res = new SqlParameter(name, SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input
            };

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

        /// <summary>Gets a input SqlParameter with float value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Float value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, float value)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with long value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Long value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, long value)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with decimal value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Decimal value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, decimal value)
        {
            return new SqlParameter(name, SqlDbType.Decimal)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with boolean value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Boolean value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, bool value)
        {
            return new SqlParameter(name, SqlDbType.Bit)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with datetime value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, DateTime value)
        {
            return new SqlParameter(name, SqlDbType.DateTime)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with nullable DateTime value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">DateTime nullable value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter Input(string name, DateTime? value)
        {
            if (value == null)
            {
                return InputNull(name);
            }

            return Input(name, value.Value);
        }

        /// <summary>Gets a input SqlParameter with text value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Text value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter InputText(string name, string value)
        {
            return new SqlParameter(name, SqlDbType.Text)
            {
                Direction = ParameterDirection.Input,
                Value = value
            };
        }

        /// <summary>Gets a input SqlParameter with BaseItem value</summary>
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

        /// <summary>Gets a output SqlParameter with string value</summary>
        /// <param name="name">Parameter name</param>
        /// <param name="size">Size of returned value</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputString(string name, int size)
        {
            return new SqlParameter(SetName(name), SqlDbType.NVarChar, size)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a output SqlParameter with intger value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputInt(string name)
        {
            return new SqlParameter(name, SqlDbType.Int)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Gets a output SqlParameter with long value</summary>
        /// <param name="name">Parameter name</param>
        /// <returns>Sql parameter</returns>
        public static SqlParameter OutputLong(string name)
        {
            return new SqlParameter(name, SqlDbType.BigInt)
            {
                Direction = ParameterDirection.Output,
                Value = DBNull.Value
            };
        }

        /// <summary>Determines if parameter name starts with "@", else adds "@" before name</summary>
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