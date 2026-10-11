using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;

namespace ArmSoft.SME.Trade.AddIn;

[ComVisible(true)]
public class AddInRibbon : IRibbonExtensibility
{
	private IRibbonUI _ribbon;

	public string GetCustomUI(string ribbonID)
	{
		if (TradeSettings.Connector.Lang() == "A")
		{
			return GetResourceText("ArmSoft.SME.Trade.AddIn.AddinRibbonHY.xml");
		}
		return GetResourceText("ArmSoft.SME.Trade.AddIn.AddinRibbon.xml");
	}

	public void Ribbon_Load(IRibbonUI ribbonUI)
	{
		_ribbon = ribbonUI;
	}

	public void Connect_Click(IRibbonControl ribbonControl)
	{
		TradeSettings.Connector.Connect();
	}

	public void CalculateReport_Click(IRibbonControl ribbonControl)
	{
		if (TradeSettings.ExcelApplication.Workbooks.Count <= 0 || TradeSettings.Connector.ShowCalculateParams(TradeSettings.ExcelApplication.ActiveWorkbook.Worksheets.Count))
		{
			TradeSettings.ExcelApplication.StatusBar = Settings.Database + " " + Settings.UserName;
			DoCalculate();
		}
	}

	public void Close_Click(IRibbonControl ribbonControl)
	{
		TradeSettings.Connector.CloseConnection();
	}

	public void EncodeFormulas_Click(IRibbonControl ribbonControl)
	{
		if (TradeSettings.Connector.IsConnected())
		{
			DoEnCodeDecode(parModeEncode: true);
		}
	}

	public void DecodeFormulas_Click(IRibbonControl ribbonControl)
	{
		if (TradeSettings.Connector.IsConnected())
		{
			DoEnCodeDecode(parModeEncode: false);
		}
	}

