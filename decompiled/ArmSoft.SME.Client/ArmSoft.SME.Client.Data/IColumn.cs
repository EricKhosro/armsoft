using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Client.Data;

public interface IColumn
{
	string Name { get; }

	string Caption { get; }

	DataColumnType Type { get; }
}
