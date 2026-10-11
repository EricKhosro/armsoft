using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Editors.Popups.Calendar;

namespace ArmSoft.SME.Core.UI.Controls;

public class DateEditCalendarMonth : DateEditCalendar
{
	public DateEditCalendarMonth()
	{
		Loaded += new SafeEventHandler<RoutedEventArgs>(DateEditCalendarMonth_Loaded).Handler;
	}

	private void DateEditCalendarMonth_Loaded(object sender, RoutedEventArgs e)
	{
		SetNewContent(DateTime, DateEditCalendarState.Year, YearInfoTemplate, DateEditCalendarTransferType.None);
	}

	protected override void OnDateTimeChanged()
	{
		UpdateDateTimeText();
		if (OwnerDateEdit == null)
		{
			SetNewContent(DateTime, DateEditCalendarState.Year, YearInfoTemplate, DateEditCalendarTransferType.None);
		}
	}

	protected override void OnTodayButtonClick(object sender, RoutedEventArgs e)
	{
		DateEditCalendarTransferType transferType;
		if (ActiveContent.State != DateEditCalendarState.Month)
		{
			transferType = ((ActiveContent.State != DateEditCalendarState.Year) ? DateEditCalendarTransferType.ZoomIn : ((DateTime.Year != DateTime.Now.Year) ? DateEditCalendarTransferType.ShiftLeft : DateEditCalendarTransferType.None));
		}
		else
		{
			transferType = DateEditCalendarTransferType.ZoomOut;
		}
		DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		SetNewContent(dt, DateEditCalendarState.Year, YearInfoTemplate, transferType);
		DateTime = ActiveContent.DateTime;
	}

	protected override void OnMonthCellButtonClick(Button button)
	{
		if (OwnerDateEdit == null && button != null)
		{
			DateTime = DateEditCalendarBase.GetDateTime((DependencyObject)(object)button);
			return;
		}
		OwnerDateEdit.IsPopupOpen = false;
		if (!OwnerDateEdit.IsReadOnly)
		{
			OwnerDateEdit.DateTime = DateEditCalendarBase.GetDateTime((DependencyObject)(object)button);
		}
	}

	protected override string GetTodayText()
	{
		return DateTime.Today.ToString(CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern, CultureInfo.CurrentCulture);
	}
}
