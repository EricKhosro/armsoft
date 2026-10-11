using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Helpers;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.ASReport;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models.AccDocs;
using ArmSoft.TaxService.Models.Invoices;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.HelpClasses;

public class TaxServiceHelper : ITaxServiceHelper
{
	protected virtual ApiClient Client => Settings.CreateApiClient();

	public string ShowSaveFileDialog(string fileName)
	{
		try
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.RegistryKey, writable: false))
			{
				if (registryKey != null)
				{
					saveFileDialog.InitialDirectory = (string)registryKey.GetValue("ExportDirectory", Directory.GetCurrentDirectory());
				}
				if (registryKey == null || !Directory.Exists(saveFileDialog.InitialDirectory))
				{
					saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
				}
			}
			saveFileDialog.DefaultExt = ".xml";
			saveFileDialog.Filter = "XML Files (.xml)|*.xml";
			saveFileDialog.CheckPathExists = true;
			saveFileDialog.FileName = string.Format(fileName + " {0} {1:yyyyMMdd HHmmss}", Settings.ParametersService.TaxCode(), DateTime.Now);
			if (Convert.ToBoolean(saveFileDialog.ShowDialog()))
			{
				string fileName2 = saveFileDialog.FileName;
				using (RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey))
				{
					registryKey2.SetValue("ExportDirectory", Path.GetDirectoryName(fileName2));
				}
				return fileName2;
			}
			return "";
		}
		catch (Exception)
		{
			return "";
		}
	}

	public bool TaxServiceAuthenticationWithSavedUser(out string exceptionMessage)
	{
		exceptionMessage = "";
		if (Settings.ParametersService.EInvoicingSaveUser())
		{
			try
			{
				Settings.TaxService.Authenticate(EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingTin()), EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingLogin()), EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingPassword()));
				ArmSoft.SME.Core.UI.Services.TaxService.AuthenticationCompleted = true;
				((DispatcherObject)Settings.HostingEnvironment).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
				{
					Settings.HostingEnvironment.StatusBarTaxServiceConnectionState.ToolTip = string.Format(Resources.TaxServiceShowTaxCodeAndDispose, EncryptHelper.Decrypt(Settings.ParametersService.EInvoicingTin()));
					Settings.HostingEnvironment.StatusBarTaxServiceConnectionState.IsVisible = true;
				}), (DispatcherPriority)6, Array.Empty<object>());
				return true;
			}
			catch (Exception ex)
			{
				exceptionMessage = ex.Message;
			}
		}
		return false;
	}

	public bool EnsureTaxServiceAuthenticationCompleted()
	{
		if (ArmSoft.SME.Core.UI.Services.TaxService.AuthenticationCompleted)
		{
			return true;
		}
		string exceptionMessage = null;
		if (!ArmSoft.SME.Core.UI.Services.TaxService.AuthenticateWithLoginWindow && TaxServiceAuthenticationWithSavedUser(out exceptionMessage))
		{
			return true;
		}
		if (!string.IsNullOrWhiteSpace(exceptionMessage))
		{
			MessageBox.Show(exceptionMessage, Resources.EInvoicing, MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		try
		{
			TaxServiceLoginWindow taxServiceLoginWindow = new TaxServiceLoginWindow(Settings.ParametersService.TaxCode());
			Cursor overrideCursor = Mouse.OverrideCursor;
			Mouse.OverrideCursor = null;
			if (taxServiceLoginWindow.ShowDialog().Value)
			{
				Mouse.OverrideCursor = overrideCursor;
				taxServiceLoginWindow.Cursor = Cursors.Wait;
				ArmSoft.SME.Core.UI.Services.TaxService.AuthenticationCompleted = true;
				ArmSoft.SME.Core.UI.Services.TaxService.AuthenticateWithLoginWindow = false;
				Settings.ParametersService.EInvoicingSaveUser_set(taxServiceLoginWindow.SaveUser, Settings.LoginService.UserId);
				Settings.ParametersService.EInvoicingLogin_set(EncryptHelper.Encrypt(taxServiceLoginWindow.UserName), Settings.LoginService.UserId);
				Settings.ParametersService.EInvoicingTin_set(EncryptHelper.Encrypt(taxServiceLoginWindow.Tin), Settings.LoginService.UserId);
				if (taxServiceLoginWindow.SaveUser)
				{
					Settings.ParametersService.EInvoicingPassword_set(EncryptHelper.Encrypt(taxServiceLoginWindow.UserPassword), Settings.LoginService.UserId);
				}
				else
				{
					Settings.ParametersService.EInvoicingPassword_set("", Settings.LoginService.UserId);
				}
				taxServiceLoginWindow.Cursor = null;
				Settings.HostingEnvironment.StatusBarTaxServiceConnectionState.ToolTip = string.Format(Resources.TaxServiceShowTaxCodeAndDispose, taxServiceLoginWindow.Tin);
				Settings.HostingEnvironment.StatusBarTaxServiceConnectionState.IsVisible = true;
				return true;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return false;
	}

	public void LogOut()
	{
		ArmSoft.SME.Core.UI.Services.TaxService.AuthenticationCompleted = false;
		Settings.TaxService.Dispose();
		Settings.HostingEnvironment.StatusBarTaxServiceConnectionState.IsVisible = false;
	}

	public TaxDocument ConvertTaxInvoiceToTaxDocument(Good taxInvoice)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxInvoice.IssuedAt?.ToDateOnly();
		taxDocument.IsCorrector = !string.IsNullOrWhiteSpace(taxInvoice.CorrectorNo);
		taxDocument.DocDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(taxInvoice.BehalfOf);
		taxDocument.SupplyDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Goods.Value);
		taxDocument.Series = taxInvoice.SerialNo.Left(1);
		taxDocument.TaxNumber = taxInvoice.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxInvoice.SerialNo;
		taxDocument.SubmittedDoc = true;
		if (taxDocument.IsCorrector)
		{
			taxDocument.CorrectionType = taxInvoice.CorrectionType;
			taxDocument.BaseSeries = taxInvoice.CorrectorNo.Left(1);
			taxDocument.BaseNumber = taxInvoice.CorrectorNo.Mid(2);
			taxDocument.BaseSerialNumber = taxInvoice.CorrectorNo;
			taxDocument.BaseDate = taxInvoice.CorrectorWrittenOfAt.ToDateOnly();
		}
		taxDocument.DocNum = taxInvoice.SerialNo;
		taxDocument.AdditionalInfo = taxInvoice.AdditionalInfo;
		taxDocument.DealNumber = taxInvoice.DealNo;
		if (taxInvoice.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxInvoice.DealtAt.ToDateOnly();
		}
		taxDocument.IsSigned = taxInvoice.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxInvoice.ApprovedAt;
		taxDocument.DeliveryMethod = taxInvoice.DeliveryMethod;
		taxDocument.TotalSumWithoutTaxes = taxInvoice.TotalWithoutTaxes;
		taxDocument.TotalVAT = taxInvoice.TotalVatAmount;
		taxDocument.TotalSum = taxInvoice.Total;
		taxDocument.SupplierTIN = taxInvoice.SupplierTin;
		taxDocument.SupplierName = taxInvoice.SupplierName;
		taxDocument.SupplierAddress = taxInvoice.SupplierAddress;
		taxDocument.SupplierBusinessAddress = taxInvoice.SourceFullAddress;
		taxDocument.SupplierBankName = taxInvoice.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxInvoice.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxInvoice.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxInvoice.BuyerTin;
		taxDocument.BuyerIdentityNo = taxInvoice.BuyerIdentityNo;
		taxDocument.BuyerName = taxInvoice.BuyerName;
		taxDocument.BuyerAddress = taxInvoice.BuyerAddress;
		taxDocument.BuyerBusinessAddress = taxInvoice.DestinationFullAddress;
		taxDocument.BuyerBankName = taxInvoice.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxInvoice.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxInvoice.BuyerAdditionalInfo;
		foreach (Good.Item item in taxInvoice.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxDocument.Type),
				ItemTypeName = Resources.Product,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = Convert.ToDecimal(item.Discount),
				Sum = item.TotalWithoutTaxes,
				DealType = (item.DealType.HasValue ? Convert.ToString(Convert.ToInt16(item.DealType)) : ""),
				VAT = ((item.VatRate == VatRate.Vat20) ? 20 : 0),
				CPACode = item.Classifier,
				SumVAT = (item.VatAmount.HasValue ? item.VatAmount.Value : 0m),
				TotalSum = item.Total,
				RowISN = (taxInvoice.IsCorrector ? item.ParentId.Value : item.Id)
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxInvoiceToTaxDocument(Excise taxInvoice)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxInvoice.IssuedAt?.ToDateOnly();
		taxDocument.IsCorrector = !string.IsNullOrWhiteSpace(taxInvoice.CorrectorNo);
		taxDocument.DocDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(taxInvoice.BehalfOf);
		taxDocument.SupplyDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Excise.Value);
		taxDocument.Series = taxInvoice.SerialNo.Left(1);
		taxDocument.TaxNumber = taxInvoice.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxInvoice.SerialNo;
		taxDocument.SubmittedDoc = true;
		if (taxDocument.IsCorrector)
		{
			taxDocument.CorrectionType = taxInvoice.CorrectionType;
			taxDocument.BaseSeries = taxInvoice.CorrectorNo.Left(1);
			taxDocument.BaseNumber = taxInvoice.CorrectorNo.Mid(2);
			taxDocument.BaseSerialNumber = taxInvoice.CorrectorNo;
			taxDocument.BaseDate = taxInvoice.CorrectorWrittenOfAt.ToDateOnly();
		}
		taxDocument.DocNum = taxInvoice.SerialNo;
		taxDocument.AdditionalInfo = taxInvoice.AdditionalInfo;
		taxDocument.DealNumber = taxInvoice.DealNo;
		if (taxInvoice.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxInvoice.DealtAt.ToDateOnly();
		}
		taxDocument.IsSigned = taxInvoice.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxInvoice.ApprovedAt;
		taxDocument.DeliveryMethod = taxInvoice.DeliveryMethod;
		taxDocument.TotalSumWithoutTaxes = taxInvoice.TotalWithoutTaxes;
		taxDocument.TotalVAT = Convert.ToDecimal(taxInvoice.TotalVatAmount);
		taxDocument.TotalExcise = Convert.ToDecimal(taxInvoice.TotalExcTaxAmount);
		taxDocument.TotalSum = taxInvoice.Total;
		taxDocument.SupplierTIN = taxInvoice.SupplierTin;
		taxDocument.SupplierName = taxInvoice.SupplierName;
		taxDocument.SupplierAddress = taxInvoice.SupplierAddress;
		taxDocument.SupplierBusinessAddress = taxInvoice.SourceFullAddress;
		taxDocument.SupplierBankName = taxInvoice.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxInvoice.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxInvoice.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxInvoice.BuyerTin;
		taxDocument.BuyerIdentityNo = taxInvoice.BuyerIdentityNo;
		taxDocument.BuyerName = taxInvoice.BuyerName;
		taxDocument.BuyerAddress = taxInvoice.BuyerAddress;
		taxDocument.BuyerBusinessAddress = taxInvoice.DestinationFullAddress;
		taxDocument.BuyerBankName = taxInvoice.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxInvoice.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxInvoice.BuyerAdditionalInfo;
		foreach (Excise.Item item in taxInvoice.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxDocument.Type),
				ItemTypeName = Resources.Product,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = Convert.ToDecimal(item.Discount),
				ExciseRate = item.ExcTaxRate,
				Excise = item.ExcTaxAmount,
				DealType = (item.DealType.HasValue ? Convert.ToString(Convert.ToInt16(item.DealType)) : ""),
				Sum = item.TotalWithoutTaxes,
				VAT = ((item.VatRate == VatRate.Vat20) ? 20 : 0),
				SumVAT = (item.VatAmount.HasValue ? Convert.ToDecimal(item.VatAmount) : 0m),
				TotalSum = item.Total,
				CPACode = item.Classifier,
				RowISN = (taxInvoice.IsCorrector ? item.ParentId.Value : item.Id)
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxInvoiceToTaxDocument(Service taxInvoice)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxInvoice.IssuedAt?.ToDateOnly();
		taxDocument.IsCorrector = !string.IsNullOrWhiteSpace(taxInvoice.CorrectorNo);
		taxDocument.DocDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(taxInvoice.BehalfOf);
		taxDocument.SupplyDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Services.Value);
		taxDocument.SubmittedDoc = true;
		taxDocument.Series = taxInvoice.SerialNo.Left(1);
		taxDocument.TaxNumber = taxInvoice.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxInvoice.SerialNo;
		if (taxDocument.IsCorrector)
		{
			taxDocument.CorrectionType = taxInvoice.CorrectionType;
			taxDocument.BaseSeries = taxInvoice.CorrectorNo.Left(1);
			taxDocument.BaseNumber = taxInvoice.CorrectorNo.Mid(2);
			taxDocument.BaseSerialNumber = taxInvoice.CorrectorNo;
			taxDocument.BaseDate = taxInvoice.CorrectorWrittenOfAt.ToDateOnly();
		}
		taxDocument.DocNum = taxInvoice.SerialNo;
		taxDocument.AdditionalInfo = taxInvoice.AdditionalInfo;
		taxDocument.DealNumber = taxInvoice.DealNo;
		if (taxInvoice.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxInvoice.DealtAt.ToDateOnly();
		}
		taxDocument.IsSigned = taxInvoice.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxInvoice.ApprovedAt;
		taxDocument.TotalSumWithoutTaxes = taxInvoice.TotalWithoutTaxes;
		taxDocument.TotalVAT = Convert.ToDecimal(taxInvoice.TotalVatAmount);
		taxDocument.TotalSum = taxInvoice.Total;
		taxDocument.SupplierTIN = taxInvoice.SupplierTin;
		taxDocument.SupplierName = taxInvoice.SupplierName;
		taxDocument.SupplierAddress = taxInvoice.SupplierAddress;
		taxDocument.SupplierBusinessAddress = "";
		taxDocument.SupplierBankName = taxInvoice.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxInvoice.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxInvoice.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxInvoice.BuyerTin;
		taxDocument.BuyerIdentityNo = taxInvoice.BuyerIdentityNo;
		taxDocument.BuyerName = taxInvoice.BuyerName;
		taxDocument.BuyerAddress = taxInvoice.BuyerAddress;
		taxDocument.BuyerBusinessAddress = "";
		taxDocument.BuyerBankName = taxInvoice.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxInvoice.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxInvoice.BuyerAdditionalInfo;
		foreach (Service.Item item in taxInvoice.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxDocument.Type),
				ItemTypeName = Resources.Service,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = Convert.ToDecimal(item.Discount),
				Sum = item.TotalWithoutTaxes,
				DealType = (item.DealType.HasValue ? Convert.ToString(Convert.ToInt16(item.DealType)) : ""),
				VAT = ((item.VatRate == VatRate.Vat20) ? 20 : 0),
				SumVAT = (item.VatAmount.HasValue ? Convert.ToDecimal(item.VatAmount) : 0m),
				TotalSum = item.Total,
				RowISN = (taxInvoice.IsCorrector ? item.ParentId.Value : item.Id)
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxInvoiceToTaxDocument(VatRefund taxInvoice)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.DocDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(BehalfOfType.None);
		taxDocument.SupplyDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.VatReturn.Value);
		taxDocument.Series = taxInvoice.SerialNo.Left(1);
		taxDocument.TaxNumber = taxInvoice.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxInvoice.SerialNo;
		taxDocument.DocNum = taxInvoice.SerialNo;
		taxDocument.SubmittedDoc = true;
		taxDocument.TotalSumWithoutTaxes = taxInvoice.TotalWithoutTaxes;
		taxDocument.TotalVAT = Convert.ToDecimal(taxInvoice.TotalVatAmount);
		taxDocument.TotalSum = taxInvoice.Total;
		taxDocument.SupplierTIN = taxInvoice.SupplierTin;
		taxDocument.SupplierName = taxInvoice.SupplierName;
		taxDocument.SupplierAddress = taxInvoice.SupplierAddress;
		taxDocument.SupplierBusinessAddress = "";
		taxDocument.SupplierAdditionalData = taxInvoice.SupplierAdditionalInfo;
		taxDocument.BuyerIdentityNo = taxInvoice.BuyerPassport;
		taxDocument.BuyerName = taxInvoice.BuyerFullName;
		taxDocument.BuyerAddress = taxInvoice.BuyerAddress;
		taxDocument.BuyerBusinessAddress = "";
		taxDocument.BuyerBankName = taxInvoice.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxInvoice.BuyerAccNo;
		foreach (VatRefund.Item item in taxInvoice.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = "",
				ItemTypeName = "",
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = Convert.ToDecimal(item.Discount),
				ExciseRate = 0m,
				Excise = 0m,
				Sum = item.TotalWithoutTaxes,
				DealType = "",
				VAT = ((item.VatRate == VatRate.Vat20) ? 20 : 0),
				SumVAT = Convert.ToDecimal(item.VatAmount),
				TotalSum = item.Total,
				RowISN = item.Id
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxInvoiceToTaxDocument(Leasing taxInvoice)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.DocDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.SubMissionDate = taxInvoice.IssuedAt?.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(BehalfOfType.None);
		taxDocument.SupplyDate = taxInvoice.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.Leasing.Value);
		taxDocument.Series = taxInvoice.SerialNo.Left(1);
		taxDocument.TaxNumber = taxInvoice.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxInvoice.SerialNo;
		taxDocument.DocNum = taxInvoice.SerialNo;
		taxDocument.SubmittedDoc = true;
		taxDocument.TotalSumWithoutTaxes = taxInvoice.TotalWithoutTaxes;
		taxDocument.TotalVAT = Convert.ToDecimal(taxInvoice.TotalVatAmount);
		taxDocument.TotalSum = taxInvoice.Total;
		taxDocument.SupplierTIN = taxInvoice.LessorTin;
		taxDocument.SupplierName = taxInvoice.LessorName;
		taxDocument.SupplierAddress = taxInvoice.LessorAddress;
		taxDocument.SupplierBusinessAddress = "";
		taxDocument.SupplierAdditionalData = taxInvoice.LessorAdditionalInfo;
		taxDocument.BuyerTIN = taxInvoice.LesseeTin;
		taxDocument.BuyerIdentityNo = taxInvoice.LesseeIdentityNo;
		taxDocument.BuyerName = taxInvoice.LesseeName;
		taxDocument.BuyerAddress = taxInvoice.LesseeAddress;
		taxDocument.BuyerBusinessAddress = "";
		taxDocument.BuyerBankName = taxInvoice.LesseeBank;
		taxDocument.BuyerBankAccountNumber = taxInvoice.LesseeAccNo;
		taxDocument.BuyerAdditionalData = taxInvoice.LesseeAdditionalInfo;
		taxDocument.BuyerHasNoTin = taxInvoice.LesseeHasNoTin;
		foreach (Leasing.Item item in taxInvoice.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = "",
				ItemTypeName = "",
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = 0m,
				ExciseRate = 0m,
				Excise = 0m,
				Sum = item.TotalWithoutTaxes,
				DealType = "",
				VAT = ((item.VatRate == VatRate.Vat20) ? 20m : 0m),
				EnvironmentalFee = 0m,
				SumVAT = Convert.ToDecimal(item.VatAmount),
				TotalSum = item.Total,
				RowISN = item.Id
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxAccDocToTaxDocument(AccDocGoods taxAccDoc)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxAccDoc.IssuedAt?.ToDateOnly();
		taxDocument.IsCorrector = !string.IsNullOrWhiteSpace(taxAccDoc.CorrectorNo);
		taxDocument.DocDate = taxAccDoc.DeliveredAt.ToDateOnly();
		taxDocument.BehalfOf = Convert.ToInt16(taxAccDoc.BehalfOf);
		taxDocument.SupplyDate = taxAccDoc.DeliveredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocGoods.Value);
		taxDocument.Series = taxAccDoc.SerialNo.Left(1);
		taxDocument.TaxNumber = taxAccDoc.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxAccDoc.SerialNo;
		if (taxDocument.IsCorrector)
		{
			taxDocument.CorrectionType = taxAccDoc.CorrectionType;
			taxDocument.BaseSeries = taxAccDoc.CorrectorNo.Left(1);
			taxDocument.BaseNumber = taxAccDoc.CorrectorNo.Mid(2);
			taxDocument.BaseSerialNumber = taxAccDoc.CorrectorNo;
			taxDocument.BaseDate = taxAccDoc.CorrectorWrittenOfAt.ToDateOnly();
		}
		taxDocument.SubmittedDoc = true;
		taxDocument.DocNum = taxAccDoc.SerialNo;
		taxDocument.AdditionalInfo = taxAccDoc.AdditionalInfo;
		taxDocument.DealNumber = taxAccDoc.DealNo;
		if (taxAccDoc.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxAccDoc.DealtAt.ToDateOnly();
		}
		taxDocument.IsSigned = taxAccDoc.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxAccDoc.ApprovedAt;
		taxDocument.DeliveryMethod = taxAccDoc.DeliveryMethod;
		taxDocument.TotalSumWithoutTaxes = taxAccDoc.TotalWithoutTaxes;
		taxDocument.TotalSum = taxAccDoc.TotalWithoutTaxes;
		taxDocument.SupplierTIN = taxAccDoc.SupplierTin;
		taxDocument.SupplierName = taxAccDoc.SupplierName;
		taxDocument.SupplierAddress = taxAccDoc.SupplierAddress;
		taxDocument.SupplierBusinessAddress = taxAccDoc.SourceFullAddress;
		taxDocument.SupplierBankName = taxAccDoc.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxAccDoc.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxAccDoc.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxAccDoc.BuyerTin;
		taxDocument.BuyerIdentityNo = taxAccDoc.BuyerIdentityNo;
		taxDocument.BuyerName = taxAccDoc.BuyerName;
		taxDocument.BuyerAddress = taxAccDoc.BuyerAddress;
		taxDocument.BuyerBusinessAddress = taxAccDoc.DestinationFullAddress;
		taxDocument.BuyerBankName = taxAccDoc.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxAccDoc.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxAccDoc.BuyerAdditionalInfo;
		foreach (AccDocGoods.Item item in taxAccDoc.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxAccDoc.AccountingDocumentType),
				ItemTypeName = Resources.Product,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Sum = item.TotalWithoutTaxes,
				DealType = (item.DealType.HasValue ? Convert.ToString(Convert.ToInt16(item.DealType)) : ""),
				CPACode = item.Classifier,
				TotalSum = item.TotalWithoutTaxes,
				RowISN = (taxAccDoc.IsCorrector ? item.ParentId.Value : item.Id)
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxAccDocToTaxDocument(AccDocTransportation taxAccDoc)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxAccDoc.IssuedAt?.ToDateOnly();
		taxDocument.SupplyDate = taxAccDoc.TransferredAt.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocTransportation.Value);
		taxDocument.Series = taxAccDoc.SerialNo.Left(1);
		taxDocument.TaxNumber = taxAccDoc.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxAccDoc.SerialNo;
		taxDocument.DocNum = taxAccDoc.SerialNo;
		taxDocument.AdditionalInfo = taxAccDoc.AdditionalInfo;
		taxDocument.DealNumber = taxAccDoc.DealNo;
		if (taxAccDoc.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxAccDoc.DealtAt.ToDateOnly();
		}
		taxDocument.SubmittedDoc = true;
		taxDocument.IsSigned = taxAccDoc.IssuedAt.HasValue;
		taxDocument.CoSignDate = taxAccDoc.IssuedAt;
		taxDocument.TotalSumWithoutTaxes = taxAccDoc.TotalWithoutTaxes;
		taxDocument.TotalSum = taxAccDoc.TotalWithoutTaxes;
		taxDocument.SupplierTIN = taxAccDoc.SupplierTin;
		taxDocument.SupplierName = taxAccDoc.SupplierName;
		taxDocument.SupplierAddress = taxAccDoc.SupplierAddress;
		taxDocument.SupplierBusinessAddress = taxAccDoc.SourceFullAddress;
		taxDocument.SupplierBankName = taxAccDoc.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxAccDoc.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxAccDoc.SupplierAdditionalInfo;
		taxDocument.DeliveryMethod = taxAccDoc.DeliveryMethod;
		foreach (AccDocTransportation.Item item in taxAccDoc.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxAccDoc.AccountingDocumentType),
				ItemTypeName = Resources.Product,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = (item.UnitPrice.HasValue ? item.UnitPrice.Value : 0m),
				Sum = item.TotalWithoutTaxes,
				TotalSum = item.TotalWithoutTaxes,
				CPACode = item.Classifier,
				RowISN = item.Id
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxAccDocToTaxDocument(AccDocServices taxAccDoc)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxAccDoc.IssuedAt?.ToDateOnly();
		taxDocument.IsCorrector = !string.IsNullOrWhiteSpace(taxAccDoc.CorrectorNo);
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocServices.Value);
		taxDocument.Series = taxAccDoc.SerialNo.Left(1);
		taxDocument.BehalfOf = Convert.ToInt16(taxAccDoc.BehalfOf);
		taxDocument.TaxNumber = taxAccDoc.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxAccDoc.SerialNo;
		taxDocument.SupplyDate = taxAccDoc.DeliveredAt.ToDateOnly();
		taxDocument.DocNum = taxAccDoc.SerialNo;
		taxDocument.AdditionalInfo = taxAccDoc.AdditionalInfo;
		taxDocument.DealNumber = taxAccDoc.DealNo;
		if (taxAccDoc.DealtAt.HasValue)
		{
			taxDocument.DealDate = taxAccDoc.DealtAt.ToDateOnly();
		}
		taxDocument.SubmittedDoc = true;
		taxDocument.IsSigned = taxAccDoc.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxAccDoc.ApprovedAt;
		taxDocument.DocDate = taxAccDoc.DeliveredAt.ToDateOnly();
		taxDocument.TotalSumWithoutTaxes = taxAccDoc.TotalWithoutTaxes;
		taxDocument.TotalSum = taxAccDoc.TotalWithoutTaxes;
		taxDocument.SupplierTIN = taxAccDoc.SupplierTin;
		taxDocument.SupplierName = taxAccDoc.SupplierName;
		taxDocument.SupplierAddress = taxAccDoc.SupplierAddress;
		taxDocument.SupplierBankName = taxAccDoc.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxAccDoc.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxAccDoc.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxAccDoc.BuyerTin;
		taxDocument.BuyerIdentityNo = taxAccDoc.BuyerIdentityNo;
		taxDocument.BuyerName = taxAccDoc.BuyerName;
		taxDocument.BuyerAddress = taxAccDoc.BuyerAddress;
		taxDocument.BuyerBankName = taxAccDoc.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxAccDoc.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxAccDoc.BuyerAdditionalInfo;
		taxDocument.BuyerHasNoTin = taxAccDoc.BuyerHasNoTin;
		if (taxDocument.IsCorrector)
		{
			taxDocument.CorrectionType = taxAccDoc.CorrectionType;
			taxDocument.BaseSeries = taxAccDoc.CorrectorNo.Left(1);
			taxDocument.BaseNumber = taxAccDoc.CorrectorNo.Mid(2);
			taxDocument.BaseSerialNumber = taxAccDoc.CorrectorNo;
			taxDocument.BaseDate = taxAccDoc.CorrectorWrittenOfAt.ToDateOnly();
		}
		foreach (AccDocServices.Item item in taxAccDoc.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxAccDoc.AccountingDocumentType),
				ItemTypeName = Resources.Service,
				ItemID = -1,
				ItemCode = "",
				ItemExternalCode = "",
				ItemNameFromDirectory = "",
				ItemName = item.Name,
				UnitID = -1,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Volume = 0m,
				Price = item.UnitPrice,
				Discount = Convert.ToDecimal(item.Discount),
				DealType = (item.DealType.HasValue ? Convert.ToString(Convert.ToInt16(item.DealType)) : ""),
				Sum = item.TotalWithoutTaxes,
				TotalSum = item.TotalWithoutTaxes,
				RowISN = (taxAccDoc.IsCorrector ? item.ParentId.Value : item.Id)
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxAccDocToTaxDocument(AccDocExportEEU taxAccDoc)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.SubMissionDate = taxAccDoc.IssuedAt?.ToDateOnly();
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocExportEEU.Value);
		taxDocument.Series = taxAccDoc.SerialNo.Left(1);
		taxDocument.TaxNumber = taxAccDoc.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxAccDoc.SerialNo;
		taxDocument.SubmittedDoc = true;
		taxDocument.IsSigned = taxAccDoc.ApprovedAt.HasValue;
		taxDocument.CoSignDate = taxAccDoc.ApprovedAt;
		taxDocument.TotalSumWithoutTaxes = taxAccDoc.TotalWithoutTaxes;
		taxDocument.TotalSum = taxAccDoc.TotalWithoutTaxes;
		taxDocument.SupplierTIN = taxAccDoc.SupplierTin;
		taxDocument.SupplierName = taxAccDoc.SupplierName;
		taxDocument.SupplierAddress = taxAccDoc.SupplierAddress;
		taxDocument.SupplierBankName = taxAccDoc.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxAccDoc.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxAccDoc.SupplierAdditionalInfo;
		taxDocument.BuyerTIN = taxAccDoc.BuyerTin;
		taxDocument.BuyerIdentityNo = taxAccDoc.BuyerIdentityNo;
		taxDocument.BuyerName = taxAccDoc.BuyerName;
		taxDocument.BuyerAddress = taxAccDoc.BuyerAddress;
		taxDocument.BuyerBankName = taxAccDoc.BuyerBank;
		taxDocument.BuyerBankAccountNumber = taxAccDoc.BuyerAccNo;
		taxDocument.BuyerAdditionalData = taxAccDoc.BuyerAdditionalInfo;
		taxDocument.DeliveryMethod = taxAccDoc.DeliveryMethod;
		foreach (AccDocExportEEU.Item item in taxAccDoc.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxAccDoc.AccountingDocumentType),
				ItemTypeName = Resources.Product,
				ItemName = item.Name,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Price = item.UnitPrice,
				Sum = item.TotalWithoutTaxes,
				TotalSum = item.TotalWithoutTaxes,
				RowISN = item.Id
			});
		}
		return taxDocument;
	}

	public TaxDocument ConvertTaxAccDocToTaxDocument(AccDocTraceableG taxAccDoc)
	{
		TaxDocument taxDocument = (TaxDocument)Settings.DocumentService.Create(DocumentTypeSE.TaxDocument.Value);
		taxDocument.FromNewInvoicing = true;
		taxDocument.Type = Convert.ToInt16(TaxAccAndInvoiceDocsTypeSE.AccDocExportEEU.Value);
		taxDocument.Series = taxAccDoc.SerialNo.Left(1);
		taxDocument.TaxNumber = taxAccDoc.SerialNo.Mid(2);
		taxDocument.SerialNumber = taxAccDoc.SerialNo;
		taxDocument.SupplyDate = taxAccDoc.DeliveredAt.ToDateOnly();
		taxDocument.IsSigned = taxAccDoc.IssuedAt.HasValue;
		taxDocument.CoSignDate = taxAccDoc.IssuedAt;
		taxDocument.SubmittedDoc = true;
		taxDocument.TotalSumWithoutTaxes = taxAccDoc.TotalWithoutTaxes;
		taxDocument.SupplierTIN = taxAccDoc.SupplierTin;
		taxDocument.SupplierName = taxAccDoc.SupplierName;
		taxDocument.SupplierAddress = taxAccDoc.SupplierAddress;
		taxDocument.SupplierBankName = taxAccDoc.SupplierBank;
		taxDocument.SupplierBankAccountNumber = taxAccDoc.SupplierAccNo;
		taxDocument.SupplierAdditionalData = taxAccDoc.SupplierAdditionalInfo;
		taxDocument.DeliveryMethod = taxAccDoc.DeliveryMethod;
		foreach (AccDocTraceableG.Item item in taxAccDoc.Items)
		{
			taxDocument.Items.Add(new TaxDocument.SpecificationRow
			{
				ItemType = Convert.ToString(taxAccDoc.AccountingDocumentType),
				ItemTypeName = Resources.Product,
				ItemName = item.Name,
				UnitBrief = item.Unit,
				Quantity = item.Quantity,
				Price = (item.UnitPrice.HasValue ? item.UnitPrice.Value : 0m),
				Sum = item.TotalWithoutTaxes,
				TotalSum = item.TotalWithoutTaxes,
				RowISN = item.Id
			});
		}
		return taxDocument;
	}

	public static AccDocGoods CreateGoodAccDoc(TaxDocument doc)
	{
		AccDocGoods accDocGoods = new AccDocGoods
		{
			BehalfOf = (BehalfOfType)((doc.BehalfOf == 0) ? 1 : doc.BehalfOf),
			Id = doc.ISN,
			CreatedAt = DateTime.Now,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			DeliveredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			SerialNo = doc.SerialNumber,
			IsCorrector = !string.IsNullOrWhiteSpace(doc.BaseSerialNumber),
			CorrectorNo = doc.BaseSerialNumber,
			CorrectorWrittenOfAt = doc.BaseDate.ToDateTime(),
			CorrectionType = ((!string.IsNullOrWhiteSpace(doc.BaseSerialNumber)) ? CorrectionType.Decreasing : CorrectionType.None),
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SourceFullAddress = doc.SourceFullAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			BuyerIdentityNo = doc.BuyerIdentityNo,
			BuyerVatTin = doc.BuyerVATNumber,
			BuyerName = doc.BuyerName,
			BuyerAddress = doc.BuyerAddress,
			BuyerAdditionalInfo = doc.BuyerAdditionalData,
			BuyerBank = doc.BuyerBankName,
			BuyerAccNo = doc.BuyerBankAccountNumber,
			BuyerSsn = doc.BuyerSocCardNum,
			DestinationFullAddress = doc.DestinationFullAddress,
			DeliveryMethod = doc.DeliveryMethod,
			BuyerHasNoTin = doc.BuyerHasNoTin,
			AuthPersonName = doc.MediatorName,
			AuthPersonPoaIssuedAt = doc.MediatorLicenseDate.ToDateTime(),
			AuthPersonPoaNo = doc.MediatorLicenseNumber,
			TotalExcTaxAmount = doc.TotalExcise,
			TotalIncEnvTax = doc.TotalEnvironmentalFee,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes - doc.TotalContainerValue,
			SourceIsCar = doc.SourceIsCar,
			SourceCarMake = doc.SourceCarMake,
			SourceCarPlateNumber = doc.SourceCarPlateNumber,
			CrmNo = doc.CrmNo,
			Items = new List<AccDocGoods.Item>()
		};
		if (doc.TotalContainerValue > 0m)
		{
			accDocGoods.TotalContainerValue = doc.TotalContainerValue;
		}
		if (!doc.BuyerHasNoTin)
		{
			if (!string.IsNullOrWhiteSpace(doc.BuyerTIN))
			{
				accDocGoods.BuyerTin = doc.BuyerTIN;
				accDocGoods.BuyerVatTin = doc.BuyerVATNumber;
				accDocGoods.BuyerIdentityType = IdentityType.None;
				accDocGoods.BuyerIdentityNo = "";
			}
			else
			{
				accDocGoods.BuyerIsNatural = true;
				accDocGoods.BuyerIdentityType = doc.BuyerIdentityType;
				accDocGoods.BuyerIdentityNo = doc.BuyerIdentityNo;
			}
		}
		else
		{
			accDocGoods.BuyerTin = doc.BuyerTIN;
			accDocGoods.BuyerIsNatural = false;
			accDocGoods.BuyerIdentityType = IdentityType.None;
			accDocGoods.BuyerIdentityNo = "";
		}
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			AccDealType? dealType = null;
			if (Enum.TryParse<AccDealType>(item.DealType, out var result))
			{
				dealType = result;
			}
			accDocGoods.Items.Add(new AccDocGoods.Item
			{
				Id = item.RowISN,
				ParentId = (doc.IsCorrector ? Settings.TaxServiceHelper.GetProductTaxISN(doc.BaseDocISN, item.ItemID, item.RowId, item.Operation) : ((Guid?)null)),
				InvoiceId = accDocGoods.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Classifier = item.CPACode,
				Quantity = item.Quantity,
				DealType = dealType,
				UnitPrice = item.Price,
				Discount = item.Discount,
				IncEnvTax = item.EnvironmentalFee,
				TotalWithoutTaxes = item.Sum - Convert.ToDecimal(item.TarePrice),
				ContainerValue = ((item.TarePrice > 0m) ? new decimal?(item.TarePrice) : ((decimal?)null))
			});
			if (item.RowId != -1)
			{
				doc.ROWISNDict.Add(item.RowId, item.RowISN);
			}
			num++;
		}
		return accDocGoods;
	}

	public static AccDocServices CreateServiceAccDoc(TaxDocument doc)
	{
		AccDocServices accDocServices = new AccDocServices
		{
			BehalfOf = (BehalfOfType)((doc.BehalfOf == 0) ? 1 : doc.BehalfOf),
			Id = doc.ISN,
			CreatedAt = DateTime.Now,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			DeliveredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			SerialNo = doc.SerialNumber,
			IsCorrector = !string.IsNullOrWhiteSpace(doc.BaseSerialNumber),
			CorrectorNo = doc.BaseSerialNumber,
			CorrectorWrittenOfAt = doc.BaseDate.ToDateTime(),
			CorrectionType = ((!string.IsNullOrWhiteSpace(doc.BaseSerialNumber)) ? CorrectionType.Decreasing : CorrectionType.None),
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			BuyerIdentityNo = doc.BuyerIdentityNo,
			BuyerName = doc.BuyerName,
			BuyerAddress = doc.BuyerAddress,
			BuyerAdditionalInfo = doc.BuyerAdditionalData,
			BuyerBank = doc.BuyerBankName,
			BuyerAccNo = doc.BuyerBankAccountNumber,
			BuyerSsn = doc.BuyerSocCardNum,
			BuyerHasNoTin = doc.BuyerHasNoTin,
			AuthPersonName = doc.MediatorName,
			AuthPersonPoaIssuedAt = doc.MediatorLicenseDate.ToDateTime(),
			AuthPersonPoaNo = doc.MediatorLicenseNumber,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes,
			CrmNo = doc.CrmNo,
			Items = new List<AccDocServices.Item>()
		};
		if (!doc.BuyerHasNoTin)
		{
			if (!string.IsNullOrWhiteSpace(doc.BuyerTIN))
			{
				accDocServices.BuyerTin = doc.BuyerTIN;
				accDocServices.BuyerVatTin = doc.BuyerVATNumber;
				accDocServices.BuyerIdentityType = IdentityType.None;
				accDocServices.BuyerIdentityNo = "";
			}
			else
			{
				accDocServices.BuyerIsNatural = true;
				accDocServices.BuyerIdentityType = doc.BuyerIdentityType;
				accDocServices.BuyerIdentityNo = doc.BuyerIdentityNo;
			}
		}
		else
		{
			accDocServices.BuyerTin = doc.BuyerTIN;
			accDocServices.BuyerIsNatural = false;
			accDocServices.BuyerIdentityType = IdentityType.None;
			accDocServices.BuyerIdentityNo = "";
		}
		if (accDocServices.BuyerIsNatural && doc.SocialExpIsRACitizen)
		{
			accDocServices.CitizenArmenia = doc.SocialExpIsRACitizen;
			accDocServices.SocialCostType = doc.SocialExpType;
			accDocServices.SocialCostReference = doc.SocialExpRACitizenSocCard;
			accDocServices.SocialCostPersonFullName = doc.SocialExpRACitizenName;
			accDocServices.SocialCostPersonDocType = doc.SocialExpRACitizenPassportType;
			accDocServices.SocialCostPersonSerialNum = doc.SocialExpRACitizenSeriaNum;
		}
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			AccDealType? dealType = null;
			if (Enum.TryParse<AccDealType>(item.DealType, out var result))
			{
				dealType = result;
			}
			accDocServices.Items.Add(new AccDocServices.Item
			{
				Id = Guid.NewGuid(),
				ParentId = (doc.IsCorrector ? Settings.TaxServiceHelper.GetProductTaxISN(doc.BaseDocISN, item.ItemID, item.RowId, item.Operation) : ((Guid?)null)),
				InvoiceId = accDocServices.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Quantity = item.Quantity,
				DealType = dealType,
				UnitPrice = item.Price,
				Discount = item.Discount,
				TotalWithoutTaxes = item.Sum
			});
			if (item.RowId != -1)
			{
				doc.ROWISNDict.Add(item.RowId, item.RowISN);
			}
			num++;
		}
		return accDocServices;
	}

	public static Good CreateGoodInvoiceDoc(TaxDocument doc)
	{
		Good good = new Good
		{
			Id = doc.ISN,
			BehalfOf = (BehalfOfType)((doc.BehalfOf == 0) ? 1 : doc.BehalfOf),
			CreatedAt = DateTime.Now,
			DeliveredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			SerialNo = doc.SerialNumber,
			IsCorrector = !string.IsNullOrWhiteSpace(doc.BaseSerialNumber),
			CorrectorNo = doc.BaseSerialNumber,
			CorrectorWrittenOfAt = doc.BaseDate.ToDateTime(),
			CorrectionType = ((!string.IsNullOrWhiteSpace(doc.BaseSerialNumber)) ? CorrectionType.Decreasing : CorrectionType.None),
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SourceFullAddress = doc.SourceFullAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			BuyerHasNoTin = doc.BuyerHasNoTin,
			BuyerVatTin = doc.BuyerVATNumber,
			BuyerName = doc.BuyerName,
			BuyerAddress = doc.BuyerAddress,
			BuyerAdditionalInfo = doc.BuyerAdditionalData,
			BuyerBank = doc.BuyerBankName,
			BuyerSsn = doc.BuyerSocCardNum,
			BuyerAccNo = doc.BuyerBankAccountNumber,
			DestinationFullAddress = doc.DestinationFullAddress,
			DeliveryMethod = doc.DeliveryMethod,
			AuthPersonName = doc.MediatorName,
			AuthPersonPoaIssuedAt = doc.MediatorLicenseDate.ToDateTime(),
			AuthPersonPoaNo = doc.MediatorLicenseNumber,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes,
			TotalVatAmount = doc.TotalVAT,
			TotalIncEnvTax = doc.TotalEnvironmentalFee,
			SourceIsCar = doc.SourceIsCar,
			SourceCarMake = doc.SourceCarMake,
			SourceCarPlateNumber = doc.SourceCarPlateNumber,
			CrmNo = doc.CrmNo,
			Total = doc.TotalSum,
			Items = new List<Good.Item>()
		};
		if (doc.TotalContainerValue > 0m)
		{
			good.TotalContainerValue = doc.TotalContainerValue;
		}
		if (!doc.BuyerHasNoTin)
		{
			if (!string.IsNullOrWhiteSpace(doc.BuyerTIN))
			{
				good.BuyerTin = doc.BuyerTIN;
				good.BuyerVatTin = doc.BuyerVATNumber;
				good.BuyerIdentityType = IdentityType.None;
				good.BuyerIdentityNo = "";
			}
			else
			{
				good.BuyerIsNatural = true;
				good.BuyerIdentityType = doc.BuyerIdentityType;
				good.BuyerIdentityNo = doc.BuyerIdentityNo;
			}
		}
		else
		{
			good.BuyerTin = doc.BuyerTIN;
			good.BuyerVatTin = doc.BuyerVATNumber;
			good.BuyerIsNatural = false;
			good.BuyerIdentityType = IdentityType.None;
			good.BuyerIdentityNo = "";
		}
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			InvoiceDealType? dealType = null;
			if (Enum.TryParse<InvoiceDealType>(item.DealType, out var result))
			{
				dealType = result;
			}
			good.Items.Add(new Good.Item
			{
				Id = item.RowISN,
				ParentId = (doc.IsCorrector ? Settings.TaxServiceHelper.GetProductTaxISN(doc.BaseDocISN, item.ItemID, item.RowId, item.Operation) : ((Guid?)null)),
				InvoiceId = good.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Classifier = item.CPACode,
				Quantity = item.Quantity,
				UnitPrice = item.Price,
				Discount = item.Discount,
				TotalWithoutTaxes = item.Sum,
				ContainerValue = ((item.TarePrice > 0m) ? new decimal?(item.TarePrice) : ((decimal?)null)),
				VatRate = ((!(item.VAT > 0m)) ? VatRate.NoVat : VatRate.Vat20),
				DealType = dealType,
				VatAmount = ((item.VAT > 0m) ? new decimal?(item.SumVAT) : ((decimal?)null)),
				Total = item.Sum + ((item.VAT > 0m) ? item.SumVAT : 0m) - item.TarePrice,
				IncEnvTax = item.EnvironmentalFee
			});
			if (item.RowId != -1)
			{
				doc.ROWISNDict.Add(item.RowId, item.RowISN);
			}
			num++;
		}
		return good;
	}

	public static Excise CreateExciseInvoiceDoc(TaxDocument doc)
	{
		Excise excise = new Excise
		{
			Id = doc.ISN,
			BehalfOf = (BehalfOfType)((doc.BehalfOf == 0) ? 1 : doc.BehalfOf),
			CreatedAt = DateTime.Now,
			DeliveredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			SerialNo = doc.SerialNumber,
			IsCorrector = !string.IsNullOrWhiteSpace(doc.BaseSerialNumber),
			CorrectorNo = doc.BaseSerialNumber,
			CorrectorWrittenOfAt = doc.BaseDate.ToDateTime(),
			CorrectionType = ((!string.IsNullOrWhiteSpace(doc.BaseSerialNumber)) ? CorrectionType.Decreasing : CorrectionType.None),
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SourceFullAddress = doc.SourceFullAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			BuyerHasNoTin = doc.BuyerHasNoTin,
			BuyerVatTin = doc.BuyerVATNumber,
			BuyerName = doc.BuyerName,
			BuyerAddress = doc.BuyerAddress,
			BuyerAdditionalInfo = doc.BuyerAdditionalData,
			BuyerBank = doc.BuyerBankName,
			BuyerSsn = doc.BuyerSocCardNum,
			BuyerAccNo = doc.BuyerBankAccountNumber,
			DestinationFullAddress = doc.DestinationFullAddress,
			DeliveryMethod = doc.DeliveryMethod,
			AuthPersonName = doc.MediatorName,
			AuthPersonPoaIssuedAt = doc.MediatorLicenseDate.ToDateTime(),
			AuthPersonPoaNo = doc.MediatorLicenseNumber,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes,
			TotalVatAmount = doc.TotalVAT,
			TotalIncEnvTax = doc.TotalEnvironmentalFee,
			TotalExcTaxAmount = doc.TotalExcise,
			CrmNo = doc.CrmNo,
			Total = doc.TotalSum,
			Items = new List<Excise.Item>()
		};
		if (doc.TotalContainerValue > 0m)
		{
			excise.TotalContainerValue = doc.TotalContainerValue;
		}
		if (!doc.BuyerHasNoTin)
		{
			if (!string.IsNullOrWhiteSpace(doc.BuyerTIN))
			{
				excise.BuyerTin = doc.BuyerTIN;
				excise.BuyerVatTin = doc.BuyerVATNumber;
				excise.BuyerIdentityType = IdentityType.None;
				excise.BuyerIdentityNo = "";
			}
			else
			{
				excise.BuyerIsNatural = true;
				excise.BuyerIdentityType = doc.BuyerIdentityType;
				excise.BuyerIdentityNo = doc.BuyerIdentityNo;
			}
		}
		else
		{
			excise.BuyerTin = doc.BuyerTIN;
			excise.BuyerVatTin = doc.BuyerVATNumber;
			excise.BuyerIsNatural = false;
			excise.BuyerIdentityType = IdentityType.None;
			excise.BuyerIdentityNo = "";
		}
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			InvoiceDealType? dealType = null;
			if (Enum.TryParse<InvoiceDealType>(item.DealType, out var result))
			{
				dealType = result;
			}
			excise.Items.Add(new Excise.Item
			{
				Id = item.RowISN,
				ParentId = (doc.IsCorrector ? Settings.TaxServiceHelper.GetProductTaxISN(doc.BaseDocISN, item.ItemID, item.RowId, item.Operation) : ((Guid?)null)),
				InvoiceId = excise.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Classifier = item.CPACode,
				Quantity = item.Quantity,
				UnitPrice = item.Price,
				Discount = item.Discount,
				VolumeLiters = item.Volume,
				TotalWithoutTaxes = item.Sum,
				ContainerValue = ((item.TarePrice > 0m) ? new decimal?(item.TarePrice) : ((decimal?)null)),
				VatRate = ((!(item.VAT > 0m)) ? VatRate.NoVat : VatRate.Vat20),
				DealType = dealType,
				VatAmount = ((item.VAT > 0m) ? new decimal?(item.SumVAT) : ((decimal?)null)),
				Total = item.Sum + ((item.VAT > 0m) ? item.SumVAT : 0m) + item.Excise - item.TarePrice,
				IncEnvTax = item.EnvironmentalFee,
				ExcTaxAmount = item.Excise,
				ExcTaxRate = item.ExciseRate
			});
			if (item.RowId != -1)
			{
				doc.ROWISNDict.Add(item.RowId, item.RowISN);
			}
			num++;
		}
		return excise;
	}

	public static Service CreateServiceInvoiceDoc(TaxDocument doc)
	{
		Service service = new Service
		{
			BehalfOf = (BehalfOfType)((doc.BehalfOf == 0) ? 1 : doc.BehalfOf),
			Id = doc.ISN,
			CreatedAt = DateTime.Now,
			DeliveredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			SerialNo = doc.SerialNumber,
			IsCorrector = !string.IsNullOrWhiteSpace(doc.BaseSerialNumber),
			CorrectorNo = doc.BaseSerialNumber,
			CorrectorWrittenOfAt = doc.BaseDate.ToDateTime(),
			CorrectionType = ((!string.IsNullOrWhiteSpace(doc.BaseSerialNumber)) ? CorrectionType.Decreasing : CorrectionType.None),
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			BuyerHasNoTin = doc.BuyerHasNoTin,
			BuyerVatTin = doc.BuyerVATNumber,
			BuyerName = doc.BuyerName,
			BuyerAddress = doc.BuyerAddress,
			BuyerSsn = doc.BuyerSocCardNum,
			BuyerAdditionalInfo = doc.BuyerAdditionalData,
			BuyerBank = doc.BuyerBankName,
			BuyerAccNo = doc.BuyerBankAccountNumber,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes,
			TotalVatAmount = doc.TotalVAT,
			AuthPersonName = doc.MediatorName,
			AuthPersonPoaIssuedAt = doc.MediatorLicenseDate.ToDateTime(),
			AuthPersonPoaNo = doc.MediatorLicenseNumber,
			CrmNo = doc.CrmNo,
			Total = doc.TotalSum,
			Items = new List<Service.Item>()
		};
		if (!doc.BuyerHasNoTin)
		{
			if (!string.IsNullOrWhiteSpace(doc.BuyerTIN))
			{
				service.BuyerTin = doc.BuyerTIN;
				service.BuyerVatTin = doc.BuyerVATNumber;
				service.BuyerIdentityType = IdentityType.None;
				service.BuyerIdentityNo = "";
			}
			else
			{
				service.BuyerIsNatural = true;
				service.BuyerIdentityType = doc.BuyerIdentityType;
				service.BuyerIdentityNo = doc.BuyerIdentityNo;
			}
		}
		else
		{
			service.BuyerTin = doc.BuyerTIN;
			service.BuyerIsNatural = false;
			service.BuyerIdentityType = IdentityType.None;
			service.BuyerIdentityNo = "";
		}
		if (service.BuyerIsNatural && doc.SocialExpIsRACitizen)
		{
			service.CitizenArmenia = doc.SocialExpIsRACitizen;
			service.SocialCostType = doc.SocialExpType;
			service.SocialCostReference = doc.SocialExpRACitizenSocCard;
			service.SocialCostPersonFullName = doc.SocialExpRACitizenName;
			service.SocialCostPersonDocType = doc.SocialExpRACitizenPassportType;
			service.SocialCostPersonSerialNum = doc.SocialExpRACitizenSeriaNum;
		}
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			InvoiceDealType? dealType = null;
			if (Enum.TryParse<InvoiceDealType>(item.DealType, out var result))
			{
				dealType = result;
			}
			service.Items.Add(new Service.Item
			{
				Id = item.RowISN,
				ParentId = (doc.IsCorrector ? Settings.TaxServiceHelper.GetProductTaxISN(doc.BaseDocISN, item.ItemID, item.RowId, item.Operation) : ((Guid?)null)),
				InvoiceId = service.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Quantity = item.Quantity,
				UnitPrice = item.Price,
				Discount = item.Discount,
				TotalWithoutTaxes = item.Sum,
				VatRate = ((!(item.VAT > 0m)) ? VatRate.NoVat : VatRate.Vat20),
				VatAmount = ((item.VAT > 0m) ? new decimal?(item.SumVAT) : ((decimal?)null)),
				DealType = dealType,
				Total = item.Sum + ((item.VAT > 0m) ? item.SumVAT : 0m)
			});
			if (item.RowId != -1)
			{
				doc.ROWISNDict.Add(item.RowId, item.RowISN);
			}
			num++;
		}
		return service;
	}

	public static AccDocTransportation CreateTransportationAccDoc(TaxDocument doc)
	{
		AccDocTransportation accDocTransportation = new AccDocTransportation
		{
			Id = doc.ISN,
			CreatedAt = DateTime.Now,
			DealtAt = doc.DealDate.ToDateTime(),
			DealNo = doc.DealNumber,
			TransferredAt = Convert.ToDateTime(doc.SupplyDate.ToDateTime()),
			AdditionalInfo = doc.AdditionalInfo,
			SerialNo = doc.SerialNumber,
			SupplierTin = doc.SupplierTIN,
			SupplierVatTin = doc.SupplierVATNumber,
			SupplierName = doc.SupplierName,
			SupplierAddress = doc.SupplierAddress,
			SupplierAdditionalInfo = doc.SupplierAdditionalData,
			SupplierBank = doc.SupplierBankName,
			SupplierAccNo = doc.SupplierBankAccountNumber,
			SourceFullAddress = doc.SourceFullAddress,
			DestinationFullAddress = doc.DestinationFullAddress,
			DeliveryMethod = doc.DeliveryMethod,
			TotalWithoutTaxes = doc.TotalSumWithoutTaxes,
			SourceIsCar = doc.SourceIsCar,
			SourceCarMake = doc.SourceCarMake,
			SourceCarPlateNumber = doc.SourceCarPlateNumber,
			Items = new List<AccDocTransportation.Item>()
		};
		short num = 0;
		foreach (TaxDocument.SpecificationRow item in doc.Items)
		{
			accDocTransportation.Items.Add(new AccDocTransportation.Item
			{
				Id = ((item.RowISN == Guid.Empty) ? Guid.NewGuid() : item.RowISN),
				InvoiceId = accDocTransportation.Id,
				SeqNo = num,
				Name = item.ItemName,
				Unit = item.UnitBrief,
				Classifier = item.CPACode,
				Quantity = item.Quantity,
				UnitPrice = item.Price,
				Discount = item.Discount,
				TotalWithoutTaxes = item.Sum
			});
			num++;
		}
		return accDocTransportation;
	}

	public static AssignTaxInvNumsRepRow BuildResultRow(TaxServiceItems item)
	{
		return new AssignTaxInvNumsRepRow
		{
			Date = item.InvoiceDetails.Date,
			TIN = item.InvoiceDetails.TaxCode?.Trim(),
			Sum = item.InvoiceDetails.Sum,
			Assigned = Convert.ToString(item.IsAssigned.ToYesNo() + " "),
			SeralNum_NotAssignReason = item.SerialNumReason,
			DocNumbers = item.DocNumbers
		};
	}

	public static TaxDocumentRepRow BuildResultRow(bool imported, string serialNumReason)
	{
		return new TaxDocumentRepRow
		{
			Imported = imported,
			SeralNum_Reason = serialNumReason
		};
	}

	public TaxServiceResponse MapTaxDocuments(List<TaxDocument> taxDocuments, DateTime minDate, DateTime maxDate, bool assignTaxNumToDocNum)
	{
		return Client.TaxService.MapTaxDocuments(taxDocuments, minDate, maxDate, assignTaxNumToDocNum);
	}

	public virtual TaxDocumentRepRow AssignTaxDocumentData(Guid isn, string taxSeriaNumber, DateOnly? submitDate, decimal total = 0m, bool assignTaxNumToDocNum = false, short docType = 0)
	{
		return Client.TaxService.AssignTaxDocumentData(isn, taxSeriaNumber, submitDate, total, assignTaxNumToDocNum, docType);
	}

	public virtual TaxServiceResponse MapReceivedTaxDocuments(List<TaxDocument> taxDocuments)
	{
		return Client.TaxService.MapReceivedTaxDocuments(taxDocuments);
	}

	public virtual List<TaxDocumentRepRow> ImportTaxDocuments(List<TaxDocument> taxDocuments)
	{
		return Client.TaxService.ImportTaxDocuments(taxDocuments);
	}

	public virtual TaxServiceResponse FindMTMoveDocuments(List<AccDoc> taxDocuments, bool assignTaxNumToDocNum)
	{
		return Client.TaxService.FindMTMoveDocuments(taxDocuments, assignTaxNumToDocNum);
	}

	public Guid? GetProductTaxISN(Guid? docISN, int itemId, int rowId = -1, string operation = null)
	{
		return Client.TaxService.GetProductTaxISN(docISN, itemId, rowId, operation);
	}

	public static (DateOnly minDate, DateOnly maxDate) GetTaxInvoicesMinAndMaxDates(List<TaxDocument> taxInvoices)
	{
		return (minDate: taxInvoices.Min((TaxDocument p) => p.SupplyDate).Value, maxDate: taxInvoices.Max((TaxDocument p) => p.SupplyDate).Value);
	}

	public static List<TaxDocument> LoadTaxDocuments(string xmlFileName)
	{
		Settings.TaxService.Authenticate();
		TaxDocType xMLTaxType = Settings.TaxService.Client.Dispatcher.GetXMLTaxType(xmlFileName);
		List<TaxDocument> list = new List<TaxDocument>();
		switch (xMLTaxType)
		{
		case TaxDocType.Invoice:
			foreach (InvoiceBase item in Settings.TaxService.Client.Dispatcher.GetInvoicesFromXML(xmlFileName))
			{
				if (item.InvoiceType == XMLInvoiceDocType.Goods)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument((Good)item));
				}
				else if (item.InvoiceType == XMLInvoiceDocType.Excise)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument((Excise)item));
				}
				else if (item.InvoiceType == XMLInvoiceDocType.Services)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument((Service)item));
				}
				else if (item.InvoiceType == XMLInvoiceDocType.Leasing)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxInvoiceToTaxDocument((Leasing)item));
				}
			}
			break;
		case TaxDocType.AccDoc:
			foreach (AccDocBase item2 in Settings.TaxService.Client.Dispatcher.GetAccDocsFromXML(xmlFileName))
			{
				if (item2.AccountingDocumentType == XMLAccDocType.AccDocGoods)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument((AccDocGoods)item2));
				}
				else if (item2.AccountingDocumentType == XMLAccDocType.AccDocServices)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument((AccDocServices)item2));
				}
				else if (item2.AccountingDocumentType == XMLAccDocType.AccDocTransportation)
				{
					list.Add(Settings.TaxServiceHelper.ConvertTaxAccDocToTaxDocument((AccDocTransportation)item2));
				}
			}
			break;
		}
		return list;
	}
}
