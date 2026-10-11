using System;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common.Parameters;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsAvailabilityFilterForInvoices : DropDownView<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.DataRow, ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices.Param>
{
	public DateOnly AsOfDate { get; set; }

	public string Storage { get; set; }

	public bool Retail { get; set; }

	public string PriceType { get; set; }

	public string LikeName { get; set; }

	public bool IsFilteredSearchView { get; set; }

	public string SubstituteItemsGroup { get; set; }

	public bool ExtendedView { get; set; }

	public bool ShowAll { get; set; }

	public override string Name => "GoodsWithAvailabilityAndPricesList";

	public GoodsAvailabilityFilterForInvoices(DateOnly asOfDate, string storage, string priceType, bool retail, string likeName = "", string substituteItemsGroup = "", bool extendedView = true, bool isFilteredSearchView = false, bool showAvailability = true, bool showAll = true)
	{
		AsOfDate = asOfDate;
		Storage = storage;
		PriceType = priceType;
		Retail = retail;
		LikeName = likeName;
		SubstituteItemsGroup = substituteItemsGroup;
		ExtendedView = extendedView;
		IsFilteredSearchView = isFilteredSearchView;
		ShowAll = showAll;
		DataProvider = new ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilterForInvoices(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		if (!showAvailability)
		{
			Columns["Quantity"].Visible = false;
		}
		if (!Settings.ParametersService.EnableSubstituteItems())
		{
			Columns["SubstituteItemsGroup"].Visible = false;
		}
		if (!Settings.ParametersService.ShowBarCodeInAvailList())
		{
			Columns["BarCode"].Visible = false;
		}
		if (isFilteredSearchView)
		{
			Columns["ItemType"].Visible = false;
			Columns["SubstituteItemsGroup"].Visible = false;
		}
		Columns["GroupPath"].Visible = false;
		Columns.AddSorting("MTCode");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.AsOfDate = AsOfDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.PriceType = PriceType;
		ParametersProvider.Retail = Retail;
		ParametersProvider.LikeName = LikeName;
		ParametersProvider.SubstituteItemsGroup = SubstituteItemsGroup;
		ParametersProvider.ExtendedView = ExtendedView;
		ParametersProvider.IsFilteredSearchView = IsFilteredSearchView;
		ParametersProvider.ShowAll = ShowAll;
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
}
