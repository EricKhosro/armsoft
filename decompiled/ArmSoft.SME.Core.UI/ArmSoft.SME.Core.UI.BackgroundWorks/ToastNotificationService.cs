using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using DevExpress.Data;
using DevExpress.Mvvm.UI;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class ToastNotificationService
{
	[Guid("91E68953-4F1A-42C1-8144-1DFCADED3318")]
	[ComVisible(true)]
	public class WindowsNotificationActivator : ToastNotificationActivator
	{
		public override void OnActivate(string arguments, Dictionary<string, string> data)
		{
			((DispatcherObject)Application.Current).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				WindowsActivator();
			}), Array.Empty<object>());
		}
	}

	private static NotificationService _notificationService;

	public static NotificationService ProgramNotificationService
	{
		get
		{
			if (_notificationService == null)
			{
				_notificationService = new NotificationService
				{
					PredefinedNotificationTemplate = NotificationTemplate.ShortHeaderAndLongText,
					PredefinedNotificationDuration = PredefinedNotificationDuration.Default,
					CustomNotificationPosition = NotificationPosition.BottomRight,
					CustomNotificationTemplate = (DataTemplate)Settings.GenericResources["CustomNotificationTemplate"],
					UseWin8NotificationsIfAvailable = false
				};
			}
			return _notificationService;
		}
	}

	public static NotificationService WindowsNotificationService
	{
		get
		{
			if (_notificationService == null)
			{
				_notificationService = new NotificationService
				{
					ApplicationIconPath = Settings.AppIconPath,
					PredefinedNotificationTemplate = NotificationTemplate.LongHeaderAndShortText,
					PredefinedNotificationDuration = PredefinedNotificationDuration.Long,
					UseWin8NotificationsIfAvailable = true,
					ApplicationActivator = typeof(WindowsNotificationActivator),
					CreateApplicationShortcut = false,
					ApplicationName = Settings.ProgramName,
					ApplicationId = Settings.ProgramName
				};
			}
			return _notificationService;
		}
	}

	internal static void WindowsActivator()
	{
		Application.Current.MainWindow.Activate();
		if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
		{
			Application.Current.MainWindow.WindowState = WindowState.Maximized;
		}
	}

	public static void RegisterWindowsNotificationActivator()
	{
		if (!ShellHelper.IsApplicationShortcutExist(Settings.ProgramName))
		{
			ShellHelper.RegisterComServer(typeof(WindowsNotificationActivator));
			ShellHelper.TryCreateShortcut(Settings.ProgramName, Settings.ProgramName, null, typeof(WindowsNotificationActivator));
		}
	}
}
