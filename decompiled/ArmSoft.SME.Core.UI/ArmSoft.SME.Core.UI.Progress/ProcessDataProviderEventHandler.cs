using ArmSoft.SME.Client.Data;

namespace ArmSoft.SME.Core.UI.Progress;

public delegate void ProcessDataProviderEventHandler<R>(object sender, ProcessDataProviderEventArgs<R> e) where R : ExtendableRow, new();
