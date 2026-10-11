using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Constants;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(44)]
public class EMarkReceiptDialogWindow : DirectoryDialogWindow<EMarkReceiptApp>
{
	private TextEditExt _name;

	private EMarkApiClient _eMarkApiClient = new EMarkApiClient(new EMarkOptions());

	private DatamatrixResponse _datamatrixResponse;

	private DispatcherTimer _activationTimer;

	private DateTime _activationStartTime;

	private static readonly TimeSpan _activationTimeout = TimeSpan.FromMinutes(5L);

	public EMarkReceiptDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public EMarkReceiptDialogWindow(EMarkReceiptApp directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	protected override void ConstructUI()
	{
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected Obj, but got Unknown
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		try
		{
			if (Mode == DialogWindowMode.NewMode)
			{
				_datamatrixResponse = _eMarkApiClient.GetDatamatrix();
				byte[] imagebytes = _datamatrixResponse.imagebytes;
				ImageEdit myControl = new ImageEdit
				{
					EditValue = imagebytes,
					ShowMenu = false,
					ShowBorder = false,
					HorizontalAlignment = HorizontalAlignment.Center,
					VerticalAlignment = VerticalAlignment.Center,
					Width = 400.0,
					Height = 250.0
				};
				AddControl("DataMatrix", "", myControl);
				SolidColorBrush blueBrush = ColorsAS.Foreground.BlueBrush;
				AddTextBlock(null, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptAddText, TextBlockImage.None).Foreground = blueBrush;
				TextBlock textBlock = AddTextBlock(null, ArmSoft.SME.Common.Resources.More, TextBlockImage.None);
				textBlock.HorizontalAlignment = HorizontalAlignment.Left;
				textBlock.Foreground = blueBrush;
				textBlock.TextDecorations = TextDecorations.Underline;
				textBlock.Cursor = Cursors.Hand;
				textBlock.MouseLeftButtonDown += (object s, MouseButtonEventArgs e) =>
				{
					Process.Start(new ProcessStartInfo("https://www.armsoft.am/banners/EMarkReceipt.htm")
					{
						UseShellExecute = true
					});
				};
			}
			if (Mode == DialogWindowMode.NewMode)
			{
				_activationTimer = new DispatcherTimer
				{
					Interval = TimeSpan.FromSeconds(1L)
				};
				_activationTimer.Tick += ActivationTimer_Tick;
			}
		}
		catch (Exception ex)
		{
			if (ex is EMarkApiException ex2)
			{
				throw new ASException(ex2.Message);
			}
			Settings.ErrorHandler(ex);
		}
	}

	protected override void OnContentRendered(EventArgs e)
	{
		base.OnContentRendered(e);
		if (Mode == DialogWindowMode.NewMode)
		{
			_activationStartTime = DateTime.Now;
			_activationTimer.Start();
		}
	}

	private void ActivationTimer_Tick(object sender, EventArgs e)
	{
		_activationTimer.Stop();
		try
		{
			if (DateTime.Now - _activationStartTime > _activationTimeout)
			{
				Close();
			}
			else if (_eMarkApiClient.CheckActivation().activated)
			{
				OnValidationWindow(okPressed: true);
			}
		}
		finally
		{
			if (IsVisible)
			{
				_activationTimer.Start();
			}
		}
	}

	protected override void OnClosed(EventArgs e)
	{
		if (Mode == DialogWindowMode.NewMode)
		{
			_activationTimer.Stop();
			_activationTimer.Tick -= ActivationTimer_Tick;
			_activationTimer = null;
		}
		base.OnClosed(e);
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (Mode == DialogWindowMode.NewMode)
		{
			_name.Text = Settings.ProgramName;
		}
		SetFocusOnActivation = ((Mode == DialogWindowMode.NewMode) ? ((TextEdit)_code) : ((TextEdit)_name));
	}

	protected override void DialogValidation(DialogWindowValidationArgs args)
	{
		try
		{
			if (Mode == DialogWindowMode.NewMode)
			{
				Directory.DeviceId = _datamatrixResponse.datamatrix_file_id;
			}
			else if (Mode == DialogWindowMode.EditMode)
			{
				_eMarkApiClient = new EMarkApiClient(new EMarkOptions
				{
					Datamatrix_file_id = Directory.DeviceId
				});
			}
			ActivationStatusResponse activationStatusResponse = _eMarkApiClient.CheckActivation();
			Directory.Token = activationStatusResponse.token;
			Directory.RefreshToken = activationStatusResponse.refresh_token;
			Directory.CRN = activationStatusResponse.crn;
			Directory.Enabled = activationStatusResponse.activated;
			if (!activationStatusResponse.activated)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationNotSucceededTryAgain, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
				args.isValid = false;
				return;
			}
			if (Mode == DialogWindowMode.EditMode)
			{
				RefreshTokenResponse refreshToken = _eMarkApiClient.GetRefreshToken();
				Directory.Token = refreshToken.token;
				Directory.RefreshToken = refreshToken.refresh_token;
			}
			if (_eMarkApiClient.UploadMetadata(Directory.DeviceId, _name.Text, ImageSourceToByteArray(), "ASTrade_128.png"))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationSucceeded, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk, MessageBoxResult.OK);
			}
			else
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.OperationNotSucceededTryAgain, Settings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
				args.isValid = false;
			}
			base.DialogValidation(args);
			if (!args.isValid)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			if (ex is EMarkApiException ex2)
			{
				throw new ASException(ex2.Message);
			}
			Settings.ErrorHandler(ex);
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		dvp.Update(Directory.Code);
	}

	public static byte[] ImageSourceToByteArray()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!(ImageService.GetImage("ASTrade_128.png") is BitmapSource source))
		{
			throw new InvalidOperationException("Only BitmapSource types are supported.");
		}
		PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
		pngBitmapEncoder.Frames.Add(BitmapFrame.Create(source));
		using MemoryStream memoryStream = new MemoryStream();
		pngBitmapEncoder.Save(memoryStream);
		return memoryStream.ToArray();
	}
}
