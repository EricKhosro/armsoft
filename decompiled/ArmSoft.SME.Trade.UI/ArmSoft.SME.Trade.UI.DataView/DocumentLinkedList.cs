using System;
using System.Runtime.CompilerServices;
using ArmSoft.SME.Common;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Trade.Client.Data.Reports;
using ArmSoft.SME.Trade.Common.Parameters;
using ArmSoft.SME.Trade.Common.SmartEnums;
using DevExpress.Xpf.Grid;
using DevExpress.XtraGrid;

namespace ArmSoft.SME.Trade.UI.DataView;

[DataView(11)]
public class DocumentLinkedList : DocumentsJournal<DocumentsJournalDataProvider.DataRow, DocumentsJournalDataProvider.Param>
{
	public string DocDate
	{
		get
		{
			return Convert.ToString(Parameters["DocDate"].Value);
		}
		set
		{
			Parameters["DocDate"].Value = value;
		}
	}

	public string DocumentTypeName
	{
		get
		{
			return Convert.ToString(Parameters["DocType"].Value);
		}
		set
		{
			Parameters["DocType"].Value = value;
		}
	}

	public Guid BaseDocISN
	{
		get
		{
			return (Guid)Parameters["BaseISN"].Value;
		}
		set
		{
			Parameters["BaseISN"].Value = value;
		}
	}

	public string DocNum
	{
		get
		{
			return Convert.ToString(Parameters["DocNum"].Value);
		}
		set
		{
			Parameters["DocNum"].Value = value;
		}
	}

	public override string Name => "DocumentLinkedList";

	public override string Caption
	{
		get
		{
			InlineArray5<object> buffer = default;
			buffer[0] = Resources.LinkedDoc;
			buffer[1] = DocumentTypeName;
			buffer[2] = DocNum;
			buffer[3] = Resources.DateShort;
			buffer[4] = DocDate;
			return string.Format("{0} ({1} N {2}, {3} {4}) ", (ReadOnlySpan<object?>)buffer);
		}
	}

	public override string HelpId => "DocumentLinkedList";

	public override bool DialogSupported => false;

	public DocumentLinkedList()
		: base(ReportTypesSE.AllDocument, (DocumentTypeSE)null)
	{
		DataProvider = new DocumentLinkedListDataProvider(TradeSettings.CreateTradeClient());
		AddParameter("DocDate", Resources.DocDateShort, "");
		AddParameter("DocNum", Resources.DocNumber, "");
		AddParameter("BaseISN", Resources.BaseDoc, "");
		Columns.Add("Link", "Link");
	}

	protected override void ApplyParametersToDataProvider()
	{
		ParametersProvider.DocISN = DocISN;
		ParametersProvider.BaseISN = BaseDocISN;
		ParametersProvider.DocOwner = DocOwner;
	}

	public override void BeforeLoadData(DataViewPanel dvp)
	{
		base.BeforeLoadData(dvp);
		dvp.Set_ColumnVisible("Link", value: false);
		dvp.Set_ColumnVisible("ISN", value: false);
		dvp.Set_ColumnVisible("Storage", value: false);
		dvp.Set_ColumnVisible("PartTaxCode", Settings.ParametersService.ShowPartnerTaxCode());
		dvp.Set_ColumnVisible("DocOwnerID", value: false);
		dvp.Set_ColumnVisible("DocState", value: false);
		dvp.Set_ColumnVisible("DocType", value: false);
		dvp.Set_ColumnFooterSum("Summ", value: true);
		dvp.GroupByColumns("Link");
		dvp.InnerGrid().Columns["Link"].SortMode = ColumnSortMode.Custom;
		dvp.InnerGrid().CustomColumnSort += OnCustomColumnSort;
		dvp.Set_ColumnSortOrder("DocDate");
		dvp.Set_ColumnSortOrder("OrderNum");
		dvp.Set_ColumnSortOrder("DocTypeName");
		dvp.Set_ColumnSortOrder("DocNum");
	}

	public override bool ShowExpanded()
	{
		return true;
	}

	private void OnCustomColumnSort(object sender, CustomColumnSortEventArgs e)
	{
		if (!(e.Column.FieldName == "Link"))
		{
			return;
		}
		e.Handled = true;
		string text = e.Value1.ToString();
		string text2 = e.Value2.ToString();
		if (text == text2)
		{
			e.Result = 0;
			return;
		}
		InlineArray4<object> buffer = default;
		buffer[0] = DocumentTypeName;
		buffer[1] = DocNum;
		buffer[2] = Resources.DateShort;
		buffer[3] = DocDate;
		if (text == string.Format(" {0} N {1}, {2} {3} ", (ReadOnlySpan<object?>)buffer))
		{
			e.Result = -1;
			return;
		}
		if (text == Resources.ParentDoc)
		{
			InlineArray4<object> buffer2 = default;
			buffer2[0] = DocumentTypeName;
			buffer2[1] = DocNum;
			buffer2[2] = Resources.DateShort;
			buffer2[3] = DocDate;
			if (text2 == string.Format(" {0} N {1}, {2} {3} ", (ReadOnlySpan<object?>)buffer2))
			{
				e.Result = 1;
			}
			else
			{
				e.Result = -1;
			}
			return;
		}
		if (!(text2 == Resources.ParentDoc))
		{
			InlineArray4<object> buffer3 = default;
			buffer3[0] = DocumentTypeName;
			buffer3[1] = DocNum;
			buffer3[2] = Resources.DateShort;
			buffer3[3] = DocDate;
			if (!(text2 == string.Format(" {0} N {1}, {2} {3} ", (ReadOnlySpan<object?>)buffer3)))
			{
				e.Result = -1;
				return;
			}
		}
		e.Result = 1;
	}
}
