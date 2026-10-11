using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using Dropbox.Api;

namespace ArmSoft.SME.Core.UI;

public partial class DropBoxTransferProgressWindow : ThemedWindow, IProgress<ProgressIndicationValue>, IComponentConnector
{
	private enum ProgressState
	{
		Started,
		Processing,
		Finished,
		Aborting
	}

	private readonly DropBoxImportExportRequest _request;

	private DispatcherTimer _timer;

	private DateTime _startTime;

	private ProgressState _state;

	private CancellationTokenSource _cts;

	private bool _isFirstCall;

	public string AccessToken { get; set; }

	public string ServiceAddress { get; set; }

	public DropBoxTransferProgressWindow(DropBoxImportExportRequest request)
	{
		InitializeComponent();
		_request = request;
		Initialize();
		Activated += new SafeEventHandler<EventArgs>(BackupRestoreProgressWindowNew_Activated).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(BackupRestoreProgressWindowNew_Closing).Handler;
		Loaded += new SafeEventHandler<RoutedEventArgs>(BackupRestoreProgressWindowNew_Loaded).Handler;
		ButtonCancel.Click += new SafeEventHandler<RoutedEventArgs>(ButtonCancel_Click).Handler;
	}

	private void Initialize()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected Obj, but got Unknown
		Owner = Settings.HostingEnvironment;
		_isFirstCall = true;
		_cts = new CancellationTokenSource();
		_startTime = DateTime.Now;
		TimeBlock.Text = "00:00";
		_state = ProgressState.Started;
		_timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), (DispatcherPriority)9, (EventHandler)TimerEventHandler, ((DispatcherObject)this).Dispatcher);
	}

	private async void BackupRestoreProgressWindowNew_Activated(object sender, EventArgs e)
	{
		if (!_isFirstCall)
		{
			return;
		}
		try
		{
			_isFirstCall = false;
			_state = ProgressState.Processing;
			if (_request.Mode == DropBoxRequestModes.ExportToDropBox)
			{
				List<string> files = await new FilesManager(new UploadDownloadRequest
				{
					ProgressStep = ProgressStep.PrepareFileForUpload,
					ServerName = "",
					ServiceAddress = ServiceAddress,
					SourceFileName = _request.FileName
				}, this).DownloadBackupFileAsync(10, copyToTemp: true, _cts.Token);
				using DropboxClient dbx = new DropboxClient(AccessToken);
				await DropBoxManager.UploadToDropBox(dbx, files, _request.FileName, this, _cts.Token);
			}
			else if (_request.Mode == DropBoxRequestModes.ImportFromDropBox)
			{
				string sourceFileName = null;
				using (DropboxClient dbx = new DropboxClient(AccessToken))
				{
					sourceFileName = await DropBoxManager.DownloadFromDropBox(dbx, _request.FileName, 100000, this, _cts.Token);
				}
				await new FilesManager(new UploadDownloadRequest
				{
					ProgressStep = ProgressStep.ImportBackupFile,
					ServerName = "",
					ServiceAddress = ServiceAddress,
					SourceFileName = sourceFileName,
					DestinationFileName = _request.FileName
				}, this).UploadBackupFileAsync(_cts.Token);
			}
			_state = ProgressState.Finished;
			_timer.Stop();
			DialogResult = true;
		}
		catch (OperationCanceledException)
		{
			_state = ProgressState.Finished;
			_timer.Stop();
			DialogResult = false;
		}
		catch (Exception ex2)
		{
			_state = ProgressState.Finished;
			DialogResult = false;
			_timer.Stop();
			Settings.ErrorHandler(ex2);
		}
	}

	private void TimerEventHandler(object sender, EventArgs e)
	{
		TimeSpan timeSpan = DateTime.Now - _startTime;
		TimeBlock.Text = timeSpan.ToString("mm\\:ss");
	}

	private void ButtonCancel_Click(object sender, RoutedEventArgs e)
	{
		if (ButtonCancel.IsEnabled && _state != ProgressState.Finished)
		{
			ButtonCancel.IsEnabled = false;
			ButtonCancel.Content = ArmSoft.SME.Common.Resources.Aborting;
			if (_state == ProgressState.Processing)
			{
				_cts.Cancel();
			}
			_state = ProgressState.Aborting;
		}
	}

	private void BackupRestoreProgressWindowNew_Closing(object sender, CancelEventArgs e)
	{
		if (_state != ProgressState.Finished)
		{
			e.Cancel = true;
		}
	}

	private void BackupRestoreProgressWindowNew_Loaded(object sender, RoutedEventArgs e)
	{
		ConstructUI();
	}

	private void ConstructUI()
	{
		switch (_request.Mode)
		{
		case DropBoxRequestModes.ExportToDropBox:
		{
			Title = ArmSoft.SME.Common.Resources.ExportToDropBox;
			ProggressInfo.Text = ArmSoft.SME.Common.Resources.Database + " - " + _request.DBName;
			TextEdit proggressInfo = ProggressInfo;
			proggressInfo.Text = proggressInfo.Text + Environment.NewLine + ArmSoft.SME.Common.Resources.Comment + " - " + _request.DBCaption.Trim();
			proggressInfo = ProggressInfo;
			proggressInfo.Text = proggressInfo.Text + Environment.NewLine + ArmSoft.SME.Common.Resources.File + " - " + _request.FileName;
			ProggressInfo.Text += $"{Environment.NewLine}{ArmSoft.SME.Common.Resources.CompressedSizeMB} - {_request.CompSize}";
			break;
		}
		case DropBoxRequestModes.ImportFromDropBox:
			Title = ArmSoft.SME.Common.Resources.ImportFromDropBox;
			ProggressInfo.Text = ArmSoft.SME.Common.Resources.File + " - " + _request.FileName;
			ProggressInfo.Text += $"{Environment.NewLine}{ArmSoft.SME.Common.Resources.CompressedSizeMB} - {_request.CompSize}";
			break;
		}
		ButtonCancel.IsEnabled = true;
		ButtonCancel.Content = ArmSoft.SME.Common.Resources.StopWord;
		ProgressBar.Minimum = 0.0;
		ProgressBar.Maximum = 100.0;
		ProgressBar.Value = 0.0;
		ProgressBar.ContentDisplayMode = ContentDisplayMode.Value;
		ProgressBar.IsPercent = true;
		ProgressBar.DisplayFormatString = "p0";
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
