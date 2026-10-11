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

public class TMADirectImporterProgress
{
	private readonly MultilineProgressWindow _progress;

	private readonly List<string> _phases = new List<string>();

	private readonly ElScalesAndTerminalsDataProvider.DataRow _row;

	public List<ElScaleGoodsShowDataProvider.DataRow> ResultRows;

	public TMADirectImporterProgress(ElScalesAndTerminalsDataProvider.DataRow row)
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
		DirectImportForTMA();
	}

	public void DirectImportForTMA()
	{
		ScalePhase scalePhase = new ScalePhase
		{
			Name = ArmSoft.SME.Common.Resources.Connecting,
			DisableCancel = true
		};
		_progress.ProcessPhase(scalePhase);
		TM_A tM_A = new TM_A(_row.ExportFile);
		if (!tM_A.CheckConnection())
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
		List<TM_A.TM_AItem> list = tM_A.Download();
		List<ElScaleGoodsShowDataProvider.DataRow> list2 = new List<ElScaleGoodsShowDataProvider.DataRow>();
		foreach (TM_A.TM_AItem item in list)
		{
			list2.Add(new ElScaleGoodsShowDataProvider.DataRow
			{
				CodeInScale = item.Code,
				Name = item.Name,
				BarCodePrefix = int.Parse(item.Prefix),
				PLU = int.Parse(item.PLU),
				Price = ((decimal)item.Price / 100m).RoundAs(2),
				IsWeight = (item.Type == 1.ToString()),
				Ingredients = item.Description,
				LabelN = item.LabelN,
				BarCodeN = item.BarCodeN
			});
		}
		_progress.ProcessPhase(phase);
		ResultRows = list2;
	}
}
