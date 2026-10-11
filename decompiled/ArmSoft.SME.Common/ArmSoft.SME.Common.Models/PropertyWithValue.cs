namespace ArmSoft.SME.Common.Models;

public class PropertyWithValue
{
	public string Name { get; set; }

	public string Value { get; set; }

	public PropertyWithValue()
	{
	}

	public PropertyWithValue(string name, string value)
	{
		Name = name;
		Value = value;
	}
}
