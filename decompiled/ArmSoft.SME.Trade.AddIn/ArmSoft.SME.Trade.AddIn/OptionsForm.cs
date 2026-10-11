using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;
using Microsoft.Win32;

namespace ArmSoft.SME.Trade.AddIn;

public class OptionsForm : Form
{
	private const string _lang = "Lang";

	private IContainer components;

	internal Label LblCurrentDate;

	internal Label LblPeriod;

	internal Label LblLang;

	internal ComboBox cmbLang;

	internal Label LblDivider;

	internal Button ButtonOk;

	internal Button ButtonCancel;

	internal Label lblPrecision;

	internal MaskedTextBox txtDivider;

	internal MaskedTextBox txtPrecision;

	internal DateControl DTPCurrentDate;

	internal DateControl DTPPeriodBegin;

	internal DateControl DTPPeriodEnd;

	public bool CalculationMode { get; set; }

	public OptionsForm()
	{
		InitializeComponent();
		Activated += (object sender, EventArgs e) =>
		{
			OnActivated(e);
		};
		Load += (object sender, EventArgs e) =>
		{
			OnLoad(e);
		};
		KeyDown += (object sender, KeyEventArgs e) =>
		{
			OnKeyDown(e);
		};
		ButtonOk.Click += ButtonOk_Click;
		cmbLang.KeyDown += CmbLang_KeyDown;
		DTPCurrentDate.KeyDown += DTPCurrentDate_KeyDown;
		DTPPeriodBegin.KeyDown += DTPPeriodBegin_KeyDown;
		DTPPeriodEnd.KeyDown += DTPPeriodEnd_KeyDown;
		txtDivider.Validating += TxtDivider_Validating;
		txtPrecision.Validating += TxtPrecision_Validating;
	}

	protected override void OnActivated(EventArgs e)
	{
		LblDivider.Text = ArmSoft.SME.Core.AddIn.Resources.Accuracy;
		lblPrecision.Text = ArmSoft.SME.Core.AddIn.Resources.DegreeOfRound;
	}

	protected override void OnLoad(EventArgs e)
	{
		if (!CalculationMode)
		{
			Text = ArmSoft.SME.Core.AddIn.Resources.Options;
		}
		else
		{
			Text = ArmSoft.SME.Core.AddIn.Resources.Calculation;
		}
		ButtonOk.Text = ArmSoft.SME.Core.AddIn.Resources.Ok;
		ButtonCancel.Text = ArmSoft.SME.Core.AddIn.Resources.Cancel;
		LblCurrentDate.Text = ArmSoft.SME.Core.AddIn.Resources.CurrentDate;
		LblPeriod.Text = ArmSoft.SME.Core.AddIn.Resources.ReportPeriod;
		LblLang.Text = ArmSoft.SME.Core.AddIn.Resources.OptionsLang;
		LblDivider.Text = ArmSoft.SME.Core.AddIn.Resources.Accuracy;
		lblPrecision.Text = ArmSoft.SME.Core.AddIn.Resources.DegreeOfRound;
		DTPPeriodBegin.Value = TradeSettings.gDateBegin;
		DTPPeriodEnd.Value = TradeSettings.gDateEnd;
		DTPCurrentDate.Value = TradeSettings.gCurrentDate;
		cmbLang.Items.Add(ArmSoft.SME.Core.AddIn.Resources.Armenian);
		cmbLang.Items.Add(ArmSoft.SME.Core.AddIn.Resources.English);
		if (TradeSettings.gLang == 1)
		{
			cmbLang.SelectedIndex = 0;
		}
		else if (TradeSettings.gLang == 2)
		{
			cmbLang.SelectedIndex = 1;
		}
		else
		{
			cmbLang.SelectedIndex = 0;
		}
		txtDivider.Text = TradeSettings.gDivider.ToString();
		txtPrecision.Text = TradeSettings.gPrecision.ToString();
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		StoreOptions();
	}

