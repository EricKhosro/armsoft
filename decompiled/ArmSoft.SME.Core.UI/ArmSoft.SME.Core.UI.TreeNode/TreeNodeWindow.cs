using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Common.Models.TreeNodes;
using ArmSoft.SME.Common.PersistableObject;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Common.TreeNodes.Definition;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Enums;
using ArmSoft.SME.Core.UI.Extensions;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.MetaData;
using ArmSoft.SME.Core.UI.MetaData.Tree;
using ArmSoft.SME.Core.UI.PersistableObject;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Editors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ArmSoft.SME.Core.UI.TreeNode;

public class TreeNodeWindow : POWindow<ArmSoft.SME.Common.Models.TreeNodes.TreeNode>
{
	private ButtonEditExt _code;

	private TextEditExt _caption;

	private DropDownControl _parent;

	private CheckEditExt _isClosed;

	private TextEditExt _spec;

	private readonly string _treeId;

	private readonly TreeCacheRow _treeCacheRow;

	protected PODefinition _definition;

	protected override double WidthValue => 600.0;

	protected override double HeightValue => 240.0;

	protected override double MinWidthValue => 200.0;

	protected override double MinHeightValue => 120.0;

	public override bool StoredInDb => PO.StoredInDb;

	public override byte State => DocumentStateSE.Processed;

	public override PODefinition Definition => _definition;

	public TreeNodeWindow(string treeId)
	{
		_treeId = treeId;
		_treeCacheRow = Settings.CoreCacheService.Tree.GetItem(treeId);
		PO = Settings.TreeService.Create(_treeCacheRow.Id);
		_definition = DefinitionCreation.CreateTreeNode(_treeCacheRow);
	}

	public TreeNodeWindow(ArmSoft.SME.Common.Models.TreeNodes.TreeNode treeNode)
	{
		_treeId = treeNode.TreeId;
		_treeCacheRow = Settings.CoreCacheService.Tree.GetItem(_treeId);
		PO = treeNode;
		_definition = DefinitionCreation.CreateTreeNode(_treeCacheRow);
	}

	protected override void ConstructUI()
	{
		base.ConstructUI();
		_code = AddButtonEditControl(null, _definition.Fields["Code"], 1);
		_code.AllowMagicButton = _treeCacheRow.NextCodeParamId.HasValue;
		_code.MagicToolTip = (string.IsNullOrWhiteSpace(_treeCacheRow.CodeCaption) ? ArmSoft.SME.Common.Resources.CodeNextNumbers : string.Format(ArmSoft.SME.Common.Resources.FieldNextNumbers, _treeCacheRow.CodeCaption));
		_code.Validate += new SafeEventHandler<ValidationEventArgs>(Code_Validate).Handler;
		_caption = AddStringControl(null, _definition.Fields["Caption"], (_treeCacheRow.CaptionLen > 20) ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left);
		if (_treeCacheRow.Multilevel)
		{
			_parent = AddTreeDropDownControl(null, _definition.Fields["Parent"]);
		}
		if (_treeCacheRow.IsClosedAvailable)
		{
			_isClosed = AddBooleanControl(null, _definition.Fields["IsClosed"]);
		}
		if (!string.IsNullOrWhiteSpace(_treeCacheRow.SpecCaption))
		{
			_spec = AddStringControl(null, _definition.Fields["Spec"], (_treeCacheRow.SpecLen > 20) ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left);
		}
		if (_treeCacheRow.HasExtension)
		{
			ArmSoft.SME.Core.UI.MetaData.Environment environment = new ArmSoft.SME.Core.UI.MetaData.Environment();
			TreeExtensionDefinitionEx treeExtensionDefinitionEx = new TreeExtensionDefinitionEx(_treeCacheRow.Extension, environment);
			ExtensionDefinition = new DocumentExtensionDefinition("", environment)
			{
				DataItems = treeExtensionDefinitionEx.DataItems,
				Elements = treeExtensionDefinitionEx.Elements
			};
		}
		_code.MagicClick += new SafeEventHandler<MagicClickEventArgs>(Code_MagicClick).Handler;
		if (_treeCacheRow.Multilevel)
		{
			_parent.EditValueChanged += new SafeEventHandler<EditValueChangedEventArgs>(Parent_EditValueChanged).Handler;
		}
		_registryKey = "Tree " + _treeCacheRow.Id;
	}

	private void Parent_EditValueChanged(object sender, EditValueChangedEventArgs e)
	{
		_code.DoValidate();
	}

	protected void Code_MagicClick(object sender, MagicClickEventArgs e)
	{
		if (_code.IsEditorActive)
		{
			_code.ClearError();
		}
		SetNextCode();
		_code.DoValidate();
		_code.SelectAll();
	}

	protected virtual void SetNextCode()
	{
		if (_treeCacheRow.NextCodeParamId.HasValue)
		{
			_code.EditValue = Settings.ParametersService.GetGeneralValue(_treeCacheRow.NextCodeParamId.Value);
		}
	}

