using System;

namespace ArmSoft.Cloud.Client.Discount;

public class AppSecret
{
	public Guid AppId { get; set; }

	public string Secret { get; set; }

	public short PartId { get; set; }

	public string Description { get; set; }

	public bool IsEnabled { get; set; }

	public string ProductsAccess { get; set; }

	public DateTime? Expiration { get; set; }

	public DateTime? Accepted { get; set; }

	public DateTime CreationDate { get; set; }
}
