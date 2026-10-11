using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.Report;

public class AsRepTableDef
{
	public string TableName { get; set; }

	public List<AsRepColumn> ColumnDefinitions { get; set; }
}
