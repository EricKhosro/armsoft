using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common.Models.Job;

namespace ArmSoft.SME.Core.UI.Progress;

public delegate DPProgressPhase PrepareProcessingDataPhaseEventHandler<R>(object sender, PrepareProcessingDataPhaseArgs<R> e) where R : ExtendableRow, new();
