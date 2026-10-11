using System.Collections.Generic;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Goods;

public class ElScaleGoodsExportResultDataView : TradeDataView<ElScaleGoodsExportDataProvider.DataRow, ElScaleGoodsExportDataProvider.Param>
{
	private readonly string _elScaleCaption;

	private string _elScaleCode;

	public override bool RefreshSupported => false;

	public override string Name => DataProvider.Name;

	public override string Caption => Resources.ScaleExportedGoods + " (" + _elScaleCaption + ")";

	public List<ElScaleGoodsExportDataProvider.DataRow> ParamRows { get; set; }

	public ElScaleGoodsExportResultDataView(string elScaleCaption, string elScaleCode)
	{
		_elScaleCaption = elScaleCaption;
		_elScaleCode = elScaleCode;
		DataProvider = new ElScaleGoodsExportDataProvider();
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
		if (_elScaleCode == ElScaleTypesSE.DigiDirect.Value)
		{
			dvp.Set_ColumnVisible("Ingredients", value: false);
		}
		if (_elScaleCode == ElScaleTypesSE.CasDirect.Value)
		{
			Columns["BarCodePrefix"].Caption = ((Resources.Prefix + _elScaleCode == ElScaleTypesSE.CasDirect.Value) ? " (Group)" : "");
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("CodeInScale");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Goods, "CodeInTrade", "Name");
	}
}
