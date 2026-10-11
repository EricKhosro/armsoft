using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Enums;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

internal class OfflineModeHelper
{
	public static MTBill CreateOfflineModeMTBillDocument()
	{
		return new MTBill
		{
			POType = DocumentTypeSE.MTBill,
			POName = DocumentTypeSE.MTBill.Name,
			OwnerID = Settings.LoginService.UserId,
			ISN = Guid.NewGuid(),
			StoredInDb = false
		};
	}

	public static MTBillReturn CreateOfflineModeMTBillReturnDocument()
	{
		return new MTBillReturn
		{
			POType = DocumentTypeSE.MTBillReturn,
			POName = DocumentTypeSE.MTBillReturn.Name,
			OwnerID = Settings.LoginService.UserId,
			ISN = Guid.NewGuid(),
			StoredInDb = false
		};
	}

	public static MTReturnFromCustomer CreateOfflineModeMTReturnFromCustomerDocument()
	{
		return new MTReturnFromCustomer
		{
			POType = DocumentTypeSE.MTReturnFromCustomer,
			POName = DocumentTypeSE.MTReturnFromCustomer.Name,
			OwnerID = Settings.LoginService.UserId,
			ISN = Guid.NewGuid(),
			StoredInDb = false
		};
	}

	public static void UpdateOfflineModeNextDocumentNumber(short docType)
	{
		DocumentTypeSE docType2 = SmartEnum<DocumentTypeSE, short>.List.First((DocumentTypeSE dc) => dc.Value == docType);
		KeyValuePair<string, DocNumOfferSource> nextDocumentNumber = TradeSettings.LocalDBService.GetNextDocumentNumber(docType2);
		string key = StringExtensions.NextDocNum(nextDocumentNumber.Key, 12);
		KeyValuePair<string, DocNumOfferSource> value = new KeyValuePair<string, DocNumOfferSource>(key, nextDocumentNumber.Value);
		TradeSettings.LocalDBService.SetNextDocumentNumber(docType2, value);
	}
}
