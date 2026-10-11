using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.InternetBank;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.ReportViewers;

public class IBBankAccountStatementReportViewer : AsReport
{
	private class ShowDailyRemsCurrencyRepRowCell : AsRepRowCell
	{
		public ShowDailyRemsCurrencyRepRowCell()
		{
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontStyle = FontStyles.Italic;
			FontWeight = FontWeights.Bold;
		}
	}

	private class SimpleCurrencyRepRowCell : AsRepRowCell
	{
		public SimpleCurrencyRepRowCell()
		{
			Alignment = TextAlignment.Right;
			ValueStyle = AsRepColStyle.CurrencyStyle;
			FontWeight = FontWeights.Bold;
		}
	}

	private List<AsRepColumn> _listOfRepRepColumns;

	private readonly string _reportTableName = "StatementTable";

	private List<InternetBankTransaction> _transactions;

	private int _colCount;

	private string _accountCur;

	private DateEditControl _startDate;

	private DateEditControl _endDate;

	private DropDownControl _account;

	private CheckEditExt _showInNCC;

	private CheckEditExt _showDailyRems;

	public override string Name => "OnlineBankStatement";

	public DateOnly StartDate
	{
		get
		{
			return (DateOnly)_parameters["StartDate"].Value;
		}
		set
		{
			_parameters["StartDate"].Value = value;
		}
	}

	public DateOnly EndDate
	{
		get
		{
			return (DateOnly)_parameters["EndDate"].Value;
		}
		set
		{
			_parameters["EndDate"].Value = value;
		}
	}

	public string Account
	{
		get
		{
			return (string)_parameters["Account"].Value;
		}
		set
		{
			_parameters["Account"].Value = value;
		}
	}

	public bool ShowDailyRemains
	{
		get
		{
			return (bool)_parameters["ShowDailyRemains"].Value;
		}
		set
		{
			_parameters["ShowDailyRemains"].Value = value;
		}
	}

	public bool ShowInNCC
	{
		get
		{
			return (bool)_parameters["ShowInNCC"].Value;
		}
		set
		{
			_parameters["ShowInNCC"].Value = value;
		}
	}

	public IBBankAccountStatementReportViewer()
	{
		DialogSupported = true;
		HelpId = "Bank_OnlineQueries_AccountStatement";
		AddParameter("StartDate", Resources.DatePeriodBegin, Settings.HostingEnvironment.DateBegin, "dd/MM/yy");
		AddParameter("EndDate", Resources.DatePeriodEnd, Settings.HostingEnvironment.DateEnd, "dd/MM/yy");
		AddParameter("Account", Resources.SettlementAccount, "");
		AddParameter("ShowInNCC", string.Format(Resources.ShowInNCC, "AMD"), false);
		AddParameter("ShowDailyRemains", Resources.ShowDailyRemTurn, false);
	}

	public override string Caption()
	{
		return Resources.BankStatement;
	}

