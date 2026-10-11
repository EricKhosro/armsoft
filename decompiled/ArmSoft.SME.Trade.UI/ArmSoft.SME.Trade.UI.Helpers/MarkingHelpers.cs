using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Documents;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Core.UI.StandartWindow;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class MarkingHelpers
{
	public static bool IsGoodMarkable(string itemCode, bool useMarking, bool markingListHasValue, DateOnly docDate, out bool isRowMarkable)
	{
		bool result = false;
		isRowMarkable = false;
		if (markingListHasValue)
		{
			result = true;
		}
		MaterialCacheRow item = TradeSettings.TradeCacheService.Material.GetItem(itemCode);
		if (item == null)
		{
			return result;
		}
		CPACodesCacheRow item2 = Settings.CoreCacheService.CPACode.GetItem(item.CPACode);
		if (item2 != null && item2.MarkInitDate.HasValue)
		{
			DateOnly? markInitDate = item2.MarkInitDate;
			if (!(docDate < markInitDate))
			{
				if (useMarking)
				{
					isRowMarkable = true;
					result = true;
				}
				return result;
			}
		}
		return result;
	}

	public static string ExtractGTINFromDataMatrix(string dataMatrixCode, out MaterialCacheRow good)
	{
		string str = ((!dataMatrixCode.StartsWith("01") || !dataMatrixCode.Substring(0, 16).IsNumeric()) ? dataMatrixCode.Substring(0, 14) : dataMatrixCode.Substring(2, 14));
		string text = "";
		str = str.Right(13);
		good = DocumentHelpers.TryGetItemByBarCode(ref str, out var qty, out var unitCode, out var unitBrief);
		if (good == null)
		{
			text = str;
			str = str.Right(12);
			good = DocumentHelpers.TryGetItemByBarCode(ref str, out qty, out unitBrief, out unitCode);
			if (good == null)
			{
				text = text + ", " + str;
				str = str.Right(8);
				good = DocumentHelpers.TryGetItemByBarCode(ref str, out qty, out unitCode, out unitBrief);
			}
		}
		if (good == null)
		{
			return text = text + ", " + str;
		}
		return str;
	}

	public static void ExportEMarkReceipts(Window owner, bool viewMode = false, bool forPOS = false, string storage = "")
	{
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = Settings.ParametersService.EMarkReceiptDevice();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.UserFieldIsEmpty, Settings.LoginService.UserName, ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDevice));
			}
			MarkingSelectWindow markingSelectWindow = new MarkingSelectWindow(owner, viewMode, isRowMarkable: true, "", "", new HashSet<string>(), new List<string>(), 0m, isReturn: false, eMarkReceipt: true, forPOS);
			if (!markingSelectWindow.ShowDialog().Value || markingSelectWindow.IsViewOnlyMode || !markingSelectWindow.MarkingList.Any())
			{
				return;
			}
			EMarkReceipt eMarkReceipt = Settings.DocumentService.Create<EMarkReceipt>(DocumentTypeSE.EMarkReceipt.Value);
			eMarkReceipt.DocDate = Settings.HostingEnvironment.CurrentDate;
			eMarkReceipt.DocNum = Settings.ParametersService.NextEMarkReceiptDoc();
			eMarkReceipt.Owner = Settings.LoginService.UserName;
			eMarkReceipt.OwnerID = Settings.LoginService.UserId;
			eMarkReceipt.Storage = ((!string.IsNullOrWhiteSpace(storage)) ? storage : Settings.ParametersService.DefaultStorage());
			int num = 0;
			foreach (string item in markingSelectWindow.MarkingList.Select((MarkingSelectWindow.MarkingRow r) => r.Marking).ToList())
			{
				string text2 = ExtractGTINFromDataMatrix(item, out var good);
				if (good == null)
				{
					throw new ASException(string.Format(ArmSoft.SME.Common.Resources.CodeNotExistDir, text2, ArmSoft.SME.Common.Resources.BarCodes));
				}
				TradeSettings.TradeCacheService.Material.GetItemByBarCode(text2, out var qntUnit, throwExceptionNotExits: true);
				EMarkReceipt.EMarkReceiptSpecificationRow eMarkReceiptSpecificationRow = new EMarkReceipt.EMarkReceiptSpecificationRow
				{
					Barcode = ((item.StartsWith("01") && item.Substring(0, 16).IsNumeric()) ? item.Substring(2, 14) : item.Substring(0, 14)),
					Id = good.Id,
					Code = good.MTCode,
					Name = good.Caption,
					UnitCode = qntUnit,
					UnitBrief = TradeSettings.TradeCacheService.QntUnit.GetItem(qntUnit, throwExceptionNotExits: true).Brief,
					CPACode = good.CPACode,
					MarkingList = new HashSet<string> { item },
					RowId = num
				};
				eMarkReceiptSpecificationRow.Quantity = eMarkReceiptSpecificationRow.MarkingList.Count;
				eMarkReceipt.MTList.Add(eMarkReceiptSpecificationRow);
				num++;
			}
			eMarkReceipt.MaxRowId = num;
			Settings.DocumentService.Store(eMarkReceipt, new StoreRequest
			{
				WithoutAccessCheck = true
			});
			Settings.ParametersService.NextEMarkReceiptDoc_set(StringExtensions.NextDocNum(eMarkReceipt.DocNum, 12));
			EMarkReceiptApp eMarkReceiptApp = Settings.DirectoryService.Load<EMarkReceiptApp>(DirectoryTypeSE.EMarkReceipt.Value, text, withoutAccessCheck: true);
			EMarkApiClient eMarkApiClient = new EMarkApiClient(new EMarkOptions
			{
				Token = eMarkReceiptApp.Token,
				RefreshToken = eMarkReceiptApp.RefreshToken,
				CRN = eMarkReceiptApp.CRN,
				Datamatrix_file_id = eMarkReceiptApp.DeviceId
			});
			PrintRequest printRequest = new PrintRequest
			{
				receipt = new List<EMarkReceiptItem>()
			};
			foreach (EMarkReceipt.EMarkReceiptSpecificationRow mT in eMarkReceipt.MTList)
			{
				printRequest.receipt.Add(new EMarkReceiptItem
				{
					gtin = mT.Barcode,
					product_name = mT.Name,
					measurement_unit = mT.UnitBrief,
					quantity = Convert.ToInt32(mT.Quantity),
					tnved = mT.CPACode,
					emarks = mT.MarkingList.ToList()
				});
			}
			PrintResponse printResponse = eMarkApiClient.Print(printRequest);
			ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.EMarkReceiptDoc,
				HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification)
			};
			toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
			if (printResponse.succeed)
			{
				IDocumentService documentService = Settings.DocumentService;
				Guid iSN = eMarkReceipt.ISN;
				int num2 = 1;
				List<PropertyWithValue> list = new List<PropertyWithValue>(num2);
				CollectionsMarshal.SetCount(list, num2);
				CollectionsMarshal.AsSpan(list)[0] = new PropertyWithValue
				{
					Name = "EMarkReceiptNumber",
					Value = printResponse.receipt_id.ToString()
				};
				documentService.EditFields(iSN, list, new StoreRequest
				{
					WithoutAccessCheck = true,
					LogComment = ArmSoft.SME.Common.Resources.VatExport + " " + ArmSoft.SME.Trade.Common.Resources.EMarkReceiptApp + " - N " + printResponse.receipt_id
				});
				toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.OperationSucceeded + Environment.NewLine + ArmSoft.SME.Trade.Common.Resources.EMarkReceiptNumber + printResponse.receipt_id;
				toastNotificationViewModel.Image = ImageService.GetImage("eMarkAccept_32.svg");
				toastNotificationViewModel.HistoryItem.Icon = "eMarkAccept_16.svg";
			}
			else
			{
				toastNotificationViewModel.Text = ArmSoft.SME.Common.Resources.OperationNotSucceededTryAgain + Environment.NewLine + ArmSoft.SME.Common.Resources.DocNumberShort + " " + eMarkReceipt.DocNum;
				toastNotificationViewModel.Image = ImageService.GetImage("eMarkError_32.svg");
				toastNotificationViewModel.HistoryItem.Icon = "eMarkError_16.svg";
			}
			toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
			Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
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
}
