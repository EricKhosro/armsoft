using System;
using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Client.Data;

public class DataProviderProgressEventArgs
{
	public Action<bool> Cancel { get; internal set; }

	public DPProgress Progress { get; set; }
}
