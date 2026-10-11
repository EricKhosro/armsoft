using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Helpers.Backups;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class FileTransferProgressWindow : ThemedWindow, IProgress<ProgressIndicationValue>, IComponentConnector
{
	private readonly FilesManager _filesManager;

	private readonly ProgressStep _progressStep;

	public FileTransferProgressWindow(UploadDownloadRequest request)
	{
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		if (File.Exists(request.DestinationFileName))
		{
			using (new FileStream(request.DestinationFileName, FileMode.Truncate, FileAccess.Write))
			{
			}
		}
		_filesManager = new FilesManager(request, this);
		_progressStep = request.ProgressStep;
		Loaded += new SafeEventHandler<RoutedEventArgs>(FileTransferWindow_Loaded).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(FileTransferProgressWindow_Closing).Handler;
		OKButton.Click += new SafeEventHandler<RoutedEventArgs>(OKButton_Click).Handler;
	}

	private void FileTransferWindow_Loaded(object sender, RoutedEventArgs e)
	{
		if (_progressStep == ProgressStep.ImportBackupFile)
		{
			Title = ArmSoft.SME.Common.Resources.BackupImport;
		}
		else
		{
			Title = ArmSoft.SME.Common.Resources.BackupExport;
		}
		((DispatcherObject)this).Dispatcher.Invoke((Action)(async () =>
		{
			await Start();
		}));
	}

	private async Task Start()
	{
		_ = 1;
		try
		{
			if (_progressStep == ProgressStep.ImportBackupFile)
			{
				await _filesManager.UploadBackupFileAsync();
			}
			else
			{
				await _filesManager.DownloadBackupFileAsync();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			OKButton.IsEnabled = true;
		}
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
			FileTransfer.Value = value.Percent;
		}
	}

	private void OKButton_Click(object sender, RoutedEventArgs e)
	{
		Close();
	}

	private void FileTransferProgressWindow_Closing(object sender, CancelEventArgs e)
	{
		if (!OKButton.IsEnabled)
		{
			e.Cancel = true;
		}
	}
}
