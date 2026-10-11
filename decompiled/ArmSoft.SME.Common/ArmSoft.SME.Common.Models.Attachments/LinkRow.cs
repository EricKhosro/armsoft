namespace ArmSoft.SME.Common.Models.Attachments;

public class LinkRow : BaseRow
{
	public string Link { get; set; }

	public LinkRow()
	{
		Link = "";
	}

	public LinkRow Clone()
	{
		return new LinkRow
		{
			Description = Description,
			Link = Link,
			IsNew = IsNew
		};
	}
}
