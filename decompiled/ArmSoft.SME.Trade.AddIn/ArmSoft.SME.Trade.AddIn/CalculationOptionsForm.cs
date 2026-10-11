using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;

namespace ArmSoft.SME.Trade.AddIn;

public class CalculationOptionsForm : Form
{
	internal RadioButton CalculateActiveSheet;

	internal RadioButton CalculateAll;

	internal Button ButtonCancel;

	internal Button ButtonOk;

	private IContainer components;

	public bool CalcAllSheets { get; set; }

	public CalculationOptionsForm()
	{
		InnerInitializeComponent();
		KeyDown += CalculationOptionsForm_KeyDown;
		Load += CalculationOptionsForm_Load;
		ButtonOk.Click += ButtonOk_Click;
	}

	private void CalculationOptionsForm_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control & (e.KeyCode == Keys.Return))
		{
			ProceesOK();
			e.Handled = true;
		}
	}

	private void CalculationOptionsForm_Load(object sender, EventArgs e)
	{
		CalculateActiveSheet.Text = ArmSoft.SME.Core.AddIn.Resources.CalculateActiveSheet;
		CalculateAll.Text = ArmSoft.SME.Core.AddIn.Resources.CalculateAllSheets;
		CalculateActiveSheet.Checked = true;
		ButtonOk.Text = ArmSoft.SME.Core.AddIn.Resources.Ok;
		ButtonCancel.Text = ArmSoft.SME.Core.AddIn.Resources.Cancel;
		Text = ArmSoft.SME.Core.AddIn.Resources.Calculation;
	}

	private void ButtonOk_Click(object sender, EventArgs e)
	{
		ProceesOK();
	}

	private void ProceesOK()
	{
		CalcAllSheets = !CalculateActiveSheet.Checked;
		DialogResult = DialogResult.OK;
	}

	private void InnerInitializeComponent()
	{
		CalculateActiveSheet = new RadioButton();
		CalculateAll = new RadioButton();
		ButtonCancel = new Button();
		ButtonOk = new Button();
		SuspendLayout();
		CalculateActiveSheet.AutoSize = true;
		CalculateActiveSheet.Checked = true;
		CalculateActiveSheet.Location = new Point(44, 12);
		CalculateActiveSheet.Name = "CalculateActiveSheet";
		CalculateActiveSheet.Size = new Size(153, 20);
		CalculateActiveSheet.TabIndex = 0;
		CalculateActiveSheet.TabStop = true;
		CalculateActiveSheet.Text = "Calculate Active Sheet";
		CalculateActiveSheet.UseVisualStyleBackColor = true;
		CalculateAll.AutoSize = true;
		CalculateAll.Location = new Point(44, 38);
		CalculateAll.Name = "CalculateAll";
		CalculateAll.Size = new Size(136, 20);
		CalculateAll.TabIndex = 1;
		CalculateAll.TabStop = true;
		CalculateAll.Text = "Calculate all sheets";
		CalculateAll.UseVisualStyleBackColor = true;
		ButtonCancel.DialogResult = DialogResult.Cancel;
		ButtonCancel.Font = new Font("Tahoma", 9.75f);
		ButtonCancel.Location = new Point(189, 64);
		ButtonCancel.Name = "ButtonCancel";
		ButtonCancel.Size = new Size(114, 26);
		ButtonCancel.TabIndex = 12;
		ButtonCancel.Text = "Cancel";
		ButtonCancel.UseVisualStyleBackColor = true;
		ButtonOk.DialogResult = DialogResult.Cancel;
		ButtonOk.Font = new Font("Tahoma", 9.75f);
		ButtonOk.Location = new Point(44, 64);
		ButtonOk.Name = "ButtonOk";
		ButtonOk.Size = new Size(117, 26);
		ButtonOk.TabIndex = 11;
		ButtonOk.Text = "OK";
		ButtonOk.UseVisualStyleBackColor = true;
		AcceptButton = ButtonOk;
		AutoScaleDimensions = new SizeF(7f, 16f);
		AutoScaleMode = AutoScaleMode.Font;
		CancelButton = ButtonCancel;
		ClientSize = new Size(357, 101);
		Controls.Add(ButtonCancel);
		Controls.Add(ButtonOk);
		Controls.Add(CalculateAll);
		Controls.Add(CalculateActiveSheet);
		Font = new Font("Tahoma", 9.75f);
		FormBorderStyle = FormBorderStyle.FixedDialog;
		KeyPreview = true;
		Margin = new Padding(3, 4, 3, 4);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "CalculationOptionsForm";
		StartPosition = FormStartPosition.CenterParent;
		Text = "Calculation Options";
		ResumeLayout(performLayout: false);
		PerformLayout();
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
		base.SuspendLayout();
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(358, 169);
		this.Font = new System.Drawing.Font("Tahoma", 8.25f);
		base.Name = "CalculationOptionsForm";
		this.Text = "CalculationOptionsForm";
		base.ResumeLayout(false);
	}
}
