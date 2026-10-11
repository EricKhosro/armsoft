namespace ArmSoft.SME.Common.Models.UserAccess;

public class POPermission
{
	public bool AllowAdd { get; set; }

	public bool AllowEdit { get; set; }

	public bool AllowView { get; set; }

	public bool AllowDelete { get; set; }

	public POPermission()
	{
		AllowAdd = false;
		AllowEdit = false;
		AllowView = false;
		AllowDelete = false;
	}

	public POPermission(bool allowAdd, bool allowEdit, bool allowView, bool allowDelete)
	{
		AllowAdd = allowAdd;
		AllowEdit = allowEdit;
		AllowView = allowView;
		AllowDelete = allowDelete;
	}
}
