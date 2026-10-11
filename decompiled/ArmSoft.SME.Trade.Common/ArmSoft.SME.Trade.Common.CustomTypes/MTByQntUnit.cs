namespace ArmSoft.SME.Trade.Common.CustomTypes;

public class MTByQntUnit
{
	public int MTID { get; set; }

	public string QntUnit { get; set; }

	public MTByQntUnit()
	{
	}

	public MTByQntUnit(int mtId, string qntUnit)
	{
		MTID = mtId;
		QntUnit = qntUnit;
	}
}
