using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Core.UI.MetaData;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Core.UI.DataViews;

public interface IDropDownView : IView
{
	bool KeyMemberSupported { get; }

	bool DialogSupported { get; }

	bool AddNewSupported { get; }

	bool EditSupported { get; }

	bool FilteredSearchSupported { get; }

	bool AddNewEnabled { get; }

	bool EditEnabled { get; }

	bool FilteredSearchEnabled { get; }

	bool ClosedFilterSupported { get; }

	string ClosedColumnName { get; }

	ReloadMode ReloadMode { get; }

	string ValueDoesNotExistInTheListMessage { get; }

	List<AsFormatting> ConditionalFormattingList { get; }

	short ValueMemberLength();

	bool Validate(string value, ref string comment, ref string realValue);

	string ValueMember(int index);

	string CommentMember(int index);

	object KeyMember(int index);

	bool ShowDialog();

	bool AddNew(ref string value, ref string comment, Window owner, double top, double left);

	bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow);

	bool FilteredSearch(ref string value, ref string comment, Window owner, double top, double left);

	object GetRowByValue(string value);

	List<string> ValuesList();

	void ReloadRows(LoadSource source = LoadSource.None);

	void BeforeLoadData(GridControl grid);

	void AfterLoadData(GridControl grid);

	void ApplyDefaultDataViewDefinition();

	void ApplyDataViewDefinition(DataViewDefinition dvd);

	ExtendableRow GetSelectedRow(int rowIndex);

	Dictionary<string, string> GetKeyValuesDict();
}
