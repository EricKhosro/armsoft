using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Layout;

namespace ArmSoft.SME.Core.UI.Services;

public interface ILayoutService
{
	bool Store(LayoutRow layout);

	List<LayoutRow> GetList(string name);

	bool Delete(LayoutRow layout);

	bool EditCaption(LayoutRow layout, string newCaption);

	string GetBody(LayoutRow layout);

	bool IsDeleteLayoutRow(string name, string caption);

	bool ExistsLayoutRow(string name, string caption);
}
