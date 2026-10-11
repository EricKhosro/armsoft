using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;
using ArmSoft.SME.Trade.AddIn.Client.Models;

namespace ArmSoft.SME.Trade.AddIn;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class TradeExcelConnector : ITradeExcelConnector
{
	private string _lastErrorMessage;

	private bool _inSession;

	private const string ExcelAddInTempDirectoryName = "ExcelAddin";

	internal FormulaEngineRoutes FormulaEngine;

	public int CalcMode { get; set; }

	public bool IsConnected()
	{
		return TradeSettings.IsConnected();
	}

	public void CloseConnection()
	{
		Settings.IsLoggedIn = false;
		Settings.ClearLoginInfo();
		FormulaEngine = null;
		if (TradeSettings.ExcelApplication != null)
		{
			TradeSettings.ExcelApplication.StatusBar = "";
		}
	}

	public void Connect()
	{
		CloseConnection();
		Settings.InitData();
		TradeSettings.SetCultures();
		try
		{
			new CloudLoginForm().ShowDialog();
		}
		catch (Exception ex)
		{
			System.Windows.Forms.MessageBox.Show(ex.Message);
		}
		TradeSettings.ExcelApplication.StatusBar = Settings.Database + " " + Settings.UserName;
		TradeSettings.gCurrentDate = DateTime.Now;
		TradeSettings.gDateBegin = new DateTime(TradeSettings.gCurrentDate.Year, TradeSettings.gCurrentDate.Month, 1);
		TradeSettings.gDateEnd = DateTime.Today;
	}

	public bool TryConnect(ref string message)
	{
		if (!IsConnected())
		{
			Connect();
			if (!IsConnected())
			{
				message = ArmSoft.SME.Core.AddIn.Resources.DatabaseNotConnected;
				return false;
			}
		}
		message = null;
		return true;
	}

	private bool InitFormulaEngine()
	{
		if (FormulaEngine == null && Settings.IsLoggedIn)
		{
			FormulaEngine = TradeSettings.CreateApiClient().FormulaEngine;
		}
		return FormulaEngine != null;
	}

	public void CleanDict()
	{
		FormulaEngine.Clear();
	}

	public string LastErrorMessage()
	{
		return _lastErrorMessage;
	}

	public void MessageBox(string Text, int icon = 0, string Caption = "")
	{
		MessageBoxIcon icon2 = icon switch
		{
			16 => MessageBoxIcon.Hand, 
			48 => MessageBoxIcon.Exclamation, 
			64 => MessageBoxIcon.Asterisk, 
			_ => MessageBoxIcon.None, 
		};
		if (string.IsNullOrEmpty(Caption))
		{
			Caption = TradeSettings.ProgramName;
		}
		System.Windows.Forms.MessageBox.Show(Text, Caption, MessageBoxButtons.OK, icon2);
	}

	public bool ShowCalculateParams(int sheetsCount)
	{
		try
		{
			if (!IsConnected())
			{
				Connect();
			}
			if (!IsConnected())
			{
				return false;
			}
			if (sheetsCount > 1)
			{
				CalculationOptionsForm calculationOptionsForm = new CalculationOptionsForm();
				if (calculationOptionsForm.ShowDialog() != DialogResult.OK)
				{
					return false;
				}
				TradeSettings.CalcAll = calculationOptionsForm.CalcAllSheets;
			}
			return new OptionsForm
			{
				CalculationMode = false
			}.ShowDialog() == DialogResult.OK;
		}
		catch (Exception ex)
		{
			System.Windows.Forms.MessageBox.Show(ex.Message);
			return false;
		}
	}

	public void StartSession()
	{
		_inSession = true;
	}

	public void EndSession()
	{
		_inSession = false;
	}

	public bool Session()
	{
		return _inSession;
	}

