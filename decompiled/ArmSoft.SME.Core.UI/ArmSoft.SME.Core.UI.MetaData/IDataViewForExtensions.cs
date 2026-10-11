using System.Collections.Generic;

namespace ArmSoft.SME.Core.UI.MetaData;

public interface IDataViewForExtensions
{
	void RefreshDataView();

	void UpdateDataView(object key);

	List<object> GetDataViewSelectedRows();

	object GetDataViewFocusedRow();
}
