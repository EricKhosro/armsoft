using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ArmSoft.SME.Common;
using ArmSoft.SME.Common.Extensions;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI.DataViews;
using ArmSoft.SME.Core.UI.DataViews.Reports;
using ArmSoft.SME.Core.UI.Enums;
using DevExpress.Xpf.Bars;

namespace ArmSoft.SME.Core.UI.Report;

public abstract class AsReport
{
	public delegate void StatusChangedEventHandler(object sender, string msg);

	public class ReportViewerParameter
	{
		public string Name { get; set; } = "";

		public string Caption { get; set; } = "";

		public object Value { get; set; }

		public string Format { get; set; } = "";

		public bool IncludeCaptionToolTip { get; set; } = true;

		public IDropDownView DataView { get; set; }
	}

	private delegate void ShowProgressWindowDelegate(AsReport rep);

	protected readonly Dictionary<string, ReportViewerParameter> _parameters;

	private readonly Dictionary<string, ContextMenuFunction> _userFunctions;

	private PopupMenu _myContextMenu;

	private bool _allowOperation = true;

	public AsReportViewer ReportPanel { get; set; }

	public abstract string Name { get; }

	public virtual bool DialogSupported { get; set; }

	public virtual bool AllowRefresh { get; set; } = true;

	public string HelpId { get; set; } = "";

	public int RowsCount => ReportPanel.RowsCount();

	public bool AllowOperation
	{
		get
		{
			return _allowOperation;
		}
		set
		{
			_allowOperation = value;
		}
	}

	public virtual bool AllowAdd => false;

	public virtual bool AllowEdit => false;

	public virtual bool AllowView => false;

	public virtual bool AllowDelete => false;

	public virtual bool AllowCreateCopy => false;

	public virtual bool AllowPreview => false;

	public virtual bool AllowDocumentHistory => false;

	public bool IsUpdate { get; set; }

	public bool IsCanceled { get; set; }

	public event StatusChangedEventHandler StatusChanged;

	public abstract string Caption();

	public AsReport()
	{
		_userFunctions = new Dictionary<string, ContextMenuFunction>();
		_parameters = new Dictionary<string, ReportViewerParameter>();
		ReportPanel = new AsReportViewer(this);
	}

	public virtual bool ShowDialog(bool useParamValues, bool isRefreshMode)
	{
		return true;
	}

	public virtual void Add(object rowInfo)
	{
	}

	public virtual void Edit(object rowInfo)
	{
		if (rowInfo != null)
		{
			IRowWithISN rowWithISN = (IRowWithISN)rowInfo;
			Settings.DocumentService.ShowWindow(rowWithISN.GetDocISN(), WindowViewMode.Edit);
		}
	}

	public virtual void View(object rowInfo)
	{
		if (rowInfo != null)
		{
			IRowWithISN rowWithISN = (IRowWithISN)rowInfo;
			Settings.DocumentService.ShowWindow(rowWithISN.GetDocISN(), WindowViewMode.View);
		}
	}

