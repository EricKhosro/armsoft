using System.Text;

namespace ArmSoft.ECRConnector.Helpers;

public static class StringHelper
{
	internal static string ToArmenianUnicode(string source)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		for (int num = source.Length - 1; i <= num; i++)
		{
			int num2 = source[i];
			if (num2 >= 128)
			{
				if ((num2 >= 178) & (num2 <= 253))
				{
					int num3 = num2 - 178;
					num2 = ((num2 % 2 != 0) ? (1377 + (num3 - 1) / 2) : (1329 + num3 / 2));
				}
				else
				{
					switch (num2)
					{
					case 168:
						num2 = 1415;
						break;
					case 167:
						num2 = 1369;
						break;
					case 166:
						num2 = 1370;
						break;
					case 177:
						num2 = 1374;
						break;
					case 176:
						num2 = 1372;
						break;
					case 170:
						num2 = 1373;
						break;
					case 175:
						num2 = 1371;
						break;
					}
				}
			}
			stringBuilder.Append((num2 < 128) ? ((object)source[i]) : char.ConvertFromUtf32(num2));
		}
		return stringBuilder.ToString();
	}
}
