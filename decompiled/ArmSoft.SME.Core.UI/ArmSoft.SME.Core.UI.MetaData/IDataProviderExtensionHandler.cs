using System.Collections;
using System.Collections.Generic;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IDataProviderExtensionHandler
{
	object InnerHandler { get; }

	void SetRow(ExtendableRow row);

	void SetParameters(Dictionary<string, object> parameters);

	void BeforeLoad(bool updateRows);

	bool OnEachRow();

	void EditRows(IEnumerable rows);

	List<AsFormatting> ConditionalFormatting();

	string Caption();

	string CaptionToolTip();
}
