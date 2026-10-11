namespace ArmSoft.SME.Common.Models.Data;

public class ColumnOrder
{
	public string Name { get; set; }

	public bool Descending { get; set; }

	public ColumnOrder()
	{
		Descending = false;
	}
}
