using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ArmSoft.Arcus2Client;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("ArmSoft.Arcus2Client.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string Accepted => ResourceManager.GetString("Accepted", resourceCulture);

	internal static string AccountRestricted => ResourceManager.GetString("AccountRestricted", resourceCulture);

	internal static string AmountIsNotCorrect => ResourceManager.GetString("AmountIsNotCorrect", resourceCulture);

	internal static string BadCard => ResourceManager.GetString("BadCard", resourceCulture);

	internal static string CallIssuer => ResourceManager.GetString("CallIssuer", resourceCulture);

	internal static string CallSecurity => ResourceManager.GetString("CallSecurity", resourceCulture);

	internal static string CannotProcessAmount => ResourceManager.GetString("CannotProcessAmount", resourceCulture);

	internal static string CardExpired => ResourceManager.GetString("CardExpired", resourceCulture);

	internal static string CardIsRestricted => ResourceManager.GetString("CardIsRestricted", resourceCulture);

	internal static string DuplicateTransaction => ResourceManager.GetString("DuplicateTransaction", resourceCulture);

	internal static string ExpiredCard => ResourceManager.GetString("ExpiredCard", resourceCulture);

	internal static string ForcedPostNoAccount => ResourceManager.GetString("ForcedPostNoAccount", resourceCulture);

	internal static string GetException => ResourceManager.GetString("GetException", resourceCulture);

	internal static string IncorrectPin => ResourceManager.GetString("IncorrectPin", resourceCulture);

	internal static string InitializationException => ResourceManager.GetString("InitializationException", resourceCulture);

	internal static string InsufficientFunds => ResourceManager.GetString("InsufficientFunds", resourceCulture);

	internal static string InvalidAccount => ResourceManager.GetString("InvalidAccount", resourceCulture);

	internal static string InvalidCard => ResourceManager.GetString("InvalidCard", resourceCulture);

	internal static string InvalidMerchantID => ResourceManager.GetString("InvalidMerchantID", resourceCulture);

	internal static string InvalidPin => ResourceManager.GetString("InvalidPin", resourceCulture);

	internal static string InvalidTransaction => ResourceManager.GetString("InvalidTransaction", resourceCulture);

	internal static string IsNotPermitted => ResourceManager.GetString("IsNotPermitted", resourceCulture);

	internal static string IssuerIsInoperable => ResourceManager.GetString("IssuerIsInoperable", resourceCulture);

	internal static string IssuerIsUnknown => ResourceManager.GetString("IssuerIsUnknown", resourceCulture);

	internal static string LimitReached => ResourceManager.GetString("LimitReached", resourceCulture);

	internal static string LostCard => ResourceManager.GetString("LostCard", resourceCulture);

	internal static string LostOriginalTransaction => ResourceManager.GetString("LostOriginalTransaction", resourceCulture);

	internal static string PickUpCard => ResourceManager.GetString("PickUpCard", resourceCulture);

	internal static string PinProcessingError => ResourceManager.GetString("PinProcessingError", resourceCulture);

	internal static string ReconcileError => ResourceManager.GetString("ReconcileError", resourceCulture);

	internal static string RequestInProgress => ResourceManager.GetString("RequestInProgress", resourceCulture);

	internal static string SetException => ResourceManager.GetString("SetException", resourceCulture);

	internal static string StolenCard => ResourceManager.GetString("StolenCard", resourceCulture);

	internal static string SystemError => ResourceManager.GetString("SystemError", resourceCulture);

	internal static string SystemMalfunction => ResourceManager.GetString("SystemMalfunction", resourceCulture);

	internal static string Timeout => ResourceManager.GetString("Timeout", resourceCulture);

	internal static string TransactionIsNotHonored => ResourceManager.GetString("TransactionIsNotHonored", resourceCulture);

	internal static string TransactionIsNotPermitted => ResourceManager.GetString("TransactionIsNotPermitted", resourceCulture);

	internal static string TryAgain => ResourceManager.GetString("TryAgain", resourceCulture);

	internal static string UnknownError => ResourceManager.GetString("UnknownError", resourceCulture);

	internal static string UnknownTransaction => ResourceManager.GetString("UnknownTransaction", resourceCulture);

	internal static string WithdrawalLimitExceeded => ResourceManager.GetString("WithdrawalLimitExceeded", resourceCulture);

	internal static string WrongMessage => ResourceManager.GetString("WrongMessage", resourceCulture);

	internal Resources()
	{
	}
}
