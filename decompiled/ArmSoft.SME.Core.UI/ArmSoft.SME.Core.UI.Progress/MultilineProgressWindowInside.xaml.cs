using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.UIRequests;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Progress;

public partial class MultilineProgressWindowInside : UserControl, IComponentConnector
{
	private const int _inQueuePhaseIndex = 0;

	private const int _dequeuePhaseIndex = 1;

	private const int _updateTimerMs = 500;

	protected const int _phaseSleepMs = 100;

	protected readonly DispatcherTimer _timer;

	private readonly DateTime _startDate;

	private DispatcherTimer _longLoadingDescriptionTimer;

	private readonly List<MultilineProgressPhaseControl> _progressPhaseControls = new List<MultilineProgressPhaseControl>();

	private bool _hasLoaded;

	protected BackgroundWorker BackgroundWorker { get; private set; }

	public bool Canceled { get; private protected set; }

	protected virtual bool SupportSeparateDequeuePhase { get; } = true;

	private int LastStartedIndex { get; set; } = -1;

	private int LastDetailsIndex { get; set; }

	private int LastSystemPhaseIndex { get; set; }

	private string AbortButtonContent { get; } = ArmSoft.SME.Common.Resources.Abort;

	private string AbortingButtonContent { get; } = ArmSoft.SME.Common.Resources.Aborting;

	public ExceptionDispatchInfo ExceptionDispatchInfo { get; private set; }

	public event BackgroundWorkerDoWorkEventHandler BackgroundWorkerDoWork;

	public event EventHandler BackgroundWorkerDoWorkCompleted;

