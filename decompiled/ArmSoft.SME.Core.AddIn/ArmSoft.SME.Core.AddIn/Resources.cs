using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ArmSoft.SME.Core.AddIn;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("ArmSoft.SME.Core.AddIn.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string Accuracy => ResourceManager.GetString("Accuracy", resourceCulture);

	public static string Armenian => ResourceManager.GetString("Armenian", resourceCulture);

	public static string b2bLoginButton => ResourceManager.GetString("b2bLoginButton", resourceCulture);

	public static string CalculateActiveSheet => ResourceManager.GetString("CalculateActiveSheet", resourceCulture);

	public static string CalculateAllSheets => ResourceManager.GetString("CalculateAllSheets", resourceCulture);

	public static string Calculation => ResourceManager.GetString("Calculation", resourceCulture);

	public static string Cancel => ResourceManager.GetString("Cancel", resourceCulture);

	public static string CurrentDate => ResourceManager.GetString("CurrentDate", resourceCulture);

	public static string DatabaseNotConnected => ResourceManager.GetString("DatabaseNotConnected", resourceCulture);

	public static string DatabaseNotSelected => ResourceManager.GetString("DatabaseNotSelected", resourceCulture);

	public static string DecodeFormulas => ResourceManager.GetString("DecodeFormulas", resourceCulture);

	public static string DecodeTitle => ResourceManager.GetString("DecodeTitle", resourceCulture);

	public static string DecodingError => ResourceManager.GetString("DecodingError", resourceCulture);

	public static string DecodingOK => ResourceManager.GetString("DecodingOK", resourceCulture);

	public static string DegreeOfRound => ResourceManager.GetString("DegreeOfRound", resourceCulture);

	public static string EncodeFormulas => ResourceManager.GetString("EncodeFormulas", resourceCulture);

	public static string EncodeTitle => ResourceManager.GetString("EncodeTitle", resourceCulture);

	public static string EncodingOK => ResourceManager.GetString("EncodingOK", resourceCulture);

	public static string English => ResourceManager.GetString("English", resourceCulture);

	public static string FileDoesNotExist => ResourceManager.GetString("FileDoesNotExist", resourceCulture);

	public static string Login => ResourceManager.GetString("Login", resourceCulture);

	public static string Ok => ResourceManager.GetString("Ok", resourceCulture);

	public static string Options => ResourceManager.GetString("Options", resourceCulture);

	public static string OptionsLang => ResourceManager.GetString("OptionsLang", resourceCulture);

	public static string PasteValues => ResourceManager.GetString("PasteValues", resourceCulture);

	public static string ReportPeriod => ResourceManager.GetString("ReportPeriod", resourceCulture);

	public static string SaveUser => ResourceManager.GetString("SaveUser", resourceCulture);

	public static string SelectDatabase => ResourceManager.GetString("SelectDatabase", resourceCulture);

	public static string User => ResourceManager.GetString("User", resourceCulture);

	public static string ValidateWrongPeriod => ResourceManager.GetString("ValidateWrongPeriod", resourceCulture);

	internal Resources()
	{
	}
}
