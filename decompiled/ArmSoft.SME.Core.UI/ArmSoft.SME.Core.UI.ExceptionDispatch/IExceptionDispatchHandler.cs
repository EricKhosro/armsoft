using System.Runtime.ExceptionServices;

namespace ArmSoft.SME.Core.UI.ExceptionDispatch;

public interface IExceptionDispatchHandler
{
	ExceptionDispatchInfo ExceptionDispatchInfo { get; }
}
