using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.Progress;

public class PrepareProcessingDataPhaseArgs<R> where R : ExtendableRow, new()
{
	public DataProviderResult<R> Result { get; internal set; }
}
