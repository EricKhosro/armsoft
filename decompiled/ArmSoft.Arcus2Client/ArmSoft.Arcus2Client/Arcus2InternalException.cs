namespace ArmSoft.Arcus2Client;

internal class Arcus2InternalException : Arcus2Exception
{
	internal Arcus2InternalException(string message)
		: base("Arcus2 Error: " + message)
	{
	}

	internal Arcus2InternalException(Constants.Arcus2InternalException exceptionType)
		: base("Arcus2 Error: " + GetInternalExceptionMessage(exceptionType))
	{
		IsInternalException = true;
	}

	internal Arcus2InternalException(Constants.Arcus2ResponseCode responseCode)
		: base($"Arcus2 Error code {(int)responseCode}: {GetErrorMessageByResponseCode(responseCode)}")
	{
		Code = (int)responseCode;
	}

	private static string GetInternalExceptionMessage(Constants.Arcus2InternalException exceptionType)
	{
		return exceptionType switch
		{
			Constants.Arcus2InternalException.InitializationException => Resources.InitializationException, 
			Constants.Arcus2InternalException.SetException => Resources.SetException, 
			Constants.Arcus2InternalException.GetException => Resources.GetException, 
			_ => Resources.UnknownError, 
		};
	}

	private static string GetErrorMessageByResponseCode(Constants.Arcus2ResponseCode responseCode)
	{
		switch (responseCode)
		{
		case Constants.Arcus2ResponseCode.Accepted1:
		case Constants.Arcus2ResponseCode.Accepted2:
		case Constants.Arcus2ResponseCode.Accepted3:
		case Constants.Arcus2ResponseCode.Accepted4:
			return Resources.Accepted;
		case Constants.Arcus2ResponseCode.AccountRestricted:
			return Resources.AccountRestricted;
		case Constants.Arcus2ResponseCode.AmountIsNotCorrect:
			return Resources.AmountIsNotCorrect;
		case Constants.Arcus2ResponseCode.BadCard:
			return Resources.BadCard;
		case Constants.Arcus2ResponseCode.CallIssuer1:
		case Constants.Arcus2ResponseCode.CallIssuer2:
			return Resources.CallIssuer;
		case Constants.Arcus2ResponseCode.CallSecurity1:
		case Constants.Arcus2ResponseCode.CallSecurity2:
			return Resources.CallSecurity;
		case Constants.Arcus2ResponseCode.CannotProcessAmount:
			return Resources.CannotProcessAmount;
		case Constants.Arcus2ResponseCode.CardExpired:
			return Resources.CardExpired;
		case Constants.Arcus2ResponseCode.CardIsRestricted:
			return Resources.CardIsRestricted;
		case Constants.Arcus2ResponseCode.DuplicateTransaction:
			return Resources.DuplicateTransaction;
		case Constants.Arcus2ResponseCode.ExpiredCard:
			return Resources.ExpiredCard;
		case Constants.Arcus2ResponseCode.ForcedPostNoAccount:
			return Resources.ForcedPostNoAccount;
		case Constants.Arcus2ResponseCode.IncorrectPin:
			return Resources.IncorrectPin;
		case Constants.Arcus2ResponseCode.InsufficientFunds:
			return Resources.InsufficientFunds;
		case Constants.Arcus2ResponseCode.InvalidAccount:
			return Resources.InvalidAccount;
		case Constants.Arcus2ResponseCode.InvalidCard1:
		case Constants.Arcus2ResponseCode.InvalidCard2:
			return Resources.InvalidCard;
		case Constants.Arcus2ResponseCode.InvalidMerchantID:
			return Resources.InvalidMerchantID;
		case Constants.Arcus2ResponseCode.InvalidPin:
			return Resources.InvalidPin;
		case Constants.Arcus2ResponseCode.InvalidTransaction:
			return Resources.InvalidTransaction;
		case Constants.Arcus2ResponseCode.IsNotPermitted:
			return Resources.IsNotPermitted;
		case Constants.Arcus2ResponseCode.IssuerIsInoperable1:
		case Constants.Arcus2ResponseCode.IssuerIsInoperable2:
			return Resources.IssuerIsInoperable;
		case Constants.Arcus2ResponseCode.IssuerIsUnknown1:
		case Constants.Arcus2ResponseCode.IssuerIsUnknown2:
			return Resources.IssuerIsUnknown;
		case Constants.Arcus2ResponseCode.LimitReached:
			return Resources.LimitReached;
		case Constants.Arcus2ResponseCode.LostCard:
			return Resources.LostCard;
		case Constants.Arcus2ResponseCode.LostOriginalTransaction:
			return Resources.LostOriginalTransaction;
		case Constants.Arcus2ResponseCode.PickUpCard:
			return Resources.PickUpCard;
		case Constants.Arcus2ResponseCode.PinProcessingError2:
		case Constants.Arcus2ResponseCode.PinProcessingError3:
		case Constants.Arcus2ResponseCode.PinProcessingError4:
		case Constants.Arcus2ResponseCode.PinProcessingError1:
			return Resources.PinProcessingError;
		case Constants.Arcus2ResponseCode.ReconcileError:
			return Resources.ReconcileError;
		case Constants.Arcus2ResponseCode.RequestInProgress:
			return Resources.RequestInProgress;
		case Constants.Arcus2ResponseCode.StolenCard:
			return Resources.StolenCard;
		case Constants.Arcus2ResponseCode.SystemError:
			return Resources.SystemError;
		case Constants.Arcus2ResponseCode.SystemMalfunction:
			return Resources.SystemMalfunction;
		case Constants.Arcus2ResponseCode.Timeout:
			return Resources.Timeout;
		case Constants.Arcus2ResponseCode.TransactionIsNotHonored:
			return Resources.TransactionIsNotHonored;
		case Constants.Arcus2ResponseCode.TransactionIsNotPermitted:
			return Resources.TransactionIsNotPermitted;
		case Constants.Arcus2ResponseCode.TryAgain:
			return Resources.TryAgain;
		case Constants.Arcus2ResponseCode.UnknownTransaction:
			return Resources.UnknownTransaction;
		case Constants.Arcus2ResponseCode.WithdrawalLimitExceeded:
			return Resources.WithdrawalLimitExceeded;
		case Constants.Arcus2ResponseCode.WrongMessage:
			return Resources.WrongMessage;
		default:
			return Resources.UnknownError;
		}
	}
}
