using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ArmSoft.SME.Core.UI.VersionManagement;

namespace ArmSoft.SME.Trade.UI.VersionManagement;

public class UIManager : UIManagerBase
{
	public override Version ServiceMinVersion => new Version(9, 2, 46);

	public override Version LastReleasedVersion => new Version(9, 2, 37);

	public override int ChangeNumber => 0;

	public override List<Version> VersionNumbers
	{
		get
		{
			int num = 56;
			List<Version> list = new List<Version>(num);
			CollectionsMarshal.SetCount(list, num);
			Span<Version> span = CollectionsMarshal.AsSpan(list);
			span[0] = new Version(9, 0, 0);
			span[1] = new Version(9, 0, 1);
			span[2] = new Version(9, 0, 2);
			span[3] = new Version(9, 0, 3);
			span[4] = new Version(9, 0, 4);
			span[5] = new Version(9, 0, 5);
			span[6] = new Version(9, 0, 6);
			span[7] = new Version(9, 0, 7);
			span[8] = new Version(9, 0, 8);
			span[9] = new Version(9, 0, 9);
			span[10] = new Version(9, 0, 10);
			span[11] = new Version(9, 0, 11);
			span[12] = new Version(9, 1, 0);
			span[13] = new Version(9, 1, 1);
			span[14] = new Version(9, 1, 2);
			span[15] = new Version(9, 1, 3);
			span[16] = new Version(9, 1, 4);
			span[17] = new Version(9, 1, 5);
			span[18] = new Version(9, 2, 0);
			span[19] = new Version(9, 2, 1);
			span[20] = new Version(9, 2, 2);
			span[21] = new Version(9, 2, 3);
			span[22] = new Version(9, 2, 4);
			span[23] = new Version(9, 2, 5);
			span[24] = new Version(9, 2, 6);
			span[25] = new Version(9, 2, 7);
			span[26] = new Version(9, 2, 8);
			span[27] = new Version(9, 2, 9);
			span[28] = new Version(9, 2, 10);
			span[29] = new Version(9, 2, 11);
			span[30] = new Version(9, 2, 12);
			span[31] = new Version(9, 2, 13);
			span[32] = new Version(9, 2, 14);
			span[33] = new Version(9, 2, 15);
			span[34] = new Version(9, 2, 16);
			span[35] = new Version(9, 2, 17);
			span[36] = new Version(9, 2, 18);
			span[37] = new Version(9, 2, 19);
			span[38] = new Version(9, 2, 20);
			span[39] = new Version(9, 2, 21);
			span[40] = new Version(9, 2, 22);
			span[41] = new Version(9, 2, 23);
			span[42] = new Version(9, 2, 24);
			span[43] = new Version(9, 2, 25);
			span[44] = new Version(9, 2, 26);
			span[45] = new Version(9, 2, 27);
			span[46] = new Version(9, 2, 28);
			span[47] = new Version(9, 2, 29);
			span[48] = new Version(9, 2, 30);
			span[49] = new Version(9, 2, 31);
			span[50] = new Version(9, 2, 32);
			span[51] = new Version(9, 2, 33);
			span[52] = new Version(9, 2, 34);
			span[53] = new Version(9, 2, 35);
			span[54] = new Version(9, 2, 36);
			span[55] = new Version(9, 2, 37);
			return list;
		}
	}

	public override List<Type> GetChangeTypes()
	{
		return (from p in Assembly.GetExecutingAssembly().GetTypes()
			where p.IsSubclassOf(typeof(UpgradeTo))
			select p).ToList();
	}

	public UIManager(IServiceProvider serviceProvider)
		: base(serviceProvider)
	{
	}
}
