using System;
using System.Text.Json;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Trade.Common.Models.Caches;

public class EmployeeCacheRow : CacheRowBaseWithIdName<string>
{
	public string EmployeeCode { get; set; }

	public string Caption { get; set; }

	public string Address { get; set; }

	public string Phone { get; set; }

	public string Passport { get; set; }

	public string EmployeeCard { get; set; }

	public bool IsSalesConsultant { get; set; }

	public string UserName { get; set; }

	public DateTime? ChangeDate { get; set; }

	public DateTime? CreationDate { get; set; }

	public override string KeyJSON => JsonSerializer.Serialize(EmployeeCode);

	public EmployeeCacheRow(int employeeID, string employeeCode, string caption, string address, string phone, string passport, string employeeCard, bool isSalesConsultant, byte[] ts)
	{
		Id = employeeID;
		EmployeeCode = employeeCode;
		Caption = caption;
		Address = address;
		Phone = phone;
		Passport = passport;
		EmployeeCard = employeeCard;
		IsSalesConsultant = isSalesConsultant;
		TS = ts;
	}

	public EmployeeCacheRow()
	{
	}
}
