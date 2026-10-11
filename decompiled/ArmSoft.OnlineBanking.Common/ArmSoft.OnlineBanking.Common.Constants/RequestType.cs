using ArmSoft.OnlineBanking.Common.Attributes;
using ArmSoft.OnlineBanking.Common.Enums;

namespace ArmSoft.OnlineBanking.Common.Constants;

public static class RequestType
{
	[RequestType(ObjectType = RequestObjectType.Role, Permission = "VERROLES")]
	public const string RoleCreate = "RQRC";

	[RequestType(ObjectType = RequestObjectType.Role, Permission = "VERROLES")]
	public const string RoleEdit = "RQRE";

	[RequestType(ObjectType = RequestObjectType.Role, Permission = "VERROLES")]
	public const string RoleDelete = "RQRD";

	[RequestType(ObjectType = RequestObjectType.News, Permission = "VERNEWS")]
	public const string NewsCreate = "RQNC";

	[RequestType(ObjectType = RequestObjectType.News, Permission = "VERNEWS")]
	public const string NewsModify = "RQNM";

	[RequestType(ObjectType = RequestObjectType.News, Permission = "VERNEWS")]
	public const string NewsDelete = "RQND";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUCREATE")]
	public const string UserCreate = "RQUC";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUEDIT")]
	public const string UserModify = "RQUE";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUEDIT")]
	public const string UserDelete = "RQUD";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUPRST")]
	public const string UserResetPassword = "RQUR";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUSPD")]
	public const string UserDeactivateSecurityProvider = "RQUDS";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUSPRST")]
	public const string UserChangeSecurityProvider = "RQUCS";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUUNLK")]
	public const string UserUnlock = "RQUU";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERUREG")]
	public const string UserRegistration = "RQURG";

	[RequestType(ObjectType = RequestObjectType.User, Permission = null)]
	public const string UserTransitionToFullUser = "RQUTF";

	[RequestType(ObjectType = RequestObjectType.UserTemplate, Permission = "VERUTCREAT")]
	public const string UserTemplateCreate = "RQUTC";

	[RequestType(ObjectType = RequestObjectType.UserTemplate, Permission = "VERUTEDIT")]
	public const string UserTemplateEdit = "RQUTE";

	[RequestType(ObjectType = RequestObjectType.UserTemplate, Permission = "VERUTEDIT")]
	public const string UserTemplateDelete = "RQUTD";

	[RequestType(ObjectType = RequestObjectType.Client, Permission = "VERCEDIT")]
	public const string ClientModify = "RQCE";

	[RequestType(ObjectType = RequestObjectType.Client, Permission = "VERCDEAC")]
	public const string ClientDeactivate = "RQCD";

	[RequestType(ObjectType = RequestObjectType.User, Permission = "VERAEDIT")]
	public const string AssociationsModify = "RQAM";

	[RequestType(ObjectType = RequestObjectType.Digipass, Permission = "VERDPS")]
	public const string DigipassReset = "RQDR";

	[RequestType(ObjectType = RequestObjectType.Digipass, Permission = "VERDPSCHS")]
	public const string DigipassChangeStatus = "RQDE";

	[RequestType(ObjectType = RequestObjectType.Digipass, Permission = "VERDPSD")]
	public const string DigipassDelete = "RQDD";

	[RequestType(ObjectType = RequestObjectType.AuthenticatorToken, Permission = "VERAUT")]
	public const string AuthenticatorTokenModify = "RQTE";

	[RequestType(ObjectType = RequestObjectType.AuthenticatorToken, Permission = "VERAUT")]
	public const string AuthenticatorTokenDelete = "RQTD";

	[RequestType(ObjectType = RequestObjectType.Param, Permission = "VERPARAM")]
	public const string ParamEdit = "RQPE";

	[RequestType(ObjectType = RequestObjectType.RateDeviation, Permission = null)]
	public const string RateDeviationModify = "RQRDM";

	[RequestType(ObjectType = RequestObjectType.RateDeviation, Permission = null)]
	public const string RateDeviationCreate = "RQRDC";

	[RequestType(ObjectType = RequestObjectType.RateDeviation, Permission = null)]
	public const string RateDeviationDelete = "RQRDD";

	[RequestType(ObjectType = RequestObjectType.ClientLimit, Permission = "VERCLEDIT")]
	public const string ClientLimitModify = "RQCLM";

	[RequestType(ObjectType = RequestObjectType.ClientLimit, Permission = "VERCLEDIT")]
	public const string ClientLimitCreate = "RQCLC";

	[RequestType(ObjectType = RequestObjectType.ClientLimit, Permission = "VERCLEDIT")]
	public const string ClientLimitDelete = "RQCLD";

	[RequestType(ObjectType = RequestObjectType.AuthenticationAvailability, Permission = null)]
	public const string AuthenticationAvailabilityModify = "RQAAM";

	[RequestType(ObjectType = RequestObjectType.AuthenticationAvailability, Permission = null)]
	public const string AuthenticationAvailabilityCreate = "RQAAC";

	[RequestType(ObjectType = RequestObjectType.AuthenticationAvailability, Permission = null)]
	public const string AuthenticationAvailabilityDelete = "RQAAD";

	[RequestType(ObjectType = RequestObjectType.RateSuggestionAvailability, Permission = null)]
	public const string RateSuggestionAvailabilityModify = "RQRAM";

	[RequestType(ObjectType = RequestObjectType.RateSuggestionAvailability, Permission = null)]
	public const string RateSuggestionAvailabilityCreate = "RQRAC";

	[RequestType(ObjectType = RequestObjectType.RateSuggestionAvailability, Permission = null)]
	public const string RateSuggestionAvailabilityDelete = "RQRAD";
}
