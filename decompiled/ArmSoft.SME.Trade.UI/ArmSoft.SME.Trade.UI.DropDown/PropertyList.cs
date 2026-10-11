using System.Text.Json;
using System.Windows;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class PropertyList : DropDownView<PropertyListDataProvider.DataRow, PropertyListDataProvider.Param>
{
	private readonly bool _isFromCode;

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(DestCode))
			{
				return Resources.PropertiesListsValues;
			}
			PropertyCacheRow item = TradeSettings.TradeCacheService.Property.GetItem(DestCode);
			if (item == null)
			{
				return Resources.PropertiesListsValues;
			}
			return string.Format(Resources.MTPropValue, item.Code, item.Caption);
		}
	}

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.PropertyList).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.PropertyList).AllowEdit;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public string DestCode { get; set; }

	public PropertyList(string destCode = "", bool isFromCode = false)
	{
		DestCode = destCode;
		_isFromCode = isFromCode;
		DataProvider = new PropertyListDataProvider();
		FillColumnsFromDataProvider();
		Columns["DestCode"].Visible = false;
		Columns.AddSorting("Code");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MyDestCode = DestCode;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Code;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left)
	{
		bool result = false;
		PropertyListDialogWindow propertyListDialogWindow;
		if (isNew)
		{
			propertyListDialogWindow = (PropertyListDialogWindow)Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.PropertyList, DialogWindowMode.NewMode);
			propertyListDialogWindow.Directory.DestCode = DestCode;
		}
		else
		{
			PropertyListDataProvider.DataRow.Key value2 = new PropertyListDataProvider.DataRow.Key
			{
				DestCode = DestCode,
				Code = (_isFromCode ? value : TradeSettings.TradeCacheService.PropertyList.GetPropertyByCaption(value, DestCode)?.Code)
			};
			propertyListDialogWindow = (PropertyListDialogWindow)Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.PropertyList, JsonSerializer.Serialize(value2)), DialogWindowMode.EditMode);
		}
		propertyListDialogWindow.IsFilteredProperty = !string.IsNullOrWhiteSpace(DestCode);
		propertyListDialogWindow.Owner = owner;
		propertyListDialogWindow.Top = top;
		propertyListDialogWindow.Left = left;
		if (propertyListDialogWindow.ShowDialog().Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			if (_isFromCode)
			{
				value = propertyListDialogWindow.Directory.Code;
				comment = propertyListDialogWindow.Directory.Name;
			}
			else
			{
				value = propertyListDialogWindow.Directory.Name;
				comment = propertyListDialogWindow.Directory.Code;
			}
		}
		return result;
	}
}
