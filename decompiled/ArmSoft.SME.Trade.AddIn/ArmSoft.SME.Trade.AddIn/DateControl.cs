using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ArmSoft.SME.Core.AddIn;

namespace ArmSoft.SME.Trade.AddIn;

public class DateControl : UserControl
{
	public delegate void ValueChangedEventHandler(object sender, EventArgs e);

	private DateTime InnerValue;

	private bool alreadyFocused;

	private bool ValidInput = true;

	private static DateTime MinDate = new DateTime(1950, 1, 1);

	private const int ShortFormatBeginYear = 50;

	private static DateTime MaxDate = new DateTime(2080, 12, 31);

	private IContainer components;

	internal DateTimePicker dtpDate;

	internal MaskedTextBox MTextBox;

	public bool isValid => ValidInput;

	public DateTime Value
	{
		get
		{
			return InnerValue;
		}
		set
		{
			if (value >= MinDate && value <= MaxDate)
			{
				InnerValue = value;
				dtpDate.Value = value;
				MTextBox.Text = DateToString(value);
			}
		}
	}

	public event ValueChangedEventHandler ValueChanged;

	private static string DateToString(DateTime value)
	{
		return value.Day.ToString("00") + "/" + value.Month.ToString("00") + "/" + ("0000" + value.Year).Right(4);
	}

	private static DateTime StringToDate(string value)
	{
		if (value.Length < 10)
		{
			value += "      ";
		}
		string text = value.Substring(6, 4).Trim();
		int num;
		if (text.Length <= 2)
		{
			num = Convert.ToInt32(text);
			num = ((num < 50) ? (num + 2000) : (num + 1900));
		}
		else
		{
			num = Convert.ToInt32(text);
		}
		int month = Convert.ToInt32(value.Substring(3, 2));
		int day = Convert.ToInt32(value.Substring(0, 2));
		return new DateTime(num, month, day);
	}

	private void MTextBox_GotFocus(object sender, EventArgs e)
	{
		if (Control.MouseButtons == MouseButtons.None)
		{
			MTextBox.SelectAll();
			alreadyFocused = true;
		}
	}

	private void MTextBox_MouseDown(object sender, MouseEventArgs e)
	{
		if (!alreadyFocused && MTextBox.SelectionLength == 0)
		{
			MTextBox.SelectAll();
			alreadyFocused = true;
		}
	}

	private void MTextBox_Leave(object sender, EventArgs e)
	{
		SyncFromMask();
		if (ValidInput && DateToString(InnerValue) != MTextBox.Text)
		{
			MTextBox.Text = DateToString(InnerValue);
		}
		alreadyFocused = false;
	}

	private void dtpDate_CloseUp(object sender, EventArgs e)
	{
		MTextBox.Focus();
	}

	private void dtpDate_ValueChanged(object sender, EventArgs e)
	{
		ValueChanged?.Invoke(sender, e);
	}

	private void dtpDate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			MTextBox.Text = DateToString(dtpDate.Value);
			InnerValue = dtpDate.Value;
			MTextBox.ForeColor = Color.Black;
			ValidInput = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void MTextBox_TextChanged(object sender, EventArgs e)
	{
		SyncFromMask();
	}

