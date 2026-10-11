using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows;
using ArmSoft.Cloud.Client;
using ArmSoft.Cloud.Client.AS7X;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI.Report;

namespace ArmSoft.SME.Core.UI;

public class GetPaymentDetailsReportViewer : AsReport
{
	public override string Name => "GetPaymentDetails";

	public override string Caption()
	{
		return Resources.PaymentDetails;
	}

	public GetPaymentDetailsReportViewer()
	{
		ReportPanel.IsSpreadSheetMode = false;
		string tableName = "PaymentDetails";
		ReportPanel.ClearAll();
		ReportPanel.AddString("");
		ReportPanel.AddTableDefinition(tableName, GetListOfRepColumns());
		List<AsRepRowCell> list = new List<AsRepRowCell>();
		AsRepRowCell asRepRowCell = new AsRepRowCell
		{
			ColumnSpan = 2,
			Value = Resources.PaymentDetails,
			Alignment = TextAlignment.Center,
			FontWeight = FontWeights.Bold,
			FontSize = Convert.ToInt32(Settings.GetFontSize()) + 2
		};
		asRepRowCell.Borders.Add(BorderType.None);
		list.Add(asRepRowCell);
		ReportPanel.AddTableSpanRow(tableName, list);
		ReportPanel.AddString("");
		int num;
		List<object> values;
		using (CloudClient cloudClient = Settings.CloudClient)
		{
			PaymentDetails paymentDetails = cloudClient.AS7X.GetPaymentDetails(1);
			if (paymentDetails != null)
			{
				num = 2;
				List<object> list2 = new List<object>(num);
				CollectionsMarshal.SetCount(list2, num);
				Span<object> span = CollectionsMarshal.AsSpan(list2);
				span[0] = Resources.Name;
				span[1] = paymentDetails.CompanyName;
				values = list2;
				ReportPanel.AddTableRow(tableName, values);
				num = 2;
				List<object> list3 = new List<object>(num);
				CollectionsMarshal.SetCount(list3, num);
				Span<object> span2 = CollectionsMarshal.AsSpan(list3);
				span2[0] = Resources.RowNumber;
				span2[1] = paymentDetails.BankAccount;
				values = list3;
				ReportPanel.AddTableRow(tableName, values);
				num = 2;
				List<object> list4 = new List<object>(num);
				CollectionsMarshal.SetCount(list4, num);
				Span<object> span3 = CollectionsMarshal.AsSpan(list4);
				span3[0] = Resources.Bank;
				span3[1] = paymentDetails.BankName;
				values = list4;
				ReportPanel.AddTableRow(tableName, values);
			}
		}
		num = 2;
		List<object> list5 = new List<object>(num);
		CollectionsMarshal.SetCount(list5, num);
		Span<object> span4 = CollectionsMarshal.AsSpan(list5);
		span4[0] = Resources.OptionsTaxCode;
		span4[1] = "00807555";
		values = list5;
		ReportPanel.AddTableRow(tableName, values);
		num = 2;
		List<object> list6 = new List<object>(num);
		CollectionsMarshal.SetCount(list6, num);
		Span<object> span5 = CollectionsMarshal.AsSpan(list6);
		span5[0] = Resources.PaymentAim;
		span5[1] = Settings.LoginService.Key;
		values = list6;
		ReportPanel.AddTableRow(tableName, values);
	}

	private static List<AsRepColumn> GetListOfRepColumns()
	{
		List<AsRepColumn> list = new List<AsRepColumn>();
		AsRepColumn item = new AsRepColumn(150.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left,
			DataBorders = new ObservableCollection<BorderType> { BorderType.None }
		};
		list.Add(item);
		item = new AsRepColumn(200.0)
		{
			ValuesStyle = AsRepColStyle.TextStyle,
			CaptionAlignment = TextAlignment.Left,
			DataAlignment = TextAlignment.Left,
			DataFontWeight = FontWeights.Regular,
			DataBorders = new ObservableCollection<BorderType> { BorderType.None }
		};
		list.Add(item);
		return list;
	}
}
