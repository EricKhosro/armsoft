using System;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Directories;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.b2b.V2;

namespace ArmSoft.SME.Trade.UI.Services;

public class TradeDirectoryService : DirectoryService
{
	protected override ApiClient Client => TradeSettings.CreateTradeClient();

	public static void StorePartnerFromB2BData(string tin, string supplierBankAccount, string supplierBankName)
	{
		StorePartnerFromOrganization(GetOrganization(tin), supplierBankAccount, supplierBankName);
	}

	public static Organization GetOrganization(string tin)
	{
		try
		{
			if (tin.Length != 8)
			{
				throw new ASException(string.Format(Resources.FieldLengthMustBeEqualTo, Resources.OptionsTaxCode, (short)8) + ": " + tin);
			}
			using B2BClient b2BClient = Settings.B2BClient;
			return b2BClient.BusinessUnits.GetOrganization(tin) ?? throw new ASException(string.Format(Resources.PartnerNotExistInB2B, tin));
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static void StorePartnerFromOrganization(Organization org, string supplierBankAccount, string supplierBankName)
	{
		Partner partner = Settings.DirectoryService.Create<Partner>(DirectoryTypeSE.Partner.Value);
		partner.Code = Settings.ParametersService.NextPartner();
		partner.TaxCode = org.TaxCode;
		partner.FullCaption = org.FullName + (string.IsNullOrWhiteSpace(org.LegalStatusShort) ? "" : (" " + org.LegalStatusShort));
		partner.Name = partner.FullCaption.Left(50);
		partner.Address = org.LegalAddress.FullPath;
		partner.BusinessAddress = (string.IsNullOrWhiteSpace(org.BusinessAddress.FullPath) ? org.LegalAddress.FullPath : org.BusinessAddress.FullPath);
		partner.ManagerPost = org.Manager.Position;
		partner.ManagerFIO = org.Manager.Name;
		partner.ChiefaccPost = org.Accountant.Position;
		partner.ChiefaccFIO = org.Accountant.Name;
		partner.Email = org.MailAddress;
		if (!string.IsNullOrWhiteSpace(supplierBankAccount) && !string.IsNullOrWhiteSpace(supplierBankName))
		{
			partner.SettlementAccounts.Add(new Partner.PartnerSettlementAccount
			{
				SettlAccount = supplierBankAccount.Trim(),
				IsDefault = true,
				Caption = ((supplierBankName.Length > 50) ? supplierBankName.Substring(0, 50) : supplierBankName)
			});
		}
		Settings.DirectoryService.Store(partner, new DirStoreRequest
		{
			LogComment = Resources.LogCommentImportedFromB2B
		});
	}

	public static void StorePartnerBankAccount(string partnerCode, string supplierBankAccount, string supplierBankName)
	{
		if (!string.IsNullOrWhiteSpace(supplierBankAccount) && !string.IsNullOrWhiteSpace(supplierBankName))
		{
			Partner partner = Settings.DirectoryService.Load<Partner>(DirectoryTypeSE.Partner.Value, partnerCode, withoutAccessCheck: true);
			supplierBankAccount = supplierBankAccount.Trim();
			if (!partner.SettlementAccounts.Exists((Partner.PartnerSettlementAccount p) => p.SettlAccount == supplierBankAccount))
			{
				partner.SettlementAccounts.Add(new Partner.PartnerSettlementAccount
				{
					SettlAccount = supplierBankAccount,
					IsDefault = (partner.SettlementAccounts.Count == 0),
					Caption = ((supplierBankName.Length > 50) ? supplierBankName.Substring(0, 50) : supplierBankName)
				});
				Settings.DirectoryService.Store(partner, new DirStoreRequest
				{
					WithoutAccessCheck = true
				});
			}
		}
	}

	public override void ShowWindow(ArmSoft.SME.Common.Models.Directories.Directory directory, bool isPOWindow, WindowViewMode viewMode, int dvpId = -1)
	{
		if (isPOWindow)
		{
			IPOWindow iPOWindow3;
			if (directory.POType == DirectoryTypeSE.Goods.Value)
			{
				IPOWindow iPOWindow2;
				if (!Settings.ParametersService.GoodAddSimpleMode())
				{
					IPOWindow iPOWindow = new GoodDocumentWindow((Good)directory);
					iPOWindow2 = iPOWindow;
				}
				else
				{
					IPOWindow iPOWindow = new GoodDocumentSimpleWindow((Good)directory);
					iPOWindow2 = iPOWindow;
				}
				iPOWindow3 = iPOWindow2;
			}
			else
			{
				iPOWindow3 = CreatePOWindow(directory);
			}
			if (dvpId != -1)
			{
				iPOWindow3.DVPID = dvpId;
			}
			iPOWindow3.Show(viewMode);
			return;
		}
		DialogWindowMode mode = DialogWindowMode.None;
		switch (viewMode)
		{
		case WindowViewMode.View:
			mode = DialogWindowMode.ViewMode;
			break;
		case WindowViewMode.Edit:
			mode = DialogWindowMode.EditMode;
			break;
		case WindowViewMode.NewCreate:
			mode = DialogWindowMode.NewMode;
			break;
		case WindowViewMode.Copy:
			mode = DialogWindowMode.CreateCopyMode;
			break;
		}
		IDirectoryDialogWindow directoryDialogWindow = CreateDialogWindow(directory, mode);
		if (dvpId != -1)
		{
			directoryDialogWindow.DVPId = dvpId;
		}
		directoryDialogWindow.ShowDialog();
	}
}
