using System;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI;

public class PrintInfo
{
	private string _repKind;

	private string _orientation;

	private float _marginLeft;

	private float _marginRight;

	private float _marginTop;

	private float _marginBottom;

	private bool _printDate;

	private bool _printPageNumbers;

	private bool _fitToPage;

	private bool _printFilterCondition;

	private string _printHeader;

	private bool _enablePageBreaks;

	private bool _printASInfo;

	private bool _printCompanyName;

	private bool _showLines;

	private int _scalePercent;

	public string PaperOrientation
	{
		get
		{
			return _orientation;
		}
		set
		{
			_orientation = value;
		}
	}

	public float MarginLeft
	{
		get
		{
			return _marginLeft;
		}
		set
		{
			_marginLeft = value;
		}
	}

	public float MarginRight
	{
		get
		{
			return _marginRight;
		}
		set
		{
			_marginRight = value;
		}
	}

	public float MarginTop
	{
		get
		{
			return _marginTop;
		}
		set
		{
			_marginTop = value;
		}
	}

	public float MarginBottom
	{
		get
		{
			return _marginBottom;
		}
		set
		{
			_marginBottom = value;
		}
	}

	public bool PrintDate
	{
		get
		{
			return _printDate;
		}
		set
		{
			_printDate = value;
		}
	}

	public bool PrintPageNumbers
	{
		get
		{
			return _printPageNumbers;
		}
		set
		{
			_printPageNumbers = value;
		}
	}

	public bool FitToPage
	{
		get
		{
			return _fitToPage;
		}
		set
		{
			_fitToPage = value;
		}
	}

	public bool PrintFilterCondition
	{
		get
		{
			return _printFilterCondition;
		}
		set
		{
			_printFilterCondition = value;
		}
	}

	public string PrintHeader
	{
		get
		{
			return _printHeader;
		}
		set
		{
			_printHeader = value;
		}
	}

	public bool EnablePageBreaks
	{
		get
		{
			return _enablePageBreaks;
		}
		set
		{
			_enablePageBreaks = value;
		}
	}

	public bool PrintASInfo
	{
		get
		{
			return _printASInfo;
		}
		set
		{
			_printASInfo = value;
		}
	}

	public bool PrintCompanyName
	{
		get
		{
			return _printCompanyName;
		}
		set
		{
			_printCompanyName = value;
		}
	}

	public bool ShowLines
	{
		get
		{
			return _showLines;
		}
		set
		{
			_showLines = value;
		}
	}

	public int ScalePercent
	{
		get
		{
			return _scalePercent;
		}
		set
		{
			_scalePercent = value;
		}
	}

	public PrintInfo(string reportName = "")
	{
		_repKind = reportName;
		GetConfigurationSettings(_repKind);
	}

