using System;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class GoodsPartiesAvailabilityListByProperty1 : DropDownView<GoodsPartiesAvailibilityListByProperty1DataProvider.DataRow, GoodsPartiesAvailibilityListByProperty1DataProvider.Param>
{
	private readonly string _storage;

	private readonly string _barCode;

	private readonly DateOnly _date;

	public override string Caption => Resources.MaterialsAndServices;

	public override string Name => "GoodsPartiesAvailibilityListByProperty1";

	public GoodsPartiesAvailabilityListByProperty1(string storage, string barCode, DateOnly date)
	{
		_storage = storage;
		_barCode = barCode;
		_date = date;
		DataProvider = new GoodsPartiesAvailibilityListByProperty1DataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		Columns["GoodID"].Visible = false;
	}

	public override string ValueMember(int index)
	{
		return Data[index].GoodCode;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartiesStorage = _storage;
		ParametersProvider.PartiesProperty1Value = _barCode;
		ParametersProvider.PartiesDate = _date;
	}
}
