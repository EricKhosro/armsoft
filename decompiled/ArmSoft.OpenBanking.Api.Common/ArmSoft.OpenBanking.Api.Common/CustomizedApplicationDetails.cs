using System.Collections.Generic;

namespace ArmSoft.OpenBanking.Api.Common;

public class CustomizedApplicationDetails : DocumentDetails<CustomizedApplication>
{
	public required Customization Customization { get; set; }

	public required List<CustomizedApplicationDetailsField> CustomFieldsWithDisplayValues { get; set; }
}
