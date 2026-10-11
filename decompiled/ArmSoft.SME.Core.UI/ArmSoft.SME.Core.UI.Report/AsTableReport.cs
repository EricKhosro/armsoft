using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models.Data;
using ArmSoft.SME.Common.Parameters;
using ArmSoft.SME.Core.UI.DataViews;
using DevExpress.Data;
using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraGrid;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;

namespace ArmSoft.SME.Core.UI.Report;

public class AsTableReport
{
	private ReportGrid _report;

	public string ExportName { get; set; }

	public ReportGrid CreateGroupedReport(DataViewPanel dvp, bool showReport, bool isPreview = true)
	{
		GridControl grid = dvp.InnerGrid();
		IPanelView panelView = (IPanelView)grid.View;
		bool flag = panelView is TreeListView;
		ObservableCollection<int> observableCollection = new ObservableCollection<int>();
		ObservableCollection<string> observableCollection2 = new ObservableCollection<string>();
		List<AsTableReportColumn> list = new List<AsTableReportColumn>();
		XRTable xRTable = new XRTable();
		float num = 0f;
		bool flag2 = true;
		int num2 = (isPreview ? 11 : 9);
		DXFontStyle dXFontStyle = DXFontStyle.Regular;
		int num3 = 0;
		DataTable dataTable = null;
		DataTable dataTable2 = null;
		XRTable xRTable2 = null;
		bool flag3 = false;
		DocumentPreviewWindow documentPreviewWindow = new DocumentPreviewWindow
		{
			Title = Settings.ParametersService.CompanyName() + " " + dvp.Caption.ToString()
		};
		_report = new ReportGrid
		{
			IsPreview = isPreview,
			PaperKind = DXPaperKind.A4,
			Name = ExportName
		};
		PrintInfo printInfo = new PrintInfo(ExportName);
		bool flag4 = printInfo.ShowLines | !isPreview;
		_report.Margins.Left = (int)(printInfo.MarginLeft * 100f);
		_report.Margins.Right = (int)(printInfo.MarginRight * 100f);
		if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			_report.TopMargin.HeightF = (int)(printInfo.MarginTop * 100f);
			_report.BottomMargin.HeightF = (int)(printInfo.MarginBottom * 100f);
		}
		if (printInfo.PaperOrientation == "Landscape")
		{
			_report.Landscape = true;
		}
		else
		{
			_report.Landscape = false;
		}
		float num4 = (float)_report.PageWidth - _report.Margins.Left - _report.Margins.Right;
		xRTable.Padding = new PaddingInfo(3f, 3f, 1f, 1f);
		xRTable.BeginInit();
		xRTable.WidthF = num4;
		double num5 = 0.65;
		string[] array = dvp.ReportTitle.Split("\r\n");
		dXFontStyle = DXFontStyle.Bold;
		bool flag5 = dvp.GetSelectedAny() && !dvp.InnerGrid().IsGrouped;
		string source = Settings.FontFamily.Source;
		string printHeader = printInfo.PrintHeader;
		bool flag6 = ((printHeader == "Always" || (printHeader == "PrintIfOpen" && dvp.InnerDetailsPanel().IsVisible)) ? true : false);
		if (string.IsNullOrEmpty(array[0]))
		{
			array[0] = dvp.Caption.ToString();
		}
		DXFont font;
		XRTableRow xRTableRow;
		for (int i = 0; i <= array.Length - 1; i++)
		{
			xRTableRow = new XRTableRow
			{
				WidthF = num4
			};
			printHeader = array[i].ToString();
			XRTableCell xRTableCell = new XRTableCell
			{
				Text = printHeader
			};
			dXFontStyle = DXFontStyle.Bold;
			if (i != 0)
			{
				font = new DXFont(source, isPreview ? 8f : ((float)num2), dXFontStyle);
			}
			else
			{
				font = (isPreview ? new DXFont(source, num2, dXFontStyle) : new DXFont(source, (float)((double)num2 * 1.4), dXFontStyle));
			}
			if (i == 1)
			{
				xRTableCell.Padding = new PaddingInfo(3f, 3f, 5f, 1f);
			}
			xRTableCell.TextAlignment = TextAlignment.TopLeft;
			xRTableCell.Font = font;
			xRTableRow.Cells.Add(xRTableCell);
			xRTable.Rows.Add(xRTableRow);
			if (i == 0 && !flag6)
			{
				break;
			}
		}
		if (grid.FilterString != "" && printInfo.PrintFilterCondition && grid.IsFilterEnabled)
		{
			xRTableRow = new XRTableRow
			{
				WidthF = num4
			};
			printHeader = "   " + Resources.FilterCondition + "  - " + Environment.NewLine + grid.FilterString;
			printHeader = printHeader.Replace("True", Resources.Yes);
			printHeader = printHeader.Replace("False", Resources.No);
			printHeader = printHeader.Replace("#", "");
			printHeader = printHeader.Replace("[" + grid.Columns[0].FieldName + "]", "[" + Resources.SelectedRows + "]");
			for (int i = 1; i <= grid.Columns.Count - 1; i++)
			{
				printHeader = printHeader.Replace("[" + grid.Columns[i].FieldName + "]", "[" + grid.Columns[i].HeaderCaption.ToString() + "]");
			}
			XRTableCell xRTableCell = new XRTableCell
			{
				Text = printHeader,
				Multiline = true
			};
			dXFontStyle = DXFontStyle.Bold;
			font = new DXFont(source, 8f, dXFontStyle);
			xRTableCell.Padding = new PaddingInfo(3f, 3f, 5f, 1f);
			xRTableCell.TextAlignment = TextAlignment.TopLeft;
			xRTableCell.Font = font;
			xRTableCell.WidthF = num4;
			xRTableRow.Cells.Add(xRTableCell);
			xRTableRow.Padding = xRTableCell.Padding;
			xRTable.Rows.Add(xRTableRow);
		}
		xRTable.EndInit();
		_report.RepHeader.Controls.AddRange(new XRControl[1] { xRTable });
		if (grid.VisibleRowCount == 0)
		{
			if (!string.IsNullOrEmpty(ExportName))
			{
				_report.Name = ExportName;
			}
			_report.CreateDocument();
			if (showReport)
			{
				documentPreviewWindow.PreviewControl.DocumentSource = _report;
				documentPreviewWindow.Show();
			}
			return _report;
		}
		for (int i = 0; i <= grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex = grid.GetRowHandleByVisibleIndex(i);
			if (rowHandleByVisibleIndex >= 0)
			{
				flag2 = false;
				break;
			}
			if (panelView is MyTableView myTableView && observableCollection.Count < myTableView.GroupedColumns.Count)
			{
				int rowLevelByRowHandle = grid.GetRowLevelByRowHandle(rowHandleByVisibleIndex);
				if (!observableCollection.Contains(rowLevelByRowHandle))
				{
					observableCollection.Add(rowLevelByRowHandle);
				}
			}
		}
		num = 0f;
		if (FirstColumnSummaryExist(panelView))
		{
			AsTableReportColumn item = new AsTableReportColumn
			{
				FieldName = "",
				Header = "",
				VisibleIndex = 0,
				WidthF = 150f,
				IsSummary = false,
				IsGroupColumn = false
			};
			list.Add(item);
		}
		if (panelView is MyTableView myTableView2)
		{
			for (int i = 0; i <= myTableView2.GroupedColumns.Count - 1; i++)
			{
				int groupLevel = myTableView2.GroupedColumns[i].GroupLevel;
				if (!observableCollection.Contains(groupLevel))
				{
					continue;
				}
				AsTableReportColumn asTableReportColumn = new AsTableReportColumn
				{
					FieldName = myTableView2.GroupedColumns[i].FieldName,
					Header = myTableView2.GroupedColumns[i].HeaderCaption.ToString()
				};
				observableCollection2.Add(asTableReportColumn.FieldName);
				asTableReportColumn.VisibleIndex = -1;
				for (int j = 0; j <= myTableView2.VisibleColumns.Count - 1; j++)
				{
					if (myTableView2.VisibleColumns[j].FieldName == asTableReportColumn.FieldName)
					{
						asTableReportColumn.VisibleIndex = j;
						asTableReportColumn.WidthF = (float)myTableView2.VisibleColumns[j].ActualDataWidth;
						break;
					}
				}
				for (int k = 0; k <= grid.Columns.Count - 1; k++)
				{
					if (grid.Columns[k].FieldName == asTableReportColumn.FieldName)
					{
						asTableReportColumn.GridIndex = k;
						break;
					}
				}
				if (asTableReportColumn.WidthF == 0f)
				{
					asTableReportColumn.WidthF = 200f;
				}
				asTableReportColumn.IsSummary = false;
				asTableReportColumn.IsGroupColumn = true;
				list.Add(asTableReportColumn);
			}
		}
		num3 = ((!flag) ? 1 : 0);
		List<ColumnBase> list2 = panelView.GetVisibleColumns().ToList();
		for (int i = num3; i <= list2.Count - 1; i++)
		{
			if (observableCollection2.Contains(list2[i].FieldName) || !(list2[i].FieldName != "Selected"))
			{
				continue;
			}
			AsTableReportColumn asTableReportColumn2 = new AsTableReportColumn
			{
				FieldName = list2[i].FieldName,
				Header = list2[i].HeaderCaption.ToString()
			};
			observableCollection2.Add(list2[i].FieldName);
			asTableReportColumn2.WidthF = (float)list2[i].ActualDataWidth;
			asTableReportColumn2.VisibleIndex = i;
			asTableReportColumn2.IsSummary = false;
			for (int l = 0; l <= grid.GroupSummary.Count - 1; l++)
			{
				if (grid.GroupSummary[l].FieldName == list2[i].FieldName && grid.GroupSummary[l].Visible)
				{
					asTableReportColumn2.IsSummary = true;
					break;
				}
			}
			for (int m = 0; m <= grid.Columns.Count - 1; m++)
			{
				if (grid.Columns[m].FieldName == asTableReportColumn2.FieldName)
				{
					asTableReportColumn2.GridIndex = m;
					break;
				}
			}
			asTableReportColumn2.IsGroupColumn = false;
			if ((!flag2 || asTableReportColumn2.IsSummary) && dvp.GetColumnType(list2[i].Name) != DataColumnType.ButtonToolTip)
			{
				list.Add(asTableReportColumn2);
			}
		}
		xRTableRow = new XRTableRow
		{
			BackColor = Color.LightBlue,
			Borders = BorderSide.All
		};
		if (flag4)
		{
			xRTableRow.BorderColor = Color.LightSteelBlue;
		}
		else
		{
			xRTableRow.BorderColor = Color.White;
		}
		float num6 = 0f;
		int num7 = 0;
		int num8 = -1;
		num = 0f;
		if ((grid.GroupSummary.Count > 0) & (list[0].WidthF < 70f))
		{
			list[0].WidthF = 70f;
		}
		for (int i = 0; i <= list.Count - 1; i++)
		{
			AsTableReportColumn asTableReportColumn3 = list[i];
			if (asTableReportColumn3.WidthF > num4)
			{
				asTableReportColumn3.WidthF = num4;
			}
			if ((num6 < num4) & (num6 + asTableReportColumn3.WidthF > num4))
			{
				float num9 = (num4 - num6) % (float)(num8 - num7 + 1);
				int num10 = (int)Math.Round((num4 - num6 - num9) / (float)(num8 - num7 + 1));
				for (int n = num7; n <= num8; n++)
				{
					list[n].WidthF = list[n].WidthF + (float)num10;
					num += (float)num10;
				}
				list[i - 1].WidthF += num9;
				num += num9;
				num6 = 0f;
				num7 = num8 + 1;
			}
			else if (num6 == num4)
			{
				num6 = 0f;
			}
			num8++;
			num6 += list[i].WidthF;
			num += list[i].WidthF;
			BaseEditSettings editSettings = grid.Columns[asTableReportColumn3.GridIndex].EditSettings;
			if (editSettings != null)
			{
				asTableReportColumn3.DisplayFormat = editSettings.DisplayFormat;
				switch (editSettings.HorizontalContentAlignment)
				{
				case EditSettingsHorizontalAlignment.Left:
					asTableReportColumn3.ContentAlignment = TextAlignment.TopLeft;
					break;
				case EditSettingsHorizontalAlignment.Right:
					asTableReportColumn3.ContentAlignment = TextAlignment.TopRight;
					break;
				case EditSettingsHorizontalAlignment.Center:
					asTableReportColumn3.ContentAlignment = TextAlignment.TopCenter;
					break;
				case EditSettingsHorizontalAlignment.Stretch:
					asTableReportColumn3.ContentAlignment = TextAlignment.TopJustify;
					break;
				default:
					asTableReportColumn3.ContentAlignment = TextAlignment.TopLeft;
					break;
				}
			}
			else
			{
				MyGridColumn myGridColumn = (MyGridColumn)grid.Columns[asTableReportColumn3.GridIndex];
				if (myGridColumn.IsBooleanBased)
				{
					list[i].BooleanBased = true;
					asTableReportColumn3.ContentAlignment = TextAlignment.TopCenter;
				}
				else if (dvp.GetColumnType(myGridColumn.Name) == DataColumnType.ButtonToolTip)
				{
					list[i].IsImage = true;
					asTableReportColumn3.ContentAlignment = TextAlignment.MiddleCenter;
				}
				else
				{
					asTableReportColumn3.ContentAlignment = TextAlignment.TopLeft;
				}
			}
		}
		xRTable = new XRTable();
		xRTable.BeginInit();
		XRTable xRTable3 = new XRTable();
		bool flag7;
		if ((num > num4 && printInfo.FitToPage) & isPreview)
		{
			flag7 = true;
			num = num4;
		}
		else
		{
			flag7 = false;
		}
		xRTable3.WidthF = num;
		xRTable.WidthF = num;
		dataTable = CreateDataTable(list, dvp);
		for (int i = 0; i <= list.Count - 1; i++)
		{
			XRTableCell xRTableCell = new XRTableCell
			{
				Text = list[i].Header,
				WordWrap = true
			};
			if (flag7)
			{
				xRTableCell.Weight = list[i].WidthF;
			}
			else
			{
				xRTableCell.WidthF = list[i].WidthF;
			}
			dXFontStyle = DXFontStyle.Regular;
			dXFontStyle = DXFontStyle.Bold;
			font = (isPreview ? new DXFont(source, (float)((double)num2 * 0.7), dXFontStyle) : new DXFont(source, num2, dXFontStyle));
			xRTableCell.TextAlignment = list[i].ContentAlignment;
			xRTableCell.Font = font;
			xRTableRow.Cells.Add(xRTableCell);
		}
		xRTableRow.Padding = new PaddingInfo(3f, 3f, 5f, 5f);
		xRTable.Rows.Add(xRTableRow);
		xRTable.EndInit();
		DetailReportBand detailReportBand = new DetailReportBand();
		DetailBand detailBand = new DetailBand();
		detailReportBand.Bands.Add(detailBand);
		detailBand.Controls.Add(xRTable);
		detailBand.HeightF = xRTable.HeightF;
		_report.Bands.Add(detailReportBand);
		DetailReportBand detailReportBand2;
		for (int i = 0; i <= grid.VisibleRowCount - 1; i++)
		{
			int rowHandleByVisibleIndex2 = grid.GetRowHandleByVisibleIndex(i);
			if (flag5 && !(bool)grid.GetCellValue(rowHandleByVisibleIndex2, "Selected"))
			{
				continue;
			}
			xRTableRow = new XRTableRow();
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
				if (isPreview)
				{
					xRTableRow.Borders = BorderSide.Left | BorderSide.Right | BorderSide.Bottom;
				}
				else
				{
					xRTableRow.Borders = BorderSide.All;
				}
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
				xRTableRow.Borders = BorderSide.All;
			}
			xRTableRow.Borders = BorderSide.All;
			if (grid.IsGroupRowHandle(rowHandleByVisibleIndex2))
			{
				if (!flag3)
				{
					if (xRTable3 != null && xRTable3.Rows.Count > 0)
					{
						xRTable3.EndInit();
						detailReportBand2 = new DetailReportBand
						{
							DataMember = "",
							DataSource = dataTable
						};
						detailBand = new DetailBand();
						detailReportBand2.Bands.Add(detailBand);
						detailBand.Controls.Add(xRTable3);
						detailBand.HeightF = xRTable3.HeightF;
						_report.Bands.Add(detailReportBand2);
					}
					xRTable2 = new XRTable
					{
						WidthF = num
					};
					if ((xRTable2.WidthF > num4 && printInfo.FitToPage) & isPreview)
					{
						flag7 = true;
						xRTable2.WidthF = num4;
					}
					else
					{
						flag7 = false;
					}
					xRTable2.Tag = grid.GetRowLevelByRowHandle(rowHandleByVisibleIndex2).ToString();
					xRTable2.BeginInit();
					flag3 = true;
				}
				else if (xRTable2.Tag.ToString() != grid.GetRowLevelByRowHandle(rowHandleByVisibleIndex2).ToString())
				{
					xRTable2.EndInit();
					detailReportBand2 = new DetailReportBand();
					detailBand = new DetailBand();
					detailReportBand2.Bands.Add(detailBand);
					detailReportBand2.DataMember = "";
					detailReportBand2.DataSource = dataTable2;
					detailBand.Controls.Add(xRTable2);
					detailBand.HeightF = xRTable2.HeightF;
					_report.Bands.Add(detailReportBand2);
					xRTable2 = new XRTable
					{
						WidthF = num
					};
					if ((xRTable2.WidthF > num4 && printInfo.FitToPage) & isPreview)
					{
						flag7 = true;
						xRTable2.WidthF = num4;
					}
					else
					{
						flag7 = false;
					}
					xRTable2.BeginInit();
					xRTable2.Tag = grid.GetRowLevelByRowHandle(rowHandleByVisibleIndex2).ToString();
					flag3 = true;
				}
				Dictionary<int, Type> dictionary = new Dictionary<int, Type>();
				if (xRTable2.Rows.Count == 0)
				{
					xRTableRow = new XRTableRow
					{
						Borders = BorderSide.All
					};
					if (flag4)
					{
						xRTableRow.BorderColor = Color.LightSteelBlue;
					}
					else
					{
						xRTableRow.BorderColor = Color.White;
					}
					for (int num11 = 0; num11 <= list.Count - 1; num11++)
					{
						XRTableCell xRTableCell = new XRTableCell();
						if (flag2)
						{
							xRTableCell.BackColor = Color.White;
						}
						else
						{
							xRTableCell.BackColor = Color.LightGray;
						}
						if (!list[num11].IsSummary)
						{
							if (panelView is MyTableView myTableView3)
							{
								myTableView3.MyGetGroupRowDisplayText(rowHandleByVisibleIndex2);
								string[] array2 = myTableView3.MyGetGroupRowCaptions(rowHandleByVisibleIndex2);
								for (int num12 = 0; num12 <= array2.Length - 1; num12++)
								{
									if (!(array2[num12] == list[num11].Header))
									{
										continue;
									}
									if (grid.Columns[list[num11].GridIndex].UnboundType == UnboundColumnType.Decimal)
									{
										if (!dictionary.ContainsKey(num11))
										{
											dictionary.Add(num11, typeof(decimal));
										}
									}
									else if (Nullable.GetUnderlyingType(grid.Columns[list[num11].GridIndex].FieldType) == null)
									{
										if (!dictionary.ContainsKey(num11))
										{
											dictionary.Add(num11, grid.Columns[list[num11].GridIndex].FieldType);
										}
									}
									else if (!dictionary.ContainsKey(num11))
									{
										dictionary.Add(num11, Nullable.GetUnderlyingType(grid.Columns[list[num11].GridIndex].FieldType));
									}
									break;
								}
							}
							else if (list[num11].BooleanBased)
							{
								xRTableCell.BeforePrint += new SafeEventHandler<CancelEventArgs>(BooleanToString).Handler;
							}
						}
						else
						{
							for (int num13 = 0; num13 <= grid.GroupSummary.Count - 1; num13++)
							{
								if (!grid.GroupSummary[num13].Visible)
								{
									continue;
								}
								GridSummaryItem gridSummaryItem = grid.GroupSummary[num13];
								xRTableCell.Multiline = true;
								xRTableCell.TextAlignment = list[num11].ContentAlignment;
								if (gridSummaryItem.FieldName == list[num11].FieldName)
								{
									dictionary.Remove(num11);
									if (gridSummaryItem.SummaryType == SummaryItemType.Sum)
									{
										dictionary.Add(num11, typeof(decimal));
									}
									else
									{
										dictionary.Add(num11, typeof(string));
									}
									switch (gridSummaryItem.SummaryType)
									{
									case SummaryItemType.Sum:
										xRTableCell.XlsxFormatString = DecimalExtension.GetCurrencyFormat(16, 2);
										break;
									case SummaryItemType.Average:
										_ = Resources.GridSummaryAvg + ": ";
										xRTableCell.XlsxFormatString = DecimalExtension.GetCurrencyFormat(16, 2);
										break;
									case SummaryItemType.Max:
										_ = Resources.GridSummaryMax + ": ";
										xRTableCell.XlsxFormatString = DecimalExtension.GetCurrencyFormat(16, 2);
										break;
									case SummaryItemType.Min:
										_ = Resources.GridSummaryMin + ": ";
										xRTableCell.XlsxFormatString = DecimalExtension.GetCurrencyFormat(16, 2);
										break;
									case SummaryItemType.Count:
										_ = Resources.GridSummaryCount + ": ";
										xRTableCell.XlsxFormatString = DecimalExtension.GetCurrencyFormat(16, 2);
										break;
									}
								}
							}
						}
						if (list[num11].WidthF > 0f)
						{
							if (flag7)
							{
								xRTableCell.Weight = list[num11].WidthF;
							}
							else
							{
								xRTableCell.WidthF = list[num11].WidthF;
							}
						}
						dXFontStyle = DXFontStyle.Regular;
						dXFontStyle = ((!flag2) ? DXFontStyle.Bold : DXFontStyle.Regular);
						font = (isPreview ? new DXFont(source, (float)((double)num2 * 0.7), dXFontStyle) : new DXFont(source, num2, dXFontStyle));
						xRTableCell.Font = font;
						XRBinding xRBinding = new XRBinding("Text", null, list[num11].FieldName);
						if (!string.IsNullOrEmpty(list[num11].DisplayFormat))
						{
							xRBinding.FormatString = list[num11].DisplayFormat;
						}
						xRTableCell.DataBindings.Add(xRBinding);
						xRTableCell.XlsxFormatString = list[num11].DisplayFormat?.Replace("{0:", "");
						xRTableCell.XlsxFormatString = xRTableCell.XlsxFormatString?.Replace("}", "");
						xRTableRow.Cells.Add(xRTableCell);
					}
					xRTable2.Rows.Add(xRTableRow);
					dataTable2 = CreateGroupDataTable(list, dvp, dictionary);
				}
				object[] groupRow = GetGroupRow(rowHandleByVisibleIndex2, list, dvp, source, isPreview ? ((float)((double)num2 * num5)) : ((float)num2));
				dataTable2.Rows.Add(groupRow);
				continue;
			}
			if (flag3)
			{
				flag3 = false;
				xRTable2.EndInit();
				detailReportBand2 = new DetailReportBand();
				detailBand = new DetailBand();
				detailReportBand2.Bands.Add(detailBand);
				detailReportBand2.DataMember = "";
				detailReportBand2.DataSource = dataTable2;
				detailBand.Controls.Add(xRTable2);
				detailBand.HeightF = xRTable2.HeightF;
				_report.Bands.Add(detailReportBand2);
				dataTable = CreateDataTable(list, dvp);
				xRTable3 = new XRTable
				{
					WidthF = num,
					Padding = new PaddingInfo(3f, 3f, 1f, 1f)
				};
				if ((xRTable3.WidthF > num4 && printInfo.FitToPage) & isPreview)
				{
					flag7 = true;
					xRTable3.WidthF = num4;
				}
				else
				{
					flag7 = false;
				}
				xRTable3.BeginInit();
			}
			object[] tableRow = GetTableRow(rowHandleByVisibleIndex2, list, dvp, source, isPreview ? ((float)((double)num2 * num5)) : ((float)num2));
			if ((!flag5 || !(bool)grid.GetCellValue(rowHandleByVisibleIndex2, "Selected")) && flag5)
			{
				continue;
			}
			dataTable.Rows.Add(tableRow);
			if (dataTable.Rows.Count != 1)
			{
				continue;
			}
			for (int num14 = 0; num14 <= list.Count - 1; num14++)
			{
				XRTableCell xRTableCell = new XRTableCell();
				if ((num14 == 0) & flag)
				{
					XRBinding binding = new XRBinding("Padding", null, "Level");
					xRTableCell.DataBindings.Add(binding);
				}
				XRBinding binding2 = new XRBinding("Font", null, "Font");
				xRTableCell.DataBindings.Add(binding2);
				XRBinding xRBinding2 = new XRBinding("Text", null, list[num14].FieldName);
				if (!string.IsNullOrWhiteSpace(list[num14].DisplayFormat))
				{
					xRBinding2.FormatString = list[num14].DisplayFormat;
				}
				xRTableCell.DataBindings.Add(xRBinding2);
				xRTableCell.XlsxFormatString = list[num14].DisplayFormat?.Replace("{0:", "");
				xRTableCell.XlsxFormatString = xRTableCell.XlsxFormatString?.Replace("}", "");
				if (list[num14].BooleanBased)
				{
					xRTableCell.BeforePrint += new SafeEventHandler<CancelEventArgs>(BooleanToString).Handler;
				}
				xRTableCell.TextAlignment = list[num14].ContentAlignment;
				if (list[num14].WidthF > 0f)
				{
					if (flag7)
					{
						xRTableCell.Weight = list[num14].WidthF;
					}
					else
					{
						xRTableCell.WidthF = list[num14].WidthF;
					}
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (!flag3)
		{
			xRTable3.EndInit();
			detailReportBand2 = new DetailReportBand();
			detailBand = new DetailBand();
			detailReportBand2.Bands.Add(detailBand);
			detailReportBand2.DataMember = "";
			detailReportBand2.DataSource = dataTable;
			detailBand.Controls.Add(xRTable3);
			detailBand.HeightF = xRTable3.HeightF;
			_report.Bands.Add(detailReportBand2);
		}
		else
		{
			xRTable2.EndInit();
			detailReportBand2 = new DetailReportBand();
			detailBand = new DetailBand();
			detailReportBand2.Bands.Add(detailBand);
			detailReportBand2.DataMember = "";
			detailReportBand2.DataSource = dataTable2;
			detailBand.Controls.Add(xRTable2);
			detailBand.HeightF = xRTable2.HeightF;
			_report.Bands.Add(detailReportBand2);
		}
		xRTable3 = new XRTable
		{
			WidthF = num,
			Padding = new PaddingInfo(3f, 3f, 1f, 1f)
		};
		if ((xRTable3.WidthF > num4 && printInfo.FitToPage) & isPreview)
		{
			flag7 = true;
			xRTable3.WidthF = num4;
		}
		else
		{
			flag7 = false;
		}
		xRTable3.BeginInit();
		dXFontStyle = DXFontStyle.Regular;
		dXFontStyle = DXFontStyle.Bold;
		font = (isPreview ? new DXFont(source, (float)((double)num2 * num5), dXFontStyle) : new DXFont(source, num2, dXFontStyle));
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		bool flag12 = false;
		bool flag13 = false;
		int num15;
		for (num15 = 0; num15 <= grid.TotalSummary.Count - 1; num15++)
		{
			if (grid.TotalSummary[num15].Visible && panelView.GetVisibleColumns().Any((ColumnBase p) => p.FieldName == grid.TotalSummary[num15].FieldName))
			{
				if (grid.TotalSummary[num15].SummaryType == SummaryItemType.Sum)
				{
					flag8 = true;
				}
				else if (grid.TotalSummary[num15].SummaryType == SummaryItemType.Average && !flag5)
				{
					flag9 = true;
				}
				else if (grid.TotalSummary[num15].SummaryType == SummaryItemType.Count && !flag5)
				{
					flag12 = true;
				}
				else if (grid.TotalSummary[num15].SummaryType == SummaryItemType.Max && !flag5)
				{
					flag10 = true;
				}
				else if (grid.TotalSummary[num15].SummaryType == SummaryItemType.Min && !flag5)
				{
					flag11 = true;
				}
				else if ((grid.TotalSummary[num15].SummaryType == SummaryItemType.Custom) & flag5)
				{
					flag13 = true;
				}
			}
		}
		if (flag8)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 3f, 3f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Text = "",
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex = list[i].GridIndex;
				if (grid.Columns[gridIndex].HasTotalSummaries)
				{
					for (int num16 = 0; num16 <= grid.Columns[gridIndex].TotalSummaries.Count - 1; num16++)
					{
						if (grid.Columns[gridIndex].TotalSummaries[num16].Item.SummaryType != SummaryItemType.Sum || ((flag5 || grid.Columns[gridIndex].TotalSummaries[num16].Item.CalculationMode == GridSummaryCalculationMode.SelectedRows) && (!flag5 || grid.Columns[gridIndex].TotalSummaries[num16].Item.CalculationMode != GridSummaryCalculationMode.SelectedRows)))
						{
							continue;
						}
						if (xRTableCell.Text != "")
						{
							xRTableCell.Text += Environment.NewLine;
						}
						if (grid.Columns[gridIndex].TotalSummaries[num16].Column.FieldName != list[i].FieldName)
						{
							XRTableCell xRTableCell2 = xRTableCell;
							xRTableCell2.Text = xRTableCell2.Text + grid.Columns[gridIndex].TotalSummaries[num16].Column.HeaderCaption.ToString() + ":";
						}
						object value = grid.Columns[gridIndex].TotalSummaries[num16].Value;
						if (value != null)
						{
							if (grid.Columns[gridIndex].TotalSummaries[num16].Column.EditSettings != null && grid.Columns[gridIndex].TotalSummaries[num16].Column.EditSettings.DisplayFormat != "")
							{
								xRTableCell.Text += string.Format(grid.Columns[gridIndex].TotalSummaries[num16].Column.EditSettings.DisplayFormat, value);
								XRBinding binding3 = new XRBinding("Text", grid.Columns[gridIndex].TotalSummaries[num16], "Value")
								{
									FormatString = grid.Columns[gridIndex].TotalSummaries[num16].Column.EditSettings.DisplayFormat
								};
								xRTableCell.DataBindings.Add(binding3);
							}
							else
							{
								xRTableCell.Text += value.ToString();
							}
							xRTableCell.TextAlignment = list[i].ContentAlignment;
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.Total;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (flag9)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 3f, 3f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex2 = list[i].GridIndex;
				if (grid.Columns[gridIndex2].HasTotalSummaries)
				{
					for (int num17 = 0; num17 <= grid.Columns[gridIndex2].TotalSummaries.Count - 1; num17++)
					{
						if (grid.Columns[gridIndex2].TotalSummaries[num17].Item.SummaryType != SummaryItemType.Average)
						{
							continue;
						}
						if (xRTableCell.Text != "")
						{
							xRTableCell.Text += Environment.NewLine;
						}
						if (grid.Columns[gridIndex2].TotalSummaries[num17].Column.FieldName != list[i].FieldName)
						{
							XRTableCell xRTableCell3 = xRTableCell;
							xRTableCell3.Text = xRTableCell3.Text + grid.Columns[gridIndex2].TotalSummaries[num17].Column.HeaderCaption.ToString() + ":";
						}
						object value2 = grid.Columns[gridIndex2].TotalSummaries[num17].Value;
						if (value2 != null)
						{
							if (grid.Columns[gridIndex2].TotalSummaries[num17].Column.EditSettings != null && grid.Columns[gridIndex2].TotalSummaries[num17].Column.EditSettings.DisplayFormat != "")
							{
								xRTableCell.Text += string.Format(grid.Columns[gridIndex2].TotalSummaries[num17].Column.EditSettings.DisplayFormat, value2);
								XRBinding binding4 = new XRBinding("Text", grid.Columns[gridIndex2].TotalSummaries[num17], "Value")
								{
									FormatString = grid.Columns[gridIndex2].TotalSummaries[num17].Column.EditSettings.DisplayFormat
								};
								xRTableCell.DataBindings.Add(binding4);
							}
							else
							{
								xRTableCell.Text += value2.ToString();
							}
							xRTableCell.TextAlignment = list[i].ContentAlignment;
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.GridSummaryAvg;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (flag12)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 5f, 5f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex3 = list[i].GridIndex;
				if (grid.Columns[gridIndex3].HasTotalSummaries)
				{
					for (int num18 = 0; num18 <= grid.Columns[gridIndex3].TotalSummaries.Count - 1; num18++)
					{
						if (grid.Columns[gridIndex3].TotalSummaries[num18].Item.SummaryType == SummaryItemType.Count)
						{
							if (xRTableCell.Text != "")
							{
								xRTableCell.Text += Environment.NewLine;
							}
							if (grid.Columns[gridIndex3].TotalSummaries[num18].Column.FieldName != list[i].FieldName)
							{
								XRTableCell xRTableCell4 = xRTableCell;
								xRTableCell4.Text = xRTableCell4.Text + grid.Columns[gridIndex3].TotalSummaries[num18].Column.HeaderCaption.ToString() + ":";
							}
							object value3 = grid.Columns[gridIndex3].TotalSummaries[num18].Value;
							if (value3 != null)
							{
								xRTableCell.Text += string.Format("{0:" + DecimalExtension.GetCurrencyFormat(16, 0) + "}", value3);
								XRBinding binding5 = new XRBinding("Text", grid.Columns[gridIndex3].TotalSummaries[num18], "Value");
								xRTableCell.DataBindings.Add(binding5);
								xRTableCell.TextAlignment = list[i].ContentAlignment;
							}
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.GridSummaryCount;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (flag10)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 5f, 5f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex4 = list[i].GridIndex;
				if (grid.Columns[gridIndex4].HasTotalSummaries)
				{
					for (int num19 = 0; num19 <= grid.Columns[gridIndex4].TotalSummaries.Count - 1; num19++)
					{
						if (grid.Columns[gridIndex4].TotalSummaries[num19].Item.SummaryType != SummaryItemType.Max)
						{
							continue;
						}
						if (xRTableCell.Text != "")
						{
							xRTableCell.Text += Environment.NewLine;
						}
						if (grid.Columns[gridIndex4].TotalSummaries[num19].Column.FieldName != list[i].FieldName)
						{
							XRTableCell xRTableCell5 = xRTableCell;
							xRTableCell5.Text = xRTableCell5.Text + grid.Columns[gridIndex4].TotalSummaries[num19].Column.HeaderCaption.ToString() + ":";
						}
						object value4 = grid.Columns[gridIndex4].TotalSummaries[num19].Value;
						if (value4 != null)
						{
							if (grid.Columns[gridIndex4].TotalSummaries[num19].Column.EditSettings != null && grid.Columns[gridIndex4].TotalSummaries[num19].Column.EditSettings.DisplayFormat != "")
							{
								xRTableCell.Text += string.Format(grid.Columns[gridIndex4].TotalSummaries[num19].Column.EditSettings.DisplayFormat, value4);
								XRBinding binding6 = new XRBinding("Text", grid.Columns[gridIndex4].TotalSummaries[num19], "Value")
								{
									FormatString = grid.Columns[gridIndex4].TotalSummaries[num19].Column.EditSettings.DisplayFormat
								};
								xRTableCell.DataBindings.Add(binding6);
							}
							else
							{
								xRTableCell.Text += value4.ToString();
							}
							xRTableCell.TextAlignment = list[i].ContentAlignment;
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.GridSummaryMax;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (flag11)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 5f, 5f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex5 = list[i].GridIndex;
				if (grid.Columns[gridIndex5].HasTotalSummaries)
				{
					for (int num20 = 0; num20 <= grid.Columns[gridIndex5].TotalSummaries.Count - 1; num20++)
					{
						if (grid.Columns[gridIndex5].TotalSummaries[num20].Item.SummaryType != SummaryItemType.Min)
						{
							continue;
						}
						if (xRTableCell.Text != "")
						{
							xRTableCell.Text += Environment.NewLine;
						}
						if (grid.Columns[gridIndex5].TotalSummaries[num20].Column.FieldName != list[i].FieldName)
						{
							XRTableCell xRTableCell6 = xRTableCell;
							xRTableCell6.Text = xRTableCell6.Text + grid.Columns[gridIndex5].TotalSummaries[num20].Column.HeaderCaption.ToString() + ":";
						}
						object value5 = grid.Columns[gridIndex5].TotalSummaries[num20].Value;
						if (value5 != null)
						{
							if (grid.Columns[gridIndex5].TotalSummaries[num20].Column.EditSettings != null && grid.Columns[gridIndex5].TotalSummaries[num20].Column.EditSettings.DisplayFormat != "")
							{
								xRTableCell.Text += string.Format(grid.Columns[gridIndex5].TotalSummaries[num20].Column.EditSettings.DisplayFormat, value5);
								XRBinding binding7 = new XRBinding("Text", grid.Columns[gridIndex5].TotalSummaries[num20], "Value")
								{
									FormatString = grid.Columns[gridIndex5].TotalSummaries[num20].Column.EditSettings.DisplayFormat
								};
								xRTableCell.DataBindings.Add(binding7);
							}
							else
							{
								xRTableCell.Text += value5.ToString();
							}
							xRTableCell.TextAlignment = list[i].ContentAlignment;
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.GridSummaryMin;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		if (flag13)
		{
			xRTableRow = new XRTableRow
			{
				BackColor = Color.LightGray,
				Borders = BorderSide.All
			};
			if (flag4)
			{
				xRTableRow.BorderColor = Color.LightSteelBlue;
			}
			else
			{
				xRTableRow.BorderColor = Color.White;
			}
			xRTableRow.Padding = new PaddingInfo(3f, 3f, 3f, 3f);
			xRTableRow.Font = font;
			for (int i = 0; i <= list.Count - 1; i++)
			{
				XRTableCell xRTableCell = new XRTableCell
				{
					Padding = new PaddingInfo(3f, 3f, 3f, 3f),
					Text = "",
					Multiline = true
				};
				if (flag7)
				{
					xRTableCell.Weight = list[i].WidthF;
				}
				else
				{
					xRTableCell.WidthF = list[i].WidthF;
				}
				int gridIndex6 = list[i].GridIndex;
				if (grid.Columns[gridIndex6].HasTotalSummaries)
				{
					for (int num21 = 0; num21 <= grid.Columns[gridIndex6].TotalSummaries.Count - 1; num21++)
					{
						if (grid.Columns[gridIndex6].TotalSummaries[num21].Item.SummaryType != SummaryItemType.Custom)
						{
							continue;
						}
						if (!string.IsNullOrWhiteSpace(xRTableCell.Text))
						{
							xRTableCell.Text += Environment.NewLine;
						}
						if (grid.Columns[gridIndex6].TotalSummaries[num21].Column.FieldName != list[i].FieldName)
						{
							XRTableCell xRTableCell7 = xRTableCell;
							xRTableCell7.Text = xRTableCell7.Text + grid.Columns[gridIndex6].TotalSummaries[num21].Column.HeaderCaption.ToString() + ":";
						}
						object value6 = grid.Columns[gridIndex6].TotalSummaries[num21].Value;
						if (value6 != null)
						{
							if (grid.Columns[gridIndex6].TotalSummaries[num21].Column.EditSettings != null && grid.Columns[gridIndex6].TotalSummaries[num21].Column.EditSettings.DisplayFormat != "")
							{
								xRTableCell.Text += string.Format(grid.Columns[gridIndex6].TotalSummaries[num21].Column.EditSettings.DisplayFormat, value6);
								XRBinding binding8 = new XRBinding("Text", grid.Columns[gridIndex6].TotalSummaries[num21], "Value")
								{
									FormatString = grid.Columns[gridIndex6].TotalSummaries[num21].Column.EditSettings.DisplayFormat
								};
								xRTableCell.DataBindings.Add(binding8);
							}
							else
							{
								xRTableCell.Text += value6.ToString();
							}
							xRTableCell.TextAlignment = list[i].ContentAlignment;
						}
					}
				}
				else if (i == 0)
				{
					xRTableCell.Text = Resources.Total;
				}
				else
				{
					xRTableCell.Text = "";
				}
				xRTableRow.Cells.Add(xRTableCell);
			}
			xRTable3.Rows.Add(xRTableRow);
		}
		xRTable3.EndInit();
		detailReportBand2 = new DetailReportBand();
		detailBand = new DetailBand();
		detailReportBand2.Bands.Add(detailBand);
		detailBand.Controls.Add(xRTable3);
		detailBand.HeightF = xRTable3.HeightF;
		_report.Bands.Add(detailReportBand2);
		XRPageInfo xRPageInfo = new XRPageInfo
		{
			Size = new Size(313, 23)
		};
		_report.RepPageFooter.Controls.Clear();
		_report.RepPageFooter.Height = 0;
		_report.RepPageFooter.Name = "PageFooter";
		if (printInfo.PrintDate)
		{
			xRPageInfo.Font = font;
			xRPageInfo.Name = "xrPageInfo1";
			xRPageInfo.Padding = new PaddingInfo(2f, 2f, 0f, 0f, 100f);
			xRPageInfo.PageInfo = PageInfo.DateTime;
			xRPageInfo.TextAlignment = TextAlignment.BottomLeft;
			_report.RepPageFooter.Controls.Add(xRPageInfo);
			_report.RepPageFooter.Height = 29;
		}
		if (printInfo.PrintPageNumbers)
		{
			XRPageInfo child = new XRPageInfo
			{
				Font = font,
				Format = "{0} / {1} ",
				Name = "xrPageInfo2",
				TextAlignment = TextAlignment.BottomRight,
				LeftF = xRPageInfo.WidthF,
				WidthF = num4 - xRPageInfo.WidthF
			};
			_report.RepPageFooter.Controls.Add(child);
			_report.RepPageFooter.Height = 29;
		}
		if (!string.IsNullOrWhiteSpace(ExportName))
		{
			_report.Name = ExportName;
		}
		_report.CreateDocument();
		if (showReport)
		{
			documentPreviewWindow.PreviewControl.DocumentSource = _report;
			documentPreviewWindow.Show();
		}
		return _report;
	}

	private static bool FirstColumnSummaryExist(IPanelView xView)
	{
		List<ColumnBase> list = xView.GetVisibleColumns().ToList();
		if (list.Count == 0 || (list.Count == 1 && list[0].Name == "Selected"))
		{
			return false;
		}
		GridColumn gridColumn = ((!(list[0].Name == "Selected")) ? ((GridColumn)list[0]) : ((GridColumn)list[1]));
		if (gridColumn.TotalSummaries != null && gridColumn.TotalSummaries.Count > 0)
		{
			foreach (GridTotalSummaryData totalSummary in gridColumn.TotalSummaries)
			{
				if (totalSummary.Item.SummaryType != SummaryItemType.Custom)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void BooleanToString(object sender, CancelEventArgs e)
	{
		XRTableCell xRTableCell = (XRTableCell)sender;
		if (xRTableCell.Text == "True")
		{
			xRTableCell.Text = Resources.Yes;
		}
		else
		{
			xRTableCell.Text = Resources.No;
		}
	}

	private static DataTable CreateDataTable(List<AsTableReportColumn> repColumns, DataViewPanel dvp)
	{
		DataTable dataTable = new DataTable();
		GridControl gridControl = dvp.InnerGrid();
		for (int i = 0; i <= repColumns.Count - 1; i++)
		{
			if (Nullable.GetUnderlyingType(gridControl.Columns[repColumns[i].GridIndex].FieldType) == null)
			{
				dataTable.Columns.Add(repColumns[i].FieldName, gridControl.Columns[repColumns[i].GridIndex].FieldType);
			}
			else
			{
				dataTable.Columns.Add(repColumns[i].FieldName, Nullable.GetUnderlyingType(gridControl.Columns[repColumns[i].GridIndex].FieldType));
			}
		}
		dataTable.Columns.Add("Level", typeof(PaddingInfo));
		dataTable.Columns.Add("Font", typeof(DXFont));
		return dataTable;
	}

	private static DataTable CreateGroupDataTable(List<AsTableReportColumn> repColumns, DataViewPanel dvp, Dictionary<int, Type> sdColTypes)
	{
		DataTable dataTable = new DataTable();
		for (int i = 0; i <= repColumns.Count - 1; i++)
		{
			if (sdColTypes.ContainsKey(i))
			{
				if ((object)sdColTypes[i] == typeof(bool))
				{
					dataTable.Columns.Add(repColumns[i].FieldName, typeof(string));
					continue;
				}
				if (((object)sdColTypes[i] == typeof(DateTime) || (object)sdColTypes[i] == typeof(DateOnly)) && repColumns[i].IsGroupColumn && dvp.Grid.Columns[repColumns[i].GridIndex].GroupInterval != ColumnGroupInterval.Date)
				{
					dataTable.Columns.Add(repColumns[i].FieldName, typeof(string));
					continue;
				}
				if (sdColTypes[i] == typeof(DateOnly))
				{
					sdColTypes[i] = typeof(DateTime);
				}
				dataTable.Columns.Add(repColumns[i].FieldName, sdColTypes[i]);
			}
			else
			{
				dataTable.Columns.Add(repColumns[i].FieldName);
			}
		}
		dataTable.Columns.Add("Level", typeof(PaddingInfo));
		dataTable.Columns.Add("Font", typeof(DXFont));
		return dataTable;
	}

	private static object[] GetTableRow(int rowHandle, List<AsTableReportColumn> repColumns, DataViewPanel dvp, string fontFamilyName, float fontSize)
	{
		dvp.InnerGrid().GetRow(rowHandle);
		object[] array = new object[1];
		int num = 0;
		bool flag = dvp.InnerGrid().View is TreeListView treeListView && treeListView == dvp.TreeView;
		for (int i = 0; i <= repColumns.Count - 1; i++)
		{
			if (num > 0)
			{
				Array.Resize(ref array, num + 1);
			}
			array[num] = dvp.InnerGrid().GetCellValue(rowHandle, dvp.InnerGrid().Columns[repColumns[i].GridIndex]);
			num++;
		}
		Array.Resize(ref array, num + 1);
		if (flag)
		{
			array[num] = new PaddingInfo(dvp.TreeView.GetNodeByRowHandle(rowHandle).Level * 10, 0f, 0f, 0f);
		}
		else
		{
			array[num] = new PaddingInfo(0f);
		}
		Array.Resize(ref array, num + 1 + 1);
		if (flag)
		{
			DXFont dXFont;
			if (!(bool)dvp.InnerGrid().GetCellValue(rowHandle, "IsLeaf"))
			{
				DXFontStyle style = DXFontStyle.Bold;
				dXFont = new DXFont(fontFamilyName, fontSize, style);
			}
			else
			{
				DXFontStyle style = DXFontStyle.Regular;
				dXFont = new DXFont(fontFamilyName, fontSize, style);
			}
			array[num + 1] = dXFont;
		}
		else
		{
			DXFontStyle style = DXFontStyle.Regular;
			DXFont dXFont = new DXFont(fontFamilyName, fontSize, style);
			array[num + 1] = dXFont;
		}
		return array;
	}

	private static object[] GetGroupRow(int rowHandle, List<AsTableReportColumn> repColumns, DataViewPanel dvp, string fontFamilyName, float fontSize)
	{
		dvp.InnerGrid().GetRow(rowHandle);
		object[] array = new object[1];
		int num = 0;
		bool flag = dvp.InnerGrid().View is TreeListView treeListView && treeListView == dvp.TreeView;
		for (int i = 0; i <= repColumns.Count - 1; i++)
		{
			if (num > 0)
			{
				Array.Resize(ref array, num + 1);
			}
			if (!repColumns[i].IsSummary)
			{
				array[num] = dvp.InnerGrid().GetCellValue(rowHandle, dvp.InnerGrid().Columns[repColumns[i].GridIndex]);
				if (array[num] == null || repColumns[i].IsImage)
				{
					array[num] = "";
				}
				else if (dvp.Grid.View is MyTableView)
				{
					object[] array2 = ((MyTableView)dvp.Grid.View).MyGetGroupRowDisplayText(rowHandle);
					string[] array3 = ((MyTableView)dvp.Grid.View).MyGetGroupRowCaptions(rowHandle);
					for (int j = 0; j <= array3.Length - 1; j++)
					{
						if (array3[j] == repColumns[i].Header)
						{
							array[num] = array2[j];
							break;
						}
						array[num] = "";
					}
				}
				else if (!string.IsNullOrEmpty(repColumns[i].DisplayFormat))
				{
					array[num] = string.Format(repColumns[i].DisplayFormat, array[num]);
				}
			}
			else
			{
				array[num] = "";
				for (int k = 0; k <= dvp.Grid.GroupSummary.Count - 1; k++)
				{
					if (!dvp.Grid.GroupSummary[k].Visible)
					{
						continue;
					}
					GridSummaryItem gridSummaryItem = dvp.Grid.GroupSummary[k];
					if (!(gridSummaryItem.FieldName == repColumns[i].FieldName))
					{
						continue;
					}
					string text = gridSummaryItem.SummaryType switch
					{
						SummaryItemType.Sum => "", 
						SummaryItemType.Average => Resources.GridSummaryAvg + ": ", 
						SummaryItemType.Max => Resources.GridSummaryMax + ": ", 
						SummaryItemType.Min => Resources.GridSummaryMin + ": ", 
						SummaryItemType.Count => Resources.GridSummaryCount + ": ", 
						_ => "", 
					};
					object groupSummaryValue = dvp.Grid.GetGroupSummaryValue(rowHandle, gridSummaryItem);
					array[num] = array[num].ToString() + (string.IsNullOrEmpty(array[num].ToString()) ? "" : Environment.NewLine) + text;
					if (groupSummaryValue != null)
					{
						if (!string.IsNullOrEmpty(repColumns[i].DisplayFormat))
						{
							array[num] = array[num].ToString() + string.Format(repColumns[i].DisplayFormat, groupSummaryValue);
						}
						else
						{
							array[num] = array[num].ToString() + groupSummaryValue.ToString();
						}
					}
				}
			}
			num++;
		}
		Array.Resize(ref array, num + 1);
		if (flag)
		{
			array[num] = new PaddingInfo(dvp.TreeView.GetNodeByRowHandle(rowHandle).Level * 10, 0f, 0f, 0f);
		}
		else
		{
			array[num] = new PaddingInfo(0f);
		}
		Array.Resize(ref array, num + 1 + 1);
		if (flag)
		{
			DXFont dXFont;
			if (!(bool)dvp.InnerGrid().GetCellValue(rowHandle, "IsLeaf"))
			{
				DXFontStyle style = DXFontStyle.Bold;
				dXFont = new DXFont(fontFamilyName, fontSize, style);
			}
			else
			{
				DXFontStyle style = DXFontStyle.Regular;
				dXFont = new DXFont(fontFamilyName, fontSize, style);
			}
			array[num + 1] = dXFont;
		}
		else
		{
			DXFontStyle style = DXFontStyle.Regular;
			DXFont dXFont = new DXFont(fontFamilyName, fontSize, style);
			array[num + 1] = dXFont;
		}
		return array;
	}
}
