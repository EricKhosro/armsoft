using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.EMarkReceiptClient.Configuration;
using ArmSoft.EMarkReceiptClient.Exceptions;
using ArmSoft.EMarkReceiptClient.Models;
using ArmSoft.EMarkReceiptClient.Services;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.DPR;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.BackgroundWorks;
using ArmSoft.SME.Core.UI.DPR;
using ArmSoft.SME.Core.UI.SmartEnums;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.DataViewDataProvider.Offline;
using ArmSoft.b2b.V2;
using Microsoft.Win32;
using Serilog;

namespace ArmSoft.SME.Trade.UI.Notifications;

public class SynchronizationsProcessor : SynchronizationsProcessorBase
{
	public bool PartnerStatisticsChecked { get; set; }

	public DateOnly? PartnerStatisticsLastDate => Settings.ParametersService.PartnerDataReceivedDateFromb2b();

	public bool OfflineDBInfoUpdatedChecked { get; set; }

	public bool EMarkReceiptUpdateChecked { get; set; }

	public DateOnly? EMarkReceiptUpdateLastDate => Settings.ParametersService.EMarkReceiptLastSyncDate();

	public short TryGetPartnersStatisticsFromb2b(DateOnly currentDate, ref string message)
	{
		message = "";
		short result = -1;
		try
		{
			if (GetPartnersStatisticsAndUpdatePartners())
			{
				result = 1;
				TradeSettings.TradeCacheService.Partner.Refresh();
				message = Resources.PartnerStatisticUpdatedFromb2b;
			}
			else
			{
				result = 0;
			}
		}
		catch (EndpointNotFoundException)
		{
			message = Resources.IBEndpointNotFound;
		}
		catch (Exception ex2)
		{
			message = ex2.Message;
		}
		return result;
	}

	private static bool GetPartnersStatisticsAndUpdatePartners()
	{
		bool result = false;
		List<string> list = (from x in TradeSettings.TradeCacheService.Partner.GetList()
			select x.TaxCode into x
			where !string.IsNullOrWhiteSpace(x)
			select x).ToList();
		if (list.Count > 0)
		{
			List<OrganizationStatistics> organizationStatistics;
			using (B2BClient b2BClient = Settings.B2BClient)
			{
				organizationStatistics = b2BClient.Statistics.GetOrganizationStatistics(list);
			}
			if (organizationStatistics != null && organizationStatistics.Count > 0)
			{
				List<PartnerB2BState> list2 = new List<PartnerB2BState>();
				foreach (OrganizationStatistics item in organizationStatistics)
				{
					foreach (PartnerCacheRow item2 in TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(item.TaxCode).ToList())
					{
						if (item2.B2BStatusCode != (byte)item.StateExtended)
						{
							list2.Add(new PartnerB2BState
							{
								PartnerCode = item2.PartCode,
								B2BStatusCode = (byte)item.StateExtended
							});
						}
					}
				}
				List<string> list3 = new List<string>();
				list3.AddRange(organizationStatistics.Select((OrganizationStatistics x) => x.TaxCode));
				List<string> list4 = list.Except(new _003C_003Ez__ReadOnlyList<string>(list3)).ToList();
				if (list4 != null && list4.Count > 0)
				{
					foreach (string item3 in list4)
					{
						foreach (PartnerCacheRow item4 in TradeSettings.TradeCacheService.Partner.GetPartnersByTaxCode(item3).ToList())
						{
							if (item4.B2BStatusCode != 254)
							{
								list2.Add(new PartnerB2BState
								{
									PartnerCode = item4.PartCode,
									B2BStatusCode = 254
								});
							}
						}
					}
				}
				PartnerStatisticsUpdateRequestModel request = new PartnerStatisticsUpdateRequestModel
				{
					B2BPartnertsStatisticsList = list2
				};
				DataProcessingRequestHelper.ExecuteThroughHandler<PartnerStatisticsUpdateRequestModel, PartnerStatisticsUpdateResponseModel>(DPRType.Other, "PartnerStatisticsUpdateDPR", request);
			}
		}
		return result;
	}

