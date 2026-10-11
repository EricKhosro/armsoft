namespace ArmSoft.SME.Common.PersistableObject;

public static class DefinitionExtensions
{
	public static void AddFieldDefinition(this PODefinition definition, FieldDefinition fieldDefinition)
	{
		definition.Fields.Add(fieldDefinition.Name, fieldDefinition);
	}

	public static void AddFieldDefinition(this GridDefinition definition, FieldDefinition fieldDefinition)
	{
		definition.Columns.Add(fieldDefinition.Name, fieldDefinition);
	}

	public static void AddGridDefinition(this PODefinition definition, GridDefinition fieldDefinition)
	{
		definition.Grids.Add(fieldDefinition.Name, fieldDefinition);
	}
}
