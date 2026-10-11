using System;
using System.Text.Json.Serialization;

namespace ArmSoft.SME.Common.SmartEnums;

[SmartEnumProviderSupport(15, "HotKey")]
public class HotKeysSE : ASSmartEnum<HotKeysSE, string>
{
	public static readonly HotKeysSE F1 = new HotKeysSE("F1", "F1", () => "F1", () => "(F1)");

	public static readonly HotKeysSE F2 = new HotKeysSE("F2", "F2", () => "F2", () => "(F2)");

	public static readonly HotKeysSE F3 = new HotKeysSE("F3", "F3", () => "F3", () => "(F3)");

	public static readonly HotKeysSE F4 = new HotKeysSE("F4", "F4", () => "F4", () => "(F4)");

	public static readonly HotKeysSE F6 = new HotKeysSE("F6", "F6", () => "F6", () => "(F6)");

	public static readonly HotKeysSE F7 = new HotKeysSE("F7", "F7", () => "F7", () => "(F7)");

	public static readonly HotKeysSE F8 = new HotKeysSE("F8", "F8", () => "F8", () => "(F8)");

	public static readonly HotKeysSE F9 = new HotKeysSE("F9", "F9", () => "F9", () => "(F9)");

	public static readonly HotKeysSE F11 = new HotKeysSE("F11", "F11", () => "F11", () => "(F11)");

	public static readonly HotKeysSE Ctrl_F12 = new HotKeysSE("Ctrl_F12", "Ctrl+F2", () => "Ctrl+F2", () => "(Ctrl+F2)");

	public static readonly HotKeysSE Ctrl_F3 = new HotKeysSE("Ctrl_F3", "Ctrl+F3", () => "Ctrl+F3", () => "(Ctrl+F3)");

	public static readonly HotKeysSE Ctrl_F5 = new HotKeysSE("Ctrl_F5", "Ctrl+F5", () => "Ctrl+F5", () => "(Ctrl+F5)");

	public static readonly HotKeysSE Ctrl_F9 = new HotKeysSE("Ctrl_F9", "Ctrl+F9", () => "Ctrl+F9", () => "(Ctrl+F9)");

	public static readonly HotKeysSE Ctrl_1 = new HotKeysSE("Ctrl_1", "Ctrl+1", () => "Ctrl+1", () => "(Ctrl+1)");

	public static readonly HotKeysSE Ctrl_2 = new HotKeysSE("Ctrl_2", "Ctrl+2", () => "Ctrl+2", () => "(Ctrl+2)");

	public static readonly HotKeysSE Ctrl_3 = new HotKeysSE("Ctrl_3", "Ctrl+3", () => "Ctrl+3", () => "(Ctrl+3)");

	public static readonly HotKeysSE Ctrl_4 = new HotKeysSE("Ctrl_4", "Ctrl+4", () => "Ctrl+4", () => "(Ctrl+4)");

	public static readonly HotKeysSE Ctrl_5 = new HotKeysSE("Ctrl_5", "Ctrl+5", () => "Ctrl+5", () => "(Ctrl+5)");

	public static readonly HotKeysSE Ctrl_6 = new HotKeysSE("Ctrl_6", "Ctrl+6", () => "Ctrl+6", () => "(Ctrl+6)");

	public static readonly HotKeysSE Ctrl_7 = new HotKeysSE("Ctrl_7", "Ctrl+7", () => "Ctrl+7", () => "(Ctrl+7)");

	public static readonly HotKeysSE Ctrl_8 = new HotKeysSE("Ctrl_8", "Ctrl+8", () => "Ctrl+8", () => "(Ctrl+8)");

	public static readonly HotKeysSE Ctrl_9 = new HotKeysSE("Ctrl_9", "Ctrl+9", () => "Ctrl+9", () => "(Ctrl+9)");

	public static readonly HotKeysSE Ctrl_F11 = new HotKeysSE("Ctrl_F11", "Ctrl+F11", () => "Ctrl+F11", () => "(Ctrl+F11)");

	public static readonly HotKeysSE Ctrl_Enter = new HotKeysSE("Ctrl_Enter", "Ctrl+Enter", () => "Ctrl+Enter", () => "(Ctrl+Enter)");

	public static readonly HotKeysSE Ctrl_D = new HotKeysSE("Ctrl_D", "Ctrl+D", () => "Ctrl+D", () => "(Ctrl+D)");

	public static readonly HotKeysSE Ctrl_E = new HotKeysSE("Ctrl_E", "Ctrl+E", () => "Ctrl+E", () => "(Ctrl+E)");

	public static readonly HotKeysSE Ctrl_I = new HotKeysSE("Ctrl_I", "Ctrl+I", () => "Ctrl+I", () => "(Ctrl+I)");

	public static readonly HotKeysSE Ctrl_N = new HotKeysSE("Ctrl_N", "Ctrl+N", () => "Ctrl+N", () => "(Ctrl+N)");

	public static readonly HotKeysSE Ctrl_Y = new HotKeysSE("Ctrl_Y", "Ctrl+Y", () => "Ctrl+Y", () => "(Ctrl+Y)");

