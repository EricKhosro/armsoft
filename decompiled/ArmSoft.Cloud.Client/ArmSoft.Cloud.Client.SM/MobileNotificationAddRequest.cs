using System;

namespace ArmSoft.Cloud.Client.SM;

public class MobileNotificationAddRequest
{
	public int? SalesPointId { get; set; }

	public string Title { get; set; } = "";

	public string Text { get; set; } = "";

	public string Link { get; set; } = "";

	public DocumentType? DocType { get; set; }

	public Guid? DocId { get; set; }

	public MobileNotificationChannel Channel { get; set; }
}
