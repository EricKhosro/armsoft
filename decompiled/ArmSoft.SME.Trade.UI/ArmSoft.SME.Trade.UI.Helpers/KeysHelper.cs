using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using System.Windows.Input;

namespace ArmSoft.SME.Trade.UI.Helpers;

public static class KeysHelper
{
	public static string ToKeyGesture(Key key)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		string text = "";
		ModifierKeys modifiers = Keyboard.Modifiers;
		if ((int)modifiers != 2)
		{
			if ((int)modifiers == 4)
			{
				text = "Shift+";
			}
		}
		else
		{
			text = "Ctrl+";
		}
		string text2 = ((object)key/*cast due to constrained. prefix*/).ToString();
		if (KeyGestureRegex().IsMatch(text2))
		{
			return text + text2.Substring(1);
		}
		return text + text2;
	}

	[GeneratedRegex("^D[0-9]$")]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.27113")]
	private static Regex KeyGestureRegex()
	{
		return _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__KeyGestureRegex_1.Instance;
	}
}
