using ArmSoft.EMarkReceiptClient.Configuration;

namespace ArmSoft.EMarkReceiptClient.Properties;

public static class Resources
{
	public static string BadRequest
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Bad Request";
				}
				return "Bad Request";
			}
			return "Հարցման տվյալների սխալ, տվյալ receipt_id-ով տեղեկատվություն չի գտնվել";
		}
	}

	public static string InternalServerError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Internal Server Error";
				}
				return "Internal Server Error";
			}
			return "Սերվերի ներքին սխալ․ Փորձեք ավելի ուշ";
		}
	}

	public static string NotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Not Found";
				}
				return "Not Found";
			}
			return "Ռեսուրսը չի գտնվել";
		}
	}

	public static string Unauthorized
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unauthorized";
				}
				return "Unauthorized";
			}
			return "Չթույլատրված դիմում";
		}
	}

	public static string UnprocessableEntity
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unprocessable Entity ";
				}
				return "Unprocessable Entity ";
			}
			return "Տեղեկատվությունն ամբողջությամբ վերադարձված է";
		}
	}

	public static string RCT_001_CodeNotFound
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Code not found";
				}
				return "Code not found";
			}
			return "Ծածկագիրը չի գտնվել";
		}
	}

	public static string RCT_002_InvalidReceiptCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt contains invalid code(s)";
				}
				return "Receipt contains invalid code(s) - deprecated";
			}
			return "Սխալ ծածկագիր";
		}
	}

	public static string RCT_004_WithdrawnReceiptCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is withdrawn";
				}
				return "Receipt code is withdrawn";
			}
			return "Ծածկագիրը «Շրջանառությունից հանված, վաճառված» է";
		}
	}

	public static string RCT_003_UnknownReceiptCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is unknown";
				}
				return "Receipt code is unknown";
			}
			return "Անհայտ ծածկագիր";
		}
	}

	public static string RCT_007_AlreadyPrintedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is already printed";
				}
				return "Receipt code is already printed";
			}
			return "Ծածկագիրն արդեն ուղարկված է";
		}
	}

	public static string RCT_005_IntroducedReceiptCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is introduced";
				}
				return "Receipt code is introduced";
			}
			return "Ծածկագիրը «Շրջանառության մեջ դրված» է";
		}
	}

	public static string RCT_006_IntroducedReturnedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is introduced returned";
				}
				return "Receipt code is introduced returned";
			}
			return "Ծածկագիրը «Շրջանառության մեջ դրված, վերադարձված» է";
		}
	}

	public static string RCT_009_NotLocalCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is not local";
				}
				return "Receipt code is already returned";
			}
			return "Այլ երկրի ծածկագիր";
		}
	}

	public static string RCT_010_CorruptedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is corrupted";
				}
				return "Receipt code is corrupted";
			}
			return "Սխալ/վնասված ծածկագիր";
		}
	}

	public static string RCT_011_EmittedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is emitted";
				}
				return "Receipt code is emitted";
			}
			return "Ծածկագիրը «Թողարկված» է";
		}
	}

	public static string RCT_012_AppliedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code status is applied";
				}
				return "Receipt code status is applied";
			}
			return "Ծածկագիրը «Մակնշված» է";
		}
	}

	public static string RCT_013_WrittenOffCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code status is written off";
				}
				return "Receipt code status is written off";
			}
			return "Ծածկագիրը «Խոտանված» է";
		}
	}

	public static string UnknownError
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Unknown Error";
				}
				return "Unknown Error";
			}
			return "Անհայտ Սխալ";
		}
	}

	public static string RCT_008_AlreadyReturned
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code is already returned";
				}
				return "Receipt code is already returned";
			}
			return "Ծածկագիրն արդեն վերադարձված է";
		}
	}

	public static string RCT_014_DisaggregatedCode
	{
		get
		{
			if (!LanguageService.IsArmenian)
			{
				if (!LanguageService.IsRussian)
				{
					return "Receipt code status is\r\ndisaggregated\r\n";
				}
				return "Receipt code status is\r\ndisaggregated\r\n";
			}
			return "Ծածկագիրը «Ապաագրեգացված» է";
		}
	}
}
