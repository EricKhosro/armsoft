using System.Data.SQLite;
using System.Text;

namespace ArmSoft.SME.Core.UI.Helpers;

public static class SQLiteHelper
{
	private const int _bufferLength = 8000;

	public static string ColumnToString(SQLiteDataReader reader, int columnOrdinal)
	{
		char[] array = new char[8000];
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		for (int num2 = (int)reader.GetChars(columnOrdinal, num, array, 0, 8000); num2 > 0; num2 = (int)reader.GetChars(columnOrdinal, num, array, 0, 8000))
		{
			stringBuilder.Append(array, 0, num2);
			num += num2;
		}
		return stringBuilder.ToString();
	}
}
