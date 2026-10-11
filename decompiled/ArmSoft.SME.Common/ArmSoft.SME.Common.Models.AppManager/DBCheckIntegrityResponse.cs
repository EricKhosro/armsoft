using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.AppManager;

public class DBCheckIntegrityResponse
{
	public List<GrouppedErrors> ErrorMessages { get; set; } = new List<GrouppedErrors>();
}
