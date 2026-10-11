using System;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Trade.Client.Models;

public class Employee : ExtendableRow
{
	public byte[] TS = new byte[8];

	public int Id { get; set; }

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
}
