using System;
using System.Text.Json;
using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.StandartWindow;
using ArmSoft.SME.Trade.Client.Routes;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class CashierShiftHelper
{
	public static bool OpenCashierShift()
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = Settings.ParametersService.DefaultCash();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.FillDefaultCashDeskForOpeningCashierShift);
			}
			DateTime serverDate = TradeSettings.CreateTradeClient().CommonServices.GetServerDate();
			SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Cash, text, -1, "AMD", serverDate.ToDateOnly());
			if (ArmSoft.SME.Core.UI.MessageBox.Show("\r\n" + ArmSoft.SME.Trade.Common.Resources.ConfirmCashierShiftOpening + "\r\n\r\n" + string.Format(ArmSoft.SME.Trade.Common.Resources.CashInCashierShiftInCashDesk, text, accRem.SummNcc.CurrencyFormat(16, 2)) + " " + ArmSoft.SME.Common.Resources.DramWordSmallLetter, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(text, throwExceptionNotExits: true);
				if (string.IsNullOrWhiteSpace(item.NextCashierShiftNum))
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.FillCashierShiftNextNumber, text + " - " + item.Caption));
				}
				CashierShift cashierShift = Settings.DirectoryService.Create<CashierShift>(DirectoryTypeSE.CashierShift.Value);
				cashierShift.CashDesk = text;
				cashierShift.ShiftNumber = item.NextCashierShiftNum;
				cashierShift.StartDate = serverDate;
				cashierShift.InitialRemain = accRem.SummNcc;
				Settings.DirectoryService.Store(cashierShift, new DirStoreRequest
				{
					WithoutAccessCheck = true
				});
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
				{
					Caption = ArmSoft.SME.Trade.Common.Resources.CashierShift,
					HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
					Image = ImageService.GetImage("accept_32.svg"),
					Text = string.Format(ArmSoft.SME.Trade.Common.Resources.CashierShiftWasOpenedSuccessfully, cashierShift.CashDesk + " - " + item.Caption, cashierShift.ShiftNumber)
				};
				toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
				toastNotificationViewModel.HistoryItem.Type = 101;
				toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
				toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
				Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return false;
		}
	}

	public static bool CloseCashierShift()
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = Settings.ParametersService.DefaultCash();
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new ASException(ArmSoft.SME.Trade.Common.Resources.FillDefaultCashDeskForOpeningCashierShift);
			}
			DateTime serverDate = TradeSettings.CreateTradeClient().CommonServices.GetServerDate();
			SummCurrSummNccPair accRem = TradeSettings.CreateTradeClient().CommonServices.GetAccRem(CommonConst.AccountingsTypes.Cash, text, -1, "AMD", serverDate.ToDateOnly());
			if (ArmSoft.SME.Core.UI.MessageBox.Show("\r\n" + ArmSoft.SME.Trade.Common.Resources.ConfirmCashierShiftClosing + "\r\n\r\n" + string.Format(ArmSoft.SME.Trade.Common.Resources.CashInEndOfCashierShiftInCashDesk, text, accRem.SummNcc.CurrencyFormat(16, 2)) + " " + ArmSoft.SME.Common.Resources.DramWordSmallLetter, Settings.ProgramName, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				string openCashierShiftNumber = GetOpenCashierShiftNumber();
				CashCacheRow item = TradeSettings.TradeCacheService.Cash.GetItem(text, throwExceptionNotExits: true);
				if (string.IsNullOrWhiteSpace(openCashierShiftNumber))
				{
					throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.CashierShiftNotExistWithDefaultCashdesk, text + " - " + item.Caption));
				}
				if (Settings.ParametersService.NotAllowCloseCashierShiftWhenHavePostponedDocs() && TradeSettings.CreateTradeClient().CommonServices.ExistPostponedSaleDocsWithCashierShift(text, openCashierShiftNumber))
				{
					throw new ASException(ArmSoft.SME.Trade.Common.Resources.ExistPostponedSaleDocs);
				}
				string code = JsonSerializer.Serialize(new CashierShiftCacheRow.RowKey(text, openCashierShiftNumber));
				CashierShift cashierShift = Settings.DirectoryService.Load<CashierShift>(DirectoryTypeSE.CashierShift.Value, code, withoutAccessCheck: true);
				cashierShift.EndDate = serverDate;
				Settings.DirectoryService.Store(cashierShift, new DirStoreRequest
				{
					WithoutAccessCheck = true
				});
				ToastNotificationViewModel toastNotificationViewModel = new ToastNotificationViewModel
				{
					Caption = ArmSoft.SME.Trade.Common.Resources.CashierShift,
					HistoryItem = new HistoryItem(HistoryItem.ItemCategory.Notification),
					Image = ImageService.GetImage("accept_32.svg"),
					Text = string.Format(ArmSoft.SME.Trade.Common.Resources.CashierShiftWasClosedSuccessfully, cashierShift.CashDesk + " - " + item.Caption, cashierShift.ShiftNumber)
				};
				toastNotificationViewModel.HistoryItem.Icon = "accept_16.svg";
				toastNotificationViewModel.HistoryItem.Type = 101;
				toastNotificationViewModel.HistoryItem.Description = toastNotificationViewModel.Caption;
				toastNotificationViewModel.HistoryItem.Details = toastNotificationViewModel.Text;
				Settings.HostingEnvironment.ShowNotificationAsync(toastNotificationViewModel);
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
			return false;
		}
	}

	public static bool ExistsOpenedCashierShift()
	{
		return !string.IsNullOrWhiteSpace(GetOpenCashierShiftNumber());
	}

	public static string GetOpenCashierShiftNumber()
	{
		if (Settings.IsOffline)
		{
			return TradeSettings.LocalDBService.OpenCashierShiftNumber;
		}
		string openCashierShiftNumberByCashDesk = TradeSettings.CreateTradeClient().CommonServices.GetOpenCashierShiftNumberByCashDesk(Settings.ParametersService.DefaultCash());
		TradeSettings.LocalDBService.OpenCashierShiftNumber = openCashierShiftNumberByCashDesk;
		return openCashierShiftNumberByCashDesk;
	}

	public static DateTime? GetOpenCashierShiftBeginDate(string currentShiftNum)
	{
		if (Settings.IsOffline)
		{
			return TradeSettings.LocalDBService.OpenCashierShiftBeginDate;
		}
		DateTime? dateTime = TradeSettings.TradeCacheService.CashierShift.GetItem(new CashierShiftCacheRow.RowKey(Settings.ParametersService.DefaultCash(), currentShiftNum))?.StartDate;
		TradeSettings.LocalDBService.OpenCashierShiftBeginDate = dateTime;
		return dateTime;
	}
}
