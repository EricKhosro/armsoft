using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Devices;
using ArmSoft.SME.Trade.UI.Devices.Terminal;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.UI.Directories;

[DirectoryDocumentWindow(43)]
public class ElScaleOrTerminalWindow : DirectoryDocumentWindow<ElScaleOrTerminal>
{
	private LayoutGroupExt _codeArea;

	private TextEditExt _name;

	private CheckEditExt _isActive;

	private DropDownControl _elType;

	private DropDownControl _elModel;

	private DropDownControl _priceList;

	private DropDownControl _asNameChoose;

	private DropDownControl _asCodeExport;

	private DropDownControl _group;

	private DropDownControl _segment;

	private CheckEditExt _exportForAdditionalQntUnit;

	private ButtonEditExt _exportFile;

	private LayoutGroupExt _ipAddressArea;

	private TextEditExt _ipAddress;

	private TextEditExt _password;

	private Button _checkConnection;

	private Button _searchSclae;

	private TextBlock _checkConnectionResult;

	private CheckEditExt _deleteAllItemsBeforeExporting;

	private LayoutGroupExt _labelNBarcodeNSettingsLG;

	private SpinEditExt _spinEditWeightLabelN;

	private SpinEditExt _spinEditWeightBarCodeN;

	private SpinEditExt _spinEditPieceLabelN;

	private SpinEditExt _spinEditPieceBarCodeN;

	private LayoutGroup _lgSettings;

	private ButtonInfo _browseButton;

	private Button _imageButton;

	protected override double WidthValue => 780.0;

	protected override double HeightValue => 700.0;

	public ElScaleOrTerminalWindow()
	{
	}

	public ElScaleOrTerminalWindow(ElScaleOrTerminal doc)
		: base(doc)
	{
	}

