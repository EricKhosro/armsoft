using System;

namespace ArmSoft.b2b.V2;

public class ActorCombined
{
	public int Id { get; set; }

	public DateTime CreationDate { get; set; }

	public string Name { get; set; }

	public string FullName { get; set; }

	public string TaxCode { get; set; }

	public byte[] Logo { get; set; }

	public Address BusinessAddress { get; set; }

	public string Description { get; set; }

	public DateTime? LastUpdateDate { get; set; }

	public string[] MailAddresses { get; set; }

	public string Website { get; set; }

	public string PhoneNumber { get; set; }
}