	public static readonly HotKeysSE Ctrl_O = new HotKeysSE("Ctrl_O", "Ctrl+O", () => "Ctrl+O", () => "(Ctrl+O)");

	public static readonly HotKeysSE Ctrl_Alt_I = new HotKeysSE("Ctrl_Alt_I", "Ctrl+Alt+I", () => "Ctrl+Alt+I", () => "(Ctrl+Alt+I)");

	public static readonly HotKeysSE Ctrl_Shift_D = new HotKeysSE("Ctrl_Shift_D", "Ctrl+Shift+D", () => "Ctrl+Shift+D", () => "(Ctrl+Shift+D)");

	public static readonly HotKeysSE Shift_F2 = new HotKeysSE("Shift_F2", "Shift+F2", () => "Shift+F2", () => "(Shift+F2)");

	public static readonly HotKeysSE Shift_F3 = new HotKeysSE("Shift_F3", "Shift+F3", () => "Shift+F3", () => "(Shift+F3)");

	public static readonly HotKeysSE Shift_F5 = new HotKeysSE("Shift_F5", "Shift+F5", () => "Shift+F5", () => "(Shift+F5)");

	public static readonly HotKeysSE Shift_F6 = new HotKeysSE("Shift_F6", "Shift+F6", () => "Shift+F6", () => "(Shift+F6)");

	public static readonly HotKeysSE Shift_F7 = new HotKeysSE("Shift_F7", "Shift+F7", () => "Shift+F7", () => "(Shift+F7)");

	public static readonly HotKeysSE Shift_F8 = new HotKeysSE("Shift_F8", "Shift+F8", () => "Shift+F8", () => "(Shift+F8)");

	public static readonly HotKeysSE Shift_F9 = new HotKeysSE("Shift_F9", "Shift+F9", () => "Shift+F9", () => "(Shift+F9)");

	public static readonly HotKeysSE Shift_F11 = new HotKeysSE("Shift_F11", "Shift+F11", () => "Shift+F11", () => "(Shift+F11)");

	public static readonly HotKeysSE Shift_F12 = new HotKeysSE("Shift_F12", "Shift+F12", () => "Shift+F12", () => "(Shift+F12)");

	[JsonIgnore]
	private readonly Func<string> _captionWithBracketsDelegate;

	public const string F1_Const = "F1";

	public const string F2_Const = "F2";

	public const string F3_Const = "F3";

	public const string F4_Const = "F4";

	public const string F6_Const = "F6";

	public const string F7_Const = "F7";

	public const string F8_Const = "F8";

	public const string F9_Const = "F9";

	public const string F11_Const = "F11";

	public const string Ctrl_F2_Const = "Ctrl+F2";

	public const string Ctrl_F3_Const = "Ctrl+F3";

	public const string Ctrl_F5_Const = "Ctrl+F5";

	public const string Ctrl_F9_Const = "Ctrl+F9";

	public const string Ctrl_1_Const = "Ctrl+1";

	public const string Ctrl_2_Const = "Ctrl+2";

	public const string Ctrl_3_Const = "Ctrl+3";

	public const string Ctrl_4_Const = "Ctrl+4";

	public const string Ctrl_5_Const = "Ctrl+5";

	public const string Ctrl_6_Const = "Ctrl+6";

	public const string Ctrl_7_Const = "Ctrl+7";

	public const string Ctrl_8_Const = "Ctrl+8";

	public const string Ctrl_9_Const = "Ctrl+9";

	public const string Ctrl_F11_Const = "Ctrl+F11";

	public const string Ctrl_Enter_Const = "Ctrl+Enter";

	public const string Ctrl_D_Const = "Ctrl+D";

	public const string Ctrl_E_Const = "Ctrl+E";

	public const string Ctrl_I_Const = "Ctrl+I";

	public const string Ctrl_N_Const = "Ctrl+N";

	public const string Ctrl_Y_Const = "Ctrl+Y";

	public const string Ctrl_O_Const = "Ctrl+O";

	public const string Ctrl_Alt_I_Const = "Ctrl+Alt+I";

	public const string Ctrl_Shift_D_Const = "Ctrl+Shift+D";

	public const string Shift_F2_Const = "Shift+F2";

	public const string Shift_F3_Const = "Shift+F3";

	public const string Shift_F5_Const = "Shift+F5";

	public const string Shift_F6_Const = "Shift+F6";

	public const string Shift_F7_Const = "Shift+F7";

	public const string Shift_F8_Const = "Shift+F8";

	public const string Shift_F9_Const = "Shift+F9";

	public const string Shift_F11_Const = "Shift+F11";

	public const string Shift_F12_Const = "Shift+F12";

	public string CaptionWithBrackets => _captionWithBracketsDelegate();

	public HotKeysSE(string name, string value, Func<string> caption, Func<string> captionWithBracketsDelegate)
		: base(name, value, caption)
	{
		_captionWithBracketsDelegate = captionWithBracketsDelegate;
	}
}
