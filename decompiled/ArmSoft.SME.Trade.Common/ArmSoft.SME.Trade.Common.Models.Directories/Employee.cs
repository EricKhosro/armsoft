namespace ArmSoft.SME.Trade.Common.Models.Directories;

public class Employee : TradeDirectory
{
	public string Address { get; set; }

	public string Phone { get; set; }

	public string Passport { get; set; }

	public string EmployeeCard { get; set; }

	public bool IsSalesConsultant { get; set; }
}
