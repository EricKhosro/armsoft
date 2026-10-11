using System.Runtime.InteropServices;
using System.Windows.Input;

namespace ArmSoft.SME.Core.UI.Constants;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct HotKeys
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct WithModifier
	{
		public static KeyGesture Control_A { get; }

		public static KeyGesture Control_B { get; }

		public static KeyGesture Control_C { get; }

		public static KeyGesture Control_D { get; }

		public static KeyGesture Control_F { get; }

		public static KeyGesture Control_I { get; }

		public static KeyGesture Control_J { get; }

		public static KeyGesture Control_K { get; }

		public static KeyGesture Control_L { get; }

		public static KeyGesture Control_N { get; }

		public static KeyGesture Control_R { get; }

		public static KeyGesture Control_T { get; }

		public static KeyGesture Control_V { get; }

		public static KeyGesture Control_W { get; }

		public static KeyGesture Control_Enter { get; }

		public static KeyGesture Control_Up { get; }

		public static KeyGesture Control_Down { get; }

		public static KeyGesture Control_OemPeriod { get; }

		public static KeyGesture Control_OemComma { get; }

		public static KeyGesture Control_OemPlus { get; }

		public static KeyGesture Control_OemMinus { get; }

		public static KeyGesture Control_Add { get; }

		public static KeyGesture Control_Subtract { get; }

		public static KeyGesture Control_F4 { get; }

		public static KeyGesture Control_F3 { get; }

		public static KeyGesture Control_F9 { get; }

		public static KeyGesture Control_F12 { get; }

		public static KeyGesture Control_O { get; }

		public static KeyGesture Shift_F2 { get; }

		public static KeyGesture Shift_F12 { get; }

		public static KeyGesture Shift_Up { get; }

		public static KeyGesture Shift_Down { get; }

		public static KeyGesture Shift_Insert { get; }

		public static KeyGesture Alt_F4 { get; }

		public static KeyGesture Alt_Shift_A { get; }

		public static KeyGesture Alt_Shift_B { get; }

		public static KeyGesture Alt_Shift_C { get; }

		public static KeyGesture Alt_Shift_D { get; }

		public static KeyGesture Alt_Shift_E { get; }

		public static KeyGesture Alt_Shift_F { get; }

		public static KeyGesture Alt_Shift_G { get; }

		public static KeyGesture Alt_Shift_H { get; }

		public static KeyGesture Alt_Shift_I { get; }

		public static KeyGesture Alt_Shift_J { get; }

		public static KeyGesture Alt_Shift_K { get; }

		public static KeyGesture Alt_Shift_L { get; }

		public static KeyGesture Control_Shift_B { get; }

		public static KeyGesture Control_Shift_D { get; }

		public static KeyGesture Control_Shift_L { get; }

		public static KeyGesture Control_Shift_M { get; }

		public static KeyGesture Control_Shift_Q { get; }

		public static KeyGesture Control_Shift_N { get; }

		public static KeyGesture Control_Shift_Y { get; }

		public static KeyGesture Control_Alt_L { get; }

		public static KeyGesture Control_Alt_D { get; }

		static WithModifier()
		{
			Control_A = new KeyGesture((Key)44, (ModifierKeys)2, "Ctrl + A");
			Control_B = new KeyGesture((Key)45, (ModifierKeys)2, "Ctrl + B");
			Control_C = new KeyGesture((Key)46, (ModifierKeys)2, "Ctrl + C");
			Control_D = new KeyGesture((Key)47, (ModifierKeys)2, "Ctrl + D");
			Control_F = new KeyGesture((Key)49, (ModifierKeys)2, "Ctrl + F");
			Control_I = new KeyGesture((Key)52, (ModifierKeys)2, "Ctrl + I");
			Control_J = new KeyGesture((Key)53, (ModifierKeys)2, "Ctrl + J");
			Control_K = new KeyGesture((Key)54, (ModifierKeys)2, "Ctrl + K");
			Control_L = new KeyGesture((Key)55, (ModifierKeys)2, "Ctrl + L");
			Control_N = new KeyGesture((Key)57, (ModifierKeys)2, "Ctrl + N");
			Control_R = new KeyGesture((Key)61, (ModifierKeys)2, "Ctrl + R");
			Control_T = new KeyGesture((Key)63, (ModifierKeys)2, "Ctrl + T");
			Control_V = new KeyGesture((Key)65, (ModifierKeys)2, "Ctrl + V");
			Control_W = new KeyGesture((Key)66, (ModifierKeys)2, "Ctrl + W");
			Control_Enter = new KeyGesture((Key)6, (ModifierKeys)2, "Ctrl + Enter");
			Control_Up = new KeyGesture((Key)24, (ModifierKeys)2, "Ctrl + ↑");
			Control_Down = new KeyGesture((Key)26, (ModifierKeys)2, "Ctrl + ↓");
			Control_OemPeriod = new KeyGesture((Key)144, (ModifierKeys)2, "Ctrl + >");
			Control_OemComma = new KeyGesture((Key)142, (ModifierKeys)2, "Ctrl + <");
			Control_OemPlus = new KeyGesture((Key)141, (ModifierKeys)2);
			Control_OemMinus = new KeyGesture((Key)143, (ModifierKeys)2);
			Control_Add = new KeyGesture((Key)85, (ModifierKeys)2);
			Control_Subtract = new KeyGesture((Key)87, (ModifierKeys)2);
			Control_F4 = new KeyGesture((Key)93, (ModifierKeys)2, "Ctrl + F4");
			Control_F3 = new KeyGesture((Key)92, (ModifierKeys)2, "Ctrl + F3");
			Control_F9 = new KeyGesture((Key)98, (ModifierKeys)2, "Ctrl + F9");
			Control_F12 = new KeyGesture((Key)101, (ModifierKeys)2, "Ctrl + F12");
			Control_O = new KeyGesture((Key)58, (ModifierKeys)2, "Ctrl + O");
			Shift_F2 = new KeyGesture((Key)91, (ModifierKeys)4, "Shift + F2");
			Shift_F12 = new KeyGesture((Key)101, (ModifierKeys)4, "Shift + F12");
			Shift_Up = new KeyGesture((Key)24, (ModifierKeys)4, "Shift + ↑");
			Shift_Down = new KeyGesture((Key)26, (ModifierKeys)4, "Shift + ↓");
			Shift_Insert = new KeyGesture((Key)31, (ModifierKeys)4, "Shift + Insert");
			Alt_F4 = new KeyGesture((Key)93, (ModifierKeys)1, "Alt + F4");
			Alt_Shift_A = new KeyGesture((Key)44, (ModifierKeys)5, "Alt + Shift + A");
			Alt_Shift_B = new KeyGesture((Key)45, (ModifierKeys)5, "Alt + Shift + B");
			Alt_Shift_C = new KeyGesture((Key)46, (ModifierKeys)5, "Alt + Shift + C");
			Alt_Shift_D = new KeyGesture((Key)47, (ModifierKeys)5, "Alt + Shift + D");
			Alt_Shift_E = new KeyGesture((Key)48, (ModifierKeys)5, "Alt + Shift + E");
			Alt_Shift_F = new KeyGesture((Key)49, (ModifierKeys)5, "Alt + Shift + F");
			Alt_Shift_G = new KeyGesture((Key)50, (ModifierKeys)5, "Alt + Shift + G");
			Alt_Shift_H = new KeyGesture((Key)51, (ModifierKeys)5, "Alt + Shift + H");
			Alt_Shift_I = new KeyGesture((Key)52, (ModifierKeys)5, "Alt + Shift + I");
			Alt_Shift_J = new KeyGesture((Key)53, (ModifierKeys)5, "Alt + Shift + J");
			Alt_Shift_K = new KeyGesture((Key)54, (ModifierKeys)5, "Alt + Shift + K");
			Alt_Shift_L = new KeyGesture((Key)55, (ModifierKeys)5, "Alt + Shift + L");
			Control_Shift_B = new KeyGesture((Key)45, (ModifierKeys)6, "Ctrl + Shift + B");
			Control_Shift_D = new KeyGesture((Key)47, (ModifierKeys)6, "Ctrl + Shift + D");
			Control_Shift_L = new KeyGesture((Key)55, (ModifierKeys)6, "Ctrl + Shift + L");
			Control_Shift_M = new KeyGesture((Key)56, (ModifierKeys)6, "Ctrl + Shift + M");
			Control_Shift_Q = new KeyGesture((Key)60, (ModifierKeys)6, "Ctrl + Shift + Q");
			Control_Shift_N = new KeyGesture((Key)57, (ModifierKeys)6, "Ctrl + Shift + N");
			Control_Shift_Y = new KeyGesture((Key)68, (ModifierKeys)6, "Ctrl + Shift + Y");
			Control_Alt_L = new KeyGesture((Key)55, (ModifierKeys)3, "Ctrl + Alt + L");
			Control_Alt_D = new KeyGesture((Key)47, (ModifierKeys)3, "Ctrl + Alt + D");
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct Single
	{
		public static KeyGesture Enter { get; }

		public static KeyGesture Escape { get; }

		public static KeyGesture Insert { get; }

		public static KeyGesture Space { get; }

		public static KeyGesture Up { get; }

		public static KeyGesture Multiply { get; }

		public static KeyGesture OemPlus { get; }

		public static KeyGesture OemMinus { get; }

		public static KeyGesture Add { get; }

		public static KeyGesture Subtract { get; }

		public static KeyGesture Delete { get; }

		static Single()
		{
			Enter = new KeyGesture((Key)6, (ModifierKeys)0);
			Escape = new KeyGesture((Key)13, (ModifierKeys)0);
			Insert = new KeyGesture((Key)31, (ModifierKeys)0);
			Space = new KeyGesture((Key)18, (ModifierKeys)0);
			Up = new KeyGesture((Key)24, (ModifierKeys)0);
			Multiply = new KeyGesture((Key)84, (ModifierKeys)0);
			OemPlus = new KeyGesture((Key)141, (ModifierKeys)0);
			OemMinus = new KeyGesture((Key)143, (ModifierKeys)0);
			Add = new KeyGesture((Key)85, (ModifierKeys)0);
			Subtract = new KeyGesture((Key)87, (ModifierKeys)0);
			Delete = new KeyGesture((Key)32, (ModifierKeys)0);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public readonly struct Functional
	{
		public static KeyGesture F1 { get; }

		public static KeyGesture F2 { get; }

		public static KeyGesture F3 { get; }

		public static KeyGesture F5 { get; }

		public static KeyGesture F12 { get; }

		static Functional()
		{
			F1 = new KeyGesture((Key)90, (ModifierKeys)0);
			F2 = new KeyGesture((Key)91, (ModifierKeys)0, "F2");
			F3 = new KeyGesture((Key)92, (ModifierKeys)0, "F3");
			F5 = new KeyGesture((Key)94, (ModifierKeys)0);
			F12 = new KeyGesture((Key)101, (ModifierKeys)0);
		}
	}
}
