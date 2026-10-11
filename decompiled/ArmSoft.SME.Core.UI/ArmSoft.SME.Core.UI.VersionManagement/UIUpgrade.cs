using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Exceptions;
using ArmSoft.SME.Common.Models.Job;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.ExceptionDispatch;
using ArmSoft.SME.Core.UI.HelpClasses;
using ArmSoft.SME.Core.UI.Progress;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.VersionManagement;

public class UIUpgrade
{
	private readonly List<string> _phases = new List<string>();

	private readonly IUIManager _uiManager;

	private readonly IParametersService _parametersService;

	private MultilineProgressWindow Progress { get; set; }

	public UIUpgrade()
	{
		_uiManager = Settings.ServiceProvider.GetRequiredService<IUIManager>();
		_parametersService = Settings.ServiceProvider.GetRequiredService<IParametersService>();
	}

	public void ShowMultilineProgressWindow()
	{
		_uiManager.VersionNumbers.IndexOf(_uiManager.VersionInDB);
		_phases.Add(string.Format(Resources.VersionChangeToX, _uiManager.VersionNumbers.Last().ToString()));
		_phases.Add(Resources.UpdateLastStage);
		Progress = new MultilineProgressWindow
		{
			Title = Resources.VersionUpgrade
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
			TryUpgrade();
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception ex3 = ex2;
			((DispatcherObject)Progress).Dispatcher.BeginInvoke((Delegate)(Action)(() =>
			{
				Settings.ErrorHandler(ex3);
			}), Array.Empty<object>());
		}
	}

	public void TryUpgrade()
	{
		if (_uiManager.CheckUpgrade())
		{
			UpgradeVersion();
			DPProgressPhase dPProgressPhase = new DPProgressPhase();
			List<string> phases = _phases;
			dPProgressPhase.Name = phases[phases.Count - 1];
			dPProgressPhase.DisableCancel = true;
			DPProgressPhase dPProgressPhase2 = dPProgressPhase;
			Progress.ProcessPhase(dPProgressPhase2);
			_uiManager.LastChangeFunction();
			Dictionary<string, string> dictionary = RegistryHelper.ReadDictionaryFromRegistry("UIVersions");
			if (dictionary.ContainsKey(Settings.LoginService.Database))
			{
				dictionary[Settings.LoginService.Database] = _uiManager.LastReleasedVersion.ToString();
			}
			else
			{
				dictionary.Add(Settings.LoginService.Database, _uiManager.LastReleasedVersion.ToString());
			}
			RegistryHelper.StoreToRegistry("UIVersions", dictionary);
			Dictionary<string, string> dictionary2 = RegistryHelper.ReadDictionaryFromRegistry("UIVersionChangeNums");
			if (dictionary2.ContainsKey(Settings.LoginService.Database))
			{
				dictionary2[Settings.LoginService.Database] = _uiManager.ChangeNumber.ToString();
			}
			else
			{
				dictionary2.Add(Settings.LoginService.Database, _uiManager.ChangeNumber.ToString());
			}
			RegistryHelper.StoreToRegistry("UIVersionChangeNums", dictionary2);
			_uiManager.VersionInDB = Settings.AppVersion;
			dPProgressPhase2.EndDate = DateTime.Now;
			Progress.ProcessPhase(dPProgressPhase2);
		}
	}

	private void UpgradeVersion()
	{
		try
		{
			DPProgressPhase dPProgressPhase = new DPProgressPhase
			{
				Name = _phases[0],
				DisableCancel = true
			};
			Progress.ProcessPhase(dPProgressPhase);
			dPProgressPhase.Total = _uiManager.VersionNumbers.Count - _uiManager.VersionNumbers.IndexOf(_uiManager.VersionInDB) - 1;
			dPProgressPhase.Row = 0;
			do
			{
				Version nextVersionLogNumber = _uiManager.GetNextVersion(_uiManager.VersionInDB);
				Type type = _uiManager.GetChangeTypes().FirstOrDefault((Type p) => p.Name.EndsWith(nextVersionLogNumber.Major + nextVersionLogNumber.Minor.ToString().PadLeft(2, '0') + nextVersionLogNumber.Build.ToString().PadLeft(2, '0')));
				if (type != null)
				{
					object upgradeObj = Activator.CreateInstance(type);
					if (type.GetCustomAttributes(typeof(CurrentVersionAttribute), inherit: false).Count() == 0)
					{
						ExecuteVersionChanges(nextVersionLogNumber, upgradeObj);
					}
				}
				_uiManager.VersionInDB = nextVersionLogNumber;
				dPProgressPhase.Row++;
			}
			while (_uiManager.VersionInDB != _uiManager.VersionNumbers.Last());
			dPProgressPhase.EndDate = DateTime.Now;
			Progress.ProcessPhase(dPProgressPhase);
		}
		catch (Exception ex)
		{
			if (ex.InnerException?.GetType() == typeof(RESTException))
			{
				throw ex.InnerException;
			}
			throw;
		}
	}

	private void ExecuteVersionChanges(Version version, object upgradeObj)
	{
		int num = 1;
		if (_uiManager.GetPreviousVersion() == _uiManager.LastReleasedVersion)
		{
			num = _uiManager.ChangeNumberInDB + 1;
		}
		foreach (KeyValuePair<short, MethodInfo> orderedChangesMethod in _uiManager.GetOrderedChangesMethods(upgradeObj))
		{
			try
			{
				if (num > 1)
				{
					if (orderedChangesMethod.Value.GetCustomAttribute<ChangeAttribute>().Number >= num)
					{
						_uiManager.ExecuteChange(upgradeObj, orderedChangesMethod.Value);
					}
				}
				else
				{
					_uiManager.ExecuteChange(upgradeObj, orderedChangesMethod.Value);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format(Resources.VersionChangeFuncFailed, version.ToString(), orderedChangesMethod.Key), ex.InnerException);
			}
		}
	}
}
