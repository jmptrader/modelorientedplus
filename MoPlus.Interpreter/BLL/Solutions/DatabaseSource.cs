/*<copyright>
Mo+ Solution Builder is a model oriented programming language and IDE, used for building models and generating code and other documents in a model driven development process.

Copyright (C) 2013 Dave Clemmer, Intelligent Coding Solutions, LLC

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
</copyright>*/
using System;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.SqlServer.Management.Common;
using MoPlus.Common;
using MoPlus.Data;
using BLL = MoPlus.Interpreter.BLL;
using MoPlus.Interpreter.BLL.Specifications;
using MoPlus.Interpreter.BLL.Config;
using MoPlus.Interpreter.Resources;
using Microsoft.SqlServer.Management.Smo;
using MySql.Data.MySqlClient;

namespace MoPlus.Interpreter.BLL.Solutions
{
	///--------------------------------------------------------------------------------
	/// <summary></summary>
	///
	/// This file is for adding customizations to the DatabaseSource class
	/// (change the Status below to something other than Generated).
	///
	/// <CreatedByUserName>INCODE-1\Dave</CreatedByUserName>
	/// <CreatedDate>12/21/2012</CreatedDate>
	/// <Status>Customized</Status>
	///--------------------------------------------------------------------------------
	public partial class DatabaseSource : Solutions.SpecificationSource
	{
		#region "Constants"
		#endregion "Constants"
		
		#region "Fields and Properties"
		///--------------------------------------------------------------------------------
		/// <summary>This property gets/sets the name of the instance.</summary>
		///--------------------------------------------------------------------------------
		[XmlIgnore]
		public override string Name
		{
			get
			{
				if (SourceDbServerName == String.Empty && SourceDbName == String.Empty)
				{
					return String.Empty;
				}
				return SourceDbServerName + "." + SourceDbName;
			}
			set
			{
				bool isFirstItem = true;
				char[] splitChar = { '.' };
				foreach (string loopItem in value.Split(splitChar, 2))
				{
					if (isFirstItem == true)
					{
						SourceDbServerName = loopItem;
					}
					else
					{
						SourceDbName = loopItem;
					}
					isFirstItem = false;
				}
			}
		}

		///--------------------------------------------------------------------------------
		/// <summary>This property gets the display name.</summary>
		///--------------------------------------------------------------------------------
		[XmlIgnore]
		public string DisplayName
		{
			get
			{
				return Name;
			}
		}

		private SqlDatabase _specDatabase = null;
		///--------------------------------------------------------------------------------
		/// <summary>This property gets/sets the specification database.</summary>
		///--------------------------------------------------------------------------------
		[XmlIgnore]
		public SqlDatabase SpecDatabase
		{
			get
			{
				if (_specDatabase == null)
				{
					LoadSpecificationSource();
				}
				if (_specDatabase != null && _specDatabase.DatabaseSource == null)
				{
					_specDatabase.DatabaseSource = this;
				}
				return _specDatabase;
			}
			set
			{
				_specDatabase = value;
			}
		}

		#endregion "Fields and Properties"
		
		#region "Methods"
		///--------------------------------------------------------------------------------
		/// <summary>This method returns a copy of the forward engineering data for the solution.</summary>
		///--------------------------------------------------------------------------------
		public DatabaseSource GetForwardInstance()
		{
			DatabaseSource forwardItem = new DatabaseSource();
			forwardItem.TransformDataFromObject(this, null, false);
			return forwardItem;
		}

