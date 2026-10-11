using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ArmSoft.TaxService.Enums;
using ArmSoft.TaxService.Models.XML;
using ArmSoft.TaxService.Models.XML.AccDoc;

namespace ArmSoft.TaxService;

public static class XMLExport
{
	public static void CreateInvoiceXMLFile(List<Invoice> models, string fileFullName, bool append)
	{
		if (append)
		{
			AppendInvoiceXMLFile(models, fileFullName);
		}
		else
		{
			CreateInvoiceXMLFile(models, fileFullName);
		}
	}

	public static void CreateAccDocXMLFile(List<AccountingDocument> models, string fileFullName, bool append)
	{
		if (append)
		{
			AppendAccDocXMLFile(models, fileFullName);
		}
		else
		{
			CreateAccDocXMLFile(models, fileFullName);
		}
	}

	private static void AppendInvoiceXMLFile(List<Invoice> models, string fileFullName)
	{
		if (File.Exists(fileFullName))
		{
			ExportedData invoiceFromXMLFile = GetInvoiceFromXMLFile(fileFullName);
			int num = invoiceFromXMLFile.Items?.Length ?? 0;
			object[] array = new object[num + models.Count];
			if (num > 0)
			{
				Array.Copy(invoiceFromXMLFile.Items, array, num);
			}
			for (int i = 0; i < models.Count; i++)
			{
				array[num + i] = models[i];
			}
			invoiceFromXMLFile.Items = array;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
			using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
			xmlSerializer.Serialize(textWriter, invoiceFromXMLFile);
			return;
		}
		CreateInvoiceXMLFile(models, fileFullName);
	}

	public static void CreateInvoiceXMLFile(List<Invoice> models, string fileFullName)
	{
		ExportedData exportedData = new ExportedData();
		exportedData.Items = new object[models.Count];
		for (int i = 0; i < models.Count; i++)
		{
			exportedData.Items[i] = models[i];
		}
		XmlSerializer xmlSerializer = new XmlSerializer(exportedData.GetType());
		using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
		using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(textWriter, exportedData);
	}

	private static void AppendAccDocXMLFile(List<AccountingDocument> models, string fileFullName)
	{
		if (File.Exists(fileFullName))
		{
			ExportedAccDocData accDocFromXMLFile = GetAccDocFromXMLFile(fileFullName);
			int num = accDocFromXMLFile.Items?.Length ?? 0;
			object[] array = new object[num + models.Count];
			if (num > 0)
			{
				Array.Copy(accDocFromXMLFile.Items, array, num);
			}
			for (int i = 0; i < models.Count; i++)
			{
				array[num + i] = models[i];
			}
			accDocFromXMLFile.Items = array;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedAccDocData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
			using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
			xmlSerializer.Serialize(textWriter, accDocFromXMLFile);
			return;
		}
		CreateAccDocXMLFile(models, fileFullName);
	}

	public static void CreateAccDocXMLFile(List<AccountingDocument> models, string fileFullName)
	{
		ExportedAccDocData exportedAccDocData = new ExportedAccDocData();
		exportedAccDocData.Items = new object[models.Count];
		for (int i = 0; i < models.Count; i++)
		{
			exportedAccDocData.Items[i] = models[i];
		}
		XmlSerializer xmlSerializer = new XmlSerializer(exportedAccDocData.GetType());
		using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
		using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
		xmlSerializer.Serialize(textWriter, exportedAccDocData);
	}

	public static void CreateInvoiceXMLFile(Invoice model, string fileFullName, bool append)
	{
		if (append)
		{
			AppendInvoiceXMLFile(model, fileFullName);
			return;
		}
		CreateInvoiceXMLFile(new List<Invoice> { model }, fileFullName);
	}

	public static void CreateAccDocXMLFile(AccountingDocument model, string fileFullName, bool append)
	{
		if (append)
		{
			AppendAccDocXMLFile(model, fileFullName);
			return;
		}
		CreateAccDocXMLFile(new List<AccountingDocument> { model }, fileFullName);
	}