	protected override void ConstructUI()
	{
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		base.ConstructUI();
		LayoutGroupExt container = AddLayoutGroup(null, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt container2 = AddLayoutGroup(container, LayoutGroupView.Group, Orientation.Vertical);
		_codeArea = AddLayoutGroup(container2, LayoutGroupView.Group, Orientation.Horizontal);
		_code = AddButtonEditControl(_codeArea, Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_isActive = AddBooleanControl(_codeArea, Definition.Fields["IsActive"]);
		_imageButton = AddButton(container, "", "", "", "", isTabStop: false, HorizontalAlignment.Center);
		_imageButton.Width = 120.0;
		_imageButton.Height = 120.0;
		_imageButton.Padding = new Thickness(0.0);
		Grid grid = new Grid();
		Image image = new Image
		{
			Source = ImageService.GetImage("scale_CAS_CL.png", isAppPathPreferred: true, "Scales/"),
			Stretch = Stretch.UniformToFill
		};
		RenderOptions.SetBitmapScalingMode((DependencyObject)(object)image, BitmapScalingMode.HighQuality);
		grid.Children.Add(image);
		Image image2 = new Image
		{
			Width = 24.0,
			Height = 24.0,
			HorizontalAlignment = HorizontalAlignment.Right,
			VerticalAlignment = VerticalAlignment.Bottom,
			Source = ImageService.GetImage("zoom_in_staticColors.svg")
		};
		RenderOptions.SetBitmapScalingMode((DependencyObject)(object)image2, BitmapScalingMode.HighQuality);
		grid.Children.Add(image2);
		_imageButton.Content = grid;
		_imageButton.Background = Brushes.Transparent;
		_imageButton.BorderThickness = new Thickness(1.0);
		_imageButton.Cursor = Cursors.Hand;
		_imageButton.Click += new SafeEventHandler<RoutedEventArgs>(ImageButton_Click).Handler;
		_name = AddStringControl(container2, Definition.Fields["Name"], HorizontalAlignment.Stretch);
		_elType = AddSmartEnumDropDownControl(container2, Definition.Fields["ElType"]);
		Dictionary<string, string> dictionary = ElScaleAndTerminalHelper.ElScaleTypes.ToDictionary((KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Key, (KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Value.ScaleType);
		dictionary.Add(ElScaleTypesSE.MassaK.Value, ElScaleTypesSE.MassaK.Caption);
		dictionary.Add(ElScaleTypesSE.CasDirect.Value, ElScaleTypesSE.CasDirect.Caption);
		dictionary.Add(ElScaleTypesSE.DigiDirect.Value, ElScaleTypesSE.DigiDirect.Caption);
		dictionary.Add(ElScaleTypesSE.TMADirect.Value, ElScaleTypesSE.TMADirect.Caption);
		dictionary.Add(ElScaleTypesSE.MertechDirect.Value, ElScaleTypesSE.MertechDirect.Caption);
		_elModel = AddDropDownControl(container2, Definition.Fields["ElModel"], new ElScaleOrTerminalTypes(dictionary, isScale: true));
		DXTabControl tc = AddTabControl(null);
		_lgSettings = AddTabbedGroup(tc, ArmSoft.SME.Common.Resources.Settings, "Connections");
		_priceList = AddDropDownControl(_lgSettings, Definition.Fields["PriceList"], new PriceTypes());
		_asNameChoose = AddSmartEnumDropDownControl(_lgSettings, Definition.Fields["AsNameChoose"]);
		_asCodeExport = AddSmartEnumDropDownControl(_lgSettings, Definition.Fields["AsCodeExport"]);
		_group = AddDropDownControl(_lgSettings, Definition.Fields["Group"], new GoodsGroups());
		_segment = AddDropDownControl(_lgSettings, Definition.Fields["Segment"], new ArmSoft.SME.Trade.UI.DropDown.Segments());
		_ipAddressArea = AddLayoutGroup(_lgSettings, LayoutGroupView.Group, Orientation.Horizontal);
		LayoutGroupExt layoutGroupExt = AddLayoutGroup(_ipAddressArea, LayoutGroupView.Group, Orientation.Vertical);
		_ipAddress = AddStringControl(layoutGroupExt, ArmSoft.SME.Common.Resources.IPAddress, isRequired: false, 512, HorizontalAlignment.Stretch, "", "IpAddress");
		_password = AddStringControl(layoutGroupExt, ArmSoft.SME.Common.Resources.Password, isRequired: false, 10, HorizontalAlignment.Left, "", "Password");
		_checkConnectionResult = new TextBlock();
		_checkConnectionResult.VerticalAlignment = VerticalAlignment.Top;
		_checkConnectionResult.Visibility = Visibility.Collapsed;
		layoutGroupExt.Children.Add(_checkConnectionResult);
		_searchSclae = AddButton(_ipAddressArea, "", ArmSoft.SME.Common.Resources.Find, "", ArmSoft.SME.Common.Resources.Find, isTabStop: true);
		_searchSclae.Click += new SafeDocumentEventHandler<RoutedEventArgs>(SearchSclae_Click, this).Handler;
		_checkConnection = AddButton(_ipAddressArea, "", ArmSoft.SME.Trade.Common.Resources.CheckConnection, "", ArmSoft.SME.Trade.Common.Resources.CheckConnection, isTabStop: true);
		_checkConnection.Click += new SafeDocumentEventHandler<RoutedEventArgs>(CheckConnection_Click, this).Handler;
		_ipAddressArea.Visibility = Visibility.Collapsed;
		_exportForAdditionalQntUnit = AddBooleanControl(_lgSettings, Definition.Fields["ExportForAdditionalQntUnit"]);
		_browseButton = new ButtonInfo
		{
			GlyphKind = GlyphKind.Regular
		};
		_exportFile = AddButtonEditControl(_lgSettings, ArmSoft.SME.Trade.Common.Resources.FileToExport, isRequired: false, 512, 0, HorizontalAlignment.Stretch, "", "ExportFile");
		Image content = new Image
		{
			Width = 14.0,
			Height = 14.0,
			Margin = new Thickness(-2.0, 0.0, -2.0, 0.0)
		};
		ButtonInfo buttonInfo = new ButtonInfo
		{
			GlyphKind = GlyphKind.Regular,
			ToolTip = ArmSoft.SME.Common.Resources.ExploreFileFolder,
			Content = content,
			Name = "ChooseFile",
			CommandParameter = _exportFile
		};
		_exportFile.Buttons.Add(buttonInfo);
		_deleteAllItemsBeforeExporting = AddBooleanControl(_lgSettings, Definition.Fields["DeleteAllItemsBeforeExporting"]);
		((LayoutItemWithRequired)(object)_deleteAllItemsBeforeExporting.Parent).Visibility = Visibility.Collapsed;
		buttonInfo.Click += new SafeEventHandler<RoutedEventArgs>(ExportFile_Click).Handler;
		_labelNBarcodeNSettingsLG = AddLayoutGroup(_lgSettings, LayoutGroupView.Group, Orientation.Horizontal);
		_labelNBarcodeNSettingsLG.Visibility = Visibility.Collapsed;
		LayoutGroupExt container3 = AddLayoutGroup(_labelNBarcodeNSettingsLG, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.WeighedItem);
		_spinEditWeightLabelN = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99,
			IsReadOnly = false,
			MinWidth = 50.0,
			EditValue = 0,
			Value = 0m,
			Name = "WeightLabelN"
		};
		LayoutItemWithRequired li = AddLayoutItem(container3, _spinEditWeightLabelN, HorizontalAlignment.Left);
		AddControl(_spinEditWeightLabelN, ArmSoft.SME.Trade.Common.Resources.PrintLabelN, li);
		_spinEditWeightBarCodeN = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99,
			IsReadOnly = false,
			MinWidth = 50.0,
			EditValue = 0,
			Value = 0m,
			Name = "WeightBarCodeN"
		};
		LayoutItemWithRequired li2 = AddLayoutItem(container3, _spinEditWeightBarCodeN, HorizontalAlignment.Left);
		AddControl(_spinEditWeightBarCodeN, ArmSoft.SME.Trade.Common.Resources.PrintBarCodeN, li2);
		LayoutGroupExt container4 = AddLayoutGroup(_labelNBarcodeNSettingsLG, LayoutGroupView.GroupBox, Orientation.Vertical, ArmSoft.SME.Trade.Common.Resources.PieceItem);
		_spinEditPieceLabelN = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99,
			IsReadOnly = false,
			MinWidth = 50.0,
			EditValue = 0,
			Value = 0m,
			Name = "PieceLabelN"
		};
		LayoutItemWithRequired li3 = AddLayoutItem(container4, _spinEditPieceLabelN, HorizontalAlignment.Left);
		AddControl(_spinEditPieceLabelN, ArmSoft.SME.Trade.Common.Resources.PrintLabelN, li3);
		_spinEditPieceBarCodeN = new SpinEditExt
		{
			IsFloatValue = false,
			MinValue = 0m,
			MaxValue = 99,
			IsReadOnly = false,
			MinWidth = 50.0,
			EditValue = 0,
			Value = 0m,
			Name = "PieceBarCodeN"
		};
		LayoutItemWithRequired li4 = AddLayoutItem(container4, _spinEditPieceBarCodeN, HorizontalAlignment.Left);
		AddControl(_spinEditPieceBarCodeN, ArmSoft.SME.Trade.Common.Resources.PrintBarCodeN, li4);
		_browseButton.Click += new SafeEventHandler<RoutedEventArgs>(ExportFile_Click).Handler;
		_elType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ElType_EditValueChanged).Handler;
		_elModel.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ElModel_EditValueChanged).Handler;
	}

	protected override void FillDocument(DirStoreRequest sr)
	{
		base.FillDocument(sr);
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value || _elModel.Code == ElScaleTypesSE.CasDirect.Value || _elModel.Code == ElScaleTypesSE.DigiDirect.Value || _elModel.Code == ElScaleTypesSE.TMADirect.Value || _elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			PO.ExportFile = _ipAddress.Text;
			PO.Password = _password.Text;
			PO.WeightLabelN = (int)_spinEditWeightLabelN.Value;
			PO.WeightBarCodeN = (int)_spinEditWeightBarCodeN.Value;
			PO.PieceLabelN = (int)_spinEditPieceLabelN.Value;
			PO.PieceBarCodeN = (int)_spinEditPieceBarCodeN.Value;
		}
		else
		{
			PO.ExportFile = _exportFile.Text;
		}
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value)
		{
			ModifyMassakSettingsFile();
		}
	}

	private void ImageButton_Click(object sender, RoutedEventArgs e)
	{
		ThemedWindow themedWindow = new ThemedWindow();
		themedWindow.Title = GetScaleNameByScaleModel(_elModel.Code);
		themedWindow.TitleAlignment = WindowTitleAlignment.Center;
		themedWindow.Width = 570.0;
		themedWindow.Height = 600.0;
		themedWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
		themedWindow.Owner = this;
		themedWindow.ShowInTaskbar = false;
		themedWindow.Icon = null;
		themedWindow.ShowIcon = false;
		Image content = new Image
		{
			Source = ((Image)((Grid)_imageButton.Content).Children[0]).Source,
			Stretch = Stretch.Uniform,
			HorizontalAlignment = HorizontalAlignment.Center,
			VerticalAlignment = VerticalAlignment.Center
		};
		themedWindow.Content = content;
		themedWindow.ShowDialog();
	}

	protected override void LoadDataToControls()
	{
		base.LoadDataToControls();
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value || _elModel.Code == ElScaleTypesSE.CasDirect || _elModel.Code == ElScaleTypesSE.DigiDirect || _elModel.Code == ElScaleTypesSE.TMADirect || _elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			_ipAddress.Text = PO.ExportFile;
			_password.Text = PO.Password;
			_spinEditWeightLabelN.Value = PO.WeightLabelN;
			_spinEditWeightBarCodeN.Value = PO.WeightBarCodeN;
			_spinEditPieceLabelN.Value = PO.PieceLabelN;
			_spinEditPieceBarCodeN.Value = PO.PieceBarCodeN;
		}
		else
		{
			_exportFile.Text = PO.ExportFile;
		}
		ElModel_EditValueChanged();
	}

	private string GetScaleNameByScaleModel(string scaleModel)
	{
		if (scaleModel == ElScaleTypesSE.MassaK.Value)
		{
			return "Massa-K RL";
		}
		if (scaleModel == ElScaleTypesSE.CasDirect.Value)
		{
			return "CAS CL";
		}
		if (scaleModel == ElScaleTypesSE.DigiDirect.Value)
		{
			return "DIGI SM";
		}
		if (scaleModel == ElScaleTypesSE.TMADirect.Value)
		{
			return "TM-A";
		}
		if (scaleModel == ElScaleTypesSE.MertechDirect.Value)
		{
			return "Mertech M-ER 727";
		}
		return "";
	}

	private void CheckConnection_Click(object sender, RoutedEventArgs e)
	{
		CheckConnection();
	}

	private void SearchSclae_Click(object sender, RoutedEventArgs e)
	{
		BroadcastDiscoveryMessage();
	}

	private void BroadcastDiscoveryMessage()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		List<IPEndPoint> source = new List<IPEndPoint>();
		List<IPAndMacAddress> paramIPEndPoints = new List<IPAndMacAddress>();
		SearchIPAddresses searchIPAddresses = null;
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value)
		{
			source = MassaK.SearchDevices();
		}
		else if (_elModel.Code == ElScaleTypesSE.CasDirect.Value)
		{
			source = Cas.SearchDevices();
		}
		else if (_elModel.Code == ElScaleTypesSE.DigiDirect.Value)
		{
			Mouse.OverrideCursor = null;
			searchIPAddresses = new SearchIPAddresses(-1, this);
			paramIPEndPoints = searchIPAddresses.ResultIPList;
		}
		else if (_elModel.Code == ElScaleTypesSE.TMADirect.Value)
		{
			source = TM_A.SearchDevices();
		}
		else if (_elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			Mouse.OverrideCursor = null;
			searchIPAddresses = new SearchIPAddresses(1111, this);
			paramIPEndPoints = searchIPAddresses.ResultIPList;
		}
		if (_elModel.Code == ElScaleTypesSE.DigiDirect.Value || _elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			if (!searchIPAddresses.Canceled)
			{
				ArmSoft.SME.Trade.UI.DropDown.IPAddress iPAddress = new ArmSoft.SME.Trade.UI.DropDown.IPAddress(SmartEnum<ElScaleTypesSE, string>.FromValue(_elModel.Code).Caption);
				iPAddress.ParamIPEndPoints = paramIPEndPoints;
				ModalBrowser modalBrowser = new ModalBrowser(iPAddress);
				modalBrowser.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				modalBrowser.Owner = this;
				if (modalBrowser.Show())
				{
					_ipAddress.Text = iPAddress.ValueMember(modalBrowser.SelectedRowIndex);
				}
			}
		}
		else
		{
			ArmSoft.SME.Trade.UI.DropDown.IPAddress iPAddress2 = new ArmSoft.SME.Trade.UI.DropDown.IPAddress(SmartEnum<ElScaleTypesSE, string>.FromValue(_elModel.Code).Caption);
			iPAddress2.ParamIPEndPoints = source.Select((IPEndPoint ep) => new IPAndMacAddress(ep.Address.ToString(), ep.Port.ToString(), string.Empty)).ToList();
			ModalBrowser modalBrowser2 = new ModalBrowser(iPAddress2);
			modalBrowser2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			modalBrowser2.Owner = this;
			Mouse.OverrideCursor = null;
			if (modalBrowser2.Show())
			{
				_ipAddress.Text = iPAddress2.ValueMember(modalBrowser2.SelectedRowIndex);
			}
		}
	}

	private void CheckConnection()
	{
		if (string.IsNullOrWhiteSpace(_ipAddress.Text))
		{
			return;
		}
		bool flag = false;
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value)
		{
			MassaK.MassaKDriverDevices massaKDriverDevices;
			try
			{
				massaKDriverDevices = new MassaK.MassaKDriverDevices();
			}
			catch
			{
				_checkConnectionResult.Text = ArmSoft.SME.Common.Resources.Disconnected;
				_checkConnectionResult.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0000");
				_checkConnectionResult.Visibility = Visibility.Visible;
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.MassakDeviceSettingsAreMissing);
			}
			if (massaKDriverDevices.GetDevice(_ipAddress.Text, _name.Text) == 0)
			{
				flag = true;
				ModifyMassakSettingsFile();
			}
		}
		else if (_elModel.Code == ElScaleTypesSE.CasDirect.Value)
		{
			flag = new Cas(_ipAddress.Text).CheckConnection();
		}
		else if (_elModel.Code == ElScaleTypesSE.DigiDirect.Value)
		{
			flag = new Digi(_ipAddress.Text).CheckConnection();
		}
		else if (_elModel.Code == ElScaleTypesSE.TMADirect.Value)
		{
			flag = new TM_A(_ipAddress.Text).CheckConnection();
		}
		else if (_elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			flag = new Mertech(_ipAddress.Text, _password.Text, _deleteAllItemsBeforeExporting.IsChecked.Value).CheckConnection();
		}
		if (flag)
		{
			_checkConnectionResult.Text = ArmSoft.SME.Common.Resources.Connected;
			_checkConnectionResult.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#008000");
			_checkConnectionResult.Visibility = Visibility.Visible;
		}
		else
		{
			_checkConnectionResult.Text = ArmSoft.SME.Common.Resources.Disconnected;
			_checkConnectionResult.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF0000");
			_checkConnectionResult.Visibility = Visibility.Visible;
		}
	}

	private void ModifyMassakSettingsFile()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string text = Path.Combine(folderPath, "Massa-K\\TerminalDriver\\Driver.ini");
		string text2 = Path.Combine(folderPath, "Massa-K\\TerminalDriver\\Driver_Temp.ini");
		try
		{
			using (StreamReader streamReader = new StreamReader(text))
			{
				using StreamWriter streamWriter = new StreamWriter(text2);
				string text3;
				while ((text3 = streamReader.ReadLine()) != null)
				{
					if (text3.StartsWith("LCID="))
					{
						text3 = "LCID=42B";
					}
					streamWriter.WriteLine(text3);
				}
			}
			File.Delete(text);
			File.Move(text2, text);
		}
		catch (Exception ex)
		{
			Console.WriteLine("An error occurred: " + ex.Message);
		}
	}

	private void ExportFile_Click(object sender, RoutedEventArgs e)
	{
		string text = "";
		try
		{
			ExtensionUIComponents extensionUIComponents = new ExtensionUIComponents();
			if (_elType.Code == DeviceTypesSE.ElScale.Value && !string.IsNullOrWhiteSpace(_elModel.Code) && _elModel.DoValidate())
			{
				text = extensionUIComponents.ShowSaveFileDialog(ElScaleAndTerminalHelper.ElScaleTypes[_elModel.Code.Trim()].ExportedFileExtension, $"{ElScaleAndTerminalHelper.ElScaleTypes[_elModel.Code.Trim()].ExportedFileDescription}|*{ElScaleAndTerminalHelper.ElScaleTypes[_elModel.Code.Trim()].ExportedFileExtension}", ElScaleAndTerminalHelper.ElScaleTypes[_elModel.Code.Trim()].ExportedFileName, overwritePrompt: false);
			}
			else if (_elType.Code == DeviceTypesSE.Terminal.Value && !string.IsNullOrWhiteSpace(_elModel.Code) && _elModel.DoValidate())
			{
				text = extensionUIComponents.ShowSaveFileDialog(ElScaleAndTerminalHelper.TerminalTypes[_elModel.Code.Trim()].FileExtension, ElScaleAndTerminalHelper.TerminalTypes[_elModel.Code.Trim()].FilesFilter, "scanin", overwritePrompt: false);
			}
			else
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				if (openFileDialog.ShowDialog() == true)
				{
					text = openFileDialog.FileName;
				}
			}
			if (!string.IsNullOrWhiteSpace(text))
			{
				_exportFile.Text = text;
			}
		}
		catch (Exception)
		{
		}
	}

	private void ElType_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		if (!_elType.DoValidate())
		{
			return;
		}
		((LayoutItemWithRequired)(object)_asNameChoose.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_asCodeExport.Parent).Visibility = Visibility.Collapsed;
		((LayoutItemWithRequired)(object)_exportForAdditionalQntUnit.Parent).Visibility = Visibility.Collapsed;
		_elModel.Code = string.Empty;
		_asNameChoose.Code = string.Empty;
		_asCodeExport.Code = string.Empty;
		_priceList.Code = string.Empty;
		_exportForAdditionalQntUnit.IsChecked = false;
		_exportFile.IsRequired = true;
		string code = _elType.Code;
		if (!(code == "1"))
		{
			if (code == "2")
			{
				((LayoutItemWithRequired)(object)_exportForAdditionalQntUnit.Parent).Visibility = Visibility.Visible;
				_elModel.DataView = new ElScaleOrTerminalTypes(ElScaleAndTerminalHelper.TerminalTypes.ToDictionary((KeyValuePair<string, TerminalHandler> item) => item.Key, (KeyValuePair<string, TerminalHandler> item) => item.Value.Brand), isScale: false);
				_asNameChoose.IsRequired = false;
				_asCodeExport.IsRequired = false;
			}
			return;
		}
		((LayoutItemWithRequired)(object)_asNameChoose.Parent).Visibility = Visibility.Visible;
		((LayoutItemWithRequired)(object)_asCodeExport.Parent).Visibility = Visibility.Visible;
		Dictionary<string, string> dictionary = ElScaleAndTerminalHelper.ElScaleTypes.ToDictionary((KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Key, (KeyValuePair<string, ElScaleExportExtensionHandler> item) => item.Value.ScaleType);
		dictionary.Add(ElScaleTypesSE.MassaK.Value, ElScaleTypesSE.MassaK.Caption);
		dictionary.Add(ElScaleTypesSE.CasDirect.Value, ElScaleTypesSE.CasDirect.Caption);
		dictionary.Add(ElScaleTypesSE.DigiDirect.Value, ElScaleTypesSE.DigiDirect.Caption);
		dictionary.Add(ElScaleTypesSE.TMADirect.Value, ElScaleTypesSE.TMADirect.Caption);
		dictionary.Add(ElScaleTypesSE.MertechDirect.Value, ElScaleTypesSE.MertechDirect.Caption);
		_elModel.DataView = new ElScaleOrTerminalTypes(dictionary, isScale: true);
		_asNameChoose.IsRequired = true;
		_asCodeExport.IsRequired = true;
	}

	private void ElModel_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		ElModel_EditValueChanged();
	}

	private void ElModel_EditValueChanged()
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		if (_elModel.Code == ElScaleTypesSE.MassaK.Value || _elModel.Code == ElScaleTypesSE.CasDirect.Value || _elModel.Code == ElScaleTypesSE.DigiDirect.Value || _elModel.Code == ElScaleTypesSE.TMADirect.Value || _elModel.Code == ElScaleTypesSE.MertechDirect.Value)
		{
			_imageButton.Visibility = Visibility.Visible;
			_exportFile.IsRequired = false;
			_ipAddressArea.Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_exportFile.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_deleteAllItemsBeforeExporting.Parent).Visibility = Visibility.Visible;
			_ipAddress.IsRequired = true;
			if (_elModel.Code == ElScaleTypesSE.MertechDirect.Value)
			{
				((Image)((Grid)_imageButton.Content).Children[0]).Source = ImageService.GetImage("scale_Mertech_M.png", isAppPathPreferred: true, "Scales/");
				((LayoutItemWithRequired)(object)_password.Parent).Visibility = Visibility.Visible;
				_password.IsRequired = true;
			}
			else
			{
				_password.IsRequired = false;
				((LayoutItemWithRequired)(object)_password.Parent).Visibility = Visibility.Collapsed;
				if (_elModel.Code == ElScaleTypesSE.TMADirect.Value)
				{
					((Image)((Grid)_imageButton.Content).Children[0]).Source = ImageService.GetImage("scale_TM_A.png", isAppPathPreferred: true, "Scales/");
				}
				else if (_elModel.Code == ElScaleTypesSE.DigiDirect.Value)
				{
					((Image)((Grid)_imageButton.Content).Children[0]).Source = ImageService.GetImage("scale_DIGI_SM.png", isAppPathPreferred: true, "Scales/");
				}
				else if (_elModel.Code == ElScaleTypesSE.CasDirect.Value)
				{
					((Image)((Grid)_imageButton.Content).Children[0]).Source = ImageService.GetImage("scale_CAS_CL.png", isAppPathPreferred: true, "Scales/");
				}
				else if (_elModel.Code == ElScaleTypesSE.MassaK.Value)
				{
					((Image)((Grid)_imageButton.Content).Children[0]).Source = ImageService.GetImage("scale_Massa-K.png", isAppPathPreferred: true, "Scales/");
				}
			}
		}
		else
		{
			_imageButton.Visibility = Visibility.Collapsed;
			_exportFile.IsRequired = true;
			_ipAddress.IsRequired = false;
			_password.IsRequired = false;
			_ipAddressArea.Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_exportFile.Parent).Visibility = Visibility.Visible;
			((LayoutItemWithRequired)(object)_deleteAllItemsBeforeExporting.Parent).Visibility = Visibility.Collapsed;
			((LayoutItemWithRequired)(object)_password.Parent).Visibility = Visibility.Collapsed;
		}
		if (_elModel.Code == ElScaleTypesSE.DigiDirect || _elModel.Code == ElScaleTypesSE.TMADirect || _elModel.Code == ElScaleTypesSE.CasDirect)
		{
			_labelNBarcodeNSettingsLG.Visibility = Visibility.Visible;
		}
		else
		{
			_labelNBarcodeNSettingsLG.Visibility = Visibility.Collapsed;
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		_isActive.IsChecked = true;
		if (string.IsNullOrWhiteSpace(_elType.Code))
		{
			_elType.Code = DeviceTypesSE.ElScale.Value.ToString();
		}
	}

	protected override void OnEdit()
	{
		base.OnEdit();
		_code.IsReadOnly = true;
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		base.UpdateDataViewPanel(dvp);
		Settings.HostingEnvironment.FindDataViewPanel(dvp.Id).Update(PO.Code);
	}
}
