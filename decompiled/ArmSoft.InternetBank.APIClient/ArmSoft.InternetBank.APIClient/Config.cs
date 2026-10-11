using System.Collections.Generic;

namespace ArmSoft.InternetBank.APIClient;

internal static class Config
{
	public const int Timeout = 90000;

	public static Dictionary<Bank, string> ApiUrls { get; set; }

	public static Dictionary<Bank, string> ApiCredentials { get; set; }

	static Config()
	{
		ApiUrls = new Dictionary<Bank, string>();
		ApiCredentials = new Dictionary<Bank, string>();
		ApiUrls[Bank.Ameria] = "https://online.ameriabank.am/InternetBank/Api";
		ApiCredentials[Bank.Ameria] = "MDVjOWFiOGQtYmVjNS00ZWVhLTljMDItMzk1NjExMGY3Y2ViOmgxZFMzczJZTmM5T0pWNjhOMkRRL1BLMw==";
		ApiUrls[Bank.Ararat] = "https://direct.araratbank.am/internetbank/Api";
		ApiCredentials[Bank.Ararat] = "YjE2ZmMyYzYtZGMzOC00Nzk1LWFjM2ItZGMwY2EyOWEyMTJjOkppK1B4akR5VUlSSFdaYk9IN25uUnVFbA==";
		ApiUrls[Bank.Amio] = "https://internetbank.amiobank.am/InternetBank/Api";
		ApiCredentials[Bank.Amio] = "NDQ2OWU2ZDktODgxZi00NDEwLWFlZWUtY2RhZjE0OTJlZjg1OlNSSnIrUlp1azlIRnJ1UlcwdGlRTXpMZXFEcnJCU1Fa";
		ApiUrls[Bank.ArmSwiss] = "https://client.armswissbank.am/InternetBank/Api";
		ApiCredentials[Bank.ArmSwiss] = "MDRiZDQxNjktMzJiNS00MDdlLWJhYWEtOTA4MDFhN2RkYmMzOkRyaEk1UTh1UmJRb2Z2TGZxT1JnMkh0RA==";
		ApiUrls[Bank.Byblos] = "https://online.byblosbankarmenia.am/InternetBank/Api";
		ApiCredentials[Bank.Byblos] = "YTAwMjcwZmYtMmE1YS00MGNjLWI3ZTUtODAxZWFkZTUzMGUyOm5Db1l0aXpXdFErQ25COThzWXFZNlpIbTMwMUp5YVRV";
		ApiUrls[Bank.Converse] = "https://ebanking.conversebank.am/InternetBank/Api";
		ApiCredentials[Bank.Converse] = "NGZhMjU2MWQtNDVmMS00MDk2LWJhYWUtMGJmZmY5YmRiMjdkOkIzYURFdW1lamFrSkFBVmxCS3VIZXNyMDMwMUF1Rm93";
		ApiUrls[Bank.Evoca] = "https://online.evoca.am/InternetBank/Api/";
		ApiCredentials[Bank.Evoca] = "MDNlNzFjMjctOTcwYi00NjNlLTg2YmMtY2YwYTNkMmZjZmIwOllzTlZ4KytFelU2eGw2UStiRFMyeFBkSw==";
		ApiUrls[Bank.Demo] = "https://internetbank.armsoft.am:9443/InternetBank/api/";
		ApiCredentials[Bank.Demo] = "OTM4MWVhYmYtZGE2YS00OThlLWI3ZjgtOTA2ZGNlZGViYTY2OmMvS1RQNVd5LzE5VjhCZmQ5cExwa0xLVg==";
	}
}