	private static void AppendInvoiceXMLFile(Invoice model, string fileFullName)
	{
		if (File.Exists(fileFullName))
		{
			ExportedData invoiceFromXMLFile = GetInvoiceFromXMLFile(fileFullName);
			int num = invoiceFromXMLFile.Items.Length;
			object[] array = new object[num];
			for (int i = 0; i < invoiceFromXMLFile.Items.Length; i++)
			{
				array[i] = invoiceFromXMLFile.Items[i];
			}
			invoiceFromXMLFile.Items = new object[num + 1];
			for (int j = 0; j < array.Length; j++)
			{
				invoiceFromXMLFile.Items[j] = array[j];
			}
			invoiceFromXMLFile.Items[num] = model;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
			using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
			xmlSerializer.Serialize(textWriter, invoiceFromXMLFile);
			return;
		}
		CreateInvoiceXMLFile(new List<Invoice> { model }, fileFullName);
	}

	private static void AppendAccDocXMLFile(AccountingDocument model, string fileFullName)
	{
		if (File.Exists(fileFullName))
		{
			ExportedAccDocData accDocFromXMLFile = GetAccDocFromXMLFile(fileFullName);
			int num = accDocFromXMLFile.Items.Length;
			object[] array = new object[num];
			for (int i = 0; i < accDocFromXMLFile.Items.Length; i++)
			{
				array[i] = accDocFromXMLFile.Items[i];
			}
			accDocFromXMLFile.Items = new object[num + 1];
			for (int j = 0; j < array.Length; j++)
			{
				accDocFromXMLFile.Items[j] = array[j];
			}
			accDocFromXMLFile.Items[num] = model;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedAccDocData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Create);
			using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8);
			xmlSerializer.Serialize(textWriter, accDocFromXMLFile);
			return;
		}
		CreateAccDocXMLFile(new List<AccountingDocument> { model }, fileFullName);
	}

	public static ExportedData GetInvoiceFromXMLFile(string fileFullName)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Open, FileAccess.Read);
			return (ExportedData)xmlSerializer.Deserialize(stream);
		}
		catch (InvalidOperationException)
		{
			throw new ApiException(Resources.Get(ResourceKey.InvoiceFormatIsIncorrect));
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static ExportedAccDocData GetAccDocFromXMLFile(string fileFullName)
	{
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportedAccDocData));
			using FileStream stream = CreateFileStream(fileFullName, FileMode.Open, FileAccess.Read);
			return (ExportedAccDocData)xmlSerializer.Deserialize(stream);
		}
		catch (InvalidOperationException)
		{
			throw new ApiException(Resources.Get(ResourceKey.AccDocFormatIsIncorrect));
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static TaxDocType GetXMLTaxType(string fileFullName)
	{
		using (XmlReader xmlReader = CreateXmlReader(fileFullName))
		{
			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element)
				{
					if (xmlReader.Name == "ExportedData")
					{
						return TaxDocType.Invoice;
					}
					if (xmlReader.Name == "ExportedAccDocData")
					{
						return TaxDocType.AccDoc;
					}
				}
			}
		}
		return TaxDocType.None;
	}

	public static FileStream CreateFileStream(string fileFullName, FileMode fileMode, FileAccess fileAccess = FileAccess.Write)
	{
		try
		{
			return new FileStream(fileFullName, fileMode, fileAccess);
		}
		catch (FileNotFoundException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.FileDoesNotExist), fileFullName));
		}
		catch (IOException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (ArgumentException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (NotSupportedException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (UnauthorizedAccessException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
	}

	private static XmlReader CreateXmlReader(string fileFullName)
	{
		try
		{
			return XmlReader.Create(fileFullName);
		}
		catch (IOException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (ArgumentException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (NotSupportedException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
		catch (UnauthorizedAccessException)
		{
			throw new ApiException(string.Format(Resources.Get(ResourceKey.IncorrectDirectoryPath), fileFullName));
		}
	}
}
