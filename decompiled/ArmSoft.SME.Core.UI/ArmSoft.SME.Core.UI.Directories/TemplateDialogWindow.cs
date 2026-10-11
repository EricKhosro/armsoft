using System;
using System.Windows;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DropDowns;

namespace ArmSoft.SME.Core.UI.Directories;

public class TemplateDialogWindow : DialogWindow
{
	private TextEditExt _name;

	private TextEditExt _caption;

	private DropDownControl _docType;

	private CheckEditExt _isEnabled;

	private readonly PrintTemplateTypeSE _templateType;

	public TemplateDialogWindow(DialogWindowMode mode, PrintTemplateTypeSE templateType, Window owner = null)
	{
		Mode = mode;
		Owner = owner;
		_templateType = templateType;
		ConstructUI();
		LoadDataToControls();
		SetControlsReadOnly();
	}

	private void ConstructUI()
	{
		Caption = ArmSoft.SME.Common.Resources.PrintTemplates;
		_name = AddStringControl("Name", ArmSoft.SME.Common.Resources.Code, 20, isRequired: true);
		_caption = AddStringControl("Caption", ArmSoft.SME.Common.Resources.Name, 50, isRequired: true);
		Type type = ((_templateType == PrintTemplateTypeSE.OnlineReport) ? typeof(OnlineReportTemplateTypeSE) : typeof(PrintTemplateDocumentTypeSE));
		_docType = AddDropDownControl("DocType", ArmSoft.SME.Common.Resources.DocumentType, new SmartEnum(type, null, padLeft: false, '0', allSubTypes: true));
		_isEnabled = AddBooleanControl("Enabled", ArmSoft.SME.Common.Resources.TemplateEnabled);
		if (Mode == DialogWindowMode.EditMode)
		{
			SetFocusOnActivation = _caption;
		}
	}

	private void LoadDataToControls()
	{
		if (Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.CreateCopyMode)
		{
			TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			if (dataRow != null)
			{
				if (Mode == DialogWindowMode.EditMode)
				{
					_name.EditValue = dataRow.Name;
				}
				_caption.EditValue = dataRow.Caption;
				if (dataRow.DocType != 0)
				{
					_docType.Code = dataRow.DocType.ToString().PadLeft(2, '0');
				}
				_isEnabled.IsChecked = dataRow.Enabled;
			}
		}
		else if (Mode == DialogWindowMode.NewMode)
		{
			_isEnabled.IsChecked = true;
		}
	}

	private void SetControlsReadOnly()
	{
		bool flag = Mode == DialogWindowMode.ViewMode;
		if (Mode == DialogWindowMode.EditMode)
		{
			TemplatesDataProvider.DataRow dataRow = (TemplatesDataProvider.DataRow)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
			flag = flag || dataRow.IsSystem;
		}
		_name.IsReadOnly = Mode == DialogWindowMode.EditMode || Mode == DialogWindowMode.ViewMode;
		_caption.IsReadOnly = flag;
		_docType.IsReadOnly = flag;
		_isEnabled.IsReadOnly = Mode == DialogWindowMode.ViewMode;
	}
}
