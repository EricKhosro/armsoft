using ArmSoft.SME.Common.Enums;

namespace ArmSoft.SME.Common.Models.Directories;

public class DirectoryNextCodeRequestModel
{
	public NextCodeOfferSource OfferSource { get; set; }

	public string Prefix { get; set; }

	public int MaxLength { get; set; }
}
