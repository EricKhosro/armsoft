using System;
using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.DataViews.Directories;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.DropDownsData;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class SynchronizationsProcessorBase
{
	protected int _userId;

	public int NotificationsCheckInterval { get; set; } = 180;

	public int NotificationsCheckPreviousDays { get; set; }

	public bool IsCheckedServerDate { get; set; }

	public bool IsCheckedUpdateVersion { get; set; }

	public bool ImportExchangeRate { get; set; } = Settings.ParametersService.ImportExchangeRates();

	public bool ExchnageRatesChecked { get; set; }

	public DateOnly? CPACodesLastSyncDate { get; set; } = Settings.ParametersService.CPACodesLastSyncDate();

	public bool CPACodesUpdatedChecked { get; set; }

	public DateOnly? BanksLastSyncDate { get; set; } = Settings.ParametersService.BanksLastSyncDate();

	public bool BanksUpdatedChecked { get; set; }

	public bool TaxServiceAuthenticationDone { get; set; }

	public SynchronizationsProcessorBase()
	{
	}

	public SynchronizationsProcessorBase(int userId)
	{
		_userId = userId;
	}

	public DateTime StartDate()
	{
		return DateTime.Today.AddDays(NotificationsCheckPreviousDays);
	}

	public virtual bool IsSynchronizationsEnabled()
	{
		return true;
	}

	public static short TryImportExchangeRates(DateOnly startDate, DateOnly endDate, ref string message)
	{
		short num = 0;
		try
		{
			using (Settings.CBAExchangeRatesClient)
			{
				List<string> list = (from x in new AvailableCurrenciesDataProvider().GetRows(new AvailableCurrenciesDataProvider.Param())
					select x.Curr).ToList();
				List<RatesOfExchangeDataProvider.DataRow> exchangeRates = ExchangeRatesClient.GetExchangeRates(startDate, endDate, list);
				List<RatesOfExchangeDataProvider.DataRow> rows = new RatesOfExchangeDataProvider(Settings.CreateApiClient()).GetRows(new RatesOfExchangeDataProvider.Param
				{
					Currencies = string.Join(",", list),
					StartDate = startDate,
					EndDate = endDate
				}, withoutAccessCheck: true);
				try
				{
					foreach (RatesOfExchangeDataProvider.DataRow cbaRate in exchangeRates)
					{
						RatesOfExchangeDataProvider.DataRow dataRow = rows.FirstOrDefault((RatesOfExchangeDataProvider.DataRow row) => row.Curr == cbaRate.Curr && row.Date == cbaRate.Date);
						if (dataRow == null)
						{
							Settings.DirectoryService.Store(new ArmSoft.SME.Common.Models.Directories.RatesOfExchange
							{
								Date = cbaRate.Date,
								Code = cbaRate.Curr,
								Rate = cbaRate.Rate,
								Base = cbaRate.Base
							}, new DirStoreRequest
							{
								WithoutAccessCheck = true
							}, withoutCacheRefresh: true);
							if (num == 0)
							{
								num = 2;
							}
						}
						else if (dataRow.Base != cbaRate.Base || dataRow.Rate != cbaRate.Rate)
						{
							num = 1;
						}
					}
				}
				catch (Exception ex)
				{
					num = -1;
					message = ex.Message;
				}
				finally
				{
					Settings.DirectoryService.RefreshDirectoryUICaches(DirectoryTypeSE.RatesOfExchange);
				}
			}
		}
		catch (Exception ex2)
		{
			num = -1;
			message = ex2.Message;
		}
		return num;
	}

	public static short TryUpdateCPACodesFromB2B(DateOnly? lastSyncedDate, out string message)
	{
		short result = 0;
		message = "";
		try
		{
			UpdateCPACodesFromB2BRequest request = new UpdateCPACodesFromB2BRequest
			{
				LastSyncedDate = lastSyncedDate
			};
			UpdateCPACodesFromB2BResponse item = DataProcessingRequestHelper.ExecuteThroughHandler<UpdateCPACodesFromB2BRequest, UpdateCPACodesFromB2BResponse>(DPRType.Other, "UpdateCPACodesFromB2B", request).Item2;
			Settings.CoreCacheService.CPACode.Refresh();
			List<string> exceptions = item.Exceptions;
			if (exceptions != null && exceptions.Count > 0)
			{
				result = -1;
				message = string.Format(Resources.ImportCompletedWithErrors, item.CreatedCount + item.EditedCount, item.Exceptions.Count);
			}
			else if (item.CreatedCount > 0 || item.EditedCount > 0)
			{
				result = 1;
				message = string.Format(Resources.ImportedOrUpdated, item.CreatedCount, item.EditedCount);
			}
		}
		catch (Exception ex)
		{
			result = -1;
			message = ex.Message;
		}
		return result;
	}

	public static short TryUpdateBanksFromB2B(out string message)
	{
		short result = 0;
		message = "";
		try
		{
			DirectoryEditFieldsWithSameValueResponseModel directoryEditFieldsWithSameValueResponseModel = Banks.ImportOrUpdateBanks(out var countAdd, out var countUpdate);
			List<DirectoryEditResponseModel> exceptions = directoryEditFieldsWithSameValueResponseModel.Exceptions;
			if (exceptions != null && exceptions.Count > 0)
			{
				result = -1;
				message = string.Format(Resources.ImportCompletedWithErrors, directoryEditFieldsWithSameValueResponseModel.SuccessCount, directoryEditFieldsWithSameValueResponseModel.Exceptions.Count);
			}
			else if (countAdd > 0 || countUpdate > 0)
			{
				result = 1;
				message = string.Format(Resources.ImportedOrUpdated, countAdd, countUpdate);
			}
		}
		catch (Exception ex)
		{
			result = -1;
			message = ex.Message;
		}
		return result;
	}
}
