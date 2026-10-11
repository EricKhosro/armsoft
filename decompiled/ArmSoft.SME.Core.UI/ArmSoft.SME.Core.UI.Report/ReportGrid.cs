using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using ArmSoft.SME.Common.Parameters;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace ArmSoft.SME.Core.UI.Report;

public class ReportGrid : XtraReport
{
	internal DetailBand Detail;

	internal TopMarginBand TopMargin;

	internal BottomMarginBand BottomMargin;

	internal XRControlStyle XrControlStyle4;

	internal PageHeaderBand PageHeader;

	internal ReportHeaderBand ReportHeader;

	internal PageFooterBand PageFooter;

	internal XRControlStyle XrControlStyle5;

	internal XRControlStyle XrControlStyle6;

	internal XRControlStyle XrControlStyle7;

	internal XRControlStyle XrControlStyle8;

	[DefaultValue(false)]
	public bool IsPreview { get; set; }

	public DetailBand RepDetail => Detail;

	public TopMarginBand RepTopMargin => TopMargin;

	public BottomMarginBand RepBottomMargin => BottomMargin;

	public PageHeaderBand RepPageHeader => PageHeader;

	public ReportHeaderBand RepHeader => ReportHeader;

	public PageFooterBand RepPageFooter => PageFooter;

	public ReportGrid()
	{
		InitializeComponent();
		BeforePrint += new SafeEventHandler<CancelEventArgs>(ReportGrid_BeforePrint).Handler;
	}

	private void InitializeComponent()
	{
		Detail = new DetailBand();
		TopMargin = new TopMarginBand();
		BottomMargin = new BottomMarginBand();
		XrControlStyle4 = new XRControlStyle();
		PageHeader = new PageHeaderBand();
		ReportHeader = new ReportHeaderBand();
		PageFooter = new PageFooterBand();
		XrControlStyle5 = new XRControlStyle();
		XrControlStyle6 = new XRControlStyle();
		XrControlStyle7 = new XRControlStyle();
		XrControlStyle8 = new XRControlStyle();
		((ISupportInitialize)this).BeginInit();
		Detail.Expanded = false;
		Detail.HeightF = 7.291667f;
		Detail.Name = "Detail";
		Detail.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		Detail.TextAlignment = TextAlignment.TopLeft;
		Margins.Left = 100f;
		Margins.Right = 100f;
		TopMargin.HeightF = 36.45833f;
		TopMargin.Name = "TopMargin";
		TopMargin.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		TopMargin.TextAlignment = TextAlignment.TopLeft;
		BottomMargin.HeightF = 48.95833f;
		BottomMargin.Name = "BottomMargin";
		BottomMargin.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		BottomMargin.TextAlignment = TextAlignment.TopLeft;
		XrControlStyle4.BackColor = Color.Gainsboro;
		XrControlStyle4.Name = "XrControlStyle4";
		XrControlStyle4.TextAlignment = TextAlignment.MiddleLeft;
		PageHeader.HeightF = 12.5f;
		PageHeader.Name = "PageHeader";
		ReportHeader.HeightF = 40.625f;
		ReportHeader.Name = "ReportHeader";
		PageFooter.HeightF = 6.25f;
		PageFooter.Name = "PageFooter";
		XrControlStyle5.Font = new Font("Times New Roman", 15.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
		XrControlStyle5.Name = "XrControlStyle5";
		XrControlStyle6.BackColor = SystemColors.ActiveCaption;
		XrControlStyle6.BorderColor = Color.Transparent;
		XrControlStyle6.Borders = BorderSide.All;
		XrControlStyle6.BorderWidth = 2f;
		XrControlStyle6.Font = new Font("Times New Roman", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
		XrControlStyle6.Name = "XrControlStyle6";
		XrControlStyle6.TextAlignment = TextAlignment.MiddleLeft;
		XrControlStyle7.Name = "XrControlStyle7";
		XrControlStyle7.TextAlignment = TextAlignment.MiddleLeft;
		XrControlStyle8.Name = "XrControlStyle8";
		Bands.AddRange(new Band[6] { Detail, TopMargin, BottomMargin, PageHeader, ReportHeader, PageFooter });
		Margins = new Margins(100, 100, 36, 49);
		StyleSheet.AddRange(new XRControlStyle[5] { XrControlStyle4, XrControlStyle5, XrControlStyle6, XrControlStyle7, XrControlStyle8 });
		Version = "24.1";
		((ISupportInitialize)this).EndInit();
	}

	public void Boolean_BeforePrint(object sender, PrintEventArgs e)
	{
		XRTableCell xRTableCell = (XRTableCell)sender;
		if (Convert.ToBoolean(GetCurrentColumnValue(xRTableCell.Name.Substring(6))))
		{
			xRTableCell.Text = "+";
		}
		else
		{
			xRTableCell.Text = "-";
		}
	}

	public void AdjustBookmarks()
	{
		IBookmarkNodeCollection bookmarkNodes = PrintingSystem.Document.BookmarkNodes;
		int num = 1;
		while (num < bookmarkNodes.Count)
		{
			if (bookmarkNodes[num].Text == bookmarkNodes[num - 1].Text)
			{
				bookmarkNodes.RemoveAt(num - 1);
			}
			else
			{
				num++;
			}
		}
	}

	private void ReportGrid_BeforePrint(object sender, CancelEventArgs e)
	{
		Band band = null;
		bool isPreview = ((ReportGrid)sender).IsPreview;
		foreach (object band2 in ((ReportGrid)sender).Bands)
		{
			if (band2 is TopMarginBand topMarginBand)
			{
				band = topMarginBand;
			}
		}
		if (band != null)
		{
			XtraReportTemplate.PrintAsInfo(band, Name, Settings.ParametersService.CompanyName(), isPreview);
		}
	}
}
