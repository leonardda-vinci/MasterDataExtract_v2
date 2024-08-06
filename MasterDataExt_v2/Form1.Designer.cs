using System.Windows.Forms;

namespace MasterDataExt_v2
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			logTxtBox = new System.Windows.Forms.TextBox();
			backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// logTxtBox
			// 
			logTxtBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			logTxtBox.Cursor = System.Windows.Forms.Cursors.Default;
			logTxtBox.Font = new System.Drawing.Font("Segoe UI", 9F);
			logTxtBox.ForeColor = System.Drawing.SystemColors.WindowText;
			logTxtBox.Location = new System.Drawing.Point(12, 12);
			logTxtBox.Multiline = true;
			logTxtBox.Name = "logTxtBox";
			logTxtBox.ReadOnly = true;
			logTxtBox.Size = new System.Drawing.Size(525, 137);
			logTxtBox.TabIndex = 0;
			// 
			// backgroundWorker2
			// 
			backgroundWorker2.WorkerSupportsCancellation = true;
			backgroundWorker2.DoWork += BackgroundWorker2_DoWork;
			backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker2_DoWork);
			// 
			// Main
			// 
			this.ClientSize = new System.Drawing.Size(549, 161);
			this.Controls.Add(logTxtBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Main";
			this.Text = "Master Data Extract";
			this.Load += new System.EventHandler(this.Main_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void BackgroundWorker2_DoWork1(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		private void BackgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			throw new System.NotImplementedException();
		}

		/*private void BackgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			throw new System.NotImplementedException();
		}*/

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private static System.Windows.Forms.TextBox logTxtBox;
		private static System.ComponentModel.BackgroundWorker backgroundWorker2;
	}
}

