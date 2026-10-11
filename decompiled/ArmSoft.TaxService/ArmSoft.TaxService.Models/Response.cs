namespace ArmSoft.TaxService.Models;

public class Response<T>
{
	public bool ok { get; set; }

	public T payload { get; set; }

	public FailureResponse failure { get; set; }
}
