using System;
using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Client.DPR.EventArgs;

public class DataProcessingRequestProgressEventArgs
{
	public Action Cancel { get; internal set; }

	public DPRProgress Progress { get; set; }
}
