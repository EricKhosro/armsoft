using System.Collections;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Data;

namespace ArmSoft.SME.Core.UI.DataViews;

public interface IView
{
	string Name { get; }

	string HelpId { get; }

	string Caption { get; set; }

	string DataViewGroup { get; }

	ViewColumnsCollection Columns { get; }

	Dictionary<string, DataViewParameter> Parameters { get; }

	object ParametersProvider { get; }

	IList Data { get; }

	string FilterString { get; set; }

	string ConditionalFormatting { get; }

	bool DataLoadedAtLeastOnce { get; set; }

	DataColumnType GetColumnType(string propertyName);

	void LoadData(LoadSource source = LoadSource.None);

	bool TreeViewAllowed(ref bool setIsDefault);

	bool ShowExpanded();

	string KeyFieldName();

	string ParentFieldName();

	string IsLeafFieldName();
}
