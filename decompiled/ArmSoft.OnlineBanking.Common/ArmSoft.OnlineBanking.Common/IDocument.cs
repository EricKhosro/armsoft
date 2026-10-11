using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common;

public interface IDocument
{
	int? ClientId { get; set; }

	string Type { get; set; }

	DocumentState State { get; set; }

	int? PartnerClientId { get; set; }

	bool IsClientSignatureRequired { get; set; }

	bool IsSignedByPartner { get; set; }

	bool IsSignableInGroup { get; set; }

	bool IsEditableByPartner { get; set; }
}
