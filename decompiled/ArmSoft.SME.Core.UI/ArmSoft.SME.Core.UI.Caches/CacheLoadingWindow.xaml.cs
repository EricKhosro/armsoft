using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using Serilog;

namespace ArmSoft.SME.Core.UI.Caches;

public partial class CacheLoadingWindow : ThemedWindow, IComponentConnector
{
	public class Data : BindableBase
	{
		public bool IsServiceCache { get; set; }

		public short CacheId { get; set; }

		public string Caption { get; set; }

		public string Time { get; set; }

		public string ErrorMsg { get; set; }

		public ImageSource StateIcon
		{
			get
			{
				return GetValue<ImageSource>("StateIcon");
			}
			set
			{
				SetValue(value, "StateIcon");
			}
		}
	}

	public ImageSource NoneIcon;

	public ImageSource StartIcon = ImageService.GetImage("arrow_right_16.svg");

	public ImageSource SuccessfulIcon = ImageService.GetImage("check_16.svg");

	public ImageSource FailedIcon = ImageService.GetImage("error_16.svg");

	private List<short> _inLocalStoredCacheIds;

	private const int _updateTimerMs = 500;

	private readonly BackgroundWorker _backgroundWorker;

	private readonly DispatcherTimer _timer;

	private readonly DispatcherTimer _phaseTimer;

	private DateTime _startDate;

	private DateTime _phaseStartDate;

	private readonly bool _executeByAsking;

	private ObservableCollection<Data> Source { get; } = new ObservableCollection<Data>();

	public CacheLoadingWindow(bool executeByAsking = false)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected Obj, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected Obj, but got Unknown
		InitializeComponent();
		_executeByAsking = executeByAsking;
		Owner = Settings.HostingEnvironment;
		Title = ArmSoft.SME.Common.Resources.LoadData;
		ASFontHelper.SetFontSettings(this);
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 500), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
		_timer.Stop();
		_phaseTimer = new DispatcherTimer((DispatcherPriority)9, ((DispatcherObject)this).Dispatcher)
		{
			Interval = new TimeSpan(0, 0, 0, 0, 500)
		};
		_backgroundWorker = new BackgroundWorker();
		_backgroundWorker.DoWork += BackgroundWorker_DoWork;
		_backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
		Loaded += Window_Loaded;
		ContentRendered += Window_ContentRendered;
		TimeTextBlock.Text = "00:00";
		FillSource();
		Caches.ItemsSource = Source;
		if (!_executeByAsking)
		{
			OkButton.Visibility = Visibility.Collapsed;
			CancelButton.Visibility = Visibility.Collapsed;
		}
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (!_executeByAsking)
		{
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)new Action(HideCloseButton), Array.Empty<object>());
		}
	}

	private void Window_ContentRendered(object sender, EventArgs e)
	{
		if (!_executeByAsking)
		{
			_backgroundWorker.RunWorkerAsync();
		}
	}

	protected void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			_startDate = DateTime.Now;
			_timer.Start();
			List<short> list = new List<short>();
			int i;
			bool success;
			string error = default;
			for (i = 0; i < Source.Count; i++)
			{
				_phaseStartDate = DateTime.Now;
				_phaseTimer.Tick += (object? s, EventArgs e2) =>
				{
					PhaseTimerEventHandler(i);
				};
				_phaseTimer.Start();
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
				{
					Source[i].StateIcon = StartIcon;
					Source[i].Time = "00:00";
					Caches.RefreshRow(i);
				}), Array.Empty<object>()).Wait();
				if (Source[i].IsServiceCache)
				{
					try
					{
						Settings.CacheFunctionsService.RefreshServiceCaches();
						success = true;
						error = "";
					}
					catch (Exception ex)
					{
						success = false;
						error = ex.Message;
						Log.ForContext<CacheLoadingWindow>().Error(ex, ex.Message);
					}
					list = Settings.CacheFunctionsService.GetChangedCaches();
				}
				else if (Source[i].CacheId > 0)
				{
					if (list.Contains(Source[i].CacheId))
					{
						ICacheFunctionsService cacheFunctionsService = Settings.CacheFunctionsService;
						ref string error2 = ref error;
						int num = 1;
						List<short> list2 = new List<short>(num);
						CollectionsMarshal.SetCount(list2, num);
						CollectionsMarshal.AsSpan(list2)[0] = Source[i].CacheId;
						success = cacheFunctionsService.RefreshCache(out error2, list2, null, checkChanging: false);
					}
					else
					{
						success = true;
						error = "";
					}
				}
				else
				{
					if (list != null && list.Count > 0)
					{
						success = Settings.CacheFunctionsService.RefreshCache(out error, list, _inLocalStoredCacheIds, checkChanging: false);
					}
					else
					{
						success = true;
						error = "";
					}
					try
					{
						Settings.ParametersService.Refresh();
						Settings.UserAccessService.LoadAllPermissionsIfNecessary();
					}
					catch (Exception ex2)
					{
						success = false;
						error = ex2.Message;
						Log.ForContext<CacheLoadingWindow>().Error(ex2, ex2.Message);
					}
				}
				((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
				{
					Source[i].ErrorMsg = error;
					Source[i].StateIcon = (success ? SuccessfulIcon : FailedIcon);
				}), Array.Empty<object>()).Wait();
				_phaseTimer.Stop();
			}
		}
		catch (Exception ex3)
		{
			Exception ex4 = ex3;
			Exception ex5 = ex4;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				Settings.ErrorHandler(ex5);
			}), Array.Empty<object>()).Wait();
		}
	}

	protected void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		_timer.Stop();
		_backgroundWorker.Dispose();
		Close();
	}

	private void FillSource()
	{
		Source.Add(new Data
		{
			IsServiceCache = true,
			Caption = ArmSoft.SME.Common.Resources.RowsLoadedInService,
			StateIcon = NoneIcon
		});
		_inLocalStoredCacheIds = (from p in Settings.CacheFunctionsService.CacheDefinitions.Values
			where p.StoreInLocalStorage
			select p.CacheId).ToList();
		foreach (CodeName item in SmartEnumHelper.GetSmartEnumCodeCaption(typeof(CacheIdSE), _inLocalStoredCacheIds.ConvertAll((short i) => i.ToString())))
		{
			Source.Add(new Data
			{
				CacheId = Convert.ToInt16(item.Code),
				Caption = item.Name,
				StateIcon = NoneIcon
			});
		}
		Source.Add(new Data
		{
			Caption = ArmSoft.SME.Common.Resources.OtherDirectories,
			StateIcon = NoneIcon
		});
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		try
		{
			TimeTextBlock.Text = (DateTime.Now - _startDate).FormatHHMMSSOrMMSS(":");
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void PhaseTimerEventHandler(int i)
	{
		try
		{
			Source[i].Time = (DateTime.Now - _phaseStartDate).FormatHHMMSSOrMMSS(":");
			if (string.IsNullOrWhiteSpace(Source[i].Time))
			{
				Source[i].Time = "00:00";
			}
			Caches.RefreshRow(i);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void OkButton_Click(object sender, RoutedEventArgs e)
	{
		OkButton.IsEnabled = false;
		CancelButton.IsEnabled = false;
		_backgroundWorker.RunWorkerAsync();
	}

	private void HideCloseButton()
	{
		ControlBoxButtonSet = ControlBoxButtons.None;
	}
}
