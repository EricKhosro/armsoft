using System.Runtime.InteropServices;

namespace ArmSoft.InternetBank.Web.API.Common.Models;

[ComVisible(true)]
[Guid("B9F7B598-8BFD-4EC8-B2F1-548BC9740F8F")]
[ClassInterface(ClassInterfaceType.AutoDual)]
public class FieldError
{
	public string Field { get; set; }

	public string Field2 { get; set; }

	public string Message { get; set; }

	public string Reason { get; set; }

	public string DecodedMessage { get; set; }

	public FieldError()
		: this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
	{
	}

	public FieldError(string field, string field2, string message, string reason, string decodedMessage)
	{
		Field = field;
		Field2 = field2;
		Message = message;
		Reason = reason;
		DecodedMessage = decodedMessage;
	}
}
