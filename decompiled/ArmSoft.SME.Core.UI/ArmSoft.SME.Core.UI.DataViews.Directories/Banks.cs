using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.b2b.V2;

namespace ArmSoft.SME.Core.UI.DataViews.Directories;

[DataView(1)]
public class Banks : DirectoryDataView<BanksDataProvider.DataRow, BanksDataProvider.Param>
{
	public override short DirectoryType => DirectoryTypeSE.Bank.Value;

	public override string HelpId
	{
		get
		{
			if (Settings.SystemType != SystemTypes.Trade)
			{
				if (Settings.SystemType != SystemTypes.Accountant)
				{
					return "Banks";
				}
				return "Accounting_Lists_Banks";
			}
			return "Purchases_Lists_OtherLists_Banks";
		}
	}

	public Banks()
	{
		DataProvider = new BanksDataProvider(Settings.CreateApiClient());
		FillColumnsFromDataProvider();
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		if (AllowAdd && AllowEdit)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ImportBanksFromB2B", Resources.ImportFromB2B, ImportBanksFromB2B, "cloud_16.svg", "", storeOpContext: true);
		}
	}

	public static void ImportBanksFromB2B()
	{
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			DirectoryEditFieldsWithSameValueResponseModel directoryEditFieldsWithSameValueResponseModel = ImportOrUpdateBanks(out var countAdd, out var countUpdate);
			string messageBoxText = string.Format(Resources.ImportedOrUpdated, countAdd, countUpdate);
			List<DirectoryEditResponseModel> exceptions = directoryEditFieldsWithSameValueResponseModel.Exceptions;
			if (exceptions != null && exceptions.Count > 0)
			{
				messageBoxText = string.Format(Resources.ImportCompletedWithErrors, directoryEditFieldsWithSameValueResponseModel.SuccessCount, directoryEditFieldsWithSameValueResponseModel.Exceptions.Count);
			}
			DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
			if (currentViewPanel != null && currentViewPanel.IDV.GetType() == typeof(Banks))
			{
				currentViewPanel.Refresh();
			}
			Mouse.OverrideCursor = null;
			MessageBox.Show(messageBoxText, Resources.ImportFromB2B, MessageBoxButton.OK);
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public static DirectoryEditFieldsWithSameValueResponseModel ImportOrUpdateBanks(out int countAdd, out int countUpdate)
	{
		countAdd = 0;
		countUpdate = 0;
		DirectoryEditFieldsWithSameValueResponseModel result = new DirectoryEditFieldsWithSameValueResponseModel();
		List<BankBase> list = new List<BankBase>();
		List<Bank> banks;
		using (B2BClient b2BClient = Settings.B2BClient)
		{
			banks = b2BClient.BusinessUnits.GetBanks();
		}
		foreach (Bank item2 in banks)
		{
			BanksCacheRow item = Settings.CoreCacheService.Bank.GetItem(item2.Code);
			if (item == null)
			{
				BankBase bankBase = (BankBase)Settings.DirectoryService.Create(DirectoryTypeSE.Bank.Value);
				bankBase.Code = item2.Code;
				bankBase.Name = item2.Name;
				bankBase.SWIFTCODE = item2.SwiftCode;
				list.Add(bankBase);
				countAdd++;
			}
			else if (item.Name != item2.Name.Trim() || item.SWIFTCODE != item2.SwiftCode.Trim())
			{
				BankBase bankBase = (BankBase)Settings.DirectoryService.Load(DirectoryTypeSE.Bank.Value, item.Code);
				bankBase.Name = item2.Name;
				bankBase.SWIFTCODE = item2.SwiftCode;
				list.Add(bankBase);
				countUpdate++;
			}
		}
		if (countAdd > 0 || countUpdate > 0)
		{
			result = Settings.DirectoryService.StoreSelected(list, new DirStoreRequest());
			Settings.DirectoryService.RefreshDirectoryUICaches(DirectoryTypeSE.Bank.Value);
		}
		return result;
	}
}
