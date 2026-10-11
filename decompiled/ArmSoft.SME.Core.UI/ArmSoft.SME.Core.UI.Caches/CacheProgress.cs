using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Models.Statistics;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.Helpers;
using ArmSoft.SME.Core.UI.Progress;
using ArmSoft.SME.Core.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Caches;

public class CacheProgress
{
	private readonly List<string> _phases = new List<string>();

	private readonly SQLiteStorageService _sqliteStorageService;

	private readonly IImagesService _imagesService;

	private readonly List<short> _inLocalStoredCacheIds;

	private readonly bool _localStorageFileExist;

	private int _indexRefreshCachesInService;

	private int _indexRefreshCachesInUI;

	private int _indexOtherData;

	private int _indexSqliteFile;

	private int _indexImages;

	private MultilineProgressWindow Progress { get; set; }

	public CacheProgress()
	{
		_sqliteStorageService = Settings.ServiceProvider.GetRequiredService<SQLiteStorageService>();
		_sqliteStorageService.StoreInUserBasedLocalStorage = false;
		_imagesService = Settings.ServiceProvider.GetRequiredService<IImagesService>();
		_localStorageFileExist = _sqliteStorageService.ExistDatabase();
		_inLocalStoredCacheIds = (from p in Settings.CacheFunctionsService.CacheDefinitions.Values
			where p.StoreInLocalStorage
			select p.CacheId).ToList();
	}

	public void ShowMultilineProgressWindow()
	{
		_phases.Add(Resources.RowsLoadedInService);
		_indexRefreshCachesInService = 0;
		if (!_localStorageFileExist)
		{
			_phases.Add(Resources.DownloadFile);
			_indexSqliteFile = _phases.Count - 1;
		}
		_phases.Add(Resources.Directories);
		_indexRefreshCachesInUI = _phases.Count - 1;
		_phases.Add(Resources.AdditionalData);
		_indexOtherData = _phases.Count - 1;
		_phases.Add(Resources.Images);
		_indexImages = _phases.Count - 1;
		Progress = new MultilineProgressWindow
		{
			Title = Resources.LoadData
		};
		Progress.AbortButton.Visibility = Visibility.Collapsed;
		Progress.PreparePrimaryPhases(_phases);
		Progress.BackgroundWorkerDoWork += BackgroundWorkerDoWorkEventHandler;
		Progress.ShowDialogWithExceptionHandling();
	}

	private void BackgroundWorkerDoWorkEventHandler(object sender, DoWorkEventArgs e)
	{
		try
		{
			DPProgressPhase phase = new DPProgressPhase
			{
				Name = _phases[_indexRefreshCachesInService],
				DisableCancel = true,
				StartDate = DateTime.Now
			};
			Progress.ProcessPhase(phase);
			Settings.CacheFunctionsService.RefreshServiceCaches();
			if (!_localStorageFileExist)
			{
				DPProgressPhase dPProgressPhase = new DPProgressPhase
				{
					Name = _phases[_indexSqliteFile],
					DisableCancel = true,
					StartDate = DateTime.Now
				};
				Progress.ProcessPhase(dPProgressPhase);
				_sqliteStorageService.SaveMetadataZipFileFromService(_inLocalStoredCacheIds);
				dPProgressPhase.EndDate = DateTime.Now;
				Progress.ProcessPhase(dPProgressPhase);
				CallsRecordingHelper.RecordCallDuration(new CallDurationStatisticStoreRequest(StatisticTypeSE.UICaches, "SQLiteFileDownload", null, dPProgressPhase.Duration.Value));
			}
			DPProgressPhase dPProgressPhase2 = new DPProgressPhase
			{
				Name = _phases[_indexRefreshCachesInUI],
				DisableCancel = true,
				StartDate = DateTime.Now
			};
			Progress.ProcessPhase(dPProgressPhase2);
			if (_inLocalStoredCacheIds.Count > 0)
			{
				foreach (CodeName item in SmartEnumHelper.GetSmartEnumCodeCaption(typeof(CacheIdSE), _inLocalStoredCacheIds.Select((short i) => i.ToString()).ToList()))
				{
					Progress.ChangProcessPhaseName(_phases[_indexRefreshCachesInUI], item.Name);
					_phases[_indexRefreshCachesInUI] = item.Name;
					dPProgressPhase2.Name = item.Name;
					_phases.Add(item.Name);
					ICacheFunctionsService cacheFunctionsService = Settings.CacheFunctionsService;
					int num = 1;
					List<short> list = new List<short>(num);
					CollectionsMarshal.SetCount(list, num);
					CollectionsMarshal.AsSpan(list)[0] = Convert.ToInt16(item.Code);
					cacheFunctionsService.RefreshCache(out var error, list);
					if (!string.IsNullOrWhiteSpace(error))
					{
						throw new ASException(error);
					}
				}
			}
			dPProgressPhase2.EndDate = DateTime.Now;
			Progress.ProcessPhase(dPProgressPhase2);
			CallsRecordingHelper.RecordCallDuration(new CallDurationStatisticStoreRequest(StatisticTypeSE.UICaches, "LoadFromSQLite", null, dPProgressPhase2.Duration.Value));
			DPProgressPhase dPProgressPhase3 = new DPProgressPhase
			{
				Name = _phases[_indexOtherData],
				DisableCancel = true,
				StartDate = DateTime.Now
			};
			Progress.ProcessPhase(dPProgressPhase3);
			Settings.ParametersService.Refresh();
			Settings.UserAccessService.LoadAllPermissionsIfNecessary();
			dPProgressPhase3.EndDate = DateTime.Now;
			Progress.ProcessPhase(dPProgressPhase3);
			DPProgressPhase dPProgressPhase4 = new DPProgressPhase
			{
				Name = _phases[_indexImages],
				DisableCancel = true,
				StartDate = DateTime.Now
			};
			Progress.ProcessPhase(dPProgressPhase4);
			if (Settings.ParametersService.StoreImagesLocal())
			{
				_imagesService.SaveItemsImages();
			}
			dPProgressPhase4.EndDate = DateTime.Now;
			Progress.ProcessPhase(dPProgressPhase4);
			CallsRecordingHelper.RecordCallDuration(new CallDurationStatisticStoreRequest(StatisticTypeSE.Images, "ImagesDownload", null, dPProgressPhase4.Duration.Value));
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			((DispatcherObject)Progress).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				MessageBox.Show(Resources.ErrorInLoadData + "\r\n" + ex3.Message);
				Settings.HostingEnvironment.LogOut(bSaveSettings: false);
			}), Array.Empty<object>());
		}
	}
}
