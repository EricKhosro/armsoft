using System.Windows;
using ArmSoft.SME.Core.UI.DataViews;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class FilteredSearch
{
	private IView flterDataView;

	private IView searchDataView;

	public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.CenterOwner;

	public string Name { get; set; }

	public string Caption { get; set; }

	public string FilterKeyFieldName { get; set; }

	public string FilterParentFieldName { get; set; }

	public IView FilterDataView
	{
		get
		{
			return flterDataView;
		}
		set
		{
			flterDataView = value;
			FilterKeyFieldName = flterDataView.KeyFieldName();
			FilterParentFieldName = flterDataView.ParentFieldName();
		}
	}

	public IView SearchDataView
	{
		get
		{
			return searchDataView;
		}
		set
		{
			searchDataView = value;
			Name = searchDataView.Name;
			Caption = searchDataView.Caption;
		}
	}

	public IView FilteredSearchDataView { get; set; }

	public IResultList ResultsList { get; set; }
}
