using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.DataView.Reports.Analysis;

public class ABCAnalysisDataView : TradeDataView<ABCAnalysisDataProvider.DataRow, ABCAnalysisDataProvider.Param>
{
	private class DataViewDialog : TradeDataViewParametersDialogWindow
	{
		private readonly ABCAnalysisDataView _dataView;

		private readonly DateEditControl _startDate;

		private readonly DateEditControl _endDate;

		private readonly DropDownControl _partnerGroup;

		private readonly MultiSelectDropDownControl _mtGroup;

		private readonly MultiSelectDropDownControl _storage;

		private readonly ComboBoxExt _criteria;

		private readonly ComboBoxExt _sumType;

		private readonly PopupCalcEditExt _percentA;

		private readonly PopupCalcEditExt _percentB;

		private readonly ComboBoxExt _chartType;

		private readonly SpinEditExt _chartRowCount;

		public DataViewDialog(ABCAnalysisDataView dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "DateBegin", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodBegin);
			_startDate.EditValue = (useParamValues ? _dataView.StartDate : Settings.HostingEnvironment.DateBegin);
			_endDate = AddDateControl(container, "DateEnd", string.Empty, useLongDate: false, isRequired: true, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_endDate.EditValue = (useParamValues ? _dataView.EndDate : Settings.HostingEnvironment.DateEnd);
			AddSpecialDateButton(container, _startDate, _endDate);
			if (_dataView._analyzeByPartners)
			{
				_partnerGroup = AddDropDownControl("Group", ArmSoft.SME.Trade.Common.Resources.PartnerGroup, new PartnerGroups(), isRequired: false, allowAdditionButtons: false, storeValue: true);
				_partnerGroup.Code = (useParamValues ? _dataView.Group : string.Empty);
			}
			else
			{
				_mtGroup = AddMultiSelectDropDownControl("Group", ArmSoft.SME.Trade.Common.Resources.MTGroup, new GoodsGroups(), isRequired: false, storeValue: true);
				_mtGroup.SelectedString = (useParamValues ? _dataView.Group : string.Empty);
				string storage = ArmSoft.SME.Common.Resources.Storage;
				bool? checkViewOrOperationAccess = true;
				_storage = AddMultiSelectDropDownControl("Storage", storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(null, checkViewOrOperationAccess), isRequired: false, storeValue: true);
				_storage.SelectedString = (useParamValues ? _dataView.Storage : string.Empty);
			}
			AddLine();
			_criteria = new ComboBoxExt
			{
				ItemsSource = FiltersHelper.CompareFeaturesList(includeProfitPercent: false, !_dataView._analyzeByPartners),
				Width = 170.0
			};
			AddControl("CategorizationCriteria", ArmSoft.SME.Trade.Common.Resources.CategorizationCriteria, _criteria, storeValue: true);
			_sumType = new ComboBoxExt
			{
				ItemsSource = FiltersHelper.SumTypeList()
			};
			if (useParamValues)
			{
				if (_dataView.SummaType == CommonConst.SalesSumType.SumNCC)
				{
					_sumType.SelectedIndex = 0;
				}
				else if (_dataView.SummaType == CommonConst.SalesSumType.WithVAT)
				{
					_sumType.SelectedIndex = 1;
				}
				else
				{
					_sumType.SelectedIndex = 2;
				}
			}
			else
			{
				_sumType.SelectedIndex = 0;
			}
			_sumType.Width = 170.0;
			AddControl("SummaType", ArmSoft.SME.Trade.Common.Resources.SummaType, _sumType, storeValue: true);
			_criteria.SelectionChanged += new SafeEventHandler<SelectionChangedEventArgs>(Criteria_SelectionChanged).Handler;
			_criteria.SelectedIndex = (useParamValues ? _dataView.CategorizationCriteria : 0);
			_percentA = AddNumericControl("PercentA", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "A"), 6, 2, isRequired: true, allowNegativeNumbers: false, useCurrencyFormat: false, storeValue: true);
			_percentA.Value = (useParamValues ? _dataView.PercentA : 80m);
			_percentB = AddNumericControl("PercentB", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "B"), 6, 2, isRequired: true, allowNegativeNumbers: false, useCurrencyFormat: false, storeValue: true);
			_percentB.Value = (useParamValues ? _dataView.PercentB : 15m);
			AddLine();
			_chartType = AddDiagramKind(null, "ChartType", storeValue: true);
			_chartType.SelectedIndex = (int)(useParamValues ? _dataView.ChartType : ChartType.Bars2d);
			_chartType.Width = 50.0;
			_chartRowCount = AddSpinEdit("ChartRowCount", ArmSoft.SME.Trade.Common.Resources.ChartRowCount, 1m, 50m, 0.0, storeValue: true);
			_chartRowCount.EditValue = ((useParamValues && _dataView.ChartRowCount >= 1 && _dataView.ChartRowCount <= 50) ? _dataView.ChartRowCount : 20);
			_chartRowCount.Width = 50.0;
			_percentA.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PercentA_EditValueChanged).Handler;
			_percentB.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(PercentB_EditValueChanged).Handler;
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.StartDate = _startDate.EditValue.Value;
			_dataView.EndDate = _endDate.EditValue.Value;
			if (_dataView._analyzeByPartners)
			{
				_dataView.Group = _partnerGroup.Code;
			}
			else
			{
				_dataView.Storage = _storage.SelectedString;
				_dataView.Group = _mtGroup.SelectedString;
			}
			_dataView.CategorizationCriteria = _criteria.SelectedIndex;
			_dataView.SummaType = (CommonConst.SalesSumType)_sumType.SelectedIndex;
			_dataView.PercentA = _percentA.Value;
			_dataView.PercentB = _percentB.Value;
			_dataView.ChartType = (ChartType)_chartType.SelectedIndex;
			_dataView.ChartRowCount = (int)_chartRowCount.Value;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			base.DialogValidation(args);
			DiagramAnalysisHelper.DialogValidation(this, args);
			if (args.isValid)
			{
				args.isValid = TradeDataViewParametersDialogWindow.CheckPeriodForActualness(_endDate.EditValue.Value);
			}
		}

		private void Criteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!_dataView._analyzeByPartners)
			{
				_sumType.IsReadOnly = _criteria.SelectedIndex == 0;
				_sumType.IsEnabled = !_sumType.IsReadOnly;
				if (_sumType.Parent != null)
				{
					((LayoutItemWithRequired)(object)_sumType.Parent).IsReadOnly = _sumType.IsReadOnly;
				}
			}
		}

		private void PercentA_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			ArmSoft.SME.Core.UI.ValidationService.ValidatePercent("PercentA", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "A"), _percentA.Value);
		}

		private void PercentB_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			ArmSoft.SME.Core.UI.ValidationService.ValidatePercent("PercentB", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "B"), _percentB.Value);
		}
	}

	private readonly bool _analyzeByPartners;

	public override string HelpId
	{
		get
		{
			if (_analyzeByPartners)
			{
				return "Trade_Reports_Analysis_ABC_Partners";
			}
			return "Trade_Reports_Analysis_ABC_Goods";
		}
	}

	public Dictionary<string, BindingList<ABCAnalysisDataProvider.DataRow>> ChartRows
	{
		get
		{
			Dictionary<string, BindingList<ABCAnalysisDataProvider.DataRow>> dictionary = new Dictionary<string, BindingList<ABCAnalysisDataProvider.DataRow>>();
			foreach (ABCAnalysisDataProvider.DataRow datum in Data)
			{
				if (dictionary.TryGetValue(datum.ABCCategory, out var value))
				{
					if (value.Count < ChartRowCount)
					{
						value.Add(datum);
					}
				}
				else
				{
					dictionary.Add(datum.ABCCategory, new BindingList<ABCAnalysisDataProvider.DataRow> { datum });
				}
			}
			return dictionary;
		}
	}

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)Parameters["DateBegin"].Value;
		}
		set
		{
			Parameters["DateBegin"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)Parameters["DateEnd"].Value;
		}
		set
		{
			Parameters["DateEnd"].Value = value;
		}
	}

	public string Storage
	{
		get
		{
			return (string)Parameters["Storage"].Value;
		}
		set
		{
			Parameters["Storage"].Value = value;
		}
	}

	public string Group
	{
		get
		{
			return (string)Parameters["Group"].Value;
		}
		set
		{
			Parameters["Group"].Value = value;
		}
	}

	public int CategorizationCriteria
	{
		get
		{
			return (int)Parameters["CategorizationCriteria"].Value;
		}
		set
		{
			Parameters["CategorizationCriteria"].Value = value;
		}
	}

	public CommonConst.SalesSumType SummaType
	{
		get
		{
			return (CommonConst.SalesSumType)Parameters["SummaType"].Value;
		}
		set
		{
			Parameters["SummaType"].Value = value;
		}
	}

	public decimal PercentA
	{
		get
		{
			return (decimal)Parameters["PercentA"].Value;
		}
		set
		{
			Parameters["PercentA"].Value = value;
		}
	}

	public decimal PercentB
	{
		get
		{
			return (decimal)Parameters["PercentB"].Value;
		}
		set
		{
			Parameters["PercentB"].Value = value;
		}
	}

	public ChartType ChartType
	{
		get
		{
			return (ChartType)Parameters["ChartType"].Value;
		}
		set
		{
			Parameters["ChartType"].Value = value;
		}
	}

	public int ChartRowCount
	{
		get
		{
			return (int)Parameters["ChartRowCount"].Value;
		}
		set
		{
			Parameters["ChartRowCount"].Value = value;
		}
	}

	public override bool DialogSupported => true;

	public ABCAnalysisDataView(bool analyzeByPartners = false)
	{
		_analyzeByPartners = analyzeByPartners;
		AddParameter("DateBegin", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("DateEnd", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		if (analyzeByPartners)
		{
			AddParameter("Group", ArmSoft.SME.Trade.Common.Resources.PartnerGroup, string.Empty);
			AddParameter("CategorizationCriteria", ArmSoft.SME.Common.Resources.CompareFeature, CommonConst.BestSellersCompareModeShort.Income);
		}
		else
		{
			AddParameter("Storage", ArmSoft.SME.Common.Resources.Storage, string.Empty);
			AddParameter("Group", ArmSoft.SME.Trade.Common.Resources.MTGroup, string.Empty);
			AddParameter("CategorizationCriteria", ArmSoft.SME.Common.Resources.CompareFeature, CommonConst.BestSellersCompareMode.Quantity);
		}
		AddParameter("SummaType", ArmSoft.SME.Trade.Common.Resources.SummaType, CommonConst.SalesSumType.SumNCC);
		AddParameter("PercentA", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "A"), 80m);
		AddParameter("PercentB", string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategoryPercent, "B"), 15m);
		AddParameter("ChartType", ArmSoft.SME.Common.Resources.DiagramKind, ChartType.Bars2d);
		AddParameter("ChartRowCount", ArmSoft.SME.Trade.Common.Resources.ChartRowCount, 20);
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.AnalyzeByPartners = _analyzeByPartners;
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		if (!_analyzeByPartners)
		{
			ParametersProvider.Storage = Storage;
		}
		ParametersProvider.Group = Group;
		ParametersProvider.CategorizationCriteria = CategorizationCriteria;
		ParametersProvider.SummaType = (byte)SummaType;
		ParametersProvider.PercentA = PercentA;
		ParametersProvider.PercentB = PercentB;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (_analyzeByPartners)
		{
			if (CategorizationCriteria == 0)
			{
				dvp.InnerGrid().Columns["ConsumptionValue"].Header = PartnerClassificationCriteriaSE.Income.Caption;
			}
			else
			{
				dvp.InnerGrid().Columns["ConsumptionValue"].Header = PartnerClassificationCriteriaSE.Profit.Caption;
			}
		}
		else
		{
			GridColumn gridColumn = dvp.InnerGrid().Columns["ConsumptionValue"];
			gridColumn.Header = CategorizationCriteria switch
			{
				0 => (object)ArmSoft.SME.Common.Resources.Quantity, 
				1 => ArmSoft.SME.Common.Resources.Income, 
				_ => ArmSoft.SME.Common.Resources.ProfitWord, 
			};
		}
		dvp.GroupByColumns("ABCCategory");
		dvp.InnerGrid().AutoExpandAllGroups = true;
		dvp.Set_ColumnVisible("QntUnitBrief", !_analyzeByPartners);
		dvp.Set_ColumnVisible("ForPie", value: false);
	}

	public override void AfterLoadData(DataViewPanel dvp)
	{
		base.AfterLoadData(dvp);
		Dictionary<string, BindingList<ABCAnalysisDataProvider.DataRow>> chartRows = ChartRows;
		if (chartRows == null)
		{
			return;
		}
		List<AsChart> list = new List<AsChart>();
		foreach (KeyValuePair<string, BindingList<ABCAnalysisDataProvider.DataRow>> item in chartRows)
		{
			AsChart asChart = new AsChart
			{
				ChartTitle = string.Format(ArmSoft.SME.Trade.Common.Resources.ABCCategory, item.Key),
				ShowLegend = true,
				Type = ChartType,
				ShowAxisXLabels = true
			};
			if (_analyzeByPartners)
			{
				asChart.AxisXTitle = ArmSoft.SME.Common.Resources.Partners;
			}
			else
			{
				asChart.AxisXTitle = ArmSoft.SME.Trade.Common.Resources.Goods;
			}
			asChart.AxisYTitle = (string)dvp.InnerGrid().Columns["ConsumptionValue"].Header;
			asChart.DataSource = item.Value;
			if (ChartType != ChartType.Pie2D && ChartType != ChartType.Pie3D)
			{
				asChart.SetSeriesInfo("Code", "ConsumptionValue", "Name");
			}
			else
			{
				asChart.SetSeriesInfo("Name", "ConsumptionValue", "ForPie");
			}
			asChart.UpdateLayout();
			list.Add(asChart);
		}
		dvp.ChartsList = list;
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.ExistsAvailableStorage())
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.AccessToStoragesDenied, Settings.LoginService.UserName), 48);
		}
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}
}
