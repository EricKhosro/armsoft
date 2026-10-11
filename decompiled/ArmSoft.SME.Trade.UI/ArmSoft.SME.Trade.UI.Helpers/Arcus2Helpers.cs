using System;
using ArmSoft.Arcus2Client;
using ArmSoft.SME.Core.UI;
using ArmSoft.SME.Trade.Common;
using ArmSoft.SME.Trade.UI.Models;

namespace ArmSoft.SME.Trade.UI.Helpers;

internal static class Arcus2Helpers
{
	internal static PosTerminalResponse CheckAndPurchase(string arcusId, decimal amount)
	{
		TerminalResponse terminalResponse;
		try
		{
			terminalResponse = Purchase(arcusId, amount);
		}
		catch (Arcus2Exception)
		{
			throw;
		}
		return new PosTerminalResponse(terminalResponse.RRN, terminalResponse.TerminalId);
	}

	internal static PosTerminalResponse CheckAndRefund(string arcusId, decimal amount, decimal originalAmount, string rrn)
	{
		TerminalResponse terminalResponse;
		try
		{
			terminalResponse = Refund(arcusId, amount, originalAmount, rrn);
		}
		catch (Arcus2Exception)
		{
			throw;
		}
		return new PosTerminalResponse(terminalResponse.RRN, terminalResponse.TerminalId);
	}

	internal static void Cancel(string arcusId, decimal amount, string rrn)
	{
		try
		{
			using ArmSoft.Arcus2Client.Arcus2Client arcus2Client = new ArmSoft.Arcus2Client.Arcus2Client(arcusId);
			arcus2Client.Cancel(amount, rrn);
		}
		catch (Arcus2Exception)
		{
			throw;
		}
	}

	private static TerminalResponse Refund(string arcusId, decimal amount, decimal originalAmount, string rrn)
	{
		try
		{
			using ArmSoft.Arcus2Client.Arcus2Client arcus2Client = new ArmSoft.Arcus2Client.Arcus2Client(arcusId);
			return arcus2Client.Refund(amount, originalAmount, rrn);
		}
		catch (BadImageFormatException)
		{
			throw new ASException(Environment.Is64BitProcess ? Resources.Requiredx32ClientForArcus2 : Resources.Requiredx64ClientForArcus2);
		}
	}

	private static TerminalResponse Purchase(string arcusId, decimal amount)
	{
		try
		{
			using ArmSoft.Arcus2Client.Arcus2Client arcus2Client = new ArmSoft.Arcus2Client.Arcus2Client(arcusId);
			return arcus2Client.Purchase(amount);
		}
		catch (BadImageFormatException)
		{
			throw new ASException(Environment.Is64BitProcess ? Resources.Requiredx32ClientForArcus2 : Resources.Requiredx64ClientForArcus2);
		}
	}
}
