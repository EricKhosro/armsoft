using System.CodeDom.Compiler;

namespace System.Text.RegularExpressions.Generated;

[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.7603")]
internal sealed class _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__TableRegex_3 : Regex
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
					if (_003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.s_hasTimeout)
					{
						CheckTimeout();
					}
				}
			}

			private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				if (num <= inputSpan.Length - 4)
				{
					ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
					int num2;
					for (num2 = 0; num2 < readOnlySpan.Length - 3; num2++)
					{
						int num3 = MemoryExtensions.IndexOf<char>(readOnlySpan.Slice(num2), '%');
						if (num3 < 0)
						{
							break;
						}
						num2 += num3;
						if ((uint)(num2 + 1) >= (uint)readOnlySpan.Length)
						{
							break;
						}
						if (_003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.IsWordChar(readOnlySpan[num2 + 1]))
						{
							runtextpos = num + num2;
							return true;
						}
					}
				}
				runtextpos = inputSpan.Length;
				return false;
			}

			private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
			{
				int num = runtextpos;
				int start = num;
				ReadOnlySpan<char> readOnlySpan = inputSpan.Slice(num);
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '%')
				{
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				int i;
				for (i = 0; (uint)i < (uint)readOnlySpan.Length && _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.IsWordChar(readOnlySpan[i]); i++)
				{
				}
				if (i == 0)
				{
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(i);
				num += i;
				if (readOnlySpan.IsEmpty || readOnlySpan[0] != '.')
				{
					return false;
				}
				num++;
				readOnlySpan = inputSpan.Slice(num);
				int j;
				for (j = 0; (uint)j < (uint)readOnlySpan.Length && _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.IsWordChar(readOnlySpan[j]); j++)
				{
				}
				if (j == 0)
				{
					return false;
				}
				readOnlySpan = readOnlySpan.Slice(j);
				num += j;
				if (!_003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.IsPostWordCharBoundary(inputSpan, num))
				{
					return false;
				}
				runtextpos = num;
				Capture(0, start, num);
				return true;
			}
		}

		protected override RegexRunner CreateInstance()
		{
			return new Runner();
		}
	}

	internal static readonly _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__TableRegex_3 Instance = new _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__TableRegex_3();

	private _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__TableRegex_3()
	{
		pattern = "%\\w+\\.\\w+\\b";
		roptions = RegexOptions.Compiled | RegexOptions.Singleline;
		Regex.ValidateMatchTimeout(_003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.s_defaultTimeout);
		internalMatchTimeout = _003CRegexGenerator_g_003EF1D9F198FAF5D9703602819DAD1F763933257B4EA9BB10A6228DBDE2DC60C992D__Utilities.s_defaultTimeout;
		factory = new RunnerFactory();
		capsize = 1;
	}
}
