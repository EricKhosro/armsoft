using System;

namespace ArmSoft.PrintTemplates;

internal class AtomicSubstitution : IComparable<AtomicSubstitution>
{
	public string Code;

	public object Value;

	public AtomicSubstitution(string code, object value)
	{
		Code = code;
		Value = value;
	}

	public int CompareTo(AtomicSubstitution other)
	{
		if (other == null || other.Code == null)
		{
			return -1;
		}
		int num = -Code.Length.CompareTo(other.Code.Length);
		if (num != 0)
		{
			return num;
		}
		return Code.CompareTo(other.Code);
	}
}
