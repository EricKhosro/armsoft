using System.Collections.Generic;
using System.Windows.Input;
using ArmSoft.Cloud.Client.Discount;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Services;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Other;
using ArmSoft.SME.Trade.Common.Models.PrintableElements;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView.Directories.DiscountServer;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDown;
using ArmSoft.SME.Trade.UI.Services;
using DevExpress.Data;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.LayoutControl;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10033)]
public class Contracts : TradeDirectoryDataView<ContractsDataProvider.DataRow, ContractsDataProvider.Param>
{
	private class PrintIDCardWindow(List<PartnerContractInfo> selectedContracts) : DialogWindow
	{
		private readonly List<PartnerContractInfo> _selectedContracts = selectedContracts;

		private DropDownControl _template;

		private DropDownControl _partner;

		private DropDownControl _contract;

		private string _partCode = string.Empty;

		private string _partContract = string.Empty;

		internal void Invoke()
		{
			Caption = ArmSoft.SME.Trade.Common.Resources.PrintIDCard;
			Name = "DlgPrintIDCard";
			HelpId = "Sales_Lists_Contracts_PrintIDCard";
			LayoutGroup container = AddHorizontalLayoutGroup();
			if (_selectedContracts.Count <= 1)
			{
				_partner = AddDropDownControl(container, "PartCode", ArmSoft.SME.Common.Resources.Partner, new PartnersFilter(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, 8);
				_partner.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Partner_EditValueChanged).Handler;
				_contract = AddDropDownControl(null, "Contract", ArmSoft.SME.Common.Resources.Contract, new ArmSoft.SME.Trade.UI.DropDown.Contracts(), isRequired: true, allowAdditionButtons: false, storeValue: false, isShowComment: true, 8);
				AddStoreValuesButton(container);
				container = null;
				if (_selectedContracts.Count == 1)
				{
					_partCode = _selectedContracts[0].PartCode;
					_partContract = _selectedContracts[0].Contract;
					_partner.Code = _partCode;
					_contract.Code = _partContract;
				}
			}
			else
			{
				AddStoreValuesButton(container);
			}
			_template = AddDropDownControl(container, "Template", ArmSoft.SME.Common.Resources.PrintTemplate, new Template(short.Parse(PrintTemplateDocumentTypeSE.IDCard.Value)), isRequired: true, allowAdditionButtons: false, storeValue: true);
			SetFocusOnActivation = _template;
			if (!ShowDialog().Value)
			{
				return;
			}
			if (_selectedContracts.Count <= 1 && _partner.Code != _partCode && _contract.Code != _partContract)
			{
				PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(_partner.Code);
				if (item != null)
				{
					PartnerCacheRow.ContractRow contract = TradeSettings.TradeCacheService.Partner.GetContract(item, _contract.Code);
					_selectedContracts.Clear();
					_selectedContracts.Add(new PartnerContractInfo(item, contract));
				}
			}
			PrintIDCard();
		}

		private void PrintIDCard()
		{
			IDCard printableElement = new IDCard
			{
				Caption = ArmSoft.SME.Trade.Common.Resources.PrintIDCard,
				ContractsList = _selectedContracts,
				Prefix = MCRInfo.GetInstance().Prefix,
				Suffix = MCRInfo.GetInstance().Suffix
			};
			Settings.ServiceProvider.GetRequiredService<ITemplateSubstitutionService>().PrintPreview(printableElement, PrintMode.PrintPreview, _template.Code, null, 1, includePrintInfo: false);
		}

