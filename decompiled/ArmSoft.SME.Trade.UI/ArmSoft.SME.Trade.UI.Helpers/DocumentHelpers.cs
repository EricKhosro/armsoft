using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Client.Models.b2b;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Helpers;
using ArmSoft.SME.Trade.Common.Models;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.Directory;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Themes;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class DocumentHelpers
{
	public static string GetDocumentDescription(TradeDocument doc, ArmSoft.SME.Common.Constants.Common.DocumentDescriptionMode mode, string ecrCheckNum = "")
	{
		if (doc == null)
		{
			return string.Empty;
		}
		DefinitionMappingsService.TryResolveDocumentDefinition(doc.POType, out var definition);
		return ArmSoft.SME.Trade.Common.Helpers.DocumentHelpers.ConfigureDocumentDescription(mode, definition.Caption, doc.DocNum, doc.DocDate, doc.Summ.Value, doc.DocOrderNum, ecrCheckNum);
	}

	public static MaterialCacheRow TryGetItemByBarCode(ref string barCode, out decimal qty, out string unitCode, out string unitBrief)
	{
		unitCode = string.Empty;
		unitBrief = string.Empty;
		string text = "";
		qty = 1m;
		MaterialCacheRow materialCacheRow = null;
		if (WeightItemInfo.GetInstance().IsValidBarCode(barCode))
		{
			qty = WeightItemInfo.GetInstance().GetQuantity(barCode);
			text = WeightItemInfo.GetInstance().GetCode(barCode);
		}
		else if (PieceItemInfo.GetInstance().IsValidBarCode(barCode))
		{
			qty = PieceItemInfo.GetInstance().GetQuantity(barCode);
			text = PieceItemInfo.GetInstance().GetCode(barCode);
		}
		if (!string.IsNullOrWhiteSpace(text))
		{
			materialCacheRow = GetItemByBarCodeDependingOnWeightedFlag(checkForWeighted: true, text, out unitCode, out unitBrief);
		}
		if (materialCacheRow != null)
		{
			barCode = text;
			return materialCacheRow;
		}
		qty = 1m;
		return GetItemByBarCodeDependingOnWeightedFlag(checkForWeighted: false, barCode, out unitCode, out unitBrief);
	}

	private static MaterialCacheRow GetItemByBarCodeDependingOnWeightedFlag(bool checkForWeighted, string barCode, out string unitCode, out string unitBrief)
	{
		unitCode = string.Empty;
		unitBrief = string.Empty;
		MaterialCacheRow itemByBarCode = TradeSettings.TradeCacheService.Material.GetItemByBarCode(barCode, out unitCode);
		if (itemByBarCode == null)
		{
			return null;
		}
		unitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(unitCode).Brief;
		if (checkForWeighted && !itemByBarCode.IsWeight)
		{
			return null;
		}
		return itemByBarCode;
	}

	public static bool IsValidPartnerEmployeeCardCode(string partnerEmployeeCardCode, string partnerEmployeeCardCodePrefixes, bool usePartnerCardCodeLenght, int partnerCardCodeLenght)
	{
		if (string.IsNullOrWhiteSpace(partnerEmployeeCardCodePrefixes))
		{
			if (usePartnerCardCodeLenght && partnerCardCodeLenght != partnerEmployeeCardCode.Length)
			{
				return false;
			}
			return true;
		}
		if (!usePartnerCardCodeLenght || partnerCardCodeLenght == partnerEmployeeCardCode.Length)
		{
			string[] array = partnerEmployeeCardCodePrefixes.Split(new string[1] { "||" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string value in array)
			{
				if (partnerEmployeeCardCode.StartsWith(value))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool GetPartnerEmployeeDataByBarCode(string cardNumber, out PartnerCacheRow partnerCacheRow, out PartnerCacheRow.ContractRow partnerContract, out EmployeeCacheRow employee, string currCode = null, bool hideExceptions = false)
	{
		partnerCacheRow = null;
		partnerContract = null;
		employee = null;
		if (!string.IsNullOrWhiteSpace(cardNumber))
		{
			partnerContract = TradeSettings.TradeCacheService.Partner.GetContractByDiscountCard(cardNumber, out partnerCacheRow);
			if (partnerContract == null)
			{
				employee = TradeSettings.TradeCacheService.Employee.GetEmployeeByCard(cardNumber);
				if (employee == null)
				{
					if (!hideExceptions)
					{
						throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.ValidCardNotExist, cardNumber));
					}
					return false;
				}
			}
			else if (!string.IsNullOrWhiteSpace(currCode) && partnerContract.CurrCode != currCode)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.CardCurrCodeIsInvalid, cardNumber, partnerContract.CurrCode));
			}
		}
		return true;
	}

	public static string GetNextMaterialCode()
	{
		string text = Settings.ParametersService.NextMaterial();
		int num = 100;
		while (TradeSettings.TradeCacheService.Material.GetItem(text) != null && num > 0)
		{
			text = StringExtensions.NextDocNum(text, 20);
			num--;
		}
		return text;
	}

	public static void ShowASInfoInGrid(DocumentGridControl gridControl)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		ControlTemplate controlTemplate = new ControlTemplate(typeof(DataPresenter));
		FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(Border))
		{
			Name = "Border"
		};
		frameworkElementFactory.SetValue(Border.BackgroundProperty, new ImageBrush(ImageService.GetImage("Bill_Background_AM.svg"))
		{
			Stretch = Stretch.None,
			AlignmentY = AlignmentY.Bottom
		});
		frameworkElementFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
		controlTemplate.VisualTree = frameworkElementFactory;
		TableViewThemeKeyExtension key = new TableViewThemeKeyExtension
		{
			IsThemeIndependent = true,
			ResourceKey = TableViewThemeKeys.DataPresenterTemplate
		};
		gridControl.Grid.Resources.Add(key, controlTemplate);
		MultiDataTrigger multiDataTrigger = new MultiDataTrigger();
		Condition item = new Condition
		{
			Binding = new Binding("SelectionState"),
			Value = "None"
		};
		Condition item2 = new Condition
		{
			Binding = new Binding
			{
				Path = new PropertyPath(BaseEdit.HasValidationErrorProperty)
			},
			Value = true
		};
		multiDataTrigger.Conditions.Add(item);
		multiDataTrigger.Conditions.Add(item2);
		Setter item3 = new Setter
		{
			Property = Control.BackgroundProperty,
			Value = (Settings.IsDarkTheme ? ((Brush)new BrushConverter().ConvertFromString("#FF5C514D")) : ((Brush)new BrushConverter().ConvertFromString("#FFD9D9")))
		};
		multiDataTrigger.Setters.Add(item3);
		gridControl.View.RowStyle.Triggers.Add(multiDataTrigger);
	}

	public static List<TerminalRow> GetSourceList()
	{
		List<TerminalRow> list = new List<TerminalRow>();
		foreach (MaterialCacheRow item2 in TradeSettings.TradeCacheService.Material.GetList((MaterialCacheRow x) => x.Type == GoodsTypesSE.Good.Value))
		{
			TerminalRow item = new TerminalRow
			{
				Id = item2.Id,
				Code = item2.MTCode,
				Name = item2.Caption,
				Unit = item2.Unit,
				AlternativeUnit = item2.AltUnit,
				BarCode = null,
				Quantity = 1
			};
			list.Add(item);
		}
		return list;
	}

	public static object CreatePaymentOrderDoc()
	{
		PayOrder payOrder = Settings.DocumentService.Create<PayOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.PayOrd);
		payOrder.DocDate = Settings.HostingEnvironment.CurrentDate;
		payOrder.CurrCode = "AMD";
		payOrder.Summ = Settings.LoginService.ContractNexPayment;
		payOrder.SummNCC = payOrder.Summ;
		payOrder.PartnerContractGroup.SummaPart = 0m;
		payOrder.ReceiverTaxCode = "00807555";
		payOrder.ReceiverLegalStatus = ReceiverLegalStatusTypeSE.LegalEntity.Value;
		payOrder.PaymentAim = Settings.LoginService.Key;
		using (CloudClient cloudClient = Settings.CloudClient)
		{
			PaymentDetails paymentDetails = cloudClient.AS7X.GetPaymentDetails(1);
			if (paymentDetails != null)
			{
				payOrder.ReceiverSettlAcc = paymentDetails.BankAccount;
				payOrder.ReceiverName = paymentDetails.CompanyName;
			}
		}
		PartnerCacheRow partnerCacheRow = TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode("00807555").FirstOrDefault();
		if (partnerCacheRow != null)
		{
			payOrder.PartnerContractGroup.PartnerCode = partnerCacheRow.PartCode;
			payOrder.PartnerID = partnerCacheRow.Id;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(partnerCacheRow);
			if (defaultContract != null)
			{
				payOrder.PartnerContractGroup.ContractCode = defaultContract.Contract;
			}
		}
		Settings.DocumentService.CreateWindow(payOrder).Show();
		return true;
	}

	public static string CreateNewGoodFromUnitedProdAndAddInDoc(string barcode, Window owner)
	{
		string result = string.Empty;
		try
		{
			b2bUnitedProductRowBase goodFromBarCode = BarcodeFromb2bCloudDialogWindow.GetGoodFromBarCode(barcode);
			if (goodFromBarCode == null)
			{
				throw new ASException(ArmSoft.SME.Common.Resources.ProductWithBarCodeDoesNotExistInB2B);
			}
			POWindow<Good> pOWindow = ((!Settings.ParametersService.GoodAddSimpleMode()) ? ((DirectoryDocumentWindow<Good>)new GoodDocumentWindow()) : ((DirectoryDocumentWindow<Good>)new GoodDocumentSimpleWindow()));
			Good good = Settings.DirectoryService.Create<Good>(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Goods.Value);
			pOWindow.Owner = owner;
			good.Name = goodFromBarCode.Name;
			good.FullName = goodFromBarCode.FullName;
			good.Unit = "001";
			good.AltUnit = good.Unit;
			good.CPACode = goodFromBarCode.CPACode;
			good.VAT = goodFromBarCode.IsVatTaxable;
			good.IsWeight = false;
			good.BarCode = goodFromBarCode.Barcode;
			pOWindow.PO = good;
			if (pOWindow.ShowDialog().Value)
			{
				result = pOWindow.PO.Code;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return result;
	}

	public static string FormNegativeQuantityMessage(decimal quantity, string storage, string mtCode, DateOnly docDate, bool checkSubstitutions, bool enableSubstitudedItemsParam)
	{
		string mTEgineInavidNegativeQTY = ArmSoft.SME.Trade.Common.Resources.MTEgineInavidNegativeQTY;
		InlineArray4<object> buffer = default;
		buffer[0] = quantity;
		buffer[1] = storage;
		buffer[2] = mtCode;
		buffer[3] = docDate.FormatDDMMYY();
		string text = string.Format(mTEgineInavidNegativeQTY, (ReadOnlySpan<object?>)buffer);
		if ((checkSubstitutions & enableSubstitudedItemsParam) && TradeSettings.TradeCacheService.Material.HasSubstitution(mtCode))
		{
			text = $"{text}{Environment.NewLine}{ArmSoft.SME.Trade.Common.Resources.SubstitutionsExists}";
		}
		return text;
	}
}