	public static void SendOfflineDBInfo()
	{
		try
		{
			List<OfflineDBInfoDocInfo> list = (from p in new LocalDBDocumentsDataProvider().GetRows(new LocalDBDocumentsDataProvider.Param(), withoutAccessCheck: true)
				where p.SyncState == LocalDBDocumentSyncStateSE.Sent.Value || p.SyncState == LocalDBDocumentSyncStateSE.NotSent.Value
				group p by new
				{
					Date = p.DocDate,
					DocType = p.DocType
				} into @group
				select new OfflineDBInfoDocInfo
				{
					DocDate = @group.Key.Date.ToDateTime(),
					DocType = @group.Key.DocType,
					Total = @group.Count(),
					NotSynchronized = @group.Count((LocalDBDocumentsDataProvider.DataRow doc) => doc.SyncState == LocalDBDocumentSyncStateSE.NotSent.Value)
				}).ToList();
			if (list == null || list.Count == 0)
			{
				return;
			}
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(Settings.RegistryKey);
			Guid guid = ((Guid.TryParse(registryKey.GetValue("StationId", Guid.Empty)?.ToString(), out var result) && result != Guid.Empty) ? result : Guid.NewGuid());
			if (result == Guid.Empty)
			{
				registryKey.SetValue("StationId", guid);
			}
			using CloudClient cloudClient = Settings.CloudClient;
			cloudClient.Token = Settings.GetApplicationToken();
			OfflineDBInfoRequest request = new OfflineDBInfoRequest
			{
				StationId = guid,
				StationName = Environment.MachineName,
				LastOfflineDocDate = list.Max((OfflineDBInfoDocInfo p) => p.DocDate),
				DocInfo = list
			};
			cloudClient.AS7X.StoreOfflineDBInfo(request);
		}
		catch (Exception exception)
		{
			Log.Warning(exception, $"SendOfflineDBInfo (Database - {Settings.LoginService.Database})");
		}
	}

	public static void TryUpdateEMarkReceipts(DateOnly lastSyncedDate)
	{
		try
		{
			List<EMarkReceiptAppDataProvider.DataRow> list = (from p in new EMarkReceiptAppDataProvider(TradeSettings.CreateTradeClient()).GetRows(new EMarkReceiptAppDataProvider.Param(), withoutAccessCheck: true)
				where p.Enabled
				select p).ToList();
			if (list.Count == 0)
			{
				return;
			}
			HttpClient httpClient = new HttpClient();
			List<EMarkReceiptApp> list2 = new List<EMarkReceiptApp>();
			foreach (EMarkReceiptAppDataProvider.DataRow item in list)
			{
				EMarkReceiptApp eMarkReceiptApp = Settings.DirectoryService.Load<EMarkReceiptApp>(DirectoryTypeSE.EMarkReceipt.Value, item.Code, withoutAccessCheck: true);
				try
				{
					EMarkOptions options = new EMarkOptions
					{
						Token = eMarkReceiptApp.Token,
						RefreshToken = eMarkReceiptApp.RefreshToken,
						CRN = eMarkReceiptApp.CRN,
						Datamatrix_file_id = eMarkReceiptApp.DeviceId
					};
					RefreshTokenResponse refreshToken = new EMarkApiClient(httpClient, options).GetRefreshToken();
					eMarkReceiptApp.Token = refreshToken.token;
					eMarkReceiptApp.RefreshToken = refreshToken.refresh_token;
				}
				catch (Exception ex)
				{
					if (ex is EMarkApiException { StatusCode: HttpStatusCode.Unauthorized })
					{
						eMarkReceiptApp.Enabled = false;
					}
					Log.Warning(ex, "TryUpdateEMarkReceipts, EMark " + item.Code + " device");
				}
				list2.Add(eMarkReceiptApp);
			}
			Settings.DirectoryService.StoreSelected(list2, new DirStoreRequest
			{
				WithoutAccessCheck = true
			});
		}
		catch (Exception exception)
		{
			Log.Warning(exception, $"TryUpdateEMarkReceipts (Database - {Settings.LoginService.Database})");
		}
	}
}
