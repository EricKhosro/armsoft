namespace ArmSoft.InternetBank.Web.API.Common.Models;

public class PaymentType
{
	public string Id { get; set; }

	public string Title { get; set; }

	public PaymentType()
	{
	}

	public PaymentType(string id, string title)
	{
		Id = id;
		Title = title;
	}
}
