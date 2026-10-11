using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Trade.Client;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.UI.Services;
using ArmSoft.TaxService.Models;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class TradeTaxServiceHelper : TaxServiceHelper
{
	public class AssignTaxInvNumsDialog : TradeDialogWindow
	{
		private ButtonEditExt _path;

		private DropDownControl _importType;

		private DropDownControl _accType;

		private DateEditControl _date1;

		private DateEditControl _date2;

		private TextEdit _buyerTin;

		private TextEdit _buyerName;

		private TextEdit _invSerialNumber;

		private CheckEditExt _assignInvNumToDocNum;

		private string _xmlFileName = "";

		public bool PrepareAndShow(ref List<string> repHeadersFooters, ref List<AssignTaxInvNumsRepRow> repRows, bool isAccDoc = false)
		{
			if (isAccDoc)
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.AssignMTMoveInvNums;
				HelpId = "Sale_Operations_SeriaNumbersSynchronization";
			}
			else
			{
				Caption = ArmSoft.SME.Common.Resources.AssignTaxInvNums;
				HelpId = "Sale_Operations_SeriaNumbersSynchronization";
			}
			Name = "AssignTaxInvNums";
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			AddStoreValuesButton(container);
			_importType = AddDropDownControl(container, "ImportType", ArmSoft.SME.Common.Resources.Import, new SmartEnum(typeof(TaxServiceSourceTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_importType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ImportType_EditValueChanged).Handler;
			LayoutGroupExt container2 = AddHorizontalLayoutGroup();
			_path = AddButtonEditControl(container2, "InputPath", ArmSoft.SME.Common.Resources.ImportFileSelection, 512, isRequired: false, 1);
			_path.IsTextEditable = false;
			_path.AllowDefaultButton = true;
			_path.IsReadOnly = _importType.Code != TaxServiceSourceTypeSE.File.ToString();
			_path.DefaultButtonClick += new SafeEventHandler<RoutedEventArgs>(PathButton_Click).Handler;
			_accType = AddDropDownControl("ACCType", ArmSoft.SME.Common.Resources.AccType, new SmartEnum(typeof(TaxDocTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			if (isAccDoc)
			{
				_accType.Code = Convert.ToString(TaxDocTypeSE.AccDocs.Value);
				((LayoutItemWithRequired)(object)_accType.Parent).Visibility = Visibility.Collapsed;
			}
			container = AddHorizontalLayoutGroup();
			_date1 = AddDateControl(container, "StartDate", ArmSoft.SME.Common.Resources.DatePeriod, useLongDate: false, isRequired: false, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			_date2 = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: false, storeValue: true, ArmSoft.SME.Common.Resources.DatePeriodEnd);
			AddSpecialDateButton(container, _date1, _date2);
			_buyerTin = AddStringControl("BuyerTin", ArmSoft.SME.Common.Resources.BuyerTaxCode, 20);
			_buyerName = AddStringControl("BuyerName", ArmSoft.SME.Common.Resources.BuyerName, 255);
			_invSerialNumber = AddStringControl("InvSerialNumber", ArmSoft.SME.Common.Resources.TaxSerialNumber, 11);
			_assignInvNumToDocNum = AddBooleanControl("AssignAccInvNumToDocNum", ArmSoft.SME.Common.Resources.AssignTaxInvNumToDocNum, storeValue: true);
			WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(Validation).Handler;
			if (Convert.ToBoolean(ShowDialog()))
			{
				try
				{
					Mouse.OverrideCursor = Cursors.Wait;
					if (_importType.Code == Convert.ToString(TaxServiceSourceTypeSE.File))
					{
						_xmlFileName = Convert.ToString(_path.EditValue);
						repHeadersFooters.Add(_xmlFileName);
						if (isAccDoc)
						{
							AssignMTMoveInvNums(repHeadersFooters, repRows, Convert.ToBoolean(_assignInvNumToDocNum.IsChecked), _xmlFileName);
						}
						else
						{
							AssignTaxInvNums(repHeadersFooters, repRows, Convert.ToBoolean(_assignInvNumToDocNum.IsChecked), _xmlFileName);
						}
					}
					else if (_importType.Code == Convert.ToString(TaxServiceSourceTypeSE.EInvoicing.Value) && Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
					{
						if (isAccDoc || _accType.Code == Convert.ToString(TaxDocTypeSE.AccDocs.Value))
						{
							SubmittedAccDocs submittedAccDocs = new SubmittedAccDocs();
							submittedAccDocs.ParamSubmitStartDate = _date1.EditValue;
							submittedAccDocs.ParamSubmitEndDate = _date2.EditValue;
							submittedAccDocs.ParamBuyerTIN = _buyerTin.Text;
							submittedAccDocs.ParamBuyerName = _buyerName.Text;
							submittedAccDocs.ParamTaxSerialNumber = _invSerialNumber.Text;
							submittedAccDocs.ParamSupplyStartDate = null;
							submittedAccDocs.ParamSupplyEndDate = null;
							submittedAccDocs.ParamCreationStartDate = null;
							submittedAccDocs.ParamCreationEndDate = null;
							submittedAccDocs.Show(useParamValues: false, openAsChild: true, null, withoutShowDialog: true);
						}
						else
						{
							SubmittedTaxInvoices submittedTaxInvoices = new SubmittedTaxInvoices();
							submittedTaxInvoices.ParamSubmitStartDate = _date1.EditValue;
							submittedTaxInvoices.ParamSubmitEndDate = _date2.EditValue;
							submittedTaxInvoices.ParamBuyerTIN = _buyerTin.Text;
							submittedTaxInvoices.ParamBuyerName = _buyerName.Text;
							submittedTaxInvoices.ParamTaxSerialNumber = _invSerialNumber.Text;
							submittedTaxInvoices.ParamSupplyStartDate = null;
							submittedTaxInvoices.ParamSupplyEndDate = null;
							submittedTaxInvoices.ParamCreationStartDate = null;
							submittedTaxInvoices.ParamCreationEndDate = null;
							submittedTaxInvoices.Show(useParamValues: false, openAsChild: true, null, withoutShowDialog: true);
						}
					}
				}
				catch (Exception ex)
				{
					Mouse.OverrideCursor = null;
					Settings.ErrorHandler(ex);
				}
				Mouse.OverrideCursor = null;
				return true;
			}
			Mouse.OverrideCursor = null;
			return false;
		}

		private void Validation(object window, DialogWindowValidationArgs args)
		{
			DialogWindow dialogWindow = (DialogWindow)window;
			if (_date1.EditValue.HasValue && _date2.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_date1.EditValue.ToDateTime(), _date2.EditValue.ToDateTime());
			}
			if (_importType.Code == Convert.ToString(TaxServiceSourceTypeSE.File.Value) && !File.Exists(Convert.ToString(_path.EditValue)))
			{
				args.isValid = false;
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, Convert.ToString(_path.EditValue)));
				_path.EditValue = "";
				args.SetFocusWhenInvalidSave = _path;
			}
			ArmSoft.SME.Core.UI.ValidationService.OnlyOneNotEmpty(new List<(string, string)>
			{
				(ArmSoft.SME.Common.Resources.BuyerTaxCode, (string)dialogWindow["BuyerTin"]),
				(ArmSoft.SME.Common.Resources.BuyerName, (string)dialogWindow["BuyerName"]),
				(ArmSoft.SME.Common.Resources.TaxSerialNumber, (string)dialogWindow["InvSerialNumber"])
			});
		}

		private void PathButton_Click(object o, RoutedEventArgs args)
		{
			try
			{
				_path.EditValue = Settings.CommonCoreHelper.GetFileNameFromImportDialog(".xml", "XML Files (.xml)|*.xml");
			}
			catch (Exception)
			{
				_path.EditValue = string.Empty;
			}
		}

		private void ImportType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			string code = _importType.Code;
			if (code == Convert.ToString(TaxServiceSourceTypeSE.EInvoicing.Value) || code == string.Empty)
			{
				_path.IsRequired = false;
				_path.IsReadOnly = true;
				_path.IsEnabled = false;
				_path.DoValidate();
				_path.Text = string.Empty;
				_date1.IsReadOnly = false;
				_date1.IsRequired = true;
				_date2.IsReadOnly = false;
				_date2.IsRequired = true;
				_assignInvNumToDocNum.IsChecked = false;
				_assignInvNumToDocNum.IsEnabled = false;
				_assignInvNumToDocNum.IsReadOnly = true;
				_buyerName.IsEnabled = true;
				_buyerName.IsReadOnly = false;
				_buyerTin.IsEnabled = true;
				_buyerTin.IsReadOnly = false;
				_invSerialNumber.IsEnabled = true;
				_invSerialNumber.IsReadOnly = false;
				_accType.IsEnabled = true;
				_accType.IsReadOnly = false;
				_accType.IsRequired = true;
				_accType.DoValidate();
			}
			else if (code == Convert.ToString(TaxServiceSourceTypeSE.File.Value))
			{
				_path.IsEnabled = true;
				_path.IsReadOnly = false;
				_path.IsRequired = true;
				_date1.IsReadOnly = true;
				_date1.IsRequired = false;
				_date1.DoValidate();
				_date2.IsReadOnly = true;
				_date2.IsRequired = false;
				_date2.DoValidate();
				_assignInvNumToDocNum.IsEnabled = true;
				_assignInvNumToDocNum.IsReadOnly = false;
				_buyerName.IsEnabled = false;
				_buyerName.IsReadOnly = true;
				_buyerTin.IsEnabled = false;
				_buyerTin.IsReadOnly = true;
				_invSerialNumber.IsEnabled = false;
				_invSerialNumber.IsReadOnly = true;
				_accType.IsEnabled = false;
				_accType.IsReadOnly = true;
				_accType.IsRequired = false;
				_accType.DoValidate();
			}
		}
	}

	public class ImportTaxInvoicesDialog : TradeDialogWindow
	{
		private ButtonEditExt _path;

		private DropDownControl _importType;

		private DateEditControl _date1;

		private DateEditControl _date2;

		private DateEditControl _supplyDate1;

		private DateEditControl _supplyDate2;

		private CheckEditExt _importReceivedInvDocs;

		private CheckEditExt _importReceivedAccDocs;

		public bool PrepareAndShow()
		{
			Name = "ReceivedTaxInvImport";
			Caption = ArmSoft.SME.Common.Resources.ReceivedTaxInvImport;
			HelpId = "Purchases_Operations_Import";
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			AddStoreValuesButton(container);
			_importType = AddDropDownControl(container, "ImportType", ArmSoft.SME.Common.Resources.Import, new SmartEnum(typeof(TaxServiceSourceTypeSE)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			_importType.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(ImportType_EditValueChanged).Handler;
			LayoutGroupExt container2 = AddHorizontalLayoutGroup();
			_path = AddButtonEditControl(container2, "InputPath", ArmSoft.SME.Common.Resources.ImportFileSelection, 512, isRequired: false, 1);
			_path.IsTextEditable = false;
			_path.AllowDefaultButton = true;
			_path.IsReadOnly = _importType.Code != TaxServiceSourceTypeSE.File.ToString();
			_path.DefaultButtonClick += new SafeEventHandler<RoutedEventArgs>(PathButton_Click).Handler;
			container = AddHorizontalLayoutGroup();
			_date1 = AddDateControl(container, "StartDate", ArmSoft.SME.Trade.Common.Resources.SubmissionDatePeriod, useLongDate: false, isRequired: false, storeValue: true);
			_date2 = AddDateControl(container, "EndDate", "", useLongDate: false, isRequired: false, storeValue: true);
			AddSpecialDateButton(container, _date1, _date2);
			container = AddHorizontalLayoutGroup();
			_supplyDate1 = AddDateControl(container, "SupplyStartDate", ArmSoft.SME.Trade.Common.Resources.SupplyDatePeriod, useLongDate: false, isRequired: false, storeValue: true);
			_supplyDate2 = AddDateControl(container, "SupplyEndDate", "", useLongDate: false, isRequired: false, storeValue: true);
			AddSpecialDateButton(container, _supplyDate1, _supplyDate2);
			_importReceivedInvDocs = AddBooleanControl("ImportReceivedInvDocs", ArmSoft.SME.Common.Resources.TaxInvoices, storeValue: true);
			_importReceivedAccDocs = AddBooleanControl("ImportReceivedAccDocs", ArmSoft.SME.Common.Resources.TaxAccDocs, storeValue: true);
			WindowValidation += new SafeEventHandler<DialogWindowValidationArgs>(Validation).Handler;
			return Convert.ToBoolean(ShowDialog());
		}

		private void Validation(object dialog, DialogWindowValidationArgs args)
		{
			if (_date1.EditValue.HasValue && _date2.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_date1.EditValue.ToDateTime(), _date2.EditValue.ToDateTime());
			}
			if (_supplyDate1.EditValue.HasValue && _supplyDate2.EditValue.HasValue)
			{
				ArmSoft.SME.Core.UI.ValidationService.ValidateWrongPeriod(_supplyDate1.EditValue.ToDateTime(), _supplyDate2.EditValue.ToDateTime());
			}
			if (_importType.Code == Convert.ToString(TaxServiceSourceTypeSE.File) && !File.Exists(Convert.ToString(_path.EditValue)))
			{
				args.isValid = false;
				ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Common.Resources.FileDoesNotExist, Convert.ToString(_path.EditValue)));
				_path.EditValue = "";
				args.SetFocusWhenInvalidSave = _path;
			}
			if (!Convert.ToBoolean(_importReceivedInvDocs.IsChecked) && !Convert.ToBoolean(_importReceivedAccDocs.IsChecked) && _importType.Code == TaxServiceSourceTypeSE.EInvoicing.Value.ToString())
			{
				args.isValid = false;
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.DocTypeNotSpecified);
			}
		}

		private void PathButton_Click(object o, RoutedEventArgs args)
		{
			try
			{
				_path.EditValue = Settings.CommonCoreHelper.GetFileNameFromImportDialog(".xml", "XML Files (.xml)|*.xml");
			}
			catch (Exception)
			{
				_path.EditValue = string.Empty;
			}
		}

		private void ImportType_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(_importType.Code) || _importType.Code == TaxServiceSourceTypeSE.EInvoicing.Value.ToString())
			{
				_path.IsRequired = false;
				_path.IsReadOnly = true;
				_path.IsEnabled = false;
				_path.DoValidate();
				_path.Text = string.Empty;
				_date1.IsReadOnly = false;
				_date1.IsRequired = true;
				_date2.IsReadOnly = false;
				_date2.IsRequired = true;
				_supplyDate1.IsReadOnly = false;
				_supplyDate2.IsReadOnly = false;
				_importReceivedAccDocs.IsEnabled = true;
				_importReceivedAccDocs.IsReadOnly = false;
				_importReceivedInvDocs.IsEnabled = true;
				_importReceivedInvDocs.IsReadOnly = false;
			}
			else if (_importType.Code == TaxServiceSourceTypeSE.File.Value.ToString())
			{
				_path.IsEnabled = true;
				_path.IsReadOnly = false;
				_path.IsRequired = true;
				_date1.IsReadOnly = true;
				_date1.IsRequired = false;
				_date1.DoValidate();
				_date2.IsReadOnly = true;
				_date2.IsRequired = false;
				_date2.DoValidate();
				_supplyDate1.IsReadOnly = true;
				_supplyDate2.IsReadOnly = true;
				_importReceivedAccDocs.IsEnabled = false;
				_importReceivedAccDocs.IsReadOnly = true;
				_importReceivedInvDocs.IsEnabled = false;
				_importReceivedInvDocs.IsReadOnly = true;
			}
		}
	}

	protected override TradeClient Client => TradeSettings.CreateTradeClient();

	public static void AssignTaxInvNums(bool isAccDoc = false, bool isFAMove = false)
	{
		List<string> repHeadersFooters = new List<string>();
		List<AssignTaxInvNumsRepRow> repRows = new List<AssignTaxInvNumsRepRow>();
		AssignTaxInvNumsDialog assignTaxInvNumsDialog = new AssignTaxInvNumsDialog();
		try
		{
			if (!assignTaxInvNumsDialog.PrepareAndShow(ref repHeadersFooters, ref repRows, isAccDoc) || repHeadersFooters.Count == 0 || string.IsNullOrWhiteSpace(repHeadersFooters[0]))
			{
				return;
			}
			if (repRows.Count == 0)
			{
				if (!isAccDoc)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NoInvoiceInXML, ArmSoft.SME.Common.Resources.AssignTaxInvNums, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NoMTMoveDocInXML, ArmSoft.SME.Common.Resources.AssignTaxInvNums, MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
			else
			{
				new TaxDocumentReportViewer.AssignInvNumReportViewer(repHeadersFooters, repRows, isAccDoc).Show();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static void AssignTaxInvNums(List<string> repHeadersFooters, List<AssignTaxInvNumsRepRow> repRows, bool assignTaxInvNumToDocNum, string xmlFileName)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(xmlFileName))
			{
				return;
			}
			List<TaxDocument> list = TaxServiceHelper.LoadTaxDocuments(xmlFileName);
			if (list.Count == 0)
			{
				return;
			}
			(DateOnly minDate, DateOnly maxDate) taxInvoicesMinAndMaxDates = TaxServiceHelper.GetTaxInvoicesMinAndMaxDates(list);
			DateOnly item = taxInvoicesMinAndMaxDates.minDate;
			DateOnly item2 = taxInvoicesMinAndMaxDates.maxDate;
			TaxServiceResponse taxServiceResponse = Settings.TaxServiceHelper.MapTaxDocuments(list, item.ToDateTime(), item2.ToDateTime(), assignTaxInvNumToDocNum);
			foreach (TaxServiceItems item3 in taxServiceResponse.Items)
			{
				repRows.Add(TaxServiceHelper.BuildResultRow(item3));
			}
			repHeadersFooters.Add(Convert.ToString(taxServiceResponse.AssignedCount));
			repHeadersFooters.Add(Convert.ToString(taxServiceResponse.NotAssignedCount));
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static void AssignMTMoveInvNums(List<string> repHeadersFooters, List<AssignTaxInvNumsRepRow> repRows, bool assignAccInvNumToDocNum, string xmlFileName)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
		xmlNamespaceManager.AddNamespace("ts", "http://www.taxservice.am/tp3/invoice/definitions");
		List<AccDoc> list = new List<AccDoc>();
		uint num = 0u;
		if (string.IsNullOrWhiteSpace(xmlFileName))
		{
			return;
		}
		xmlDocument.Load(xmlFileName);
		foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
		{
			DateTimeOffset result;
			if (childNode.NodeType == XmlNodeType.Element && childNode.Name == "AccountingDocument")
			{
				AssignTaxInvNumsRepRow assignTaxInvNumsRepRow = new AssignTaxInvNumsRepRow();
				XmlNode xmlNode2 = childNode.SelectSingleNode("ts:GeneralInfo", xmlNamespaceManager);
				if (xmlNode2 != null)
				{
					XmlNode xmlNode3 = xmlNode2.SelectSingleNode("ts:DeliveryDate", xmlNamespaceManager);
					if (xmlNode3 != null && DateTimeOffset.TryParse(xmlNode3.InnerText, out result))
					{
						assignTaxInvNumsRepRow.Date = new DateTime(result.Year, result.Month, result.Day);
					}
				}
				assignTaxInvNumsRepRow.Assigned = ArmSoft.SME.Common.Resources.No + " ";
				assignTaxInvNumsRepRow.SeralNum_NotAssignReason = ArmSoft.SME.Common.Resources.NotSigned;
				assignTaxInvNumsRepRow.DocNumbers = "";
				num++;
				repRows.Add(assignTaxInvNumsRepRow);
			}
			else
			{
				if (childNode.NodeType != XmlNodeType.Element || !(childNode.Name == "SignedAccDocData"))
				{
					continue;
				}
				AccDoc accDoc = new AccDoc();
				XmlNode xmlNode4 = childNode.SelectSingleNode("ts:AccDocMetadata", xmlNamespaceManager);
				if (xmlNode4 != null)
				{
					xmlNode4 = xmlNode4.SelectSingleNode("ts:SubmissionDate", xmlNamespaceManager);
					if (xmlNode4 != null && DateTimeOffset.TryParse(xmlNode4.InnerText, out result))
					{
						accDoc.SubmissionDate = new DateOnly(result.Year, result.Month, result.Day);
					}
				}
				XmlNode xmlNode5 = childNode.SelectSingleNode("ts:Data", xmlNamespaceManager);
				if (xmlNode5 == null)
				{
					continue;
				}
				XmlNode xmlNode6 = xmlNode5.SelectSingleNode("ts:SignableData", xmlNamespaceManager);
				if (xmlNode6 == null)
				{
					continue;
				}
				XmlNode xmlNode7 = xmlNode6.SelectSingleNode("ts:GeneralInfo", xmlNamespaceManager);
				if (xmlNode7 == null)
				{
					continue;
				}
				xmlNode4 = xmlNode7.SelectSingleNode("ts:DeliveryDate", xmlNamespaceManager);
				if (xmlNode4 != null && DateTimeOffset.TryParse(xmlNode4.InnerText, out result))
				{
					accDoc.DocDate = new DateTime(result.Year, result.Month, result.Day);
				}
				XmlNode xmlNode8 = xmlNode7.SelectSingleNode("ts:InvoiceNumber", xmlNamespaceManager);
				if (xmlNode8 != null)
				{
					xmlNode4 = xmlNode8.SelectSingleNode("ts:Number", xmlNamespaceManager);
					if (xmlNode4 != null)
					{
						accDoc.TaxNumber = xmlNode4.InnerText;
					}
					xmlNode4 = xmlNode8.SelectSingleNode("ts:Series", xmlNamespaceManager);
					if (xmlNode4 != null)
					{
						accDoc.TaxSeries = xmlNode4.InnerText;
					}
				}
				xmlNode4 = xmlNode7.SelectSingleNode("ts:AdditionalData", xmlNamespaceManager);
				if (xmlNode4 != null)
				{
					accDoc.AdditionalData = xmlNode4.InnerText;
				}
				XmlNode xmlNode9 = xmlNode6.SelectSingleNode("ts:GoodsInfo", xmlNamespaceManager);
				if (xmlNode9 != null)
				{
					XmlNodeList xmlNodeList = xmlNode9.SelectNodes("ts:Good", xmlNamespaceManager);
					accDoc.RowCount = xmlNodeList.Count;
					foreach (XmlNode item in xmlNodeList)
					{
						xmlNode4 = item.SelectSingleNode("ts:Amount", xmlNamespaceManager);
						if (xmlNode4 != null && decimal.TryParse(xmlNode4.InnerText, out var result2))
						{
							accDoc.TotalQuantity += result2;
						}
					}
				}
				list.Add(accDoc);
			}
		}
		TaxServiceResponse taxServiceResponse = Settings.TaxServiceHelper.FindMTMoveDocuments(list, assignAccInvNumToDocNum);
		foreach (TaxServiceItems item2 in taxServiceResponse.Items)
		{
			repRows.Add(TaxServiceHelper.BuildResultRow(item2));
		}
		repHeadersFooters.Add(Convert.ToString(taxServiceResponse.AssignedCount));
		repHeadersFooters.Add(Convert.ToString(taxServiceResponse.NotAssignedCount + num));
	}

	public static bool ImportReceivedTaxDocuments()
	{
		ImportTaxInvoicesDialog importTaxInvoicesDialog = new ImportTaxInvoicesDialog();
		try
		{
			if (importTaxInvoicesDialog.PrepareAndShow())
			{
				Mouse.OverrideCursor = Cursors.Wait;
				string text = Convert.ToString(importTaxInvoicesDialog["InputPath"]);
				string text2 = Convert.ToString(importTaxInvoicesDialog["ImportType"]);
				bool importReceivedInvDocs = Convert.ToBoolean(importTaxInvoicesDialog["ImportReceivedInvDocs"]);
				bool importReceivedAccDocs = Convert.ToBoolean(importTaxInvoicesDialog["ImportReceivedAccDocs"]);
				DateTime? issuedDateStart = ((DateOnly?)importTaxInvoicesDialog["StartDate"]).ToDateTime();
				DateTime? issuedDateEnd = ((DateOnly?)importTaxInvoicesDialog["EndDate"]).ToDateTime();
				DateTime? supplyDateStart = ((DateOnly?)importTaxInvoicesDialog["SupplyStartDate"]).ToDateTime();
				DateTime? supplyDateEnd = ((DateOnly?)importTaxInvoicesDialog["SupplyEndDate"]).ToDateTime();
				List<TaxDocument> taxDocuments = new List<TaxDocument>();
				List<string> list = new List<string>();
				List<TaxDocumentRepRow> repRows = new List<TaxDocumentRepRow>();
				if (text2 == TaxServiceSourceTypeSE.File.Value.ToString())
				{
					if (string.IsNullOrEmpty(text))
					{
						Mouse.OverrideCursor = null;
						return false;
					}
					list.Add(text);
					taxDocuments = TaxServiceHelper.LoadTaxDocuments(text);
					foreach (TaxDocument item in taxDocuments)
					{
						if (!item.BuyerHasNoTin)
						{
							IReadOnlyList<PartnerCacheRow> partnersByTaxCode = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(item.SupplierTIN);
							if (partnersByTaxCode.Count == 0)
							{
								TradeDirectoryService.StorePartnerFromB2BData(item.SupplierTIN, item.SupplierBankAccountNumber, item.SupplierBankName);
							}
							else
							{
								TradeDirectoryService.StorePartnerBankAccount(partnersByTaxCode[0].PartCode, item.SupplierBankAccountNumber, item.SupplierBankName);
							}
						}
					}
					ImportReceivedTaxDocuments(out var succImpCount, out var matchedCount, out repRows, ref taxDocuments);
					list.Add(matchedCount.ToString());
					list.Add(succImpCount.ToString());
				}
				else if (text2 == TaxServiceSourceTypeSE.EInvoicing.Value.ToString())
				{
					if (!Settings.TaxServiceHelper.EnsureTaxServiceAuthenticationCompleted())
					{
						Mouse.OverrideCursor = null;
						return false;
					}
					list.Add(ArmSoft.SME.Common.Resources.ReceivedTaxInvImport);
					LoadTaxDocuments(importReceivedInvDocs, importReceivedAccDocs, issuedDateStart, issuedDateEnd, supplyDateStart, supplyDateEnd, ref taxDocuments);
					foreach (TaxDocument item2 in taxDocuments)
					{
						if (!item2.BuyerHasNoTin)
						{
							IReadOnlyList<PartnerCacheRow> partnersByTaxCode2 = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(item2.SupplierTIN);
							if (partnersByTaxCode2.Count == 0)
							{
								TradeDirectoryService.StorePartnerFromB2BData(item2.SupplierTIN, item2.SupplierBankAccountNumber, item2.SupplierBankName);
							}
							else
							{
								TradeDirectoryService.StorePartnerBankAccount(partnersByTaxCode2[0].PartCode, item2.SupplierBankAccountNumber, item2.SupplierBankName);
							}
						}
					}
					ImportReceivedTaxDocuments(out var succImpCount2, out var matchedCount2, out repRows, ref taxDocuments);
					list.Add(matchedCount2.ToString());
					list.Add(succImpCount2.ToString());
				}
				Mouse.OverrideCursor = null;
				if (string.IsNullOrWhiteSpace(list[0]))
				{
					return false;
				}
				if (repRows.Count == 0)
				{
					ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.NoImportedInv, ArmSoft.SME.Common.Resources.ReceivedTaxInvImport, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return false;
				}
				new TaxDocumentReportViewer.ImportReceivedTaxInvoiceRepViewer(list, repRows).Show();
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			Mouse.OverrideCursor = null;
			Settings.ErrorHandler(ex);
			return false;
		}
	}

	private static void LoadTaxDocuments(bool importReceivedInvDocs, bool importReceivedAccDocs, DateTime? issuedDateStart, DateTime? issuedDateEnd, DateTime? supplyDateStart, DateTime? supplyDateEnd, ref List<TaxDocument> taxDocuments)
	{
		if (importReceivedInvDocs)
		{
			List<TaxDoc> receivedInvoices = Settings.TaxService.Client.Invoice.GetReceivedInvoices(issuedDateStart ?? ((DateTime?)null), issuedDateEnd ?? ((DateTime?)null), null, null, null, supplyDateStart ?? ((DateTime?)null), supplyDateEnd ?? ((DateTime?)null));
			if (receivedInvoices != null)
			{
				foreach (TaxDoc item6 in receivedInvoices)
				{
					if ((short)item6.InvoiceType == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Goods.Value))
					{
						Good good = Settings.TaxService.Client.Goods.Get(item6.Id, loadItems: true);
						if (good != null)
						{
							TaxDocument item = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(good);
							taxDocuments.Add(item);
						}
					}
					else if ((short)item6.InvoiceType == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Excise.Value))
					{
						Excise excise = Settings.TaxService.Client.Excise.Get(item6.Id, loadItems: true);
						if (excise != null)
						{
							TaxDocument item2 = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(excise);
							taxDocuments.Add(item2);
						}
					}
					else if ((short)item6.InvoiceType == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Services.Value))
					{
						Service service = Settings.TaxService.Client.Services.Get(item6.Id, loadItems: true);
						if (service != null)
						{
							TaxDocument item3 = Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument(service);
							taxDocuments.Add(item3);
						}
					}
				}
			}
		}
		if (!importReceivedAccDocs)
		{
			return;
		}
		List<TaxDoc> receivedAccDocs = Settings.TaxService.Client.Invoice.GetReceivedAccDocs(issuedDateStart ?? ((DateTime?)null), issuedDateEnd ?? ((DateTime?)null), null, null, null, supplyDateStart ?? ((DateTime?)null), supplyDateEnd ?? ((DateTime?)null));
		if (receivedAccDocs == null)
		{
			return;
		}
		foreach (TaxDoc item7 in receivedAccDocs)
		{
			if ((short)item7.AccDocType == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value))
			{
				AccDocGoods accDocGoods = Settings.TaxService.Client.AccDocGoods.Get(item7.Id, loadItems: true);
				if (accDocGoods != null)
				{
					TaxDocument item4 = Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(accDocGoods);
					taxDocuments.Add(item4);
				}
			}
			else if ((short)item7.AccDocType == Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value))
			{
				AccDocServices accDocServices = Settings.TaxService.Client.AccDocServices.Get(item7.Id, loadItems: true);
				if (accDocServices != null)
				{
					TaxDocument item5 = Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument(accDocServices);
					taxDocuments.Add(item5);
				}
			}
		}
	}

	public static void ImportReceivedTaxDocuments(out long succImpCount, out long matchedCount, out List<TaxDocumentRepRow> repRows, ref List<TaxDocument> taxDocuments)
	{
		succImpCount = 0L;
		matchedCount = 0L;
		repRows = new List<TaxDocumentRepRow>();
		if (taxDocuments.Count > 0)
		{
			repRows.AddRange(Settings.TaxServiceHelper.ImportTaxDocuments(taxDocuments));
		}
		succImpCount = repRows.Count((TaxDocumentRepRow p) => p.Imported);
		matchedCount = repRows.Count((TaxDocumentRepRow p) => !p.Imported);
	}

	public static void SetTotals(TaxDocument doc)
	{
		decimal num = 0m;
		decimal num2 = 0m;
		decimal num3 = 0m;
		decimal totalEnvironmentalFee = 0m;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			num3 += item.Price;
			num += Convert.ToDecimal(item.VAT);
			num2 += Convert.ToDecimal(item.Excise);
			totalEnvironmentalFee += Convert.ToDecimal(item.EnvironmentalFee);
		}
		doc.TotalPrice = num3;
		doc.TotalVAT = num;
		doc.TotalExcise = num2;
		doc.TotalSumWithoutTaxes = num3 - num - num2;
		doc.TotalEnvironmentalFee = totalEnvironmentalFee;
	}
}
