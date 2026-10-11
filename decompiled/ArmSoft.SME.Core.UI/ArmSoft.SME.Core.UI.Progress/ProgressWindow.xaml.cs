using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.Progress;

public partial class ProgressWindow : ThemedWindow, IExceptionDispatchHandler, IComponentConnector
{
	private const int _sleepMs = 300;

	private readonly DateTime _startDate;

	public bool CancellationPending => BackgroundWorker?.CancellationPending ?? false;

	private BackgroundWorker BackgroundWorker { get; set; }

	private DispatcherTimer Timer { get; set; }

	public ExceptionDispatchInfo ExceptionDispatchInfo { get; private set; }

	public ProgressWindow(string title = "", Window owner = null)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected Obj, but got Unknown
		InitializeComponent();
		Title = title;
		TimeTextBlock.Text = "00:00";
		_startDate = DateTime.Now;
		AbortButton.Content = ArmSoft.SME.Common.Resources.Abort;
		Owner = owner ?? Settings.HostingEnvironment;
		Timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 1), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
		BackgroundWorker = new BackgroundWorker
		{
			WorkerReportsProgress = true,
			WorkerSupportsCancellation = true
		};
		BackgroundWorker.ProgressChanged += UpdateProgress;
		BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
		Loaded += ProgressWindow_Loaded;
		AbortButton.Click += AbortButton_Click;
		KeyDown += ProgressWindow_KeyDown;
		if (Title == string.Empty)
		{
			WindowStyle = WindowStyle.None;
		}
	}

	public void Show<T>(List<T> values, ProcessItem<T> doWork, int minItemsCountForShow = 3, string progressName = "", params object[] parameters)
	{
		if (values.Count >= minItemsCountForShow)
		{
			int total = values.Count;
			Width += SizeCalculator.WidthForString(progressName.Length + 2 * total.ToString().Length + 2);
			BackgroundWorker.DoWork += (object? sender, DoWorkEventArgs e) =>
			{
				for (int i = 0; i < values.Count; i++)
				{
					T item = values[i];
					if (CancellationPending)
					{
						e.Cancel = true;
						break;
					}
					BackgroundWorker.ReportProgress((i + 1) * 100 / total, progressName + " " + $"{i + 1}/{total}");
					doWork(item, parameters);
				}
			};
			BackgroundWorker.RunWorkerAsync();
			this.ShowDialogWithExceptionHandling();
			return;
		}
		foreach (T value in values)
		{
			doWork(value, parameters);
		}
	}

	public void ShowWithoutPercent(DoWorkEventHandler doWork, string progressName = "")
	{
		Width += SizeCalculator.WidthForString(progressName.Length);
		ProgressBarExec.StyleSettings = new ProgressBarMarqueeStyleSettings
		{
			AccelerateRatio = 3.0
		};
		ProgressBlock.Text = progressName;
		BackgroundWorker.DoWork += doWork;
		BackgroundWorker.RunWorkerAsync();
		this.ShowDialogWithExceptionHandling();
	}

	public void ChangeProgressName(string progressName)
	{
		ProgressBlock.Text = progressName;
	}

	private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Timer.Stop();
		BackgroundWorker.Dispose();
		Thread.Sleep(300);
		DialogResult = true;
		if (e.Error != null)
		{
			ExceptionDispatchInfo = ExceptionDispatchInfo.Capture(e.Error);
		}
	}

	private void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
	{
		((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action(HideCloseButton), Array.Empty<object>());
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		TimeTextBlock.Text = (DateTime.Now - _startDate).FormatHHMMSSOrMMSS(":");
	}

	private void ProgressWindow_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		if ((int)Keyboard.Modifiers == 0 && (int)e.Key == 13)
		{
			Aborting();
		}
	}

	private void AbortButton_Click(object sender, RoutedEventArgs e)
	{
		Aborting();
	}

	private void UpdateProgress(object sender, ProgressChangedEventArgs e)
	{
		ProgressBarExec.Value = e.ProgressPercentage;
		ProgressBlock.Text = e.UserState?.ToString();
	}

	private void HideCloseButton()
	{
		ControlBoxButtonSet = ControlBoxButtons.None;
	}

	private void Aborting()
	{
		AbortButton.IsEnabled = false;
		AbortButton.Content = ArmSoft.SME.Common.Resources.Aborting;
		BackgroundWorker.CancelAsync();
	}
}
