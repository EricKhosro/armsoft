using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ArmSoft.SME.Common.Directories;
using ArmSoft.SME.Common.Documents;
using ArmSoft.SME.Common.MetaData;
using ArmSoft.SME.Common.SmartEnums;
using ArmSoft.SME.Core.UI.Directories;
using ArmSoft.SME.Core.UI.Documents;
using ArmSoft.SME.Core.UI.DropDowns;
using ArmSoft.SME.Core.UI.MetaData;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ArmSoft.SME.Core.UI.Services;

public class DefinitionMappingsService
{
	private static readonly Dictionary<string, Type> _partialSmartEnumsByTypes;

	private static readonly ConcurrentDictionary<short, DocDefinition> _documentDefsByTypes;

	private static readonly ConcurrentDictionary<short, Type> _documentWindowsByTypes;

	private static readonly ConcurrentDictionary<short, Type> _directoryWindowsByTypes;

	private static readonly ConcurrentDictionary<short, Type> _directoryDocumentWindowsByTypes;

	private static readonly ConcurrentDictionary<short, Type> _dataViewsByTypes;

	private static readonly ConcurrentDictionary<short, DirectoryDefinition> _directoryDefsByTypes;

	private static readonly ConcurrentDictionary<short, Type> _dropDownsByTypes;

	private static readonly Dictionary<string, Type> _dictionaryUserScriptsByTypes;

	private static readonly Dictionary<string, Type> _dvsByTypes;

	private static readonly Dictionary<string, Type> _dvdByTypes;

	private static readonly Dictionary<string, Type> _cfByTypes;

	static DefinitionMappingsService()
	{
		_partialSmartEnumsByTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
		_documentWindowsByTypes = new ConcurrentDictionary<short, Type>();
		_documentDefsByTypes = new ConcurrentDictionary<short, DocDefinition>();
		_directoryWindowsByTypes = new ConcurrentDictionary<short, Type>();
		_dataViewsByTypes = new ConcurrentDictionary<short, Type>();
		_directoryDefsByTypes = new ConcurrentDictionary<short, DirectoryDefinition>();
		_directoryDocumentWindowsByTypes = new ConcurrentDictionary<short, Type>();
		_dropDownsByTypes = new ConcurrentDictionary<short, Type>();
		_dictionaryUserScriptsByTypes = new Dictionary<string, Type>();
		_dvsByTypes = new Dictionary<string, Type>();
		_dvdByTypes = new Dictionary<string, Type>();
		_cfByTypes = new Dictionary<string, Type>();
	}

