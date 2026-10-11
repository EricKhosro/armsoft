using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;

namespace ArmSoft.SME.Trade.AddIn;

[StartupObject(0)]
[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
public sealed class ThisAddIn : AddInBase
{
	private Dictionary<string, bool> CheckedFiles;

	internal CustomTaskPaneCollection CustomTaskPanes;

	internal SmartTagCollection VstoSmartTags;

	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	private object missing = Type.Missing;

	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	internal Microsoft.Office.Interop.Excel.Application Application;

	protected override object RequestComAddInAutomationService()
	{
		if (TradeSettings.Connector == null)
		{
			TradeSettings.Connector = new TradeExcelConnector();
		}
		return TradeSettings.Connector;
	}

	protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
	{
		return new AddInRibbon();
	}

	private void ThisAddIn_Startup(object sender, EventArgs e)
	{
		int num = 0;
		Settings.SystemType = Settings.SystemTypes.Trade;
		TradeSettings.ExcelApplication = Application;
		AppDomain.CurrentDomain.AssemblyResolve += BinAssemblyResolver;
		if (Convert.ToDecimal(TradeSettings.ExcelApplication.Version) > 12m)
		{
			AddIns2 addIns = TradeSettings.ExcelApplication.AddIns2;
			bool flag = false;
			foreach (Microsoft.Office.Interop.Excel.AddIn item in addIns)
			{
				if (item.Name.ToUpper().EndsWith("AsTradeCloudAddIn.xla".ToUpper()))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (TradeSettings.ExcelApplication.Workbooks.Count == 0)
				{
					TradeSettings.ExcelApplication.Workbooks.Add(Type.Missing);
					num = TradeSettings.ExcelApplication.Workbooks.Count;
				}
				TradeSettings.AddInPath = AppDomain.CurrentDomain.BaseDirectory + "AsTradeCloudAddIn.XLA";
				addIns.Add(TradeSettings.AddInPath, Type.Missing);
				TradeSettings.ExcelApplication.AddIns["AsTradeCloudAddIn"].Installed = true;
				if (num > 0)
				{
					TradeSettings.ExcelApplication.Workbooks[num].Close(Type.Missing, Type.Missing, Type.Missing);
				}
			}
			return;
		}
		try
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\AddIns";
			string sourceFileName = AppDomain.CurrentDomain.BaseDirectory + "AsTradeCloudAddIn.XLA";
			string text2 = (TradeSettings.AddInPath = text + "\\AsTradeCloudAddIn.xla");
			bool flag2 = !File.Exists(text2);
			File.Copy(sourceFileName, text2, overwrite: true);
			if (flag2)
			{
				TradeSettings.ExcelApplication.AddIns["AsTradeCloudAddIn"].Installed = true;
			}
		}
		catch (Exception)
		{
		}
	}

	public Assembly BinAssemblyResolver(object sender, ResolveEventArgs args)
	{
		if (CheckedFiles == null)
		{
			CheckedFiles = new Dictionary<string, bool>();
		}
		string text = args.Name.Split(',')[0];
		if (CheckedFiles.ContainsKey(text) && !CheckedFiles[text])
		{
			return null;
		}
		string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin");
		string path2 = text + ".dll";
		string text2 = Path.Combine(path, path2);
		if (File.Exists(text2))
		{
			Assembly result = Assembly.LoadFrom(text2);
			CheckedFiles.Add(text, value: true);
			return result;
		}
		CheckedFiles.Add(text, value: false);
		return null;
	}

	private void ThisAddIn_Shutdown(object sender, EventArgs e)
	{
		Settings.IsLoggedIn = false;
	}

	private void InternalStartup()
	{
		Startup += ThisAddIn_Startup;
		Shutdown += ThisAddIn_Shutdown;
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public ThisAddIn(ApplicationFactory factory, IServiceProvider serviceProvider)
		: base((Microsoft.Office.Tools.Factory)factory, serviceProvider, "AddIn", "ThisAddIn")
	{
		Globals.Factory = factory;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void Initialize()
	{
		base.Initialize();
		Application = GetHostItem<Microsoft.Office.Interop.Excel.Application>(typeof(Microsoft.Office.Interop.Excel.Application), "Application");
		Globals.ThisAddIn = this;
		System.Windows.Forms.Application.EnableVisualStyles();
		InitializeCachedData();
		InitializeControls();
		InitializeComponents();
		InitializeData();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void FinishInitialization()
	{
		InternalStartup();
		OnStartup();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void InitializeDataBindings()
	{
		BeginInitialization();
		BindToData();
		EndInitialization();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void InitializeCachedData()
	{
		if (base.DataHost != null && base.DataHost.IsCacheInitialized)
		{
			base.DataHost.FillCachedData(this);
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void InitializeData()
	{
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void BindToData()
	{
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	private void StartCaching(string MemberName)
	{
		base.DataHost.StartCaching(this, MemberName);
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	private void StopCaching(string MemberName)
	{
		base.DataHost.StopCaching(this, MemberName);
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	private bool IsCached(string MemberName)
	{
		return base.DataHost.IsCached(this, MemberName);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void BeginInitialization()
	{
		BeginInit();
		CustomTaskPanes.BeginInit();
		VstoSmartTags.BeginInit();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void EndInitialization()
	{
		VstoSmartTags.EndInit();
		CustomTaskPanes.EndInit();
		EndInit();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void InitializeControls()
	{
		CustomTaskPanes = Globals.Factory.CreateCustomTaskPaneCollection(null, null, "CustomTaskPanes", "CustomTaskPanes", this);
		VstoSmartTags = Globals.Factory.CreateSmartTagCollection(null, null, "VstoSmartTags", "VstoSmartTags", this);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private void InitializeComponents()
	{
	}

	[DebuggerNonUserCode]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	private bool NeedsFill(string MemberName)
	{
		return base.DataHost.NeedsFill(this, MemberName);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "17.0.0.0")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	protected override void OnShutdown()
	{
		VstoSmartTags.Dispose();
		CustomTaskPanes.Dispose();
		base.OnShutdown();
	}
}
