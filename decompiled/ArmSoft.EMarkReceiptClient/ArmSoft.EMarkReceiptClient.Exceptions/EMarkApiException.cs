using System;
using System.Net;
using ArmSoft.EMarkReceiptClient.Properties;

namespace ArmSoft.EMarkReceiptClient.Exceptions;

public class EMarkApiException : Exception
{
	public enum ReceiptErrorCodes
	{
		Unknown,
		RCT_001,
		RCT_002,
		RCT_003,
		RCT_004,
		RCT_005,
		RCT_006,
		RCT_007,
		RCT_008,
		RCT_009,
		RCT_010,
		RCT_011,
		RCT_012,
		RCT_013,
		RCT_014
	}

	private const string _errorPrefix = "E-Mark սխալ. ";

	public HttpStatusCode StatusCode { get; }

	public EMarkErrorCode ErrorCode { get; }

	public string ResponseContent { get; }

	public EMarkApiException(EMarkErrorDetails details)
		: base(details.Message)
	{
		StatusCode = details.StatusCode;
		ErrorCode = details.ErrorCode;
		ResponseContent = details.ResponseContent;
	}

	public EMarkApiException(HttpStatusCode statusCode, string responseContent)
		: this(BuildDetails(statusCode, responseContent))
	{
	}

	private static EMarkErrorDetails BuildDetails(HttpStatusCode statusCode, string responseContent)
	{
		EMarkErrorCode errorCode = EMarkErrorCode.Unknown;
		string text;
		switch (statusCode)
		{
		case HttpStatusCode.BadRequest:
			errorCode = EMarkErrorCode.BadRequest;
			text = Resources.BadRequest;
			break;
		case HttpStatusCode.Unauthorized:
			errorCode = EMarkErrorCode.Unauthorized;
			text = Resources.Unauthorized;
			break;
		case HttpStatusCode.NotFound:
			errorCode = EMarkErrorCode.NotFound;
			text = Resources.NotFound;
			break;
		case HttpStatusCode.UnprocessableEntity:
			errorCode = EMarkErrorCode.UnprocessableEntity;
			text = Resources.UnprocessableEntity;
			break;
		case HttpStatusCode.InternalServerError:
			errorCode = EMarkErrorCode.InternalServerError;
			text = Resources.InternalServerError;
			break;
		default:
			text = $"{(int)statusCode} ({statusCode}).";
			break;
		}
		if (!string.IsNullOrWhiteSpace(responseContent))
		{
			text = text + " Response: " + responseContent;
		}
		return new EMarkErrorDetails
		{
			StatusCode = statusCode,
			ErrorCode = errorCode,
			Message = "E-Mark սխալ. " + text,
			ResponseContent = (responseContent ?? string.Empty)
		};
	}

	public static void ThrowReceiptError(string errorCode, string item, string datamatrix)
	{
		string text = GetErrorMessageByResponseCode(ParseReceiptResponseCode(errorCode));
		if (!string.IsNullOrWhiteSpace(item))
		{
			text = text + Environment.NewLine + item;
		}
		if (!string.IsNullOrWhiteSpace(datamatrix))
		{
			text = text + Environment.NewLine + "DataMatrix - \"" + datamatrix + "\"";
		}
		throw new EMarkApiException(new EMarkErrorDetails
		{
			Message = text
		});
	}

	private static ReceiptErrorCodes ParseReceiptResponseCode(string code)
	{
		if (!Enum.TryParse<ReceiptErrorCodes>(code, out var result))
		{
			return ReceiptErrorCodes.Unknown;
		}
		return result;
	}

	private static string GetErrorMessageByResponseCode(ReceiptErrorCodes responseCode)
	{
		string value = responseCode switch
		{
			ReceiptErrorCodes.RCT_001 => Resources.RCT_001_CodeNotFound, 
			ReceiptErrorCodes.RCT_002 => Resources.RCT_002_InvalidReceiptCode, 
			ReceiptErrorCodes.RCT_003 => Resources.RCT_003_UnknownReceiptCode, 
			ReceiptErrorCodes.RCT_004 => Resources.RCT_004_WithdrawnReceiptCode, 
			ReceiptErrorCodes.RCT_005 => Resources.RCT_005_IntroducedReceiptCode, 
			ReceiptErrorCodes.RCT_006 => Resources.RCT_006_IntroducedReturnedCode, 
			ReceiptErrorCodes.RCT_007 => Resources.RCT_007_AlreadyPrintedCode, 
			ReceiptErrorCodes.RCT_008 => Resources.RCT_008_AlreadyReturned, 
			ReceiptErrorCodes.RCT_009 => Resources.RCT_009_NotLocalCode, 
			ReceiptErrorCodes.RCT_010 => Resources.RCT_010_CorruptedCode, 
			ReceiptErrorCodes.RCT_011 => Resources.RCT_011_EmittedCode, 
			ReceiptErrorCodes.RCT_012 => Resources.RCT_012_AppliedCode, 
			ReceiptErrorCodes.RCT_013 => Resources.RCT_013_WrittenOffCode, 
			ReceiptErrorCodes.RCT_014 => Resources.RCT_014_DisaggregatedCode, 
			_ => Resources.UnknownError, 
		};
		return $"{"E-Mark սխալ. "}{responseCode} - {value}";
	}
}