	public MultilineProgressWindowInside()
	{
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Expected Obj, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected Obj, but got Unknown
		InitializeComponent();
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Background = Brushes.Transparent;
		MainBorder.Background = Settings.GetBackgroundBrush();
		MainBorder.BorderBrush = Settings.GetBorderBrush();
		if (!Settings.IsWin11Theme)
		{
			MainBorder.BorderThickness = new Thickness(1.0);
			MainBorder.CornerRadius = new CornerRadius(0.0);
			MainBorderShadow.Opacity = 0.0;
		}
		TimeTextBlock.Text = "00:00";
		_startDate = DateTime.Now;
		AbortButton.Content = AbortButtonContent;
		MultilineProgressPhaseControl multilineProgressPhaseControl = new MultilineProgressPhaseControl(ArmSoft.SME.Common.Resources.InQueue);
		AddPhaseControl(multilineProgressPhaseControl);
		multilineProgressPhaseControl.Visibility = Visibility.Collapsed;
		MultilineProgressPhaseControl multilineProgressPhaseControl2 = new MultilineProgressPhaseControl(ArmSoft.SME.Common.Resources.ExecuteQuery);
		AddPhaseControl(multilineProgressPhaseControl2);
		multilineProgressPhaseControl2.Visibility = Visibility.Collapsed;
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
		BackgroundWorker = new BackgroundWorker();
		BackgroundWorker.DoWork += BackgroundWorker_DoWork;
		BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
		Loaded += Window_Loaded;
		PreviewKeyDown += Window_PreviewKeyDown;
		LastSystemPhaseIndex = (SupportSeparateDequeuePhase ? 1 : 0);
		_longLoadingDescriptionTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromSeconds(1L)
		};
		_longLoadingDescriptionTimer.Tick += (object? s, EventArgs args) =>
		{
			LongLoadingDescriptionBorder.Visibility = Visibility.Visible;
			_longLoadingDescriptionTimer.Stop();
		};
		_longLoadingDescriptionTimer.Start();
		if (Settings.IsWin11Theme)
		{
			LongLoadingDescriptionBorder.CornerRadius = new CornerRadius(6.0);
		}
		LongLoadingDescriptionBorder.BorderBrush = Settings.GetBorderBrush();
		LongLoadingDescriptionBorder.Background = Settings.GetEditorBackground();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (!_hasLoaded)
		{
			BackgroundWorker.RunWorkerAsync();
			_hasLoaded = true;
		}
	}

	public void Process<T>(ProgressBase<T> progress) where T : ProgressPhaseBase, new()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			if (progress == null || progress.Details == null || progress.Details.Count == 0)
			{
				int num = ((progress != null) ? 1 : 0);
				ProcessAfterPhases(num);
				MultilineProgressPhaseControl multilineProgressPhaseControl = _progressPhaseControls[num];
				if (multilineProgressPhaseControl.State == MultilineProgressPhaseState.Waiting)
				{
					multilineProgressPhaseControl.Visibility = Visibility.Visible;
					multilineProgressPhaseControl.Start();
					LastStartedIndex = num;
					SetDefaultCancelButtonState();
				}
			}
			else
			{
				if (LastStartedIndex < 0)
				{
					LastStartedIndex = LastSystemPhaseIndex;
					SetDefaultCancelButtonState();
				}
				for (int i = LastDetailsIndex; i < progress.Details.Count; i++)
				{
					InnerProcessPhase(progress.Details[i], LastSystemPhaseIndex + i + 1);
				}
				LastDetailsIndex = progress.Details.Count - 1;
			}
		}), Array.Empty<object>()).Wait();
	}

	public void ProcessPhase(ProgressPhaseBase phase)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			InnerProcessPhase(phase);
		}), Array.Empty<object>()).Wait();
	}

	public void PreparePrimaryPhases(List<string> phasesNames)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			if (phasesNames.Count > 0)
			{
				int i = 0;
				if (!SupportSeparateDequeuePhase)
				{
					_progressPhaseControls[1].NameTextBlock.Text = phasesNames[i++];
					_progressPhaseControls[1].Visibility = Visibility.Visible;
				}
				for (; i < phasesNames.Count; i++)
				{
					AddPhaseControl(new MultilineProgressPhaseControl(phasesNames[i]));
				}
			}
		}), Array.Empty<object>()).Wait();
	}

	public UIRequestResult ShowUIRequestAndGetResult(UIRequestData uIRequestData, Guid guid, Dictionary<int, Func<JsonElement, JsonElement>> customUIRequestsConfig = null)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		UIRequestResult uiRequestResult = null;
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			uiRequestResult = new UIRequestResult();
			IUIRequestService requiredService = Settings.ServiceProvider.GetRequiredService<IUIRequestService>();
			uiRequestResult = requiredService.ShowAndGetResult(uIRequestData, guid, customUIRequestsConfig);
		}), Array.Empty<object>()).Wait();
		return uiRequestResult;
	}

	protected virtual void SetDefaultCancelButtonState(ProgressPhaseBase phase = null)
	{
		Canceled = false;
		AbortButton.IsEnabled = phase == null || !phase.DisableCancel;
		AbortButton.Content = AbortButtonContent;
	}

	public bool IfErrorExist()
	{
		if (ExceptionDispatchInfo != null)
		{
			_longLoadingDescriptionTimer.Stop();
			LongLoadingDescriptionBorder.Visibility = Visibility.Collapsed;
			Title.Text = ArmSoft.SME.Common.Resources.ErrorWord;
			TimeTextBlock.Visibility = Visibility.Collapsed;
			Phases.Visibility = Visibility.Collapsed;
			AbortButton.Visibility = Visibility.Collapsed;
			ErrorGrid.Visibility = Visibility.Visible;
			MsgTextBlock.Text = ExceptionDispatchInfo.SourceException?.Message;
			return true;
		}
		return false;
	}

	private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		BackgroundWorkerDoWork(sender, e);
	}

	protected virtual void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		_timer.Stop();
		if (e.Error != null)
		{
			ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(e.Error);
		}
		BackgroundWorker.Dispose();
		BackgroundWorkerDoWorkCompleted(this, e);
	}

	public void ChangProcessPhaseName(string oldName, string newName)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
		{
			_progressPhaseControls.FirstOrDefault((MultilineProgressPhaseControl x) => x.NameTextBlock.Text == oldName)?.ChangeName(newName);
		}), Array.Empty<object>()).Wait();
	}

	private void InnerProcessPhase(ProgressPhaseBase detail, int? index = null)
	{
		MultilineProgressPhaseControl multilineProgressPhaseControl = null;
		if (index.HasValue && _progressPhaseControls.Count > index)
		{
			multilineProgressPhaseControl = _progressPhaseControls[index.Value];
			if (!detail.Overwritable && multilineProgressPhaseControl.NameTextBlock.Text != detail.Name)
			{
				multilineProgressPhaseControl = new MultilineProgressPhaseControl(detail.Name);
				AddPhaseControl(multilineProgressPhaseControl, index);
			}
		}
		else if (!index.HasValue)
		{
			multilineProgressPhaseControl = _progressPhaseControls.FirstOrDefault((MultilineProgressPhaseControl x) => x.NameTextBlock.Text == detail.Name);
		}
		if (multilineProgressPhaseControl == null)
		{
			multilineProgressPhaseControl = new MultilineProgressPhaseControl(detail.Name);
			AddPhaseControl(multilineProgressPhaseControl, LastStartedIndex + 1);
		}
		ProcessAfterPhases(_progressPhaseControls.IndexOf(multilineProgressPhaseControl));
		if (multilineProgressPhaseControl.State == MultilineProgressPhaseState.Waiting)
		{
			multilineProgressPhaseControl.Start(detail);
			LastStartedIndex++;
			SetDefaultCancelButtonState(detail);
		}
		if (detail.Overwritable)
		{
			multilineProgressPhaseControl.NameTextBlock.Text = detail.Name;
		}
		if (detail.Row != -1)
		{
			multilineProgressPhaseControl.CountTextBlock.Text = detail.Row.ToString();
		}
		if (detail.EndDate.HasValue && multilineProgressPhaseControl.State != MultilineProgressPhaseState.Stopped)
		{
			multilineProgressPhaseControl.Stop();
		}
	}

	private void ProcessAfterPhases(int newPhaseIndex)
	{
		for (int i = ((LastStartedIndex >= 0) ? LastStartedIndex : 0); i < newPhaseIndex; i++)
		{
			if (_progressPhaseControls[i].Visibility == Visibility.Visible)
			{
				if (_progressPhaseControls[i].State == MultilineProgressPhaseState.Waiting)
				{
					_progressPhaseControls[i].Start();
					_progressPhaseControls[i].Stop();
				}
				else
				{
					_progressPhaseControls[i].Stop();
				}
			}
		}
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		TimeTextBlock.Text = (DateTime.Now - _startDate).FormatHHMMSSOrMMSS(":");
	}

	private void AddPhaseControl(MultilineProgressPhaseControl phaseControl, int? index = null)
	{
		int valueOrDefault = index.GetValueOrDefault();
		if (!index.HasValue)
		{
			valueOrDefault = Phases.Children.Count;
			index = valueOrDefault;
		}
		Phases.Children.Insert(index.Value, phaseControl);
		_progressPhaseControls.Insert(index.Value, phaseControl);
	}

	private void AbortButton_Click(object sender, RoutedEventArgs e)
	{
		AbortButtonClick();
	}

	private void AbortButtonClick()
	{
		Canceled = true;
		AbortButton.IsEnabled = false;
		AbortButton.Content = AbortingButtonContent;
	}

	private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Invalid comparison between Unknown and I4
		if ((int)e.Key == 13)
		{
			AbortButtonClick();
		}
	}
}
