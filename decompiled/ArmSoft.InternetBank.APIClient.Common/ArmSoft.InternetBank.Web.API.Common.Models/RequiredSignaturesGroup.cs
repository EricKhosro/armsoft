using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class RequiredSignaturesGroup
{
	public List<RequiredSignatureRow> Rows { get; set; }
}
