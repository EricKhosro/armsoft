using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using ArmSoft.SME.Client.Data.Reports;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models.Caches;
using ArmSoft.SME.Core.UI.Caches;
using ArmSoft.SME.Core.UI.Documents;

namespace ArmSoft.SME.Core.UI.DataViews.Reports;

[DataView(33)]
public class TreeLog : DataView<TreeLogDataProvider.DataRow, TreeLogDataProvider.Param>
{
	private string _caption;

	public override string Caption => _caption;

	[Required]
	public string ParamTreeId
	{
		get
		{
			return (string)Parameters["TreeId"].Value;
		}
		set
		{
			Parameters["TreeId"].Value = value;
		}
	}

	[Required]
	public string ParamCode
	{
		get
		{
			return (string)Parameters["Code"].Value;
		}
		set
		{
			Parameters["Code"].Value = value;
		}
	}

	public TreeLog()
	{
		DataProvider = new TreeLogDataProvider(Settings.CreateApiClient());
		AddParameter("TreeId", Resources.NameOfDirectories, "", null, includeCaptionToolTip: false);
		AddParameter("Code", Resources.Code, "", null, includeCaptionToolTip: false);
		FillColumnsFromDataProvider();
	}

	protected override void SetColumnsProperties()
	{
		base.SetColumnsProperties();
		Columns["UserID"].Visible = false;
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.TreeId = ParamTreeId;
		ParametersProvider.Code = ParamCode;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("Date");
		}
		try
		{
			TreeCacheRow item = Settings.CoreCacheService.Tree.GetItem(ParamTreeId, throwExceptionNotExits: true);
			TreeNodesCache treeNodesCache = (TreeNodesCache)item.TreeNodeCache;
			_caption = string.Format(Resources.DirectoryItemLogCaption, item.NodeCaption + " - " + ParamCode.Trim() + " (" + treeNodesCache.GetItem(ParamCode, throwExceptionNotExits: true).Name + ")");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Resources.ErrorWord, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}
}
