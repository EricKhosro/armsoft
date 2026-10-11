using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Ardalis.SmartEnum;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.ECR;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Helpers;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Directory;

[DirectoryWindow(32)]
public class CashDialogWindow : DirectoryDialogWindow<Cash>
{
	private class ECROperatorsAndDepsDialogWindow : DialogWindow
	{
		public ECROperatorsAndDepsDialogWindow(Window owner, Utilities.GetOperatorsAndDepsResponse result)
			: base(owner)
		{
			Caption = ArmSoft.SME.Common.Resources.ECROpersAndDeps;
			Mode = DialogWindowMode.ViewMode;
			LayoutGroupExt container = AddVerticalLayoutGroup();
			DocumentGridControl documentGridControl = AddGridControl(container, "ECROperatorsGrid");
			documentGridControl.Caption = ArmSoft.SME.Common.Resources.Operators;
			documentGridControl.AddStringColumn("Id", ArmSoft.SME.Common.Resources.ECROperator, 10);
			documentGridControl.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Name, 10);
			documentGridControl.AddStringColumn("AvailableDepartments", ArmSoft.SME.Common.Resources.ECRAvailableDepartments, 10);
			documentGridControl.IsReadOnly = true;
			documentGridControl.ShowButtons = false;
			BindingList<Operator> bindingList = new BindingList<Operator>();
			foreach (Utilities.Cashier item in result.c)
			{
				bindingList.Add(new Operator(item));
			}
			documentGridControl.ItemsSource = bindingList;
			DocumentGridControl documentGridControl2 = AddGridControl(container, "ECRDepartmentsGrid");
			documentGridControl2.Caption = ArmSoft.SME.Common.Resources.Departments;
			documentGridControl2.AddStringColumn("Id", ArmSoft.SME.Common.Resources.ECRDep, 10);
			documentGridControl2.AddStringColumn("Name", ArmSoft.SME.Common.Resources.Name, 10);
			documentGridControl2.AddStringColumn("Type", ArmSoft.SME.Common.Resources.TypeWord, 10);
			documentGridControl2.AddStringColumn("DepartmentType", ArmSoft.SME.Common.Resources.ECRDepartmentType, 20);
			documentGridControl2.IsReadOnly = true;
			documentGridControl2.ShowButtons = false;
			BindingList<Department> bindingList2 = new BindingList<Department>();
			foreach (Utilities.Department item2 in result.d)
			{
				bindingList2.Add(new Department(item2));
			}
			documentGridControl2.ItemsSource = bindingList2;
		}
	}

	private TextEditExt _name;

	private TextEditExt _nextNumIn;

	private TextEditExt _nextNumOut;

	private TextEditExt _nextMTBillNum;

	private TextEditExt _nextMTBillReturnNum;

	private TextEditExt _cashierShiftNum;

	private CheckEditExt _isDefault;

	private CheckEditExt _isECR;

	private TextEditExt _ecrIP;

	private TextEditExt _ecrMacAddress;

	private TextEditExt _ecrPort;

	private TextEditExt _ecrPassword;

	private TextEditExt _ecrOperator;

	private TextEditExt _ecrOperatorPassword;

	private Button _checkECRIP;

	private Button _searchECR;

	private Button _updateECRIP;

	private Button _ecrDeps;

	private Button _printTestCheck;

	private ECR _ecr;

	private PartnerContractGroupControls _bankAcquiringContainerPOS1;

	private PartnerContractGroupControls _bankAcquiringContainerPOS2;

	private PartnerContractGroupControls _bankAcquiringContainerPOS3;

	private PartnerContractGroupControls _bankAcquiringContainerPOS4;

	private SafeEventHandler<EditValueChangedEventArgs> _bankAcquiringContainer_PartnerCodeEditValueChangedHandler;

	private readonly bool _fromPOS;

	public CashDialogWindow(DialogWindowMode mode, Window owner = null)
		: base(mode, owner)
	{
	}

	public CashDialogWindow(Cash directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null)
		: base(directory, mode, owner)
	{
	}

	public CashDialogWindow(Cash directory, DialogWindowMode mode = DialogWindowMode.NewMode, Window owner = null, bool fromPOS = false)
		: base(directory, mode, owner)
	{
		_fromPOS = fromPOS;
	}

	protected override void ConstructUI()
	{
		_code = AddButtonEditControl(Definition.Fields["Code"], 1);
		_code.AllowMagicButton = true;
		_code.MagicToolTip = ArmSoft.SME.Common.Resources.CodeNextNumbers;
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		_name = AddStringControl(Definition.Fields["Name"]);
		_nextNumIn = AddStringControl(Definition.Fields["NextNumIn"]);
		_nextNumOut = AddStringControl(Definition.Fields["NextNumOut"]);
		_nextMTBillNum = AddStringControl(Definition.Fields["NextMTBillNum"]);
		_nextMTBillReturnNum = AddStringControl(Definition.Fields["NextMTBillReturnNum"]);
		_cashierShiftNum = AddStringControl(Definition.Fields["NextCashierShiftNum"]);
		_isDefault = AddBooleanControl(Definition.Fields["IsDefault"]);
		DXTabControl tc = AddTabControl(null);
		LayoutGroup container = AddTabedGroup(tc, ArmSoft.SME.Common.Resources.ECR);
		_isECR = AddBooleanControl(container, Definition.Fields["IsECR"]);
		AddStringControl(container, Definition.Fields["ECRSN"]);
		AddStringControl(container, Definition.Fields["ECRCrn"]);
		LayoutGroupExt container2 = AddHorizontalLayoutGroup(container);
		_ecrIP = AddStringControl(container2, Definition.Fields["ECRIP"]);
		_ecrIP.MaskType = MaskType.RegEx;
		_ecrIP.Mask = "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}";
		_ecrIP.Width = 122.0;
		_checkECRIP = AddButton(container2, "CheckIP", "", ArmSoft.SME.Common.Resources.ECRCheckConnection, "", "", isTabStop: false, HorizontalAlignment.Right);
		_checkECRIP.Click += new SafeEventHandler<RoutedEventArgs>(CheckECRIP_Click).Handler;
		_searchECR = AddButton(container2, "SearchECR", "", ArmSoft.SME.Common.Resources.SearchECR, "", "", isTabStop: false, HorizontalAlignment.Right);
		_searchECR.Click += new SafeEventHandler<RoutedEventArgs>(SearchECR_Click).Handler;
		_searchECR.Width = 120.0;
		LayoutGroupExt container3 = AddHorizontalLayoutGroup(container);
		_ecrMacAddress = AddStringControl(container3, Definition.Fields["ECRMacAddress"], HorizontalAlignment.Stretch);
		_ecrMacAddress.MaskType = MaskType.RegEx;
		_ecrMacAddress.Mask = "([0-9A-Fa-f]{2}-){5}[0-9A-Fa-f]{2}";
		_updateECRIP = AddButton(container3, "UpdateIP", "", ArmSoft.SME.Common.Resources.UpdateIP, "", "", isTabStop: false, HorizontalAlignment.Right);
		_updateECRIP.Click += new SafeEventHandler<RoutedEventArgs>(UpdateECR_Click).Handler;
		_updateECRIP.Width = 120.0;
		_ecrPort = AddStringControl(container, Definition.Fields["ECRPort"]);
		_ecrPort.MaskType = MaskType.RegEx;
		_ecrPort.Mask = "([1-9][0-9]{0,3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])";
		_ecrPassword = AddStringControl(container, Definition.Fields["ECRPassword"]);
		IEnumerable<string> localIPAddresses = GetLocalIPAddresses();
		ComboBoxEditExt comboBoxEditExt = AddComboBoxControl(container, "CompIPAddesses", ArmSoft.SME.Common.Resources.ECRCompIP, 15, localIPAddresses);
		comboBoxEditExt.IsTextEditable = false;
		if (localIPAddresses.Count() == 1)
		{
			comboBoxEditExt.SelectedIndex = 0;
		}
		_ecrDeps = AddButton(container, "ECRDeps", "", ArmSoft.SME.Common.Resources.ViewECROpersAndDeps);
		_ecrDeps.Click += new SafeEventHandler<RoutedEventArgs>(ECRDeps_Click).Handler;
		AddStringControl(container, Definition.Fields["ECRDepWithVAT"]);
		AddStringControl(container, Definition.Fields["ECRDepWithOutVAT"]);
		AddSmartEnumDropDownControl(container, Definition.Fields["CashSummLimitType"]);
		AddBooleanControl(container, Definition.Fields["ShowECRTracking"]);
		_ecrOperator = AddStringControl(container, Definition.Fields["ECROperator"]);
		_ecrOperatorPassword = AddStringControl(container, Definition.Fields["ECROperatorPassword"]);
		_printTestCheck = AddButton(container, "PrintTestCheck", "", ArmSoft.SME.Common.Resources.ECRPrintTestCheck);
		_printTestCheck.Click += new SafeEventHandler<RoutedEventArgs>(PrintTestCheck_Click).Handler;
		AddPOSTerminalsTab(tc, this);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _name;
		}
	}

	protected override void LoadDataToControls()
	{
		RemoveEditValueChangedHandlers();
		base.LoadDataToControls();
		if (Mode != DialogWindowMode.NewMode)
		{
			ConfigurePosTerminalValues();
		}
		AddEditValueChangedHandlers();
	}

	protected override void FillDocument()
	{
		base.FillDocument();
		FillNonCashInfo();
	}

	private void FillNonCashInfo()
	{
		if (!Directory.NonCashInfoList.TryGetValue(5, out var value))
		{
			value = new Cash.NonCashInfo();
			Directory.NonCashInfoList.Add(5, value);
		}
		value.Type = 5;
		SetPosTerminalValues(value, CommonConst.CashDeskNonCashTye.POS1.ToString().Right(1));
		if (!Directory.NonCashInfoList.TryGetValue(6, out value))
		{
			value = new Cash.NonCashInfo();
			Directory.NonCashInfoList.Add(6, value);
		}
		value.Type = 6;
		SetPosTerminalValues(value, CommonConst.CashDeskNonCashTye.POS2.ToString().Right(1));
		if (!Directory.NonCashInfoList.TryGetValue(7, out value))
		{
			value = new Cash.NonCashInfo();
			Directory.NonCashInfoList.Add(7, value);
		}
		value.Type = 7;
		SetPosTerminalValues(value, CommonConst.CashDeskNonCashTye.POS3.ToString().Right(1));
		if (!Directory.NonCashInfoList.TryGetValue(8, out value))
		{
			value = new Cash.NonCashInfo();
			Directory.NonCashInfoList.Add(8, value);
		}
		value.Type = 8;
		SetPosTerminalValues(value, CommonConst.CashDeskNonCashTye.POS4.ToString().Right(1));
	}

	private void SetPosTerminalValues(Cash.NonCashInfo nonCachInfoItem, string terminalName)
	{
		nonCachInfoItem.Partner = (string)base["POS" + terminalName + "PartnerCode"];
		nonCachInfoItem.Contract = (string)base["POS" + terminalName + "Contract"];
		nonCachInfoItem.Caption = (string)base["POS" + terminalName + "Caption"];
		nonCachInfoItem.Device = POSTerminalHelpers.GetPOSTerminalTypeCodeByCaption((string)base["POS" + terminalName + "Device"]);
		nonCachInfoItem.POSTerminalID = (string)base["POS" + terminalName + "TerminalID"];
		nonCachInfoItem.POSTerminalAddress = (string)base["POS" + terminalName + "TerminalAddress"];
		ComboBoxExt comboBoxExt = (ComboBoxExt)GetUIElement("POS" + terminalName + "ECRTerminal");
		if (comboBoxExt.SelectedItem != null)
		{
			NonCashPaymentMethodsSE paymentType = ((NonCashPaymentHelpers.NonCashPaymentItem)comboBoxExt.SelectedItem).PaymentType;
			nonCachInfoItem.ECRPaymentMethod = paymentType;
		}
	}

	protected override void SetControlsReadOnlyInner()
	{
		base.SetControlsReadOnlyInner();
		_code.IsReadOnly = Mode == DialogWindowMode.EditMode;
		_name.IsReadOnly = _fromPOS;
		_nextNumIn.IsReadOnly = _fromPOS;
		_nextNumOut.IsReadOnly = _fromPOS;
		_nextMTBillNum.IsReadOnly = _fromPOS;
		_nextMTBillReturnNum.IsReadOnly = _fromPOS;
		_cashierShiftNum.IsReadOnly = _fromPOS;
		_isDefault.IsReadOnly = _fromPOS;
		_isECR.IsReadOnly = _fromPOS;
		_bankAcquiringContainerPOS1.IsReadOnly = _fromPOS;
		_bankAcquiringContainerPOS2.IsReadOnly = _fromPOS;
		_bankAcquiringContainerPOS3.IsReadOnly = _fromPOS;
		_bankAcquiringContainerPOS4.IsReadOnly = _fromPOS;
	}

	private void CheckECRIP_Click(object sender, RoutedEventArgs e)
	{
		if (_ecrIP.DoValidate() && !string.IsNullOrWhiteSpace(_ecrIP.Text))
		{
			if (PingIP(_ecrIP.Text, out var pingReply))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.PingECRIPOK, _ecrIP.Text.Trim()) + "\r\nReply from " + _ecrIP.Text.Trim() + ": bytes=" + pingReply.Buffer.Length + " time=" + pingReply.RoundtripTime + "ms TTL=" + pingReply.Options.Ttl);
			}
			else
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.PingECRIPNotOK, _ecrIP.Text.Trim()), "", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
	}

	private void SearchECR_Click(object sender, RoutedEventArgs e)
	{
		if (!int.TryParse(_ecrPort.Text, out var result))
		{
			result = 1025;
		}
		SearchIPAddresses searchIPAddresses = new SearchIPAddresses(result, this);
		ArmSoft.SME.Trade.UI.DropDown.IPAddress iPAddress = new ArmSoft.SME.Trade.UI.DropDown.IPAddress(ArmSoft.SME.Common.Resources.ECR)
		{
			ParamIPEndPoints = searchIPAddresses.ResultIPList
		};
		if (!searchIPAddresses.Canceled)
		{
			ModalBrowser modalBrowser = new ModalBrowser(iPAddress)
			{
				WindowStartupLocation = WindowStartupLocation.CenterOwner,
				Owner = this
			};
			if (modalBrowser.Show())
			{
				_ecrIP.Text = iPAddress.ValueMember(modalBrowser.SelectedRowIndex).Split(':').FirstOrDefault();
				_ecrMacAddress.Text = iPAddress.CommentMember(modalBrowser.SelectedRowIndex);
			}
		}
	}

	private void UpdateECR_Click(object sender, RoutedEventArgs e)
	{
		string ipAddressFromMac = GetIpAddressFromMac(_ecrMacAddress.Text);
		if (!string.IsNullOrWhiteSpace(ipAddressFromMac))
		{
			_ecrIP.Text = ipAddressFromMac;
		}
		else if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.NothingWasFound + "\r\n" + ArmSoft.SME.Common.Resources.PerformՕperationQuestion, ArmSoft.SME.Common.Resources.SearchECR), ArmSoft.SME.Common.Resources.UpdateIP, MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.No)
		{
			if (!int.TryParse(_ecrPort.Text, out var result))
			{
				result = 1025;
			}
			new SearchIPAddresses(result, this);
			ipAddressFromMac = GetIpAddressFromMac(_ecrMacAddress.Text);
			if (!string.IsNullOrWhiteSpace(ipAddressFromMac))
			{
				_ecrIP.Text = ipAddressFromMac;
			}
			else
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NothingWasFound, ArmSoft.SME.Common.Resources.SearchECR);
			}
		}
	}

	public static string GetIpAddressFromMac(string macAddress)
	{
		try
		{
			using Process process = Process.Start(new ProcessStartInfo
			{
				FileName = "arp",
				Arguments = "-a",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});
			if (process == null)
			{
				return null;
			}
			string input = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			string pattern = "(\\d+\\.\\d+\\.\\d+\\.\\d+)\\s+" + Regex.Escape(macAddress);
			Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
			return match.Success ? match.Groups[1].Value : null;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static IEnumerable<string> GetLocalIPAddresses()
	{
		return from ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList
			where ip.AddressFamily == AddressFamily.InterNetwork
			select ip.ToString();
	}

	private static bool PingIP(string ip, out PingReply pingReply)
	{
		pingReply = null;
		if (string.IsNullOrWhiteSpace(ip))
		{
			return false;
		}
		Ping ping = new Ping();
		pingReply = ping.Send(ip, 1000);
		if (pingReply != null && pingReply.Status == IPStatus.Success)
		{
			return true;
		}
		return false;
	}

	private void ECRDeps_Click(object sender, RoutedEventArgs e)
	{
		if (!_ecrIP.DoValidate())
		{
			return;
		}
		bool flag = false;
		try
		{
			InitECR();
			if (_ecr == null)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ViewECROpersAndDeps, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			_ecr.Connect();
			flag = true;
			Utilities.GetOperatorsAndDepsResponse operatorsAndDeps = _ecr.GetOperatorsAndDeps();
			new ECROperatorsAndDepsDialogWindow(this, operatorsAndDeps).ShowDialog();
		}
		catch (Utilities.ECRException ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					_ecr.DisConnect();
				}
			}
			catch
			{
			}
		}
	}

	private void PrintTestCheck_Click(object sender, RoutedEventArgs e)
	{
		if (!_ecrIP.DoValidate())
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			string text = _ecrOperator.Text;
			string text2 = _ecrOperatorPassword.Text;
			if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(text2))
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidateECROperatorOrECROperatorPin, _code.Text), ArmSoft.SME.Common.Resources.ECRPrintTemplate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			if (!text.IsNumeric())
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintTemplate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			InitECR();
			if (_ecr == null)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintTemplate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			_ecr.Connect();
			flag = true;
			_ecr.OperatorLogin(int.Parse(text), text2);
			flag2 = true;
			_ecr.PrintTemplate();
			ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.IsDone, ArmSoft.SME.Common.Resources.ECRPrintTemplate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
		catch (Utilities.ECRException ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					if (flag2)
					{
						_ecr.OperatorLogout();
					}
					_ecr.DisConnect();
				}
			}
			catch
			{
			}
		}
	}

	private void InitECR()
	{
		string text = _ecrIP.Text.Trim();
		string text2 = _ecrPort.Text.Trim();
		string text3 = _ecrPassword.Text.Trim();
		if (!string.IsNullOrWhiteSpace(text) && !string.IsNullOrWhiteSpace(text2) && !string.IsNullOrWhiteSpace(text3))
		{
			if (!text2.IsNumeric())
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
			}
			_ecr = new ECR(text, int.Parse(text2), text3);
			return;
		}
		throw new ASException(ArmSoft.SME.Common.Resources.ECRInitFailed);
	}

	private void AddPOSTerminalsTab(DXTabControl tabControl, DialogWindow dialog)
	{
		LayoutGroup posTerminalsTab = dialog.AddTabedGroup(tabControl, ArmSoft.SME.Trade.Common.Resources.POSTerminals);
		AddPosTerminalsLayoutGroup(dialog, posTerminalsTab, ArmSoft.SME.Trade.Common.Resources.POSTerminal + " N1", "POS1");
		AddPosTerminalsLayoutGroup(dialog, posTerminalsTab, ArmSoft.SME.Trade.Common.Resources.POSTerminal + " N2", "POS2");
		AddPosTerminalsLayoutGroup(dialog, posTerminalsTab, ArmSoft.SME.Trade.Common.Resources.POSTerminal + " N3", "POS3");
		AddPosTerminalsLayoutGroup(dialog, posTerminalsTab, ArmSoft.SME.Trade.Common.Resources.POSTerminal + " N4", "POS4");
		AddEditValueChangedHandlers();
	}

	private void AddPosTerminalsLayoutGroup(DialogWindow dialog, LayoutGroup posTerminalsTab, string header, string termName)
	{
		LayoutGroupExt layoutGroupExt = dialog.AddHorizontalLayoutGroup(posTerminalsTab);
		layoutGroupExt.Orientation = Orientation.Vertical;
		layoutGroupExt.HorizontalAlignment = HorizontalAlignment.Stretch;
		layoutGroupExt.Header = header;
		layoutGroupExt.View = LayoutGroupView.GroupBox;
		layoutGroupExt.IsCollapsible = true;
		layoutGroupExt.IsCollapsed = true;
		dialog.AddStringControl(layoutGroupExt, termName + "Caption", ArmSoft.SME.Common.Resources.Name, 50);
		switch (termName)
		{
		case "POS1":
			_bankAcquiringContainerPOS1 = new PartnerContractGroupControls(dialog);
			_bankAcquiringContainerPOS1.ConstructDlgUI(layoutGroupExt, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: false, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, termName, showRefreshButtonCaption: false, allowadditionalButtons: true);
			_bankAcquiringContainerPOS1.ContractCurrCode.ControlVisibility = Visibility.Collapsed;
			_bankAcquiringContainerPOS1.PartnerGroup.ControlVisibility = Visibility.Collapsed;
			break;
		case "POS2":
			_bankAcquiringContainerPOS2 = new PartnerContractGroupControls(dialog);
			_bankAcquiringContainerPOS2.ConstructDlgUI(layoutGroupExt, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: false, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, termName, showRefreshButtonCaption: false, allowadditionalButtons: true);
			_bankAcquiringContainerPOS2.ContractCurrCode.ControlVisibility = Visibility.Collapsed;
			_bankAcquiringContainerPOS2.PartnerGroup.ControlVisibility = Visibility.Collapsed;
			break;
		case "POS3":
			_bankAcquiringContainerPOS3 = new PartnerContractGroupControls(dialog);
			_bankAcquiringContainerPOS3.ConstructDlgUI(layoutGroupExt, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: false, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, termName, showRefreshButtonCaption: false, allowadditionalButtons: true);
			_bankAcquiringContainerPOS3.ContractCurrCode.ControlVisibility = Visibility.Collapsed;
			_bankAcquiringContainerPOS3.PartnerGroup.ControlVisibility = Visibility.Collapsed;
			break;
		case "POS4":
			_bankAcquiringContainerPOS4 = new PartnerContractGroupControls(dialog);
			_bankAcquiringContainerPOS4.ConstructDlgUI(layoutGroupExt, storeValue: true, "", isFilterBySuppliers: false, isFilterByCustomers: false, isShowContractCurrCode: true, isShowSummaDebt: false, "", "", showOnlyBonusContracts: false, showOnlyCurCodeNCC: true, connectTob2bArmsoftDiscountServerforBonusSum: false, termName, showRefreshButtonCaption: false, allowadditionalButtons: true);
			_bankAcquiringContainerPOS4.ContractCurrCode.ControlVisibility = Visibility.Collapsed;
			_bankAcquiringContainerPOS4.PartnerGroup.ControlVisibility = Visibility.Collapsed;
			break;
		}
		ComboBoxEditExt comboBoxEditExt = dialog.AddComboBoxControl(layoutGroupExt, termName + "Device", ArmSoft.SME.Trade.Common.Resources.PluggedDevice, 20, (from x in SmartEnum<POSTerminalTypesSE, string>.List
			orderby x.Value
			select x.Caption).ToList());
		comboBoxEditExt.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Device_Edit).Handler;
		ComboBoxExt comboBoxExt = new ComboBoxExt
		{
			ItemTemplate = (DataTemplate)Application.Current.Resources["PlasticCardTemplate"],
			ItemsSource = NonCashPaymentHelpers.GetNonCashPayments(withCardTypes: false)
		};
		dialog.AddControl(layoutGroupExt, termName + "ECRTerminal", ArmSoft.SME.Common.Resources.ECR + " " + ArmSoft.SME.Common.Resources.PaymentSystem, comboBoxExt);
		comboBoxExt.IsEnabled = true;
		comboBoxExt.Width = comboBoxEditExt.Width - 18.0;
		comboBoxExt.ControlVisibility = Visibility.Collapsed;
		dialog.AddStringControl(layoutGroupExt, termName + "TerminalID", ArmSoft.SME.Trade.Common.Resources.TerminalCode, 12).ControlVisibility = Visibility.Collapsed;
		TextEditExt textEditExt = dialog.AddStringControl(layoutGroupExt, termName + "TerminalAddress", ArmSoft.SME.Common.Resources.IPAddress + " : " + ArmSoft.SME.Common.Resources.TCPPort.ToLower(), 21);
		textEditExt.MaskType = MaskType.RegEx;
		textEditExt.Mask = "([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}(:([0-9]|[1-9][0-9]{1,3}|[1-5][0-9]{4}|6([0-4][0-9]{3}|5([0-4][0-9]{2}|5([0-2][0-9]|3[0-5])))))";
		textEditExt.ControlVisibility = Visibility.Collapsed;
		dialog.AddButton(layoutGroupExt, termName + "PRINTPOSREPORT", "", "POS " + ArmSoft.SME.Common.Resources.Report).Click += new SafeEventHandler<RoutedEventArgs>(PrintPosReport_Click).Handler;
	}

	private void PrintPosReport_Click(object sender, RoutedEventArgs e)
	{
		Button button = (Button)sender;
		PossoftPayClientHelper.SettlementReport(((TextEditExt)GetUIElement(button.Name.Left(4) + "TerminalAddress")).Text);
	}

	private void Device_Edit(object sender, EditValueChangedEventArgs e)
	{
		ComboBoxEditExt comboBoxEditExt = (ComboBoxEditExt)sender;
		TextEditExt textEditExt = (TextEditExt)GetUIElement(comboBoxEditExt.Name.Left(4) + "TerminalID");
		TextEditExt textEditExt2 = (TextEditExt)GetUIElement(comboBoxEditExt.Name.Left(4) + "TerminalAddress");
		Button button = (Button)GetUIElement(comboBoxEditExt.Name.Left(4) + "PRINTPOSREPORT");
		ComboBoxExt comboBoxExt = (ComboBoxExt)GetUIElement(comboBoxEditExt.Name.Left(4) + "ECRTerminal");
		if (comboBoxEditExt.SelectedItem == null || string.IsNullOrWhiteSpace(comboBoxEditExt.SelectedItem.ToString()) || comboBoxEditExt.SelectedItem.ToString() == POSTerminalTypesSE.None.Value)
		{
			comboBoxExt.ControlVisibility = Visibility.Collapsed;
			comboBoxExt.SelectedItem = NonCashPaymentMethodsSE.None.Value;
			textEditExt.ControlVisibility = Visibility.Collapsed;
			textEditExt.Text = "";
			textEditExt2.Text = "";
			textEditExt2.ControlVisibility = Visibility.Collapsed;
			((Control)(object)button.Parent).Visibility = Visibility.Collapsed;
		}
		else if (comboBoxEditExt.SelectedItem.ToString() == POSTerminalTypesSE.ECRPOS.Caption)
		{
			comboBoxExt.ControlVisibility = Visibility.Visible;
			textEditExt.ControlVisibility = Visibility.Collapsed;
			textEditExt2.ControlVisibility = Visibility.Collapsed;
			((Control)(object)button.Parent).Visibility = Visibility.Collapsed;
		}
		else if (comboBoxEditExt.SelectedItem.ToString() == POSTerminalTypesSE.IngenicoACRUS.Caption)
		{
			comboBoxExt.ControlVisibility = Visibility.Collapsed;
			comboBoxExt.SelectedItem = NonCashPaymentMethodsSE.None.Value;
			textEditExt.ControlVisibility = Visibility.Visible;
			textEditExt2.ControlVisibility = Visibility.Collapsed;
			((Control)(object)button.Parent).Visibility = Visibility.Collapsed;
		}
		else if (comboBoxEditExt.SelectedItem.ToString() == POSTerminalTypesSE.FeitianPOSSOFT.Caption)
		{
			comboBoxExt.ControlVisibility = Visibility.Collapsed;
			comboBoxExt.SelectedItem = NonCashPaymentMethodsSE.None.Value;
			textEditExt.ControlVisibility = Visibility.Collapsed;
			textEditExt2.ControlVisibility = Visibility.Visible;
			((Control)(object)button.Parent).Visibility = Visibility.Visible;
		}
	}

	private void ConfigurePosTerminalValues()
	{
		Cash.NonCashInfo nonCashInfo = Directory.NonCashInfoList[5];
		if (nonCashInfo != null)
		{
			base["POS1Contract"] = nonCashInfo.Contract;
			base["POS1PartnerCode"] = nonCashInfo.Partner;
			((ComboBoxEditExt)GetUIElement("POS1Device")).SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(nonCashInfo.Device);
			base["POS1Caption"] = nonCashInfo.Caption;
			base["POS1TerminalID"] = nonCashInfo.POSTerminalID;
			base["POS1TerminalAddress"] = nonCashInfo.POSTerminalAddress;
			((ComboBoxExt)GetUIElement("POS1ECRTerminal")).SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
		}
		nonCashInfo = Directory.NonCashInfoList[6];
		if (nonCashInfo != null)
		{
			base["POS2Contract"] = nonCashInfo.Contract;
			base["POS2PartnerCode"] = nonCashInfo.Partner;
			((ComboBoxEditExt)GetUIElement("POS2Device")).SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(nonCashInfo.Device);
			base["POS2Caption"] = nonCashInfo.Caption;
			base["POS2TerminalID"] = nonCashInfo.POSTerminalID;
			base["POS2TerminalAddress"] = nonCashInfo.POSTerminalAddress;
			((ComboBoxExt)GetUIElement("POS2ECRTerminal")).SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
		}
		nonCashInfo = Directory.NonCashInfoList[7];
		if (nonCashInfo != null)
		{
			base["POS3Contract"] = nonCashInfo.Contract;
			base["POS3PartnerCode"] = nonCashInfo.Partner;
			((ComboBoxEditExt)GetUIElement("POS3Device")).SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(nonCashInfo.Device);
			base["POS3Caption"] = nonCashInfo.Caption;
			base["POS3TerminalID"] = nonCashInfo.POSTerminalID;
			base["POS3TerminalAddress"] = nonCashInfo.POSTerminalAddress;
			((ComboBoxExt)GetUIElement("POS3ECRTerminal")).SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
		}
		nonCashInfo = Directory.NonCashInfoList[8];
		if (nonCashInfo != null)
		{
			base["POS4Contract"] = nonCashInfo.Contract;
			base["POS4PartnerCode"] = nonCashInfo.Partner;
			((ComboBoxEditExt)GetUIElement("POS4Device")).SelectedItem = POSTerminalHelpers.GetPOSTerminalTypeCaptionByCode(nonCashInfo.Device);
			base["POS4Caption"] = nonCashInfo.Caption;
			base["POS4TerminalID"] = nonCashInfo.POSTerminalID;
			base["POS4TerminalAddress"] = nonCashInfo.POSTerminalAddress;
			((ComboBoxExt)GetUIElement("POS4ECRTerminal")).SelectedItem = NonCashPaymentHelpers.GetPaymentItem(nonCashInfo.ECRPaymentMethod);
		}
	}

	private void RemoveEditValueChangedHandlers()
	{
		_bankAcquiringContainerPOS1.PartnerCodeEditValueChanged -= _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS2.PartnerCodeEditValueChanged -= _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS3.PartnerCodeEditValueChanged -= _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS4.PartnerCodeEditValueChanged -= _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
	}

	private void AddEditValueChangedHandlers()
	{
		_bankAcquiringContainer_PartnerCodeEditValueChangedHandler = new SafeEventHandler<EditValueChangedEventArgs>(BankAcquiringContainer_PartnerCodeEditValueChanged);
		_bankAcquiringContainerPOS1.PartnerCodeEditValueChanged += _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS2.PartnerCodeEditValueChanged += _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS3.PartnerCodeEditValueChanged += _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
		_bankAcquiringContainerPOS4.PartnerCodeEditValueChanged += _bankAcquiringContainer_PartnerCodeEditValueChangedHandler.Handler;
	}

	private void BankAcquiringContainer_PartnerCodeEditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		PartnerContractGroupControls partnerContractGroupControls = (PartnerContractGroupControls)sender;
		if (partnerContractGroupControls != null && !string.IsNullOrWhiteSpace(partnerContractGroupControls.PartnerCode.Code))
		{
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerContractGroupControls.PartnerCode.Code);
			if (defaultContract != null)
			{
				partnerContractGroupControls.Contract.Code = defaultContract.Contract;
			}
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (Directory.IsDefault)
		{
			dvp.Refresh();
		}
		else
		{
			base.UpdateDataViewPanel(dvp);
		}
	}
}
