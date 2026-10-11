using System;
using System.Text.Json;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class MTFavorites : DropDownView<MTFavoritesDataProvider.DataRow, MTFavoritesDataProvider.Param>
{
	private readonly int _userId;

	private readonly bool _onlyGroups;

	private readonly bool _onlyMaterials;

	private readonly string _priceType = string.Empty;

	private readonly DateOnly _date;

	private readonly string _vatType = string.Empty;

	private readonly string _currCode = string.Empty;

	public override bool AddNewSupported
	{
		get
		{
			if (!_onlyGroups)
			{
				return Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTFavoriteMaterial).AllowAdd;
			}
			return Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTFavoritesGroup).AllowAdd;
		}
	}

	public override bool EditSupported
	{
		get
		{
			if (!_onlyGroups)
			{
				return Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTFavoritesGroup).AllowEdit;
			}
			return Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.MTFavoritesGroup).AllowEdit;
		}
	}

	public override string Caption
	{
		get
		{
			if (!_onlyGroups)
			{
				return ArmSoft.SME.Trade.Common.Resources.MTFavorites;
			}
			return ArmSoft.SME.Trade.Common.Resources.FavoriteGroups;
		}
	}

	public MTFavorites(int userId, bool onlyMaterials = true, bool onlyGroups = false, string priceType = "", DateOnly date = default(DateOnly), string vatType = "", string currCode = "")
	{
		DataProvider = new MTFavoritesDataProvider(TradeSettings.CreateTradeClient());
		_userId = userId;
		_onlyGroups = onlyGroups;
		_onlyMaterials = onlyMaterials;
		_priceType = priceType;
		_date = date;
		_vatType = vatType;
		_currCode = currCode;
		Columns.Add("MTCode", ArmSoft.SME.Common.Resources.Code);
		Columns.Add("Caption", ArmSoft.SME.Common.Resources.Name);
		if (!onlyGroups)
		{
			Columns.Add("UnitCode", ArmSoft.SME.Common.Resources.Unit);
			Columns.Add("UnitBrief", ArmSoft.SME.Common.Resources.Brief);
		}
		Columns.Add("HotKey", ArmSoft.SME.Common.Resources.HotKey);
		Columns.Add("OrderNum", ArmSoft.SME.Common.Resources.RowNumber).Visible = false;
		if (!onlyMaterials & !onlyGroups)
		{
			Columns.Add("FavType", "");
		}
		if (onlyMaterials)
		{
			Columns.Add("GroupCode", ArmSoft.SME.Common.Resources.Group);
			Columns.Add("GroupCaption", ArmSoft.SME.Common.Resources.Name);
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.OnlyMaterials = _onlyMaterials;
		ParametersProvider.OnlyGroups = _onlyGroups;
		ParametersProvider.UserId = _userId;
		ParametersProvider.PriceType = _priceType;
		ParametersProvider.Date = _date;
		ParametersProvider.VatType = _vatType;
		ParametersProvider.CurrCode = _currCode;
	}

	public override short ValueMemberLength()
	{
		if (!_onlyGroups)
		{
			return 24;
		}
		return 5;
	}

	public override string ValueMember(int index)
	{
		return Data[index].MTCode + ((!_onlyGroups) ? ("!" + Data[index].UnitCode) : string.Empty);
	}

	public override string CommentMember(int index)
	{
		if (!_onlyGroups)
		{
			return Data[index].HotKey;
		}
		return Data[index].Caption;
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left, (MTFavoritesDataProvider.DataRow)selectedRow);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left, MTFavoritesDataProvider.DataRow selectedRow = null)
	{
		bool result = false;
		if (_onlyGroups)
		{
			MTFavoritesGroupDialogWindow mTFavoritesGroupDialogWindow = (isNew ? ((MTFavoritesGroupDialogWindow)Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.MTFavoritesGroup, DialogWindowMode.NewMode)) : ((MTFavoritesGroupDialogWindow)Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.MTFavoritesGroup, JsonSerializer.Serialize(new MTFavoritesDataProvider.DataRow.MTFavoritesRowKey
			{
				UserID = _userId,
				MTID = selectedRow.MTID
			})), DialogWindowMode.EditMode)));
			mTFavoritesGroupDialogWindow.Owner = owner;
			mTFavoritesGroupDialogWindow.Top = top;
			mTFavoritesGroupDialogWindow.Left = left;
			mTFavoritesGroupDialogWindow.Directory.UserId = _userId;
			if (mTFavoritesGroupDialogWindow.ShowDialog().Value)
			{
				DataLoadedAtLeastOnce = false;
				result = true;
				value = mTFavoritesGroupDialogWindow.Directory.Code;
				comment = mTFavoritesGroupDialogWindow.Directory.Name;
			}
		}
		else
		{
			MTFavoriteMaterialDialogWindow mTFavoriteMaterialDialogWindow = (isNew ? ((MTFavoriteMaterialDialogWindow)Settings.DirectoryService.CreateDialogWindow(DirectoryTypeSE.MTFavoriteMaterial, DialogWindowMode.NewMode)) : ((MTFavoriteMaterialDialogWindow)Settings.DirectoryService.CreateDialogWindow(Settings.DirectoryService.Load(DirectoryTypeSE.MTFavoriteMaterial, value), DialogWindowMode.EditMode)));
			mTFavoriteMaterialDialogWindow.Owner = owner;
			mTFavoriteMaterialDialogWindow.Top = top;
			mTFavoriteMaterialDialogWindow.Left = left;
			mTFavoriteMaterialDialogWindow.Directory.UserId = _userId;
			if (mTFavoriteMaterialDialogWindow.ShowDialog().Value)
			{
				DataLoadedAtLeastOnce = false;
				result = true;
				value = mTFavoriteMaterialDialogWindow.Directory.Code;
				comment = mTFavoriteMaterialDialogWindow.Directory.Name;
			}
		}
		return result;
	}
}