	public override bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = Caption(),
			Name = Name,
			HelpId = HelpId
		};
		LayoutGroupExt container = dialogWindow.AddHorizontalLayoutGroup();
		if (!isRefreshMode)
		{
			dialogWindow.AddStoreValuesButton(container);
		}
		_startDate = dialogWindow.AddDateControl(container, "StartDate", Resources.DatePeriod, useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodBegin);
		_startDate.EditValue = (useParamValues ? StartDate : Settings.HostingEnvironment.DateBegin);
		_endDate = dialogWindow.AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: true, storeValue: true, Resources.DatePeriodEnd);
		_endDate.EditValue = (useParamValues ? EndDate : Settings.HostingEnvironment.DateEnd);
		dialogWindow.AddSpecialDateButton(container, _startDate, _endDate);
		IDropDownService requiredService = Settings.ServiceProvider.GetRequiredService<IDropDownService>();
		_account = dialogWindow.AddDropDownControl("Account", Resources.Account, requiredService.CreateDropDown(1), isRequired: true);
		_account.Code = (useParamValues ? Account : "");
		_showInNCC = dialogWindow.AddBooleanControl("ShowInNCC", string.Format(Resources.ShowInNCC, "AMD"), storeValue: true);
		_showDailyRems = dialogWindow.AddBooleanControl("ShowDailyRems", Resources.ShowDailyRemTurn, storeValue: true);
		_showInNCC.IsChecked = useParamValues && ShowInNCC;
		_showDailyRems.IsChecked = useParamValues && ShowDailyRemains;
		dialogWindow.WindowValidation += ParametersDialogValidation;
		if (dialogWindow.ShowDialog().Value)
		{
			StartDate = _startDate.EditValue.Value;
			EndDate = _endDate.EditValue.Value;
			Account = _account.Code;
			ShowInNCC = _showInNCC.IsChecked.Value;
			ShowDailyRemains = _showDailyRems.IsChecked.Value;
			return true;
		}
		return false;
	}

	private void ParametersDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
	{
		ValidationService.ValidateWrongPeriod(_startDate.EditValue, _endDate.EditValue);
		if (!InternetBankHelper.EnsureLoggedIn(new HashSet<string> { SettlementAccountHelper.GetBankMainCode(_account.Code) }))
		{
			args.isValid = false;
		}
	}

	public override void ExecuteDPR()
	{
		_accountCur = InternetBankHelper.GetAccountDetails(Account)?.Currency ?? "";
		DateOnly startDate = StartDate;
		DateOnly endDate = EndDate;
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = Account;
		_transactions = InternetBankHelper.GetTransactions(startDate, endDate, list, null, IBOperation.AccountStatement, alsoRevaluationType: true);
		base.ExecuteDPR();
	}

	public override bool BeforeShowReport()
	{
		ReportPanel.ClearAll();
		_listOfRepRepColumns = GetListOfRepColumns();
		_colCount = _listOfRepRepColumns.Count;
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		ReportPanel.AddTableDefinition(_reportTableName, _listOfRepRepColumns);
		AddHeaderLines();
		if (_transactions == null || _transactions.Count == 0)
		{
			ReportPanel.AddString(Resources.WgNoInform);
			return true;
		}
		decimal num = 0m;
		if (ShowDailyRemains)
		{
			num = (InternetBankHelper.GetAccountBalances(Account, StartDate.ToDateTime().AddDays(-1.0), EndDate.ToDateTime(), writeLog: false)?.OpeningBalance).GetValueOrDefault();
			list = new List<AsRepRowCell>();
			AsRepRowCell item = new AsRepRowCell
			{
				ColumnSpan = 5,
				Value = string.Format(Resources.RemInStartDay, StartDate.FormatDDMMYY()),
				Alignment = TextAlignment.Right,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ((num >= 0m) ? num : 0m),
				ValueStyle = AsRepColStyle.CurrencyStyle,
				Alignment = TextAlignment.Right,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				ColumnSpan = 1,
				Value = ((num >= 0m) ? 0m : Math.Abs(num)),
				ValueStyle = AsRepColStyle.CurrencyStyle,
				Alignment = TextAlignment.Right,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				ColumnSpan = _colCount - 7,
				Value = "",
				Alignment = TextAlignment.Center,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			ReportPanel.AddTableSpanRow(_reportTableName, list);
		}
		ReportPanel.AddTableColumnsHeader(_reportTableName);
		IOrderedEnumerable<InternetBankTransaction> source = _transactions.OrderBy((InternetBankTransaction p) => p.OperationDate);
		DateTime operationDate = source.First().OperationDate;
		decimal num2 = 0m;
		decimal num3 = 0m;
		decimal num4 = num;
		foreach (InternetBankTransaction item2 in source.ToList())
		{
			if (ShowDailyRemains && operationDate != item2.OperationDate)
			{
				list = new List<AsRepRowCell>();
				AsRepRowCell item = new AsRepRowCell
				{
					ColumnSpan = 5,
					Value = Resources.TurnOver,
					Alignment = TextAlignment.Right,
					FontWeight = FontWeights.Bold
				};
				list.Add(item);
				item = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = num2
				};
				list.Add(item);
				item = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = num3
				};
				list.Add(item);
				item = new AsRepRowCell
				{
					ColumnSpan = _colCount - 7,
					Value = "",
					Alignment = TextAlignment.Center,
					FontWeight = FontWeights.Bold
				};
				list.Add(item);
				ReportPanel.AddTableSpanRow(_reportTableName, list);
				num4 = num4 + num2 - num3;
				list = new List<AsRepRowCell>();
				item = new AsRepRowCell
				{
					ColumnSpan = 5,
					Value = string.Format(Resources.RemInEndDay, operationDate.FormatDDMMYY()),
					Alignment = TextAlignment.Right,
					FontWeight = FontWeights.Bold
				};
				list.Add(item);
				item = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = ((num4 >= 0m) ? num4 : 0m)
				};
				list.Add(item);
				item = new SimpleCurrencyRepRowCell
				{
					ColumnSpan = 1,
					Value = ((num4 >= 0m) ? 0m : Math.Abs(num4))
				};
				list.Add(item);
				item = new AsRepRowCell
				{
					ColumnSpan = _colCount - 7,
					Value = "",
					Alignment = TextAlignment.Center,
					FontWeight = FontWeights.Bold
				};
				list.Add(item);
				ReportPanel.AddTableSpanRow(_reportTableName, list);
				num2 = 0m;
				num3 = 0m;
				operationDate = item2.OperationDate;
			}
			int num5 = 7;
			List<object> list2 = new List<object>(num5);
			CollectionsMarshal.SetCount(list2, num5);
			Span<object> span = CollectionsMarshal.AsSpan(list2);
			span[0] = item2.OperationDate;
			span[1] = item2.DocumentNumber;
			span[2] = item2.OperationType;
			span[3] = item2.CorrespondentAccount;
			span[4] = item2.CorrespondentName;
			span[5] = item2.DebitAmountCur;
			span[6] = item2.CreditAmountCur;
			List<object> list3 = list2;
			if (ShowInNCC)
			{
				list3.Add(item2.DebitAmount);
				list3.Add(item2.CreditAmount);
			}
			list3.Add(item2.Comment);
			ReportPanel.AddTableRow(_reportTableName, list3);
			if (ShowDailyRemains)
			{
				num2 += item2.DebitAmountCur;
				num3 += item2.CreditAmountCur;
			}
		}
		if (ShowDailyRemains)
		{
			list = new List<AsRepRowCell>();
			AsRepRowCell item = new AsRepRowCell
			{
				ColumnSpan = 5,
				Value = Resources.TurnOver,
				Alignment = TextAlignment.Right,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			item = new SimpleCurrencyRepRowCell
			{
				ColumnSpan = 1,
				Value = num2
			};
			list.Add(item);
			item = new SimpleCurrencyRepRowCell
			{
				ColumnSpan = 1,
				Value = Math.Abs(num3)
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				ColumnSpan = _colCount - 7,
				Value = "",
				Alignment = TextAlignment.Center,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			ReportPanel.AddTableSpanRow(_reportTableName, list);
			num4 = num4 + num2 - num3;
			list = new List<AsRepRowCell>();
			item = new AsRepRowCell
			{
				ColumnSpan = 5,
				Value = string.Format(Resources.RemInEndDay, EndDate.FormatDDMMYY()),
				Alignment = TextAlignment.Right,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			item = new SimpleCurrencyRepRowCell
			{
				ColumnSpan = 1,
				Value = ((num4 >= 0m) ? num4 : 0m)
			};
			list.Add(item);
			item = new SimpleCurrencyRepRowCell
			{
				ColumnSpan = 1,
				Value = ((num4 >= 0m) ? 0m : Math.Abs(num4))
			};
			list.Add(item);
			item = new AsRepRowCell
			{
				ColumnSpan = _colCount - 7,
				Value = "",
				Alignment = TextAlignment.Center,
				FontWeight = FontWeights.Bold
			};
			list.Add(item);
			ReportPanel.AddTableSpanRow(_reportTableName, list);
		}
		return true;
	}

	private void AddHeaderLines()
	{
		ReportPanel.UnmovableHeaderLinesCount = (ShowDailyRemains ? 6 : 5);
		AsRepRowCell asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = _colCount,
			Value = string.Format(Resources.BankStatement, default(ReadOnlySpan<object>)),
			Alignment = TextAlignment.Center,
			FontWeight = FontWeights.Bold,
			FontSize = (int)Settings.GetFontSize() + 4
		};
		AsReportViewer reportPanel = ReportPanel;
		string reportTableName = _reportTableName;
		int num = 1;
		List<AsRepRowCell> list = new List<AsRepRowCell>(num);
		CollectionsMarshal.SetCount(list, num);
		CollectionsMarshal.AsSpan(list)[0] = asRepRowCell;
		reportPanel.AddTableSpanRow(reportTableName, list);
		asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = _colCount,
			Value = StartDate.FormatDDMMYYYY() + " - " + EndDate.FormatDDMMYYYY(),
			Alignment = TextAlignment.Center,
			FontWeight = FontWeights.Bold,
			FontSize = (int)Settings.GetFontSize() + 4
		};
		AsReportViewer reportPanel2 = ReportPanel;
		string reportTableName2 = _reportTableName;
		num = 1;
		List<AsRepRowCell> list2 = new List<AsRepRowCell>(num);
		CollectionsMarshal.SetCount(list2, num);
		CollectionsMarshal.AsSpan(list2)[0] = asRepRowCell;
		reportPanel2.AddTableSpanRow(reportTableName2, list2);
		ReportPanel.AddString("");
		ReportPanel.AddString(Resources.SettlementAccount + "  -  " + Account);
		ReportPanel.AddString(Resources.Currency + "  -  " + _accountCur);
	}

	private List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(Resources.DateWord, 80.0)
		{
			ValuesStyle = AsRepColStyle.DateStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.DocNumber, 120.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.Operation, 150.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.SettlementAccount, 154.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.PayerName, 150.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		item = new AsRepColumn(Resources.Debit, 96.0)
		{
			ValuesStyle = AsRepColStyle.CurrencyStyle,
			CaptionAlignment = TextAlignment.Right,
			CurrencyPrecision = 2
		};
		list.Add(item);
		item = new AsRepColumn(Resources.Credit, 96.0)
		{
			ValuesStyle = AsRepColStyle.CurrencyStyle,
			CaptionAlignment = TextAlignment.Right,
			CurrencyPrecision = 2
		};
		list.Add(item);
		if (ShowInNCC)
		{
			item = new AsRepColumn(Resources.DebitNCC, 96.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				CurrencyPrecision = 2
			};
			list.Add(item);
			item = new AsRepColumn(Resources.CreditNCC, 96.0)
			{
				ValuesStyle = AsRepColStyle.CurrencyStyle,
				CaptionAlignment = TextAlignment.Right,
				CurrencyPrecision = 2
			};
			list.Add(item);
		}
		item = new AsRepColumn(Resources.PaymentAim, 150.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left
		};
		list.Add(item);
		return list;
	}
}
