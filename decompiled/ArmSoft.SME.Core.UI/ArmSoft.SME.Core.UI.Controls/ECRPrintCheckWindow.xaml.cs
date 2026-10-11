using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using DevExpress.Xpf.Core;

namespace ArmSoft.SME.Core.UI.Controls;

public partial class ECRPrintCheckWindow : ThemedWindow, IComponentConnector
{
	public enum ECROperation
	{
		Idle,
		Connect,
		Login,
		Printing,
		Disconnect,
		Logout
	}

	public enum ECRCheckType
	{
		Standard,
		Return,
		PrePayment,
		CashInOut,
		Report
	}

	public delegate void PrintingStartedOrErrorHappenedEventHandler(string checkNumber, string errorMessage);

	private DispatcherTimer _timer;

	private DateTime _timerStart;

	private int _timeToLive;

	private readonly ImageSource _imageSucceeded;

	private readonly ImageSource _imageRun;

	private readonly ImageSource _imageFailed;

	private readonly ImageSource _imageNotStarted;

	private ECROperation _currentlyInProcess;

	private bool _printFailed;

	private CancellationTokenSource _cts;

	private readonly int _ecrOperator;

	private readonly string _ecrOperatorPassword;

	private readonly bool _showECRTracking;

	private readonly object _request;

	private readonly Action<ECRPrintCheckWindow, Utilities.PrintReceiptResponse> _printOnOtherPrinter;

	public ECRCheckType CheckType { get; private set; }

	public ArmSoft.ECRConnector.ECR ECR { get; private set; }

	public string CheckNumber { get; set; }

	public string TaxCode { get; set; }

	public string CompanyName { get; set; }

	public string CompanyAddress { get; set; }

	public string ECRCrn { get; set; }

	public bool Succeeded
	{
		get
		{
			if (!_showECRTracking || !ShowDialog().Value)
			{
				return !_printFailed;
			}
			return true;
		}
	}

	public event PrintingStartedOrErrorHappenedEventHandler PrintingStartedOrErrorHappened;

