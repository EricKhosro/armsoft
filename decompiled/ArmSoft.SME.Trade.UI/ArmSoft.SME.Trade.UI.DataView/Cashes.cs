using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using ArmSoft.ECRConnector;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.Controls;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Directories;
using ArmSoft.SME.Trade.Client.Models;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.Common.SmartEnums;
using ArmSoft.SME.Trade.UI.Helpers;
using ArmSoft.SME.Trade.UI.OtherWindows;
using DevExpress.Data;
using DevExpress.Xpf.LayoutControl;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(10013)]
public class Cashes : TradeDirectoryDataView<ArmSoft.SME.Trade.Client.Models.Cashes, CashesDataProvider.Param>
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
	{
		WriteIndented = true
	};

	public override short DirectoryType => DirectoryTypeSE.Cash.Value;

	public override string HelpId => "Purchases_Lists_Otherlists_Cash";

	public bool CheckUserAccess
	{
		get
		{
			return (bool)Parameters["CheckUserAccess"].Value;
		}
		set
		{
			Parameters["CheckUserAccess"].Value = value;
		}
	}

	public bool ShowOnlyECRCashes
	{
		get
		{
			return (bool)Parameters["ShowOnlyECRCashes"].Value;
		}
		set
		{
			Parameters["ShowOnlyECRCashes"].Value = value;
		}
	}

	public Cashes()
	{
		DataProvider = new CashesDataProvider(TradeSettings.CreateTradeClient());
		FillColumnsFromDataProvider();
		AddParameter("ShowOnlyECRCashes", "ShowOnlyECRCashes", false);
		AddParameter("CheckUserAccess", "CheckUserAccess", false);
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("ECRSN", value: false);
		dvp.Set_ColumnVisible("ECRCrn", value: false);
		dvp.Set_ColumnVisible("ECRIP", value: false);
		dvp.Set_ColumnVisible("ECRPort", value: false);
		dvp.Set_ColumnVisible("ECRPassword", value: false);
		dvp.Set_ColumnVisible("ECRDepWithVAT", value: false);
		dvp.Set_ColumnVisible("ECRDepWithOutVAT", value: false);
		dvp.Set_ColumnVisible("ECRPrintMode", value: false);
		dvp.Set_ColumnVisible("ShowECRTracking", value: false);
		dvp.Set_ColumnVisible("B2BPublish", value: false);
		if (!dvp.IsSorted)
		{
			dvp.Set_ColumnSortOrder("IsDefault", ColumnSortOrder.Descending);
			dvp.Set_ColumnSortOrder("Code");
		}
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.ShowOnlyECRCashes = ShowOnlyECRCashes;
		ParametersProvider.CheckUserAccess = CheckUserAccess;
	}

	public override void InitContextFunctions()
	{
		base.InitContextFunctions();
		ArmSoft.SME.Trade.Client.Models.Cashes currentRow = (ArmSoft.SME.Trade.Client.Models.Cashes)Settings.HostingEnvironment.CurrentViewPanel.FocusedRow();
		if (currentRow == null)
		{
			return;
		}
		Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "PrintECRReport", ArmSoft.SME.Common.Resources.PrintECRReport, () =>
		{
			PrintECRReport(currentRow.Code);
		});
		if (currentRow.IsECR)
		{
			Settings.HostingEnvironment.CurrentViewPanel.AddContextFunction("ContextFunctions", "ViewECRCheck", ArmSoft.SME.Trade.Common.Resources.ViewECRCheck, () =>
			{
				ViewECRCheck(currentRow);
			});
		}
	}

	private static void PrintECRReport(string cashDeskCode)
	{
		new ECRPrintReportWindow(cashDeskCode).ShowDialog();
	}

	private void ViewECRCheck(ArmSoft.SME.Trade.Client.Models.Cashes currentRow)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Name = "ViewECR",
			Caption = ArmSoft.SME.Trade.Common.Resources.ViewECRCheck,
			HelpId = HelpId
		};
		Utilities.GetReceiptResponse getReceiptResponse = null;
		ECR ecr = null;
		TextEditExt textEditExt = dialogWindow.AddStringControl("ViewECR", ArmSoft.SME.Common.Resources.ECRCheckNum, 8, isRequired: true);
		TextEditExt textEditExt2 = dialogWindow.AddStringControl("ECRCrn", ArmSoft.SME.Common.Resources.ECRCrn, 12, isRequired: true);
		textEditExt2.Text = currentRow.ECRCrn;
		if (!dialogWindow.ShowDialog().Value)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		try
		{
			string JSON = string.Empty;
			ECRHelpers.InitECR(ref ecr, currentRow.Code);
			if (ecr == null)
			{
				ArmSoft.SME.Core.UI.MessageBox.Show(ArmSoft.SME.Common.Resources.ECRInitFailed, ArmSoft.SME.Common.Resources.ECRPrintLastCheckDuplicate, MessageBoxButton.OK, MessageBoxImage.Asterisk);
				return;
			}
			ecr.Connect();
			flag = true;
			ecr.OperatorLogin(Convert.ToInt32(currentRow.ECROperator), currentRow.ECROperatorPassword);
			flag2 = true;
			getReceiptResponse = ecr.GetReceiptWithJSON(textEditExt.Text, textEditExt2.Text, ref JSON);
			if (getReceiptResponse != null)
			{
				CreateECRReceiptResponseWindow(getReceiptResponse, JSON);
			}
		}
		catch (Utilities.ECRException ex)
		{
			Settings.ErrorHandler(new ASException(ex.Message));
		}
		catch (Exception ex2)
		{
			Settings.ErrorHandler(ex2);
		}
		finally
		{
			try
			{
				if (flag)
				{
					if (flag2)
					{
						ecr.OperatorLogout();
					}
					ecr.DisConnect();
				}
			}
			catch
			{
			}
		}
	}

	private void CreateECRReceiptResponseWindow(Utilities.GetReceiptResponse response, string json)
	{
		DialogWindow dialogWindow = new DialogWindow
		{
			Name = "ECRViewDialog",
			HelpId = HelpId
		};
		string text = "";
		TimeSpan timeSpan;
		try
		{
			timeSpan = TimeSpan.FromMilliseconds(Convert.ToInt64(response.time) * 1000);
		}
		catch
		{
			timeSpan = TimeSpan.FromMilliseconds(Convert.ToInt64(response.time));
		}
		response.time = (new DateTime(1970, 1, 1) + timeSpan).ToString();
		switch (Convert.ToInt32(response.saleType))
		{
		case 0:
			text = "0 - " + ArmSoft.SME.Trade.Common.Resources.Sale;
			break;
		case 2:
			text = "2 - " + ArmSoft.SME.Trade.Common.Resources.ReturnWord;
			break;
		case 3:
			text = "3 - " + ArmSoft.SME.Trade.Common.Resources.PrePayment;
			break;
		}
		dialogWindow.Height = 525.0;
		dialogWindow.Width = 1150.0;
		LayoutGroup container = dialogWindow.AddHorizontalLayoutGroup(null, LayoutGroupView.Tabs);
		LayoutGroup container2 = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.Group, ArmSoft.SME.Common.Resources.ECRCheck);
		LayoutGroup layoutGroup = dialogWindow.AddVerticalLayoutGroup(container, LayoutGroupView.Group, ArmSoft.SME.Trade.Common.Resources.JSON);
		dialogWindow.AddStringControl(container2, "NextNumber", ArmSoft.SME.Trade.Common.Resources.NextNumber, 50).Text = response.rseq.ToString();
		dialogWindow.AddStringControl(container2, "CashierID", ArmSoft.SME.Trade.Common.Resources.Cashier, 50).Text = response.cid;
		dialogWindow.AddStringControl(container2, "RegistrDate", ArmSoft.SME.Trade.Common.Resources.RegistrDate, 50).Text = response.time;
		dialogWindow.AddStringControl(container2, "TotalAmount", ArmSoft.SME.Common.Resources.Total + " " + ArmSoft.SME.Common.Resources.Summa.ToLower(), 50).Text = response.ta;
		dialogWindow.AddStringControl(container2, "Cash", ArmSoft.SME.Trade.Common.Resources.CashSumma, 50).Text = response.cash;
		dialogWindow.AddStringControl(container2, "NonCash", ArmSoft.SME.Common.Resources.PlCardSum + " " + ArmSoft.SME.Common.Resources.Summa.ToLower(), 50).Text = response.card;
		dialogWindow.AddStringControl(container2, "PartSumm", ArmSoft.SME.Trade.Common.Resources.PartSumm, 50).Text = response.ppa;
		dialogWindow.AddStringControl(container2, "UsedPrePayment", ArmSoft.SME.Trade.Common.Resources.UsedPrePayment, 50).Text = response.ppu;
		dialogWindow.AddStringControl(container2, "SaleType", ArmSoft.SME.Common.Resources.DealType, 50).Text = text;
		dialogWindow.AddStringControl(container2, "BuyerTaxCode", ArmSoft.SME.Common.Resources.BuyerTaxCode, 50).Text = response.rTin;
		dialogWindow.AddStringControl(container2, "ECRetCheckNum", ArmSoft.SME.Trade.Common.Resources.ECRetCheckNum, 50).Text = response.@ref;
		dialogWindow.AddStringControl(container2, "ECRetCheckCrn", ArmSoft.SME.Trade.Common.Resources.ECRetCheckCrn, 50).Text = response.refcrn;
		DocumentGridControl documentGridControl = dialogWindow.AddGridControl(container2, "ERCTotalsGrid");
		try
		{
			documentGridControl.AllowAdd = false;
			documentGridControl.AllowDelete = false;
			documentGridControl.AllowDuplicate = false;
			documentGridControl.ShowButtons = false;
			List<Utilities.SubTotal> list = new List<Utilities.SubTotal>();
			foreach (Utilities.SubTotal total in response.totals)
			{
				Utilities.SubTotal item = new Utilities.SubTotal
				{
					adg = total.adg,
					adsc = total.adsc,
					did = total.did,
					dsc = total.dsc,
					dsct = total.dsct,
					dt = total.dt,
					dtm = total.dtm,
					gc = total.gc,
					gn = total.gn,
					mu = total.mu,
					p = total.p,
					qty = total.qty,
					rpid = total.rpid,
					t = total.t,
					tt = total.tt
				};
				list.Add(item);
			}
			documentGridControl.ItemsSource = list;
			documentGridControl.AddStringColumn("did", ArmSoft.SME.Trade.Common.Resources.SectionNum, 5);
			documentGridControl.AddStringColumn("dt", ArmSoft.SME.Trade.Common.Resources.SectionVAT, 5);
			documentGridControl.AddStringColumn("dtm", ArmSoft.SME.Trade.Common.Resources.SectionVatInv, 5);
			documentGridControl.AddStringColumn("t", ArmSoft.SME.Trade.Common.Resources.TotalWithoutVat, 5);
			documentGridControl.AddStringColumn("tt", ArmSoft.SME.Trade.Common.Resources.TotalWithVat, 5);
			documentGridControl.AddStringColumn("gc", ArmSoft.SME.Common.Resources.Code, 2);
			documentGridControl.AddStringColumn("gn", ArmSoft.SME.Common.Resources.Name, 4);
			documentGridControl.AddStringColumn("qty", ArmSoft.SME.Common.Resources.Quantity, 3);
			documentGridControl.AddStringColumn("p", ArmSoft.SME.Common.Resources.Price, 2);
			documentGridControl.AddStringColumn("adg", ArmSoft.SME.Trade.Common.Resources.CPACode, 5);
			documentGridControl.AddStringColumn("mu", ArmSoft.SME.Common.Resources.QuantityUnitShort, 2);
			documentGridControl.AddStringColumn("dsc", ArmSoft.SME.Trade.Common.Resources.Discount, 2);
			documentGridControl.AddStringColumn("adsc", ArmSoft.SME.Trade.Common.Resources.ProportionallyDiscount, 5);
			documentGridControl.AddStringColumn("dsct", ArmSoft.SME.Trade.Common.Resources.DiscountType, 5);
			documentGridControl.AddStringColumn("rpid", ArmSoft.SME.Trade.Common.Resources.RowID, 5);
		}
		catch
		{
		}
		layoutGroup.Height = dialogWindow.Height;
		layoutGroup.Width = dialogWindow.Width;
		ScrollViewer scrollViewer = new ScrollViewer
		{
			VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
			HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
			Margin = new Thickness(0.0),
			BorderThickness = new Thickness(1.0)
		};
		TextBox textBox = new TextBox
		{
			IsReadOnly = true,
			Background = Settings.GetBackgroundBrush(),
			HorizontalAlignment = HorizontalAlignment.Stretch,
			VerticalAlignment = VerticalAlignment.Stretch
		};
		JsonElement value = JsonSerializer.Deserialize<JsonElement>(json);
		textBox.Text = JsonSerializer.Serialize(value, JsonSerializerOptions);
		scrollViewer.Content = textBox;
		layoutGroup.Children.Add(scrollViewer);
		dialogWindow.ShowDialog();
	}
}
