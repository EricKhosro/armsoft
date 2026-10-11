using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10029)]
public class Prices : TradeDirectoryDataView<PricesDataProvider.DataRow, PricesDataProvider.Param>
{
	private class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly Prices _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _priceType;

		private readonly DropDownControl _goodGroup;

		private readonly DropDownControl _goodCode;

		private readonly bool _isSelectedGoods;

		public DataViewDialog(Prices dataView, bool useParamValues, bool isRefreshMode, bool isSelectedGoods, Window owner)
			: base(dataView, owner)
		{
			_dataView = dataView;
			Caption = ArmSoft.SME.Common.Resources.Prices;
			_isSelectedGoods = isSelectedGoods;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? _dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", string.Empty, useLongDate: false, isRequired: false, !useParamValues, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? _dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			_priceType = AddDropDownControl("PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes(), isRequired: false, allowAdditionButtons: false, !useParamValues);
			_priceType.Code = (useParamValues ? _dataView.PriceType : string.Empty);
			if (!isSelectedGoods)
			{
				_goodGroup = AddDropDownControl("MTGroup", ArmSoft.SME.Common.Resources.Group, new GoodsGroups(), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_goodGroup.Code = (useParamValues ? _dataView.MTGroup : string.Empty);
				_goodCode = AddDropDownControl("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, new GoodsFilter(), isRequired: false, allowAdditionButtons: false, !useParamValues);
				_goodCode.Code = (useParamValues ? _dataView.MTCode : string.Empty);
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.PriceType = _priceType.Code;
			if (!_isSelectedGoods)
			{
				_dataView.MTCode = (string.IsNullOrWhiteSpace(_goodCode.Code) ? _goodCode.Code : (TradeSettings.TradeCacheService.Material.GetItem(_goodCode.Code)?.MTCode ?? string.Empty));
				_dataView.MTGroup = _goodGroup.Code;
			}
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				base.DialogValidation(args);
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}
	}

	private readonly bool _isSelectedGoods;

	protected IDataViewService _dataViewService = Settings.ServiceProvider.GetRequiredService<IDataViewService>();

	public override short DirectoryType => DirectoryTypeSE.Price.Value;

	public override string HelpId => "Purchases_Lists_Productsandservicessublist_Prices";

	public override string Caption => InnerCaption();

	public override bool DialogSupported => true;

	public override LoadIndication LoadingIndication => LoadIndication.AlwaysInside;

	public DateOnly? StartDate
	{
		get
		{
			return (DateOnly?)Parameters["StartDate"].Value;
		}
		set
		{
			Parameters["StartDate"].Value = value;
		}
	}

	public DateOnly? EndDate
	{
		get
		{
			return (DateOnly?)Parameters["EndDate"].Value;
		}
		set
		{
			Parameters["EndDate"].Value = value;
		}
	}

	public string MTCode
	{
		get
		{
			return Convert.ToString(Parameters["MTCode"].Value);
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
			return Convert.ToString(Parameters["MTGroup"].Value);
		}
		set
		{
			Parameters["MTGroup"].Value = value;
		}
	}

	public string PriceType
	{
		get
		{
			return Convert.ToString(Parameters["PriceType"].Value);
		}
		set
		{
			Parameters["PriceType"].Value = value;
		}
	}

	public Prices(List<string> selectedGoodsList = null)
	{
		DataProvider = new PricesDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, string.Empty);
		AddParameter("MTCode", ArmSoft.SME.Trade.Common.Resources.MaterialOrService, string.Empty, "", !_isSelectedGoods);
		AddParameter("MTGroup", ArmSoft.SME.Common.Resources.Group, string.Empty, "", !_isSelectedGoods);
		if (selectedGoodsList != null)
		{
			if (selectedGoodsList.Count > 1)
			{
				_isSelectedGoods = true;
			}
			MTCode = string.Join(",", selectedGoodsList);
		}
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("MTID", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
			dvp.Set_ColumnSortOrder("MTCode");
		}
	}