	public string TGetCompanyName()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyName();
	}

	public string TGetCompanyAddress()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyAddress();
	}

	public string TGetCompanyBussAddress()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyBussAddress();
	}

	public string TGetCompanyStateRegN()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyStateRegN();
	}

	public string TGetCompanyTIN()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyTIN();
	}

	public string TGetCompanyManagerName()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyManagerName();
	}

	public string TGetCompanyManagerPost()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyManagerPost();
	}

	public string TGetCompanyGenAccName()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyGenAccName();
	}

	public string TGetCompanyGenAccPost()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyGenAccPost();
	}

	public string TGetCompanyPhone()
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetCompanyPhone();
	}

	public int Precision()
	{
		return TradeSettings.gPrecision;
	}

	public int Divider()
	{
		return TradeSettings.gDivider;
	}

	public string ToArmenianUnicode(string msg)
	{
		return msg.ToArmenianUnicode();
	}

	public string ToGeorgianUnicode(string msg)
	{
		return "";
	}

	public string Lang()
	{
		if (TradeSettings.gLang.ToString() == "1")
		{
			return "A";
		}
		if (TradeSettings.gLang.ToString() == "2")
		{
			return "E";
		}
		return "G";
	}

	public string UserName()
	{
		if (Settings.IsLoggedIn)
		{
			return Settings.UserName;
		}
		return "";
	}

	public string DatabaseName()
	{
		if (Settings.IsLoggedIn)
		{
			return Settings.Database;
		}
		return "";
	}

	public string TGetPartNameByCode(string PartnerCode, bool IsFullName = false)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetPartNameByCode(PartnerCode, IsFullName);
	}

	public string TGetPartCodeByTaxCode(string TaxCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetPartCodeByTaxCode(TaxCode);
	}

	public string TGetPartCodeByPassport(string Passport)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetPartCodeByPassport(Passport);
	}

	public string TGetMTNameByCode(string MTCode, bool IsFullName = false)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTNameByCode(MTCode, IsFullName);
	}

	public string TGetMTCodeByName(string MTName, bool IsFullName = false)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTCodeByName(MTName, IsFullName);
	}

	public string TGetMTCPACodeByMTCode(string MTCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTCPACodeByMTCode(MTCode);
	}

	public string TGetMTDescriptionByMTCode(string MTCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTDescriptionByMTCode(MTCode);
	}

	public string TGetMTUnitCodeByMTBarCode(string BarCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTUnitCodeByMTBarCode(BarCode);
	}

	public string TGetMTCodeByMTExternalCode(string ExternalCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTCodeByMTExternalCode(ExternalCode);
	}

	public string TGetMTGroupCodeByMTCode(string MTCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return string.Empty;
		}
		return FormulaEngine.TGetMTGroupCodeByMTCode(MTCode);
	}

	public string TGetMTGroupNameByMTCode(string MTCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return string.Empty;
		}
		return FormulaEngine.TGetMTGroupNameByMTCode(MTCode);
	}

	public decimal TGetMTCoeffByMTCodeAndUnit(string MTCode, string Unit = "")
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return 0m;
		}
		return FormulaEngine.TGetMTCoeffByMTCodeAndUnit(MTCode, Unit);
	}

	public string TGetQntUnitCodeByName(string UnitName)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetQntUnitCodeByName(UnitName);
	}

	public string TGetDefContractCodeByPartCode(string PartCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetDefContractCodeByPartCode(PartCode);
	}

	public decimal AsRound(decimal Number, int Precision)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		return Number.RoundAs(Precision);
	}

	public DateTime TStartDate()
	{
		return TradeSettings.gDateBegin;
	}

	public DateTime TEndDate()
	{
		return TradeSettings.gDateEnd;
	}

	public DateTime TCurrentDate()
	{
		return TradeSettings.gCurrentDate;
	}

	public decimal TMTRemSum(Enums.SummaTypes SummaType, string Materials = "", object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTRemSum(SummaType, Materials, value, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupRemSum(Enums.SummaTypes SummaType, string MTGroups, object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupRemSum(SummaType, MTGroups, value, Storages);
		}
		return 0m;
	}

	public decimal TMTRemRetailSum(string Materials = "", object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTRemRetailSum(Materials, value, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupRemRetailSum(string MTGroups, object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupRemRetailSum(MTGroups, value, Storages);
		}
		return 0m;
	}

	public decimal TMTRemCount(string Materials = "", object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTRemCount(Materials, value, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupRemCount(string MTGroups, object RemDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupRemCount(MTGroups, value, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnInSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnInSum(SummaType, Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnInSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnInSum(SummaType, MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnInRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnInRetailSum(Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnInRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnInRetailSum(MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnInCount(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnInCount(Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnInCount(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnInCount(MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnOutSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnOutSum(SummaType, Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnOutSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnOutSum(SummaType, MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnOutRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnOutRetailSum(Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnOutRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnOutRetailSum(MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTTurnOutCount(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTTurnOutCount(Materials, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTGroupTurnOutCount(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupTurnOutCount(MTGroups, value, value2, Storages);
		}
		return 0m;
	}

	public decimal TMTMoveSum(Enums.SummaTypes SummaType, string StorageIn, string StorageOut, string Materials = "", object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTMoveSum(SummaType, StorageIn, StorageOut, Materials, value, value2);
		}
		return 0m;
	}

	public decimal TMTGroupMoveSum(Enums.SummaTypes SummaType, string MTGroup, string StorageIn, string StorageOut, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupMoveSum(SummaType, MTGroup, StorageIn, StorageOut, value, value2);
		}
		return 0m;
	}

	public decimal TMTMoveCount(string StorageIn, string StorageOut, string Materials = "", object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTMoveCount(StorageIn, StorageOut, Materials, value, value2);
		}
		return 0m;
	}

	public decimal TMTGroupMoveCount(string MTGroup, string StorageIn, string StorageOut, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupMoveCount(MTGroup, StorageIn, StorageOut, value, value2);
		}
		return 0m;
	}

	public decimal TMTSaleQty(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleQty(Materials, Convert.ToDateTime(value), Convert.ToDateTime(value2), Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTSaleQtyByDescr(string Description, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleQtyByDescr(Description, Materials, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTGroupSaleQty(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupSaleQty(MTGroups, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTSaleCostSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleCostSum(SummaType, Materials, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTGroupSaleCostSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupSaleCostSum(SummaType, MTGroups, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTRetailSum(string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTRetailSum(Materials, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTGroupRetailSum(string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupRetailSum(MTGroups, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TLastSaleSum(Enums.SummaTypes summaType, string partner = "", string opTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		if (InitFormulaEngine())
		{
			return FormulaEngine.TLastSaleSum(summaType, partner, opTypes);
		}
		return 0m;
	}

	public decimal TMTSaleSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleSum(SummaType, Materials, value, value2, Storages, Partners, OpTypes, Discount);
		}
		return 0m;
	}

	public decimal TMTSaleSumByDescr(Enums.SummaTypes SummaType, string Description, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleSumByDescr(SummaType, Description, Materials, value, value2, Storages, Partners, OpTypes, Discount);
		}
		return 0m;
	}

	public decimal TMTGroupSaleSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "", string Discount = "0")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupSaleSum(SummaType, MTGroups, value, value2, Storages, Partners, OpTypes, Discount);
		}
		return 0m;
	}

	public decimal TPrGroupSaleSum(Enums.SummaTypes SummaType, string PrGroups, object StartDate = null, object EndDate = null, string Storages = "", string Materials = "", string OpTypes = "", string Discount = "0")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TPrGroupSaleSum(SummaType, PrGroups, value, value2, Storages, Materials, OpTypes, Discount);
		}
		return 0m;
	}

	public decimal TMTSaleDiscountSum(Enums.SummaTypes SummaType, string Materials = "", object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTSaleDiscountSum(SummaType, Materials, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTGroupSaleDiscountSum(Enums.SummaTypes SummaType, string MTGroups, object StartDate = null, object EndDate = null, string Storages = "", string Partners = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTGroupSaleSum(SummaType, MTGroups, value, value2, Storages, Partners, OpTypes);
		}
		return 0m;
	}

	public decimal TMTPriceByPriceType(string Material, string PriceType, string Unit = "", object Date = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((Date == null) ? TradeSettings.gCurrentDate : Convert.ToDateTime(Date));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TMTPriceByPriceType(Material, PriceType, Unit, value);
		}
		return 0m;
	}

	public string TGetMTCodeByBarCode(string BarCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTCodeByBarCode(BarCode);
	}

	public string TGetMTExternalCodeByMTCode(string Code)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTExternalCodeByMTCode(Code);
	}

	public string TGetMTUnitByCode(string MTCode, bool IsAltUnit = false)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTUnitByCode(MTCode, IsAltUnit);
	}

	public string TGetMTBarCodeByCodeAndUnit(string MTCode, string Unit = "")
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return "";
		}
		return FormulaEngine.TGetMTBarCodeByCodeAndUnit(MTCode, Unit);
	}

	public decimal TGetMTDiscountByCode(string MTCode)
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return 0m;
		}
		return FormulaEngine.TGetMTDiscountByCode(MTCode);
	}

	public DateTime? TGetMTLastStockTakingDate(string MTCode, string Storage = "")
	{
		if (CalcMode != 0 || !IsConnected() || !InitFormulaEngine())
		{
			return null;
		}
		return FormulaEngine.TGetMTLastStockTakingDate(MTCode, Storage);
	}

	public decimal TRemCash(object RemDate = null, string Cash = "", string Cur = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TRemCash(value, Cash, Cur);
		}
		return 0m;
	}

	public decimal TCurRemCash(string Cur, object RemDate = null, string Cash = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurRemCash(Cur, value, Cash);
		}
		return 0m;
	}

	public decimal TTurnCashIn(object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnCashIn(value, value2, Cash, PRGroup, Partners, Cur, OpTypes);
		}
		return 0m;
	}

	public decimal TTurnCashOut(object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnCashOut(value, value2, Cash, PRGroup, Partners, Cur, OpTypes);
		}
		return 0m;
	}

	public decimal TCurTurnCashIn(string Cur, object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnCashin(Cur, value, value2, Cash, PRGroup, Partners);
		}
		return 0m;
	}

	public decimal TCurTurnCashOut(string Cur, object StartDate = null, object EndDate = null, string Cash = "", string PRGroup = "", string Partners = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnCashOut(Cur, value, value2, Cash, PRGroup, Partners);
		}
		return 0m;
	}

	public decimal TRemBank(object RemDate = null, string Bank = "", string Cur = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TRemBank(value, Bank, Cur);
		}
		return 0m;
	}

	public decimal TCurRemBank(string Cur, object RemDate = null, string Bank = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurRemBank(Cur, value, Bank);
		}
		return 0m;
	}

	public decimal TTurnBankIn(object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnBankIn(value, value2, Bank, PRGroup, Partners, Cur, OpTypes);
		}
		return 0m;
	}

	public decimal TTurnBankOut(object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "", string Cur = "", string OpTypes = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnBankOut(value, value2, Bank, PRGroup, Partners, Cur, OpTypes);
		}
		return 0m;
	}

	public decimal TCurTurnBankIn(string Cur, object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnBankIn(Cur, value, value2, Bank, PRGroup, Partners);
		}
		return 0m;
	}

	public decimal TCurTurnBankOut(string Cur, object StartDate = null, object EndDate = null, string Bank = "", string PRGroup = "", string Partners = "")
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((StartDate == null) ? ((object)TradeSettings.gDateBegin) : StartDate);
		DateTime value2 = Convert.ToDateTime((EndDate == null) ? ((object)TradeSettings.gDateEnd) : EndDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnBankOut(Cur, value, value2, Bank, PRGroup, Partners);
		}
		return 0m;
	}

	public decimal TRemPr(object RemDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TRemPr(value, Partners, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurRemPr(string Cur, object RemDate = null, string Partners = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurRemPr(Cur, value, Partners, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TRemPrByGroup(string PrGroup, object RemDate = null, string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TRemPrByGroup(PrGroup, value, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurRemPrByGroup(string Cur, string PrGroup, object RemDate = null, short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurRemPrByGroup(Cur, PrGroup, value, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TRemPrByContract(string Partner, string Contract, object RemDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TRemPrByContract(Partner, Contract, value);
		}
		return 0m;
	}

	public decimal TCurRemPrByContract(string Partner, string Contract, object RemDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = Convert.ToDateTime((RemDate == null) ? ((object)TradeSettings.gCurrentDate) : RemDate);
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurRemPrByContract(Partner, Contract, value);
		}
		return 0m;
	}

	public decimal TTurnPrIn(object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrIn(value, value2, Partners, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TTurnPrInByGroup(string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrInByGroup(PrGroup, value, value2, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TTurnPrOut(object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOut(value, value2, Partners, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TTurnPrOutByGroup(string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOutByGroup(PrGroup, value, value2, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrIn(string Cur, object StartDate = null, object EndDate = null, string Partners = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrIn(Cur, value, value2, Partners, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrInByGroup(string Cur, string PrGroup, object StartDate = null, object EndDate = null, short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrInByGroup(Cur, PrGroup, value, value2, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrOut(string Cur, object StartDate = null, object EndDate = null, string Partners = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrOut(Cur, value, value2, Partners, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrOutByGroup(string Cur, string PrGroup, object StartDate = null, object EndDate = null, short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrOutByGroup(Cur, PrGroup, value, value2, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TTurnPrInByContract(string Partner, string Contract, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrInByContract(Partner, Contract, value, value2);
		}
		return 0m;
	}

	public decimal TCurTurnPrInByContract(string Partner, string Contract, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrInByContract(Partner, Contract, value, value2);
		}
		return 0m;
	}

	public decimal TTurnPrOutByContract(string Partner, string Contract, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOutByContract(Partner, Contract, value, value2);
		}
		return 0m;
	}

	public decimal TCurTurnPrOutByContract(string Partner, string Contract, object StartDate = null, object EndDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrOutByContract(Partner, Contract, value, value2);
		}
		return 0m;
	}

	public decimal TTurnPrOper(short OpType, object StartDate = null, object EndDate = null, string Partners = "", string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOper(OpType, value, value2, Partners, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TTurnPrOperByGroup(short OpType, string PrGroup, object StartDate = null, object EndDate = null, string Cur = "", short Supplier = -1, short Customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((StartDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(StartDate));
		DateTime value2 = ((EndDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(EndDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOperByGroup(OpType, PrGroup, value, value2, Cur, Supplier, Customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrOper(string cur, short opType, object startDate = null, object endDate = null, string partners = "", short supplier = -1, short customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((startDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(startDate));
		DateTime value2 = ((endDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(endDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TCurTurnPrOper(opType, value, value2, partners, cur, supplier, customer);
		}
		return 0m;
	}

	public decimal TCurTurnPrOperByGroup(string cur, short opType, string prGroup, object startDate = null, object endDate = null, short supplier = -1, short customer = -1)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((startDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(startDate));
		DateTime value2 = ((endDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(endDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOperByGroup(opType, prGroup, value, value2, cur, supplier, customer);
		}
		return 0m;
	}

	public decimal TTurnPrOperByContract(short opType, string partner, string contract, object startDate = null, object endDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((startDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(startDate));
		DateTime value2 = ((endDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(endDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOperByContract(opType, partner, contract, value, value2);
		}
		return 0m;
	}

	public decimal TCurTurnPrOperByContract(short opType, string partner, string contract, object startDate = null, object endDate = null)
	{
		if (CalcMode != 0 || !IsConnected())
		{
			return 0m;
		}
		DateTime value = ((startDate == null) ? TradeSettings.gDateBegin : Convert.ToDateTime(startDate));
		DateTime value2 = ((endDate == null) ? TradeSettings.gDateEnd : Convert.ToDateTime(endDate));
		if (InitFormulaEngine())
		{
			return FormulaEngine.TTurnPrOperByContract(opType, partner, contract, value, value2);
		}
		return 0m;
	}

	public Partner CreatePartner()
	{
		return new Partner();
	}

	public bool ImportPartner(Partner partner, string birthday, string settlementAccount, string defaultContractCurr, string defaultContractPriceType, string defaultContractName, DateTime? defaultContractDate, string defaultContractDescription, string defaultContractDiscountCard, decimal defaultContractDiscPercent, bool defaultContractIsBonusCard, decimal defaultContractBonus, bool defaultContractControlReceivables, decimal defaultContractReceivablesLimit, DateTime? defaultContractBCValidPeriodStart, DateTime? defaultContractBCValidPeriodEnd, bool defaultContractBCCanceled)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(birthday) && DateTime.TryParse(birthday, out var result))
			{
				partner.Birthday = result;
			}
			partner.PartnerContracts = new List<PartnerContract>();
			PartnerContract item = new PartnerContract
			{
				Name = defaultContractName,
				CurrencyCode = defaultContractCurr,
				PriceType = defaultContractPriceType,
				Percent = defaultContractDiscPercent,
				DiscountCard = defaultContractDiscountCard,
				ContractDate = defaultContractDate,
				Description = defaultContractDescription,
				IsDefault = true,
				IsBonusCard = defaultContractIsBonusCard,
				Bonus = defaultContractBonus,
				ControlReceivables = defaultContractControlReceivables,
				ReceivablesLimit = defaultContractReceivablesLimit,
				BonusCardValidPeriodStart = defaultContractBCValidPeriodStart,
				BonusCardValidPeriodEnd = defaultContractBCValidPeriodEnd,
				BonusCardCanceled = defaultContractBCCanceled
			};
			partner.PartnerContracts.Add(item);
			if (!string.IsNullOrWhiteSpace(settlementAccount))
			{
				partner.SettlementAccounts = new List<PartnerSettlementAccount>();
				PartnerSettlementAccount item2 = new PartnerSettlementAccount
				{
					Account = settlementAccount
				};
				partner.SettlementAccounts.Add(item2);
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(partner);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool EditPartner(Partner partner, string supplier, string customer, string foreign, string birthday)
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(supplier))
			{
				partner.Supplier = Convert.ToBoolean(Convert.ToInt16(supplier));
			}
			if (!string.IsNullOrWhiteSpace(customer))
			{
				partner.Customer = Convert.ToBoolean(Convert.ToInt16(customer));
			}
			if (!string.IsNullOrWhiteSpace(foreign))
			{
				partner.Foreign = Convert.ToBoolean(Convert.ToInt16(foreign));
			}
			if (!string.IsNullOrWhiteSpace(birthday) && DateTime.TryParse(birthday, out var result))
			{
				partner.Birthday = result;
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Edit(partner);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool UpdatePartnerCode(string oldCode, string newCode)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.EditOnlyCode(oldCode, newCode);
			_lastErrorMessage = string.Empty;
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportContract(string partnerCode, string contractCode, string currCode, string priceType, string contractName, DateTime? contractDate, string description, string discountCard, decimal discountPercent, bool isBonusCard, decimal bonus, decimal point, bool controlReceivables, decimal receivablesLimit, DateTime? bonusCardValidPeriodStart, DateTime? bonusCardValidPeriodEnd, bool bonusCardCanceled)
	{
		try
		{
			Contract exchange = new Contract
			{
				PartnerCode = partnerCode,
				Code = contractCode,
				Name = contractName,
				PriceType = priceType,
				Percent = discountPercent,
				IsDefault = false,
				CurrencyCode = currCode,
				DiscountCard = discountCard,
				IsBonusCard = isBonusCard,
				Bonus = bonus,
				Point = point,
				ControlReceivables = controlReceivables,
				ReceivablesLimit = receivablesLimit,
				ContractDate = contractDate,
				Description = description,
				BonusCardValidPeriodStart = bonusCardValidPeriodStart,
				BonusCardValidPeriodEnd = bonusCardValidPeriodEnd,
				BonusCardCanceled = bonusCardCanceled
			};
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(exchange);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool EditContract(string partnerCode, string contractCode, string currCode, string priceType, string contractName, DateTime? contractDate, string description, string discountCard, string discountPercent, string isBonusCard, string bonus, string point, string controlReceivables, string receivablesLimit, string bonusCardValidPeriodStart, string bonusCardValidPeriodEnd, string bonusCardCanceled)
	{
		try
		{
			Contract contract = new Contract
			{
				PartnerCode = partnerCode,
				Code = contractCode,
				Name = contractName,
				PriceType = priceType,
				CurrencyCode = currCode,
				DiscountCard = discountCard,
				ContractDate = contractDate,
				Description = description
			};
			if (!string.IsNullOrWhiteSpace(discountPercent) && decimal.TryParse(discountPercent, out var result))
			{
				contract.Percent = result;
			}
			if (!string.IsNullOrWhiteSpace(isBonusCard))
			{
				contract.IsBonusCard = Convert.ToBoolean(Convert.ToInt16(isBonusCard));
			}
			if (!string.IsNullOrWhiteSpace(bonus) && decimal.TryParse(bonus, out var result2))
			{
				contract.Bonus = result2;
			}
			if (!string.IsNullOrWhiteSpace(point) && decimal.TryParse(point, out var result3))
			{
				contract.Point = result3;
			}
			if (!string.IsNullOrWhiteSpace(controlReceivables))
			{
				contract.ControlReceivables = Convert.ToBoolean(Convert.ToInt16(controlReceivables));
			}
			if (!string.IsNullOrWhiteSpace(receivablesLimit) && decimal.TryParse(receivablesLimit, out var result4))
			{
				contract.ReceivablesLimit = result4;
			}
			if (!string.IsNullOrWhiteSpace(bonusCardCanceled))
			{
				contract.BonusCardCanceled = Convert.ToBoolean(Convert.ToInt16(bonusCardCanceled));
			}
			if (!string.IsNullOrWhiteSpace(bonusCardValidPeriodStart) && DateTime.TryParse(bonusCardValidPeriodStart, out var result5))
			{
				contract.BonusCardValidPeriodStart = result5;
			}
			if (!string.IsNullOrWhiteSpace(bonusCardValidPeriodEnd) && DateTime.TryParse(bonusCardValidPeriodEnd, out var result6))
			{
				contract.BonusCardValidPeriodEnd = result6;
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Edit(contract);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportPartSettlementAccount(string partnerCode, string accCode, string accName, bool isDefault)
	{
		try
		{
			Partner partner = new Partner
			{
				Code = partnerCode,
				SettlementAccounts = new List<PartnerSettlementAccount>()
			};
			PartnerSettlementAccount partnerSettlementAccount = new PartnerSettlementAccount
			{
				Account = accCode,
				IsDefault = isDefault
			};
			if (!string.IsNullOrWhiteSpace(accName))
			{
				partnerSettlementAccount.Name = accName;
			}
			partner.SettlementAccounts.Add(partnerSettlementAccount);
			TradeSettings.CreateApiClient().ExchangeRoutes.ImportSettlementAccount(partner);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public Goods CreateGood()
	{
		return new Goods();
	}

	public bool ImportGood(Goods good, string pluCode, string imagePath)
	{
		try
		{
			good.AltUnit = good.Unit;
			if (string.IsNullOrWhiteSpace(pluCode))
			{
				good.PLUCode = null;
			}
			else
			{
				if (!int.TryParse(pluCode, out var result))
				{
					throw new Exception(Resources.PLUCodeNegetivValueErrorMessage);
				}
				good.PLUCode = result;
			}
			if (!string.IsNullOrWhiteSpace(imagePath))
			{
				good.Image = Convert.ToBase64String(ReadFileToByteArray(imagePath));
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(good);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool EditGood(Goods good, string vat, string showInPriceList, string isWeight, string minQTY, string maxQty, string extraPercent, string bonus, string point, string coeff, string discount, string barCode, string ecoFeeRate, string pluCode, string checkMarking, string labelCoeff, string imagePath, string ignoreBonus = "")
	{
		try
		{
			if (!string.IsNullOrWhiteSpace(vat))
			{
				good.VAT = Convert.ToBoolean(Convert.ToInt16(vat));
			}
			if (!string.IsNullOrWhiteSpace(showInPriceList))
			{
				good.ShowInPriceList = Convert.ToBoolean(Convert.ToInt16(showInPriceList));
			}
			if (!string.IsNullOrWhiteSpace(isWeight))
			{
				good.IsWeight = Convert.ToBoolean(Convert.ToInt16(isWeight));
			}
			if (!string.IsNullOrWhiteSpace(checkMarking))
			{
				good.CheckMarking = Convert.ToBoolean(Convert.ToInt16(checkMarking));
			}
			if (!string.IsNullOrWhiteSpace(labelCoeff) && decimal.TryParse(labelCoeff, out var result))
			{
				good.LabelCoeff = result;
			}
			if (!string.IsNullOrWhiteSpace(minQTY) && decimal.TryParse(minQTY, out var result2))
			{
				good.MinQTY = result2;
			}
			if (!string.IsNullOrWhiteSpace(maxQty) && decimal.TryParse(maxQty, out var result3))
			{
				good.MaxQTY = result3;
			}
			if (!string.IsNullOrWhiteSpace(extraPercent) && decimal.TryParse(extraPercent, out var result4))
			{
				good.Extra = result4;
			}
			if (!string.IsNullOrWhiteSpace(bonus) && decimal.TryParse(bonus, out var result5))
			{
				good.Bonus = result5;
			}
			if (!string.IsNullOrWhiteSpace(point) && decimal.TryParse(point, out var result6))
			{
				good.Point = result6;
			}
			if (!string.IsNullOrWhiteSpace(ignoreBonus))
			{
				good.IgnoreBonus = Convert.ToBoolean(Convert.ToInt16(ignoreBonus));
			}
			if (!string.IsNullOrWhiteSpace(coeff) && decimal.TryParse(coeff, out var result7))
			{
				good.Coeff = result7;
			}
			if (!string.IsNullOrWhiteSpace(discount) && decimal.TryParse(discount, out var result8))
			{
				good.Discount = result8;
			}
			if (!string.IsNullOrWhiteSpace(ecoFeeRate) && decimal.TryParse(ecoFeeRate, out var result9))
			{
				good.EnvironmentalFeePercent = result9;
			}
			if (!string.IsNullOrWhiteSpace(pluCode))
			{
				if (!int.TryParse(pluCode, out var result10))
				{
					throw new ApiException(Resources.PLUCodeNegetivValueErrorMessage);
				}
				good.PLUCode = result10;
			}
			if (!string.IsNullOrWhiteSpace(imagePath))
			{
				good.Image = Convert.ToBase64String(ReadFileToByteArray(imagePath));
			}
			good.BarCode = barCode;
			TradeSettings.CreateApiClient().ExchangeRoutes.Edit(good);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool UpdateGoodCode(string oldCode, string newCode)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.GoodEditOnlyCode(oldCode, newCode);
			_lastErrorMessage = string.Empty;
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportComplects(string complectCode, string partCode, string unitCode, decimal qty, decimal complectQty, string summPart, string summ)
	{
		try
		{
			ComplectPart complectPart = new ComplectPart
			{
				DestCode = complectCode,
				SourceCode = partCode,
				Unit = unitCode,
				Quantity = qty,
				ComplectQuantity = complectQty
			};
			if (!string.IsNullOrWhiteSpace(summPart))
			{
				complectPart.SummPart = Convert.ToDecimal(summPart);
			}
			if (!string.IsNullOrWhiteSpace(summ))
			{
				complectPart.Summ = Convert.ToDecimal(summ);
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(complectPart);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportQntUnits(string goodCode, string unitCode, decimal coef, bool isAltUnit, string imagePath)
	{
		try
		{
			QuantityUnitPart quantityUnitPart = new QuantityUnitPart
			{
				GoodsCode = goodCode,
				Unit = unitCode,
				Coef = coef,
				IsAltUnit = isAltUnit
			};
			if (!string.IsNullOrWhiteSpace(imagePath))
			{
				quantityUnitPart.Image = Convert.ToBase64String(ReadFileToByteArray(imagePath));
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(quantityUnitPart);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool EditQntUnit(string goodCode, string unitCode, string coef, string isAltUnit, string imagePath)
	{
		try
		{
			QuantityUnitPart quantityUnitPart = new QuantityUnitPart
			{
				GoodsCode = goodCode,
				Unit = unitCode
			};
			if (!string.IsNullOrWhiteSpace(coef) && decimal.TryParse(coef, out var result))
			{
				quantityUnitPart.Coef = result;
			}
			if (!string.IsNullOrWhiteSpace(isAltUnit))
			{
				quantityUnitPart.IsAltUnit = Convert.ToBoolean(Convert.ToInt16(isAltUnit));
			}
			if (!string.IsNullOrWhiteSpace(imagePath))
			{
				quantityUnitPart.Image = Convert.ToBase64String(ReadFileToByteArray(imagePath));
			}
			TradeSettings.CreateApiClient().ExchangeRoutes.Edit(quantityUnitPart);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportBarCodes(string goodCode, string unitCode, string barCode)
	{
		try
		{
			MTBarCode exchange = new MTBarCode
			{
				GoodsCode = goodCode,
				Unit = unitCode,
				BarCode = barCode
			};
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(exchange);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportAddGoodsToSegment(string segmentCode, string goodCode)
	{
		try
		{
			GoodsBySegment exchange = new GoodsBySegment
			{
				SegmentCode = segmentCode,
				MTCode = goodCode
			};
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(exchange);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportPrices(string priceTypeCode, string goodCode, string unitCode, DateTime priceDate, string currCode, decimal price)
	{
		try
		{
			Prices exchange = new Prices
			{
				PriceType = priceTypeCode,
				Code = goodCode,
				Unit = unitCode,
				Date = priceDate,
				CurrencyCode = currCode,
				Price = price
			};
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(exchange);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportPartnersMT(string partnerCode, string type, string partnerItemCode, string partnerItemName, string partnerItemUnit, decimal coeff, string internalItemCode, string internalItemUnitCode)
	{
		try
		{
			PartnersMT exchange = new PartnersMT
			{
				PartnerCode = partnerCode,
				Type = type,
				PartnerItemCode = partnerItemCode,
				PartnerItemName = partnerItemName,
				PartnerItemUnitName = partnerItemUnit,
				Coeff = coeff,
				ItemCode = internalItemCode,
				ItemUnitCode = internalItemUnitCode
			};
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(exchange);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public CashIn CreateCashIn()
	{
		return new CashIn();
	}

	public bool ImportCashIn(CashIn cashIn)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(cashIn);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public CashOut CreateCashOut()
	{
		return new CashOut();
	}

	public bool ImportCashOut(CashOut cashOut)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(cashOut);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public BankIn CreateBankIn()
	{
		return new BankIn();
	}

	public bool ImportBankIn(BankIn bankIn)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(bankIn);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public BankOut CreateBankOut()
	{
		return new BankOut();
	}

	public bool ImportBankOut(BankOut bankOut)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(bankOut);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public PartnerRemGroupCorrect CreatePartnerRemGroupCorrect()
	{
		return new PartnerRemGroupCorrect();
	}

	public bool ImportPartnerRemGroupCorrect(PartnerRemGroupCorrect partnerRemGroupCorrect)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(partnerRemGroupCorrect);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public BonusGroupCorrect CreateBonusGroupCorrect()
	{
		return new BonusGroupCorrect();
	}

	public bool ImportBonusGroupCorrect(BonusGroupCorrect bonusGroupCorrect)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(bonusGroupCorrect);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTMove CreateMTMove()
	{
		return new MTMove();
	}

	public bool ImportMTMove(MTMove mtMove)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtMove);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTMoveWithConfirmation CreateMTMoveWithConfirmation()
	{
		return new MTMoveWithConfirmation();
	}

	public bool ImportMTMoveWithConfirmation(MTMoveWithConfirmation mtMoveWithConfirmation)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtMoveWithConfirmation);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTMoveOrder CreateMTMoveOrder()
	{
		return new MTMoveOrder();
	}

	public bool ImportMTMoveOrder(MTMoveOrder mtMoveOrder)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtMoveOrder);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTExpense CreateMTExpense()
	{
		return new MTExpense();
	}

	public bool ImportMTExpense(MTExpense mtExpense)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtExpense);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public PartnerRemMove CreatePartnerRemMove()
	{
		return new PartnerRemMove();
	}

	public bool ImportPartnerRemMove(PartnerRemMove partnerRemMove)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(partnerRemMove);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTReturnToSupplier CreateMTReturnToSupplier()
	{
		return new MTReturnToSupplier();
	}

	public bool ImportMTReturnToSupplier(MTReturnToSupplier mtReturnToSupplier)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtReturnToSupplier);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTIncome CreateMTIncome()
	{
		return new MTIncome();
	}

	public bool ImportMTIncome(MTIncome mtIncome, bool byPrice)
	{
		try
		{
			string requestParamJSON = JsonSerializer.Serialize(new MTIncomeRequestParams
			{
				ByPrice = byPrice
			}, JsonSerializerFactory.JsonOptions);
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtIncome, requestParamJSON);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTIncomeImport CreateMTIncomeImport()
	{
		return new MTIncomeImport();
	}

	public bool ImportMTIncomeImport(MTIncomeImport mtIncomeImport, bool byPrice)
	{
		try
		{
			string requestParamJSON = JsonSerializer.Serialize(new MTIncomeImportRequestParams
			{
				ByPrice = byPrice
			}, JsonSerializerFactory.JsonOptions);
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtIncomeImport, requestParamJSON);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTIncomeRetail CreateMTIncomeRetail()
	{
		return new MTIncomeRetail();
	}

	public bool ImportMTIncomeRetail(MTIncomeRetail mtIncomeRetail)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtIncomeRetail);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTIncomeRem CreateMTIncomeRem()
	{
		return new MTIncomeRem();
	}

	public ReceivedService CreateReceivedService()
	{
		return new ReceivedService();
	}

	public bool ImportReceivedService(ReceivedService receivedService)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(receivedService);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool ImportMTIncomeRem(MTIncomeRem mtIncomeRem)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtIncomeRem);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTInvoice CreateMTInvoice()
	{
		return new MTInvoice();
	}

	public bool ImportMTInvoice(MTInvoice mtInvoiceDoc, bool byPrice = false)
	{
		try
		{
			string requestParamJSON = JsonSerializer.Serialize(new MTInvoiceRequestParams
			{
				ByPrice = byPrice
			}, JsonSerializerFactory.JsonOptions);
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtInvoiceDoc, requestParamJSON);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTInvoiceRetail CreateMTInvoiceRetail()
	{
		return new MTInvoiceRetail();
	}

	public bool ImportMTInvoiceRetail(MTInvoiceRetail mtInvoiceRetailDoc)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtInvoiceRetailDoc);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTBill CreateMTBill()
	{
		return new MTBill();
	}

	public bool ImportMTBill(MTBill mtBillDoc)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtBillDoc);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTComplectation CreateMTComplectation()
	{
		return new MTComplectation();
	}

	public bool ImportMTComplectation(MTComplectation mtComplectation)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtComplectation);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTDecomplectation CreateMTDecomplectation()
	{
		return new MTDecomplectation();
	}

	public bool ImportMTDecomplectation(MTDecomplectation mtDecomplectation)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtDecomplectation);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTReturnFromCustomer CreateMTReturnFromCustomer()
	{
		return new MTReturnFromCustomer();
	}

	public bool ImportMTReturnFromCustomer(MTReturnFromCustomer mtRetFromCustDoc, string baseDocType, string baseDocDate, string baseDocNum)
	{
		try
		{
			string requestParamJSON = JsonSerializer.Serialize(new MTReturnFromCustomerRequestParams
			{
				BaseDocType = baseDocType,
				BaseDocDate = baseDocDate,
				BaseDocNum = baseDocNum
			}, JsonSerializerFactory.JsonOptions);
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtRetFromCustDoc, requestParamJSON);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTStockTaking CreateMTStockTaking()
	{
		return new MTStockTaking();
	}

	public bool ImportMTStockTaking(MTStockTaking mtStockTakingDoc)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtStockTakingDoc);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public MTStockTakingRetail CreateMTStockTakingRetail()
	{
		return new MTStockTakingRetail();
	}

	public bool ImportMTStockTakingRetail(MTStockTakingRetail mtStockTakingRetailDoc)
	{
		try
		{
			TradeSettings.CreateApiClient().ExchangeRoutes.Import(mtStockTakingRetailDoc);
			_lastErrorMessage = "";
			return true;
		}
		catch (Exception ex)
		{
			_lastErrorMessage = ex.Message;
			return false;
		}
	}

	public byte[] ReadFileToByteArray(string fileFullName)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (Stream stream = ReadFileToStream(fileFullName))
		{
			stream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}

	public Stream ReadFileToStream(string fileFullName)
	{
		try
		{
			return File.OpenRead(fileFullName);
		}
		catch (Exception)
		{
			throw new Exception(string.Format(ArmSoft.SME.Core.AddIn.Resources.FileDoesNotExist, fileFullName));
		}
	}

	public string GetTempPath()
	{
		return string.Format("{0}{1}", Settings.GetTempPath(), "ExcelAddin");
	}
}
