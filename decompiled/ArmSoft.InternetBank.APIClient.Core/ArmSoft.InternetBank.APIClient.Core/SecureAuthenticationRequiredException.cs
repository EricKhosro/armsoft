namespace ArmSoft.InternetBank.APIClient.Core;

public class SecureAuthenticationRequiredException : BadRequestException
{
	public string Description { get; private set; }

	public string Challenge { get; private set; }

	public string AuthenticationMode { get; private set; }

	public string SecurityProvider { get; private set; }

	public bool HasDeviceWithIntegratedOtp { get; private set; }

	public string TokenSerial { get; private set; }

	public SecureAuthenticationRequiredException(string description, string challenge, string authenticationMode, string securityProvider, bool hasDeviceWithIntegratedOtp, string tokenSerial)
		: base("secure_authentication_required")
	{
		Description = description;
		Challenge = challenge;
		AuthenticationMode = authenticationMode;
		SecurityProvider = securityProvider;
		HasDeviceWithIntegratedOtp = hasDeviceWithIntegratedOtp;
		TokenSerial = tokenSerial;
	}
}
