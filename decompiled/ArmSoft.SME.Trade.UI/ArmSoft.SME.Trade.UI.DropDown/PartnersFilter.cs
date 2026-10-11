using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.CustomUIElements;
using ArmSoft.SME.Trade.UI.Directory;
using ArmSoft.SME.Trade.UI.DropDownDataProvider;

namespace ArmSoft.SME.Trade.UI.DropDown;

[DropDown(10002)]
public class PartnersFilter : DropDownView<PartnersFilterDataProvider.DataRow, PartnersFilterDataProvider.Param>
{
	public enum ShowPartnersModeType
	{
		CodeCaption,
		TINCode,
		CodeSettlAccCaptionAim,
		AllColumns,
		FindBuyerPOS
	}

	private readonly ShowPartnersModeType _showColumnsModeType;

	public override bool AddNewSupported => Settings.UserAccessService.GetDirectoryPermission(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner).AllowAdd;

	public override bool EditSupported => Settings.UserAccessService.GetDirectoryPermission(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner).AllowEdit;

	public override bool FilteredSearchSupported => true;

	public override string Caption
	{
		get
		{
			if (IsFilterBySuppliers && !IsFilterByCustomers)
			{
				return ArmSoft.SME.Common.Resources.Suppliers;
			}
			if (IsFilterByCustomers && !IsFilterBySuppliers)
			{
				return ArmSoft.SME.Trade.Common.Resources.Customers;
			}
			return ArmSoft.SME.Common.Resources.Partners;
		}
	}

	public override ReloadMode ReloadMode => ReloadMode.Reloadable;

	public string PartType { get; set; } = "";

	public bool IsFilterBySuppliers { get; set; }

	public bool IsFilterByCustomers { get; set; }

	public List<string> Partners { get; set; }

