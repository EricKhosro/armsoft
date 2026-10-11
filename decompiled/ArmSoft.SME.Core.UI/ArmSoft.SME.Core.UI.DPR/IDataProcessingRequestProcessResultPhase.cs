using System;
using System.Windows.Threading;

namespace ArmSoft.SME.Core.UI.DPR;

public interface IDataProcessingRequestProcessResultPhase<R>
{
	int Total { get; }

	int Row { get; }

	bool DisableCancel { get; }

	string Caption { get; }

	void Process(R result, Func<bool> updatePhaseAndGetCancellationStatus, Dispatcher progressWindowDispatcher);
}
