using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;

namespace ArmSoft.SME.Core.UI.ECR;

public class Department
{
	public int Id { get; set; }

	public string Name { get; set; }

	public int Type { get; set; }

	public string DepartmentType => Type switch
	{
		1 => Resources.ECRDepTypeWithVAT, 
		2 => Resources.ECRDepTypeWithOutVAT, 
		3 => Resources.ECRDepTypeTurnOver, 
		4 => Resources.ECRDepTypePatent, 
		5 => Resources.ECRDepTypeFixed, 
		6 => Resources.ECRDepTypeFamily, 
		7 => Resources.ECRDepTypeMicro, 
		_ => Type.ToString(), 
	};

	public Department(Utilities.Department d)
	{
		Id = d.id;
		Name = d.name;
		Type = d.type;
	}
}