	private void GetConfigurationSettings(string reportKind)
	{
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"Print Settings"}\\{(string.IsNullOrWhiteSpace(reportKind) ? "Default" : reportKind)}", writable: false);
		if (registryKey == null)
		{
			PaperOrientation = "Portrait";
			MarginLeft = Convert.ToSingle(0.5);
			MarginRight = Convert.ToSingle(0.5);
			MarginTop = Convert.ToSingle(0.5);
			MarginBottom = Convert.ToSingle(0.5);
			PrintHeader = "DoNotPrint";
			PrintDate = false;
			PrintPageNumbers = false;
			PrintFilterCondition = false;
			FitToPage = true;
			EnablePageBreaks = false;
			ScalePercent = 100;
			if (string.IsNullOrWhiteSpace(reportKind))
			{
				PrintASInfo = true;
			}
		}
		else
		{
			_repKind = reportKind;
			PaperOrientation = Convert.ToString(registryKey.GetValue("PaperOrientation"));
			MarginLeft = Convert.ToSingle(registryKey.GetValue("PaperMarginsLeft"));
			MarginRight = Convert.ToSingle(registryKey.GetValue("PaperMarginsRight"));
			MarginTop = Convert.ToSingle(registryKey.GetValue("PaperMarginsTop"));
			MarginBottom = Convert.ToSingle(registryKey.GetValue("PaperMarginsBottom"));
			PrintHeader = Convert.ToString(registryKey.GetValue("PrintHeader"));
			PrintDate = Convert.ToBoolean(registryKey.GetValue("PrintDate"));
			PrintPageNumbers = Convert.ToBoolean(registryKey.GetValue("PrintPageNumbers"));
			PrintFilterCondition = Convert.ToBoolean(registryKey.GetValue("PrintFilterCondition"));
			FitToPage = Convert.ToBoolean(registryKey.GetValue("PrintFitToPage"));
			EnablePageBreaks = Convert.ToBoolean(registryKey.GetValue("PrintEnablePageBreaks"));
			ScalePercent = Convert.ToInt32(registryKey.GetValue("ScalePercent"));
			if (string.IsNullOrWhiteSpace(reportKind))
			{
				if (!MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey.GetValueNames(), "PrintSystemName"))
				{
					PrintASInfo = true;
				}
				else
				{
					PrintASInfo = Convert.ToBoolean(registryKey.GetValue("PrintSystemName"));
				}
				if (!MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey.GetValueNames(), "ShowGridLines"))
				{
					ShowLines = false;
				}
				else
				{
					ShowLines = Convert.ToBoolean(registryKey.GetValue("ShowGridLines"));
				}
			}
		}
		RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey($"{Settings.RegistryKey}\\{"Print Settings"}\\{"Default"}", writable: false);
		if (registryKey == null || !MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey.GetValueNames(), "PrintCompanyName"))
		{
			if (string.IsNullOrWhiteSpace(reportKind))
			{
				if (registryKey2 != null && MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey2.GetValueNames(), "PrintCompanyName"))
				{
					PrintCompanyName = Convert.ToBoolean(registryKey2.GetValue("PrintCompanyName"));
				}
				else
				{
					PrintCompanyName = false;
				}
			}
			else
			{
				PrintCompanyName = false;
			}
		}
		else if (registryKey != null)
		{
			PrintCompanyName = Convert.ToBoolean(registryKey.GetValue("PrintCompanyName"));
		}
		else
		{
			PrintCompanyName = false;
		}
		if (registryKey == null || !MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey.GetValueNames(), "ShowGridLines"))
		{
			if (string.IsNullOrWhiteSpace(reportKind))
			{
				if (registryKey2 != null && MemoryExtensions.Contains<string>((ReadOnlySpan<string>)registryKey2.GetValueNames(), "ShowGridLines"))
				{
					ShowLines = Convert.ToBoolean(registryKey2.GetValue("ShowGridLines"));
				}
				else
				{
					ShowLines = false;
				}
			}
			else
			{
				ShowLines = false;
			}
		}
		else if (registryKey != null)
		{
			ShowLines = Convert.ToBoolean(registryKey.GetValue("ShowGridLines"));
		}
		else
		{
			ShowLines = false;
		}
	}

	public void StoreSettings()
	{
		using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey($"{Settings.RegistryKey}\\{"Print Settings"}\\{(string.IsNullOrWhiteSpace(_repKind) ? "Default" : _repKind)}");
		registryKey.SetValue("PaperOrientation", PaperOrientation);
		registryKey.SetValue("PaperMarginsLeft", MarginLeft);
		registryKey.SetValue("PaperMarginsRight", MarginRight);
		registryKey.SetValue("PaperMarginsTop", MarginTop);
		registryKey.SetValue("PaperMarginsBottom", MarginBottom);
		registryKey.SetValue("PrintHeader", PrintHeader);
		registryKey.SetValue("PrintDate", PrintDate);
		registryKey.SetValue("PrintPageNumbers", PrintPageNumbers);
		registryKey.SetValue("PrintFilterCondition", PrintFilterCondition);
		registryKey.SetValue("PrintFitToPage", FitToPage);
		registryKey.SetValue("PrintEnablePageBreaks", EnablePageBreaks);
		registryKey.SetValue("ScalePercent", ScalePercent);
		if (string.IsNullOrWhiteSpace(_repKind))
		{
			registryKey.SetValue("PrintSystemName", PrintASInfo);
		}
		registryKey.SetValue("PrintCompanyName", PrintCompanyName);
		registryKey.SetValue("ShowGridLines", ShowLines);
	}
}
