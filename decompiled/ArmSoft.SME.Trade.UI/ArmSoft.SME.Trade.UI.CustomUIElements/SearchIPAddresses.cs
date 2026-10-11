using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Client;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Trade.UI.Devices.ElScale;

namespace ArmSoft.SME.Trade.UI.CustomUIElements;

public class SearchIPAddresses
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly int _port;

	public List<IPAndMacAddress> ResultIPList { get; set; }

	public bool Canceled { get; set; }

	public SearchIPAddresses(int port, Window owner)
	{
		_port = port;
		_progress = new MultilineProgressWindow
		{
			Title = Resources.FindingDevices,
			Owner = owner
		};
		_phases.Add(Resources.FindingDevices);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		ConcurrentBag<IPAndMacAddress> res = new ConcurrentBag<IPAndMacAddress>();
		List<string> iplist = Digi.NetworkUtils.GetAllPossibleIPs();
		ScalePhase scaleItemsUploadPhase = new ScalePhase
		{
			Name = Resources.FindingDevices,
			DisableCancel = false,
			Row = 0,
			Total = iplist.Count
		};
		_progress.ProcessPhase(scaleItemsUploadPhase);
		int row = 0;
		int maxDOP = Environment.ProcessorCount * 5;
		AsyncHelper.RunSync(async () =>
		{
			await Parallel.ForEachAsync(iplist, new ParallelOptions
			{
				MaxDegreeOfParallelism = maxDOP
			}, async (string ip, CancellationToken cancellationToken) =>
			{
				if (_progress.Canceled)
				{
					Canceled = true;
				}
				else
				{
					int port = _port;
					if (_port == -1)
					{
						port = int.Parse(ip.Split('.').LastOrDefault()) + 2000;
					}
					if (await TryConnectAsync(ip, port))
					{
						res.Add(new IPAndMacAddress(ip, port.ToString(), ""));
					}
					int value2 = Interlocked.Increment(ref row);
					scaleItemsUploadPhase.Row = value2;
					_progress.ProcessPhase(scaleItemsUploadPhase);
				}
			});
		});
		ReadOnlyDictionary<string, string> arpTable = GetArpTable();
		foreach (IPAndMacAddress item in res)
		{
			if (arpTable.TryGetValue(item.IP, out var value))
			{
				item.MacAddress = value;
			}
		}
		ResultIPList = res.ToList();
		scaleItemsUploadPhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scaleItemsUploadPhase);
	}

	public static async Task<bool> TryConnectAsync(string ip, int port)
	{
		TimeSpan delay = TimeSpan.FromMilliseconds(500L);
		using TcpClient client = new TcpClient();
		try
		{
			using CancellationTokenSource cts = new CancellationTokenSource(delay);
			await client.ConnectAsync(ip, port, cts.Token);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static string GetMacAddress(string ipAddress)
	{
		try
		{
			using Process process = Process.Start(new ProcessStartInfo
			{
				FileName = "arp",
				Arguments = "-a",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});
			if (process == null)
			{
				return null;
			}
			string input = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			string pattern = "\\b" + Regex.Escape(ipAddress) + "\\s+([0-9A-Fa-f-:]+)";
			Match match = Regex.Match(input, pattern);
			return match.Success ? match.Groups[1].Value : null;
		}
		catch
		{
			return string.Empty;
		}
	}

	private static ReadOnlyDictionary<string, string> GetArpTable()
	{
		try
		{
			using Process process = Process.Start(new ProcessStartInfo
			{
				FileName = "arp",
				Arguments = "-a",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});
			if (process == null)
			{
				return ReadOnlyDictionary<string, string>.Empty;
			}
			string input = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (Match item in IpAndMacRegex().Matches(input))
			{
				if (item.Groups.Count >= 3)
				{
					string value = item.Groups[1].Value;
					string value2 = item.Groups[2].Value;
					if (!dictionary.ContainsKey(value))
					{
						dictionary[value] = value2;
					}
				}
			}
			return dictionary.AsReadOnly();
		}
		catch
		{
			return ReadOnlyDictionary<string, string>.Empty;
		}
	}

	[GeneratedRegex("(?m)^\\s*(\\d{1,3}(?:\\.\\d{1,3}){3})\\s+([0-9A-Fa-f:-]{7,})")]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "10.0.14.27113")]
	private static Regex IpAndMacRegex()
	{
		return _003CRegexGenerator_g_003EF17082FD15FDD511A6F7E64B99833FD3E9D05A5843F4F12E4DA97386527DA3027__IpAndMacRegex_0.Instance;
	}
}
