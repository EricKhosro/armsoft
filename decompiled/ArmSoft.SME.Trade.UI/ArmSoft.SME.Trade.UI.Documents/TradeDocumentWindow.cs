using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Common.CustomTypes;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.PersistableObject;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements.UserControls;
using ArmSoft.SME.Trade.UI.DataView;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.Documents;

public abstract class TradeDocumentWindow<T> : DocumentWindow<T> where T : TradeDocument, new()
{
	protected readonly Dictionary<Guid, byte[]> _images = new Dictionary<Guid, byte[]>();

	protected override bool AllowCreateChildDoc => true;

	public TradeDocumentWindow()
	{
	}

	public TradeDocumentWindow(T doc)
		: base(doc)
	{
	}

	protected override void SetControlValue(Control control, object value)
	{
		base.SetControlValue(control, value);
		if (control != null && control is PartnerContractGroupControls partnerContractGroupControls)
		{
			PartnerContractGroup partnerContractGroup = (PartnerContractGroup)value;
			partnerContractGroupControls.PartnerCode.Code = partnerContractGroup.PartnerCode;
			partnerContractGroupControls.Contract.Code = partnerContractGroup.ContractCode;
			if (partnerContractGroupControls.SummaPart != null)
			{
				partnerContractGroupControls.SummaPart.EditValue = partnerContractGroup.SummaPart;
			}
		}
	}

	protected override object GetControlValue(Control control)
	{
		if (control is PartnerContractGroupControls partnerContractGroupControls)
		{
			return new PartnerContractGroup(partnerContractGroupControls.PartnerCode.Code, partnerContractGroupControls.Contract.Code, (partnerContractGroupControls.SummaPart != null) ? partnerContractGroupControls.SummaPart.Value : 0m);
		}
		return base.GetControlValue(control);
	}

	public PartnerContractGroupControls AddPartnerContractGroupControls(LayoutGroup container, PartnerContractGroupDefinition partnerContractGroupDefinition, DateEditControl docDateControl, DropDownControl docCurrCodeControl, PopupCalcEditExt docSummaControl, PopupCalcEditExt docSummaNCCControl = null, bool isShowContractCurrCode = true, bool isShowSummaDebt = true, bool showOnlyCurCodeNCC = false, bool isContractRequired = true, bool allowPartnerAdditionalButtons = true)
	{
		return AddPartnerContractGroupControls(container, docDateControl, docCurrCodeControl, docSummaControl, docSummaNCCControl, partnerContractGroupDefinition.PartnerFieldDefinition.Caption, partnerContractGroupDefinition.PartnerFieldDefinition.IsSupplier, partnerContractGroupDefinition.PartnerFieldDefinition.IsCustomer, partnerContractGroupDefinition.SummaPartFieldDefinition != null, partnerContractGroupDefinition.ContractFieldDefinition.IsBonusCard, partnerContractGroupDefinition.ContractFieldDefinition.Caption, isShowContractCurrCode, isShowSummaDebt, showOnlyCurCodeNCC, isContractRequired, allowPartnerAdditionalButtons, partnerContractGroupDefinition.Name);
	}

	public PartnerContractGroupControls AddPartnerContractGroupControls(LayoutGroup container, DateEditControl docDateControl, DropDownControl docCurrCodeControl, PopupCalcEditExt docSummaControl, PopupCalcEditExt docSummaNCCControl = null, string partnerCaption = "", bool isFilterBySuppliers = false, bool isFilterByCustomers = false, bool isVisibleSummaPart = true, bool showOnlyBonusContracts = false, string contractCaption = "", bool isShowContractCurrCode = true, bool isShowSummaDebt = true, bool showOnlyCurCodeNCC = false, bool isContractRequired = true, bool allowPartnerAdditionalButtons = true, string dataItemName = "")
	{
		PartnerContractGroupControls partnerContractGroupControls = new PartnerContractGroupControls(this);
		partnerContractGroupControls.ConstructUI(container, docDateControl, docCurrCodeControl, docSummaControl, docSummaNCCControl, partnerCaption, isFilterBySuppliers, isFilterByCustomers, isVisibleSummaPart, showOnlyBonusContracts, contractCaption, isShowContractCurrCode, isShowSummaDebt, showOnlyCurCodeNCC, isContractRequired, allowPartnerAdditionalButtons, dataItemName);
		AddControl(partnerContractGroupControls, partnerCaption, null, dataItemName);
		return partnerContractGroupControls;
	}

	protected override void SetControlReadonly(Control control, bool value)
	{
		if (control is PartnerContractGroupControls { SummaPart: var summaPart } partnerContractGroupControls)
		{
			if (summaPart != null)
			{
				summaPart.IsReadOnly = true;
			}
			PopupCalcEditExt summaPartDebt = partnerContractGroupControls.SummaPartDebt;
			if (summaPartDebt != null)
			{
				summaPartDebt.IsReadOnly = true;
			}
		}
		else
		{
			base.SetControlReadonly(control, value);
		}
	}

	public InfoDisplayControls AddInfoDisplayControls(DocumentTypeSE documetType, bool isShowItemPictureOnInfoPanel = false, bool isShowStorageAvailabilityOnInfoPanel = false, bool isShowWholeAvailabilityOnInfoPanel = false, bool isShowPriceAvailabilityOnInfoPanel = false, bool showDescriptionOnInfoPanel = false, bool enableSubstituteItems = false, double fontSizeLarge = 22.0, LayoutGroup container = null)
	{
		InfoDisplayControls infoDisplayControls = new InfoDisplayControls(this, documetType, isShowItemPictureOnInfoPanel, isShowStorageAvailabilityOnInfoPanel, isShowWholeAvailabilityOnInfoPanel, isShowPriceAvailabilityOnInfoPanel, showDescriptionOnInfoPanel, enableSubstituteItems, fontSizeLarge);
		infoDisplayControls.ConstructUI(container);
		return infoDisplayControls;
	}

	protected override void BarButtonDocumentLinkedList_ItemClick(object sender, ItemClickEventArgs e)
	{
		string documentTypeName = "";
		if (SmartEnum<DocumentTypeSE, short>.TryFromValue(PO.POType, out var result))
		{
			documentTypeName = result.Caption;
		}
		DocumentLinkedList documentLinkedList = new DocumentLinkedList();
		documentLinkedList.DocISN = PO.ISN;
		documentLinkedList.DocumentTypeName = documentTypeName;
		documentLinkedList.DocNum = PO.DocNum;
		documentLinkedList.BaseDocISN = PO.BaseISN;
		documentLinkedList.DocDate = Convert.ToString(PO.DocDate);
		documentLinkedList.DocOwner = (Settings.ParametersService.DenyAccessToOtherOwnerDocs() ? PO.Owner : null);
		documentLinkedList.Show(useParamValues: true, openAsChild: false);
		Keyboard.Focus(documentLinkedList.DVP);
	}

	protected string GetReportsAdditionalCaption()
	{
		return string.Format(ArmSoft.SME.Common.Resources.DocDescr, SmartEnum<DocumentTypeSE, short>.FromValue(PO.POType).Caption, PO.DocNum);
	}
}
