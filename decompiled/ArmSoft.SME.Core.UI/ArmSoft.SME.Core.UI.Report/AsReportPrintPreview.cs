using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Parameters;
using DevExpress.Drawing.Printing;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace ArmSoft.SME.Core.UI.Report;

public class AsReportPrintPreview
{
	private ReportGrid _rep = new ReportGrid();

	private AsReportViewer MyReport { get; set; }

	public bool Landscape { get; set; }

	public float PortraitWidth
	{
		get
		{
			bool landscape = _rep.Landscape;
			_rep.Landscape = false;
			float result = (float)_rep.PageWidth - _rep.Margins.Left - _rep.Margins.Right;
			_rep.Landscape = landscape;
			return result;
		}
	}

	public float LandscapeWidth
	{
		get
		{
			bool landscape = _rep.Landscape;
			_rep.Landscape = true;
			float result = (float)_rep.PageWidth - _rep.Margins.Left - _rep.Margins.Right;
			_rep.Landscape = landscape;
			return result;
		}
	}

	public AsReportPrintPreview(AsReportViewer sourceReport)
	{
		_rep = new ReportGrid();
		MyReport = sourceReport;
	}

	public ReportGrid CreateReport(bool showReport, bool isPreview = true, bool showDocumentMap = true)
	{
		XRTable xRTable = new XRTable();
		DocumentPreviewWindow documentPreviewWindow = new DocumentPreviewWindow
		{
			Title = Settings.ParametersService.CompanyName() + " " + MyReport.Caption.ToString()
		};
		_rep = new ReportGrid();
		PrintInfo printInfo = new PrintInfo(MyReport.ReportName);
		_rep.PaperKind = DXPaperKind.A4;
		_rep.Margins.Left = (int)(printInfo.MarginLeft * 100f);
		_rep.Margins.Right = (int)(printInfo.MarginRight * 100f);
		_rep.TopMargin.HeightF = (int)(printInfo.MarginTop * 100f);
		_rep.BottomMargin.HeightF = (int)(printInfo.MarginBottom * 100f);
		_rep.PaperKind = DXPaperKind.A4;
		double num = 8.0;
		bool showLines = printInfo.ShowLines;
		if (printInfo.PaperOrientation == "Landscape")
		{
			_rep.Landscape = true;
		}
		else
		{
			_rep.Landscape = false;
		}
		float widthF;
		if (isPreview)
		{
			widthF = (float)_rep.PageWidth - _rep.Margins.Left - _rep.Margins.Right;
		}
		else
		{
			_rep.PaperKind = DXPaperKind.Custom;
			_rep.PageHeight = 20000;
			widthF = 10000f;
		}
		XRTable xRTable2 = new XRTable
		{
			Padding = new PaddingInfo(3f, 3f, 1f, 1f)
		};
		xRTable2.BeginInit();
		xRTable2.WidthF = widthF;
		string[] array = MyReport.ReportTitle().Split(Environment.NewLine);
		string source = Settings.FontFamily.Source;
		double fontSize = Settings.GetFontSize();
		string printHeader = printInfo.PrintHeader;
		bool flag = ((printHeader == "Always" || (printHeader == "PrintIfOpen" && MyReport.DetailsPanel.IsVisible)) ? true : false);
		if (flag)
		{
			for (int i = 0; i <= array.Length; i++)
			{
				XRTableRow xRTableRow = new XRTableRow();
				printHeader = array[i].ToString();
				XRTableCell xRTableCell = new XRTableCell
				{
					Text = printHeader
				};
				System.Drawing.FontStyle style = System.Drawing.FontStyle.Bold;
				Font font = ((i != 0) ? new Font(source, 8f, style) : new Font(source, 11f, style));
				if (i == 1)
				{
					xRTableCell.Padding = new PaddingInfo(3f, 3f, 5f, 1f);
				}
				xRTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
				xRTableCell.Font = font;
				xRTableRow.Cells.Add(xRTableCell);
				xRTable2.Rows.Add(xRTableRow);
				if (i == 0 && !flag)
				{
					break;
				}
			}
			xRTable2.EndInit();
			_rep.ReportHeader.Controls.AddRange(new XRControl[1] { xRTable2 });
		}
		if (showDocumentMap)
		{
			_rep.Bookmark = (string)MyReport.Caption;
		}
		widthF = (float)_rep.PageWidth - _rep.Margins.Left - _rep.Margins.Right;
		DetailReportBand detailReportBand = new DetailReportBand();
		_rep.Bands.Add(detailReportBand);
		bool flag2 = isPreview && printInfo.FitToPage;
		double num2 = MyReport.PageWidth;
		if (num2 <= 0.0 || double.IsNaN(num2))
		{
			num2 = widthF;
		}
		double num3;
		if (!isPreview)
		{
			num3 = 1.0;
		}
		else if (flag2)
		{
			num3 = widthF / (float)num2;
			num3 = ((!(num3 >= 1.0)) ? 0.75 : 0.9);
		}
		else
		{
			num3 = 0.9;
		}
		float widthF2;
		if ((num2 > (double)widthF) & flag2)
		{
			xRTable.WidthF = widthF;
			widthF2 = widthF;
		}
		else
		{
			xRTable.WidthF = (float)(num2 * num3);
			widthF2 = xRTable.WidthF;
		}
		xRTable.Padding = new PaddingInfo(3f, 3f, 1f, 1f);
		Dictionary<int, string> dictionary = new Dictionary<int, string>();
		xRTable.BeginInit();
		bool flag3 = false;
		XRTable xRTable3 = new XRTable
		{
			WidthF = xRTable.WidthF
		};
		xRTable3.BeginInit();
		for (int j = 0; j <= 2; j++)
		{
			List<AsRepRow> list;
			switch (j)
			{
			case 0:
				list = MyReport.headerRows;
				break;
			case 1:
				if (MyReport.SelectedRows.Count > 0 && MyReport.SelectedRows.Count < MyReport.RepRows.Count)
				{
					list = new List<AsRepRow>();
					foreach (int key in MyReport.SelectedRows.Keys)
					{
						list.Add(MyReport.RepRows[key]);
						if (MyReport.pageBreaks.ContainsKey(key))
						{
							dictionary.Add(list.Count - 1, "");
						}
					}
					flag3 = true;
				}
				else
				{
					list = MyReport.RepRows;
					dictionary = MyReport.pageBreaks;
				}
				break;
			default:
				list = MyReport.footerRows;
				break;
			}
			if (list == null)
			{
				continue;
			}
			for (int k = 0; k <= list.Count - 1; k++)
			{
				XRTableRow xRTableRow2 = new XRTableRow
				{
					WidthF = xRTable.WidthF
				};
				AsRepRow asRepRow = list[k];
				XRTableRow xRTableRow = new XRTableRow
				{
					WidthF = xRTable.WidthF
				};
				XRTableCell xRTableCell;
				for (int l = 0; l <= asRepRow.RowCells.Count - 1; l++)
				{
					xRTableCell = new XRTableCell
					{
						Text = asRepRow.RowCells[l].ValueToString
					};
					if (!string.IsNullOrWhiteSpace(asRepRow.RowCells[l].XlsxFormatString))
					{
						xRTableCell.XlsxFormatString = asRepRow.RowCells[l].XlsxFormatString;
					}
					else
					{
						switch (asRepRow.RowCells[l].ValueStyle)
						{
						case AsRepColStyle.TextStyle:
							if (xRTableCell.Text.StartsWith('0'))
							{
								xRTableCell.XlsxFormatString = new string('0', xRTableCell.Text.Trim().Length);
							}
							else
							{
								xRTableCell.XlsxFormatString = "@";
							}
							break;
						case AsRepColStyle.CurrencyStyle:
						case AsRepColStyle.NumericStyle:
							xRTableCell.XlsxFormatString = GetExcelCurrencyFormat(xRTableCell.Text);
							break;
						case AsRepColStyle.DateStyle:
							if (xRTableCell.Text.Length == 10)
							{
								xRTableCell.XlsxFormatString = "dd/mm/yyyy";
							}
							else
							{
								xRTableCell.XlsxFormatString = "dd/mm/yy";
							}
							break;
						default:
							xRTableCell.XlsxFormatString = "General";
							break;
						}
					}
					System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
					if (asRepRow.RowCells[l].FontWeight == FontWeights.Bold)
					{
						style = System.Drawing.FontStyle.Bold;
					}
					if (asRepRow.RowCells[l].FontStyle == FontStyles.Italic)
					{
						style |= System.Drawing.FontStyle.Italic;
					}
					double num4 = ((!((double)asRepRow.RowCells[l].FontSize >= fontSize)) ? ((double)asRepRow.RowCells[l].FontSize) : (num + ((double)asRepRow.RowCells[l].FontSize - fontSize)));
					if (!isPreview && num4 < 9.0)
					{
						num4 = 9.0;
					}
					Font font = new Font(source, (float)num4, style);
					xRTableCell.Font = font;
					xRTableCell.Padding = new PaddingInfo(3f, 3f, 1f, 1f);
					if (!flag2)
					{
						xRTableCell.WidthF = (float)(asRepRow.RowCells[l].WidthD * num3);
					}
					else
					{
						xRTableCell.Weight = (float)asRepRow.RowCells[l].WidthD;
					}
					if (asRepRow.IsTableCaption)
					{
						if (showLines)
						{
							xRTableCell.BackColor = System.Drawing.Color.LightBlue;
							if (l == asRepRow.RowCells.Count - 1)
							{
								xRTableCell.Borders = BorderSide.All;
							}
							else
							{
								xRTableCell.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Bottom;
							}
							xRTableCell.BorderColor = System.Drawing.Color.LightSteelBlue;
						}
						else
						{
							xRTableCell.BackColor = System.Drawing.Color.LightBlue;
							xRTableCell.Borders = BorderSide.All;
							xRTableCell.BorderColor = System.Drawing.Color.White;
						}
					}
					else if (showLines)
					{
						if (k == 0 || dictionary.ContainsKey(k - 1))
						{
							xRTableCell.Borders = BorderSide.Left | BorderSide.Top | BorderSide.Bottom;
							if (l == asRepRow.RowCells.Count - 1)
							{
								xRTableCell.Borders = BorderSide.All;
							}
						}
						else if (l == asRepRow.RowCells.Count - 1)
						{
							xRTableCell.Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
						}
						else
						{
							xRTableCell.Borders = BorderSide.Left | BorderSide.Bottom;
						}
						xRTableCell.BorderColor = System.Drawing.Color.LightSteelBlue;
					}
					else if (asRepRow.RowCells[l].Borders != null && asRepRow.RowCells[l].Borders.Count > 0)
					{
						if (asRepRow.RowCells[l].Borders.Contains(BorderType.AllBorders))
						{
							xRTableCell.Borders = BorderSide.All;
						}
						else
						{
							if (asRepRow.RowCells[l].Borders.Contains(BorderType.LeftBorder))
							{
								xRTableCell.Borders |= BorderSide.Left;
							}
							if (asRepRow.RowCells[l].Borders.Contains(BorderType.RightBorder))
							{
								xRTableCell.Borders |= BorderSide.Right;
							}
							if (asRepRow.RowCells[l].Borders.Contains(BorderType.TopBorder))
							{
								xRTableCell.Borders |= BorderSide.Top;
							}
							if (asRepRow.RowCells[l].Borders.Contains(BorderType.BottomBorder))
							{
								xRTableCell.Borders |= BorderSide.Bottom;
							}
						}
						xRTableCell.BorderColor = System.Drawing.Color.Black;
					}
					else
					{
						xRTableCell.BorderColor = System.Drawing.Color.White;
						xRTableCell.Borders = BorderSide.All;
					}
					xRTableCell.BorderWidth = 1f;
					switch (asRepRow.RowCells[l].Alignment)
					{
					case System.Windows.TextAlignment.Center:
						xRTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
						break;
					case System.Windows.TextAlignment.Left:
						xRTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
						break;
					case System.Windows.TextAlignment.Right:
						xRTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
						break;
					default:
						xRTableCell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
						break;
					}
					System.Windows.Media.Color backGround = asRepRow.RowCells[l].BackGround;
					if (!asRepRow.IsTableCaption)
					{
						xRTableCell.BackColor = System.Drawing.Color.FromArgb(backGround.A, backGround.R, backGround.G, backGround.B);
					}
					xRTableRow.Cells.Add(xRTableCell);
				}
				xRTable3.Rows.Add(xRTableRow);
				if (!(dictionary.ContainsKey(k) & (j == 1)))
				{
					continue;
				}
				string text = dictionary[k];
				if (!printInfo.EnablePageBreaks)
				{
					xRTableRow = new XRTableRow
					{
						WidthF = xRTable3.WidthF,
						Borders = BorderSide.Top,
						BorderColor = System.Drawing.Color.Black,
						BorderDashStyle = BorderDashStyle.Dot
					};
					XRTableRow lastRow = xRTable3.Rows.LastRow;
					int m = 0;
					for (int num5 = lastRow.Cells.Count - 1; m <= num5; m++)
					{
						xRTableCell = new XRTableCell
						{
							Padding = new PaddingInfo(0f, 0f, 10f, 10f),
							HeightF = 20f
						};
						if (flag2)
						{
							xRTableCell.Weight = lastRow.Cells[m].Weight;
						}
						else
						{
							xRTableCell.WidthF = lastRow.Cells[m].WidthF;
						}
						xRTableCell.Text = "#";
						xRTableCell.ForeColor = xRTableRow.BackColor;
						xRTableRow.Cells.Add(xRTableCell);
					}
					xRTable3.Rows.Add(xRTableRow);
				}
				xRTable3.EndInit();
				if (xRTable3.Rows.Count > 30 || printInfo.EnablePageBreaks)
				{
					xRTable.EndInit();
					if (detailReportBand.Bands.Count == 0)
					{
						detailReportBand.Bands.Add(new DetailBand());
					}
					Band band = detailReportBand.Bands[0];
					band.Controls.Add(xRTable);
					XRPageBreak child = new XRPageBreak
					{
						TopF = xRTable.BottomF + 1f
					};
					band.Controls.Add(child);
					xRTable = new XRTable
					{
						WidthF = widthF2
					};
					xRTable.BeginInit();
				}
				detailReportBand = new DetailReportBand();
				_rep.Bands.Add(detailReportBand);
				xRTableCell = new XRTableCell
				{
					WidthF = xRTable.WidthF
				};
				if (!string.IsNullOrEmpty(text) && xRTable3.Rows.Count > 0 && showDocumentMap)
				{
					xRTableCell.Bookmark = text;
				}
				xRTableCell.Controls.Add(xRTable3);
				xRTableRow2.Cells.Add(xRTableCell);
				xRTable.Rows.Add(xRTableRow2);
				xRTable3 = new XRTable
				{
					WidthF = xRTable.WidthF
				};
				xRTable3.BeginInit();
			}
		}
		if (xRTable.Rows.Count > 0 || xRTable3.Rows.Count > 0)
		{
			xRTable3.EndInit();
			XRTableRow xRTableRow3 = new XRTableRow();
			XRTableCell xRTableCell = new XRTableCell
			{
				WidthF = xRTable.WidthF
			};
			xRTableCell.Controls.Add(xRTable3);
			xRTableRow3.Cells.Add(xRTableCell);
			xRTable.Rows.Add(xRTableRow3);
			xRTable.EndInit();
			DetailBand detailBand = new DetailBand();
			if (detailReportBand.Bands.Count > 0)
			{
				detailReportBand = new DetailReportBand();
			}
			detailReportBand.Bands.Add(detailBand);
			detailBand.Controls.Add(xRTable);
			_rep.Bands.Add(detailReportBand);
		}
		if (flag3)
		{
			XRLabel xRLabel = new XRLabel
			{
				Text = Resources.PrintSelectedRows,
				Size = new Size((int)Math.Round(widthF), 23)
			};
			_rep.PageHeader.Controls.Clear();
			_rep.PageHeader.Controls.Add(xRLabel);
			xRLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
		}
		XRPageInfo xRPageInfo = new XRPageInfo();
		XRPageInfo xRPageInfo2 = new XRPageInfo();
		_rep.PageFooter.Controls.Clear();
		xRPageInfo2.Size = new Size(313, 23);
		if (printInfo.PrintDate)
		{
			_rep.PageFooter.Controls.AddRange(new XRControl[1] { xRPageInfo2 });
			xRPageInfo2.Name = "xrPageInfo1";
			xRPageInfo2.Padding = new PaddingInfo(2f, 2f, 0f, 0f, 100f);
			xRPageInfo2.PageInfo = PageInfo.DateTime;
			xRPageInfo2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
		}
		_rep.PageFooter.Name = "PageFooter";
		if (printInfo.PrintPageNumbers)
		{
			_rep.PageFooter.Controls.AddRange(new XRControl[1] { xRPageInfo });
			xRPageInfo.Format = "{0} / {1} ";
			xRPageInfo.Name = "xrPageInfo2";
			xRPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xRPageInfo.LeftF = xRPageInfo2.WidthF;
			xRPageInfo.WidthF = widthF - xRPageInfo2.WidthF;
		}
		if (printInfo.PrintDate || printInfo.PrintPageNumbers)
		{
			_rep.PageFooter.Height = 29;
		}
		else
		{
			_rep.PageFooter.Height = 0;
		}
		if (MyReport.ExportName != "")
		{
			_rep.Name = MyReport.ExportName;
		}
		if (showDocumentMap)
		{
			_rep.Bookmark = (string)MyReport.Caption;
		}
		_rep.CreateDocument();
		if (showDocumentMap)
		{
			_rep.AdjustBookmarks();
		}
		if (showReport)
		{
			documentPreviewWindow.PreviewControl.DocumentSource = _rep;
			documentPreviewWindow.Show();
		}
		return _rep;
	}

	private static string GetExcelCurrencyFormat(string sValue)
	{
		int num = sValue.IndexOf('.');
		bool flag = sValue.IndexOf(',') > 0;
		int num2 = ((num != -1) ? sValue.Substring(num + 1).Length : 0);
		if (!decimal.TryParse(sValue, out var _))
		{
			return "General";
		}
		if (flag)
		{
			return num2 switch
			{
				0 => "#,##0_);[Red](#,##0)", 
				1 => "#,##0.0_);[Red](#,##0.0)", 
				3 => "#,##0.000_);[Red](#,##0.000)", 
				4 => "#,##0.0000_);[Red](#,##0.0000)", 
				_ => "#,##0.00_);[Red](#,##0.00)", 
			};
		}
		return num2 switch
		{
			0 => "###0_);[Red](###0)", 
			1 => "###0.0_);[Red](###0.0)", 
			3 => "###0.000_);[Red](###0.000)", 
			4 => "###0.0000_);[Red](###0.0000)", 
			_ => "###0.00_);[Red](###0.00)", 
		};
	}
}
