using System.Runtime.InteropServices;

namespace ArmSoft.SME.Common.Constants;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct TempFileLifeInM
{
	public const short Default = 2;

	public const short Caches = 120;
}
