using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArmSoft.ElScaleConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Trade.Client.DataProviders.Directories;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.DataViewDataProvider;

namespace ArmSoft.SME.Trade.UI.Devices.ElScale;

public class CasDirectImporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsShowDataProvider.DataRow> ResultRows;

	public CasDirectImporterProgress(ElScalesAndTerminalsDataProvider.DataRow row)
	{
		_row = row;
		_progress = new MultilineProgressWindow
		{
			Title = ArmSoft.SME.Trade.Common.Resources.ViewScaleItemList
		};
		_phases.Add(ArmSoft.SME.Common.Resources.Connecting);
		_phases.Add(ArmSoft.SME.Common.Resources.RowsLoaded);
		_progress.PreparePrimaryPhases(_phases);
		_progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		_progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		DirectImportForCAS();
	}

	public void DirectImportForCAS()
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		Cas cas = new Cas(_row.ExportFile);
		if (!cas.CheckConnection())
		{
			throw new ASException(ArmSoft.SME.Trade.Common.Resources.ConnectingToElScaleError);
		}
		scalePhase.EndDate = DateTime.Now;
		_progress.ProcessPhase(scalePhase);
		ScalePhase phase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.RowsLoaded,
			DisableCancel = true
		};
		_progress.ProcessPhase(phase);
		ResultRows = new List<ElScaleGoodsShowDataProvider.DataRow>();
		List<Cas.CasItem> list = cas.Download();
		List<ElScaleGoodsShowDataProvider.DataRow> list2 = new List<ElScaleGoodsShowDataProvider.DataRow>();
		foreach (Cas.CasItem item in list)
		{
			list2.Add(new ElScaleGoodsShowDataProvider.DataRow
			{
				CodeInScale = item.ItemCode.ToString(),
				Name = item.Name1,
				PLU = (int)item.PLU,
				Price = ((decimal)item.UnitPrice / 100m).RoundAs(2),
				IsWeight = (item.PLUType == 1),
				BarCodePrefix = item.Group,
				LabelN = item.Label,
				BarCodeN = item.Barcode,
				Ingredients = item.DirectMessage
			});
		}
		_progress.ProcessPhase(phase);
		ResultRows = list2;
	}
}
