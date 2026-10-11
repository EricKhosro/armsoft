namespace ArmSoft.SME.Core.UI.MetaData;

public class ViewColumn
{
	public ViewColumnAction Action { get; set; }

	public string Name { get; set; }

	public string CaptionId { get; set; }

	public string Caption { get; set; }

	public bool FooterSum { get; set; }

	public bool Visible { get; set; }

	public string After { get; set; }

	public string Before { get; set; }

	public string Format { get; set; }

	public ViewColumn()
	{
		CaptionId = "";
		Caption = "";
		FooterSum = false;
		Visible = true;
		After = "";
		Before = "";
		Format = "";
	}
}
