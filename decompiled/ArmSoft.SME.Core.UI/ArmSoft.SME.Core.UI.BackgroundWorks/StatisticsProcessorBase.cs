using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common.Models.Statistics;
using ArmSoft.SME.Core.UI.Helpers;

namespace ArmSoft.SME.Core.UI.BackgroundWorks;

public class StatisticsProcessorBase
{
	public static void SendData()
	{
		List<CallCountStatisticStoreRequest> calls = CallsRecordingHelper.Calls;
		if (calls != null && calls.Count > 0)
		{
			List<CallCountStatisticStoreRequest> request = (from c in CallsRecordingHelper.Calls
				group c by new { c.Type, c.Key }).Select(g =>
			{
				CallCountStatisticStoreRequest callCountStatisticStoreRequest = g.First();
				callCountStatisticStoreRequest.Count = g.Sum((CallCountStatisticStoreRequest x) => x.Count);
				return callCountStatisticStoreRequest;
			}).ToList();
			Settings.CreateApiClient().Statistics.StoreCallCount(request);
		}
		List<CallDurationStatisticStoreRequest> callsDurations = CallsRecordingHelper.CallsDurations;
		if (callsDurations != null && callsDurations.Count > 0)
		{
			Settings.CreateApiClient().Statistics.StoreCallDuration(CallsRecordingHelper.CallsDurations);
		}
		CallsRecordingHelper.Clear(onlyRecords: true);
	}
}
