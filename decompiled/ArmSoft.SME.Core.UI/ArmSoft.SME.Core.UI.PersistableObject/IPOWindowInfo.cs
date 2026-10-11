using System;

namespace ArmSoft.SME.Core.UI.PersistableObject;

public interface IPOWindowInfo
{
	object GetControlValue(string datItemName);

	Type GetGridRowType(string dataItemName);

	Guid POISN();
}
