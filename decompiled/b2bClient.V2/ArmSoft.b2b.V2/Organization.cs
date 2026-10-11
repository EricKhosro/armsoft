using System;

namespace ArmSoft.b2b.V2;

public class Organization
{
	public int Id { get; set; }

	public string FullName { get; set; }

	public string TaxCode { get; set; }

	public Address LegalAddress { get; set; }

	public Address BusinessAddress { get; set; }

	public Contact Manager { get; set; }

	public Contact Accountant { get; set; }

	public string MailAddress { get; set; }

	public string Description { get; set; }

	public bool IsVatPayer { get; set; }

	public int TaxationMode { get; set; }

	public string LegalStatus { get; set; }

	public string LegalStatusShort { get; set; }

	public StateEnum State { get; set; }

	public int StateExtended { get; set; }

	public DateTime LastValidDate { get; set; }
}
