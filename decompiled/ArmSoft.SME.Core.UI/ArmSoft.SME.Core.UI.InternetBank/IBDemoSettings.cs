namespace ArmSoft.SME.Core.UI.InternetBank;

public class IBDemoSettings
{
	public bool UseDemo { get; set; }

	public string Credential { get; set; } = "OTM4MWVhYmYtZGE2YS00OThlLWI3ZjgtOTA2ZGNlZGViYTY2OmMvS1RQNVd5LzE5VjhCZmQ5cExwa0xLVg==";

	public string Service777URL { get; set; } = "https://internetbank.armsoft.am:7443/Demo/777/InternetBank/api";

	public string Service888URL { get; set; } = "https://internetbank.armsoft.am:7443/Demo/888/InternetBank/api";
}
