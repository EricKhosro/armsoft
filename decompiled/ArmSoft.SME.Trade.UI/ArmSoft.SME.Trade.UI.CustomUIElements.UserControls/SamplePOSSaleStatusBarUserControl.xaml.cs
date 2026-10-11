using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Trade.Common;

namespace ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;

public partial class SamplePOSSaleStatusBarUserControl : UserControl, IComponentConnector
{
	private DateTime _lastUpdatedTime = new DateTime(2000, 1, 1);

	public string GridRowInfo
	{
		get
		{
			return TB_GridRowInfo.Text;
		}
		set
		{
			TB_GridRowInfo.Text = value;
		}
	}

	public SamplePOSSaleStatusBarUserControl()
	{
		InitializeComponent();
		TB_QTYCaption.Text = ArmSoft.SME.Common.Resources.SummaryCnt + " ";
		TB_ShiftCaption.Text = ArmSoft.SME.Trade.Common.Resources.CashierShiftNumShort + " ";
		TB_TurnedOnb2barmsoftDiscountServer.Text = ArmSoft.SME.Trade.Common.Resources.TurnedOnb2barmsoftDiscountServer;
		ASFontHelper.SetFontSettings(this);
	}

	public void SetTurnedOnb2barmsoftDiscountServerIsVisibility(bool turnedOnb2barmsoftDiscountServerIsVisible)
	{
		if (turnedOnb2barmsoftDiscountServerIsVisible)
		{
			TB_TurnedOnb2barmsoftDiscountServer.Visibility = Visibility.Visible;
		}
		else
		{
			TB_TurnedOnb2barmsoftDiscountServer.Visibility = Visibility.Collapsed;
		}
	}

	public void RefreshTime()
	{
		DateTime dateTime = DateTime.Now;
		dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, dateTime.Minute, 0);
		if (dateTime.Subtract(_lastUpdatedTime).TotalMinutes > 1.0)
		{
			TB_Time.Text = dateTime.ToString("dd/MM/yy HH:mm");
		}
	}
}