		private void Partner_EditValueChanged(object sender, EditValueChangedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(_partner.Code))
			{
				ArmSoft.SME.Trade.UI.DropDown.Contracts dataView = new ArmSoft.SME.Trade.UI.DropDown.Contracts(_partCode);
				_contract.DataView = dataView;
			}
		}
	}

	private readonly bool _useDiscountServer = Settings.ParametersService.UseBonus() && Settings.ParametersService.ConnectTob2barmsoftDiscountServer();

	public override short DirectoryType => DirectoryTypeSE.Contract.Value;

	public override bool AllowExport => true;

	public override string HelpId => "Purchases_Lists_Partnerssublists_contracts";

	public override string Caption
	{
		get
		{
			if (string.IsNullOrWhiteSpace(PartnerCode))
			{
				return ArmSoft.SME.Common.Resources.Contracts;
			}
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(PartnerCode);
			return string.Format(ArmSoft.SME.Trade.Common.Resources.PartnerContracts, item.PartCode, item.Caption);
		}
	}

	public override bool AllowEditSelected => AllowOperation;

	public override LoadIndication LoadingIndication => LoadIndication.Always;

	public string PartnerCode
	{
		get
		{
			return (string)Parameters["PartnerCode"].Value;
		}
		set
		{
			Parameters["PartnerCode"].Value = value;
		}
	}

	public Contracts()
	{
		DataProvider = new ContractsDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("PartnerCode", ArmSoft.SME.Common.Resources.Partner, string.Empty);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!string.IsNullOrWhiteSpace(PartnerCode))
		{
			dvp.Set_ColumnVisible("PartnerCode", value: false);
			dvp.Set_ColumnVisible("PartnerName", value: false);
			dvp.Set_ColumnVisible("PartnerTaxCode", value: false);
			dvp.Set_ColumnVisible("PartnerPhone", value: false);
			dvp.Set_ColumnVisible("PartnerGroup", value: false);
			dvp.Set_ColumnVisible("PartnerGroupName", value: false);
		}
		if (!Settings.ParametersService.UseBonus())
		{
			dvp.Set_ColumnVisible("Bonus", value: false);
			dvp.Set_ColumnVisible("Point", value: false);
			dvp.Set_ColumnVisible("ExportedToDiscountServer", value: false);
		}
		else
		{
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				dvp.Set_ColumnVisible("Point", value: false);
			}
			else
			{
				dvp.Set_ColumnVisible("Bonus", value: false);
			}
			if (!Settings.ParametersService.UseBonus() && !Settings.ParametersService.ConnectTob2barmsoftDiscountServer())
			{
				dvp.Set_ColumnVisible("ExportedToDiscountServer", value: false);
			}
		}
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("PartnerCode");
			dvp.Set_ColumnSortOrder("IsDefault", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("Contract");
		}
		Columns.AddDirectoryLinkColumn(DirectoryTypeSE.Partner.Value, "PartnerCode", "PartnerName");
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		DataViewPanel currentViewPanel = Settings.HostingEnvironment.CurrentViewPanel;
		currentViewPanel?.AddContextFunction("ContextFunctions", "PrintIdCard", ArmSoft.SME.Trade.Common.Resources.PrintIDCard, PrintIdCard);
		if (_useDiscountServer)
		{
			currentViewPanel.AddContextMenuGroup("b2b", ArmSoft.SME.Common.Resources.b2bSystem, "cloud_16.svg");
			currentViewPanel.AddContextFunction("b2b", "ExportToDiscountServer", ArmSoft.SME.Trade.Common.Resources.ExportToDiscountServer, ExportToDiscountServer);
			currentViewPanel.AddContextFunction("b2b", "CompareWithDiscountServer", ArmSoft.SME.Trade.Common.Resources.CompareWithDiscountServer, CompareWithDiscountServer);
		}
	}

	private void CompareWithDiscountServer()
	{
		Mouse.OverrideCursor = Cursors.Wait;
		List<string> list = new List<string>();
		foreach (ContractsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<ContractsDataProvider.DataRow>())
		{
			if (selectedOrFocusedRow != null && selectedOrFocusedRow.IsBonusCard && !string.IsNullOrWhiteSpace(selectedOrFocusedRow.DiscountCard))
			{
				list.Add(selectedOrFocusedRow.DiscountCard);
			}
		}
		Mouse.OverrideCursor = null;
		if (list.Count > 0)
		{
			BonusCardsComparisonDataView bonusCardsComparisonDataView = new BonusCardsComparisonDataView();
			bonusCardsComparisonDataView.CardNumbers = string.Join(",", list);
			bonusCardsComparisonDataView.Show();
			return;
		}
		throw new ASException(ArmSoft.SME.Trade.Common.Resources.NoneOfContractsIsBonusCard);
	}

	private void ExportToDiscountServer()
	{
		bool flag = false;
		Mouse.OverrideCursor = Cursors.Wait;
		foreach (ContractsDataProvider.DataRow selectedOrFocusedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<ContractsDataProvider.DataRow>())
		{
			if (ExportCardToDiscountServer(selectedOrFocusedRow))
			{
				flag = true;
			}
		}
		Mouse.OverrideCursor = null;
		if (flag)
		{
			MessageBox.Show(ArmSoft.SME.Common.Resources.ChangeRequestCompleted, ArmSoft.SME.Trade.Common.Resources.ExportToDiscountServer);
		}
	}

	private static bool ExportCardToDiscountServer(ContractsDataProvider.DataRow row)
	{
		if (row.IsBonusCard && !string.IsNullOrWhiteSpace(row.DiscountCard) && !row.ExportedToDiscountServer)
		{
			DiscountServerService.CreateCardInfo(row);
			TradeSettings.CreateTradeClient().CommonServices.EditDiscountServerData(row.PartnerId, row.Contract, exportedToDiscountServer: true);
			Settings.HostingEnvironment.CurrentViewPanel.Update(new ContractsDataProvider.DataRow.Key
			{
				Contract = row.Contract,
				PartnerId = row.PartnerId
			});
			return true;
		}
		return false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartnerCode = PartnerCode;
	}

	public override void Add()
	{
		ContractDialogWindow contractDialogWindow = new ContractDialogWindow(DialogWindowMode.NewMode)
		{
			DVPId = Settings.HostingEnvironment.CurrentViewPanel.Id
		};
		ContractsDataProvider.DataRow dataRow = (ContractsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null)
		{
			contractDialogWindow.Directory.PartCode = dataRow.PartnerCode;
		}
		else
		{
			contractDialogWindow.Directory.PartCode = PartnerCode;
		}
		contractDialogWindow.ShowDialog();
	}

	public override void Edit()
	{
		base.Edit();
		ContractsDataProvider.DataRow dataRow = (ContractsDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (dataRow != null && _useDiscountServer && dataRow.IsBonusCard && dataRow.ExportedToDiscountServer && !string.IsNullOrWhiteSpace(dataRow.DiscountCard))
		{
			try
			{
				DiscountServerService.UpdateCardInfo(new CardUpdateRequest
				{
					CardNumber = dataRow.DiscountCard,
					BonusPercent = dataRow.Bonus
				});
			}
			catch
			{
			}
		}
	}

	protected override void AddControlsInEditSelectedDialog(DialogWindow dialog, out bool doRefresh)
	{
		base.AddControlsInEditSelectedDialog(dialog, out doRefresh);
		if (Settings.ParametersService.UseBonus())
		{
			DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "IsBonusCard", ArmSoft.SME.Trade.Common.Resources.BonusCard);
			if (Settings.ParametersService.BonusType() == BonusTypeSE.ByPercent.Value)
			{
				DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Bonus", string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Trade.Common.Resources.PercentSymbol), 6, 2, isRequired: false, allowNegativeNumbers: true);
			}
			else
			{
				DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "Point", string.Format(ArmSoft.SME.Trade.Common.Resources.Bonus, ArmSoft.SME.Common.Resources.Unit), 16, 2, isRequired: false, allowNegativeNumbers: true);
			}
		}
		DialogChangeDemandControls.AddChangeDemandDropDownControl(dialog, "PriceType", ArmSoft.SME.Trade.Common.Resources.PriceType, new ArmSoft.SME.Trade.UI.DropDown.PriceTypes());
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "DiscountPercent", ArmSoft.SME.Trade.Common.Resources.Discount + " (%)", 6, 2, isRequired: false, allowNegativeNumbers: true);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "ControlReceivables", ArmSoft.SME.Trade.Common.Resources.ControlReceivables);
		DialogChangeDemandControls.AddChangeDemandNumericControl(dialog, "ReceivablesLimit", ArmSoft.SME.Trade.Common.Resources.ReceivablesLimit, 16, 2);
		DialogChangeDemandControls.AddChangeDemandBooleanControl(dialog, "BonusCardCanceled", ArmSoft.SME.Trade.Common.Resources.BonusCardCanceled);
		DialogChangeDemandControls.AddChangeDemandDateEditControl(dialog, "BonusCardValidPeriodStart", ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.BeginOfPeriodShort);
		DialogChangeDemandControls.AddChangeDemandDateEditControl(dialog, "BonusCardValidPeriodEnd", ArmSoft.SME.Trade.Common.Resources.BonusCardValidationPeriod + " - " + ArmSoft.SME.Common.Resources.EndOfPeriodShort);
	}

	public override void EditSelected()
	{
		base.EditSelected();
		foreach (ContractsDataProvider.DataRow selectedRow in Settings.HostingEnvironment.CurrentViewPanel.GetSelectedRows<ContractsDataProvider.DataRow>())
		{
			if (_useDiscountServer && selectedRow.IsBonusCard && selectedRow.ExportedToDiscountServer && !string.IsNullOrWhiteSpace(selectedRow.DiscountCard))
			{
				try
				{
					DiscountServerService.UpdateCardInfo(new CardUpdateRequest
					{
						CardNumber = selectedRow.DiscountCard,
						BonusPercent = selectedRow.Bonus
					});
				}
				catch
				{
				}
			}
		}
	}

	private void PrintIdCard()
	{
		List<ContractsDataProvider.DataRow> selectedOrFocusedRows = Settings.HostingEnvironment.CurrentViewPanel.GetSelectedOrFocusedRows<ContractsDataProvider.DataRow>();
		List<PartnerContractInfo> list = new List<PartnerContractInfo>();
		foreach (ContractsDataProvider.DataRow item2 in selectedOrFocusedRows)
		{
			PartnerContractInfo item = new PartnerContractInfo
			{
				PartID = item2.PartnerId,
				PartCaption = item2.PartnerName,
				PartCode = item2.PartnerCode,
				PartTaxCode = item2.PartnerTaxCode,
				PartPhone = item2.PartnerPhone,
				PartGroup = item2.PartnerGroup,
				PartGroupName = item2.PartnerGroupName,
				PartIsCustomer = item2.PartnerIsCustomer,
				PartIsSupplier = item2.PartnerIsSupplier,
				Contract = item2.Contract,
				Caption = item2.Name,
				CurrCode = item2.CurrCode,
				PriceType = item2.PriceType,
				Limit = item2.Limit,
				Percent = item2.Percent,
				DiscountCard = item2.DiscountCard,
				IsBonusCard = item2.IsBonusCard,
				Bonus = item2.Bonus,
				Point = item2.Point,
				ControlReceivables = item2.ControlReceivables,
				ReceivablesLimit = item2.ReceivablesLimit,
				IsDefault = item2.IsDefault,
				ContractDate = item2.ContractDate,
				Description = item2.Description,
				BonusCardValidPeriodEnd = item2.BonusCardValidPeriodEnd,
				BonusCardValidPeriodStart = item2.BonusCardValidPeriodStart,
				BonusCardCanceled = item2.BonusCardCanceled,
				ExportedToDiscountServer = item2.ExportedToDiscountServer
			};
			list.Add(item);
		}
		new PrintIDCardWindow(list).Invoke();
	}
}
