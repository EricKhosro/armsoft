using System.Collections.Generic;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public interface IPanelView
{
	bool ShowFilterRow { get; set; }

	int GroupingLevel { get; }

	void ExpandGroups();

	void ExpandGroupLevel(int level);

	void CollapseGroups();

	IEnumerable<ColumnBase> GetVisibleColumns();

	void BestFitColumn(GridColumn col);
}
