using System.Collections.Generic;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;
using DevExpress.Xpf.Editors;

namespace ArmSoft.SME.Core.UI.DataViews;

internal class PrintPreviewDialog : DialogWindow
{
	private readonly DropDownCommentControl _selectedPrintMode;

	private readonly CheckEditExt _insertPageBreaks;

	private readonly DropDownCommentControl _printTemplate;

	private readonly IReadOnlyList<TemplatesCacheRow> _printTemplates;

	private const string _modeConst = "Mode";

	private const string _templateConst = "Template";

	private const string _breaksConst = "BREAKS";

	public bool PrintSelectedRows => _selectedPrintMode.Text == ArmSoft.SME.Common.Resources.SelectedRowsMode;

	public bool InsertPageBreaks => _insertPageBreaks.IsChecked.Value;

	public TemplatesCacheRow PrintTemplate
	{
		get
		{
			if (_printTemplate.KeyValue == null)
			{
				return _printTemplates[0];
			}
			return _printTemplates.FirstOrDefault((TemplatesCacheRow p) => p.Name == _printTemplate.KeyValue.ToString());
		}
	}

	public PrintPreviewDialog(IReadOnlyList<TemplatesCacheRow> templates, TemplatesCacheRow defaultTemplate, bool isForSelectedRows)
	{
		_printTemplates = templates;
		FontFamily = Settings.FontFamily;
		FontSize = Settings.GetFontSize();
		Caption = ArmSoft.SME.Common.Resources.PrintPreview;
		_selectedPrintMode = AddDropDownCommentControl("Mode", ArmSoft.SME.Common.Resources.Print, new SmartEnum(typeof(PrintPreviewSelectedModeSE)), isRequired: true, storeValue: false, 50);
		_selectedPrintMode.IsTextEditable = false;
		_printTemplate = AddDropDownCommentControl("Template", ArmSoft.SME.Common.Resources.PrintTemplate, new Template(templates), isRequired: true, storeValue: false, 50);
		_printTemplate.IsTextEditable = false;
		_printTemplate.Text = defaultTemplate?.Caption;
		_insertPageBreaks = AddBooleanControl("BREAKS", ArmSoft.SME.Common.Resources.InsertPageBreaks);
		_selectedPrintMode.Text = (isForSelectedRows ? ArmSoft.SME.Common.Resources.SelectedRowsMode : ArmSoft.SME.Common.Resources.CurrentRowMode);
		if (!isForSelectedRows)
		{
			_selectedPrintMode.IsReadOnly = true;
			SetFocusOnActivation = _printTemplate;
		}
		else
		{
			_insertPageBreaks.IsChecked = true;
		}
		_selectedPrintMode.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Mode_EditValueChanged).Handler;
		Mode_EditValueChanged();
	}

	private void Mode_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		Mode_EditValueChanged();
	}

	private void Mode_EditValueChanged()
	{
		if (_insertPageBreaks != null)
		{
			if (_selectedPrintMode.Text == ArmSoft.SME.Common.Resources.CurrentRowMode)
			{
				_insertPageBreaks.IsReadOnly = true;
				_insertPageBreaks.IsChecked = false;
			}
			else
			{
				_insertPageBreaks.IsReadOnly = false;
			}
		}
	}
}