	public virtual void Delete(object rowInfo)
	{
		if (rowInfo != null)
		{
			IRowWithISN rowWithISN = (IRowWithISN)rowInfo;
			if (MessageBox.Show(string.Format(Resources.ConfDocDel, Settings.DocumentService.GetFullCaption(rowWithISN.GetDocType())), "", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
			{
				Settings.DocumentService.Delete(rowWithISN.GetDocISN());
				UpdateData();
			}
		}
	}

	public virtual void CreateCopy(object rowInfo)
	{
		if (rowInfo != null)
		{
			IRowWithISN rowWithISN = (IRowWithISN)rowInfo;
			Settings.DocumentService.ShowWindow(Settings.DocumentService.CreateCopy(rowWithISN.GetDocISN()), WindowViewMode.Copy);
		}
	}

	public virtual void DocumentHistory(object rowInfo)
	{
		if (rowInfo != null)
		{
			new DocumentLog(((IRowWithISN)rowInfo).GetDocISN()).Show();
		}
	}

	public virtual object CreatePreview(object rowInfo)
	{
		return null;
	}

	protected virtual string GetDefaultFunction()
	{
		return "";
	}

	public virtual string CaptionToolTip()
	{
		return Caption();
	}

	public virtual string ReportTitle()
	{
		return ComposeTitle();
	}

	public string ComposeTitle()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(CaptionToolTip());
		foreach (ReportViewerParameter param in _parameters.Values)
		{
			if (!param.IncludeCaptionToolTip)
			{
				continue;
			}
			stringBuilder.AppendLine();
			stringBuilder.Append(new string(' ', 3));
			stringBuilder.Append(param.Caption);
			stringBuilder.Append("  - ");
			if (param.Value is string)
			{
				if (string.IsNullOrWhiteSpace(param.Value.ToString()))
				{
					stringBuilder.Append(Resources.All);
					continue;
				}
				if (param.DataView == null)
				{
					stringBuilder.Append('"');
					stringBuilder.Append(param.Value.ToString());
					stringBuilder.Append('"');
					continue;
				}
				int num = param.DataView.ValuesList().FindIndex((string p) => p == param.Value.ToString());
				if (num >= 0)
				{
					stringBuilder.Append(param.DataView.CommentMember(num));
					continue;
				}
				stringBuilder.Append('"');
				stringBuilder.Append(param.Value.ToString());
				stringBuilder.Append('"');
			}
			else if (param.Value == null)
			{
				if (param.Format.InList("dd/MM/yy", "dd/MM/yyyy"))
				{
					stringBuilder.Append(Resources.CompareOpEmpty);
				}
				else
				{
					stringBuilder.Append(Resources.All);
				}
			}
			else if (param.Value is bool value)
			{
				stringBuilder.Append(value.ToYesNo());
			}
			else if (string.IsNullOrWhiteSpace(param.Format))
			{
				stringBuilder.Append(param.Value.ToString());
			}
			else
			{
				stringBuilder.Append(string.Format(param.Value.ToString(), param.Format));
			}
		}
		return stringBuilder.ToString();
	}

	protected void FillMenuFromRibbon()
	{
		string text = GetDefaultFunction();
		if (string.IsNullOrEmpty(text))
		{
			text = ReportPanel.DataViewEditBarButtonItem.Name;
		}
		ClearMenu();
		if (ReportPanel.ReportContextMenu == null)
		{
			ReportPanel.ReportContextMenu = new PopupMenu();
		}
		if (AllowAdd)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DataViewAddBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
		if (AllowEdit)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DataViewEditBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
		if (AllowPreview)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DataViewDocumentBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
		if (AllowDelete)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DataViewDeleteBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
		if (AllowCreateCopy)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DataViewCreateCopyBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
		if (Settings.SystemType != SystemTypes.MarketPlace && AllowDocumentHistory)
		{
			BarButtonItemLink barButtonItemLink = (BarButtonItemLink)ReportPanel.DocumentHistoryBarButtonItem.CreateLink();
			if (text == barButtonItemLink.BarItemName)
			{
				barButtonItemLink.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
			AddContextMenuSeparator();
			ReportPanel.ReportContextMenu.ItemLinks.Add(barButtonItemLink);
		}
	}

	public void FillMenu(object rowInfo)
	{
		FillMenuFromRibbon();
		AddContextMenuSeparator();
		FillContextMenu(rowInfo);
		foreach (BarItemLink item in _myContextMenu.ItemLinks.Cast<BarItemLink>())
		{
			if (item.Item != null && item.Item.Content.ToString() == GetDefaultFunction())
			{
				item.Item.ContentTemplate = (DataTemplate)Settings.GenericResources["DefButtonTemplate"];
			}
		}
	}

	protected virtual void FillContextMenu(object rowInfo)
	{
	}

	protected void AddParameter(string name, string caption, object defaultValue, string format = "", bool includeCaptionToolTip = true, IDropDownView dataView = null)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new InvalidOperationException("Parameter name is empty.");
		}
		ReportViewerParameter reportViewerParameter = new ReportViewerParameter
		{
			Name = name.Trim(),
			Caption = caption,
			Value = defaultValue,
			Format = format,
			IncludeCaptionToolTip = includeCaptionToolTip,
			DataView = dataView
		};
		_parameters.Add(reportViewerParameter.Name, reportViewerParameter);
	}