	public void StoreOptions()
	{
		try
		{
			TradeSettings.gDateBegin = DTPPeriodBegin.Value;
			TradeSettings.gDateEnd = DTPPeriodEnd.Value;
			TradeSettings.gCurrentDate = DTPCurrentDate.Value;
			if (TradeSettings.gDateBegin > TradeSettings.gDateEnd)
			{
				throw new Exception(ArmSoft.SME.Core.AddIn.Resources.ValidateWrongPeriod);
			}
			int gLang = TradeSettings.gLang;
			if (cmbLang.SelectedIndex == 0)
			{
				TradeSettings.gLang = 1;
			}
			else
			{
				TradeSettings.gLang = 2;
			}
			if (gLang != TradeSettings.gLang)
			{
				using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(Settings.ExcelAddInRegistryKey, writable: true);
				registryKey.SetValue("Lang", TradeSettings.gLang.ToString());
			}
			TradeSettings.SetCurrentThreadCulture();
			if (int.TryParse(txtDivider.Text, out var result) && result > 0)
			{
				TradeSettings.gDivider = result;
			}
			else
			{
				TradeSettings.gDivider = 1;
			}
			if (int.TryParse(txtPrecision.Text, out result))
			{
				TradeSettings.gPrecision = result;
			}
			else
			{
				TradeSettings.gPrecision = 2;
			}
			DialogResult = DialogResult.OK;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, TradeSettings.ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbLang_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Next)
		{
			cmbLang.DroppedDown = true;
		}
	}

