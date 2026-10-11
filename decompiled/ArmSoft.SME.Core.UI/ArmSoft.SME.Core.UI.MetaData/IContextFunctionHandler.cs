using System.Collections.Generic;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IContextFunctionHandler
{
	object InnerHandler { get; }

	void SetRows(List<IDynamicValue> rows);

	void UserDefinedContextFunction(IDataViewForExtensions idv);
}
