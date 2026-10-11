using Ardalis.SmartEnum;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class MTQntUnits : DropDownView<MTQntUnitDataProvider.DataRow, MTQntUnitDataProvider.Param>
{
	public override string Caption
	{
		get
		{
			if (GoodCode == "")
			{
				return Resources.MTQntUnits;
			}
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(GoodCode);
			if (item == null)
			{
				return Resources.MTQntUnits;
			}
			return string.Format(Resources.MaterialQUNITS, item.MTCode, item.Caption);
		}
	}

	public override string Name => "MTQntUnits";

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool KeyMemberSupported => true;

	public string GoodCode { get; set; }

	public string GroupCode { get; set; }

	public bool IsShowOnlyComplects { get; set; }

	public ItemTypesSE ItemType { get; set; }

	public MTQntUnits(string goodCode = "", bool isShowQntUnitCaption = false, bool isShowOnlyComplects = false, string group = "", string itemType = "0", bool showImageExists = true)
	{
		GoodCode = goodCode;
		IsShowOnlyComplects = isShowOnlyComplects;
		GroupCode = group;
		ItemType = SmartEnum<ItemTypesSE, string>.FromValue(itemType);
		DataProvider = new MTQntUnitDataProvider();
		FillColumnsFromDataProvider();
		bool visible = string.IsNullOrWhiteSpace(goodCode);
		Columns["MTCode"].Visible = visible;
		Columns["MTCaption"].Visible = visible;
		Columns["ImageExists"].Visible = showImageExists;
		Columns.AddSorting("QntUnit");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = GoodCode;
		ParametersProvider.GroupCode = GroupCode;
		ParametersProvider.IsShowOnlyComplects = IsShowOnlyComplects;
		ParametersProvider.ItemType = ItemType;
	}

	public override short ValueMemberLength()
	{
		return 3;
	}

	public override string ValueMember(int index)
	{
		return Data[index].QntUnit;
	}

	public override string CommentMember(int index)
	{
		return Data[index].QntUnitCaption;
	}

	public override object KeyMember(int index)
	{
		return new MTQntUnitDataProvider.DataRow.Key
		{
			MTID = Data[index].MTID,
			QntUnit = Data[index].QntUnit
		};
	}
}
