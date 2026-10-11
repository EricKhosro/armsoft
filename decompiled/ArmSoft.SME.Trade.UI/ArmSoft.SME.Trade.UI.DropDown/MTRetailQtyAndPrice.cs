using System;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.DropDowns;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class MTRetailQtyAndPrice : DropDownView<MTRetailQtyAndPriceDataProvider.DataRow, MTRetailQtyAndPriceDataProvider.Param>
{
	public override string HelpId => "Reports_MTRetailQtyAndPrice";

	public override string Caption => string.Empty;

	public string MTCode { get; set; }

	public string Storage { get; set; }

	public DateOnly? Date { get; set; }

	public decimal UnitCoef { get; set; }

	public MTRetailQtyAndPrice(string mtCode, decimal unitCoef, string storage, DateOnly? date)
	{
		MTCode = mtCode;
		UnitCoef = unitCoef;
		Storage = storage;
		Date = date;
		DataProvider = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.Storage = Storage;
		ParametersProvider.Date = Date;
		ParametersProvider.UnitCoef = UnitCoef;
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].Quantity.ToString();
	}

	public override string CommentMember(int index)
	{
		return Data[index].Price.ToString();
	}
}
