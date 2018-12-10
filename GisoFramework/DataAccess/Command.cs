// --------------------------------
// <copyright file="Command.cs" company="OpenFramework">
//     Copyright (c) OpenFramework. All rights reserved.
// </copyright>
// <author>Juan Castilla Calderón - jcastilla@openframework.es</author>
// --------------------------------
namespace GisoFramework.DataAccess
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>Implementation of Command class.</summary>
    public class Command : IDisposable
    {
        /// <summary>SqlCommand to build</summary>
        private SqlCommand command;

        /// <summary>Gets a Sql Command that calls a stored procedure</summary>
        /// <param name="stored">Stored procedure name</param>
        /// <returns>A sql command</returns>
        public SqlCommand Stored(string stored)
        {
            if (!string.IsNullOrEmpty(stored))
            {
                this.command = new SqlCommand
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = stored,
                    Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cns"].ConnectionString)
                };

                return this.command;
            }

            return new SqlCommand();
        }

        /// <summary>Dispose Comand class</summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>The bulk of the clean-up code is implemented in Dispose(bool)</summary>
        /// <param name="disposing">Disposing managed objects</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.command != null)
                {
                    if (this.command.Connection != null)
                    {
                        this.command.Connection.Dispose();
                    }

                    this.command.Dispose();
                }
            }
        }
    }
}