	public static void Configure(Assembly assembly)
	{
		Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			short docType;
			short dirType;
			short dirType2;
			short dataProviderType;
			short dropDownType;
			if (IsPartialSmartEnum(type, out var isWithAllValues))
			{
				AddPartialSmartEnums(type, isWithAllValues);
			}
			else if (IsDocumentDefinitions(type))
			{
				FillDocumentDefinitions(type);
			}
			else if (IsDirectoryDefinitions(type))
			{
				FillDirectoryDefinitions(type);
			}
			else if (IsDocumentWindow(type, out docType))
			{
				_documentWindowsByTypes.TryAdd(docType, type);
			}
			else if (IsDirectoryWindow(type, out dirType))
			{
				_directoryWindowsByTypes.TryAdd(dirType, type);
			}
			else if (IsDirectoryDocumentWindow(type, out dirType2))
			{
				_directoryDocumentWindowsByTypes.TryAdd(dirType2, type);
			}
			else if (IsDataView(type, out dataProviderType))
			{
				_dataViewsByTypes.TryAdd(dataProviderType, type);
			}
			else if (IsDropDown(type, out dropDownType))
			{
				_dropDownsByTypes.TryAdd(dropDownType, type);
			}
		}
	}

	private static void AddPartialSmartEnums(Type type, bool isWithAllValues)
	{
		if (!_partialSmartEnumsByTypes.TryAdd(type.Name, type) && isWithAllValues)
		{
			_partialSmartEnumsByTypes[type.Name] = type;
		}
	}

	public static void ClearDynamics()
	{
		_partialSmartEnumsByTypes.Clear();
		_documentDefsByTypes.Clear();
		_documentWindowsByTypes.Clear();
		_directoryWindowsByTypes.Clear();
		_directoryDocumentWindowsByTypes.Clear();
		_dataViewsByTypes.Clear();
		_directoryDefsByTypes.Clear();
	}

	private static bool IsPartialSmartEnum(Type type, out bool isWithAllValues)
	{
		if (type.IsDefined(typeof(SmartEnumPartialAttribute), inherit: false))
		{
			IEnumerable<SmartEnumPartialAttribute> customAttributes = type.GetCustomAttributes<SmartEnumPartialAttribute>();
			isWithAllValues = customAttributes.Any((SmartEnumPartialAttribute p) => p.SmartEnumType == SmartEnumType.Special);
			return true;
		}
		isWithAllValues = false;
		return false;
	}

	public static bool IsDropDown(Type type, out short dropDownType)
	{
		if (type.IsDefined(typeof(DropDownAttribute), inherit: false))
		{
			DropDownAttribute customAttribute = type.GetCustomAttribute<DropDownAttribute>();
			dropDownType = customAttribute.DropDownType;
			return true;
		}
		dropDownType = 0;
		return false;
	}

	public static bool IsDocumentWindow(Type type, out short docType)
	{
		if (type.IsDefined(typeof(DocumentWindowAttribute), inherit: false))
		{
			DocumentWindowAttribute documentWindowAttribute = (DocumentWindowAttribute)Attribute.GetCustomAttribute(type, typeof(DocumentWindowAttribute));
			docType = documentWindowAttribute.DocType;
			return true;
		}
		docType = 0;
		return false;
	}

	public static bool IsDirectoryWindow(Type type, out short dirType)
	{
		if (type.IsDefined(typeof(DirectoryWindowAttribute), inherit: false))
		{
			DirectoryWindowAttribute directoryWindowAttribute = (DirectoryWindowAttribute)Attribute.GetCustomAttribute(type, typeof(DirectoryWindowAttribute));
			dirType = directoryWindowAttribute.DirectoryType;
			return true;
		}
		dirType = 0;
		return false;
	}

	public static bool IsDirectoryDocumentWindow(Type type, out short dirType)
	{
		if (type.IsDefined(typeof(DirectoryDocumentWindowAttribute), inherit: false))
		{
			DirectoryDocumentWindowAttribute directoryDocumentWindowAttribute = (DirectoryDocumentWindowAttribute)Attribute.GetCustomAttribute(type, typeof(DirectoryDocumentWindowAttribute));
			dirType = directoryDocumentWindowAttribute.DirectoryType;
			return true;
		}
		dirType = 0;
		return false;
	}

	public static bool IsDataView(Type type, out short dataProviderType)
	{
		if (type.IsDefined(typeof(DataViewAttribute), inherit: false))
		{
			DataViewAttribute dataViewAttribute = (DataViewAttribute)Attribute.GetCustomAttribute(type, typeof(DataViewAttribute));
			dataProviderType = dataViewAttribute.DataProviderType;
			return true;
		}
		dataProviderType = 0;
		return false;
	}

	public static Type GetPartialSmartEnum(string name)
	{
		_partialSmartEnumsByTypes.TryGetValue(name, out var value);
		return value;
	}

	private static bool IsDocumentDefinitions(Type type)
	{
		if (type == typeof(AllDocDefinitions) || type.BaseType == typeof(AllDocDefinitions))
		{
			return true;
		}
		return false;
	}

	private static bool IsDirectoryDefinitions(Type type)
	{
		if (type == typeof(AllDirectoryDefinitions) || type.BaseType == typeof(AllDirectoryDefinitions))
		{
			return true;
		}
		return false;
	}

	private static void FillDocumentDefinitions(Type type)
	{
		object obj = Activator.CreateInstance(type);
		PropertyInfo[] properties = type.GetProperties();
		for (int i = 0; i < properties.Length; i++)
		{
			DocDefinition docDefinition = properties[i].GetValue(obj) as DocDefinition;
			_documentDefsByTypes.TryAdd(docDefinition.POType, docDefinition);
		}
	}

	private static void FillDirectoryDefinitions(Type type)
	{
		object obj = Activator.CreateInstance(type);
		PropertyInfo[] properties = type.GetProperties();
		for (int i = 0; i < properties.Length; i++)
		{
			DirectoryDefinition directoryDefinition = properties[i].GetValue(obj) as DirectoryDefinition;
			_directoryDefsByTypes.TryAdd(directoryDefinition.POType, directoryDefinition);
		}
	}

	public static bool TryResolveDocumentDefinition(short docType, out DocDefinition definition)
	{
		return _documentDefsByTypes.TryGetValue(docType, out definition);
	}

	public static bool TryResolveDocumentWindow(short docType, out Type docWindow)
	{
		return _documentWindowsByTypes.TryGetValue(docType, out docWindow);
	}

	public static bool TryResolveDirectoryDefinition(short dirType, out DirectoryDefinition definition)
	{
		return _directoryDefsByTypes.TryGetValue(dirType, out definition);
	}

	public static bool TryResolveDirectoryWindow(short dirType, out Type dirWindow)
	{
		return _directoryWindowsByTypes.TryGetValue(dirType, out dirWindow);
	}

	public static bool TryResolveDirectoryDocumentWindow(short dirType, out Type definition)
	{
		return _directoryDocumentWindowsByTypes.TryGetValue(dirType, out definition);
	}

	public static bool TryResolveDataView(short dataProviderType, out Type dataView)
	{
		return _dataViewsByTypes.TryGetValue(dataProviderType, out dataView);
	}

	public static bool TryResolveDropDown(short dropDownType, out Type dropDown)
	{
		return _dropDownsByTypes.TryGetValue(dropDownType, out dropDown);
	}

	public static Type ResolveUserScript(string name, Type inheritType)
	{
		if (_dictionaryUserScriptsByTypes.TryGetValue(name, out var value))
		{
			return value;
		}
		UserScriptDefinition userScriptDefinition = Settings.ServiceProvider.GetRequiredService<IUserScriptService>().Load(name);
		if (CSharpCompilerHelper.TryCompile("UserScript_" + name, userScriptDefinition.Script, loadAssembly: true, out var assembly, out var errorDiagnostics))
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.IsSubclassOf(inheritType))
				{
					_dictionaryUserScriptsByTypes.Add(name, type);
					return type;
				}
			}
			throw new InvalidOperationException($"The class derived from {inheritType.Name} not found for user script {name}.");
		}
		Log.ForContext(typeof(DefinitionMappingsService)).Error(string.Join("\r\n", errorDiagnostics));
		throw new InvalidOperationException("User script " + name + " C# compilation error.");
	}

	public static Type ResolveDVS(string group)
	{
		if (_dvsByTypes.TryGetValue(group, out var value))
		{
			return value;
		}
		Type typeFromHandle = typeof(IDataViewSettingsHandler);
		foreach (string item in Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(group))
		{
			ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dataViewDefinition = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(group, item);
			if (dataViewDefinition.Script == null || !dataViewDefinition.Script.Contains("DataViewSettingsHandler"))
			{
				continue;
			}
			if (CSharpCompilerHelper.TryCompile("DVS_" + group, dataViewDefinition.Script, loadAssembly: true, out var assembly, out var errorDiagnostics))
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (typeFromHandle.IsAssignableFrom(type))
					{
						_dvsByTypes.Add(group, type);
						return type;
					}
				}
				throw new InvalidOperationException($"The class derived from {typeFromHandle.Name} not found for user script {group}.");
			}
			Log.ForContext(typeof(DefinitionMappingsService)).Error(string.Join("\r\n", errorDiagnostics));
			throw new InvalidOperationException("User script " + group + " C# compilation error.");
		}
		return null;
	}

	public static Type ResolveDVD(string group, string name)
	{
		if (_dvdByTypes.TryGetValue(name, out var value))
		{
			return value;
		}
		Type typeFromHandle = typeof(IDataProviderExtensionHandler);
		ArmSoft.SME.Core.UI.MetaData.DataViewDefinition dataViewDefinition = Settings.ServiceProvider.GetRequiredService<IDVDService>().Load(group, name);
		if (dataViewDefinition.Script != null)
		{
			if (CSharpCompilerHelper.TryCompile("DVD_" + name, dataViewDefinition.Script, loadAssembly: true, out var assembly, out var errorDiagnostics))
			{
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (typeFromHandle.IsAssignableFrom(type))
					{
						_dvdByTypes.Add(name, type);
						return type;
					}
				}
				throw new InvalidOperationException($"The class derived from {typeFromHandle.Name} not found for DVD {name}.");
			}
			Log.ForContext(typeof(DefinitionMappingsService)).Error(string.Join("\r\n", errorDiagnostics));
			throw new InvalidOperationException("DVD " + name + " C# compilation error.");
		}
		return null;
	}

	public static Type ResolveCF(string group, string name)
	{
		if (_cfByTypes.TryGetValue(name, out var value))
		{
			return value;
		}
		Type typeFromHandle = typeof(IContextFunctionHandler);
		ContextFunctionDefinition contextFunctionDefinition = Settings.ServiceProvider.GetRequiredService<IContextFunctionService>().Load(group, name);
		if (CSharpCompilerHelper.TryCompile("CF_" + name, contextFunctionDefinition.Script, loadAssembly: true, out var assembly, out var errorDiagnostics))
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeFromHandle.IsAssignableFrom(type))
				{
					_cfByTypes.Add(name, type);
					return type;
				}
			}
			throw new InvalidOperationException($"The class derived from {typeFromHandle.Name} not found for CF {name}.");
		}
		Log.ForContext(typeof(DefinitionMappingsService)).Error(string.Join("\r\n", errorDiagnostics));
		throw new InvalidOperationException("CF " + name + " C# compilation error.");
	}
}
