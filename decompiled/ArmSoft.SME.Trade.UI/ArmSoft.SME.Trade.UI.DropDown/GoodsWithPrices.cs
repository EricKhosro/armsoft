using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsWithPrices : DropDownView<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsWithPrices.DataRow, ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsWithPrices.Param>
{
	private PartnerCacheRow _partner;

	public DateOnly Date { get; set; }

	public string PriceType { get; set; }

	public string LikeName { get; set; }

	public string PartCode { get; set; }

	public string ItemType { get; set; }

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Goods.Value).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Goods.Value).AllowEdit;

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(PartCode))
			{
				return ArmSoft.SME.Trade.Common.Resources.Goods;
			}
			return string.Format(ArmSoft.SME.Common.Resources.PartnerMTDirectory, PartCode, _partner.Caption);
		}
	}

	public GoodsWithPrices(DateOnly date, string priceType, string likeName, string partCode = "", string itemType = "")
	{
		Date = date;
		PriceType = priceType;
		LikeName = likeName;
		PartCode = partCode;
		ItemType = itemType;
		if (!string.IsNullOrWhiteSpace(partCode))
		{
			_partner = TradeSettings.TradeCacheService.Partner.GetItem(partCode, throwExceptionNotExits: true);
		}
		DataProvider = new ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsWithPrices(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		Columns["GroupPath"].Visible = false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Date = Date;
		ParametersProvider.PriceType = PriceType;
		ParametersProvider.LikeName = LikeName;
		ParametersProvider.PartCode = PartCode;
		ParametersProvider.ItemType = ItemType;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].GoodCode;
	}

	public override string CommentMember(int index)
	{
		return Data[index].Caption;
	}

	public override bool Validate(string value, ref string comment, ref string realValue)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(value);
		if (item == null)
		{
			return false;
		}
		comment = item.Caption;
		realValue = item.MTCode;
		return true;
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
		if (isNew)
		{
			if (Settings.ParametersService.GoodAddSimpleMode())
			{
				GoodDocumentSimpleWindow goodDocumentSimpleWindow = new GoodDocumentSimpleWindow
				{
					Owner = owner,
					Top = top,
					Left = left
				};
				if (goodDocumentSimpleWindow.ShowDialog(WindowViewMode.NewCreate).Value)
				{
					DataLoadedAtLeastOnce = false;
					result = true;
					value = goodDocumentSimpleWindow.PO.Code;
					comment = goodDocumentSimpleWindow.PO.Name;
				}
			}
			else
			{
				GoodDocumentWindow goodDocumentWindow = new GoodDocumentWindow
				{
					Owner = owner,
					Top = top,
					Left = left
				};
				if (goodDocumentWindow.ShowDialog(WindowViewMode.NewCreate).Value)
				{
					DataLoadedAtLeastOnce = false;
					result = true;
					value = goodDocumentWindow.PO.Code;
					comment = goodDocumentWindow.PO.Name;
				}
			}
		}
		else
		{
			ArmSoft.SME.Common.Models.Directories.Directory directory = Settings.DirectoryService.Load(DirectoryTypeSE.Goods.Value, value);
			if (Settings.ParametersService.GoodAddSimpleMode())
			{
				GoodDocumentSimpleWindow goodDocumentSimpleWindow2 = new GoodDocumentSimpleWindow((Good)directory)
				{
					Owner = owner,
					Top = top,
					Left = left
				};
				if (goodDocumentSimpleWindow2.ShowDialog(WindowViewMode.Edit).Value)
				{
					DataLoadedAtLeastOnce = false;
					result = true;
					value = goodDocumentSimpleWindow2.PO.Code;
					comment = goodDocumentSimpleWindow2.PO.Name;
				}
			}
			else
			{
				GoodDocumentWindow goodDocumentWindow2 = new GoodDocumentWindow((Good)directory)
				{
					Owner = owner,
					Top = top,
					Left = left
				};
				if (goodDocumentWindow2.ShowDialog(WindowViewMode.Edit).Value)
				{
					DataLoadedAtLeastOnce = false;
					result = true;
					value = goodDocumentWindow2.PO.Code;
					comment = goodDocumentWindow2.PO.Name;
				}
			}
		}
		return result;
	}
}
