using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class MC : AccountPayment
{
	public List<MCRow> Rows { get; set; }
}