	public void Show(bool useParamValues = false, bool openAsChild = true)
	{
		try
		{
			if ((DialogSupported && ShowDialog(useParamValues, isRefreshMode: false)) || !DialogSupported)
			{
				ReportPanel.Caption = Caption();
				ReportPanel.ToolTip = CaptionToolTip();
				ReportPanel.ShowInner = openAsChild;
				((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Delegate)new ShowProgressWindowDelegate(ShowProgressWindow), (DispatcherPriority)2, new object[1] { this });
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public bool BeforeLoadWrapper()
	{
		try
		{
			return BeforeShowReport();
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
			if (ReportPanel.RepRows.Count == 0)
			{
				ReportPanel.AddString("Report is empty!", AsRepRowStyle.CaptionRow);
			}
		}
		return false;
	}

	public virtual void ExecuteDPR()
	{
	}

	private void ShowProgressWindow(AsReport rep)
	{
		try
		{
			Mouse.OverrideCursor = Cursors.Wait;
			ExecuteDPR();
			if (BeforeLoadWrapper())
			{
				ReportPanel.Show();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = null;
		}
	}

	public void ShowStatus(string message)
	{
		StatusChanged?.Invoke(this, message);
	}

	public virtual bool BeforeShowReport()
	{
		return true;
	}

	public void GetContextMenu(object rowInfo)
	{
		if (ReportPanel.ReportContextMenu == null)
		{
			ReportPanel.ReportContextMenu = new PopupMenu();
		}
		_myContextMenu = ReportPanel.ReportContextMenu;
		_myContextMenu.ItemLinks.Clear();
		FillMenu(rowInfo);
		if (_myContextMenu != null && _myContextMenu.ItemLinks.Count > 0)
		{
			BarItemLinkCollection itemLinks = _myContextMenu.ItemLinks;
			if (itemLinks[itemLinks.Count - 1] is BarItemLinkSeparator)
			{
				_myContextMenu.ItemLinks.RemoveAt(_myContextMenu.ItemLinks.Count - 1);
			}
		}
		if (_myContextMenu != null && _myContextMenu.ItemLinks.Count > 0 && _myContextMenu.ItemLinks[0] is BarItemLinkSeparator)
		{
			_myContextMenu.ItemLinks.RemoveAt(0);
		}
	}

	public void ClearMenu()
	{
		if (ReportPanel.ReportContextMenu == null)
		{
			ReportPanel.ReportContextMenu = new PopupMenu();
		}
		_myContextMenu = ReportPanel.ReportContextMenu;
		_myContextMenu.ItemLinks.Clear();
		_userFunctions.Clear();
		int num = 0;
		while (num < ReportPanel.BarManager.Items.Count)
		{
			if (ReportPanel.BarManager.Items[num].Tag != null && ReportPanel.BarManager.Items[num].Tag.ToString() == "UserFunction")
			{
				ReportPanel.BarManager.Items.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	public void AddContextMenuSeparator()
	{
		if (_myContextMenu != null)
		{
			_myContextMenu.ItemLinks.Add(new BarItemLinkSeparator());
		}
	}

	public void AddContextMenuFunction(string itemCaption, ContextMenuFunction func, ImageSource imgSource = null, bool isRibbonFunction = false, bool isEnabled = true)
	{
		if (_myContextMenu == null)
		{
			if (ReportPanel.ReportContextMenu == null)
			{
				ReportPanel.ReportContextMenu = new PopupMenu();
			}
			_myContextMenu = ReportPanel.ReportContextMenu;
			_myContextMenu.ItemLinks.Clear();
			_userFunctions.Clear();
		}
		BarButtonItem barButtonItem = new BarButtonItem
		{
			IsEnabled = isEnabled,
			Content = itemCaption.ToString()
		};
		if (imgSource != null)
		{
			barButtonItem.Glyph = imgSource;
		}
		if (func != null)
		{
			_userFunctions.Add(itemCaption, func);
			barButtonItem.Tag = "UserFunction";
			ReportPanel.BarManager.Items.Add(barButtonItem);
			barButtonItem.ItemClick += new SafeEventHandler<RoutedEventArgs>(ExecuteUserFunction).Handler;
		}
		if (isRibbonFunction)
		{
			barButtonItem.Tag = "SYS";
		}
		else
		{
			barButtonItem.Tag = "";
		}
		_myContextMenu.ItemLinks.Add(barButtonItem.CreateLink());
	}

	public object ActiveRowInfo()
	{
		return ReportPanel.ActiveRowInfo();
	}

	public void ExecuteUserFunction(object sender, RoutedEventArgs e)
	{
		try
		{
			BarButtonItem barButtonItem = sender as BarButtonItem;
			if (_userFunctions.ContainsKey(barButtonItem.Content.ToString()) && _userFunctions[barButtonItem.Content.ToString()] != null)
			{
				string text = barButtonItem.Content.ToString();
				if (text == ReportPanel.AddFunction.Item.Content.ToString())
				{
					ReportPanel.AddFunction.Item.PerformClick();
					return;
				}
				if (text == ReportPanel.EditFunction.Item.Content.ToString())
				{
					ReportPanel.EditFunction.Item.PerformClick();
					return;
				}
				if (text == ReportPanel.CreateCopyFunction.Item.Content.ToString())
				{
					ReportPanel.CreateCopyFunction.Item.PerformClick();
					return;
				}
				if (text == ReportPanel.DeleteFunction.Item.Content.ToString())
				{
					ReportPanel.DeleteFunction.Item.PerformClick();
					return;
				}
				if (text == ReportPanel.PreviewFunction.Item.Content.ToString())
				{
					ReportPanel.PreviewFunction.Item.PerformClick();
					return;
				}
				if (Settings.SystemType != SystemTypes.MarketPlace && barButtonItem.Content.ToString() == ReportPanel.DocumentHistoryFunction.Item.Content.ToString())
				{
					ReportPanel.DocumentHistoryFunction.Item.PerformClick();
					return;
				}
				ReportMenuItem reportMenuItem = new ReportMenuItem(_userFunctions[barButtonItem.Content.ToString()]);
				if (reportMenuItem != null)
				{
					((DispatcherObject)ReportPanel).Dispatcher.Invoke((Delegate)reportMenuItem.Invoker(), new object[1] { ReportPanel.ActiveRowInfo() });
				}
			}
			else
			{
				ReportPanel.SetFocus();
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void ExecuteDefaultFunction()
	{
		FillMenu(ReportPanel.ActiveRowInfo());
		string text = GetDefaultFunction();
		if (string.IsNullOrEmpty(text))
		{
			if (!AllowView && !AllowEdit)
			{
				return;
			}
			text = ReportPanel.EditFunction.Item.Content.ToString();
		}
		try
		{
			string text2 = text ?? "";
			ContextMenuFunction value;
			if (text2 == ReportPanel.AddFunction.Item.Content.ToString())
			{
				ReportPanel.AddFunction.Item.PerformClick();
			}
			else if (text2 == ReportPanel.EditFunction.Item.Content.ToString())
			{
				if (AllowEdit)
				{
					ReportPanel.Edit();
				}
				else if (AllowView)
				{
					ReportPanel.View();
				}
			}
			else if (text2 == ReportPanel.CreateCopyFunction.Item.Content.ToString())
			{
				ReportPanel.CreateCopyFunction.Item.PerformClick();
			}
			else if (text2 == ReportPanel.DeleteFunction.Item.Content.ToString())
			{
				ReportPanel.DeleteFunction.Item.PerformClick();
			}
			else if (text2 == ReportPanel.PreviewFunction.Item.Content.ToString())
			{
				ReportPanel.PreviewFunction.Item.PerformClick();
			}
			else if (Settings.SystemType != SystemTypes.MarketPlace && text == ReportPanel.DocumentHistoryFunction.Item.Content.ToString())
			{
				ReportPanel.DocumentHistoryFunction.Item.PerformClick();
			}
			else if (_userFunctions.TryGetValue(text, out value))
			{
				ReportMenuItem reportMenuItem = new ReportMenuItem(value);
				if (reportMenuItem != null)
				{
					((DispatcherObject)ReportPanel).Dispatcher.Invoke((Delegate)reportMenuItem.Invoker(), new object[1] { ReportPanel.ActiveRowInfo() });
				}
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}

	public void UpdateData()
	{
		try
		{
			IsUpdate = true;
			((DispatcherObject)Settings.HostingEnvironment).Dispatcher.Invoke((Delegate)new ShowProgressWindowDelegate(ShowProgressWindow), (DispatcherPriority)2, new object[1] { this });
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
	}
}