	private static string PartnerB2BStateExpression
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte item in new List<byte> { 3, 5, 6, 7, 8, 9, 10, 11, 14, 254 })
			{
				if (SmartEnum<PartnerStatusInB2BSE, byte>.TryFromValue(item, out var result))
				{
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(11, 2, stringBuilder2);
					handler.AppendLiteral("[");
					handler.AppendFormatted("B2BStatusName");
					handler.AppendLiteral("] = '");
					handler.AppendFormatted(result.Caption);
					handler.AppendLiteral("' or ");
					stringBuilder2.Append(ref handler);
				}
			}
			return stringBuilder.ToString().Left(stringBuilder.Length - 4);
		}
	}

	public PartnersFilter(ShowPartnersModeType showColumnsModeType = ShowPartnersModeType.CodeCaption, string partType = "", bool isFilterBySuppliers = false, bool isFilterByCustomers = false, List<string> partners = null)
	{
		_showColumnsModeType = showColumnsModeType;
		PartType = partType;
		IsFilterBySuppliers = isFilterBySuppliers;
		IsFilterByCustomers = isFilterByCustomers;
		Partners = partners;
		DataProvider = new PartnersFilterDataProvider();
		if (_showColumnsModeType == ShowPartnersModeType.TINCode)
		{
			Columns.Add("TaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
		}
		Columns.Add("PartCode", ArmSoft.SME.Common.Resources.Code);
		if (_showColumnsModeType == ShowPartnersModeType.CodeSettlAccCaptionAim)
		{
			Columns.Add("SettlAcc", ArmSoft.SME.Common.Resources.SettlementAccount);
			Columns.Add("Caption", ArmSoft.SME.Common.Resources.Name);
			Columns.Add("TaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
			Columns.Add("MainAIM", ArmSoft.SME.Common.Resources.PaymentMainAim);
			return;
		}
		Columns.Add("Caption", ArmSoft.SME.Common.Resources.Name);
		if (_showColumnsModeType != ShowPartnersModeType.TINCode && _showColumnsModeType != ShowPartnersModeType.FindBuyerPOS)
		{
			Columns.Add("TaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
		}
		if (_showColumnsModeType == ShowPartnersModeType.FindBuyerPOS)
		{
			Columns.Add("Phone", ArmSoft.SME.Common.Resources.PhoneNumber);
			Columns.Add("Email", ArmSoft.SME.Common.Resources.Email);
			Columns.Add("TaxCode", ArmSoft.SME.Common.Resources.OptionsTaxCode);
			Columns.Add("FullCaption", ArmSoft.SME.Common.Resources.FullCaption);
		}
		Columns.Add("B2BStatusName", ArmSoft.SME.Common.Resources.Partnerb2bStatus);
		if (_showColumnsModeType == ShowPartnersModeType.AllColumns)
		{
			Columns.Add("Type", ArmSoft.SME.Common.Resources.Group);
			Columns.Add("SettlAcc", ArmSoft.SME.Common.Resources.SettlementAccount);
			Columns.Add("Supplier", ArmSoft.SME.Common.Resources.Supplier);
			Columns.Add("Customer", ArmSoft.SME.Common.Resources.Buyer);
			Columns.Add("Address", ArmSoft.SME.Common.Resources.LegalAddresses);
			Columns.Add("BusinessAddress", ArmSoft.SME.Common.Resources.BusinessAddresses);
			Columns.Add("ManagerFIO", ArmSoft.SME.Common.Resources.ManagerFIO);
			Columns.Add("ManagerPost", ArmSoft.SME.Common.Resources.ManagerPost);
			Columns.Add("ChiefaccFIO", ArmSoft.SME.Common.Resources.ChAccFIO);
			Columns.Add("ChiefaccPost", ArmSoft.SME.Common.Resources.AccountantPost);
			Columns.Add("MainAIM", ArmSoft.SME.Common.Resources.PaymentMainAim);
			Columns.Add("Phone", ArmSoft.SME.Common.Resources.PhoneNumber);
			Columns.Add("Email", ArmSoft.SME.Common.Resources.Email);
			Columns.Add("PartStateRegister", ArmSoft.SME.Trade.Common.Resources.PartStateRegister);
			Columns.Add("Passport", ArmSoft.SME.Common.Resources.PassportNum);
			Columns.Add("FullCaption", ArmSoft.SME.Common.Resources.FullCaption);
		}
		if (_showColumnsModeType == ShowPartnersModeType.CodeCaption)
		{
			Columns.Add("FullCaption", ArmSoft.SME.Common.Resources.FullCaption);
		}
		AddPartnersConditionalFormatting();
	}

	public override short ValueMemberLength()
	{
		return 20;
	}

	public override string ValueMember(int index)
	{
		return Data[index].PartCode;
	}

	public override string CommentMember(int index)
	{
		if (_showColumnsModeType == ShowPartnersModeType.TINCode)
		{
			return Data[index].TaxCode;
		}
		return Data[index].Caption;
	}

	public override bool Validate(string value, ref string comment, ref string realValue)
	{
		PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(value);
		if (item == null)
		{
			return false;
		}
		comment = ((_showColumnsModeType == ShowPartnersModeType.TINCode) ? item.TaxCode : item.Caption);
		realValue = item.PartCode;
		return true;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.PartType = PartType;
		ParametersProvider.IsFilterBySuppliers = IsFilterBySuppliers;
		ParametersProvider.IsFilterByCustomers = IsFilterByCustomers;
		ParametersProvider.Partners = Partners;
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
		PartnerDocumentWindow partnerDocumentWindow = (isNew ? ((PartnerDocumentWindow)Settings.DirectoryService.CreatePOWindow(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner)) : ((PartnerDocumentWindow)Settings.DirectoryService.CreatePOWindow(Settings.DirectoryService.Load(ArmSoft.SME.Trade.Common.SmartEnums.DirectoryTypeSE.Partner, value))));
		partnerDocumentWindow.Owner = owner;
		partnerDocumentWindow.Top = top;
		partnerDocumentWindow.Left = left;
		if (partnerDocumentWindow.ShowDialog(isNew ? WindowViewMode.NewCreate : WindowViewMode.Edit).Value)
		{
			DataLoadedAtLeastOnce = false;
			result = true;
			value = partnerDocumentWindow.PO.Code;
			comment = ((_showColumnsModeType == ShowPartnersModeType.TINCode) ? partnerDocumentWindow.PO.TaxCode : partnerDocumentWindow.PO.Name);
		}
		return result;
	}

	public override bool FilteredSearch(ref string value, ref string comment, Window owner, double top, double left)
	{
		FilteredSearchWindow filteredSearchWindow = new FilteredSearchWindow(new FilteredSearch
		{
			FilterDataView = new PartnerGroups("", showAsTree: true, filteredSearch: true),
			SearchDataView = new PartnersFilter(ShowPartnersModeType.CodeCaption, "", IsFilterBySuppliers, IsFilterByCustomers),
			WindowStartupLocation = WindowStartupLocation.Manual
		}, owner, isBasketMode: false)
		{
			Top = top,
			Left = left
		};
		if (Convert.ToBoolean(filteredSearchWindow.ShowDialog()))
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(Convert.ToString(filteredSearchWindow.SimpleResult));
			if (item != null)
			{
				value = item.PartCode;
				if (_showColumnsModeType == ShowPartnersModeType.TINCode)
				{
					comment = item.TaxCode;
				}
				else
				{
					comment = item.Caption;
				}
			}
			return true;
		}
		return false;
	}

	private void AddPartnersConditionalFormatting()
	{
		UserDefinedFormat customFormat = new UserDefinedFormat
		{
			ForegroundColor = "#E50000"
		};
		AsCondititionFormat asCondititionFormat = new AsCondititionFormat
		{
			Expression = PartnerB2BStateExpression,
			FieldName = "TaxCode",
			FormatName = "Custom",
			CustomFormat = customFormat,
			IsFixed = true
		};
		AsCondititionFormat asCondititionFormat2 = new AsCondititionFormat
		{
			Expression = PartnerB2BStateExpression,
			FieldName = "B2BStatusName",
			FormatName = "Custom",
			CustomFormat = customFormat,
			IsFixed = true
		};
		ConditionalFormattingList.AddRange(new _003C_003Ez__ReadOnlyArray<AsFormatting>(new AsFormatting[2] { asCondititionFormat, asCondititionFormat2 }));
	}
}
