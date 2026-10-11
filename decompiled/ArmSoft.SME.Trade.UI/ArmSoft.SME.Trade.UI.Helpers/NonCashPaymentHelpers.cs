using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Ardalis.SmartEnum;
using ArmSoft.SME.Common.Constants;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common.SmartEnums;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class NonCashPaymentHelpers
{
	public class NonCashPaymentItem
	{
		public string Caption => PaymentType.Caption;

		public ImageSource ItemImage
		{
			get
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				return ImageService.GetImage(PaymentType.ImagePath, isAppPathPreferred: false, "PaymentSystems/");
			}
		}

		public NonCashPaymentMethodsSE PaymentType { get; }

		public NonCashPaymentItem(NonCashPaymentMethodsSE paymentType)
		{
			PaymentType = paymentType;
		}
	}

	private static List<NonCashPaymentItem> nonCashPaymentItemsWithCardTypes;

	private static List<NonCashPaymentItem> nonCashPaymentItems;

	public static List<NonCashPaymentItem> GetNonCashPayments(bool withCardTypes = true)
	{
		if (nonCashPaymentItemsWithCardTypes == null)
		{
			nonCashPaymentItemsWithCardTypes = new List<NonCashPaymentItem>();
			nonCashPaymentItems = new List<NonCashPaymentItem>();
			NonCashPaymentItem item = new NonCashPaymentItem(NonCashPaymentMethodsSE.None);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
			item = new NonCashPaymentItem(NonCashPaymentMethodsSE.CardPayment);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
			nonCashPaymentItemsWithCardTypes.Add(new NonCashPaymentItem(NonCashPaymentMethodsSE.ARCA));
			nonCashPaymentItemsWithCardTypes.Add(new NonCashPaymentItem(NonCashPaymentMethodsSE.VISA));
			nonCashPaymentItemsWithCardTypes.Add(new NonCashPaymentItem(NonCashPaymentMethodsSE.MASTER));
			nonCashPaymentItemsWithCardTypes.Add(new NonCashPaymentItem(NonCashPaymentMethodsSE.AMEX));
			nonCashPaymentItemsWithCardTypes.Add(new NonCashPaymentItem(NonCashPaymentMethodsSE.MIR));
			item = new NonCashPaymentItem(NonCashPaymentMethodsSE.PayX);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
			item = new NonCashPaymentItem(NonCashPaymentMethodsSE.Idram);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
			item = new NonCashPaymentItem(NonCashPaymentMethodsSE.InecoPay);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
			item = new NonCashPaymentItem(NonCashPaymentMethodsSE.Telcell);
			nonCashPaymentItemsWithCardTypes.Add(item);
			nonCashPaymentItems.Add(item);
		}
		if (!withCardTypes)
		{
			return nonCashPaymentItems;
		}
		return nonCashPaymentItemsWithCardTypes;
	}

	public static int? GetECRPaymentSystem(ArmSoft.SME.Common.Constants.Common.NonCashPaymentType paymentMethod)
	{
		switch (paymentMethod)
		{
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.ARCA:
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.VISA:
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MASTER:
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.AMEX:
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.MIR:
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.CardPayment:
			return int.Parse("1");
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.PayX:
			return int.Parse("17");
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.Idram:
			return int.Parse("13");
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.InecoPay:
			return int.Parse("18");
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.EasyPay:
			return int.Parse("11");
		case ArmSoft.SME.Common.Constants.Common.NonCashPaymentType.Telcell:
			return int.Parse("10");
		default:
			return null;
		}
	}

	public static NonCashPaymentItem GetPaymentItem(short nonCashPaymentMethod)
	{
		if (SmartEnum<NonCashPaymentMethodsSE, short>.TryFromValue(nonCashPaymentMethod, out var nonCashPaymentMethodsSE))
		{
			return nonCashPaymentItemsWithCardTypes.FirstOrDefault((NonCashPaymentItem x) => x.PaymentType == nonCashPaymentMethodsSE);
		}
		return null;
	}
}
