using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MultipleUtilityCommission
{
	public List<Commission> UtilityCommisions;

	public decimal TotalAmount;
}
