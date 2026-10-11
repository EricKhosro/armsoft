using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;
using System.Xml.Serialization;
using Ardalis.SmartEnum;
using ArmSoft.SME.Client;
using ArmSoft.SME.Client.Data;
using ArmSoft.SME.Common;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Serilog;

namespace ArmSoft.SME.Core.UI.MetaData;

public static class CSharpCompilerHelper
{
	public static bool TryCompile(string logicalName, string source, bool loadAssembly, out Assembly assembly, out IEnumerable<Diagnostic> errorDiagnostics)
	{
		assembly = null;
		errorDiagnostics = null;
		IEnumerable<MetadataReference> assemblyReferences = GetAssemblyReferences();
		string randomFileName = Path.GetRandomFileName();
		OptimizationLevel value = OptimizationLevel.Release;
		SyntaxTree item = CSharpSyntaxTree.ParseText(source, CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
		CSharpCompilation cSharpCompilation = CSharpCompilation.Create(randomFileName, new _003C_003Ez__ReadOnlySingleElementList<SyntaxTree>(item), assemblyReferences, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithOptimizationLevel(value).WithPlatform(Platform.AnyCpu));
		errorDiagnostics = from x in cSharpCompilation.GetDiagnostics()
			where x.Severity == DiagnosticSeverity.Error
			select x;
		if (errorDiagnostics.Any())
		{
			return false;
		}
		if (loadAssembly)
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				EmitResult emitResult = cSharpCompilation.Emit(memoryStream);
				if (emitResult.Success)
				{
					memoryStream.Seek(0L, SeekOrigin.Begin);
					assembly = AssemblyLoadContext.Default.LoadFromStream(memoryStream);
					return true;
				}
				errorDiagnostics = emitResult.Diagnostics.Where((Diagnostic x) => x.Severity == DiagnosticSeverity.Error);
			}
			return false;
		}
		return true;
	}

	internal static IEnumerable<MetadataReference> GetAssemblyReferences()
	{
		return (from p in GetAssemblyPaths()
			select MetadataReference.CreateFromFile(p)).ToList();
	}

	internal static List<string> GetAssemblyPaths()
	{
		HashSet<string> hashSet = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator).ToHashSet(StringComparer.InvariantCultureIgnoreCase);
		hashSet.Add(typeof(object).Assembly.Location);
		hashSet.Add(typeof(TypeConverter).Assembly.Location);
		hashSet.Add(typeof(IServiceProvider).Assembly.Location);
		hashSet.Add(typeof(IXmlSerializable).Assembly.Location);
		hashSet.Add(typeof(DbParameter).Assembly.Location);
		hashSet.Add(typeof(HashSet<>).Assembly.Location);
		hashSet.Add(typeof(ConcurrentStack<>).Assembly.Location);
		hashSet.Add(typeof(Component).Assembly.Location);
		hashSet.Add(typeof(Log).Assembly.Location);
		hashSet.Add(typeof(Enumerable).Assembly.Location);
		hashSet.Add(typeof(SmartEnum<>).Assembly.Location);
		hashSet.Add(typeof(JsonElement).Assembly.Location);
		hashSet.Add(typeof(JavaScriptEncoder).Assembly.Location);
		hashSet.Add(typeof(DataProvider).Assembly.Location);
		hashSet.Add(typeof(AsyncHelper).Assembly.Location);
		hashSet.Add(typeof(DialogWindow).Assembly.Location);
		hashSet.Add(typeof(MessageBoxResult).Assembly.Location);
		string text = null;
		string directoryName = Path.GetDirectoryName(typeof(DialogWindow).Assembly.Location);
		if (Settings.SystemType == SystemTypes.Trade)
		{
			text = "ArmSoft.SME.Trade.UI.dll";
		}
		else if (Settings.SystemType == SystemTypes.Accountant)
		{
			text = "ArmSoft.SME.Accountant.UI.dll";
		}
		else if (Settings.SystemType == SystemTypes.SalesManagement)
		{
			text = "ArmSoft.SME.SM.UI.dll";
		}
		if (text != null)
		{
			hashSet.Add(Path.Combine(directoryName, text));
		}
		return hashSet.ToList();
	}
}
