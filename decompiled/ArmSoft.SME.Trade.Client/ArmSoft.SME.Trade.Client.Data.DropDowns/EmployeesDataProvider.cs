using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Trade.Client.Models;

namespace ArmSoft.SME.Trade.Client.Data.DropDowns;

public class EmployeesDataProvider : DataProvider<Employee, EmployeesDataProvider.Param>
{
	public class Param
	{
	}

	public override string Name => "Employees";

	public EmployeesDataProvider(TradeClient client)
		: base((ApiClient)client)
	{
	}
}
