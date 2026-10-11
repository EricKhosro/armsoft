using System;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OpenBanking.Api.Common;

public class DocumentForEdit<TModel> : SignableObjectForEdit<TModel> where TModel : Document
{
	public bool CanBeCanceled { get; set; }

	[Obsolete("Use CommissionStatus instead.Remove when mobile min version is 8.13.0.")]
	public bool IsCommissionAvailable { get; set; }

	public CommissionStatus CommissionStatus { get; set; }
}