	public void DoEnCodeDecode(bool parModeEncode)
	{
		ObservableCollection<string> observableCollection = new ObservableCollection<string>();
		string caption = (parModeEncode ? ArmSoft.SME.Core.AddIn.Resources.EncodeTitle : ArmSoft.SME.Core.AddIn.Resources.DecodeTitle);
		Workbook activeWorkbook = TradeSettings.ExcelApplication.ActiveWorkbook;
		if (TradeSettings.ExcelApplication.Workbooks.Count == 0 || activeWorkbook == null || ((dynamic)activeWorkbook.ActiveSheet == null) || TradeSettings.ExcelApplication.ActiveWorkbook.IsAddin)
		{
			return;
		}
		if (parModeEncode)
		{
			if (MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.EncodeFormulas, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
			{
				return;
			}
		}
		else if (MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.DecodeFormulas, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
		{
			return;
		}
		TradeSettings.Connector.CalcMode = 0;
		TradeSettings.ExcelApplication.DisplayAlerts = true;
		for (int i = 1; i <= activeWorkbook.Worksheets.Count; i++)
		{
			((dynamic)activeWorkbook.Worksheets[i]).Activate();
			dynamic val = ((dynamic)activeWorkbook.Worksheets[i]).UsedRange;
			dynamic val2 = ((!parModeEncode) ? val.Find("\a=", LookIn: -4163) : val.Find("=", LookIn: -4123));
			if ((object)val2 == null)
			{
				continue;
			}
			object obj = val2.Address;
			bool flag = true;
			while (flag)
			{
				try
				{
					if (!((dynamic)activeWorkbook.Worksheets[i]).ProtectContents || !val2.Locked)
					{
						if (parModeEncode)
						{
							val2.Formula = val2.Formula.ToString().Replace("=", "\a=");
						}
						else
						{
							TradeSettings.Connector.CalcMode = 0;
							val2.Formula = val2.Formula.ToString().Replace("\a=", "=");
						}
					}
				}
				catch (Exception)
				{
					string item = "Sh " + i + " " + val2.address.ToString().Replace("$", "");
					if (observableCollection.Contains(item))
					{
						flag = false;
					}
					else
					{
						observableCollection.Add(item);
					}
				}
				if (flag)
				{
					val2 = val.FindNext((object)val2);
					if ((object)val2 == null)
					{
						flag = false;
					}
					else if (val2.address.ToString() == obj.ToString())
					{
						flag = false;
					}
				}
			}
		}
		if (observableCollection.Count > 0)
		{
			MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.DecodingError, caption);
		}
		else if (parModeEncode)
		{
			MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.EncodingOK, caption);
		}
		else
		{
			MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.DecodingOK, caption);
		}
	}

	public void PasteValues_Click(IRibbonControl ribbonControl)
	{
		if (!TradeSettings.Connector.IsConnected())
		{
			return;
		}
		Workbook activeWorkbook = TradeSettings.ExcelApplication.ActiveWorkbook;
		if (TradeSettings.ExcelApplication.Workbooks.Count == 0 || activeWorkbook == null || TradeSettings.ExcelApplication.ActiveWorkbook.IsAddin || MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.PasteValues, "Paste values", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No || ((dynamic)activeWorkbook.LinkSources(XlLinkType.xlLinkTypeExcelLinks) == null) || !(activeWorkbook.LinkSources(XlLinkType.xlLinkTypeExcelLinks) is Array array))
		{
			return;
		}
		for (int i = 1; i <= array.Length; i++)
		{
			string text = array.GetValue(i)?.ToString();
			if (!string.IsNullOrEmpty(text) && text.IndexOf("ASTRADECLOUDADDIN.XLA", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				activeWorkbook.BreakLink(text, XlLinkType.xlLinkTypeExcelLinks);
				break;
			}
		}
	}

	private void DoCalculate()
	{
		try
		{
			if (!TradeSettings.IsConnected())
			{
				MessageBox.Show(ArmSoft.SME.Core.AddIn.Resources.DatabaseNotConnected);
				return;
			}
			if (TradeSettings.Connector.FormulaEngine != null)
			{
				TradeSettings.Connector.FormulaEngine.Clear();
			}
			TradeSettings.ExcelApplication.ScreenUpdating = true;
			TradeSettings.ExcelApplication.Calculation = XlCalculation.xlCalculationManual;
			TradeSettings.Connector.CalcMode = 1;
			if (!TradeSettings.CalcAll)
			{
				SafeCalcRange(((dynamic)TradeSettings.ExcelApplication.ActiveWorkbook.ActiveSheet).UsedRange);
			}
			else
			{
				for (int i = 1; i <= TradeSettings.ExcelApplication.ActiveWorkbook.Worksheets.Count; i++)
				{
					((dynamic)TradeSettings.ExcelApplication.ActiveWorkbook.Worksheets[i]).Activate();
					SafeCalcRange(((dynamic)TradeSettings.ExcelApplication.ActiveWorkbook.Worksheets[i]).UsedRange);
				}
			}
			TradeSettings.Connector.CalcMode = 0;
			TradeSettings.ExcelApplication.Calculation = XlCalculation.xlCalculationAutomatic;
			TradeSettings.Connector.EndSession();
		}
		catch (Exception ex)
		{
			if (((dynamic)ex).Number == 18)
			{
				MessageBox.Show("Calculation interrupted!");
			}
			else
			{
				MessageBox.Show("Error calculating report!");
			}
			TradeSettings.CalcMode = 0;
			TradeSettings.Connector.EndSession();
		}
	}

	private void SafeCalcRange(Range rng)
	{
		foreach (dynamic cell in rng.Cells)
		{
			if (cell.HasArray)
			{
				rng = TradeSettings.ExcelApplication.Union(rng, cell.CurrentArray);
			}
		}
		rng.Calculate();
	}

	private static string GetResourceText(string resourceName)
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		string[] manifestResourceNames = executingAssembly.GetManifestResourceNames();
		for (int i = 0; i <= manifestResourceNames.Length - 1; i++)
		{
			if (string.Compare(resourceName, manifestResourceNames[i], StringComparison.OrdinalIgnoreCase) != 0)
			{
				continue;
			}
			using StreamReader streamReader = new StreamReader(executingAssembly.GetManifestResourceStream(manifestResourceNames[i]));
			if (streamReader != null)
			{
				return streamReader.ReadToEnd();
			}
		}
		return null;
	}
}
