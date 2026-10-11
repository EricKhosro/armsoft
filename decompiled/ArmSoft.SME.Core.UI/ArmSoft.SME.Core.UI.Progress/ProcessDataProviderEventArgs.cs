using System;
using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.Progress;

public class ProcessDataProviderEventArgs<R> where R : ExtendableRow, new()
{
	public Action Cancel { get; internal set; }

	public Action<int> ProcessPhaseRow { get; internal set; }

	public DataProviderResult<R> Result { get; internal set; }
}
