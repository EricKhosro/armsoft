using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models;

public class WarningModel
{
	public string Message { get; set; }

	public List<string> Details { get; set; }

	public WarningModel()
	{
		Details = new List<string>();
	}

	public WarningModel(string message, List<string> details = null)
	{
		Message = message;
		Details = details ?? new List<string>();
	}
}
