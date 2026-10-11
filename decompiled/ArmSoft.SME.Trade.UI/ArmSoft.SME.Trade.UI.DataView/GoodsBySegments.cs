using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10012)]
public class GoodsBySegments : TradeDirectoryDataView<GoodsBySegmentDataProvider.DataRow, GoodsBySegmentDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly GoodsBySegments _dataView;

		private readonly DropDownControl _segmentCode;

		private readonly DropDownControl _goodCode;

		public DataViewDialog(GoodsBySegments dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			LayoutGroupExt container2 = AddVerticalLayoutGroup(container);
			_segmentCode = AddDropDownControl(container2, "SegmentCode", ArmSoft.SME.Trade.Common.Resources.Segment, new ArmSoft.SME.Trade.UI.DropDown.Segments(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_segmentCode.Code = (useParamValues ? dataView.SegmentCode : string.Empty);
			_goodCode = AddDropDownControl(container2, "MTCode", ArmSoft.SME.Common.Resources.Product, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_goodCode.Code = (useParamValues ? dataView.GoodCode : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.SegmentCode = _segmentCode.Code;
			_dataView.GoodCode = _goodCode.Code;
		}
	}

	public override short DirectoryType => DirectoryTypeSE.GoodsBySegment.Value;

	public override string HelpId => "Purchases_Lists_ProductsAndServicesSublist_ProductsBySegment";

	public override bool DialogSupported => true;

	public override bool AllowEditSelected => base.AllowEdit;

	public string GoodCode
	{
		get
		{
			return (string)Parameters["MTCode"].Value;
		}
		set
		{
			Parameters["MTCode"].Value = value;
		}
	}

	public string SegmentCode
	{
		get
		{
			return (string)Parameters["SegmentCode"].Value;
		}
		set
		{
			Parameters["SegmentCode"].Value = value;
		}
	}

	public GoodsBySegments()
	{
		DataProvider = new GoodsBySegmentDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("SegmentCode", ArmSoft.SME.Trade.Common.Resources.Segment, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Common.Resources.Product, string.Empty);
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		dvp.Set_ColumnVisible("Type", value: false);
		dvp.Set_ColumnVisible("Unit", value: false);
		dvp.Set_ColumnVisible("AltUnit", value: false);
		dvp.Set_ColumnVisible("Point", value: false);
		dvp.Set_ColumnVisible("Body", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("SegmentCode");
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = GoodCode;
		ParametersProvider.SegmentCode = SegmentCode;
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "Code", ArmSoft.SME.Trade.Common.Resources.Segment, new ArmSoft.SME.Trade.UI.DropDown.Segments(), isRequired: true);
	}
}
