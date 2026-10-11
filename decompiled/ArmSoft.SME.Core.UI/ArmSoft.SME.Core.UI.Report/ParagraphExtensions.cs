using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ArmSoft.SME.Core.UI.Report;

public class ParagraphExtensions
{
	private static Dictionary<Paragraph, InlineUIContainer> _collapseButtonDictionary;

	private static Dictionary<Paragraph, List<Inline>> _paragraphInlineStorage;

	public static readonly DependencyProperty IsCollapsible = DependencyProperty.RegisterAttached("IsCollapsible", typeof(bool), typeof(Paragraph), (PropertyMetadata)(object)new UIPropertyMetadata(false, new SafeEventHandler<DependencyPropertyChangedEventArgs>(IsCollapsibleChanged).Handler));

	public static bool GetIsCollapsible(DependencyObject obj)
	{
		return (bool)obj.GetValue(IsCollapsible);
	}

	public static void SetIsCollapsible(DependencyObject obj, bool value)
	{
		obj.SetValue(IsCollapsible, (object)value);
	}

	private static void IsCollapsibleChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (_collapseButtonDictionary == null)
		{
			_collapseButtonDictionary = new Dictionary<Paragraph, InlineUIContainer>();
		}
		if (_paragraphInlineStorage == null)
		{
			_paragraphInlineStorage = new Dictionary<Paragraph, List<Inline>>();
		}
		Paragraph paragraph = (Paragraph)sender;
		InlineUIContainer value2;
		if ((bool)e.NewValue)
		{
			InlineUIContainer inlineUIContainer;
			if (_collapseButtonDictionary.TryGetValue(paragraph, out var value))
			{
				inlineUIContainer = value;
			}
			else
			{
				ToggleButton toggleButton = new ToggleButton
				{
					Tag = paragraph,
					Content = "Hide"
				};
				inlineUIContainer = new InlineUIContainer();
				StackPanel stackPanel = new StackPanel
				{
					Orientation = Orientation.Horizontal
				};
				stackPanel.Children.Add(toggleButton);
				TextBlock element = new TextBlock
				{
					TextWrapping = TextWrapping.Wrap,
					Text = "This is a test",
					Width = 200.0,
					Margin = new Thickness(0.0, 5.0, 0.0, 5.0)
				};
				stackPanel.Children.Add(element);
				TextBlock element2 = new TextBlock
				{
					TextWrapping = TextWrapping.Wrap,
					Text = "Small.",
					Width = 50.0,
					Margin = new Thickness(0.0, 5.0, 0.0, 5.0),
					Foreground = Brushes.Red
				};
				stackPanel.Children.Add(element2);
				CheckBox element3 = new CheckBox
				{
					IsChecked = true,
					Width = 120.0,
					VerticalAlignment = VerticalAlignment.Top,
					Margin = new Thickness(20.0, 5.0, 0.0, 5.0)
				};
				stackPanel.Children.Add(element3);
				inlineUIContainer.Child = stackPanel;
				_collapseButtonDictionary.Add(paragraph, inlineUIContainer);
				toggleButton.Click += new SafeEventHandler<RoutedEventArgs>(CollapseButton_Click).Handler;
			}
			if (paragraph.Inlines.Count > 0)
			{
				paragraph.Inlines.InsertBefore(paragraph.Inlines.First(), inlineUIContainer);
			}
			else
			{
				paragraph.Inlines.Add(inlineUIContainer);
			}
		}
		else if (_collapseButtonDictionary.TryGetValue(paragraph, out value2))
		{
			InlineUIContainer inlineUIContainer2 = value2;
			if (((ToggleButton)inlineUIContainer2.Child).IsChecked.Value)
			{
				SetCollapsedState(isCollapsed: false, paragraph);
			}
			paragraph.Inlines.Remove(inlineUIContainer2);
		}
	}

	private static void CollapseButton_Click(object sender, RoutedEventArgs e)
	{
		ToggleButton toggleButton = (ToggleButton)sender;
		SetCollapsedState(p: (Paragraph)toggleButton.Tag, isCollapsed: toggleButton.IsChecked.Value);
	}

	private static void SetCollapsedState(bool isCollapsed, Paragraph p)
	{
		InlineUIContainer item = _collapseButtonDictionary[p];
		List<Inline> list;
		if (_paragraphInlineStorage.TryGetValue(p, out var value))
		{
			list = value;
		}
		else
		{
			list = new List<Inline>();
			_paragraphInlineStorage.Add(p, list);
		}
		if (isCollapsed)
		{
			foreach (Inline inline in p.Inlines)
			{
				list.Add(inline);
			}
			p.Inlines.Clear();
			p.Inlines.Add(item);
			return;
		}
		p.Inlines.Clear();
		foreach (Inline item2 in list)
		{
			p.Inlines.Add(item2);
		}
		list.Clear();
	}

	static ParagraphExtensions()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected Obj, but got Unknown
	}
}
