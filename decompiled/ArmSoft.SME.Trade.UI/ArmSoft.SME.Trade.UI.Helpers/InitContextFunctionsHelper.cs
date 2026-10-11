using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.CustomTypes;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Report;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Data.DropDowns;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.DPR;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Models.Other;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataView;
using ArmSoft.SME.Trade.UI.Documents;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDown;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Grid;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class InitContextFunctionsHelper
{
	public class FindRowByBarCodeDialog : DialogWindow
	{
		private readonly string _mtCodeColumnName;

		private readonly CheckEditExt _preservePreviousSelection;

		private readonly TextEditExt _barcode;

		public bool PreservePreviousSelection { get; private set; }

		public FindRowByBarCodeDialog(string mtCodeColumnName)
		{
			Name = "FindRowByBarCodeDialog";
			Caption = ArmSoft.SME.Common.Resources.SearchByBarcode;
			_mtCodeColumnName = mtCodeColumnName;
			LayoutGroupExt container = AddHorizontalLayoutGroup();
			_barcode = AddStringControl(container, "Barcode", ArmSoft.SME.Common.Resources.BarCode, 20);
			AddStoreValuesButton(container);
			_preservePreviousSelection = AddBooleanControl("PreservePreviousSelection", ArmSoft.SME.Common.Resources.PreserveSelection, storeValue: true);
			SetFocusOnActivation = _barcode;
			PreviewKeyDown += new SafeEventHandler<KeyEventArgs>(Dialog_PreviewKeyDown).Handler;
			WindowValidation += FindByBarCodeDialogValidation;
		}

		private void FindByBarCodeDialogValidation(DialogWindow dialog, DialogWindowValidationArgs args)
		{
			try
			{
				PreservePreviousSelection = _preservePreviousSelection.IsChecked.Value;
				FocusRowByBarcode((string)dialog["Barcode"], _mtCodeColumnName, PreservePreviousSelection);
			}
			catch (Exception ex)
			{
				Settings.ErrorHandler(ex);
				args.isValid = false;
				args.SetFocusWhenInvalidSave = dialog.GetUIElement("Barcode");
			}
		}

		private void Dialog_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Invalid comparison between Unknown and I4
			if ((int)e.Key == 6)
			{
				try
				{
					PreservePreviousSelection = Convert.ToBoolean(_preservePreviousSelection.IsChecked);
					FocusRowByBarcode(_barcode.EditText, _mtCodeColumnName, PreservePreviousSelection);
					DialogResult = true;
				}
				catch (Exception ex)
				{
					Settings.ErrorHandler(ex);
				}
			}
		}
	}

	public static ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE ChooseDocTypeFromMenu(List<short> docTypes)
	{
		try
		{
			DocumentType documentType = new DocumentType(docTypes, checkViewAccess: false, checkAddAccess: true);
			if (documentType.ParametersProvider.Values.Count != 0)
			{
				ModalBrowser modalBrowser = new ModalBrowser(documentType)
				{
					Owner = Settings.HostingEnvironment,
					WindowStartupLocation = WindowStartupLocation.CenterScreen
				};
				if (modalBrowser.Show())
				{
					return SmartEnum<ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE, short>.FromValue(short.Parse(documentType.ValueMember(modalBrowser.SelectedRowIndex)));
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return null;
	}

	public static void CreateNewDocFromMenu(List<short> docTypes)
	{
		ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE documentTypeSE = ChooseDocTypeFromMenu(docTypes);
		if (documentTypeSE != null)
		{
			Settings.DocumentService.ShowWindow((TradeDocument)Settings.DocumentService.Create(documentTypeSE), WindowViewMode.NewCreate, Settings.HostingEnvironment.CurrentViewPanel.Id);
		}
	}

	public static void ChangeGoodsBySegments(List<string> goodsCodes)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.GoodsBySegment
		};
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl("AddToSegment", ArmSoft.SME.Trade.Common.Resources.AddToSegment, new ArmSoft.SME.Trade.UI.DropDown.Segments(), isRequired: true);
		DropDownControl dropDownControl2 = dialogWindow.AddDropDownControl("RemoveFromSegment", ArmSoft.SME.Trade.Common.Resources.RemoveFromSegment, new ArmSoft.SME.Trade.UI.DropDown.Segments());
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.StateChangeComment, "ChangeGoodsBySegments");
		GoodsBySegmentsSelectedChangeRequestModel request = new GoodsBySegmentsSelectedChangeRequestModel
		{
			GoodsCodes = goodsCodes,
			AddToSegmentCode = dropDownControl.Code,
			RemoveFromSegmentCode = dropDownControl2.Code
		};
		GoodsBySegmentsSelectedChangeResponseModel goodsBySegmentsSelectedChangeResponseModel = DataProcessingRequestHelper.Execute<GoodsBySegmentsSelectedChangeRequestModel, GoodsBySegmentsSelectedChangeResponseModel>(DPRType.Other, "GoodsBySegmentsSelectedChange", request);
		if (goodsBySegmentsSelectedChangeResponseModel.Exceptions != null && goodsBySegmentsSelectedChangeResponseModel.Exceptions.Count != 0)
		{
			foreach (GoodsBySegmentsChangeResponseModel exception in goodsBySegmentsSelectedChangeResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message);
			}
		}
		ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.SelectedMaterialsAddToSegment, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.OK, MessageBoxImage.Asterisk);
	}

	public static bool ChangeDocState(List<Guid> isns, ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE state)
	{
		bool result = false;
		if (isns == null || isns.Count == 0)
		{
			return false;
		}
		if (isns.Count == 1)
		{
			DefinitionMappingsService.TryResolveDocumentDefinition(Settings.DocumentService.GetDocType(isns[0]), out var definition);
			if (!definition.AllowStoreAsDraft)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.UnnecessaryMakeThisOperation);
			}
			if (ArmSoft.SME.Core.UI.MessageBox.Show(string.Format((state == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft) ? ArmSoft.SME.Common.Resources.ConfDocDraft : ArmSoft.SME.Common.Resources.ConfProcDrDoc, definition.FullCaption), Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Mouse.OverrideCursor = Cursors.Wait;
				Settings.DocumentService.ChangeDocState(isns[0], state, regenerateEntries: false);
				Settings.HostingEnvironment.CurrentViewPanel.Update(isns[0]);
				Mouse.OverrideCursor = null;
				result = true;
			}
		}
		else if (ArmSoft.SME.Core.UI.MessageBox.Show((state == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft) ? ArmSoft.SME.Common.Resources.ConfSelDocDraft : ArmSoft.SME.Common.Resources.ConfSelDocProc, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
		{
			EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.StateChangeComment, "ChangeDocStateErrors");
			Settings.DocumentService.ChangeDocState(isns, state, regenerateEntries: false, enumerateErrorsReportViewer);
			Settings.HostingEnvironment.CurrentViewPanel.Update(isns.Cast<object>().ToList());
			if (enumerateErrorsReportViewer.HasErrors)
			{
				enumerateErrorsReportViewer.Show();
			}
			result = true;
		}
		return result;
	}

	public static void EditDocumentOrderNumbers(ChangeDocOrderNumTypesSE opType)
	{
		List<ChangeDocumentOrderNumbersProperties> list = new List<ChangeDocumentOrderNumbersProperties>();
		EnumerateErrorsReportViewer enumerateErrorsReportViewer = new EnumerateErrorsReportViewer(ArmSoft.SME.Common.Resources.EditErrors, "EditSelectedErrors");
		List<DocumentsJournalDataProvider.DataRow> list2 = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>().ToList();
		foreach (DocumentsJournalDataProvider.DataRow item in list2)
		{
			if (item.DocState == ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft.Value)
			{
				list.Add(new ChangeDocumentOrderNumbersProperties
				{
					ISN = item.ISN,
					DocDate = item.DocDate,
					DocOrderNum = item.OrderNum
				});
			}
		}
		ChangeDocumentOrderNumbersRequestModel request = new ChangeDocumentOrderNumbersRequestModel
		{
			Properties = list,
			OpType = opType
		};
		DocumentEditFieldsResponseModel documentEditFieldsResponseModel = DataProcessingRequestHelper.Execute<ChangeDocumentOrderNumbersRequestModel, DocumentEditFieldsResponseModel>(DPRType.Other, "TradeChangeDocumentOrderNumbers", request);
		if (documentEditFieldsResponseModel.Exceptions != null && documentEditFieldsResponseModel.Exceptions.Count != 0)
		{
			foreach (DocumentEditResponseModel exception in documentEditFieldsResponseModel.Exceptions)
			{
				enumerateErrorsReportViewer.AddError(exception.Exception.Message, exception.DocISN);
			}
		}
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		List<object> list3 = new List<object>();
		foreach (Guid item2 in list2.Select((DocumentsJournalDataProvider.DataRow x) => x.ISN))
		{
			list3.Add(item2);
		}
		currentViewPanel.Update(list3);
		if (enumerateErrorsReportViewer.HasErrors)
		{
			enumerateErrorsReportViewer.Show();
		}
	}

	public static void GroupCreateNewReceivedServices()
	{
		string text = Settings.ParametersService.ReceivedServiceNumber();
		List<Guid> list = new List<Guid>();
		foreach (DocumentsJournalDataProvider.DataRow item2 in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<DocumentsJournalDataProvider.DataRow>().ToList())
		{
			Document document = Settings.DocumentService.Load(item2.ISN, item2.DocType);
			ReceivedTaxInvoice receivedTaxInvoiceBase = (ReceivedTaxInvoice)document;
			ReceivedService receivedService = Settings.DocumentService.Create<ReceivedService>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.ReceivedService);
			receivedService.OwnerID = Settings.LoginService.UserId;
			receivedService.Owner = Settings.LoginService.UserName;
			receivedService.BaseISN = receivedTaxInvoiceBase.ISN;
			receivedService.DocNum = text;
			bool flag = receivedTaxInvoiceBase.Type == Convert.ToInt16("3");
			receivedService.DocDate = receivedTaxInvoiceBase.DocDate;
			PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetList((PartnerCacheRow p) => p.TaxCode == receivedTaxInvoiceBase.SupplierTIN && p.Supplier).FirstOrDefault();
			if (partnerCacheRow != null)
			{
				receivedService.PartnerContractGroup.PartnerCode = partnerCacheRow.PartCode;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
				receivedService.PartnerContractGroup.ContractCode = defaultContract?.Contract;
			}
			receivedService.ExportedType = TaxInvoicesWriteTypesSE.TaxInvExportFormElectronic;
			if (receivedTaxInvoiceBase.Series.InList("Բ", "B"))
			{
				if (!receivedService.VATType.InList(VatTypesSE.CostWithoutVAT.Value, VatTypesSE.CostZeroVAT.Value))
				{
					receivedService.VATType = VatTypesSE.CostWithoutVAT.Value;
				}
			}
			else if (!receivedService.VATType.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value))
			{
				receivedService.VATType = VatTypesSE.CostWithVAT.Value;
			}
			receivedService.InvoiceNumber = receivedTaxInvoiceBase.Series + receivedTaxInvoiceBase.TaxNumber;
			receivedService.Comment = receivedTaxInvoiceBase.AdditionalInfo;
			foreach (TaxDocument.SpecificationRow item3 in receivedTaxInvoiceBase.Items)
			{
				if (!string.IsNullOrWhiteSpace(item3.ItemCode) && item3.ItemType == ItemTypesSE.Services.Value)
				{
					ReceivedService.ReceivedServiceSpecificationRow receivedServiceSpecificationRow = new ReceivedService.ReceivedServiceSpecificationRow
					{
						ServCode = item3.ItemCode,
						Quantity = ((item3.Quantity == 0m) ? 1m : (Math.Truncate(item3.Quantity * 1000m) / 1000m)),
						VAT = (item3.SumVAT > 0m)
					};
					if (receivedService.VATType == VatTypesSE.CostWithInVAT.Value)
					{
						receivedServiceSpecificationRow.Summa = item3.TotalSum.RoundAs(2);
					}
					else
					{
						receivedServiceSpecificationRow.Summa = item3.Sum.RoundAs(2);
					}
					if (flag)
					{
						receivedServiceSpecificationRow.Summa += item3.Excise.RoundAs(2);
					}
					receivedServiceSpecificationRow.PriceVAT = (receivedServiceSpecificationRow.Summa / receivedServiceSpecificationRow.Quantity).RoundAs(2);
					receivedServiceSpecificationRow.Price = (receivedServiceSpecificationRow.Summa / receivedServiceSpecificationRow.Quantity).RoundAs(2);
					if (receivedService.VATType != "2")
					{
						receivedServiceSpecificationRow.PriceVAT = receivedServiceSpecificationRow.Price;
					}
					receivedService.SummVAT += item3.SumVAT;
					receivedService.Summ += (decimal?)item3.TotalSum;
					MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(receivedServiceSpecificationRow.ServCode);
					if (item != null)
					{
						receivedServiceSpecificationRow.ServID = item.Id;
						receivedServiceSpecificationRow.ServName = item.Caption;
						receivedServiceSpecificationRow.UnitCode = item3.UnitCodeFromDirectory;
						receivedServiceSpecificationRow.UnitBrief = item3.UnitBriefFromDirectory;
						receivedServiceSpecificationRow.UnitFraction = true;
					}
					receivedService.ReceivedServiceList.Add(receivedServiceSpecificationRow);
				}
			}
			if (receivedService.ReceivedServiceList.Count != 0)
			{
				Settings.DocumentService.Store(receivedService, new StoreRequest());
				text = StringExtensions.NextDocNum(receivedService.DocNum, 12);
				Settings.ParametersService.ReceivedServiceNumber_set(text);
				list.Add(item2.ISN);
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(list.Cast<object>().ToList());
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CreatedDocCount, list.Count));
	}

	public static void GroupCreateNewMTIncomes(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType)
	{
		DialogWindow dialogWindow = new DialogWindow();
		bool flag = (short)docType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail.Value;
		dialogWindow.Caption = (flag ? ArmSoft.SME.Trade.Common.Resources.DocMTIncomeRetail : ArmSoft.SME.Common.Resources.DocMTIncome);
		dialogWindow.Name = "StorageDlg";
		DropDownControl dropDownControl = dialogWindow.AddDropDownControl("Storage", ArmSoft.SME.Common.Resources.Storage, new ArmSoft.SME.Trade.UI.DropDown.Storages(flag ? ((bool?)null) : new bool?(false)), isRequired: true);
		CheckEditExt checkEditExt = dialogWindow.AddBooleanControl("IncludeVATInCost", ArmSoft.SME.Trade.Common.Resources.IncludeVATInCost);
		checkEditExt.IsChecked = Settings.ParametersService.IncludeVATInCost();
		List<Guid> list = new List<Guid>();
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		foreach (DocumentsJournalDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>())
		{
			Document document = Settings.DocumentService.Load(selectedRow.ISN, selectedRow.DocType);
			ReceivedTaxInvoice baseReceivedTaxInvoice = (ReceivedTaxInvoice)document;
			bool excise = baseReceivedTaxInvoice.Type == Convert.ToInt16("3");
			decimal summaVAT = 0m;
			decimal totalSumma = 0m;
			MTIncomeBase mTIncomeBase = ((!flag) ? ((MTIncomeBase)Settings.DocumentService.Create<MTIncome>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome)) : ((MTIncomeBase)Settings.DocumentService.Create<MTIncomeRetail>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail)));
			mTIncomeBase.BaseISN = baseReceivedTaxInvoice.ISN;
			mTIncomeBase.DocDate = baseReceivedTaxInvoice.DocDate;
			mTIncomeBase.Storage = dropDownControl.Code;
			mTIncomeBase.CurrCode = "AMD";
			PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetList().FirstOrDefault((PartnerCacheRow p) => p.TaxCode == baseReceivedTaxInvoice.SupplierTIN && p.Supplier);
			if (partnerCacheRow != null)
			{
				mTIncomeBase.PartnerContractGroup.PartnerCode = partnerCacheRow.PartCode;
				PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
				mTIncomeBase.PartnerContractGroup.ContractCode = defaultContract?.Contract;
			}
			mTIncomeBase.ExportedType = TaxInvoicesWriteTypesSE.TaxInvExportFormElectronic;
			mTIncomeBase.InvoiceNumber = baseReceivedTaxInvoice.Series + baseReceivedTaxInvoice.TaxNumber;
			mTIncomeBase.InvoiceDate = baseReceivedTaxInvoice.InvoiceDate;
			if (baseReceivedTaxInvoice.Series.InList("Բ", "B"))
			{
				if (!mTIncomeBase.VATType.InList(VatTypesSE.CostWithoutVAT.Value, VatTypesSE.CostZeroVAT.Value))
				{
					mTIncomeBase.VATType = VatTypesSE.CostWithoutVAT.Value;
				}
			}
			else if (!mTIncomeBase.VATType.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value))
			{
				mTIncomeBase.VATType = VatTypesSE.CostWithVAT.Value;
			}
			mTIncomeBase.IncludeVATInCost = checkEditExt.IsChecked.Value && mTIncomeBase.VATType.InList(VatTypesSE.CostWithVAT.Value, VatTypesSE.CostWithInVAT.Value);
			mTIncomeBase.Trust = baseReceivedTaxInvoice.MediatorLicenseNumber;
			mTIncomeBase.BaseISN = baseReceivedTaxInvoice.ISN;
			mTIncomeBase.Comment = baseReceivedTaxInvoice.AdditionalInfo;
			if (flag)
			{
				mTIncomeBase.DocNum = Settings.ParametersService.MTIncomeRetailNumber();
			}
			else
			{
				mTIncomeBase.DocNum = Settings.ParametersService.MTIncomeNumber();
			}
			mTIncomeBase.Chief = Settings.ParametersService.ChiefAccountantName();
			mTIncomeBase.MTList.Clear();
			foreach (TaxDocument.SpecificationRow item in baseReceivedTaxInvoice.Items)
			{
				AddMTIncomeGridRow(mTIncomeBase.MTList, item, mTIncomeBase.VATType, ref summaVAT, ref totalSumma, excise, flag, partnerCacheRow?.Id ?? (-1));
			}
			mTIncomeBase.MaxRowID = mTIncomeBase.MTList.Count;
			mTIncomeBase.SummVAT = summaVAT;
			mTIncomeBase.Summ = totalSumma;
			mTIncomeBase.OwnerID = Settings.LoginService.UserId;
			mTIncomeBase.Owner = Settings.LoginService.UserName;
			if (mTIncomeBase.MTList.Count != 0)
			{
				Settings.DocumentService.Store(mTIncomeBase, new StoreRequest());
				list.Add(selectedRow.ISN);
				string value = StringExtensions.NextDocNum(mTIncomeBase.DocNum, 12);
				if (flag)
				{
					Settings.ParametersService.MTIncomeRetailNumber_set(value);
				}
				else
				{
					Settings.ParametersService.MTIncomeNumber_set(value);
				}
			}
		}
		Settings.HostingEnvironment.CurrentViewPanel.Update(list.Cast<object>().ToList());
		ArmSoft.SME.Core.UI.MessageBox.Show(string.Format(ArmSoft.SME.Trade.Common.Resources.CreatedDocCount, list.Count));
	}

	private static void AddMTIncomeGridRow(List<MTIncomeBase.MTIncomeSpecificationRow> destList, TaxDocument.SpecificationRow sourceRow, string vatType, ref decimal summaVAT, ref decimal totalSumma, bool excise = false, bool isRetailDoc = false, int partnerId = -1)
	{
		MTIncomeBase.MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeBase.MTIncomeSpecificationRow();
		if (string.IsNullOrWhiteSpace(sourceRow.ItemCode) || !(sourceRow.ItemType == InvoiceRowTypeSE.Goods.Value))
		{
			return;
		}
		mTIncomeSpecificationRow.MTCode = sourceRow.ItemCode;
		mTIncomeSpecificationRow.Quantity = Math.Truncate(sourceRow.Quantity * 1000m) / 1000m;
		mTIncomeSpecificationRow.Summa = ((vatType == VatTypesSE.CostWithInVAT.Value) ? sourceRow.TotalSum : sourceRow.Sum);
		if (partnerId != -1)
		{
			PartnerGoodCacheRow itemFromMapping = TradeSettings.TradeCacheService.PartnerGood.GetItemFromMapping(partnerId, sourceRow.ItemExternalCode);
			if (itemFromMapping != null && itemFromMapping.Coeff != 0m && itemFromMapping.Coeff != 1m)
			{
				mTIncomeSpecificationRow.Quantity = (mTIncomeSpecificationRow.Quantity * itemFromMapping.Coeff).RoundAs(3);
			}
		}
		if (mTIncomeSpecificationRow.Quantity != 0m)
		{
			mTIncomeSpecificationRow.DiscountPrice = (mTIncomeSpecificationRow.Summa / mTIncomeSpecificationRow.Quantity).RoundAs(2);
		}
		else
		{
			mTIncomeSpecificationRow.DiscountPrice = sourceRow.Price.RoundAs(2);
		}
		if (excise)
		{
			if (mTIncomeSpecificationRow.Quantity != 0m)
			{
				mTIncomeSpecificationRow.DiscountPrice += (sourceRow.Excise / mTIncomeSpecificationRow.Quantity).RoundAs(2);
			}
			mTIncomeSpecificationRow.Summa += sourceRow.Excise.RoundAs(2);
		}
		if (Settings.ParametersService.AllowDiscountInPurchase())
		{
			mTIncomeSpecificationRow.Discount = sourceRow.Discount.RoundAs(2);
			if (mTIncomeSpecificationRow.Discount == 100m || mTIncomeSpecificationRow.Discount == 0m)
			{
				mTIncomeSpecificationRow.Discount = 0m;
				mTIncomeSpecificationRow.Price = mTIncomeSpecificationRow.DiscountPrice;
			}
			else
			{
				mTIncomeSpecificationRow.Price = (100m * mTIncomeSpecificationRow.DiscountPrice / (100m - mTIncomeSpecificationRow.Discount)).RoundAs(2);
			}
		}
		else
		{
			mTIncomeSpecificationRow.Discount = 0m;
			mTIncomeSpecificationRow.Price = mTIncomeSpecificationRow.DiscountPrice;
		}
		mTIncomeSpecificationRow.RowID = destList.Count;
		mTIncomeSpecificationRow.VAT = sourceRow.SumVAT > 0m;
		summaVAT += sourceRow.SumVAT;
		totalSumma += sourceRow.TotalSum;
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.ItemCode);
		if (item != null)
		{
			mTIncomeSpecificationRow.UnitCode = sourceRow.UnitCodeFromDirectory;
			mTIncomeSpecificationRow.UnitBrief = sourceRow.UnitBriefFromDirectory;
			mTIncomeSpecificationRow.MTName = item.Caption;
			if (isRetailDoc)
			{
				mTIncomeSpecificationRow.ExtraPercent = ((item.Extra == 0m) ? Settings.ParametersService.Extra() : item.Extra);
				if (vatType != "3")
				{
					mTIncomeSpecificationRow.SalePrice = (mTIncomeSpecificationRow.DiscountPrice + mTIncomeSpecificationRow.DiscountPrice * Settings.ParametersService.VAT() / 100m + mTIncomeSpecificationRow.DiscountPrice * mTIncomeSpecificationRow.ExtraPercent / 100m).RoundAs(2);
				}
				else
				{
					mTIncomeSpecificationRow.SalePrice = (mTIncomeSpecificationRow.DiscountPrice + mTIncomeSpecificationRow.DiscountPrice * mTIncomeSpecificationRow.ExtraPercent / 100m).RoundAs(2);
				}
				mTIncomeSpecificationRow.SaleSumma = PriceHelper.CalculateSum(mTIncomeSpecificationRow.SalePrice, mTIncomeSpecificationRow.Quantity);
			}
		}
		destList.Add(mTIncomeSpecificationRow);
	}

	public static void GroupCreateNewCashOutDoc()
	{
		List<DocumentsJournalDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>();
		decimal num = 0m;
		MTIncome mTIncome = null;
		int num2 = 0;
		string text = "";
		decimal num3 = 0m;
		List<string> partContracts = new List<string>();
		(string, DateOnly?, decimal, string, decimal, decimal, decimal) tuple = ("", null, 0m, "", 0m, 0m, 0m);
		if ((from row in selectedRows
			where row.DocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value
			select row.DocType).Distinct().Any())
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrDifferentDocTypeNotAllowedForDocuments, ArmSoft.SME.Common.Resources.CashOutputOrderVS, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.FullCaption));
		}
		IEnumerable<string> source = selectedRows.Select((DocumentsJournalDataProvider.DataRow row) => row.CurrCode).Distinct();
		if (source.Count() > 1)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrDifferentCurrNotAllowedForDocument, ArmSoft.SME.Common.Resources.CashOutputOrderVS));
		}
		text = source.First();
		partContracts = (from dataRow in selectedRows
			select Regex.Match(dataRow.Body, "Contract".ToUpper() + ":(\\d+)") into match
			where match.Success
			select match.Groups[1].Value).ToList();
		if ((from row in partContracts
			where row != partContracts.First()
			select (row)).Any())
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrDifferentContractTypeNotAllowedForDocument, ArmSoft.SME.Common.Resources.CashOutputOrderVS));
		}
		foreach (DocumentsJournalDataProvider.DataRow item3 in selectedRows)
		{
			if (item3 != null)
			{
				if (num2 <= 0)
				{
					num2 = item3.PartID;
				}
				else if (num2 != item3.PartID)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrDifferentPartNotAllowedForDocument, ArmSoft.SME.Common.Resources.CashOutputOrderVS));
				}
				mTIncome = (MTIncome)Settings.DocumentService.Load(item3.ISN, item3.DocType);
				tuple.Item1 = mTIncome.InvoiceNumber;
				tuple.Item2 = mTIncome.InvoiceDate;
				tuple.Item3 = mTIncome.Summ.Value;
				tuple.Item7 = mTIncome.SummVAT;
				tuple.Item4 = mTIncome.VATType;
				tuple.Item5 = mTIncome.ExchangeRate.Rate;
				tuple.Item6 = mTIncome.ExchangeRate.Base;
				decimal num4 = 0m;
				num4 = tuple.Item3;
				if (text == "AMD")
				{
					num3 += num4;
				}
				else
				{
					num3 += (num4 * tuple.Item5 / tuple.Item6).RoundAs(2);
				}
				num += num4;
			}
		}
		if (mTIncome != null)
		{
			CashOut cashOut = Settings.DocumentService.Create<CashOut>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.CashOut);
			CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(Settings.ParametersService.DefaultCash());
			if (item != null)
			{
				cashOut.CashDesk = item.Code;
				cashOut.CurrCode = mTIncome.CurrCode;
				cashOut.DocDate = Settings.HostingEnvironment.CurrentDate;
			}
			if (num == 0m)
			{
				cashOut.Summ = mTIncome.Summ;
			}
			else
			{
				cashOut.Summ = num;
			}
			cashOut.PartnerContractGroup.PartnerCode = mTIncome.PartnerContractGroup.PartnerCode;
			if (num3 == 0m)
			{
				num3 = ((!(mTIncome.CurrCode == "AMD")) ? (cashOut.Summ.Value * mTIncome.ExchangeRate.Rate / mTIncome.ExchangeRate.Base).RoundAs(2) : cashOut.Summ.Value);
			}
			cashOut.SummNCC = num3;
			if (!string.IsNullOrWhiteSpace(cashOut.PartnerContractGroup.PartnerCode))
			{
				PartnerCacheRow item2 = TradeSettings.TradeCacheService.Partner.GetItem(cashOut.PartnerContractGroup.PartnerCode);
				cashOut.ReceiverName = item2.Caption;
				cashOut.NPassport = item2.Passport;
			}
			cashOut.PartnerContractGroup.ContractCode = partContracts.FirstOrDefault();
			if (item == null && !string.IsNullOrWhiteSpace(text))
			{
				cashOut.CurrCode = text;
			}
			if (cashOut.CurrCode != mTIncome.CurrCode)
			{
				cashOut.CashDesk = "";
				cashOut.CurrCode = "";
				cashOut.SummNCC = cashOut.Summ;
			}
			IPOWindow iPOWindow = Settings.DocumentService.CreateWindow(cashOut);
			if (Settings.HostingEnvironment.CurrentViewPanel != null && Settings.HostingEnvironment.CurrentViewPanel.InnerGrid().DataContext is DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>)
			{
				iPOWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
			}
			iPOWindow.Show();
		}
	}

	public static void GroupCreateNewPayOrderDoc()
	{
		List<DocumentsJournalDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<DocumentsJournalDataProvider.DataRow>();
		decimal num = 0m;
		MTIncome mTIncome = null;
		int num2 = 0;
		string text = "";
		decimal num3 = 0m;
		List<string> partContracts = new List<string>();
		(string, DateOnly?, decimal, string, decimal, decimal, decimal) tuple = ("", null, 0m, "", 0m, 0m, 0m);
		if ((from row in selectedRows
			where row.DocType != ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.Value
			select row.DocType).Distinct().Any())
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrDifferentDocTypeNotAllowedForDocuments, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.FullCaption, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome.FullCaption));
		}
		IEnumerable<string> source = selectedRows.Select((DocumentsJournalDataProvider.DataRow row) => row.CurrCode).Distinct();
		if (source.Count() > 1)
		{
			throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrDifferentCurrNotAllowedForDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.FullCaption));
		}
		text = source.First();
		partContracts = (from dataRow in selectedRows
			select Regex.Match(dataRow.Body, "Contract".ToUpper() + ":(\\d+)") into match
			where match.Success
			select match.Groups[1].Value).ToList();
		if ((from row in partContracts
			where row != partContracts.First()
			select (row)).Any())
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ErrDifferentContractTypeNotAllowedForDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.FullCaption));
		}
		foreach (DocumentsJournalDataProvider.DataRow item2 in selectedRows)
		{
			if (item2 != null)
			{
				if (num2 <= 0)
				{
					num2 = item2.PartID;
				}
				else if (num2 != item2.PartID)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrDifferentPartNotAllowedForDocument, ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.FullCaption));
				}
				mTIncome = Settings.DocumentService.Load<MTIncome>(item2.ISN, item2.DocType, withoutAccessCheck: false, withoutGrids: true);
				tuple.Item1 = mTIncome.InvoiceNumber;
				tuple.Item2 = mTIncome.InvoiceDate;
				tuple.Item3 = mTIncome.Summ.Value;
				tuple.Item7 = mTIncome.SummVAT;
				tuple.Item4 = mTIncome.VATType;
				tuple.Item5 = mTIncome.ExchangeRate.Rate;
				tuple.Item6 = mTIncome.ExchangeRate.Base;
				decimal num4 = 0m;
				num4 = tuple.Item3;
				if (text == "AMD")
				{
					num3 += num4;
				}
				else
				{
					num3 += (num4 * tuple.Item5 / tuple.Item6).RoundAs(2);
				}
				num += num4;
			}
		}
		if (mTIncome == null)
		{
			return;
		}
		PayOrder payOrder = Settings.DocumentService.Create<PayOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd.Value);
		AccountInBankCacheRow defaultSettlementAccount = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount();
		if (defaultSettlementAccount != null)
		{
			payOrder.PayerSettlAcc = defaultSettlementAccount.SettlAccount;
			payOrder.CurrCode = mTIncome.CurrCode;
			payOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
		}
		if (num == 0m)
		{
			payOrder.Summ = mTIncome.Summ;
		}
		else
		{
			payOrder.Summ = num;
		}
		payOrder.PartnerContractGroup.PartnerCode = mTIncome.PartnerContractGroup.PartnerCode;
		if (num3 == 0m)
		{
			num3 = ((!(mTIncome.CurrCode == "AMD")) ? (payOrder.Summ.Value * mTIncome.ExchangeRate.Rate / mTIncome.ExchangeRate.Base).RoundAs(2) : payOrder.Summ.Value);
		}
		payOrder.SummNCC = num3;
		if (!string.IsNullOrWhiteSpace(payOrder.PartnerContractGroup.PartnerCode))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(payOrder.PartnerContractGroup.PartnerCode);
			if (item != null)
			{
				payOrder.ReceiverName = item.Caption;
				payOrder.ReceiverSettlAcc = item.SettlAcc;
				payOrder.ReceiverTaxCode = item.TaxCode;
				payOrder.PaymentAim = item.MainAIM;
			}
		}
		payOrder.PartnerContractGroup.ContractCode = partContracts.FirstOrDefault();
		if (defaultSettlementAccount == null && !string.IsNullOrWhiteSpace(text))
		{
			payOrder.CurrCode = text;
		}
		if (payOrder.CurrCode != mTIncome.CurrCode)
		{
			payOrder.PayerSettlAcc = "";
			payOrder.CurrCode = "";
			payOrder.SummNCC = payOrder.Summ;
		}
		IPOWindow iPOWindow = Settings.DocumentService.CreateWindow(payOrder);
		if (Settings.HostingEnvironment.CurrentViewPanel != null && Settings.HostingEnvironment.CurrentViewPanel.InnerGrid().DataContext is DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>)
		{
			iPOWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
		}
		iPOWindow.Show();
	}

	public static void CreateNewMTIncomeDoc(DateOnly docDate, bool isFromTurnOver = false)
	{
		MTIncome mTIncome = Settings.DocumentService.Create<MTIncome>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncome);
		int rowID = 0;
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		try
		{
			mTIncome.DocDate = docDate;
			rowID = (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny() ? FillMTIncomeDocList(mTIncome, rowID, isFromTurnOver) : FillMTIncomeDoc(mTIncome, rowID, isFromTurnOver));
			mTIncome.MaxRowID = rowID;
			mTIncome.Chief = Settings.ParametersService.ChiefAccountantName();
			mTIncome.PriceType = Settings.ParametersService.DefaultBuyingPrice();
			MTIncomeWindow mTIncomeWindow = new MTIncomeWindow();
			mTIncomeWindow.PO = mTIncome;
			mTIncomeWindow.Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static int FillMTIncomeDoc(MTIncomeBase doc, int rowID, bool isFromTurnOver = false)
	{
		int num = rowID;
		if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsRemains || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTurnOver || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTreeViewRemains)
		{
			GoodsRemainsDataProvider.DataRow availabilityRowFromAvailabilityOrTurnOver = GetAvailabilityRowFromAvailabilityOrTurnOver(isFromTurnOver);
			if (availabilityRowFromAvailabilityOrTurnOver != null)
			{
				num = FillMTincome(doc, num, availabilityRowFromAvailabilityOrTurnOver.Storage, availabilityRowFromAvailabilityOrTurnOver);
			}
		}
		else if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is SalesAnalysis)
		{
			SalesAnalysisDataProvider.DataRow dataRow = (SalesAnalysisDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				num = FillMTincome(doc, num, dataRow.Storage, dataRow);
			}
		}
		return num;
	}

	private static int FillMTIncomeDocList(MTIncomeBase doc, int rowID, bool isFromTurnOver = false)
	{
		int num = rowID;
		if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsRemains || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTurnOver || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTreeViewRemains)
		{
			List<GoodsRemainsDataProvider.DataRow> availabilityRowsFromAvailabilityOrTurnOver = GetAvailabilityRowsFromAvailabilityOrTurnOver(isFromTurnOver);
			CheckIsSameStorages(availabilityRowsFromAvailabilityOrTurnOver);
			num = FillMTincome(doc, num, availabilityRowsFromAvailabilityOrTurnOver.First().Storage, null, availabilityRowsFromAvailabilityOrTurnOver);
		}
		else if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is SalesAnalysis)
		{
			List<SalesAnalysisDataProvider.DataRow> selectedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<SalesAnalysisDataProvider.DataRow>();
			CheckIsSameStorages(selectedRows);
			num = FillMTincome(doc, num, selectedRows.First().Storage, null, selectedRows);
		}
		return num;
	}

	private static GoodsRemainsDataProvider.DataRow GetAvailabilityRowFromAvailabilityOrTurnOver(bool isFromTurnOver, bool fillQtyOut = true, bool detailByOperations = false)
	{
		if (isFromTurnOver)
		{
			return ConvertTurnOverRowToAvailabilityRow((GoodsTurnOverDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow(), fillQtyOut, detailByOperations);
		}
		return (GoodsRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
	}

	private static List<GoodsRemainsDataProvider.DataRow> GetAvailabilityRowsFromAvailabilityOrTurnOver(bool isFromTurnOver, bool fillQtyOut = true, bool detailByOperations = false)
	{
		List<GoodsRemainsDataProvider.DataRow> list = new List<GoodsRemainsDataProvider.DataRow>();
		if (isFromTurnOver)
		{
			foreach (GoodsTurnOverDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsTurnOverDataProvider.DataRow>())
			{
				list.Add(ConvertTurnOverRowToAvailabilityRow(selectedRow, fillQtyOut, detailByOperations));
			}
		}
		else
		{
			list = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>();
		}
		return list;
	}

	private static void GetAvailabilityFromAvailabilityOrTurnOver(bool isFromTurnOver, ref GoodsRemainsDataProvider.DataRow availabilityRow, ref List<GoodsRemainsDataProvider.DataRow> selectedRows, bool fillQtyOut = false, bool detailByOperations = false)
	{
		if (!Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			availabilityRow = GetAvailabilityRowFromAvailabilityOrTurnOver(isFromTurnOver, fillQtyOut, detailByOperations);
		}
		else
		{
			selectedRows = GetAvailabilityRowsFromAvailabilityOrTurnOver(isFromTurnOver, fillQtyOut, detailByOperations);
		}
	}

	private static int FillMTincome(MTIncomeBase doc, int rowID, string storageInc, SalesAnalysisDataProvider.DataRow availabilityRow = null, List<SalesAnalysisDataProvider.DataRow> selectedRows = null)
	{
		int num = rowID;
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storageInc);
		if (item != null && item.Retail)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Common.Resources.DocMTIncome));
		}
		if (!string.IsNullOrWhiteSpace(storageInc))
		{
			CheckStorageAvailable(storageInc);
			doc.Storage = storageInc;
		}
		if (selectedRows != null)
		{
			foreach (SalesAnalysisDataProvider.DataRow selectedRow in selectedRows)
			{
				num = AddRowInMTIncomeList(doc, selectedRow, num);
			}
		}
		else if (availabilityRow != null)
		{
			num = AddRowInMTIncomeList(doc, availabilityRow, num);
		}
		return num;
	}

	private static int FillMTincome(MTIncomeBase doc, int rowID, string storageInc, GoodsRemainsDataProvider.DataRow availabilityRow = null, List<GoodsRemainsDataProvider.DataRow> selectedRows = null)
	{
		int num = rowID;
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storageInc);
		if (item != null && item.Retail)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Common.Resources.DocMTIncome));
		}
		if (!string.IsNullOrWhiteSpace(storageInc))
		{
			CheckStorageAvailable(storageInc);
			doc.Storage = storageInc;
		}
		if (selectedRows != null)
		{
			foreach (GoodsRemainsDataProvider.DataRow selectedRow in selectedRows)
			{
				num = AddRowInMTIncomeList(doc, selectedRow, num);
			}
		}
		else if (availabilityRow != null)
		{
			num = AddRowInMTIncomeList(doc, availabilityRow, num);
		}
		return num;
	}

	private static int AddRowInMTIncomeList(MTIncomeBase doc, SalesAnalysisDataProvider.DataRow sourceRow, int rowID)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.GoodCode);
		if (item != null)
		{
			MTIncomeBase.MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeBase.MTIncomeSpecificationRow();
			mTIncomeSpecificationRow.RowID = rowID;
			mTIncomeSpecificationRow.MTID = sourceRow.GoodID;
			mTIncomeSpecificationRow.MTCode = item.MTCode;
			mTIncomeSpecificationRow.VAT = item.VAT;
			mTIncomeSpecificationRow.MTName = item.Caption;
			mTIncomeSpecificationRow.Quantity = sourceRow.Quantity;
			mTIncomeSpecificationRow.UnitCode = sourceRow.UnitCode;
			mTIncomeSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTIncomeSpecificationRow.Summa = 0m;
			mTIncomeSpecificationRow.Country = item.Country;
			mTIncomeSpecificationRow.Producer = item.Producer;
			PropertyCacheRow item2 = TradeSettings.TradeCacheService.Property.GetItem(item.Property1);
			if (item2 != null)
			{
				mTIncomeSpecificationRow.Property1Caption = item2.Caption;
				mTIncomeSpecificationRow.Property1 = item2.Code;
			}
			item2 = TradeSettings.TradeCacheService.Property.GetItem(item.Property2);
			if (item2 != null)
			{
				mTIncomeSpecificationRow.Property2Caption = item2.Caption;
				mTIncomeSpecificationRow.Property2 = item2.Code;
			}
			doc.MTList.Add(mTIncomeSpecificationRow);
		}
		return ++rowID;
	}

	private static int AddRowInMTIncomeList(MTIncomeBase doc, GoodsRemainsDataProvider.DataRow sourceRow, int rowID)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			MTIncomeBase.MTIncomeSpecificationRow mTIncomeSpecificationRow = new MTIncomeBase.MTIncomeSpecificationRow();
			mTIncomeSpecificationRow.RowID = rowID;
			mTIncomeSpecificationRow.MTID = sourceRow.MTID;
			mTIncomeSpecificationRow.MTCode = item.MTCode;
			mTIncomeSpecificationRow.VAT = item.VAT;
			mTIncomeSpecificationRow.MTName = item.Caption;
			mTIncomeSpecificationRow.Quantity = ((sourceRow.Quantity < 0m) ? (-sourceRow.Quantity) : 0m);
			mTIncomeSpecificationRow.UnitCode = sourceRow.UnitCode;
			mTIncomeSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTIncomeSpecificationRow.Summa = 0m;
			mTIncomeSpecificationRow.Country = item.Country;
			mTIncomeSpecificationRow.Producer = item.Producer;
			PropertyCacheRow item2 = TradeSettings.TradeCacheService.Property.GetItem(item.Property1);
			if (item2 != null)
			{
				mTIncomeSpecificationRow.Property1Caption = item2.Caption;
				mTIncomeSpecificationRow.Property1 = item2.Code;
			}
			item2 = TradeSettings.TradeCacheService.Property.GetItem(item.Property2);
			if (item2 != null)
			{
				mTIncomeSpecificationRow.Property2Caption = item2.Caption;
				mTIncomeSpecificationRow.Property2 = item2.Code;
			}
			doc.MTList.Add(mTIncomeSpecificationRow);
		}
		return ++rowID;
	}

	public static void CreateNewMTIncomeRetailDoc(DateOnly asOfDate, bool isFromRetail = true)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTIncomeRetail mTIncomeRetail = Settings.DocumentService.Create<MTIncomeRetail>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTIncomeRetail);
		int num = 0;
		string text = "";
		try
		{
			mTIncomeRetail.DocDate = asOfDate;
			if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsRemains || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTurnOver || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsRetailAvailability || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsRetailTurnOver || Settings.HostingEnvironment.CurrentViewPanel.DataContext is GoodsTreeViewRemains)
			{
				List<GoodsRemainsDataProvider.DataRow> list = new List<GoodsRemainsDataProvider.DataRow>();
				if (isFromRetail)
				{
					foreach (GoodsRetailAvailabilityDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRetailAvailabilityDataProvider.DataRow>())
					{
						list.Add(ConvertAvailabilityRetRowToAvailabilityRow(selectedOrFocusedRow));
					}
				}
				else
				{
					list = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<GoodsRemainsDataProvider.DataRow>();
				}
				CheckIsSameStorages(list);
				text = list[0].Storage;
				foreach (GoodsRemainsDataProvider.DataRow item in list)
				{
					num = AddRowInMTIncomeList(mTIncomeRetail, item, num);
				}
			}
			else if (Settings.HostingEnvironment.CurrentViewPanel.DataContext is SalesAnalysis)
			{
				List<SalesAnalysisDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<SalesAnalysisDataProvider.DataRow>();
				CheckIsSameStorages(selectedOrFocusedRows);
				text = selectedOrFocusedRows[0].Storage;
				foreach (SalesAnalysisDataProvider.DataRow item2 in selectedOrFocusedRows)
				{
					num = AddRowInMTIncomeList(mTIncomeRetail, item2, num);
				}
			}
			if (!string.IsNullOrWhiteSpace(text) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				mTIncomeRetail.Storage = text;
			}
			mTIncomeRetail.MaxRowID = num;
			mTIncomeRetail.Summ = 0m;
			mTIncomeRetail.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTIncomeRetail).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static GoodsRemainsDataProvider.DataRow ConvertAvailabilityRetRowToAvailabilityRow(GoodsRetailAvailabilityDataProvider.DataRow row)
	{
		return new GoodsRemainsDataProvider.DataRow
		{
			MTID = row.GoodID,
			MTCode = row.GoodCode,
			Storage = row.Storage,
			Quantity = row.Quantity,
			UnitCode = row.Unit,
			UnitBrief = row.UnitBrief
		};
	}

	public static void CreateNewMTReturnToSupplierDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTReturnToSupplier mTReturnToSupplier = Settings.DocumentService.Create<MTReturnToSupplier>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTReturnToSupplier);
		int rowID = 0;
		try
		{
			mTReturnToSupplier.DocDate = docDate;
			mTReturnToSupplier.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			FillMTReturnToSupplierStorage(mTReturnToSupplier);
			FillMTReturnToSupplierPartnerAndContract(mTReturnToSupplier);
			FillMTReturnToSupplierDocList(mTReturnToSupplier, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTReturnToSupplier.MaxRowID = rowID;
			mTReturnToSupplier.SummVAT = mTReturnToSupplier.MTList.Where((MTReturnToSupplier.MTRetToSupSpecificationRow row) => row.VAT).Sum((MTReturnToSupplier.MTRetToSupSpecificationRow row) => row.Summa * Settings.ParametersService.VAT() / 100m).RoundAs(2);
			mTReturnToSupplier.Summ = (mTReturnToSupplier.MTList.Sum((MTReturnToSupplier.MTRetToSupSpecificationRow row) => row.Summa) + mTReturnToSupplier.SummVAT).RoundAs(2);
			mTReturnToSupplier.SupplierName = Settings.ParametersService.CompanyName();
			mTReturnToSupplier.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			mTReturnToSupplier.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
			mTReturnToSupplier.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: false);
			mTReturnToSupplier.SupplierTaxCode = Settings.ParametersService.TaxCode();
			mTReturnToSupplier.SupplierMngPost = Settings.ParametersService.ManagerPost();
			mTReturnToSupplier.SupplierMngName = Settings.ParametersService.ManagerName();
			mTReturnToSupplier.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			mTReturnToSupplier.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTReturnToSupplier).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTReturnToSupplierStorage(MTReturnToSupplier doc)
	{
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
			{
				doc.Storage = text;
			}
		}
	}

	private static void FillMTReturnToSupplierPartnerAndContract(MTReturnToSupplier doc)
	{
		string text = Settings.ParametersService.DefaultSupplier();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
		if (item != null && item.Customer)
		{
			doc.PartnerContractGroup.PartnerCode = text;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			if (defaultContract != null)
			{
				doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
			}
		}
	}

	private static void FillMTReturnToSupplierDocList(MTReturnToSupplier doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			IsStoragesOnlyRetailOrWholesale(selectedRows, string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Trade.Common.Resources.DocInvoice));
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowInMTReturnToSupplierList(doc, fillAvailQty, item, ref rowID, showByDifferentParties);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckStorageAvailable(availabilityRow.Storage);
			AddRowInMTReturnToSupplierList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties);
		}
	}

	private static void AddRowInMTReturnToSupplierList(MTReturnToSupplier doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow sourceRow, ref int rowID, bool showByDifferentParties)
	{
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(sourceRow, fillAvailQty, rowID);
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			MTReturnToSupplier.MTRetToSupSpecificationRow mTRetToSupSpecificationRow = new MTReturnToSupplier.MTRetToSupSpecificationRow();
			mTRetToSupSpecificationRow.RowID = rowID;
			mTRetToSupSpecificationRow.Storage = sourceRow.Storage;
			mTRetToSupSpecificationRow.MTID = sourceRow.MTID;
			mTRetToSupSpecificationRow.MTCode = item.MTCode;
			mTRetToSupSpecificationRow.MTName = item.Caption;
			mTRetToSupSpecificationRow.IsPartySelected = showByDifferentParties && sourceRow.Quantity > 0m;
			mTRetToSupSpecificationRow.Quantity = GetQuantity(sourceRow.Quantity, fillAvailQty);
			mTRetToSupSpecificationRow.UnitCode = sourceRow.UnitCode;
			mTRetToSupSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTRetToSupSpecificationRow.VAT = item.VAT;
			if (mTRetToSupSpecificationRow.Quantity != 0m)
			{
				mTRetToSupSpecificationRow.Price = Convert.ToDecimal(sourceRow.Cost);
			}
			else
			{
				mTRetToSupSpecificationRow.Price = 0m;
			}
			mTRetToSupSpecificationRow.DiscountPrice = mTRetToSupSpecificationRow.Price;
			if (fillAvailQty)
			{
				mTRetToSupSpecificationRow.Summa = sourceRow.CostSumm;
			}
			else
			{
				mTRetToSupSpecificationRow.Summa = mTRetToSupSpecificationRow.Price;
			}
			FillMTReturnToSupplierSalePrice(mTRetToSupSpecificationRow, doc.DocDate);
			doc.MTList.Add(mTRetToSupSpecificationRow);
			rowID++;
		}
	}

	private static void FillMTReturnToSupplierSalePrice(MTReturnToSupplier.MTRetToSupSpecificationRow specificationRow, DateOnly docDate)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(specificationRow.Storage);
		if (item == null || !item.Retail)
		{
			return;
		}
		MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(specificationRow.MTID, specificationRow.UnitCode);
		if (qntUnit != null)
		{
			MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
			{
				MTCode = specificationRow.MTCode,
				UnitCoef = qntUnit.Coef,
				Storage = specificationRow.Storage,
				Date = docDate
			};
			List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
			if (rows.Count > 0)
			{
				specificationRow.SalePrice = rows[0].Price;
				specificationRow.SaleSumma = PriceHelper.CalculateSum(specificationRow.SalePrice, specificationRow.Quantity);
			}
		}
	}

	public static void CreateNewMTInvoiceDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTInvoice mTInvoice = Settings.DocumentService.Create<MTInvoice>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTInvoice.Value);
		int rowID = 0;
		try
		{
			mTInvoice.DocDate = docDate;
			mTInvoice.VATType = Settings.ParametersService.VATType();
			FillMTinvoiceStorageAndBusinessAddress(mTInvoice);
			FillMTInvoicePartnerContractAndDiscount(mTInvoice);
			mTInvoice.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			mTInvoice.PriceListType = Settings.ParametersService.DefaultSalePrice();
			FillMTinvoiceDocList(mTInvoice, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTInvoice.MaxRowID = rowID;
			mTInvoice.SummVAT = mTInvoice.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.VAT.Value).Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa * Settings.ParametersService.VAT() / 100m).RoundAs(2);
			mTInvoice.Summ = (mTInvoice.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa) + mTInvoice.SummVAT).RoundAs(2);
			mTInvoice.SupplierName = Settings.ParametersService.CompanyName();
			mTInvoice.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			mTInvoice.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			mTInvoice.SupplierTaxCode = Settings.ParametersService.TaxCode();
			mTInvoice.SupplierMngPost = Settings.ParametersService.ManagerPost();
			mTInvoice.SupplierMngName = Settings.ParametersService.ManagerName();
			mTInvoice.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			mTInvoice.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTInvoice).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void IsStoragesOnlyRetailOrWholesale(List<GoodsRemainsDataProvider.DataRow> rows, string message, bool isRetail = false)
	{
		if (rows.Any((GoodsRemainsDataProvider.DataRow row) => isRetail ^ (TradeSettings.TradeCacheService.Storage.GetItem(row.Storage)?.Retail ?? false)))
		{
			throw new ASException(message);
		}
	}

	private static void FillMTinvoiceDocList(MTInvoice doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		bool envFeePayer = Settings.ParametersService.EnvironmentalFeePayer();
		bool useDiscount = Settings.ParametersService.DiscountUse();
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			IsStoragesOnlyRetailOrWholesale(selectedRows, string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Trade.Common.Resources.DocInvoice));
			{
				foreach (GoodsRemainsDataProvider.DataRow item2 in selectedRows)
				{
					CheckStorageAvailable(item2.Storage);
					AddRowInMTinvoiceList(doc, fillAvailQty, item2, ref rowID, showByDifferentParties, useDiscount, envFeePayer);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(availabilityRow.Storage);
			if (item != null && item.Retail)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Trade.Common.Resources.DocInvoice));
			}
			CheckStorageAvailable(availabilityRow.Storage);
			AddRowInMTinvoiceList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties, useDiscount, envFeePayer);
		}
	}

	private static void AddRowInMTinvoiceList(MTInvoice doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow row, ref int rowID, bool showByDifferentParties, bool useDiscount, bool envFeePayer)
	{
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(row, fillAvailQty, rowID);
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.MTCode);
		if (item == null)
		{
			return;
		}
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow();
		mTInvoiceSpecificationRow.RowID = rowID;
		mTInvoiceSpecificationRow.ItemType = item.Type;
		mTInvoiceSpecificationRow.Storage = row.Storage;
		mTInvoiceSpecificationRow.ItemID = row.MTID;
		mTInvoiceSpecificationRow.ItemCode = row.MTCode;
		mTInvoiceSpecificationRow.ItemName = item.Caption;
		mTInvoiceSpecificationRow.IsPartySelected = showByDifferentParties && row.Quantity > 0m;
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow2 = mTInvoiceSpecificationRow;
		decimal quantity;
		if (fillAvailQty)
		{
			quantity = ((row.Quantity > 0m) ? row.Quantity : 0m);
		}
		else
		{
			quantity = ((row.Quantity > 0m) ? 1m : 0m);
		}
		mTInvoiceSpecificationRow2.Quantity = quantity;
		if (useDiscount)
		{
			mTInvoiceSpecificationRow.Discount = doc.Discount;
			if (mTInvoiceSpecificationRow.Discount == 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
			else if (item.Discount != 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
		}
		mTInvoiceSpecificationRow.UnitCode = row.UnitCode;
		mTInvoiceSpecificationRow.UnitBrief = row.UnitBrief;
		mTInvoiceSpecificationRow.VAT = item.VAT;
		mTInvoiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(doc.PriceListType, doc.VATType, row.MTID, row.UnitCode, doc.DocDate, "AMD");
		mTInvoiceSpecificationRow.DiscountPrice = PriceHelper.CalculateDiscountPrice(mTInvoiceSpecificationRow.Price, mTInvoiceSpecificationRow.Discount);
		mTInvoiceSpecificationRow.Summa = PriceHelper.CalculateSum(mTInvoiceSpecificationRow.DiscountPrice, mTInvoiceSpecificationRow.Quantity);
		if (envFeePayer)
		{
			mTInvoiceSpecificationRow.EnvFeePercent = item.EcoFeeRate;
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, doc.VATType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		doc.MTList.Add(mTInvoiceSpecificationRow);
		rowID++;
	}

	private static void FillMTinvoiceStorageAndBusinessAddress(MTInvoice doc)
	{
		string text = Settings.ParametersService.DefaultStorage();
		doc.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(text);
		if ((item == null || !item.Retail) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(text))
		{
			doc.Storage = text;
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(text);
			if (item2 != null && !string.IsNullOrWhiteSpace(item2.Address))
			{
				doc.SupplierBusinessAddress = item2.Address;
			}
		}
	}

	private static void FillMTInvoicePartnerContractAndDiscount(MTInvoice doc)
	{
		string text = Settings.ParametersService.DefaultCustomer();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
		if (item != null && item.Customer)
		{
			doc.PartnerContractGroup.PartnerCode = text;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			if (defaultContract != null)
			{
				doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
				doc.Discount = defaultContract.Percent;
			}
		}
	}

	public static void CreateNewMTBillDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		try
		{
			MTBill mTBill = Settings.DocumentService.Create<MTBill>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTBill);
			int rowID = 0;
			mTBill.DocDate = docDate;
			mTBill.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			FillMTBillPartnerContractAndDiscount(mTBill);
			FillMTBillDocList(mTBill, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTBill.MaxRowID = rowID;
			Settings.ParametersService.VAT();
			mTBill.SummVAT = mTBill.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.VAT.Value).Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa * Settings.ParametersService.VAT() / 100m).RoundAs(2);
			mTBill.Summ = mTBill.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa).RoundAs(2);
			mTBill.SummaChange = mTBill.Summ.Value;
			Settings.DocumentService.CreateWindow(mTBill).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTBillDocList(MTBill doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		bool paramAllowChangeStorageInBill = Settings.ParametersService.AllowChangeStorageInBill();
		bool flag = Settings.ParametersService.DiscountUse();
		string text = Settings.ParametersService.DefaultMTCheckPriceType();
		string priceListTypeRoundDgr = string.Empty;
		string text2 = Settings.ParametersService.DefaultMTCheckDiscountPriceType();
		string discountPriceTypeRoundDegree = string.Empty;
		string priceListRowVATType = string.Empty;
		string priceListRowCurr = string.Empty;
		decimal discountPriceListCurrRate = 1m;
		decimal discountPriceListCurrBase = 1m;
		if (string.IsNullOrWhiteSpace(text))
		{
			priceListTypeRoundDgr = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(text);
		}
		if (flag && !string.IsNullOrWhiteSpace(text2))
		{
			discountPriceTypeRoundDegree = TradeSettings.CreateTradeClient().CommonServices.GetPriceTypeRoungDgr(text2);
			PriceHelper.GetPriceListRowVATType(text2, out priceListRowVATType, out priceListRowCurr);
			discountPriceListCurrRate = 1m;
			discountPriceListCurrBase = 1m;
			if (priceListRowCurr != doc.CurrCode && priceListRowCurr != "AMD")
			{
				ExchangeRate exchangeRates = TradeSettings.CreateTradeClient().CommonServices.GetExchangeRates(priceListRowCurr, doc.DocDate);
				discountPriceListCurrRate = exchangeRates.Rate;
				discountPriceListCurrBase = exchangeRates.Base;
			}
		}
		if (selectedRows != null)
		{
			CheckingMTBillDifferentTypeOfStorages(selectedRows, paramAllowChangeStorageInBill);
			FillMTBillStorage(doc, selectedRows.First().Storage, paramAllowChangeStorageInBill);
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					AddRowInMTBillList(doc, fillAvailQty, item, ref rowID, showByDifferentParties, flag, text, priceListTypeRoundDgr, text2, discountPriceTypeRoundDegree, priceListRowVATType, priceListRowCurr, discountPriceListCurrRate, discountPriceListCurrBase);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			FillMTBillStorage(doc, availabilityRow.Storage, paramAllowChangeStorageInBill);
			AddRowInMTBillList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties, flag, text, priceListTypeRoundDgr, text2, discountPriceTypeRoundDegree, priceListRowVATType, priceListRowCurr, discountPriceListCurrRate, discountPriceListCurrBase);
		}
	}

	private static void FillMTBillStorage(MTBill doc, string storage, bool paramAllowChangeStorageInBill)
	{
		if (string.IsNullOrWhiteSpace(storage))
		{
			return;
		}
		if (!paramAllowChangeStorageInBill)
		{
			if (!(storage == Settings.ParametersService.DefaultStorage()))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, storage));
			}
			doc.Storage = storage;
		}
		else if (TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
		{
			doc.Storage = storage;
		}
	}

	private static void CheckingMTBillDifferentTypeOfStorages(List<GoodsRemainsDataProvider.DataRow> selectedRows, bool paramAllowChangeStorageInBill)
	{
		IEnumerable<string> enumerable = selectedRows.Select((GoodsRemainsDataProvider.DataRow x) => x.Storage).Distinct();
		if (enumerable.Count() <= 1)
		{
			return;
		}
		if (!(Settings.ParametersService.MTBillAllowDifferentStorages() & paramAllowChangeStorageInBill))
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
		}
		foreach (string item in enumerable)
		{
			if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(item))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, item));
			}
		}
		CheckIsSameTypeOfStorages(selectedRows);
	}

	private static void AddRowInMTBillList(MTBill doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow row, ref int rowID, bool showByDifferentParties, bool useDiscount, string priceListType, string priceListTypeRoundDgr, string discountPriceListType, string discountPriceTypeRoundDegree, string discountPriceListVATType, string discountPriceListCurr, decimal discountPriceListCurrRate, decimal discountPriceListCurrBase)
	{
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(row, fillAvailQty, rowID);
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		MTInvoiceBase.MTInvoiceSpecificationRow mTBillCurrentMTListRow = GetMTBillCurrentMTListRow(doc, row, fillAvailQty, rowID, priceListType, priceListTypeRoundDgr, discountPriceListType, discountPriceTypeRoundDegree, discountPriceListVATType, discountPriceListCurr, discountPriceListCurrRate, discountPriceListCurrBase, showByDifferentParties, useDiscount);
		if (mTBillCurrentMTListRow != null)
		{
			doc.MTList.Add(mTBillCurrentMTListRow);
			rowID++;
		}
	}

	private static void FillMTBillPartnerContractAndDiscount(MTBill doc)
	{
		string text = Settings.ParametersService.DefaultCustomer();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
		if (item != null && item.Customer)
		{
			doc.PartnerContractGroup.PartnerCode = text;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			if (defaultContract != null)
			{
				doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
				doc.Discount = defaultContract.Percent;
			}
		}
	}

	public static void CreateNewMTInvoiceRetailDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTInvoiceRetail mTInvoiceRetail = Settings.DocumentService.Create<MTInvoiceRetail>(21);
		int rowID = 0;
		try
		{
			mTInvoiceRetail.DocDate = docDate;
			mTInvoiceRetail.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			FillMTInvoiceRetailContractAndDiscount(mTInvoiceRetail);
			FillMTInvoiceRetailDocList(mTInvoiceRetail, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTInvoiceRetail.MaxRowID = rowID;
			Settings.ParametersService.VAT();
			mTInvoiceRetail.SummVAT = mTInvoiceRetail.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.VAT.Value).Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa * Settings.ParametersService.VAT() / 100m).RoundAs(2);
			mTInvoiceRetail.Summ = (mTInvoiceRetail.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa) + mTInvoiceRetail.SummVAT).RoundAs(2);
			mTInvoiceRetail.SupplierName = Settings.ParametersService.CompanyName();
			mTInvoiceRetail.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			mTInvoiceRetail.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			mTInvoiceRetail.SupplierTaxCode = Settings.ParametersService.TaxCode();
			mTInvoiceRetail.SupplierMngPost = Settings.ParametersService.ManagerPost();
			mTInvoiceRetail.SupplierMngName = Settings.ParametersService.ManagerName();
			mTInvoiceRetail.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			mTInvoiceRetail.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTInvoiceRetail).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTInvoiceRetailDocList(MTInvoiceRetail doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		bool useDiscount = Settings.ParametersService.DiscountUse();
		string priceListType = Settings.ParametersService.DefaultSalePrice();
		bool flag = false;
		flag = Settings.ParametersService.EnvironmentalFeePayer();
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			CheckIsSameStorages(selectedRows);
			CheckAndFillMTInvoiceRetailStorage(doc, selectedRows.First().Storage);
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowInMTInvocieRetailList(doc, fillAvailQty, item, ref rowID, showByDifferentParties, useDiscount, priceListType, flag);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckAndFillMTInvoiceRetailStorage(doc, availabilityRow.Storage);
			CheckStorageAvailable(availabilityRow.Storage);
			AddRowInMTInvocieRetailList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties, useDiscount, priceListType, flag);
		}
	}

	private static void CheckAndFillMTInvoiceRetailStorage(MTInvoiceRetail doc, string storage)
	{
		doc.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
		if (item == null || !item.Retail)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForRetailStorage, ArmSoft.SME.Common.Resources.DocInvoiceRetail));
		}
		if (!string.IsNullOrWhiteSpace(storage) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
		{
			doc.Storage = storage;
		}
		StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(storage);
		if (item2 != null && !string.IsNullOrWhiteSpace(item2.Address))
		{
			doc.SupplierBusinessAddress = item2.Address;
		}
	}

	private static void FillMTInvoiceRetailContractAndDiscount(MTInvoiceRetail doc)
	{
		string text = Settings.ParametersService.DefaultCustomer();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
		if (item != null && item.Customer)
		{
			doc.PartnerContractGroup.PartnerCode = text;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			if (defaultContract != null)
			{
				doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
				doc.Discount = defaultContract.Percent;
			}
		}
	}

	private static void AddRowInMTInvocieRetailList(MTInvoiceRetail doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow row, ref int rowId, bool showByDifferentParties, bool useDiscount, string priceListType, bool envFeePayer)
	{
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(row, fillAvailQty, rowId);
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(row.MTCode);
		if (item == null)
		{
			return;
		}
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow
		{
			RowID = rowId,
			ItemType = item.Type,
			Storage = row.Storage,
			ItemID = row.MTID,
			ItemCode = row.MTCode,
			ItemName = item.Caption,
			IsPartySelected = (showByDifferentParties && row.Quantity > 0m),
			Quantity = ((!fillAvailQty) ? ((row.Quantity > 0m) ? 1m : 0m) : ((row.Quantity > 0m) ? row.Quantity : 0m))
		};
		if (useDiscount)
		{
			mTInvoiceSpecificationRow.Discount = doc.Discount;
			if (mTInvoiceSpecificationRow.Discount == 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
			else if (item.Discount != 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
		}
		mTInvoiceSpecificationRow.UnitCode = row.UnitCode;
		mTInvoiceSpecificationRow.UnitBrief = row.UnitBrief;
		mTInvoiceSpecificationRow.VAT = item.VAT;
		mTInvoiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(priceListType, doc.VATType, row.MTID, row.UnitCode, doc.DocDate, "AMD");
		mTInvoiceSpecificationRow.DiscountPrice = PriceHelper.CalculateDiscountPrice(mTInvoiceSpecificationRow.Price, mTInvoiceSpecificationRow.Discount);
		mTInvoiceSpecificationRow.Summa = PriceHelper.CalculateSum(mTInvoiceSpecificationRow.DiscountPrice, mTInvoiceSpecificationRow.Quantity);
		if (envFeePayer)
		{
			mTInvoiceSpecificationRow.EnvFeePercent = item.EcoFeeRate;
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, doc.VATType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		doc.MTList.Add(mTInvoiceSpecificationRow);
		rowId++;
	}

	public static void CreateNewMTExpenseDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTExpense mTExpense = Settings.DocumentService.Create<MTExpense>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTExpense);
		int rowID = 0;
		try
		{
			mTExpense.DocDate = docDate;
			FillMTExpenseDocList(mTExpense, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTExpense.MaxRowID = rowID;
			mTExpense.Summ = mTExpense.MTList.Sum((MTExpense.MTExpenseSpecificationRow row) => row.Summa).RoundAs(2);
			mTExpense.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTExpense).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTExpenseDocList(MTExpense doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			CheckIsSameStorages(selectedRows);
			FillMTExpenseSotrage(doc, selectedRows.First().Storage);
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowInMTExpenseList(doc, fillAvailQty, item, ref rowID, showByDifferentParties);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckStorageAvailable(availabilityRow.Storage);
			FillMTExpenseSotrage(doc, availabilityRow.Storage);
			AddRowInMTExpenseList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties);
		}
	}

	private static void FillMTExpenseSotrage(MTExpense doc, string storage)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
		if (item != null && item.Retail)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Common.Resources.DocMTExpense));
		}
		doc.Storage = storage;
	}

	private static void AddRowInMTExpenseList(MTExpense doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow sourceRow, ref int rowId, bool showByDifferentParties)
	{
		decimal summa = 0m;
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(sourceRow, fillAvailQty, rowId);
			summa = currentPartiesListRow.ExpSumma;
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			MTExpense.MTExpenseSpecificationRow mTExpenseSpecificationRow = new MTExpense.MTExpenseSpecificationRow();
			mTExpenseSpecificationRow.RowID = rowId;
			mTExpenseSpecificationRow.MTID = sourceRow.MTID;
			mTExpenseSpecificationRow.MTCode = item.MTCode;
			mTExpenseSpecificationRow.MTName = item.Caption;
			mTExpenseSpecificationRow.IsPartySelected = showByDifferentParties && sourceRow.Quantity > 0m;
			mTExpenseSpecificationRow.Quantity = GetQuantity(sourceRow.Quantity, fillAvailQty);
			mTExpenseSpecificationRow.UnitCode = sourceRow.UnitCode;
			mTExpenseSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTExpenseSpecificationRow.Summa = summa;
			doc.MTList.Add(mTExpenseSpecificationRow);
			rowId++;
		}
	}

	public static void CreateNewMTMoveDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTMove mTMove = Settings.DocumentService.Create<MTMove>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMove);
		int rowID = 0;
		try
		{
			mTMove.DocDate = docDate;
			FillMTMoveDocList(mTMove, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTMove.MaxRowID = rowID;
			mTMove.Summ = mTMove.MTList.Sum((MTMoveBase.MTMoveSpecificationRow row) => (!row.Summa.HasValue) ? 0m : row.Summa.Value).RoundAs(2);
			mTMove.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTMove).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static void CreateNewMTMoveWithConfirmationDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTMoveWithConfirmation mTMoveWithConfirmation = Settings.DocumentService.Create<MTMoveWithConfirmation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveWithConfirmation);
		int rowID = 0;
		try
		{
			mTMoveWithConfirmation.DocDate = docDate;
			FillMTMoveDocList(mTMoveWithConfirmation, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver);
			mTMoveWithConfirmation.MaxRowID = rowID;
			mTMoveWithConfirmation.Summ = mTMoveWithConfirmation.MTList.Sum((MTMoveBase.MTMoveSpecificationRow row) => (!row.Summa.HasValue) ? 0m : row.Summa.Value).RoundAs(2);
			mTMoveWithConfirmation.Chief = Settings.ParametersService.ChiefAccountantName();
			Settings.DocumentService.CreateWindow(mTMoveWithConfirmation).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTMoveDocList(MTMoveWithConfirmationBase doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			if (isFromTurnOver && !selectedRows.Any((GoodsRemainsDataProvider.DataRow x) => x.Quantity != 0m))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.AllRowQtyRemZero);
			}
			CheckIsSameStorages(selectedRows);
			doc.StorageExpense = selectedRows.First().Storage;
			bool storageExpenseIsRetail = TradeSettings.TradeCacheService.Storage.GetItem(doc.StorageExpense)?.Retail ?? false;
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					if (!isFromTurnOver || (isFromTurnOver && item.Quantity != 0m))
					{
						AddRowInMTMoveList(doc, fillAvailQty, item, ref rowID, showByDifferentParties, storageExpenseIsRetail);
					}
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			if (isFromTurnOver && availabilityRow.Quantity == 0m)
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.AllRowQtyRemZero);
			}
			CheckStorageAvailable(availabilityRow.Storage);
			doc.StorageExpense = availabilityRow.Storage;
			AddRowInMTMoveList(doc, fillAvailQty, availabilityRow, ref rowID, showByDifferentParties, TradeSettings.TradeCacheService.Storage.GetItem(doc.StorageExpense)?.Retail ?? false);
		}
	}

	private static void AddRowInMTMoveList(MTMoveWithConfirmationBase doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow sourceRow, ref int rowId, bool showByDifferentParties, bool storageExpenseIsRetail)
	{
		if (showByDifferentParties)
		{
			PartyRow currentPartiesListRow = GetCurrentPartiesListRow(sourceRow, fillAvailQty, rowId);
			doc.MTExpDocSelectedPartiesList.Add(currentPartiesListRow);
		}
		AddRowToMTMoveList(doc, sourceRow, fillAvailQty, ref rowId, showByDifferentParties, fillQtyOut: false, storageExpenseIsRetail);
	}

	private static void AddRowToMTMoveList(MTMoveBase doc, GoodsRemainsDataProvider.DataRow sourceRow, bool fillAvailQty, ref int rowId, bool showByDifferentParties, bool fillQtyOut = false, bool storageExpenseIsRetail = false, string minQtys = "", bool detailByOperations = false)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		decimal value = 0m;
		if (storageExpenseIsRetail)
		{
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(sourceRow.MTID, sourceRow.UnitCode);
			if (qntUnit != null)
			{
				MTRetailQtyAndPriceDataProvider.Param param = new MTRetailQtyAndPriceDataProvider.Param
				{
					MTCode = sourceRow.MTCode,
					UnitCoef = qntUnit.Coef,
					Storage = doc.StorageExpense,
					Date = doc.DocDate
				};
				List<MTRetailQtyAndPriceDataProvider.DataRow> rows = new MTRetailQtyAndPriceDataProvider(TradeSettings.CreateTradeClient()).GetRows(param, withoutAccessCheck: true);
				if (rows.Count > 0)
				{
					value = rows[0].Price;
				}
			}
		}
		if (item != null)
		{
			MTMoveBase.MTMoveSpecificationRow mTMoveSpecificationRow = new MTMoveBase.MTMoveSpecificationRow
			{
				RowID = rowId,
				MTID = sourceRow.MTID,
				MTCode = item.MTCode,
				MTName = item.Caption,
				IsPartySelected = (showByDifferentParties && sourceRow.Quantity > 0m),
				Quantity = (string.IsNullOrWhiteSpace(minQtys) ? GetQuantity(sourceRow.Quantity, fillAvailQty, fillQtyOut) : sourceRow.OrderQty),
				UnitCode = sourceRow.UnitCode,
				UnitBrief = sourceRow.UnitBrief,
				Price = value
			};
			mTMoveSpecificationRow.Summa = PriceHelper.CalculateSum(mTMoveSpecificationRow.Price.Value, mTMoveSpecificationRow.Quantity);
			doc.MTList.Add(mTMoveSpecificationRow);
			rowId++;
		}
	}

	public static void CreateNewMTMoveOrderDoc(bool fillAvailQty, DateOnly docDate, bool showByDifferentParties, bool isFromTurnOver = false, string minQtys = "", bool detailByOperations = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			MTMoveOrder mTMoveOrder = Settings.DocumentService.Create<MTMoveOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTMoveOrder);
			mTMoveOrder.DocDate = docDate;
			int rowID = 0;
			FillMTMoveOrderDocList(mTMoveOrder, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver, minQtys, detailByOperations);
			mTMoveOrder.MaxRowID = rowID;
			mTMoveOrder.Summ = mTMoveOrder.MTList.Sum((MTMoveBase.MTMoveSpecificationRow row) => row.Summa.Value).RoundAs(2);
			Settings.DocumentService.CreateWindow(mTMoveOrder).Show();
		}
	}

	private static void FillMTMoveOrderDocList(MTMoveOrder doc, bool fillAvailQty, ref int rowID, bool showByDifferentParties, bool isFromTurnOver, string minQtys = "", bool detailByOperations = false)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows, isFromTurnOver, detailByOperations);
		if (selectedRows != null)
		{
			CheckIsSameStorages(selectedRows);
			doc.StorageIncome = selectedRows.First().Storage;
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowToMTMoveList(doc, item, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver, storageExpenseIsRetail: false, minQtys, detailByOperations);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckStorageAvailable(availabilityRow.Storage);
			doc.StorageIncome = availabilityRow.Storage;
			AddRowToMTMoveList(doc, availabilityRow, fillAvailQty, ref rowID, showByDifferentParties, isFromTurnOver, storageExpenseIsRetail: false, minQtys, detailByOperations);
		}
	}

	public static void CreateNewMTStockTakingDoc(bool fillAvailQty, DateOnly docDate, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTStockTaking mTStockTaking = Settings.DocumentService.Create<MTStockTaking>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking);
		int rowID = 0;
		try
		{
			mTStockTaking.DocDate = docDate;
			FillMTStockTakingDocList(mTStockTaking, fillAvailQty, ref rowID, isFromTurnOver);
			Settings.DocumentService.CreateWindow(mTStockTaking).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void CheckAndSetMTStockTakingStorage(MTStockTaking doc, string storage)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
		int num;
		if (item == null)
		{
			num = 0;
		}
		else
		{
			num = (item.Retail ? 1 : 0);
			if (num != 0 && doc.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTaking.Value)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Trade.Common.Resources.DocMTStockTaking));
			}
		}
		if (num == 0 && doc.POType == ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail.Value)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForRetailStorage, ArmSoft.SME.Trade.Common.Resources.DocMTInventoryRetail));
		}
		if (!string.IsNullOrWhiteSpace(storage) && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
		{
			doc.Storage = storage;
		}
	}

	private static void AddRowInMTStockTakingList(MTStockTaking doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow sourceRow, ref int rowId)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			MTStockTakingBase.MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingBase.MTStockTakingSpecificationRow();
			mTStockTakingSpecificationRow.MTID = sourceRow.MTID;
			mTStockTakingSpecificationRow.MTCode = item.MTCode;
			mTStockTakingSpecificationRow.MTName = item.Caption;
			mTStockTakingSpecificationRow.AccQuantity = sourceRow.Quantity;
			mTStockTakingSpecificationRow.ActualQuantity = (fillAvailQty ? mTStockTakingSpecificationRow.AccQuantity : 0m);
			mTStockTakingSpecificationRow.VarianceQuantity = mTStockTakingSpecificationRow.ActualQuantity - mTStockTakingSpecificationRow.AccQuantity;
			mTStockTakingSpecificationRow.UnitCode = sourceRow.UnitCode;
			mTStockTakingSpecificationRow.UnitBrief = sourceRow.UnitBrief;
			mTStockTakingSpecificationRow.AccSum = sourceRow.CostSumm;
			if (mTStockTakingSpecificationRow.AccQuantity != 0m)
			{
				mTStockTakingSpecificationRow.Price = (mTStockTakingSpecificationRow.AccSum / mTStockTakingSpecificationRow.AccQuantity).RoundAs(2);
			}
			if (mTStockTakingSpecificationRow.AccQuantity == mTStockTakingSpecificationRow.ActualQuantity)
			{
				mTStockTakingSpecificationRow.ActualSum = mTStockTakingSpecificationRow.AccSum;
			}
			else
			{
				mTStockTakingSpecificationRow.ActualSum = (mTStockTakingSpecificationRow.Price * mTStockTakingSpecificationRow.ActualQuantity).RoundAs(2);
			}
			doc.MTList.Add(mTStockTakingSpecificationRow);
			rowId++;
		}
	}

	private static void FillMTStockTakingDocList(MTStockTaking doc, bool fillAvailQty, ref int rowID, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			CheckIsSameStorages(selectedRows);
			CheckAndSetMTStockTakingStorage(doc, selectedRows.First().Storage);
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowInMTStockTakingList(doc, fillAvailQty, item, ref rowID);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckAndSetMTStockTakingStorage(doc, availabilityRow.Storage);
			CheckStorageAvailable(availabilityRow.Storage);
			AddRowInMTStockTakingList(doc, fillAvailQty, availabilityRow, ref rowID);
		}
	}

	public static void CreateNewMTStockTakingRetailDoc(bool fillAvailQty, DateOnly docDate, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTStockTakingRetail mTStockTakingRetail = Settings.DocumentService.Create<MTStockTakingRetail>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTStockTakingRetail);
		int rowID = 0;
		try
		{
			mTStockTakingRetail.DocDate = docDate;
			FillMTStockTakingDocList((MTStockTaking)mTStockTakingRetail, fillAvailQty, ref rowID, isFromTurnOver);
			Settings.DocumentService.CreateWindow(mTStockTakingRetail).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private void FillMTStockTakingDocList(MTStockTakingRetail doc, bool fillAvailQty, ref int rowID, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		if (selectedRows != null)
		{
			CheckIsSameStorages(selectedRows);
			CheckAndSetMTStockTakingRetailStorage(doc, selectedRows.First().Storage);
			{
				foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
				{
					CheckStorageAvailable(item.Storage);
					AddRowInMTStockTakingRetailList(doc, fillAvailQty, item, ref rowID);
				}
				return;
			}
		}
		if (availabilityRow != null)
		{
			CheckAndSetMTStockTakingRetailStorage(doc, availabilityRow.Storage);
			CheckStorageAvailable(availabilityRow.Storage);
			AddRowInMTStockTakingRetailList(doc, fillAvailQty, availabilityRow, ref rowID);
		}
	}

	private static void CheckAndSetMTStockTakingRetailStorage(MTStockTakingRetail doc, string storage)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(storage);
		if (item != null && item.Retail && !string.IsNullOrWhiteSpace(storage))
		{
			StoragesCacheRow item2 = TradeSettings.TradeCacheService.Storage.GetItem(storage);
			if (item2 != null && item2.Retail && TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
			{
				doc.Storage = storage;
			}
		}
	}

	private void AddRowInMTStockTakingRetailList(MTStockTakingRetail doc, bool fillAvailQty, GoodsRemainsDataProvider.DataRow sourceRow, ref int rowId)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item != null)
		{
			MTStockTakingBase.MTStockTakingSpecificationRow mTStockTakingSpecificationRow = new MTStockTakingBase.MTStockTakingSpecificationRow
			{
				MTID = sourceRow.MTID,
				MTCode = item.MTCode,
				MTName = item.Caption,
				AccQuantity = sourceRow.Quantity,
				ActualQuantity = (fillAvailQty ? sourceRow.Quantity : 0m),
				UnitCode = sourceRow.UnitCode,
				UnitBrief = sourceRow.UnitBrief,
				AccSum = sourceRow.CostSumm,
				Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(doc.PriceType, doc.VATType, sourceRow.MTID, sourceRow.UnitCode, doc.DocDate, "AMD")
			};
			mTStockTakingSpecificationRow.VarianceQuantity = mTStockTakingSpecificationRow.ActualQuantity - mTStockTakingSpecificationRow.AccQuantity;
			mTStockTakingSpecificationRow.ActualSum = ((mTStockTakingSpecificationRow.AccQuantity == mTStockTakingSpecificationRow.ActualQuantity) ? mTStockTakingSpecificationRow.AccSum : PriceHelper.CalculateSum(mTStockTakingSpecificationRow.Price, mTStockTakingSpecificationRow.ActualQuantity));
			doc.MTList.Add(mTStockTakingSpecificationRow);
			rowId++;
		}
	}

	public static void CreateNewMTRevaluationDoc()
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		MTRevaluation mTRevaluation = Settings.DocumentService.Create<MTRevaluation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTRevaluation);
		mTRevaluation.DocDate = Settings.HostingEnvironment.CurrentDate;
		mTRevaluation.Coefficient = 1m;
		int num = 0;
		bool flag = true;
		if (Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
		{
			bool flag2 = false;
			bool flag3 = false;
			foreach (GoodsRemainsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>())
			{
				if (selectedRow.MTExpMethod == "3")
				{
					flag2 = true;
				}
				else
				{
					flag3 = true;
				}
				if (flag2 & flag3)
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.AvgWeigtItemsRevaluatedSeparately, 64);
				}
			}
			if (!flag2)
			{
				flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillAllAvailability, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
			}
			foreach (GoodsRemainsDataProvider.DataRow selectedRow2 in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>())
			{
				MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(selectedRow2.MTCode);
				mTRevaluation.MTList.Add(new MTRevaluation.MTRevaluationSpecificationRow
				{
					MTCode = selectedRow2.MTCode,
					MTID = selectedRow2.MTID,
					Storage = selectedRow2.Storage,
					MTName = selectedRow2.Caption,
					Quantity = (flag ? selectedRow2.Quantity : 1m),
					UnitCode = selectedRow2.UnitCode,
					UnitBrief = selectedRow2.UnitBrief,
					Cost = selectedRow2.Cost.Value,
					CostWithVAT = selectedRow2.CostWithVAT.Value,
					CurCostWithVAT = selectedRow2.CurrCost.Value,
					NewCost = selectedRow2.Cost.Value,
					NewCostWithVAT = selectedRow2.CostWithVAT.Value,
					NewCurCostWithVAT = selectedRow2.CurrCost.Value,
					CostSumm = selectedRow2.CostSumm,
					CostSummWithVAT = selectedRow2.CostSummWithVAT,
					CurCostSummWithVAT = selectedRow2.CurrCostSumm,
					VATSumm = selectedRow2.CostWithVAT.Value,
					IncomeDate = selectedRow2.IncomeDate,
					IncomeDocDescr = selectedRow2.PartyDocType + "  N " + selectedRow2.PartyDocNum,
					PartyBase = selectedRow2.PartyBase,
					PartyRow = selectedRow2.PartyRow,
					PartID = selectedRow2.PartyPartID,
					PartnerName = selectedRow2.PartyPartCaption,
					ContractName = selectedRow2.PartyContractCaption,
					ContractCode = selectedRow2.PartyContractCode,
					ProducerCode = selectedRow2.PartyDocProducerCode,
					ProducerName = selectedRow2.PartyDocProducer,
					CountryName = selectedRow2.PartyDocCountry,
					CountryCode = selectedRow2.PartyDocCountryCode,
					UseLife = selectedRow2.PartyDocUsableDate,
					Property1Code = selectedRow2.PartyProperty1TypeCode,
					Property1Value = selectedRow2.PartyDocPropertyValue1,
					Property2Code = selectedRow2.PartyProperty2TypeCode,
					Property2Value = selectedRow2.PartyDocPropertyValue2,
					ExpenseMethod = ((item != null) ? item.ExpenseMethod : ""),
					UnitFraction = true,
					RowID = num
				});
				num++;
			}
		}
		else if (Settings.HostingEnvironment.CurrentViewPanel.FocusedRow() != null)
		{
			GoodsRemainsDataProvider.DataRow dataRow = (GoodsRemainsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				if (dataRow.MTExpMethod != "3")
				{
					flag = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillAllAvailability, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
				}
				MaterialCacheRow item2 = TradeSettings.TradeCacheService.Material.GetItem(dataRow.MTCode);
				mTRevaluation.MTList.Add(new MTRevaluation.MTRevaluationSpecificationRow
				{
					MTCode = dataRow.MTCode,
					MTID = dataRow.MTID,
					Storage = dataRow.Storage,
					MTName = dataRow.Caption,
					Quantity = (flag ? dataRow.Quantity : 1m),
					UnitCode = dataRow.UnitCode,
					UnitBrief = dataRow.UnitBrief,
					Cost = dataRow.Cost.Value,
					CostWithVAT = dataRow.CostWithVAT.Value,
					CurCostWithVAT = dataRow.CurrCost.Value,
					NewCost = dataRow.Cost.Value,
					NewCostWithVAT = dataRow.CostWithVAT.Value,
					NewCurCostWithVAT = dataRow.CurrCost.Value,
					CostSumm = dataRow.CostSumm,
					CostSummWithVAT = dataRow.CostSummWithVAT,
					CurCostSummWithVAT = dataRow.CurrCostSumm,
					VATSumm = dataRow.CostWithVAT.Value,
					IncomeDate = dataRow.IncomeDate,
					IncomeDocDescr = dataRow.PartyDocType + "  N " + dataRow.PartyDocNum,
					PartyBase = dataRow.PartyBase,
					PartyRow = dataRow.PartyRow,
					PartID = dataRow.PartyPartID,
					PartnerName = dataRow.PartyPartCaption,
					ContractName = dataRow.PartyContractCaption,
					ContractCode = dataRow.PartyContractCode,
					ProducerCode = dataRow.PartyDocProducerCode,
					ProducerName = dataRow.PartyDocProducer,
					CountryName = dataRow.PartyDocCountry,
					CountryCode = dataRow.PartyDocCountryCode,
					UseLife = dataRow.PartyDocUsableDate,
					Property1Code = dataRow.PartyProperty1TypeCode,
					Property1Value = dataRow.PartyDocPropertyValue1,
					Property2Code = dataRow.PartyProperty2TypeCode,
					Property2Value = dataRow.PartyDocPropertyValue2,
					ExpenseMethod = ((item2 != null) ? item2.ExpenseMethod : ""),
					UnitFraction = true,
					RowID = num
				});
			}
		}
		if (mTRevaluation.MTList.Count == 0)
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.NotExistRev);
		}
		Settings.DocumentService.CreateWindow(mTRevaluation).Show();
	}

	public static void CheckIsSameTypeOfStorages(List<GoodsRemainsDataProvider.DataRow> selectedRows)
	{
		if (selectedRows.Where((GoodsRemainsDataProvider.DataRow x) => TradeSettings.TradeCacheService.Storage.GetItem(x.Storage)?.Retail ?? false).Count() > 1)
		{
			throw new RESTException(ArmSoft.SME.Trade.Common.Resources.NotTheSameTypeOfStorages);
		}
	}

	public static void CreateNewInvoiceForPayDoc(bool fillAvailQty, bool isFromTurnOver)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		InvoiceForPay invoiceForPay = Settings.DocumentService.Create<InvoiceForPay>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.InvoiceForPay);
		int rowID = 0;
		try
		{
			invoiceForPay.DocDate = Settings.HostingEnvironment.CurrentDate;
			invoiceForPay.DocNum = Settings.ParametersService.InvoiceForPayNumber();
			FillInvoiceForPayPartnerContractAndDiscount(invoiceForPay);
			invoiceForPay.SalesConsultant = Settings.ParametersService.DefaultSalesConsultant();
			invoiceForPay.PriceListType = Settings.ParametersService.DefaultSalePrice();
			invoiceForPay.SupplierBusinessAddress = Settings.ParametersService.CompanyBusinessAddress();
			invoiceForPay.SupplierName = Settings.ParametersService.CompanyName();
			invoiceForPay.SupplierLegalAddress = Settings.ParametersService.CompanyAddress();
			invoiceForPay.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			invoiceForPay.SupplierSettlAcc = TradeSettings.TradeCacheService.AccountInBank.GetDefaultSettlementAccount(isSettlAccount: true);
			invoiceForPay.SupplierTaxCode = Settings.ParametersService.TaxCode();
			invoiceForPay.SupplierMngPost = Settings.ParametersService.ManagerPost();
			invoiceForPay.SupplierMngName = Settings.ParametersService.ManagerName();
			invoiceForPay.SupplierChiefAccPost = Settings.ParametersService.ChiefAccountantPost();
			invoiceForPay.SupplierChiefAccName = Settings.ParametersService.ChiefAccountantName();
			invoiceForPay.MaxRowID = rowID;
			FillInvoiceForPayDocList(invoiceForPay, fillAvailQty, ref rowID, isFromTurnOver);
			invoiceForPay.SummVAT = invoiceForPay.MTList.Where((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.VAT.Value).Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa * Settings.ParametersService.VAT() / 100m).RoundAs(2);
			invoiceForPay.Summ = (invoiceForPay.MTList.Sum((MTInvoiceBase.MTInvoiceSpecificationRow row) => row.Summa) + invoiceForPay.SummVAT).RoundAs(2);
			((InvoiceForPayWindow)Settings.DocumentService.CreateWindow(invoiceForPay)).Show();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillInvoiceForPayDocList(InvoiceForPay doc, bool fillAvailQty, ref int rowID, bool isFromTurnOver)
	{
		GoodsRemainsDataProvider.DataRow availabilityRow = null;
		List<GoodsRemainsDataProvider.DataRow> selectedRows = null;
		GetAvailabilityFromAvailabilityOrTurnOver(isFromTurnOver, ref availabilityRow, ref selectedRows);
		string text = Settings.ParametersService.DefaultStorage();
		if (!string.IsNullOrWhiteSpace(text))
		{
			doc.Storage = text;
		}
		if (selectedRows != null)
		{
			foreach (GoodsRemainsDataProvider.DataRow item in selectedRows)
			{
				CheckStorageAvailable(item.Storage);
				AddCurrentRowToInvoiceForPayList(doc, item, rowID, fillAvailQty);
			}
			return;
		}
		if (availabilityRow != null)
		{
			CheckStorageAvailable(availabilityRow.Storage);
			AddCurrentRowToInvoiceForPayList(doc, availabilityRow, rowID, fillAvailQty);
		}
	}

	public static void AddCurrentRowToInvoiceForPayList(InvoiceForPay doc, GoodsRemainsDataProvider.DataRow sourceRow, int rowID, bool fillAvailQty)
	{
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
		if (item == null)
		{
			return;
		}
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow();
		mTInvoiceSpecificationRow.UnitCode = item.Unit;
		mTInvoiceSpecificationRow.RowID = doc.MaxRowID;
		mTInvoiceSpecificationRow.ItemType = GoodsTypesSE.Good;
		mTInvoiceSpecificationRow.Storage = sourceRow.Storage;
		mTInvoiceSpecificationRow.ItemID = sourceRow.MTID;
		mTInvoiceSpecificationRow.ItemCode = item.MTCode;
		mTInvoiceSpecificationRow.ItemName = item.Caption;
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow2 = mTInvoiceSpecificationRow;
		decimal quantity;
		if (fillAvailQty)
		{
			quantity = ((sourceRow.Quantity > 0m) ? sourceRow.Quantity : 0m);
		}
		else
		{
			quantity = ((sourceRow.Quantity > 0m) ? 1m : 0m);
		}
		mTInvoiceSpecificationRow2.Quantity = quantity;
		if (Settings.ParametersService.DiscountUse())
		{
			mTInvoiceSpecificationRow.Discount = doc.Discount;
			if (mTInvoiceSpecificationRow.Discount == 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
			else if (item.Discount != 0m)
			{
				mTInvoiceSpecificationRow.Discount = item.Discount;
			}
		}
		mTInvoiceSpecificationRow.UnitBrief = sourceRow.UnitBrief;
		mTInvoiceSpecificationRow.VAT = item.VAT;
		mTInvoiceSpecificationRow.Price = TradeSettings.CreateTradeClient().CommonServices.GetPriceForGivenPriceListAndVATType(doc.PriceListType, doc.VATType, mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode, doc.DocDate, "AMD");
		mTInvoiceSpecificationRow.DiscountPrice = (mTInvoiceSpecificationRow.Price * (1m - mTInvoiceSpecificationRow.Discount / 100m)).RoundAs(2);
		mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.DiscountPrice * mTInvoiceSpecificationRow.Quantity).RoundAs(2);
		if (Settings.ParametersService.EnvironmentalFeePayer())
		{
			mTInvoiceSpecificationRow.EnvFeePercent = item.EcoFeeRate;
			mTInvoiceSpecificationRow.EnvFeeSumma = ArmSoft.SME.Trade.Common.Helpers.PriceHelper.GetEnvironmentalFeeAmount(mTInvoiceSpecificationRow.Summa, mTInvoiceSpecificationRow.EnvFeePercent, mTInvoiceSpecificationRow.VAT.Value, doc.VATType, Settings.ParametersService.VAT(), Settings.ParametersService.VATIncluded());
		}
		doc.MTList.Add(mTInvoiceSpecificationRow);
		doc.MaxRowID++;
		rowID++;
	}

	private static void FillInvoiceForPayPartnerContractAndDiscount(InvoiceForPay doc)
	{
		string text = Settings.ParametersService.DefaultCustomer();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(text);
		if (item != null && item.Customer)
		{
			doc.PartnerContractGroup.PartnerCode = text;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(item);
			if (defaultContract != null)
			{
				doc.PartnerContractGroup.ContractCode = defaultContract.Contract;
				doc.Discount = defaultContract.Percent;
			}
		}
	}

	public static void CreateNewMTComplectionDoc(DateOnly docDate, bool isFromTurnOver = false)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel == null)
		{
			return;
		}
		try
		{
			if (!Settings.HostingEnvironment.CurrentViewPanel.GetSelectedAny())
			{
				MTComplectation doc = Settings.DocumentService.Create<MTComplectation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation);
				doc.DocDate = docDate;
				GoodsRemainsDataProvider.DataRow availabilityRowFromAvailabilityOrTurnOver = GetAvailabilityRowFromAvailabilityOrTurnOver(isFromTurnOver);
				FillMTComplection(ref doc, availabilityRowFromAvailabilityOrTurnOver.Storage, availabilityRowFromAvailabilityOrTurnOver);
				Settings.DocumentService.CreateWindow(doc).Show();
				return;
			}
			IEnumerable<GoodsRemainsDataProvider.DataRow> enumerable = from row in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<GoodsRemainsDataProvider.DataRow>()
				where row.Quantity < 0m && !(TradeSettings.TradeCacheService.Storage.GetItem(row.Storage)?.Retail ?? false)
				select row;
			if (enumerable.Count() == 0)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStoreAndNegativeQty, ArmSoft.SME.Trade.Common.Resources.DocMTComplectation));
			}
			MTComplectation mTComplectation = Settings.DocumentService.Create<MTComplectation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation);
			DialogWindow dialogWindow = new DialogWindow();
			DropDownControl dropDownControl = new DropDownControl();
			dialogWindow.Name = ArmSoft.SME.Trade.Common.Resources.DocMTComplectation;
			dropDownControl = dialogWindow.AddDropDownControl("OutStorage", ArmSoft.SME.Trade.Common.Resources.OutStorage, new ArmSoft.SME.Trade.UI.DropDown.Storages());
			dropDownControl.Code = enumerable.FirstOrDefault().Storage;
			dialogWindow.AddBooleanControl("ShowStorageDocsDataView", ArmSoft.SME.Trade.Common.Resources.ShowDocumentsJournal);
			dialogWindow.ShowDialog();
			foreach (GoodsRemainsDataProvider.DataRow item in enumerable)
			{
				mTComplectation = Settings.DocumentService.Create<MTComplectation>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation);
				mTComplectation.DocDate = docDate;
				mTComplectation.DocNum = Settings.ParametersService.MTComplect();
				mTComplectation.Owner = Settings.LoginService.UserName;
				FillMTComplection(ref mTComplectation, dropDownControl.Code, item);
				try
				{
					Settings.DocumentService.Store(mTComplectation, new StoreRequest());
				}
				catch
				{
					Settings.DocumentService.Store(mTComplectation, new StoreRequest
					{
						State = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.Draft
					});
				}
			}
			if ((bool)dialogWindow["ShowStorageDocsDataView"])
			{
				AllDocumentsJournalDataView allDocumentsJournalDataView = new AllDocumentsJournalDataView();
				allDocumentsJournalDataView.StartDate = docDate;
				allDocumentsJournalDataView.EndDate = docDate;
				allDocumentsJournalDataView.DocTypeParam = Convert.ToString(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTComplectation.Value);
				allDocumentsJournalDataView.DocOwner = string.Empty;
				allDocumentsJournalDataView.DocState = ArmSoft.SME.Trade.Common.SmartEnums.DocumentStateSE.DraftOrProcessed.ValueUI;
				allDocumentsJournalDataView.Show(useParamValues: true, openAsChild: false);
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static void FillMTComplection(ref MTComplectation doc, string storageEx, GoodsRemainsDataProvider.DataRow availabilityRow)
	{
		StoragesCacheRow item = TradeSettings.TradeCacheService.Storage.GetItem(availabilityRow.Storage);
		if (item != null && item.Retail)
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.DocOnlyForWholesaleStore, ArmSoft.SME.Trade.Common.Resources.DocMTComplectation));
		}
		if (!string.IsNullOrWhiteSpace(availabilityRow.Storage))
		{
			CheckStorageAvailable(availabilityRow.Storage);
			doc.StorageIncome = availabilityRow.Storage;
			CheckStorageAvailable(storageEx);
			doc.StorageExpense = storageEx;
		}
		doc.MTComplectCode = availabilityRow.MTCode;
		doc.QuantityUnit = availabilityRow.UnitCode;
		if (availabilityRow.Quantity < 0m)
		{
			doc.Quantity = Math.Abs(availabilityRow.Quantity);
		}
		else
		{
			doc.Quantity = 0m;
		}
		LoadComplectPartsList(doc, availabilityRow);
	}

	private static void LoadComplectPartsList(MTComplectation doc, GoodsRemainsDataProvider.DataRow sourceRow)
	{
		try
		{
			MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(sourceRow.MTCode);
			decimal value = doc.Quantity.Value;
			if (item == null)
			{
				return;
			}
			MaterialCacheRow.QntUnitRow qntUnit = TradeSettings.TradeCacheService.Material.GetQntUnit(item.MTCode, doc.QuantityUnit);
			if (qntUnit != null)
			{
				value *= qntUnit.Coef;
			}
			List<MTComplectsPartsDataProvider.DataRow> rows = new MTComplectsPartsDataProvider(TradeSettings.CreateTradeClient()).GetRows(new MTComplectsPartsDataProvider.Param
			{
				ComplectCode = sourceRow.MTCode
			}, withoutAccessCheck: true);
			if (rows.Count <= 0)
			{
				return;
			}
			doc.MaxRowID = 0;
			foreach (MTComplectsPartsDataProvider.DataRow item2 in rows)
			{
				MTComplectation.MTComplectationSpecificationRow mTComplectationSpecificationRow = new MTComplectation.MTComplectationSpecificationRow();
				mTComplectationSpecificationRow.ItemType = item2.ItemType;
				mTComplectationSpecificationRow.ItemCode = item2.MTSourceCode;
				mTComplectationSpecificationRow.ItemID = item2.MTSourceID;
				mTComplectationSpecificationRow.ItemName = item2.MTSourceCaption;
				mTComplectationSpecificationRow.UnitCode = item2.QntUnit;
				mTComplectationSpecificationRow.UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(item2.QntUnit)?.Brief;
				mTComplectationSpecificationRow.Quantity = (item2.QTY * value).RoundAs(3);
				if (item2.ComplectQTY != 0m)
				{
					mTComplectationSpecificationRow.Quantity = (mTComplectationSpecificationRow.Quantity.Value / item2.ComplectQTY).RoundAs(3);
				}
				mTComplectationSpecificationRow.Summa = 0m;
				if (mTComplectationSpecificationRow.ItemType != GoodsTypesSE.Good.Value)
				{
					mTComplectationSpecificationRow.Summa = (item2.Summ * mTComplectationSpecificationRow.Quantity.Value).RoundAs(3);
				}
				if (mTComplectationSpecificationRow.ItemType == GoodsTypesSE.Good.Value)
				{
					mTComplectationSpecificationRow.IsPartySelected = false;
				}
				else
				{
					mTComplectationSpecificationRow.IsPartySelected = null;
				}
				mTComplectationSpecificationRow.RowID = doc.MaxRowID;
				doc.ComplectPartsList.Add(mTComplectationSpecificationRow);
				doc.MaxRowID++;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	private static decimal GetQuantity(decimal quantity, bool fillAvailQty = true, bool fillQtyOut = false)
	{
		if (!(quantity > 0m))
		{
			return 0m;
		}
		if (!(fillAvailQty | fillQtyOut))
		{
			return 1m;
		}
		return quantity;
	}

	private static string GetPartyMethod(bool showByDifferentParties, decimal quantity)
	{
		if (showByDifferentParties && quantity > 0m)
		{
			return ArmSoft.SME.Common.Resources.PartySelected;
		}
		return ArmSoft.SME.Common.Resources.AutoSelection;
	}

	public static void CheckIsSameStorages(List<GoodsRemainsDataProvider.DataRow> selectedRows)
	{
		if (!selectedRows.All((GoodsRemainsDataProvider.DataRow p) => p.Storage == selectedRows.First().Storage))
		{
			throw new RESTException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
		}
	}

	public static void CheckIsSameStorages(List<SalesAnalysisDataProvider.DataRow> selectedRows)
	{
		if (!selectedRows.All((SalesAnalysisDataProvider.DataRow p) => p.Storage == selectedRows.First().Storage))
		{
			throw new RESTException(ArmSoft.SME.Trade.Common.Resources.RowsMustHaveSameStorage);
		}
	}

	private static void CheckStorageAvailable(string storage)
	{
		if (!TradeSettings.CreateTradeClient().CommonServices.IsStorageAvailable(storage))
		{
			throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.StorageNotAvailForUser, Settings.LoginService.UserName, storage));
		}
	}

	private static PartyRow GetCurrentPartiesListRow(GoodsRemainsDataProvider.DataRow currentRow, bool fillAvailQty, int rowID)
	{
		PartyRow partyRow = new PartyRow();
		partyRow.Base = currentRow.PartyBase;
		partyRow.Row = currentRow.PartyRow;
		partyRow.GridRow = rowID;
		partyRow.IncomeDate = currentRow.IncomeDate;
		partyRow.GoodID = currentRow.MTID;
		partyRow.Qty = GetQuantity(currentRow.Quantity, fillAvailQty);
		partyRow.ExpQty = partyRow.Qty;
		partyRow.Summa = currentRow.CostSumm;
		partyRow.Cost = Convert.ToDecimal(currentRow.Cost);
		partyRow.ExpSumma = (fillAvailQty ? partyRow.Summa : partyRow.Cost);
		return partyRow;
	}

	private static GoodsRemainsDataProvider.DataRow ConvertTurnOverRowToAvailabilityRow(GoodsTurnOverDataProvider.DataRow row, bool fillQtyOut = false, bool detailByOperations = false)
	{
		GoodsRemainsDataProvider.DataRow dataRow = new GoodsRemainsDataProvider.DataRow();
		dataRow.MTCode = row.GoodCode;
		dataRow.MTID = row.GoodID;
		dataRow.Storage = row.Storage;
		GoodsRemainsDataProvider.DataRow dataRow2 = dataRow;
		decimal quantity;
		if (detailByOperations)
		{
			quantity = row.QTYOutSale + row.QTYOutSaleRetail - row.QTYInReturnFromCustomer;
		}
		else
		{
			quantity = (fillQtyOut ? row.QtyOut : row.QtyEnd);
		}
		dataRow2.Quantity = quantity;
		dataRow.UnitCode = row.Unit;
		dataRow.UnitBrief = row.UnitBrief;
		dataRow.PartyBase = row.PartyBase;
		dataRow.PartyRow = row.PartyRow;
		dataRow.IncomeDate = row.IncomeDate;
		dataRow.CostSumm = row.SumEnd;
		dataRow.PartyDocPropertyValue1 = row.PartyDocPropertyValue1;
		dataRow.PartyDocPropertyValue2 = row.PartyDocPropertyValue2;
		if (row.QtyEnd == 0m)
		{
			dataRow.Cost = 0m;
		}
		else
		{
			dataRow.Cost = Convert.ToDecimal(row.SumEnd / row.QtyEnd);
		}
		return dataRow;
	}

	public static MTInvoiceBase.MTInvoiceSpecificationRow GetMTBillCurrentMTListRow(MTBill doc, GoodsRemainsDataProvider.DataRow currentRow, bool fillAvailQty, int rowID, string defaultSalePrice, string priceListTypeRoundDgr, string discountPriceListType, string discountPriceTypeRoundDegree, string discountPriceListVATType, string discountPriceListCurr, decimal discountPriceListCurrRate, decimal discountPriceListCurrBase, bool showByDifferentParties, bool discountUse)
	{
		MTInvoiceBase.MTInvoiceSpecificationRow mTInvoiceSpecificationRow = null;
		MaterialCacheRow itemById = TradeSettings.TradeCacheService.Material.GetItemById(currentRow.MTID);
		if (itemById != null)
		{
			mTInvoiceSpecificationRow = new MTInvoiceBase.MTInvoiceSpecificationRow
			{
				RowID = rowID,
				ItemType = itemById.Type,
				Storage = currentRow.Storage,
				ItemID = currentRow.MTID,
				ItemCode = itemById.MTCode,
				ItemName = itemById.Caption,
				UnitCode = currentRow.UnitCode,
				UnitBrief = currentRow.UnitBrief,
				VAT = itemById.VAT,
				Quantity = ((!fillAvailQty) ? ((currentRow.Quantity > 0m) ? 1m : 0m) : ((currentRow.Quantity > 0m) ? currentRow.Quantity : 0m)),
				Price = Methods.RoundPrice(PriceHelper.GetPriceForGivenPriceListAndVATType(defaultSalePrice, "2", currentRow.MTID, currentRow.UnitCode, doc.DocDate, "AMD"), priceListTypeRoundDgr)
			};
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(mTInvoiceSpecificationRow.ItemID, mTInvoiceSpecificationRow.UnitCode);
			mTInvoiceSpecificationRow.BarCode = ((barCode == null) ? "" : barCode.BarCode);
			if (discountUse)
			{
				mTInvoiceSpecificationRow.Discount = doc.Discount;
				if (string.IsNullOrWhiteSpace(discountPriceListType) && (mTInvoiceSpecificationRow.Discount == 0m || itemById.Discount != 0m))
				{
					mTInvoiceSpecificationRow.Discount = itemById.Discount;
				}
			}
			if (string.IsNullOrWhiteSpace(discountPriceListType))
			{
				mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice((mTInvoiceSpecificationRow.Price * (1m - Convert.ToDecimal(mTInvoiceSpecificationRow.Discount) / 100m)).RoundAs(2), priceListTypeRoundDgr);
			}
			else
			{
				PricesDataProvider.DataRow priceCalculatedForGivenOrBaseUnit = PriceHelper.GetPriceCalculatedForGivenOrBaseUnit(discountPriceListType, mTInvoiceSpecificationRow.ItemCode, mTInvoiceSpecificationRow.UnitCode, doc.DocDate);
				if (priceCalculatedForGivenOrBaseUnit != null && priceCalculatedForGivenOrBaseUnit.Price != 0m)
				{
					if (discountPriceListCurr == "AMD")
					{
						mTInvoiceSpecificationRow.DiscountPrice = priceCalculatedForGivenOrBaseUnit.Price;
					}
					else
					{
						mTInvoiceSpecificationRow.DiscountPrice = (priceCalculatedForGivenOrBaseUnit.Price * discountPriceListCurrRate / discountPriceListCurrBase).RoundAs(2);
					}
					if (discountPriceListVATType == "1" && mTInvoiceSpecificationRow.VAT.Value)
					{
						mTInvoiceSpecificationRow.DiscountPrice += PriceHelper.CalcVATAmountByType(mTInvoiceSpecificationRow.DiscountPrice, "1", itemById.VAT);
					}
					mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice(mTInvoiceSpecificationRow.DiscountPrice, discountPriceTypeRoundDegree);
					if (mTInvoiceSpecificationRow.Price != 0m)
					{
						mTInvoiceSpecificationRow.Discount = (100m * (mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.DiscountPrice) / mTInvoiceSpecificationRow.Price).RoundAs(2);
					}
				}
				else if (doc.Discount != 0m)
				{
					mTInvoiceSpecificationRow.Discount = doc.Discount;
					mTInvoiceSpecificationRow.DiscountPrice = Methods.RoundPrice(mTInvoiceSpecificationRow.Price * (100m - mTInvoiceSpecificationRow.Discount) / 100m, priceListTypeRoundDgr);
				}
				else
				{
					mTInvoiceSpecificationRow.DiscountPrice = mTInvoiceSpecificationRow.Price;
					mTInvoiceSpecificationRow.Discount = 0m;
				}
			}
			if (discountUse)
			{
				mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.DiscountPrice).RoundAs(2);
				mTInvoiceSpecificationRow.DiscountSumma = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.Price - mTInvoiceSpecificationRow.Summa).RoundAs(2);
			}
			else
			{
				mTInvoiceSpecificationRow.Summa = (mTInvoiceSpecificationRow.Quantity * mTInvoiceSpecificationRow.Price).RoundAs(2);
			}
			mTInvoiceSpecificationRow.IsPartySelected = showByDifferentParties && currentRow.Quantity > 0m;
		}
		return mTInvoiceSpecificationRow;
	}

	public static bool FillAllAvailabilityAvailabilityOrTurnOver(short docType)
	{
		bool result = false;
		switch (docType)
		{
		case 16:
		case 22:
			result = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillZeroForActualQuantity, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.No;
			break;
		default:
			result = ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Trade.Common.Resources.FillAllAvailability, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
			break;
		case 7:
		case 11:
		case 20:
		case 32:
		case 35:
		case 36:
			break;
		}
		return result;
	}

	public static void FindDocInDataViewByBarcode(string barCode, string colName = "DocNum", short docType = 18, bool preservePreviousSelection = false, bool caseSensitive = false, bool scrollToFocusedRow = true)
	{
		if (Settings.HostingEnvironment.CurrentViewPanel != null && !string.IsNullOrWhiteSpace(barCode))
		{
			string text = "DocType";
			InlineArray4<object> buffer = default;
			buffer[0] = colName;
			buffer[1] = barCode;
			buffer[2] = text;
			buffer[3] = docType;
			CriteriaOperator filterString = CriteriaOperator.Parse(string.Format("(Trim([{0}]) = '{1}' And Trim([{2}]) = '{3}')", (ReadOnlySpan<object?>)buffer));
			Settings.HostingEnvironment.CurrentViewPanel.SelectByCondition(filterString, preservePreviousSelection);
		}
	}

	public static void FindRowByBarcode(string mtCodeColumnName, out bool preservePreviousSelection)
	{
		preservePreviousSelection = false;
		FindRowByBarCodeDialog findRowByBarCodeDialog = new FindRowByBarCodeDialog(mtCodeColumnName);
		if (findRowByBarCodeDialog.ShowDialog().Value)
		{
			preservePreviousSelection = findRowByBarCodeDialog.PreservePreviousSelection;
		}
	}

	public static void FocusRowByBarcode(string barCode, string mtCodeColumnName, bool preservePreviousSelection = false)
	{
		if (string.IsNullOrWhiteSpace(mtCodeColumnName))
		{
			throw new ArgumentNullException("MTCodeColumnName property doesn't have value");
		}
		if (string.IsNullOrWhiteSpace(barCode))
		{
			return;
		}
		try
		{
			MaterialCacheRow itemByBarCode = TradeSettings.TradeCacheService.Material.GetItemByBarCode(barCode);
			if (itemByBarCode != null)
			{
				DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
				if (!preservePreviousSelection)
				{
					currentViewPanel.ClearSelection();
				}
				int num = -1;
				GridControl gridControl = currentViewPanel.InnerGrid();
				int visibleRowCount = gridControl.VisibleRowCount;
				if (visibleRowCount != 0)
				{
					int focusedRowHandle = gridControl.View.FocusedRowHandle;
					object row = gridControl.GetRow((focusedRowHandle >= 0) ? focusedRowHandle : 0);
					if (row == null)
					{
						return;
					}
					PropertyInfo property = row.GetType().GetProperty(mtCodeColumnName);
					string text = property.GetValue(row, null).ToString();
					int num2 = 0;
					int num3;
					if (itemByBarCode.MTCode != text)
					{
						int rowHandleByVisibleIndex = gridControl.GetRowHandleByVisibleIndex(0);
						if (rowHandleByVisibleIndex >= 0)
						{
							row = gridControl.GetRow(rowHandleByVisibleIndex);
							text = property.GetValue(row, null).ToString();
							if (text == itemByBarCode.MTCode)
							{
								gridControl.CurrentItem = row;
								gridControl.SelectedItems.Add(row);
								gridControl.RefreshData();
								return;
							}
						}
						num2 = 1;
						num3 = 0;
					}
					else
					{
						num2 = (num + 1) % visibleRowCount;
						num3 = num;
					}
					while (num2 != num3)
					{
						int rowHandleByVisibleIndex = gridControl.GetRowHandleByVisibleIndex(num2);
						if (rowHandleByVisibleIndex >= 0)
						{
							row = gridControl.GetRow(rowHandleByVisibleIndex);
							text = property.GetValue(row, null).ToString();
							if (text == itemByBarCode.MTCode)
							{
								gridControl.CurrentItem = row;
								gridControl.SelectedItems.Add(row);
								gridControl.RefreshData();
								return;
							}
						}
						num2 = (num2 + 1) % visibleRowCount;
					}
				}
				if (num == -1 && currentViewPanel.DataContext is SalesAnalysis)
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.BarCodeInSaleAnalysisNotExist, barCode));
				}
			}
			else
			{
				if (!(Settings.HostingEnvironment.CurrentViewPanel.DataContext is ArmSoft.SME.Trade.UI.DataView.Goods))
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.ErrBarCodeNotExist, barCode));
				}
				if (ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.FindeInUnitedProducts, ArmSoft.SME.Trade.Common.Resources.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
				{
					CreateNewGoodFromb2bUnitedProduct(barCode, Settings.HostingEnvironment.CurrentViewPanel.Id);
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static Label.GoodLabelRow BuildLabelGoodRow(MaterialCacheRow goodRow, string prop1ValueCode = "", string prop1ValueName = "", string prop2ValueCode = "", string prop2ValueName = "")
	{
		Label.GoodLabelRow goodLabelRow = null;
		if (goodRow != null)
		{
			goodLabelRow = new Label.GoodLabelRow();
			goodLabelRow.MaterialeRow = goodRow;
			MaterialCacheRow.BarCodeRow barCode = TradeSettings.TradeCacheService.Material.GetBarCode(goodRow.Id, goodRow.Unit);
			if (barCode != null)
			{
				goodLabelRow.BarCode = barCode.BarCode;
			}
			barCode = TradeSettings.TradeCacheService.Material.GetBarCode(goodRow.Id, goodRow.AltUnit);
			if (barCode != null)
			{
				goodLabelRow.AltUnitBarCode = barCode.BarCode;
			}
			goodLabelRow.Property1Value = new CodeName
			{
				Code = prop1ValueCode,
				Name = prop1ValueName
			};
			goodLabelRow.Property2Value = new CodeName
			{
				Code = prop2ValueCode,
				Name = prop2ValueName
			};
		}
		return goodLabelRow;
	}

	public static void CreateNewGoodFromb2bUnitedProduct(string barCode, int windowDvpID)
	{
		try
		{
			b2bUnitedProductRowBase b2bUnitedProductRowBase2 = null;
			if (!string.IsNullOrWhiteSpace(barCode))
			{
				b2bUnitedProductRowBase2 = BarcodeFromb2bCloudDialogWindow.GetGoodFromBarCode(barCode);
			}
			if (b2bUnitedProductRowBase2 != null)
			{
				Good good = (Good)Settings.DirectoryService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods);
				good.Name = b2bUnitedProductRowBase2.Name;
				good.FullName = b2bUnitedProductRowBase2.FullName;
				good.Unit = "001";
				good.AltUnit = good.Unit;
				good.CPACode = b2bUnitedProductRowBase2.CPACode;
				good.VAT = b2bUnitedProductRowBase2.IsVatTaxable;
				good.IsWeight = false;
				good.BarCode = b2bUnitedProductRowBase2.Barcode;
				Settings.DirectoryService.ShowWindow(good, isPOWindow: true, WindowViewMode.NewCreate, (windowDvpID != -1) ? windowDvpID : (-1));
				return;
			}
			throw new ASException(ArmSoft.SME.Common.Resources.ProductWithBarCodeDoesNotExistInB2B);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static bool AddGoodFromUnitedProducts(b2bUnitedProductRowBase productRow, string mtGroup)
	{
		if (productRow != null)
		{
			Good good = (Good)Settings.DirectoryService.Create(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods);
			good.Code = Settings.ParametersService.NextMaterial();
			good.Name = productRow.Name;
			good.FullName = productRow.FullName;
			good.ExpenseMethod = "1";
			good.Unit = "001";
			good.AltUnit = good.Unit;
			good.CPACode = productRow.CPACode;
			good.VAT = productRow.IsVatTaxable;
			good.IsWeight = false;
			good.BarCode = productRow.Barcode;
			good.ShowInPriceList = true;
			if (!string.IsNullOrEmpty(mtGroup))
			{
				good.Group = mtGroup;
			}
			Settings.DirectoryService.Store(good, new DirStoreRequest
			{
				IsImported = true,
				LogComment = ArmSoft.SME.Common.Resources.ImportedFromUnitedProducts
			});
			return true;
		}
		throw new ASException(ArmSoft.SME.Common.Resources.ProductWithBarCodeDoesNotExistInB2B);
	}

	public static void UpdateDocTransactionISN(TradeDocument doc, Guid transactionISN)
	{
		string logComment = ((transactionISN == Guid.Empty) ? string.Empty : $"{ArmSoft.SME.Trade.Common.Resources.RecordInDiscountServer} - {transactionISN}");
		Settings.DocumentService.Store(doc, new StoreRequest
		{
			LogComment = logComment
		});
	}
}
