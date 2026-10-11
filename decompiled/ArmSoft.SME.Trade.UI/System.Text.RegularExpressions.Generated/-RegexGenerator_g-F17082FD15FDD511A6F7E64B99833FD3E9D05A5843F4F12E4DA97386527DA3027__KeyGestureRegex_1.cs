using System.CodeDom.Compiler;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.27113")]
internal sealed class _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__KeyGestureRegex_1 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				if (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan))
				{
					runtextpos = inputSpan.Length;
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if (num <= inputSpan.Length - 2 && num == 0)
				{
					return true;
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (num != 0)
				{
					return false;
				}
				if ((uint)readOnlySpan.Length < 2u || readOnlySpan[0] != 'D' || !char.IsAsciiDigit(readOnlySpan[1]))
				{
					return false;
				}
				if (3 < readOnlySpan.Length || (2 < readOnlySpan.Length && readOnlySpan[2] != '\n'))
				{
					return false;
				}
				Capture(0, start, runtextpos = num + 2);
				return true;
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__KeyGestureRegex_1 Instance = new _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__KeyGestureRegex_1();

	private _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__KeyGestureRegex_1()
	{
		pattern = "^D[0-9]$";
		roptions = RegexOptions.None;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 1;
	}
}
