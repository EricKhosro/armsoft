using System;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.PersistableObject;
using ArmSoft.SME.Trade.Common.Models.Caches;
using ArmSoft.SME.Trade.Common.Models.Documents;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public class CreateBasedOnDocHelper
{
	public static void CreateDocument(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE docType, Guid baseISN, int dvp)
	{
		TradeDocument tradeDocument = (TradeDocument)Settings.DocumentService.Create(docType);
		tradeDocument.BaseISN = baseISN;
		tradeDocument.DocDate = Settings.HostingEnvironment.CurrentDate;
		IPOWindow iPOWindow = Settings.DocumentService.CreateWindow(tradeDocument);
		iPOWindow.DVPID = dvp;
		iPOWindow.Show();
	}

	internal static void CreateNewMTPODocCommon(TradeDocument baseDocument)
	{
		if (baseDocument == null)
		{
			throw new ASException(Resources.BaseDocNotExist);
		}
		MTPurchaseOrder mTPurchaseOrder = Settings.DocumentService.Create<MTPurchaseOrder>(ArmSoft.SME.Trade.Common.SmartEnums.DocumentTypeSE.MTPurchaseOrder);
		mTPurchaseOrder.PartnerContractGroup.PartnerCode = baseDocument.PartnerContractGroup.PartnerCode;
		if (string.IsNullOrWhiteSpace(mTPurchaseOrder.PartnerContractGroup.PartnerCode))
		{
			mTPurchaseOrder.POStates = POStatesSE.EmptySupplier.Value;
		}
		else
		{
			PartnerCacheRow item = TradeSettings.TradeCacheService.Partner.GetItem(mTPurchaseOrder.PartnerContractGroup.PartnerCode);
			if (item != null)
			{
				mTPurchaseOrder.PartnerContractGroup.PartnerCode = item.PartCode;
				mTPurchaseOrder.PartnerName = item.Caption;
				mTPurchaseOrder.SupplierEMail = item.Email;
			}
		}
		int num = 0;
		switch (baseDocument.POType)
		{
		case 7:
			foreach (MTIncomeBase.MTIncomeSpecificationRow mT in ((MTIncome)baseDocument).MTList)
			{
				mTPurchaseOrder.MTList.Add(new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
				{
					MTCode = mT.MTCode,
					MTID = mT.MTID,
					MTName = mT.MTName,
					UnitCode = mT.UnitCode,
					UnitBrief = mT.UnitBrief,
					Quantity = mT.Quantity,
					RowID = num
				});
				num++;
			}
			break;
		case 36:
			foreach (MTMoveBase.MTMoveSpecificationRow mT2 in ((MTMoveOrder)baseDocument).MTList)
			{
				mTPurchaseOrder.MTList.Add(new MTPurchaseOrder.MTPurchaseOrderSpecificationRow
				{
					MTCode = mT2.MTCode,
					MTID = mT2.MTID,
					MTName = mT2.MTName,
					UnitCode = mT2.UnitCode,
					UnitBrief = mT2.UnitBrief,
					Quantity = mT2.Quantity,
					RowID = num
				});
				num++;
			}
			break;
		}
		mTPurchaseOrder.MaxRowId = num;
		IPOWindow iPOWindow = Settings.DocumentService.CreateWindow(mTPurchaseOrder);
		if (Settings.HostingEnvironment.CurrentViewPanel != null)
		{
			iPOWindow.DVPID = Settings.HostingEnvironment.CurrentViewPanel.Id;
		}
		iPOWindow.Show();
	}
}
