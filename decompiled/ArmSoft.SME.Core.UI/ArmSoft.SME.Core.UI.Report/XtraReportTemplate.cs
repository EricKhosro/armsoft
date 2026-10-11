using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Core.UI.Services;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using Microsoft.Extensions.DependencyInjection;

namespace ArmSoft.SME.Core.UI.Report;

public class XtraReportTemplate : XtraReport
{
	private readonly short _printModelType;

	private readonly ITemplateService _templateService;

	private DetailBand _detail;

	private TopMarginBand _topMargin;

	private BottomMarginBand _bottomMargin;

	public XtraReportTemplate(short printModelType)
		: this()
	{
		_printModelType = printModelType;
		DataSource = _templateService.InitDesignerByDocumentType(_printModelType, out var text);
		DisplayName = text;
	}

	public XtraReportTemplate()
	{
		InitializeComponent();
		_templateService = Settings.ServiceProvider.GetRequiredService<ITemplateService>();
	}

	private void InitializeComponent()
	{
		_detail = new DetailBand();
		_topMargin = new TopMarginBand();
		_bottomMargin = new BottomMarginBand();
		((ISupportInitialize)this).BeginInit();
		_detail.HeightF = 27.08333f;
		_detail.Name = "Detail";
		_detail.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		_detail.TextAlignment = TextAlignment.TopLeft;
		_topMargin.HeightF = 45.83333f;
		_topMargin.Name = "TopMargin";
		_topMargin.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		_topMargin.TextAlignment = TextAlignment.TopLeft;
		_bottomMargin.HeightF = 34.375f;
		_bottomMargin.Name = "BottomMargin";
		_bottomMargin.Padding = new PaddingInfo(0f, 0f, 0f, 0f, 100f);
		_bottomMargin.TextAlignment = TextAlignment.TopLeft;
		Bands.AddRange(new Band[3] { _detail, _topMargin, _bottomMargin });
		Margins = new Margins(100, 100, 46, 34);
		Version = "11.2";
		((ISupportInitialize)this).EndInit();
	}

	public static void PrintAsInfo(Band reportBand, string repName, string companyName, bool isPreview)
	{
		PrintInfo printInfo = new PrintInfo();
		float num = 0f;
		string source = Settings.FontFamily.Source;
		bool printASInfo = printInfo.PrintASInfo;
		int count = reportBand.Controls.Count;
		float topF = ((count != 0) ? (reportBand.Controls[count - 1].BottomF + 5f) : 15f);
		if (!string.IsNullOrEmpty(repName) && new PrintInfo(repName).PrintCompanyName)
		{
			XRLabel xRLabel = new XRLabel
			{
				Text = companyName,
				TopF = topF
			};
			num = xRLabel.RightF + 5f;
			xRLabel.Font = new Font(source, 8f);
			xRLabel.WidthF = 600f;
			reportBand.Controls.Add(xRLabel);
		}
		if (printASInfo & isPreview)
		{
			XRLabel child = new XRLabel
			{
				Text = string.Format(Resources.PrintedByAsSystem, Settings.ProgramName) + Resources.PrintedByAsSystemWebPage,
				TopF = topF,
				Left = (int)Math.Round(num),
				Font = new Font(source, 8f),
				WidthF = 600f
			};
			reportBand.Controls.Add(child);
		}
	}

	private void PrintDate()
	{
		if (new PrintInfo().PrintDate)
		{
			XRLabel child = new XRLabel
			{
				Text = DateTime.Now.FormatDDMMYY(),
				Font = new Font(Settings.FontFamily.Source, 8f),
				TextAlignment = TextAlignment.BottomLeft
			};
			_bottomMargin.Controls.Add(child);
		}
	}
}
