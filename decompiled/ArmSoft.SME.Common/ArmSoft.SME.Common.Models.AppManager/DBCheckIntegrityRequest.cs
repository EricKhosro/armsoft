using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.AppManager;

public class DBCheckIntegrityRequest
{
	public List<short> Elements { get; set; } = new List<short>();
}
