﻿using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace CommandApp
{
	/// <summary>
	/// Filters out collations with NULL id (e.g. UCA-14.0.0) from SHOW COLLATION command
    /// Credit to Jeffraska 
    ///    https://github.com/jeffraska/Jf.MySql.Data.Collations
	/// </summary>
	public sealed class Interceptor : BaseCommandInterceptor
    {
		public override bool ExecuteReader(string sql, CommandBehavior behavior, ref MySqlDataReader returnValue)
		{
            if (!sql.Equals("SHOW COLLATION", StringComparison.OrdinalIgnoreCase))
			{
                return false;
			}

			using var command = ActiveConnection.CreateCommand();
			command.CommandText = "SHOW COLLATION WHERE id IS NOT NULL";
			returnValue = command.ExecuteReader(behavior);
			return true;
		}
	}
}
