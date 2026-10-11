using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArmSoft.OpenBanking.Api.Common;

public static class BankCode
{
	public const string Ameria = "157";

	public const string Ararat = "151";

	public const string Amio = "115";

	public const string ArmSwiss = "250";

	public const string Byblos = "214";

	public const string Converse = "193";

	public const string Evoca = "166";

	public const string Ardshin = "247";

	public const string Econom = "163";

	public const string UniversalBank = "666";

	public const string HatisBank = "777";

	public const string OrinakeliBank = "776";

	public const string KarbiBank = "888";

	public const string DemoBank = "999";

	public static IEnumerable<string> GetAll()
	{
		return from x in typeof(BankCode).GetFields(BindingFlags.Static | BindingFlags.Public)
			select (string)x.GetValue(null);
	}
}
