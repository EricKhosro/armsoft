using System.Windows.Controls;
using ArmSoft.SME.Common.MetaData;

namespace ArmSoft.SME.Core.UI.MetaData;

public class ExtensionControl
{
	public Control Control { get; set; }

	public DataItem DataItem { get; set; }

	public string ReadOnlySource { get; set; }
}
