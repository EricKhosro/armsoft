using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Attributes;

public class RequestTypeAttribute : Attribute
{
	public string Permission { get; set; }

	public RequestObjectType ObjectType { get; set; }
}
