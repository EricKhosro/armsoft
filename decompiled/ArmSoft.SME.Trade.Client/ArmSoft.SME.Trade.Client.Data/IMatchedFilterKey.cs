namespace ArmSoft.SME.Trade.Client.Data;

public interface IMatchedFilterKey
{
	object FilterKey { get; }

	bool FilterMatched(object key, object path);
}
