using System.Collections.Generic;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Goods;

public class ElScaleGoodsShowDataView : TradeDataView<ElScaleGoodsShowDataProvider.DataRow, ElScaleGoodsShowDataProvider.Param>
{
	private readonly string _elScaleCaption;

	private string _elScaleCode;

	public override string Name => DataProvider.Name;

	public override string Caption => Resources.ViewScaleItemList + " (" + _elScaleCaption + ")";

	public override bool RefreshSupported => false;

	public List<ElScaleGoodsShowDataProvider.DataRow> ParamRows { get; set; }

	public ElScaleGoodsShowDataView(string elScalecaption, string elScaleCode)
	{
		_elScaleCaption = elScalecaption;
		_elScaleCode = elScaleCode;
		DataProvider = new ElScaleGoodsShowDataProvider();
		FillColumnsFromDataProvider();
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.Rows = ParamRows;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!(_elScaleCode == ElScaleTypesSE.TMADirect.Value) && !(_elScaleCode == ElScaleTypesSE.DigiDirect.Value) && !(_elScaleCode == ElScaleTypesSE.CasDirect.Value))
		{
			dvp.Set_ColumnVisible("LabelN", value: false);
			dvp.Set_ColumnVisible("BarCodeN", value: false);
		}
		if (_elScaleCode == ElScaleTypesSE.CasDirect.Value)
		{
			Columns["BarCodePrefix"].Caption = ((Resources.Prefix + _elScaleCode == ElScaleTypesSE.CasDirect.Value) ? " (Group)" : "");
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("CodeInScale");
		}
	}
}