	private void Code_Validate(object sender, ValidationEventArgs e)
	{
		if (_treeCacheRow.Multilevel && _treeCacheRow.PrefixBased && !string.IsNullOrWhiteSpace(_parent.Code) && e.Value.TryParseToString(out var result) && !string.IsNullOrWhiteSpace(result) && result.StartsWith(_parent.Code.Trim()))
		{
			e.IsValid = false;
			e.ErrorType = ErrorType.Warning;
			e.ErrorContent = string.Format(ArmSoft.SME.Common.Resources.ErrPrefixChildParent, _treeCacheRow.Caption, result, _parent.Code.Trim());
		}
	}

	protected override void SetControlsReadonly()
	{
		base.SetControlsReadonly();
		if (_viewMode != WindowViewMode.View)
		{
			_code.IsReadOnly = _viewMode == WindowViewMode.Edit;
			if (_treeCacheRow.IsClosedAvailable)
			{
				_isClosed.IsReadOnly = !PO.StoredInDb;
			}
		}
	}

	protected virtual bool BeforeStore(POStoreRequest sr)
	{
		return true;
	}

	protected override void GetPO()
	{
		if (_treeCacheRow != null)
		{
			PO = Settings.TreeService.Create(_treeCacheRow.Id);
		}
	}

	protected override void OnNew()
	{
		base.OnNew();
		SetNextCode();
	}

	protected override void OnNewAfterSave()
	{
		PO = Settings.TreeService.Create(_treeCacheRow.Id);
		base.OnNewAfterSave();
		SetNextCode();
		_caption.Text = "";
		if (_treeCacheRow.IsClosedAvailable)
		{
			_isClosed.IsChecked = false;
		}
	}

	public override void Show(WindowViewMode? viewMode = null, bool disableGridValidators = false)
	{
		WindowViewMode valueOrDefault = viewMode.GetValueOrDefault();
		if (!viewMode.HasValue)
		{
			valueOrDefault = ((!StoredInDb) ? WindowViewMode.NewCreate : WindowViewMode.Edit);
			viewMode = valueOrDefault;
		}
		if (viewMode != WindowViewMode.NewCreate)
		{
			viewMode.GetValueOrDefault();
			_ = 2;
		}
		viewMode.GetValueOrDefault();
		_ = 3;
		base.Show(viewMode, disableGridValidators);
		if (Convert.ToBoolean(CallsRecordingHelper.Config?.RecordDirectoryDefinition))
		{
			CallsRecordingHelper.ExportDirectoryWindowDefinition(_treeId, Caption, isDialogWindow: false, GetControlsInfoForRecord(withoutValues: true));
		}
	}

	protected override void ReLoad()
	{
		PO = Settings.TreeService.Load(_treeCacheRow.Id, PO.Code);
		IsChanged = false;
	}

	protected override bool InnerSave(bool isAsDraft = false)
	{
		POStoreRequest sr = new POStoreRequest();
		try
		{
			if (InnerValidate(out var errorMgs, isAsDraft))
			{
				bool isNewDocument = !StoredInDb;
				FillDocument(sr);
				if (BeforeStore(sr))
				{
					PO = Settings.TreeService.Store(PO, sr);
					AfterStore(sr, isNewDocument);
					UpdateDataViewPanel(DVPID);
					return true;
				}
				return false;
			}
			throw new ASException(string.IsNullOrWhiteSpace(errorMgs) ? ArmSoft.SME.Common.Resources.ErrorInSave : errorMgs);
		}
		catch (ASException ex)
		{
			Settings.ErrorHandler(ex);
			if (ex.FieldName != null)
			{
				_setFocusAfterInvalidSave = ControlByName(ex.FieldName);
				ControlsHelper.SetFocus(_setFocusAfterInvalidSave);
			}
			return false;
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
			return false;
		}
	}

	protected override void UpdateDataViewPanel(DataViewPanel dvp)
	{
		if (dvp == null)
		{
			return;
		}
		bool flag = true;
		object obj = dvp.FocusedRow();
		if (obj != null)
		{
			TreeNodesDataProvider.DataRow dataRow = (TreeNodesDataProvider.DataRow)obj;
			if (dataRow.Parent == PO.Parent)
			{
				flag = false;
				dvp.Update(PO.Code);
			}
			if (flag)
			{
				dvp.Refresh();
				dvp.Grid.CurrentItem = dataRow;
			}
		}
		else
		{
			dvp.Update(PO.Code);
		}
	}

	protected override void BarButtonExportXML_ItemClick(object sender, ItemClickEventArgs e)
	{
		ValidateWithNoneAndAlsoFillDocumentForPrint(isForExportToXML: true);
		string fileNameFromExportDialog = Settings.CommonCoreHelper.GetFileNameFromExportDialog(".xml", "XML Files (.xml)|*.xml", "Export_" + DateTime.Now.ToString("MMdd_HHmm"));
		if (!string.IsNullOrWhiteSpace(fileNameFromExportDialog))
		{
			using (FileStream fileStream = new FileStream(fileNameFromExportDialog, FileMode.Create))
			{
				ExchangeRoutes exchange = Settings.CreateApiClient().Exchange;
				string treeId = _treeId;
				int num = 1;
				List<string> list = new List<string>(num);
				CollectionsMarshal.SetCount(list, num);
				CollectionsMarshal.AsSpan(list)[0] = _code.Text;
				fileStream.Write(exchange.ExportTreeNode(treeId, list));
			}
		}
	}
}