		///--------------------------------------------------------------------------------
		/// <summary>This method loads a specification source with information from a
		/// SQL database.</summary>
		///--------------------------------------------------------------------------------
		public void LoadSpecificationSource()
		{
			try
			{
				switch (DatabaseTypeCode)
				{
					case (int)BLL.Config.DatabaseTypeCode.SqlServer:
						Database sqlDatabase = null;

				        Server sqlServer;
				        if (SourceDbServerName.StartsWith("(localdb)", StringComparison.OrdinalIgnoreCase))
				        {
				            var conn = new ServerConnection(new SqlConnection(String.Format("server={0};integrated security=true", SourceDbServerName)));
				            sqlServer = new Server(conn);
                            sqlServer.SetDefaultInitFields(true); 
				        }
				        else
				        {
				            sqlServer = new Server(SourceDbServerName);
				            sqlServer.SetDefaultInitFields(true);
				            if (!String.IsNullOrEmpty(PasswordClearText) && !String.IsNullOrEmpty(UserName))
				            {
				                // sql server authentication
								sqlServer.ConnectionContext.LoginSecure = false;
								sqlServer.ConnectionContext.Login = UserName;
				                sqlServer.ConnectionContext.Password = PasswordClearText;
				            }
				            else
				            {
				                // windows authentication
				                sqlServer.ConnectionContext.LoginSecure = true;
				            }
				        }
				        sqlServer.ConnectionContext.Connect();
						if (sqlServer.ConnectionContext.IsOpen == false)
						{
							SpecDatabase = null;
							ApplicationException ex = new ApplicationException(String.Format(DisplayValues.Exception_SourceDbServerConnection, SourceDbServerName));
							Solution.ShowIssue(ex.Message + "\r\n" + ex.StackTrace);
							throw ex;
						}
                            
				        if (SourceDbName.Contains("\\"))
				        {
                            var dbName = Path.GetFileNameWithoutExtension(SourceDbName);
							sqlDatabase = sqlServer.Databases[dbName];

				            if (sqlDatabase == null)
				            {
				                // attach the database instead of opening the database by name
				                var stringColl = new StringCollection();
				                stringColl.Add(SourceDbName);
				                stringColl.Add(Path.Combine(Path.GetDirectoryName(SourceDbName), Path.GetFileNameWithoutExtension(SourceDbName) + "_log.ldf"));

				                sqlServer.AttachDatabase(dbName, stringColl);

								sqlDatabase = sqlServer.Databases[SourceDbName];
							}
				        }
				        else
				        {
							sqlDatabase = sqlServer.Databases[SourceDbName];
				        }

				        if (sqlDatabase == null)
						{
							SpecDatabase = null;
							ApplicationException ex = new ApplicationException(String.Format(DisplayValues.Exception_SourceDbNotFound, SourceDbName, SourceDbServerName));
							Solution.ShowIssue(ex.Message + "\r\n" + ex.StackTrace);
							throw ex;
						}
						else
						{
							// load the database information
							SpecDatabase = new SqlDatabase();
							SpecDatabase.SqlDatabaseID = Guid.NewGuid();
							SpecDatabase.Solution = Solution;
							SpecDatabase.LoadSqlServerDatabase(sqlDatabase);
						}
						break;
					case (int)BLL.Config.DatabaseTypeCode.MySQL:
						string myConnectionString = "SERVER=" + SourceDbServerName + ";" +
							"DATABASE=" + SourceDbName + ";" +
							"UID=" + UserName + ";" +
							"PASSWORD=" + PasswordClearText + ";";
						MySqlConnection connection = new MySqlConnection(myConnectionString);
						using (connection)
						{
							try
							{
								connection.Open();
							}
							catch
							{
								SpecDatabase = null;
								ApplicationException ex = new ApplicationException(String.Format(DisplayValues.Exception_MySQLConnection, SourceDbName, SourceDbServerName));
								Solution.ShowIssue(ex.Message + "\r\n" + ex.StackTrace);
								throw;
							}
							// load the database information
							SpecDatabase = new SqlDatabase();
							SpecDatabase.SqlDatabaseID = Guid.NewGuid();
							SpecDatabase.Solution = Solution;
							SpecDatabase.LoadMySQLDatabase(connection);
							connection.Close();
						}
						break;
					default:
                        throw new NotImplementedException("DatabaseTypeCode value " + DatabaseTypeCode + " not implemented!");
				}
			}
			catch (ApplicationAbortException)
			{
				throw;
			}
			catch (Exception ex)
			{
				bool reThrow = BusinessConfiguration.HandleException(ex);
				Solution.ShowIssue(ex.ToString());
			}
		}

		#endregion "Methods"
		
		#region "Constructors"
		#endregion "Constructors"
	}
}