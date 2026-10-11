using System.Collections.Generic;

namespace ArmSoft.b2b.V2;

public class SalesPoint
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string TaxCode { get; set; }

	public Address Address { get; set; }

	public string MailAddress { get; set; }

	public string Description { get; set; }

	public int Space { get; set; }

	public int Sphere { get; set; }

	public StateEnum State { get; set; }

	public List<Contact> Contacts { get; set; }
}
