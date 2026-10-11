using System.Text.Json;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10035)]
public class PropertyList : TradeDirectoryDataView<PropertyListDataProvider.DataRow, PropertyListDataProvider.Param>
{
	private readonly string _destCode;

	private readonly bool _isShowDestCodeColumn;

	public override short DirectoryType => DirectoryTypeSE.PropertyList.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_PropertiesValues";

	public override string Caption
	{
		get
		{
			if (_isShowDestCodeColumn)
			{
				return Resources.PropertiesListsValues;
			}
			PropertyCacheRow item = TradeSettings.TradeCacheService.Property.GetItem(_destCode);
			if (item == null)
			{
				throw new ASException(string.Format(Resources.ValueDoesNotExistInTheListWithName, Resources.AttributeType, _destCode, Resources.MTPropTypes));
			}
			return string.Format(Resources.MTPropValue, item.Code, item.Caption);
		}
	}

	public PropertyList(string destCode = "")
	{
		_destCode = destCode;
		_isShowDestCodeColumn = string.IsNullOrWhiteSpace(_destCode);
		DataProvider = new PropertyListDataProvider(TradeSettings.CreateTradeClient());
		if (!_isShowDestCodeColumn && TradeSettings.TradeCacheService.Property.GetItem(_destCode) == null)
		{
			throw new ASException(string.Format(Resources.ValueDoesNotExistInTheListWithName, Resources.AttributeType, _destCode, Resources.MTPropTypes));
		}
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnVisible("DestCode", string.IsNullOrWhiteSpace(_destCode));
			if (_isShowDestCodeColumn)
			{
				dvp.Set_ColumnSortOrder("DestCode");
			}
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		base.ApplyParametersToDataProvider();
		ParametersProvider.MyDestCode = _destCode;
	}

	public override void Add()
	{
		PropertyListDataProvider.DataRow dataRow = (PropertyListDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		PropertyListDialogWindow propertyListDialogWindow = new PropertyListDialogWindow(DialogWindowMode.NewModeWithNext)
		{
			DVPId = ParentViewPanel.Id,
			IsFilteredProperty = !string.IsNullOrWhiteSpace(_destCode)
		};
		propertyListDialogWindow.Directory.DestCode = (propertyListDialogWindow.IsFilteredProperty ? _destCode : (dataRow?.DestCode ?? ""));
		propertyListDialogWindow.ShowDialog();
	}

	public override void CreateCopy()
	{
		object key = GetKey(ParentViewPanel.FocusedRow());
		PropertyListDialogWindow propertyListDialogWindow = new PropertyListDialogWindow(Settings.DirectoryService.CreateCopy<ArmSoft.SME.Trade.Common.Models.Directories.PropertyList>(DirectoryType, JsonSerializer.Serialize(key)), DialogWindowMode.CreateCopyModeWithNext);
		propertyListDialogWindow.DVPId = ParentViewPanel.Id;
		propertyListDialogWindow.IsFilteredProperty = !string.IsNullOrWhiteSpace(_destCode);
		propertyListDialogWindow.ShowDialog();
	}
}
