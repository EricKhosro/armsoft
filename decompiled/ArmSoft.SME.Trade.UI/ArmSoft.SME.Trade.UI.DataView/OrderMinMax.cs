using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.DropDown;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10030)]
public class OrderMinMax : TradeDirectoryDataView<OrderMinMaxDataProvider.DataRow, OrderMinMaxDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly OrderMinMax _dataView;

		private readonly DropDownControl _mtCode;

		private readonly DropDownControl _mtGroup;

		private readonly DropDownControl _storageCode;

		public DataViewDialog(OrderMinMax dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			string storage = ArmSoft.SME.Common.Resources.Storage;
			bool? checkViewOrOperationAccess = true;
			_storageCode = AddDropDownControl(container, "StorageCode", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_storageCode.Code = (useParamValues ? dataView.StorageCode : string.Empty);
			_mtGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_mtGroup.Code = (useParamValues ? dataView.MTGroup : string.Empty);
			_mtCode = AddDropDownControl("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_mtCode.Code = (useParamValues ? dataView.MTCode : string.Empty);
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.MTCode = _mtCode.Code;
			_dataView.MTGroup = _mtGroup.Code;
			_dataView.StorageCode = _storageCode.Code;
		}
	}

	public override short DirectoryType => DirectoryTypeSE.OrderMinMax.Value;

	public override string Caption
	{
		get
		{
			string caption = base.Caption;
			if (!string.IsNullOrWhiteSpace(StorageCode))
			{
				return caption += $", {ArmSoft.SME.Common.Resources.Storage}` {StorageCode} - {TradeSettings.TradeCacheService.Storage.GetItem(StorageCode).Caption}";
			}
			if (!string.IsNullOrWhiteSpace(MTCode))
			{
				return caption += $", {ArmSoft.SME.Trade.Common.Resources.MaterialValue}` {MTCode} - {TradeSettings.TradeCacheService.Material.GetItem(MTCode).Caption}";
			}
			return caption;
		}
	}

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_OrderFormattingParameters";

	public override bool DialogSupported => true;

	public override bool AllowEditSelected => base.AllowEdit;

	public string MTCode
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

	public string MTGroup
	{
		get
		{
			return (string)Parameters["MTGroup"].Value;
		}
		set
		{
			Parameters["MTGroup"].Value = value;
		}
	}

	public string StorageCode
	{
		get
		{
			return (string)Parameters["StorageCode"].Value;
		}
		set
		{
			Parameters["StorageCode"].Value = value;
		}
	}

	public OrderMinMax()
	{
		DataProvider = new OrderMinMaxDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialValue, string.Empty);
		AddParameter("MTGroup", ArmSoft.SME.Common.Resources.Group, string.Empty);
		AddParameter("StorageCode", ArmSoft.SME.Common.Resources.Storage, string.Empty);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("StorageCode");
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	public override void Add()
	{
		OrderMinMaxDialogWindow orderMinMaxDialogWindow = new OrderMinMaxDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		OrderMinMaxDataProvider.DataRow dataRow = (OrderMinMaxDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			orderMinMaxDialogWindow.Directory.Code = dataRow.MTCode;
		}
		else
		{
			orderMinMaxDialogWindow.Directory.Code = MTCode;
		}
		orderMinMaxDialogWindow.ShowDialog();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.MTCode = MTCode;
		ParametersProvider.MTGroup = MTGroup;
		ParametersProvider.StorageCode = StorageCode;
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "MinQty", ArmSoft.SME.Common.Resources.MinQty, 13, 3);
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "MaxQty", ArmSoft.SME.Common.Resources.MaxQty, 13, 3);
	}
}