	private string InnerCaption()
	{
		string text = string.Empty;
		if (!string.IsNullOrWhiteSpace(PriceType))
		{
			text = TradeSettings.TradeCacheService.PriceType.GetItem(PriceType).Caption;
		}
		if (string.IsNullOrWhiteSpace(MTCode) || _isSelectedGoods)
		{
			if (string.IsNullOrWhiteSpace(PriceType))
			{
				return string.Format(ArmSoft.SME.Trade.Common.Resources.MTAndServicesPrices, StartDate.FormatDDMMYY(), EndDate.FormatDDMMYY());
			}
			string mTAndServicesPriceType = ArmSoft.SME.Trade.Common.Resources.MTAndServicesPriceType;
			InlineArray4<object> buffer = default;
			buffer[0] = StartDate.FormatDDMMYY();
			buffer[1] = EndDate.FormatDDMMYY();
			buffer[2] = PriceType;
			buffer[3] = text;
			return string.Format(mTAndServicesPriceType, (ReadOnlySpan<object?>)buffer);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(MTCode);
		if (item != null)
		{
			if (item.Type == Convert.ToString(ItemTypesSE.Goods))
			{
				if (string.IsNullOrWhiteSpace(PriceType))
				{
					string mTPrices = ArmSoft.SME.Trade.Common.Resources.MTPrices;
					InlineArray4<object> buffer2 = default;
					buffer2[0] = StartDate.FormatDDMMYY();
					buffer2[1] = EndDate.FormatDDMMYY();
					buffer2[2] = MTCode;
					buffer2[3] = item.Caption;
					return string.Format(mTPrices, (ReadOnlySpan<object?>)buffer2);
				}
				string mTPricesByPriceType = ArmSoft.SME.Trade.Common.Resources.MTPricesByPriceType;
				InlineArray5<object> buffer3 = default;
				buffer3[0] = StartDate.FormatDDMMYY();
				buffer3[1] = EndDate.FormatDDMMYY();
				buffer3[2] = MTCode;
				buffer3[3] = item.Caption;
				buffer3[4] = PriceType + " - " + text;
				return string.Format(mTPricesByPriceType, (ReadOnlySpan<object?>)buffer3);
			}
			if (string.IsNullOrWhiteSpace(PriceType))
			{
				string servicePrices = ArmSoft.SME.Trade.Common.Resources.ServicePrices;
				InlineArray4<object> buffer4 = default;
				buffer4[0] = StartDate.FormatDDMMYY();
				buffer4[1] = EndDate.FormatDDMMYY();
				buffer4[2] = MTCode;
				buffer4[3] = item.Caption;
				return string.Format(servicePrices, (ReadOnlySpan<object?>)buffer4);
			}
			string servicePricesByPriceType = ArmSoft.SME.Trade.Common.Resources.ServicePricesByPriceType;
			InlineArray5<object> buffer5 = default;
			buffer5[0] = StartDate.FormatDDMMYY();
			buffer5[1] = EndDate.FormatDDMMYY();
			buffer5[2] = MTCode;
			buffer5[3] = item.Caption;
			buffer5[4] = PriceType + " - " + text;
			return string.Format(servicePricesByPriceType, (ReadOnlySpan<object?>)buffer5);
		}
		throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.MaterialNotExist, MTCode));
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode, _isSelectedGoods, null);
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode, Window owner)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode, _isSelectedGoods, owner);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.PriceType = PriceType;
		ParametersProvider.MTGroup = MTGroup;
		ParametersProvider.MTCode = MTCode;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		PricesDataProvider.DataRow dataRow = (PricesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
			if (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny() || item.Type == GoodsTypesSE.Good)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintLabel", ArmSoft.SME.Trade.Common.Resources.PrintLabel, PrintLabel);
			}
			if (dataRow != null)
			{
				Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ShowBarCodes", ArmSoft.SME.Common.Resources.BarCodes, ShowBarCodes);
			}
		}
	}

	private void ShowBarCodes()
	{
		PricesDataProvider.DataRow dataRow = (PricesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			MTBarCodes mTBarCodes = new MTBarCodes();
			mTBarCodes.MTCode = dataRow.MTCode;
			mTBarCodes.Show();
		}
	}

	private void PrintLabel()
	{
		List<Label.GoodLabelRow> list = new List<Label.GoodLabelRow>();
		foreach (PricesDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<PricesDataProvider.DataRow>())
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedOrFocusedRow.MTCode);
			if (item != null && item.Type == GoodsTypesSE.Good)
			{
				Label.GoodLabelRow item2 = InitContextFunctionsHelper.BuildLabelGoodRow(item);
				list.Add(item2);
			}
		}
		new PrintLabelDialog(null, null, list).Invoke();
	}

	public override void AfterLoadData(DataViewPanel dvp)
	{
		base.AfterLoadData(dvp);
	}

	public override void Add()
	{
		PriceDialogWindow priceDialogWindow = new PriceDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		PricesDataProvider.DataRow dataRow = (PricesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			priceDialogWindow.Directory.Code = dataRow.MTCode;
			priceDialogWindow.Directory.QntUnit = dataRow.QntUnit;
		}
		else
		{
			priceDialogWindow.Directory.Code = MTCode;
			priceDialogWindow.Directory.QntUnit = TradeSettings.TradeCacheService.Material.GetItem(MTCode)?.Unit;
		}
		priceDialogWindow.ShowDialog();
	}
}
