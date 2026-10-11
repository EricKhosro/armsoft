using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class FinalizeTracer : ThemedWindow, IComponentConnector
{
	private static readonly Dictionary<Type, int> _info = new Dictionary<Type, int>();

	private DispatcherTimer _timer;

	public FinalizeTracer()
	{
		InitializeComponent();
		Closing += new SafeEventHandler<CancelEventArgs>(FinalizeTracer_Closing).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(FinalizeTracer_Loaded).Handler;
		ASFontHelper.SetFontSettings(this);
	}

	public static void Register(object obj)
	{
		Type type = obj.GetType();
		lock (_info)
		{
			if (_info.TryGetValue(type, out var value))
			{
				_info[type] = value + 1;
			}
			else
			{
				_info.Add(type, 1);
			}
		}
	}

	public static void UnRegister(object obj)
	{
		Type type = obj.GetType();
		lock (_info)
		{
			if (_info.TryGetValue(type, out var value))
			{
				_info[type] = value - 1;
			}
		}
	}

	public static string GetInfoText()
	{
		StringBuilder stringBuilder = new StringBuilder();
		lock (_info)
		{
			foreach (KeyValuePair<Type, int> item in _info)
			{
				if (item.Value != 0)
				{
					stringBuilder.Append(item.Key.Name);
					stringBuilder.Append(" - ");
					stringBuilder.AppendLine(item.Value.ToString());
				}
			}
		}
		return stringBuilder.ToString();
	}

	public static void ShowWindow()
	{
		FinalizeTracer finalizeTracer = new FinalizeTracer();
		finalizeTracer.Owner = Settings.HostingEnvironment;
		finalizeTracer.Show();
	}

	private void ShowInfo_Click(object sender, RoutedEventArgs e)
	{
		InfoTB.Text = GetInfoText();
	}

	private void CallAllFinalize_Click(object sender, RoutedEventArgs e)
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		InfoTB.Text = GetInfoText();
	}

	private void FinalizeTracer_Closing(object sender, CancelEventArgs e)
	{
		_timer.Stop();
	}

	private void FinalizeTracer_Loaded(object sender, RoutedEventArgs e)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected Obj, but got Unknown
		InfoTB.Text = GetInfoText();
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 1), (DispatcherPriority)9, (EventHandler)new SafeEventHandler<EventArgs>(TimerEventHandler).Handler, ((DispatcherObject)this).Dispatcher);
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		InfoTB.Text = GetInfoText();
	}
}
