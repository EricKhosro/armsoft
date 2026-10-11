using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Armsoft.PrintTemplates2.Resources;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "18.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class Resource
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
				resourceMan = new ResourceManager("Armsoft.PrintTemplates2.Resources.Resource", typeof(Resource).Assembly);
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

	public static string AdditionalColumnExceed => ResourceManager.GetString("AdditionalColumnExceed", resourceCulture);

	public static string ColumnExists => ResourceManager.GetString("ColumnExists", resourceCulture);

	public static string ColumnNameEmpty => ResourceManager.GetString("ColumnNameEmpty", resourceCulture);

	public static string ColumnNotExists => ResourceManager.GetString("ColumnNotExists", resourceCulture);

	public static string InvalidColumn => ResourceManager.GetString("InvalidColumn", resourceCulture);

	public static string InvalidConvertValue => ResourceManager.GetString("InvalidConvertValue", resourceCulture);

	public static string InvalidGridSubstitution => ResourceManager.GetString("InvalidGridSubstitution", resourceCulture);

	public static string InvalidSubstitutionElement => ResourceManager.GetString("InvalidSubstitutionElement", resourceCulture);

	public static string InvalidTextTemplateRow => ResourceManager.GetString("InvalidTextTemplateRow", resourceCulture);

	public static string SideBySideTables => ResourceManager.GetString("SideBySideTables", resourceCulture);

	public static string UnableToCastTimeSpan => ResourceManager.GetString("UnableToCastTimeSpan", resourceCulture);

	public static string WrongFileExtension => ResourceManager.GetString("WrongFileExtension", resourceCulture);

	internal Resource()
	{
	}
}
