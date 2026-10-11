using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using ArmSoft.Cloud.Client.Edge;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Helpers;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI;

public partial class CreateDatabaseProgressWindow : ThemedWindow, IProgress<ProgressIndicationValue>, IComponentConnector
{
	private readonly CreateDatabaseManager _createDatabaseManager;

	public CreateDatabaseProgressWindow(DatabaseCreateRequest request, string edgeService)
	{
		InitializeComponent();
		Owner = Settings.HostingEnvironment;
		_createDatabaseManager = new CreateDatabaseManager(request, edgeService, this);
		Loaded += new SafeEventHandler<RoutedEventArgs>(Window_Loaded).Handler;
		Closing += new SafeEventHandler<CancelEventArgs>(Window_Closing).Handler;
		CloseButton.Click += new SafeEventHandler<RoutedEventArgs>(CloseButton_Click).Handler;
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		Title = ArmSoft.SME.Common.Resources.RestoreInfo;
		LProgressBar1.Label = ArmSoft.SME.Common.Resources.CreateDatabase;
		((DispatcherObject)this).Dispatcher.Invoke((Action)(async () =>
		{
			await Start();
		}));
	}

	private async Task Start()
	{
		try
		{
			await _createDatabaseManager.CreateDatabaseAsync();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		if (ProgressBar1.Value == 100.0)
		{
			Title = ArmSoft.SME.Common.Resources.OperationSucceeded;
		}
		else
		{
			Title = ArmSoft.SME.Common.Resources.RestoreError;
		}
		CloseButton.IsEnabled = true;
	}

	public void Report(ProgressIndicationValue value)
	{
		if (value.UIMode)
		{
			value.UIMode = false;
			((DispatcherObject)this).Dispatcher.Invoke((Delegate)new Action<ProgressIndicationValue>(Report), (DispatcherPriority)7, new object[1] { value });
		}
		else if (value.ProgressStep == ProgressStep.CreateDatabaseFromTemplate)
		{
			ProgressBar1.Value = value.Percent;
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
