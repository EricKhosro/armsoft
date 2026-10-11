using System.Collections.Generic;
using ArmSoft.SME.Core.UI.Controls;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public interface IResultList
{
	List<FilteredSearchGridColumn> Columns { get; }

	IEnumerable<DocumentGridRow> ItemsSource { get; }

	int AddToItemsSource(object key, params object[] args);
}
