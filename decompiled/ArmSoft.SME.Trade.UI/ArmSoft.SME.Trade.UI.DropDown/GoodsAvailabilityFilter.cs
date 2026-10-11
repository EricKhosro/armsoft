using System;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsAvailabilityFilter : DropDownView<ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilter.DataRow, ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilter.Param>
{
	public DateOnly AsOfDate { get; set; }

	public string Storage { get; set; }

	public bool ForRetail { get; set; }

	public bool IncludeServices { get; set; }

	public bool ShowAll { get; set; }

	public string Group { get; set; }

	public string PriceListType { get; set; }

	public string MTCode { get; set; }

	public override string Caption
	{
		get
		{
			if (IncludeServices)
			{
				return Resources.MaterialsAndServices;
			}
			if (!IncludeServices)
			{
				return Resources.Goods;
			}
			return DataProvider.GetSchema().Caption;
		}
	}

	public GoodsAvailabilityFilter(DateOnly asOfDate, string storage, bool forRetail, bool includeServices = false, bool showAll = false, string group = "", string priceType = "", bool forGrid = true, bool applyDefaultDataViewDefinition = true, string mtCode = "")
	{
		AsOfDate = asOfDate;
		Storage = storage;
		ForRetail = forRetail;
		IncludeServices = includeServices;
		ShowAll = showAll;
		Group = group;
		PriceListType = priceType;
		MTCode = mtCode;
		DataProvider = new ArmSoft.SME.Trade.Client.Data.DropDowns.GoodsAvailabilityFilter(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		Columns["ItemType"].Visible = false;
		if (!includeServices)
		{
			Columns["ItemTypeCaption"].Visible = false;
		}
		Columns["MTID"].Visible = false;
		Columns["Unit"].Visible = false;
		Columns["VAT"].Visible = false;
		Columns["GroupPath"].Visible = false;
		if (forGrid)
		{
			Columns["Price"].Visible = false;
		}
		Columns.AddSorting("MTCode");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.AsOfDate = AsOfDate;
		ParametersProvider.Storage = Storage;
		ParametersProvider.ForRetail = ForRetail;
		ParametersProvider.IncludeServices = IncludeServices;
		ParametersProvider.ShowAll = ShowAll;
		ParametersProvider.Group = Group;
		ParametersProvider.PriceListType = PriceListType;
		ParametersProvider.MTCode = MTCode;
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
		return Data[index].MTCode;
	}
}
