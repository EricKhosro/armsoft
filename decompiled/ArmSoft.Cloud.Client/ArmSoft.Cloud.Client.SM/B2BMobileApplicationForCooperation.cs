using System;

namespace ArmSoft.Cloud.Client.SM;

public class B2BMobileApplicationForCooperation
{
	public int CooperationId { get; set; }

	public int SalesPointId { get; set; }

	public string Phone { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Email { get; set; }

	public MobileUserApplicationState State { get; set; }

	public string Comment { get; set; }

	public DateTime CreationDate { get; set; }
}
