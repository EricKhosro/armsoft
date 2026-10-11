using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(10001)]
public class GoodsFilter : DropDownView<GoodsFilterDataProvider.DataRow, GoodsFilterDataProvider.Param>
{
	private PartnerCacheRow _partner;

	public ItemTypesSE ItemType { get; set; }

	public string Group { get; set; }

	public bool IsShowOnlyInPriceList { get; set; }

	public string LikeName { get; set; }

	public string PartnerCode { get; set; }

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(PartnerCode))
			{
				if (ItemType == ItemTypesSE.Goods)
				{
					return ArmSoft.SME.Trade.Common.Resources.Goods;
				}
				if (ItemType == ItemTypesSE.Services)
				{
					return ArmSoft.SME.Common.Resources.Services;
				}
				return ArmSoft.SME.Trade.Common.Resources.MaterialsAndServices;
			}
			return string.Format(ArmSoft.SME.Common.Resources.PartnerMTDirectory, PartnerCode, _partner.Caption);
		}
	}

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Goods).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Goods).AllowEdit;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override string HelpId => "Purchases_Lists_ProductsandServices";

	public GoodsFilter(bool shortMode = true, string itemType = "0", string group = "", bool isShowOnlyInPriceList = false, string likeName = "", bool applyDefaultDataViewDefinition = true, string partnerCode = "")
	{
		ItemType = SmartEnum<ItemTypesSE, string>.FromValue(itemType);
		Group = group;
		LikeName = likeName;
		PartnerCode = partnerCode;
		IsShowOnlyInPriceList = isShowOnlyInPriceList;
		DataProvider = new GoodsFilterDataProvider();
		if (!string.IsNullOrWhiteSpace(partnerCode))
		{
			_partner = TradeSettings.TradeCacheService.Partner.GetItem(partnerCode, throwExceptionNotExits: true);
		}
		FillColumnsFromDataProvider();
		if (ItemType != ItemTypesSE.All)
		{
			Columns["TypeName"].Visible = false;
		}
		Columns["GroupCaption"].Visible = !shortMode;
		Columns["AltUnitBrief"].Visible = !shortMode;
		Columns["Country"].Visible = !shortMode;
		Columns["ExpenseMethodName"].Visible = !shortMode;
		Columns["VAT"].Visible = !shortMode;
		Columns["ShowInPriceList"].Visible = !shortMode;
		Columns["FullCaption"].Visible = !shortMode;
		Columns["IsWeight"].Visible = !shortMode;
		Columns["CheckMarking"].Visible = !shortMode && Settings.ParametersService.UseMarking();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ItemType = ItemType.Value;
		ParametersProvider.Group = Group;
		ParametersProvider.IsShowOnlyInPriceList = IsShowOnlyInPriceList;
		ParametersProvider.LikeName = LikeName;
		ParametersProvider.PartnerCode = PartnerCode;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].MTCode;
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
		if (isNew)
		{
			if (Settings.ParametersService.GoodAddSimpleMode())
			{
				GoodDocumentSimpleWindow goodDocumentSimpleWindow = new GoodDocumentSimpleWindow();
				goodDocumentSimpleWindow.Owner = owner;
				goodDocumentSimpleWindow.Top = top;
				goodDocumentSimpleWindow.Left = left;
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
				GoodDocumentWindow goodDocumentWindow = new GoodDocumentWindow();
				goodDocumentWindow.Owner = owner;
				goodDocumentWindow.Top = top;
				goodDocumentWindow.Left = left;
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
			ArmSoft.SME.Common.Models.Directories.Directory directory = Settings.DirectoryService.Load(DirectoryTypeSE.Goods, value);
			if (Settings.ParametersService.GoodAddSimpleMode())
			{
				GoodDocumentSimpleWindow goodDocumentSimpleWindow2 = new GoodDocumentSimpleWindow((Good)directory);
				goodDocumentSimpleWindow2.Owner = owner;
				goodDocumentSimpleWindow2.Top = top;
				goodDocumentSimpleWindow2.Left = left;
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
				GoodDocumentWindow goodDocumentWindow2 = new GoodDocumentWindow((Good)directory);
				goodDocumentWindow2.Owner = owner;
				goodDocumentWindow2.Top = top;
				goodDocumentWindow2.Left = left;
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

	public override bool Validate(string value, ref string comment, ref string realValue)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(value);
		if (item == null || (ItemType != ItemTypesSE.All && item.Type != ItemType.Value))
		{
			return false;
		}
		comment = item.Caption;
		realValue = item.MTCode;
		return true;
	}
}