	protected override void OnKeyDown(KeyEventArgs e)
	{
		if (e.Control & (e.KeyCode == Keys.Return))
		{
			StoreOptions();
			e.Handled = true;
		}
		else if (e.KeyCode == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
	}

	private void DTPCurrentDate_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Next)
		{
			SendKeys.Send("{F4}");
		}
	}

	private void DTPPeriodBegin_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Next)
		{
			SendKeys.Send("{F4}");
		}
	}

	private void DTPPeriodEnd_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Next)
		{
			SendKeys.Send("{F4}");
		}
	}

	private void TxtDivider_Validating(object sender, CancelEventArgs e)
	{
		if (!int.TryParse(txtDivider.Text, out var result) || result == 0)
		{
			e.Cancel = true;
		}
	}

	private void TxtPrecision_Validating(object sender, CancelEventArgs e)
	{
		if (!int.TryParse(txtPrecision.Text, out var _))
		{
			e.Cancel = true;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.LblCurrentDate = new System.Windows.Forms.Label();
		this.LblPeriod = new System.Windows.Forms.Label();
		this.LblLang = new System.Windows.Forms.Label();
		this.cmbLang = new System.Windows.Forms.ComboBox();
		this.LblDivider = new System.Windows.Forms.Label();
		this.ButtonOk = new System.Windows.Forms.Button();
		this.ButtonCancel = new System.Windows.Forms.Button();
		this.lblPrecision = new System.Windows.Forms.Label();
		this.txtDivider = new System.Windows.Forms.MaskedTextBox();
		this.txtPrecision = new System.Windows.Forms.MaskedTextBox();
		this.DTPCurrentDate = new ArmSoft.SME.Trade.AddIn.DateControl();
		this.DTPPeriodBegin = new ArmSoft.SME.Trade.AddIn.DateControl();
		this.DTPPeriodEnd = new ArmSoft.SME.Trade.AddIn.DateControl();
		base.SuspendLayout();
		this.LblCurrentDate.AutoSize = true;
		this.LblCurrentDate.Location = new System.Drawing.Point(14, 10);
		this.LblCurrentDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.LblCurrentDate.Name = "LblCurrentDate";
		this.LblCurrentDate.Size = new System.Drawing.Size(79, 16);
		this.LblCurrentDate.TabIndex = 12;
		this.LblCurrentDate.Text = "Current date";
		this.LblPeriod.AutoSize = true;
		this.LblPeriod.Location = new System.Drawing.Point(14, 49);
		this.LblPeriod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.LblPeriod.Name = "LblPeriod";
		this.LblPeriod.Size = new System.Drawing.Size(43, 16);
		this.LblPeriod.TabIndex = 13;
		this.LblPeriod.Text = "Period";
		this.LblLang.AutoSize = true;
		this.LblLang.Location = new System.Drawing.Point(14, 89);
		this.LblLang.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.LblLang.Name = "LblLang";
		this.LblLang.Size = new System.Drawing.Size(34, 16);
		this.LblLang.TabIndex = 14;
		this.LblLang.Text = "Lang";
		this.cmbLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbLang.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.cmbLang.FormattingEnabled = true;
		this.cmbLang.Location = new System.Drawing.Point(271, 86);
		this.cmbLang.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.cmbLang.MaxDropDownItems = 5;
		this.cmbLang.Name = "cmbLang";
		this.cmbLang.Size = new System.Drawing.Size(207, 24);
		this.cmbLang.TabIndex = 15;
		this.LblDivider.AutoSize = true;
		this.LblDivider.Location = new System.Drawing.Point(14, 128);
		this.LblDivider.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.LblDivider.Name = "LblDivider";
		this.LblDivider.Size = new System.Drawing.Size(59, 16);
		this.LblDivider.TabIndex = 17;
		this.LblDivider.Text = "lblDivider";
		this.ButtonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.ButtonOk.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.ButtonOk.Location = new System.Drawing.Point(100, 208);
		this.ButtonOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.ButtonOk.Name = "ButtonOk";
		this.ButtonOk.Size = new System.Drawing.Size(136, 32);
		this.ButtonOk.TabIndex = 19;
		this.ButtonOk.Text = "OK";
		this.ButtonOk.UseVisualStyleBackColor = true;
		this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.ButtonCancel.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.ButtonCancel.Location = new System.Drawing.Point(295, 208);
		this.ButtonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.ButtonCancel.Name = "ButtonCancel";
		this.ButtonCancel.Size = new System.Drawing.Size(133, 32);
		this.ButtonCancel.TabIndex = 20;
		this.ButtonCancel.Text = "Cancel";
		this.ButtonCancel.UseVisualStyleBackColor = true;
		this.lblPrecision.AutoSize = true;
		this.lblPrecision.Location = new System.Drawing.Point(14, 169);
		this.lblPrecision.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPrecision.Name = "lblPrecision";
		this.lblPrecision.Size = new System.Drawing.Size(58, 16);
		this.lblPrecision.TabIndex = 21;
		this.lblPrecision.Text = "Precision";
		this.txtDivider.Location = new System.Drawing.Point(271, 126);
		this.txtDivider.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtDivider.Name = "txtDivider";
		this.txtDivider.ResetOnSpace = false;
		this.txtDivider.Size = new System.Drawing.Size(116, 23);
		this.txtDivider.TabIndex = 16;
		this.txtDivider.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDivider.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
		this.txtPrecision.Location = new System.Drawing.Point(270, 165);
		this.txtPrecision.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.txtPrecision.Mask = "0";
		this.txtPrecision.Name = "txtPrecision";
		this.txtPrecision.Size = new System.Drawing.Size(42, 23);
		this.txtPrecision.TabIndex = 18;
		this.txtPrecision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.DTPCurrentDate.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.DTPCurrentDate.Location = new System.Drawing.Point(271, 10);
		this.DTPCurrentDate.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.DTPCurrentDate.Name = "DTPCurrentDate";
		this.DTPCurrentDate.Size = new System.Drawing.Size(113, 31);
		this.DTPCurrentDate.TabIndex = 22;
		this.DTPCurrentDate.Value = new System.DateTime(2024, 8, 9, 0, 0, 0, 0);
		this.DTPPeriodBegin.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.DTPPeriodBegin.Location = new System.Drawing.Point(271, 50);
		this.DTPPeriodBegin.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.DTPPeriodBegin.Name = "DTPPeriodBegin";
		this.DTPPeriodBegin.Size = new System.Drawing.Size(113, 31);
		this.DTPPeriodBegin.TabIndex = 23;
		this.DTPPeriodBegin.Value = new System.DateTime(2024, 8, 9, 0, 0, 0, 0);
		this.DTPPeriodEnd.Font = new System.Drawing.Font("Tahoma", 9.75f);
		this.DTPPeriodEnd.Location = new System.Drawing.Point(393, 50);
		this.DTPPeriodEnd.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
		this.DTPPeriodEnd.Name = "DTPPeriodEnd";
		this.DTPPeriodEnd.Size = new System.Drawing.Size(113, 31);
		this.DTPPeriodEnd.TabIndex = 24;
		this.DTPPeriodEnd.Value = new System.DateTime(2024, 8, 9, 0, 0, 0, 0);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.ButtonCancel;
		base.ClientSize = new System.Drawing.Size(558, 251);
		base.Controls.Add(this.DTPPeriodEnd);
		base.Controls.Add(this.DTPPeriodBegin);
		base.Controls.Add(this.DTPCurrentDate);
		base.Controls.Add(this.txtPrecision);
		base.Controls.Add(this.txtDivider);
		base.Controls.Add(this.lblPrecision);
		base.Controls.Add(this.ButtonCancel);
		base.Controls.Add(this.ButtonOk);
		base.Controls.Add(this.LblDivider);
		base.Controls.Add(this.cmbLang);
		base.Controls.Add(this.LblLang);
		base.Controls.Add(this.LblPeriod);
		base.Controls.Add(this.LblCurrentDate);
		this.Font = new System.Drawing.Font("Tahoma", 9.75f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "OptionsForm";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "OptionsForm";
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
