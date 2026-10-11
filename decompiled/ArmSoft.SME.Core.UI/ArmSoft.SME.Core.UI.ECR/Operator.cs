using ArmSoft.ECRConnector;

namespace ArmSoft.SME.Core.UI.ECR;

public class Operator
{
	public int Id { get; }

	public string Name { get; }

	public int[] Deps { get; }

	public string AvailableDepartments
	{
		get
		{
			if (Deps != null)
			{
				return string.Join(", ", Deps);
			}
			return string.Empty;
		}
	}

	public Operator(Utilities.Cashier c)
	{
		Id = c.id;
		Name = c.name;
		Deps = c.deps;
	}
}
