<CONFIG>
	NAME DBTest
	NODE SpecificationSource
	TOPLEVEL False
</CONFIG>
<CONTENT>
<%%:
// this is merely a test, no content is needed for this template!
foreach(SqlTable)
{
	trace("\r\nSqlTableName is: " + SqlTableName, "c:\\temp\\test.txt")
<%%-
Table:%%><%%=SqlTableName%%>
	foreach (SqlColumn)
	{
	<%%-
	Column:%%><%%=SqlColumnName%%><%%-, %%><%%=InPrimaryKey%%>
	CurrentSqlColumn = SqlColumn
	}
	<%%-
	Column:%%><%%=CurrentSqlColumn.SqlColumnName%%>
	CurrentSqlColumn = null
	<%%-
	Column:%%><%%=CurrentSqlColumn.SqlColumnName%%>
	foreach (SqlIndex)
	{
	<%%-
	Index:%%><%%=SqlIndexName%%>
		foreach (SqlIndexedColumn)
		{
		<%%-
		Index Column:%%><%%=SqlIndexedColumnName%%>
		}
	}
	foreach (SqlForeignKey)
	{
	<%%-
	Foreign Key:%%><%%=SqlForeignKeyName%%>
		foreach (SqlForeignKeyColumn)
		{
		<%%-
		Foreign Key Column:%%><%%=SqlForeignKeyColumnName%%><%%-, %%><%%=ReferencedColumn%%>
		}
	}
	foreach (SqlProperty)
	{
	<%%-
	Property:%%><%%=SqlPropertyName%%><%%-, %%><%%=SqlValue%%>
	}
	foreach (SqlExtendedProperty)
	{
	<%%-
	Extended Property:%%><%%=SqlExtendedPropertyName%%><%%-, %%><%%=SqlValue%%>
	}
}
%%></CONTENT>