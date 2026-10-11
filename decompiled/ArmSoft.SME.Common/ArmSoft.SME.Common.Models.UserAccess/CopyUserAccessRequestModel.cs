namespace ArmSoft.SME.Common.Models.UserAccess;

public class CopyUserAccessRequestModel
{
	public int DestId { get; set; }

	public int SourceId { get; set; }

	public bool CopyUserPermissions { get; set; }

	public bool CopyStorageAccess { get; set; }

	public bool CopyCashesAccess { get; set; }

	public bool CopyDocOptions { get; set; }

	public bool CopyMTFavourites { get; set; }
}
