using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Core.UI.Enums;

namespace ArmSoft.SME.Core.UI.Progress;

internal partial class MultilineProgressPhaseControl : UserControl, IComponentConnector
{
	private const int _updateTimerMs = 500;

	private DispatcherTimer _timer;

	private DateTime _startDate;

	public MultilineProgressPhaseState State { get; private set; }

	public MultilineProgressPhaseControl(string name)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		StateImage.Source = ImageService.GetImage("arrow_right_16.svg");
		StateImage.Visibility = Visibility.Hidden;
		TimeTextBlock.Text = "00:00";
		NameTextBlock.Text = name;
		TotalTextBlock.Visibility = Visibility.Collapsed;
		SlashTextBlock.Visibility = Visibility.Collapsed;
		CountTextBlock.Visibility = Visibility.Collapsed;
	}

	public void ChangeName(string name)
	{
		NameTextBlock.Text = name;
	}

	public void Start(ProgressPhaseBase progressPhaseBase = null)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Expected Obj, but got Unknown
		if (progressPhaseBase != null)
		{
			if (progressPhaseBase.Total >= 0)
			{
				TotalTextBlock.Text = progressPhaseBase.Total.ToString();
				TotalTextBlock.Visibility = Visibility.Visible;
				SlashTextBlock.Visibility = Visibility.Visible;
			}
			if (progressPhaseBase.Row >= 0)
			{
				CountTextBlock.Text = progressPhaseBase.Row.ToString();
				CountTextBlock.Visibility = Visibility.Visible;
			}
		}
		State = MultilineProgressPhaseState.Started;
		StateImage.Visibility = Visibility.Visible;
		_startDate = DateTime.Now;
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
	}

	public void Stop()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		StateImage.Source = ImageService.GetImage("check_16.svg");
		_timer.Stop();
		State = MultilineProgressPhaseState.Stopped;
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		TimeTextBlock.Text = (DateTime.Now - _startDate).FormatHHMMSSOrMMSS(":");
	}
}
