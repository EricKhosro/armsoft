using System.Windows;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Directories;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Directories;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.Documents.Goods;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;
using ArmSoft.SME.Trade.UI.Services;

namespace ArmSoft.SME.Trade.UI.DropDown;

public class Contracts : DropDownView<ContractDataProvider.DataRow, ContractDataProvider.Param>
{
	private readonly bool _useBonus;

	private readonly string _bonusType;

	private readonly bool _useDiscountServer;

	private readonly bool _isValueMemberPartner;

	private bool showPartnerColumn => string.IsNullOrWhiteSpace(PartnerCode);

	public override string Caption
	{
		get
		{
			if (showPartnerColumn)
			{
				return ArmSoft.SME.Common.Resources.Contract;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode);
			if (item == null)
			{
				throw new ASException(string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerIDIsNotExist, PartnerCode));
			}
			return string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerContracts, item.PartCode, item.Caption);
		}
	}

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Partner).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(DirectoryTypeSE.Partner).AllowEdit;

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public override bool KeyMemberSupported => true;

	public string PartnerCode { get; set; } = string.Empty;

	public string CurrCode { get; set; } = string.Empty;

	public bool IsExceptCurrCodeNCC { get; set; }

	public bool IsFilterBySuppliers { get; set; }

	public bool IsFilterByCustomers { get; set; }

	public bool ShowOnlyBonusCards { get; set; }

	public Contracts(string partnerCode = "", string currCode = "", bool isExceptCurrCodeNCC = false, bool isFilterBySuppliers = false, bool isFilterByCustomers = false, bool isValueMemberPartner = false, bool showOnlyBonusCards = false)
	{
		PartnerCode = partnerCode;
		CurrCode = currCode;
		IsExceptCurrCodeNCC = isExceptCurrCodeNCC;
		IsFilterBySuppliers = isFilterBySuppliers;
		IsFilterByCustomers = isFilterByCustomers;
		ShowOnlyBonusCards = showOnlyBonusCards;
		_isValueMemberPartner = isValueMemberPartner;
		DataProvider = new ContractDataProvider();
		FillColumnsFromDataProvider();
		_useBonus = Settings.ParametersService.UseBonus();
		_bonusType = Settings.ParametersService.BonusType();
		_useDiscountServer = _useBonus && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();
		if (!showPartnerColumn && !isValueMemberPartner)
		{
			Columns["PartCode"].Visible = false;
			Columns["PartCaption"].Visible = false;
			Columns["PartTaxCode"].Visible = false;
			Columns["PartPhone"].Visible = false;
			Columns["PartGroup"].Visible = false;
			Columns["PartGroupName"].Visible = false;
		}
		if (!_useBonus)
		{
			Columns["Bonus"].Visible = false;
			Columns["Point"].Visible = false;
			Columns["ExportedToDiscountServer"].Visible = false;
		}
		else
		{
			if (_bonusType == BonusTypeSE.ByPercent.Value)
			{
				Columns["Point"].Visible = false;
			}
			else
			{
				Columns["Bonus"].Visible = false;
			}
			if (!_useDiscountServer)
			{
				Columns["ExportedToDiscountServer"].Visible = false;
			}
		}
		if (showPartnerColumn | isValueMemberPartner)
		{
			Columns.AddSorting("PartCode");
		}
		Columns.AddSorting("IsDefault", descending: true);
		Columns.AddSorting("Contract");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartnerCode = PartnerCode;
		ParametersProvider.CurrCode = CurrCode;
		ParametersProvider.IsExceptCurrCodeNCC = IsExceptCurrCodeNCC;
		ParametersProvider.IsFilterBySuppliers = IsFilterBySuppliers;
		ParametersProvider.IsFilterByCustomers = IsFilterByCustomers;
		ParametersProvider.ShowOnlyBonusCards = ShowOnlyBonusCards;
	}

	public override short ValueMemberLength()
	{
		if (!_isValueMemberPartner)
		{
			return 3;
		}
		return 20;
	}

	public override string ValueMember(int index)
	{
		if (!_isValueMemberPartner)
		{
			return Data[index].Contract;
		}
		return Data[index].PartCode;
	}

	public override string CommentMember(int index)
	{
		if (!_isValueMemberPartner)
		{
			return Data[index].Caption;
		}
		return Data[index].PartCaption;
	}

	public override object KeyMember(int index)
	{
		return new ContractDataProvider.DataRow.Key
		{
			PartCode = Data[index].PartCode,
			Contract = Data[index].Contract
		};
	}

	public override bool AddNew(ref string value, ref string comment, Window owner, double top, double left)
	{
		return AddOrEdit(ref value, ref comment, isNew: true, owner, top, left);
	}

	public override bool Edit(ref string value, ref string comment, Window owner, double top, double left, object selectedRow)
	{
		return AddOrEdit(ref value, ref comment, isNew: false, owner, top, left);
	}

	private bool AddOrEdit(ref string value, ref string comment, bool isNew, Window owner, double top, double left)
	{
		bool result = false;
		DirectoryDocumentWindow<Partner> directoryDocumentWindow;
		if (owner is MTBillWindow { UseBuyerAddOrEditQuickMode: not false })
		{
			directoryDocumentWindow = (isNew ? new PartnerDocumentQuickModeWindow((Partner)Settings.DirectoryService.Create(DirectoryTypeSE.Partner)) : new PartnerDocumentQuickModeWindow((Partner)Settings.DirectoryService.Load(DirectoryTypeSE.Partner, value)));
		}
		else
		{
			directoryDocumentWindow = (isNew ? ((PartnerDocumentWindow)Settings.DirectoryService.CreatePOWindow(DirectoryTypeSE.Partner)) : ((PartnerDocumentWindow)Settings.DirectoryService.CreatePOWindow(Settings.DirectoryService.Load(DirectoryTypeSE.Partner, value))));
		}
		directoryDocumentWindow.Owner = owner;
		directoryDocumentWindow.Top = top;
		directoryDocumentWindow.Left = left;
		if (directoryDocumentWindow.ShowDialog(isNew ? WindowViewMode.NewCreate : WindowViewMode.Edit).Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = directoryDocumentWindow.PO.Code;
			comment = directoryDocumentWindow.PO.Name;
			PartnerCacheRow.ContractRow defaultContract = TradeSettings.TradeCacheService.Partner.GetDefaultContract(value, out var partnerCacheRow);
			if (defaultContract != null && defaultContract.IsBonusCard && !string.IsNullOrWhiteSpace(defaultContract.DiscountCard) && _useDiscountServer)
			{
				DiscountServerService.CreateCardInfo(partnerCacheRow, defaultContract);
				TradeSettings.CreateTradeClient().CommonServices.EditDiscountServerData(partnerCacheRow.Id, defaultContract.Contract, exportedToDiscountServer: true);
			}
		}
		return result;
	}
}
