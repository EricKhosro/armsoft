namespace ArmSoft.EMarkReceiptClient.Exceptions;

public enum EMarkErrorCode
{
	Unknown = 0,
	BadRequest = 400,
	Unauthorized = 401,
	NotFound = 404,
	UnprocessableEntity = 422,
	InternalServerError = 500
}
