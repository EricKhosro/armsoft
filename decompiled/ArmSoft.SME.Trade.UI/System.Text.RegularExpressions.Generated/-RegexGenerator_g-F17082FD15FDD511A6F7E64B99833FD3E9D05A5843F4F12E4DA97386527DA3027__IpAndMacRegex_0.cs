using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.27113")]
internal sealed class _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__IpAndMacRegex_0 : Regex
{
	private sealed class RunnerFactory : RegexRunnerFactory
	{
		private sealed class Runner : RegexRunner
		{
			protected override void Scan(ReadOnlySpan<char> inputSpan)
			{
				while (TryFindNextPossibleStartingPosition(inputSpan) && !TryMatchAtCurrentPosition(inputSpan) && runtextpos != inputSpan.Length)
				{
					runtextpos++;
					if (_003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if (num <= inputSpan.Length - 15)
				{
					if (num <= 0 || inputSpan[num - 1] == '\n')
					{
						goto IL_005a;
					}
					int num2 = MemoryExtensions.IndexOf<char>(inputSpan.Slice(num), '\n');
					if ((uint)num2 <= inputSpan.Length - num - 1)
					{
						num += num2 + 1;
						if (num <= inputSpan.Length - 15)
						{
							goto IL_005a;
						}
					}
				}
				goto IL_0077;
				IL_005a:
				int num3 = inputSpan.Slice(num).IndexOfNonAsciiOrAny_46BDC41D56D0BA532B34C35CECFC93F6BB44E3A2B465E50D2E80E540998E67D8();
				if (num3 >= 0)
				{
					runtextpos = num + num3;
					return true;
				}
				goto IL_0077;
				IL_0077:
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				int num2 = 0;
				int num3 = 0;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (num > 0 && inputSpan[num - 1] != '\n')
				{
					UncaptureUntil(0);
					return false;
				}
				int i;
				for (i = 0; (uint)i < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[i]); i++)
				{
				}
				readOnlySpan = readOnlySpan.Slice(i);
				num += i;
				num2 = num;
				int j;
				for (j = 0; j < 3 && (uint)j < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[j]); j++)
				{
				}
				if (j == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(j);
				num += j;
				for (int k = 0; k < 3; k++)
				{
					if (readOnlySpan.IsEmpty || readOnlySpan[0] != '.')
					{
						UncaptureUntil(0);
						return false;
					}
					num++;
					readOnlySpan = inputSpan.Slice(num);
					int l;
					for (l = 0; l < 3 && (uint)l < (uint)readOnlySpan.Length && char.IsDigit(readOnlySpan[l]); l++)
					{
					}
					if (l == 0)
					{
						UncaptureUntil(0);
						return false;
					}
					readOnlySpan = readOnlySpan.Slice(l);
					num += l;
				}
				Capture(1, num2, num);
				int m;
				for (m = 0; (uint)m < (uint)readOnlySpan.Length && char.IsWhiteSpace(readOnlySpan[m]); m++)
				{
				}
				if (m == 0)
				{
					UncaptureUntil(0);
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(m);
				num += m;
				num3 = num;
				int num4 = MemoryExtensions.IndexOfAnyExcept<char>(readOnlySpan, _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_ascii_20FF077E0000007E000000);
				if (num4 < 0)
				{
					num4 = readOnlySpan.Length;
				}
				if (num4 < 7)
				{
					UncaptureUntil(0);
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(num4);
				num += num4;
				Capture(2, num3, num);
				runtextpos = num;
				Capture(0, start, num);
				return true;
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				void UncaptureUntil(int capturePosition)
				{
					while (Crawlpos() > capturePosition)
					{
						Uncapture();
					}
				}
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__IpAndMacRegex_0 Instance = new _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__IpAndMacRegex_0();

	private _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__IpAndMacRegex_0()
	{
		pattern = "(?m)^\\s*(\\d{1,3}(?:\\.\\d{1,3}){3})\\s+([0-9A-Fa-f:-]{7,})";
		roptions = RegexOptions.None;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 3;
	}
}
