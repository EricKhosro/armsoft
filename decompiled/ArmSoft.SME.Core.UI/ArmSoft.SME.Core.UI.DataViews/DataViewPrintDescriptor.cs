using System;
using System.Collections.Generic;
using ArmSoft.SME.Common.Models.Caches;

namespace ArmSoft.SME.Core.UI.DataViews;

public class DataViewPrintDescriptor
{
	private int _indexOfDefault;

	private List<TemplatesCacheRow> _templates;

	public bool ShowDialog { get; set; }

	public bool PrintSelectedRows { get; set; }

	public bool InsertPageBreaks { get; set; }

	public List<TemplatesCacheRow> Templates => _templates;

	public TemplatesCacheRow DefaultTemplate
	{
		get
		{
			if (_indexOfDefault == -1)
			{
				return null;
			}
			return _templates[_indexOfDefault];
		}
	}

	public int IndexOfDefault
	{
		get
		{
			return _indexOfDefault;
		}
		set
		{
			if (value < 0)
			{
				_indexOfDefault = -1;
				return;
			}
			if (value >= _templates.Count)
			{
				throw new InvalidOperationException("Invalid index " + value);
			}
			_indexOfDefault = value;
		}
	}

	public DataViewPrintDescriptor()
	{
		Initialize();
	}

	public DataViewPrintDescriptor(byte docType)
	{
		Initialize();
		Fill(docType);
	}

	private void Initialize()
	{
		_indexOfDefault = -1;
		_templates = new List<TemplatesCacheRow>();
		ShowDialog = true;
		PrintSelectedRows = false;
		InsertPageBreaks = true;
	}

	public void Fill(byte docType)
	{
		Templates.AddRange(Settings.CoreCacheService.Template.GetTemplates(docType));
		if (Templates.Count > 1)
		{
			TemplatesCacheRow defaultPrintTemplate = Settings.CoreCacheService.Template.GetDefaultPrintTemplate(docType);
			for (int i = 0; i <= Templates.Count - 1; i++)
			{
				if (Templates[i].Name == defaultPrintTemplate.Name && Templates[i].FileFormat == defaultPrintTemplate.FileFormat)
				{
					IndexOfDefault = i;
					break;
				}
			}
		}
		else if (Templates.Count == 1)
		{
			IndexOfDefault = 0;
		}
	}
}
