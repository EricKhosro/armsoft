using System;
using System.Collections.Generic;

namespace ArmSoft.SME.Common.Models.Dialog;

public class DialogValuesTemplate : ICloneable
{
	public int? UserId { get; set; }

	public int? Id { get; set; }

	public string Caption { get; set; }

	public Dictionary<string, string> Values { get; set; }

	public object Clone()
	{
		return new DialogValuesTemplate
		{
			UserId = UserId,
			Id = Id,
			Caption = Caption,
			Values = new Dictionary<string, string>(Values)
		};
	}
}
