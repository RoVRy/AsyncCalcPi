using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace AsynchCalcPi {
  /// <summary>
  /// Summary description for Form1.
  /// </summary>
  public class Form1 : System.Windows.Forms.Form {
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button _calcButton;
    private System.Windows.Forms.NumericUpDown _digits;
    private System.Windows.Forms.TextBox _pi;
    private System.Windows.Forms.ProgressBar _piProgress;
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.Container components = null;

    public Form1() {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      //
      // TODO: Add any constructor code after InitializeComponent call
      //
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose( bool disposing ) {
      if( disposing ) {
        if (components != null) {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }

		#region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
		this.panel1 = new System.Windows.Forms.Panel();
		this._calcButton = new System.Windows.Forms.Button();
		this._digits = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this._pi = new System.Windows.Forms.TextBox();
		this._piProgress = new System.Windows.Forms.ProgressBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)(this._digits)).BeginInit();
		this.SuspendLayout();
		// 
		// panel1
		// 
		this.panel1.Controls.Add(this._calcButton);
		this.panel1.Controls.Add(this._digits);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(232, 40);
		this.panel1.TabIndex = 0;
		// 
		// _calcButton
		// 
		this._calcButton.Location = new System.Drawing.Point(144, 8);
		this._calcButton.Name = "_calcButton";
		this._calcButton.Size = new System.Drawing.Size(75, 23);
		this._calcButton.TabIndex = 2;
		this._calcButton.Text = "Calc";
		this._calcButton.Click += new System.EventHandler(this._calcButton_Click);
		// 
		// _digits
		// 
		this._digits.Location = new System.Drawing.Point(80, 8);
		this._digits.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
		this._digits.Name = "_digits";
		this._digits.Size = new System.Drawing.Size(56, 20);
		this._digits.TabIndex = 1;
		// 
		// label1
		// 
		this.label1.Location = new System.Drawing.Point(8, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 23);
		this.label1.TabIndex = 0;
		this.label1.Text = "Digits of Pi";
		// 
		// _pi
		// 
		this._pi.BackColor = System.Drawing.SystemColors.Window;
		this._pi.Dock = System.Windows.Forms.DockStyle.Fill;
		this._pi.Location = new System.Drawing.Point(0, 40);
		this._pi.Multiline = true;
		this._pi.Name = "_pi";
		this._pi.ReadOnly = true;
		this._pi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this._pi.Size = new System.Drawing.Size(232, 103);
		this._pi.TabIndex = 1;
		this._pi.Text = "3";
		// 
		// _piProgress
		// 
		this._piProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
		this._piProgress.Location = new System.Drawing.Point(0, 143);
		this._piProgress.Maximum = 1;
		this._piProgress.Name = "_piProgress";
		this._piProgress.Size = new System.Drawing.Size(232, 23);
		this._piProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this._piProgress.TabIndex = 2;
		// 
		// Form1
		// 
		this.AcceptButton = this._calcButton;
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(232, 166);
		this.Controls.Add(this._pi);
		this.Controls.Add(this.panel1);
		this.Controls.Add(this._piProgress);
		this.Name = "Form1";
		this.Text = "Digits of Pi";
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)(this._digits)).EndInit();
		this.ResumeLayout(false);
		this.PerformLayout();

    }
		#endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {
		Application.EnableVisualStyles();
      Application.Run(new Form1());
    }

    delegate void ShowProgressDelegate(string pi, int totalDigits, int digitsSoFar);

    void ShowProgress(string pi, int totalDigits, int digitsSoFar) {
      // Make sure we're on the right thread
      if( _pi.InvokeRequired == false ) {
        _pi.Text = pi;
        _piProgress.Maximum = totalDigits;
        _piProgress.Value = digitsSoFar;
      }
      else {
        // Show progress asynchronously
        ShowProgressDelegate  showProgress = new ShowProgressDelegate(ShowProgress);
        //Invoke(showProgress, new object[] { pi, totalDigits, digitsSoFar});
        BeginInvoke(showProgress, new object[] { pi, totalDigits, digitsSoFar});
      }
    }

    void CalcPi(int digits) {
      StringBuilder pi = new StringBuilder("3", digits + 2);

      // Show progress
      ShowProgress(pi.ToString(), digits, 0);

      if( digits > 0 ) {
        pi.Append(".");

        for( int i = 0; i < digits; i += 9 ) {
          int nineDigits = NineDigitsOfPi.StartingAt(i+1);
          int digitCount = Math.Min(digits - i, 9);
          string ds = string.Format("{0:D9}", nineDigits);
          pi.Append(ds.Substring(0, digitCount));
			

          // Show progress
          ShowProgress(pi.ToString(), digits, i + digitCount);
        }
      }
    }

    delegate void CalcPiDelegate(int digits);

    private void _calcButton_Click(object sender, System.EventArgs e) {
      // Synch method
//      CalcPi((int)_digits.Value);

      // Asynch delegate method
      CalcPiDelegate  calcPi = new CalcPiDelegate(CalcPi);
      calcPi.BeginInvoke((int)_digits.Value, null, null);
    }
  }
}














