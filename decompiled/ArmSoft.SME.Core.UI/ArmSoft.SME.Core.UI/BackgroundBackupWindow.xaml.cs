using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using ArmSoft.SME.Core.UI.StandartWindow;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Dropbox.Api;

namespace ArmSoft.SME.Core.UI;

public partial class BackgroundBackupWindow : ThemedWindow, IProgress<ProgressIndicationValue>, IComponentConnector
{
	public enum TransferModes
	{
		None = 1,
		DropBox,
		LocalDirectory
	}

	private enum ProgressState
	{
		Started,
		Processing,
		Finished,
		FinishedBackup,
		Aborting,
		AbortingBackup
	}

	private ProgressState _state;

	private readonly DispatcherTimer _timer;

	private readonly DateTime _startTime;

	private readonly CancellationTokenSource _cts;

	private readonly string _accessToken;

	private readonly TransferModes _transferMode;

	private readonly List<string> _databases;

	private readonly string _localDirectoryPath;

	private BackupsManager _backupsManager;

	public BackgroundBackupWindow(List<string> databases, TransferModes mode)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected Obj, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_databases = databases;
		mode = ((mode == TransferModes.DropBox) ? TransferModes.None : mode);
		_transferMode = mode;
		switch (mode)
		{
		case TransferModes.DropBox:
			_accessToken = Settings.ParametersService.DropBoxAccountToken();
			break;
		case TransferModes.LocalDirectory:
			_localDirectoryPath = Settings.ParametersService.LocalBackUpPath();
			if (Directory.Exists(_localDirectoryPath))
			{
				DropBoxImage.Source = ImageService.GetImage("folder_open_32_disabled.svg");
				break;
			}
			_transferMode = TransferModes.None;
			ArrowToDropBoxImage.Visibility = Visibility.Collapsed;
			DropBoxImage.Visibility = Visibility.Collapsed;
			break;
		default:
			ArrowToDropBoxImage.Visibility = Visibility.Collapsed;
			DropBoxImage.Visibility = Visibility.Collapsed;
			break;
		}
		Icon = Settings.HostingEnvironment.Icon;
		_startTime = DateTime.Now;
		TimeBlock.Text = "00:00";
		Cancelling.Text = ArmSoft.SME.Common.Resources.CancellingProcessMsg;
		_cts = new CancellationTokenSource();
		_state = ProgressState.Started;
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
		Title = ArmSoft.SME.Common.Resources.AutoBackUp;
		TB_DBName.Text = databases[0];
		ButtonCancel.IsEnabled = true;
		ButtonCancel.Content = ArmSoft.SME.Common.Resources.StopWord;
		ButtonCancel.Click += ButtonCancel_Click;
		ProgressBar.Minimum = 0.0;
		ProgressBar.Maximum = 100.0;
		ProgressBar.Value = 0.0;
		ProgressBar.ContentDisplayMode = ContentDisplayMode.Value;
		ProgressBar.IsPercent = true;
		ProgressBar.DisplayFormatString = "p0";
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		((DispatcherObject)this).Dispatcher.Invoke((Action)(async () =>
		{
			await Start();
		}));
		Closing += Window_Closing;
	}

	private async Task Start()
	{
		SettingsHelper.IsOSVersionWindows7OrGreater();
		string database = "";
		try
		{
			_state = ProgressState.Processing;
			foreach (string database2 in _databases)
			{
				database = database2;
				string optionAutoBackUpTab = ArmSoft.SME.Common.Resources.OptionAutoBackUpTab;
				string fileName = $"{database} {DateTime.Now.FormatYYYYMMDD()} {DateTime.Now.FormatHHMMSS()}";
				BackupRestoreRequest request = new BackupRestoreRequest
				{
					Type = OperationTypes.Backup,
					ServiceAddress = Settings.LoginService.EdgeServiceAddress,
					IsAutoBackup = true,
					DoNotArchiveBackup = true,
					DatabaseName = database,
					Comment = optionAutoBackUpTab,
					FileName = fileName
				};
				_backupsManager = new BackupsManager(request, this);
				if (_transferMode == TransferModes.DropBox)
				{
					ArrowToDropBoxImage.Source = ImageService.GetImage("arrow_right_16_disabled.svg");
					DropBoxImage.Source = ImageService.GetImage("DropBox_32_disabled.svg");
				}
				else if (_transferMode == TransferModes.LocalDirectory)
				{
					ArrowToDropBoxImage.Source = ImageService.GetImage("arrow_right_16_disabled.svg");
					DropBoxImage.Source = ImageService.GetImage("folder_open_32_disabled.svg");
				}
				TB_DBName.Text = database;
				await _backupsManager.CreateBackupFileAsync();
				if (!_cts.IsCancellationRequested)
				{
					_state = ProgressState.FinishedBackup;
				}
				if (_cts.IsCancellationRequested)
				{
					if (_state != ProgressState.AbortingBackup)
					{
						_backupsManager.DeleteBackupFile();
					}
					_cts.Token.ThrowIfCancellationRequested();
				}
				_backupsManager.UpdateCloudDbLastBackupDate();
				_cts.Token.ThrowIfCancellationRequested();
				try
				{
					if (_transferMode == TransferModes.DropBox)
					{
						ArrowToDropBoxImage.Source = ImageService.GetImage("arrow_right_16.svg");
						List<string> files = await new FilesManager(new UploadDownloadRequest
						{
							ProgressStep = ProgressStep.PrepareFileForUpload,
							ServiceAddress = Settings.LoginService.EdgeServiceAddress,
							SourceFileName = fileName + "." + Settings.BackupExtension
						}, this).DownloadBackupFileAsync(10, copyToTemp: true, _cts.Token);
						DropBoxImage.Source = ImageService.GetImage("DropBox_32.svg");
						using DropboxClient dbx = new DropboxClient(_accessToken);
						await DropBoxManager.UploadToDropBox(dbx, files, fileName + "." + Settings.BackupExtension, this, _cts.Token);
					}
					else if (_transferMode == TransferModes.LocalDirectory)
					{
						ArrowToDropBoxImage.Source = ImageService.GetImage("arrow_right_16.svg");
						DropBoxImage.Source = ImageService.GetImage("folder_open_32.svg");
						await new FilesManager(new UploadDownloadRequest
						{
							ProgressStep = ProgressStep.CopyToLocalFile,
							ServiceAddress = Settings.LoginService.EdgeServiceAddress,
							SourceFileName = fileName + "." + Settings.NewBackupExtension,
							DestinationFileName = Path.Combine(_localDirectoryPath, fileName + "." + Settings.NewBackupExtension)
						}, this).DownloadBackupFileAsync(1, copyToTemp: false, _cts.Token);
					}
					ShowNotification(database, ArmSoft.SME.Common.Resources.Backup, string.Format(ArmSoft.SME.Common.Resources.BackUpCompleted, database), null);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					ShowNotification(database, ArmSoft.SME.Common.Resources.Backup, string.Format(ArmSoft.SME.Common.Resources.BackUpCompleted, database), ex2);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex4)
		{
			ShowNotification(database, ArmSoft.SME.Common.Resources.Backup, string.Format(ArmSoft.SME.Common.Resources.BackUpCompleted, database), ex4);
		}
		_timer.Stop();
		_state = ProgressState.Finished;
		Settings.HostingEnvironment.AutoBackupWindow = null;
		Close();
	}

	private static void ShowNotification(string dbName, string caption, string message, Exception ex)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
		{
			Caption = caption,
			Text = message,
			HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
		};
		if (ex == null)
		{
			toastNotificationViewModel.Image = ImageService.GetImage("database_backup_32.svg");
			toastNotificationViewModel.HistoryItem.Icon = "database_backup_16.svg";
		}
		else
		{
			toastNotificationViewModel.Image = ImageService.GetImage("error_32.svg");
			toastNotificationViewModel.HistoryItem.Icon = "error_16.svg";
		}
		toastNotificationViewModel.HistoryItem.Type = 4;
		if (ex == null)
		{
			toastNotificationViewModel.HistoryItem.Description = message;
		}
		else
		{
			toastNotificationViewModel.HistoryItem.Description = message;
			toastNotificationViewModel.HistoryItem.Details = ex.Message;
		}
		toastNotificationViewModel.HistoryItem.Id = "Backup_" + dbName;
		Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
	}

	private void ButtonCancel_Click(object sender, RoutedEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (ButtonCancel.IsEnabled && _state != ProgressState.Finished)
			{
				ButtonCancel.IsEnabled = false;
				ButtonCancel.Content = ArmSoft.SME.Common.Resources.Aborting;
				if (_state != ProgressState.FinishedBackup)
				{
					_backupsManager.StopBackup();
					_state = ProgressState.AbortingBackup;
				}
				else
				{
					_state = ProgressState.Aborting;
				}
				Cancelling.Visibility = Visibility.Visible;
				_cts.Cancel();
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				Settings.ErrorHandler(ex3);
			}), Array.Empty<object>()).Wait();
		}
	}

	private void Window_Closing(object sender, CancelEventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_state != ProgressState.Finished)
			{
				e.Cancel = true;
			}
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				Settings.ErrorHandler(ex3);
			}), Array.Empty<object>()).Wait();
		}
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		TimeSpan timeSpan = DateTime.Now - _startTime;
		TimeBlock.Text = timeSpan.ToString("mm\\:ss");
	}

	public void Report(ProgressIndicationValue value)
	{
		if (value.UIMode)
		{
			value.UIMode = false;
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<ProgressIndicationValue>(Report), (DispatcherPriority)7, new object[1] { value });
		}
		else
		{
			ProgressBar.Value = value.Percent;
		}
	}
}
