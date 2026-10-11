using System;
using System.Collections.Generic;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Documents;
using DevExpress.Xpf.Editors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Trade.UI.DataView.Reports;

public class ChangeDayOrderWindow : DialogWindow
{
	private Guid _isn;

	private int _documentOrderNumber;

	private DateOnly _documentDate;

	private TextEditExt _previous;

	private TextEditExt _next;

	private PopupCalcEditExt _value;

	private ButtonInfo _firstButtonInfo;

	private ButtonInfo _leftButtonInfo;

	private ButtonInfo _rightButtonInfo;

	private ButtonInfo _lastButtonInfo;

	private TextEditExt _current;

	public ChangeDayOrderWindow(Guid isn)
	{
		_isn = isn;
		TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Load(isn);
		_documentOrderNumber = tradeDocument.DocOrderNum;
		_documentDate = tradeDocument.DocDate;
	}

	internal bool? Invoke()
	{
		Caption = ArmSoft.SME.Common.Resources.ChangeDocumentOrderNumber;
		_current = AddStringControl("Current", ArmSoft.SME.Common.Resources.DocumentThatDayOrderNumberToBeChanged, 255);
		_current.IsReadOnly = true;
		_value = AddNumericControl("Value", string.Format(ArmSoft.SME.Trade.Common.Resources.NewOrderNumberInDay, _documentDate.FormatDDMMYY()), 7, 0, isRequired: false, allowNegativeNumbers: true, useCurrencyFormat: false);
		_value.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Value_EditValueChanged).Handler;
		_value.Validate += new SafeEventHandler<ValidationEventArgs>(Value_Validate).Handler;
		_value.Width += 80.0;
		_value.AllowDefaultButton = false;
		_firstButtonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.PrevPage,
			ToolTip = ArmSoft.SME.Common.Resources.BringToDayBegin,
			IsLeft = true
		};
		_value.Buttons.Add(_firstButtonInfo);
		_firstButtonInfo.Click += new SafeEventHandler<RoutedEventArgs>(FirstButtonInfo_Click).Handler;
		_leftButtonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Left,
			ToolTip = ArmSoft.SME.Common.Resources.BringOneStepToDayBegin,
			IsLeft = true
		};
		_value.Buttons.Add(_leftButtonInfo);
		_leftButtonInfo.Click += new SafeEventHandler<RoutedEventArgs>(LeftButtonInfo_Click).Handler;
		_rightButtonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Right,
			ToolTip = ArmSoft.SME.Common.Resources.BringOneStepToDayEnd
		};
		_value.Buttons.Add(_rightButtonInfo);
		_rightButtonInfo.Click += new SafeEventHandler<RoutedEventArgs>(RightButtonInfo_Click).Handler;
		_lastButtonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.NextPage,
			ToolTip = ArmSoft.SME.Common.Resources.BringToDayEnd
		};
		_value.Buttons.Add(_lastButtonInfo);
		_lastButtonInfo.Click += new SafeEventHandler<RoutedEventArgs>(LastButtonInfo_Click).Handler;
		AddLine();
		_previous = AddStringControl("Previous", ArmSoft.SME.Common.Resources.DocumentWithPreviousDayOrderNumber, 255);
		_previous.IsReadOnly = true;
		_next = AddStringControl("Next", ArmSoft.SME.Common.Resources.DocumentWithNextDayOrderNumber, 255);
		_next.IsReadOnly = true;
		_current.Text = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(_isn, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber);
		_value.Value = _documentOrderNumber;
		Activated += new SafeEventHandler<EventArgs>(ChangeDayOrderWindow_WindowActivated).Handler;
		WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(ChangeDayOrderWindow_WindowValidation).Handler;
		return ShowDialog();
	}

	private void CalculateNextAndPrevious()
	{
		int orderNumber = Convert.ToInt32(_value.Value);
		Guid? nextDocumentByOrderNumber = TradeSettings.CreateTradeClient().CommonServices.GetNextDocumentByOrderNumber(orderNumber, _documentDate, direction: true, _isn);
		if (nextDocumentByOrderNumber.HasValue)
		{
			_next.Text = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(nextDocumentByOrderNumber.Value, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber);
		}
		else
		{
			_next.Text = "\r\n\r\n";
		}
		Guid? nextDocumentByOrderNumber2 = TradeSettings.CreateTradeClient().CommonServices.GetNextDocumentByOrderNumber(orderNumber, _documentDate, direction: false, _isn);
		if (nextDocumentByOrderNumber2.HasValue)
		{
			_previous.Text = TradeSettings.CreateTradeClient().CommonServices.GetDocumentDescription(nextDocumentByOrderNumber2.Value, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode.ChangeDayOrderNumber);
		}
		else
		{
			_previous.Text = "\r\n\r\n";
		}
	}

	private void Value_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		CalculateNextAndPrevious();
	}

	private void ChangeDayOrderWindow_WindowActivated(object sender, EventArgs e)
	{
		_value.Focus();
	}

	private void LeftButtonInfo_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			_value.Value = TradeSettings.CreateTradeClient().CommonServices.GetPreviousFreeOrderNumber(_isn, Convert.ToInt32(_value.Value), _documentDate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void RightButtonInfo_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			_value.Value = TradeSettings.CreateTradeClient().CommonServices.GetNextFreeOrderNumber(_isn, Convert.ToInt32(_value.Value), _documentDate);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FirstButtonInfo_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			int orderNumber = Convert.ToInt32(_value.Value);
			List<DocumentDayOrder> documentsByOrderNumbers = TradeSettings.CreateTradeClient().CommonServices.GetDocumentsByOrderNumbers(orderNumber, _documentDate, direction: false, _isn);
			if (documentsByOrderNumbers.Count > 0)
			{
				_value.Value = documentsByOrderNumbers[documentsByOrderNumbers.Count - 1].OrderNum - 10;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void LastButtonInfo_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			int orderNumber = Convert.ToInt32(_value.Value);
			List<DocumentDayOrder> documentsByOrderNumbers = TradeSettings.CreateTradeClient().CommonServices.GetDocumentsByOrderNumbers(orderNumber, _documentDate, direction: true, _isn);
			if (documentsByOrderNumbers.Count > 0)
			{
				_value.Value = documentsByOrderNumbers[documentsByOrderNumbers.Count - 1].OrderNum + 10;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void Value_Validate(object sender, ValidationEventArgs e)
	{
		try
		{
			int num = Convert.ToInt32(_value.Value);
			if (TradeSettings.CreateTradeClient().CommonServices.GetDocumentByOrderNumber(num, _documentDate, _isn).HasValue)
			{
				e.ErrorType = ErrorType.Critical;
				e.ErrorContent = string.Format(ArmSoft.SME.Common.Resources.DayOrderNumberAssignedAnotherDocument, num, _documentDate.FormatDDMMYY());
				e.IsValid = false;
				_next.Text = "\r\n\r\n";
				_previous.Text = "\r\n\r\n";
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void ChangeDayOrderWindow_WindowValidation(object sender, DialogWindowValidationArgs args)
	{
		int orderNumber = Convert.ToInt32(_value.Value);
		TradeSettings.CreateTradeClient().CommonServices.ChangeDocumentOrderNumber(_isn, orderNumber);
	}
}