	private void MTextBox_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Next || (e.KeyCode == Keys.Down && Control.ModifierKeys == Keys.Control))
		{
			dtpDate.Focus();
			SendKeys.Send("{F4}");
		}
		else if (e.KeyCode == Keys.Down)
		{
			int selectionStart = MTextBox.SelectionStart;
			if (selectionStart >= 0 && selectionStart <= 2)
			{
				if (ShiftDate(1, increment: false))
				{
					MTextBox.SelectionStart = 1;
					MTextBox.Select(MTextBox.SelectionStart - 1, 2);
				}
			}
			else if (selectionStart >= 3 && selectionStart <= 5)
			{
				if (ShiftDate(2, increment: false))
				{
					MTextBox.SelectionStart = 4;
					MTextBox.Select(MTextBox.SelectionStart - 1, 2);
				}
			}
			else if (selectionStart > 5 && ShiftDate(3, increment: false))
			{
				MTextBox.SelectionStart = 9;
				MTextBox.Select(MTextBox.SelectionStart - 1, 2);
			}
			e.Handled = true;
		}
		else if (e.KeyCode == Keys.Up)
		{
			int selectionStart2 = MTextBox.SelectionStart;
			if (selectionStart2 >= 0 && selectionStart2 <= 2)
			{
				if (ShiftDate(1, increment: true))
				{
					MTextBox.SelectionStart = 1;
					MTextBox.Select(MTextBox.SelectionStart - 1, 2);
				}
			}
			else if (selectionStart2 >= 3 && selectionStart2 <= 5)
			{
				if (ShiftDate(2, increment: true))
				{
					MTextBox.SelectionStart = 4;
					MTextBox.Select(MTextBox.SelectionStart - 1, 2);
				}
			}
			else if (selectionStart2 > 5 && ShiftDate(3, increment: true))
			{
				MTextBox.SelectionStart = 9;
				MTextBox.Select(MTextBox.SelectionStart - 1, 2);
			}
			e.Handled = true;
		}
		else if (e.KeyCode == Keys.Left && Control.ModifierKeys == Keys.Control)
		{
			if (MTextBox.SelectionStart >= 6)
			{
				MTextBox.SelectionStart = 4;
			}
			else
			{
				MTextBox.SelectionStart = 1;
			}
			MTextBox.Select(MTextBox.SelectionStart - 1, 2);
			e.Handled = true;
		}
		else if (e.KeyCode == Keys.Right && Control.ModifierKeys == Keys.Control)
		{
			if (MTextBox.SelectionStart <= 2)
			{
				MTextBox.SelectionStart = 4;
			}
			else
			{
				MTextBox.SelectionStart = 9;
			}
			MTextBox.Select(MTextBox.SelectionStart - 1, 2);
			e.Handled = true;
		}
	}

	private bool ShiftDate(int unit, bool increment)
	{
		if (ValidInput)
		{
			DateTime innerValue = InnerValue;
			DateTime dateTime;
			switch (unit)
			{
			case 1:
				dateTime = innerValue.AddDays(increment ? 1 : (-1));
				break;
			case 2:
				dateTime = innerValue.AddMonths(increment ? 1 : (-1));
				break;
			case 3:
				dateTime = innerValue.AddYears(increment ? 1 : (-1));
				break;
			default:
				return false;
			}
			if (dateTime < MinDate)
			{
				dateTime = MinDate;
			}
			else if (dateTime > MaxDate)
			{
				dateTime = MaxDate;
			}
			Value = dateTime;
			return true;
		}
		return false;
	}

	private void SyncFromMask()
	{
		try
		{
			DateTime value = (InnerValue = StringToDate(MTextBox.Text));
			dtpDate.Value = value;
			MTextBox.ForeColor = Color.Black;
			ValidInput = true;
		}
		catch (Exception)
		{
			MTextBox.ForeColor = Color.Red;
			ValidInput = false;
		}
	}

	public DateControl()
	{
		InitializeComponent();
		dtpDate.MinDate = MinDate;
		dtpDate.MaxDate = MaxDate;
		Value = DateTime.Today;
		Validating += DateControl_Validating;
		dtpDate.CloseUp += dtpDate_CloseUp;
		dtpDate.ValueChanged += dtpDate_ValueChanged;
		dtpDate.TextChanged += dtpDate_TextChanged;
		MTextBox.GotFocus += MTextBox_GotFocus;
		MTextBox.MouseDown += MTextBox_MouseDown;
		MTextBox.Leave += MTextBox_Leave;
		MTextBox.TextChanged += MTextBox_TextChanged;
		MTextBox.KeyDown += MTextBox_KeyDown;
	}

	private void DateControl_Validating(object sender, CancelEventArgs e)
	{
		if (!ValidInput)
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
		this.dtpDate = new System.Windows.Forms.DateTimePicker();
		this.MTextBox = new System.Windows.Forms.MaskedTextBox();
		base.SuspendLayout();
		this.dtpDate.CalendarFont = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpDate.CustomFormat = "";
		this.dtpDate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.dtpDate.Location = new System.Drawing.Point(75, 0);
		this.dtpDate.Margin = new System.Windows.Forms.Padding(0);
		this.dtpDate.Name = "dtpDate";
		this.dtpDate.Size = new System.Drawing.Size(19, 23);
		this.dtpDate.TabIndex = 3;
		this.dtpDate.TabStop = false;
		this.MTextBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.MTextBox.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
		this.MTextBox.Location = new System.Drawing.Point(0, 0);
		this.MTextBox.Margin = new System.Windows.Forms.Padding(0);
		this.MTextBox.Mask = "00/00/0000";
		this.MTextBox.Name = "MTextBox";
		this.MTextBox.PromptChar = ' ';
		this.MTextBox.Size = new System.Drawing.Size(74, 23);
		this.MTextBox.TabIndex = 2;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.dtpDate);
		base.Controls.Add(this.MTextBox);
		this.Font = new System.Drawing.Font("Tahoma", 9.75f);
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "DateControl";
		base.Size = new System.Drawing.Size(97, 25);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
