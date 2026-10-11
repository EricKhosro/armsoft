using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using ArmSoft.SME.Common.Models;
using ArmSoft.SME.Core.UI.HelpClasses;
using Microsoft.Win32;

namespace ArmSoft.SME.Core.UI.Helpers;

public class CommonCoreHelper : ICommonCoreHelper
{
	private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	public string GetDirectorySafe(string path)
	{
		if (string.IsNullOrWhiteSpace(path))
		{
			return path;
		}
		if (!Path.HasExtension(path) && !path.EndsWith(Path.DirectorySeparatorChar))
		{
			path += Path.DirectorySeparatorChar;
		}
		return Path.GetDirectoryName(path);
	}

	public string GetExportDirectory()
	{
		try
		{
			using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.InitialDirectory = RegistryHelper.ReadExportDirectory();
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				RegistryHelper.WriteExportDirectory(folderBrowserDialog.SelectedPath);
				return folderBrowserDialog.SelectedPath;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		return "";
	}

	public string GetFileNameFromExportDialog(string defaultExt, string filter, string fileName, bool overwritePrompt = true)
	{
		System.Windows.Input.Cursor overrideCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = null;
		try
		{
			Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
			{
				InitialDirectory = RegistryHelper.ReadExportDirectory(),
				DefaultExt = defaultExt,
				Filter = filter,
				FileName = fileName,
				OverwritePrompt = overwritePrompt
			};
			if (saveFileDialog.ShowDialog().Value)
			{
				RegistryHelper.WriteExportDirectory(Path.GetDirectoryName(saveFileDialog.FileName));
				return saveFileDialog.FileName;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = overrideCursor;
		}
		return "";
	}

	public string GetFileNameFromImportDialog(string defaultExt, string filter, string initialDirectory = "")
	{
		System.Windows.Input.Cursor overrideCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = null;
		try
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = (string.IsNullOrWhiteSpace(initialDirectory) ? RegistryHelper.ReadImportDirectory() : initialDirectory),
				DefaultExt = defaultExt,
				Filter = filter
			};
			if (openFileDialog.ShowDialog().Value)
			{
				RegistryHelper.WriteImportDirectory(Path.GetDirectoryName(openFileDialog.FileName));
				return openFileDialog.FileName;
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = overrideCursor;
		}
		return "";
	}

	public (string FolderPath, string FileNames) GetFileNamesFromImportDialog(string defaultExt, string filter, string initialDirectory = "")
	{
		System.Windows.Input.Cursor overrideCursor = Mouse.OverrideCursor;
		Mouse.OverrideCursor = null;
		try
		{
			Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
			{
				InitialDirectory = (string.IsNullOrWhiteSpace(initialDirectory) ? RegistryHelper.ReadImportDirectory() : initialDirectory),
				DefaultExt = defaultExt,
				Filter = filter,
				Multiselect = true
			};
			if (openFileDialog.ShowDialog().Value)
			{
				string? directoryName = Path.GetDirectoryName(openFileDialog.FileName);
				RegistryHelper.WriteImportDirectory(directoryName);
				return (FolderPath: directoryName, FileNames: string.Join(" ", openFileDialog.SafeFileNames.Select((string p) => "'" + p + "'")));
			}
		}
		catch (Exception ex)
		{
			Settings.ErrorHandler(ex);
		}
		finally
		{
			Mouse.OverrideCursor = overrideCursor;
		}
		return (FolderPath: "", FileNames: "");
	}

	public void SaveXMLToFile(XmlDocument xmlDoc, string fileName)
	{
		using FileStream output = new FileStream(fileName, FileMode.Create);
		XmlWriterSettings settings = new XmlWriterSettings
		{
			Encoding = Encoding.UTF8,
			NamespaceHandling = NamespaceHandling.OmitDuplicates,
			Indent = true,
			OmitXmlDeclaration = false
		};
		XmlWriter xmlWriter = XmlWriter.Create(output, settings);
		XmlDeclaration newChild = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "no");
		xmlDoc.InsertBefore(newChild, xmlDoc.DocumentElement);
		xmlDoc.Save(xmlWriter);
		xmlWriter.Flush();
	}

	public void CopyToFile(string filename, MemoryStream ms)
	{
		using FileStream fileStream = File.OpenWrite(filename);
		ms.WriteTo(fileStream);
		fileStream.Flush();
		fileStream.Close();
	}

	public virtual void CheckWorkingPeriod(DateOnly currentDate, string controlName = null)
	{
		throw new NotImplementedException();
	}

	public virtual WorkingPeriod GetWorkingPeriod()
	{
		throw new NotImplementedException();
	}

	public string GetRandomString(int length)
	{
		Random random = new Random();
		char[] array = new char[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)];
		}
		return new string(array);
	}
}
