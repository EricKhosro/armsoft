using System;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DataView.Directories;

[DataView(10121)]
public class BonusTransactionsDataView : TradeDataView<BonusTransactionsDataProvider.DataRow, BonusTransactionsDataProvider.Param>
{
	public class DataViewDialog : DataViewParametersDialogWindow
	{
		private readonly BonusTransactionsDataView _dataView;

		private DateEditControl _startDate;

		private DateEditControl _endDate;

		private CheckEditExt _showDeleted;

		private TextEditExt _cardNumber;

		public DataViewDialog(BonusTransactionsDataView dataView, bool useParamValues, bool isRefreshMode)
			: base(dataView)
		{
			_dataView = dataView;
			Caption = _dataView.Caption;
			Name = _dataView.Name;
			HelpId = dataView.HelpId;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			if (!isRefreshMode)
			{
				AddStoreValuesButton(container);
			}
			_startDate = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, !useParamValues);
			_startDate.EditValue = (useParamValues ? _dataView.StartDate : new DateOnly?(Settings.HostingEnvironment.DateBegin));
			_endDate = AddDateControl(container, "EndDate", string.Empty, useLongDate: false, isRequired: false, !useParamValues);
			_endDate.EditValue = (useParamValues ? _dataView.EndDate : new DateOnly?(Settings.HostingEnvironment.DateEnd));
			AddSpecialDateButton(container, _startDate, _endDate);
			_cardNumber = AddStringControl("CardNumber", ArmSoft.SME.Trade.Common.Resources.DiscountCardCode, 255, isRequired: false, storeValue: true);
			_cardNumber.Text = (useParamValues ? _dataView.CardNumber : string.Empty);
			_showDeleted = AddBooleanControl("ShowDeleted", ArmSoft.SME.Trade.Common.Resources.ShowDeleted, storeValue: true);
			_showDeleted.IsChecked = useParamValues && dataView.ShowDeleted;
		}

		protected override void DialogValidation(DialogWindowValidationArgs args)
		{
			try
			{
				ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
				base.DialogValidation(args);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
			}
		}

		public override void ApplyParams(bool useParamValues, bool isRefreshMode)
		{
			_dataView.CardNumber = _cardNumber.Text;
			_dataView.StartDate = _startDate.EditValue;
			_dataView.EndDate = _endDate.EditValue;
			_dataView.ShowDeleted = _showDeleted.IsChecked.Value;
		}
	}

	public override string HelpId => "Sales_Lists_DiscountsGroups";

	public override bool DialogSupported => true;

	public override bool AllowDelete => AllowOperation;

	public string CardNumber
	{
		get
		{
			return (string)Parameters["CardNumber"].Value;
		}
		set
		{
			Parameters["CardNumber"].Value = value;
		}
	}

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

	public bool ShowDeleted
	{
		get
		{
			return (bool)Parameters["ShowDeleted"].Value;
		}
		set
		{
			Parameters["ShowDeleted"].Value = value;
		}
	}

	public BonusTransactionsDataView()
	{
		DataProvider = new BonusTransactionsDataProvider();
		AddParameter("CardNumber", ArmSoft.SME.Trade.Common.Resources.PartnerCard, string.Empty);
		AddParameter("StartDate", ArmSoft.SME.Common.Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", ArmSoft.SME.Common.Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("ShowDeleted", "ShowDeleted", false);
		FillColumnsFromDataProvider();
	}

	public override DataViewParametersDialogWindow CreateDialog(bool useParamValues, bool isRefreshMode)
	{
		return new DataViewDialog(this, useParamValues, isRefreshMode);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("IsDeleted", ShowDeleted);
		dvp.Set_ColumnSortOrder("TransactionISN");
	}

	public override void Delete()
	{
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		if (currentViewPanel == null || new Func<object>(currentViewPanel.FocusedRow) == null)
		{
			return;
		}
		BonusTransactionsDataProvider.DataRow dataRow = (BonusTransactionsDataProvider.DataRow)currentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			if (dataRow.IsDeleted)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowIsAlreadyDeleted);
			}
			if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.ConfRecordDel, ArmSoft.SME.Trade.Common.Resources.DiscountCardCode + " " + dataRow.CardNumber + ",  " + dataRow.TransactionDate.ToString("dd/MM/yyyy HH:mm:ss") + " " + ArmSoft.SME.Common.Resources.DateShort), ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				DiscountServerService.DeleteTransactionsByISN(dataRow.TransactionISN);
				currentViewPanel.Update(dataRow.TransactionISN);
			}
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.CardNumber = CardNumber;
		ParametersProvider.StartDate = StartDate;
		ParametersProvider.EndDate = EndDate;
		ParametersProvider.ShowDeleted = ShowDeleted;
	}
}
