using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.DataViewsData;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class BackupRestoreProgressWindow : ThemedWindow, IProgress<ProgressIndicationValue>, IComponentConnector
{
	private readonly BackupsManager _backupsManager;

	private readonly OperationTypes _requestType;

	private readonly bool _autoCloseMode;

	private readonly bool _doNotArchiveBackup;

	public BackupRestoreProgressWindow(BackupRestoreRequest request, bool isAutoClose = false)
	{
		InitializeComponent();
		if (Settings.LoginService.DemoMode)
		{
			decimal? num = new BackupsDataProvider().GetRows(new BackupsDataProvider.Param
			{
				ServiceAddress = request.ServiceAddress
			}).FirstOrDefault((BackupsDataProvider.DataRow p) => p.FileName == request.FileName)?.Size;
			decimal num2 = Settings.ParametersService.DemoModeDbSizeMB();
			if ((num.GetValueOrDefault() > num2) & num.HasValue)
			{
				throw new ASException(string.Format(ArmSoft.SME.Common.Resources.DatabaseSizeLimitInDemo, Settings.ParametersService.DemoModeDbSizeMB()));
			}
		}
		Owner = Settings.HostingEnvironment;
		LDBName.Label = ArmSoft.SME.Common.Resources.Database;
		TEDBName.Text = request.DatabaseName;
		_requestType = request.Type;
		_doNotArchiveBackup = request.DoNotArchiveBackup;
		_backupsManager = new BackupsManager(request, this);
		_autoCloseMode = isAutoClose;
		Loaded += new SafeEventHandler<RoutedEventArgs>(Window_Loaded).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(Window_Closing).Handler;
		CloseButton.Click += new SafeEventHandler<RoutedEventArgs>(CloseButton_Click).Handler;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		if (_requestType == OperationTypes.Backup)
		{
			Title = ArmSoft.SME.Common.Resources.BackupInfo;
			LProgressBar1.Label = ArmSoft.SME.Common.Resources.CreatingBackup;
			if (_doNotArchiveBackup)
			{
				LProgressBar2.Visibility = Visibility.Collapsed;
			}
			else
			{
				LProgressBar2.Label = ArmSoft.SME.Common.Resources.CreatingArchive;
			}
		}
		else
		{
			Title = ArmSoft.SME.Common.Resources.RestoreInfo;
			if (_doNotArchiveBackup)
			{
				LProgressBar1.Visibility = Visibility.Collapsed;
			}
			else
			{
				LProgressBar1.Label = ArmSoft.SME.Common.Resources.ExtractingArchive;
			}
			LProgressBar2.Label = ArmSoft.SME.Common.Resources.Restoring;
		}
		((DispatcherObject)this).Dispatcher.Invoke((Action)(async () =>
		{
			await Start();
		}));
	}

	private async Task Start()
	{
		if (_requestType == OperationTypes.Backup)
		{
			try
			{
				await _backupsManager.CreateBackupAsync();
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
			}
			if (ProgressBar1.Value == 100.0)
			{
				Title = ArmSoft.SME.Common.Resources.BackupSuccessful;
			}
			else
			{
				Title = Title + " - " + ArmSoft.SME.Common.Resources.ErrorWord;
			}
		}
		else
		{
			try
			{
				await _backupsManager.RestoreBackupAsync();
			}
			catch (Exception ex2)
			{
				Settings.ErrorHandler(ex2);
			}
			Title = ((ProgressBar2.Value == 100.0) ? ArmSoft.SME.Common.Resources.RestoreSuccessful : ArmSoft.SME.Common.Resources.RestoreError);
		}
		CloseButton.IsEnabled = true;
		if (_autoCloseMode)
		{
			Close();
		}
	}

	public void Report(ProgressIndicationValue value)
	{
		if (value.UIMode)
		{
			value.UIMode = false;
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<ProgressIndicationValue>(Report), (DispatcherPriority)7, new object[1] { value });
		}
		else if (value.ProgressStep == ProgressStep.CreateBackup || value.ProgressStep == ProgressStep.ExtractBackupFile)
		{
			ProgressBar1.Value = value.Percent;
		}
		else if (value.ProgressStep == ProgressStep.ZIPBackupFile || value.ProgressStep == ProgressStep.RestoreBackup)
		{
			ProgressBar2.Value = value.Percent;
		}
	}

	private void CloseButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		if (!CloseButton.IsEnabled)
		{
			e.Cancel = true;
		}
	}
}
