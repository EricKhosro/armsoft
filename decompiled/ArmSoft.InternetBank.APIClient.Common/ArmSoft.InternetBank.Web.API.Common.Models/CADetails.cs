using System.Collections.Generic;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class CADetails : CABase
{
	public List<CADetailsField> CustomFields { get; set; }
}