	public ECRPrintCheckWindow(Window owner, ArmSoft.ECRConnector.ECR ecr, string ecrOperator, string ecrOperatorPassword, bool showECRTracking, object request, ECRCheckType checkType, Action<ECRPrintCheckWindow, Utilities.PrintReceiptResponse> printOnOtherPrinter = null, PrintingStartedOrErrorHappenedEventHandler printingStartedOrErrorHappened = null)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		Loaded += new SafeEventHandler<RoutedEventArgs>(ECRPrintCheckWindow_Loaded).Handler;
		Closing += new SafeEventHandler<EventArgs>(ECRWaitingWindow_Closing).Handler;
		InitializeComponent();
		if (!Settings.IsWin11Theme)
		{
			WindowStyle = WindowStyle.ToolWindow;
		}
		CancelButton.Click += new SafeEventHandler<RoutedEventArgs>(CancelButton_Click).Handler;
		LabelCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		_imageSucceeded = ImageService.GetImage("check_20.svg");
		_imageRun = ImageService.GetImage("run_20.svg");
		_imageFailed = ImageService.GetImage("delete_20.svg");
		_imageNotStarted = ImageService.GetImage("check_disable_20.svg");
		ConnectImage.Source = _imageNotStarted;
		LoginImage.Source = _imageNotStarted;
		PrintingImage.Source = _imageNotStarted;
		LogoutImage.Source = _imageNotStarted;
		DisconnectImage.Source = _imageNotStarted;
		ImageCancel.Source = ImageService.GetImage("cancel_16.svg");
		_currentlyInProcess = ECROperation.Idle;
		_printFailed = false;
		_cts = new CancellationTokenSource();
		ECR = ecr;
		_ecrOperator = int.Parse(ecrOperator);
		_ecrOperatorPassword = ecrOperatorPassword;
		_showECRTracking = showECRTracking;
		_request = request;
		CheckType = checkType;
		CheckNumber = string.Empty;
		TaxCode = string.Empty;
		CompanyName = string.Empty;
		CompanyAddress = string.Empty;
		ECRCrn = string.Empty;
		Title = ArmSoft.SME.Common.Resources.ECRC;
		Owner = owner;
		if (printingStartedOrErrorHappened != null)
		{
			PrintingStartedOrErrorHappened += printingStartedOrErrorHappened;
		}
		_printOnOtherPrinter = printOnOtherPrinter;
		if (showECRTracking)
		{
			ConfigureTimer();
			((DispatcherObject)this).Dispatcher.BeginInvoke((Delegate)(Action)(async () =>
			{
				try
				{
					await PrintCheckAsync();
				}
				catch (Exception ex)
				{
					Settings.ErrorHandler(ex);
				}
			}), Array.Empty<object>());
		}
		else
		{
			PrintCheck();
		}
	}

	private void ECRPrintCheckWindow_Loaded(object sender, RoutedEventArgs e)
	{
		ControlBoxButtonSet = ControlBoxButtons.None;
	}

	private void CancelButton_Click(object sender, RoutedEventArgs e)
	{
		StopTimer();
		CancelButton.IsEnabled = false;
		LabelCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		_cts.Cancel();
	}

	private void ECRWaitingWindow_Closing(object sender, EventArgs e)
	{
		StopTimer();
	}

	private void Timer_Tick(object sender, EventArgs e)
	{
		int num = (int)DateExtension.DateDiff(DateExtension.Interval.Second, _timerStart, DateTime.Now);
		if (num < _timeToLive)
		{
			LabelCancel.Content = $"{ArmSoft.SME.Common.Resources.Cancel} ({_timeToLive - num})";
			return;
		}
		if (num == _timeToLive)
		{
			LabelCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
			return;
		}
		StopTimer();
		CancelButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
	}

	private void ConfigureTimer(int timeToLive = 180)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected Obj, but got Unknown
		_timeToLive = timeToLive;
		if (_timeToLive > 0)
		{
			_timer = new DispatcherTimer
			{
				Interval = new TimeSpan(0, 0, 0, 0, 10)
			};
			_timerStart = DateTime.Now;
			LabelCancel.Content = $"{ArmSoft.SME.Common.Resources.Cancel} ({_timeToLive})";
			_timer.Tick += new SafeEventHandler<EventArgs>(Timer_Tick).Handler;
			_timer.Start();
		}
		else
		{
			LabelCancel.Content = ArmSoft.SME.Common.Resources.Cancel;
		}
	}

	public void ResetTimer(int timeToLive)
	{
		_cts = new CancellationTokenSource();
		ConfigureTimer(timeToLive);
	}

	private void StopTimer()
	{
		if (_timer != null && _timer.IsEnabled)
		{
			_timer.Stop();
		}
	}

	private void RunOperation(ECROperation operation)
	{
		switch (operation)
		{
		case ECROperation.Connect:
			ConnectLabel.FontWeight = FontWeights.Bold;
			ConnectImage.Source = _imageRun;
			break;
		case ECROperation.Login:
			LoginLabel.FontWeight = FontWeights.Bold;
			LoginImage.Source = _imageRun;
			break;
		case ECROperation.Printing:
			PrintingLabel.FontWeight = FontWeights.Bold;
			PrintingImage.Source = _imageRun;
			PrintingStartedOrErrorHappened?.Invoke("XXXXXXXX", string.Empty);
			break;
		case ECROperation.Logout:
			LogoutLabel.FontWeight = FontWeights.Bold;
			LogoutImage.Source = _imageRun;
			break;
		case ECROperation.Disconnect:
			DisconnectLabel.FontWeight = FontWeights.Bold;
			DisconnectImage.Source = _imageRun;
			break;
		}
		_currentlyInProcess = operation;
	}

	private void OperationSucceeded(ECROperation operation)
	{
		switch (operation)
		{
		case ECROperation.Connect:
			ConnectImage.Source = _imageSucceeded;
			break;
		case ECROperation.Login:
			LoginImage.Source = _imageSucceeded;
			break;
		case ECROperation.Printing:
			PrintingImage.Source = _imageSucceeded;
			break;
		case ECROperation.Logout:
			LogoutImage.Source = _imageSucceeded;
			break;
		case ECROperation.Disconnect:
			DisconnectImage.Source = _imageSucceeded;
			break;
		}
	}

	private void OperationFailed(ECROperation operation)
	{
		switch (operation)
		{
		case ECROperation.Connect:
			ConnectImage.Source = _imageFailed;
			break;
		case ECROperation.Login:
			LoginImage.Source = _imageFailed;
			break;
		case ECROperation.Printing:
			PrintingImage.Source = _imageFailed;
			break;
		case ECROperation.Logout:
			LogoutImage.Source = _imageFailed;
			break;
		case ECROperation.Disconnect:
			DisconnectImage.Source = _imageFailed;
			break;
		}
	}

	private void GetDataFromECRResponse(Utilities.PrintReceiptResponse response)
	{
		CheckNumber = response.rseq.ToString().PadLeft(8, '0');
		TaxCode = response.tin;
		CompanyName = response.taxpayer;
		CompanyAddress = response.address;
		ECRCrn = response.crn;
	}

	private void HandleFailure()
	{
		OperationFailed(_currentlyInProcess);
		_printFailed = true;
		ResetTimer(20);
	}

	private void PrintCheck()
	{
		bool flag = false;
		bool flag2 = false;
		try
		{
			ECR.Connect();
			flag = true;
			ECR.OperatorLogin(_ecrOperator, _ecrOperatorPassword);
			flag2 = true;
			Utilities.PrintReceiptResponse printReceiptResponse = null;
			switch (CheckType)
			{
			case ECRCheckType.Standard:
			case ECRCheckType.PrePayment:
				printReceiptResponse = ECR.PrintReceipt((Utilities.PrintReceiptRequest)_request);
				GetDataFromECRResponse(printReceiptResponse);
				break;
			case ECRCheckType.Return:
				printReceiptResponse = ECR.PrintReturnReceipt((Utilities.PrintReturnReceiptRequest)_request);
				GetDataFromECRResponse(printReceiptResponse);
				break;
			case ECRCheckType.CashInOut:
				ECR.PrintCashInOut((Utilities.PrintCashInOutRequest)_request);
				CheckNumber = new string('0', 8);
				TaxCode = string.Empty;
				CompanyName = string.Empty;
				CompanyAddress = string.Empty;
				ECRCrn = string.Empty;
				break;
			}
			if (_printOnOtherPrinter != null)
			{
				_printOnOtherPrinter(this, printReceiptResponse);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
			_printFailed = true;
		}
		try
		{
			if (flag2)
			{
				ECR.OperatorLogout();
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			if (flag)
			{
				ECR.DisConnect();
			}
		}
	}

	public async Task PrintCheckAsync()
	{
		bool connected = false;
		bool loggedIn = false;
		try
		{
			RunOperation(ECROperation.Connect);
			await ECR.ConnectAsync(_cts.Token);
			connected = true;
			OperationSucceeded(ECROperation.Connect);
			RunOperation(ECROperation.Login);
			await ECR.OperatorLoginAsync(_ecrOperator, _ecrOperatorPassword, _cts.Token);
			loggedIn = true;
			OperationSucceeded(ECROperation.Login);
			RunOperation(ECROperation.Printing);
			Utilities.PrintReceiptResponse response = null;
			switch (CheckType)
			{
			case ECRCheckType.Standard:
			case ECRCheckType.PrePayment:
				response = await ECR.PrintReceiptAsync((Utilities.PrintReceiptRequest)_request, _cts.Token);
				GetDataFromECRResponse(response);
				break;
			case ECRCheckType.Return:
				response = await ECR.PrintReturnReceiptAsync((Utilities.PrintReturnReceiptRequest)_request, _cts.Token);
				GetDataFromECRResponse(response);
				break;
			case ECRCheckType.CashInOut:
				await ECR.PrintCashInOutAsync((Utilities.PrintCashInOutRequest)_request, _cts.Token);
				CheckNumber = new string('0', 8);
				TaxCode = string.Empty;
				CompanyName = string.Empty;
				CompanyAddress = string.Empty;
				ECRCrn = string.Empty;
				break;
			}
			if (_printOnOtherPrinter != null)
			{
				_printOnOtherPrinter(this, response);
			}
			OperationSucceeded(ECROperation.Printing);
		}
		catch (OperationCanceledException)
		{
			HandleFailure();
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(new ASException(ex2.Message));
			HandleFailure();
			bool flag = ex2 is ApiException { Code: var code } && code == 20007;
			if (loggedIn && !flag)
			{
				PrintingStartedOrErrorHappened?.Invoke(string.Empty, ex2.Message);
			}
		}
		try
		{
			if (loggedIn)
			{
				RunOperation(ECROperation.Logout);
				await ECR.OperatorLogoutAsync(_cts.Token);
				OperationSucceeded(ECROperation.Logout);
			}
		}
		catch (Exception)
		{
			OperationFailed(_currentlyInProcess);
		}
		finally
		{
			if (connected)
			{
				RunOperation(ECROperation.Disconnect);
				ECR.DisConnect();
				OperationSucceeded(ECROperation.Disconnect);
			}
		}
		DialogResult = !_printFailed;
	}
}
