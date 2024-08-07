using MasterDataExtract;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterDataExt_v2
{
	public partial class Main : Form
	{
		private string path;
		// FileSystemWatcher fileWatcher;
		IniParser iniFile;
		SqlConnection Conn;

		public Main()
		{
			iniFile = new IniParser(@"C:\in\SAPconnsettings.ini");
			//path = this.iniFile.GetSetting("Filter", "DestPath");
			path = "C:\\in";

			InitializeComponent();

			logTxtBox.AppendText("Master Data Extract Running... \r\n");
		}

		private async void ProcessExtract()
		{
			string Server = this.iniFile.GetSetting("Connection", "Server");
			string DbUsername = this.iniFile.GetSetting("Connection", "DbUserName");
			string DbPassword = this.iniFile.GetSetting("Connection", "DbPassword");
			string SBOCompanyDB = this.iniFile.GetSetting("Connection", "CompanyDB");

			/*string strConn = "Data Source=" + Server + ";" +
							 "Initial Catalog=" + SBOCompanyDB + ";" +
							 "User ID=" + DbUsername + ";" +
							 "Password=" + DbPassword;*/

			string strConn = "Data Source=192.168.1.73;Initial Catalog=001_RCS_CP1_V10;User ID=sa;Password=P@ssw0rd";
			// Debug.WriteLine(strConn);

			Conn = new SqlConnection(strConn);
			logTextBox("Opening Database Connection...");
			Conn.Open();

			if (Conn.State == ConnectionState.Open)
			{
				logTxtBox.Invoke((Action)delegate
				{
					logTxtBox.Text = "Database Connection Successfully!" + "\r\n" + logTxtBox.Text;
				});
				if (this.iniFile.GetSetting("Data", "Items") == "YES")
				{
					await Task.Run(() => ProcessExtractItem(Conn));

					logTextBox("Closing Database Connection...");
					Conn.Close();
				}

				/*if (this.iniFile.GetSetting("Data", "ItemsUpdate") == "YES")
				{
					logTextBox("Please wait while extracting Item Update Master data...");
					await Task.Run(() => ProcessExtractPrice(Conn));
					logTextBox("Item Master File generated successfully! \r\nITEMS-UPDATE OK");

					logTextBox("Closing Database Connection...");
					Conn.Close();
				}

				if (this.iniFile.GetSetting("Data", "Supplier") == "YES")
				{
					logTextBox("Please wait while extracting Supplier Master data...");
					await Task.Run(() => ProcessExtractSupplier(Conn));
					logTextBox("Item Master File generated successfully! \r\nSUPPLIER OK");

					logTextBox("Closing Database Connection...");
					Conn.Close();
				}

				if (this.iniFile.GetSetting("Data", "Customer") == "YES")
				{
					logTextBox("Please wait while extracting Customer Master data...");
					await Task.Run(() => ProcessExtractCustomer(Conn));
					logTextBox("Item Master File generated successfully! \r\nCUSTOMER OK");

					logTextBox("Closing Database Connection...");
					Conn.Close();
				}

				if (this.iniFile.GetSetting("Data", "FreightCharge") == "YES")
				{
					logTextBox("Please wait while extracting Item Master FC data...");
					await Task.Run(() => ProcessExtractItemFc(Conn));
					logTextBox("Item Master File generated successfully! \r\nITEMS-FC OK");

					logTextBox("Closing Database Connection...");
					Conn.Close();
				}*/
			}
		}

		private async Task ProcessExtractItem(SqlConnection Conn)
		{
			string DestPath = this.iniFile.GetSetting("Data", "DestPath");
			string BingoFile1 = DestPath + "master_product.bingo";
			string strFileName = "master_product.dat";
			string sepChar = "|";
			string path = Path.Combine(DestPath, strFileName);

			try
			{
				logTextBox("Please wait while extracting Item Master data...");
				string strQuery = "[dbo].[RCS_EXTRACT_ITEMMASTER]";

				SqlCommand cmd = new SqlCommand(strQuery, Conn);
				cmd.CommandType = CommandType.StoredProcedure;
				SqlDataAdapter sda = new SqlDataAdapter(cmd);
				sda.SelectCommand.CommandTimeout = 0;
				DataTable table = new DataTable();
				await Task.Run(() => sda.Fill(table));

				MessageBox.Show("Data retrieval completed! Rows retrieved: " + table.Rows.Count);

				StreamWriter writer = new StreamWriter(path);
				string sep = "";
				StringBuilder builder = new StringBuilder();
				foreach (DataColumn col in table.Columns)
				{
					builder.Append(sep).Append(col.ColumnName);
					sep = sepChar;
				}
				builder.AppendLine();
				writer.Write(builder.ToString());

				builder.Clear();

				foreach (DataRow row in table.Rows)
				{
					sep = "";
					foreach (DataColumn col in table.Columns)
					{
						builder.Append(sep).Append(row[col.ColumnName]);
						sep = sepChar;
					}
					writer.WriteLine(builder.ToString());
					builder.Clear();
				}
				writer.Close();

				if (File.Exists(path))
				{
					File.Delete(path);
				}

				if (!File.Exists(BingoFile1))
				{
					FileInfo bingoLog = new FileInfo(BingoFile1);
					writer = bingoLog.CreateText();

					writer.WriteLine(DateTime.Today.ToString() + "");
					writer.Close();
				}
				logTextBox("Item Master File generated successfully! \r\nITEMS OK");
			}
			catch (Exception err)
			{
				logTextBox($"Error Desc: {0} {err.Message}");
			}
		}

		/*private bool ProcessExtractSupplier(SqlConnection Conn)
		{
			string DestPath = this.iniFile.GetSetting("Data", "DestPath");
			string BingoFile2 = DestPath + "master_supplier.bingo";
			string strFileName = "master_supplier.dat";
			string sepChar = "|";
			string path = Path.Combine(DestPath, strFileName);

			try
			{
				string strQuery = "[dbo].[RCS_EXTRACT_BPSupplier]";
				using (SqlCommand cmd = new SqlCommand(strQuery, Conn))
				{
					Dat(cmd, sepChar, BingoFile2, path);
				}
				return true;
			}
			catch (Exception err)
			{
				logTxtBox.Invoke((Action)delegate
				{
					logTxtBox.Text = "Error Desc: {0} " + err.Message + "\r\n" + logTxtBox.Text;
				});
				return false;
			}
		}

		private bool ProcessExtractCustomer(SqlConnection Conn)
		{
			string DestPath = this.iniFile.GetSetting("Data", "DestPath");
			string BingoFile3 = DestPath + "master_customer.bingo";
			string strFileName = "master_customer.dat";
			string sepChar = "|";
			string path = Path.Combine(DestPath, strFileName);

			try
			{
				string strQuery = "[dbo].[RCS_EXTRACT_BPCustomer]";
				using (SqlCommand cmd = new SqlCommand(strQuery, Conn))
				{
					Dat(cmd, sepChar, BingoFile3, path);
				}
				return true;
			}
			catch (Exception err)
			{
				logTxtBox.Invoke((Action)delegate
				{
					logTxtBox.Text = "Error Desc: {0} " + err.Message + "\r\n" + logTxtBox.Text;
				});
				return false;
			}
		} 

		private bool ProcessExtractItemFc(SqlConnection Conn)
		{
			string DestPath = this.iniFile.GetSetting("Data", "DestPath");
			string BingoFile4 = DestPath + "master_product_fc.bingo";
			string strFileName = "master_product_fc.dat";
			string sepChar = "|";
			string path = Path.Combine(DestPath, strFileName);
			try
			{
				string strQuery = "[dbo].[RCS_EXTRACT_ITEMMASTER_FC]";
				using (SqlCommand cmd = new SqlCommand(strQuery, Conn))
				{
					Dat(cmd, sepChar, BingoFile4, path);
				}
				return true;
			}
			catch (Exception err)
			{
				logTxtBox.Invoke((Action)delegate
				{
					logTxtBox.Text = "Error Desc: {0} " + err.Message + "\r\n" + logTxtBox.Text;
				});
				return false;
			}
		}

		private bool ProcessExtractPrice(SqlConnection Conn)
		{
			string DestPath = this.iniFile.GetSetting("Data", "DestPath");
			string dates = DateTime.Today.ToString("MM/dd/yyyy");
			string BingoFile5 = DestPath + "master_product_update.bingo";
			string strFileName = "master_product_update.dat";
			string sepChar = "|";
			string path = Path.Combine(DestPath, strFileName);
			
			try
			{
				string strQuery = "[dbo].[RCS_Extract_Product_Update]";
				using (SqlCommand cmd = new SqlCommand(strQuery, Conn))
				{
					Dat(cmd, sepChar, BingoFile5, path);
				}
				return true;
			}
			catch (Exception err)
			{
				logTxtBox.Invoke((Action)delegate
				{
					logTxtBox.Text = "Error Desc: {0} " + err.Message + "\r\n" + logTxtBox.Text;
				});
				return false;
			}
		}

		private void Dat(SqlCommand cmd, string sepChar, string bingoFile, string path)
		{
			cmd.CommandType = CommandType.StoredProcedure;
			using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
			{
				sda.SelectCommand.CommandTimeout = 0;
				DataTable dt = new DataTable();
				sda.Fill(dt);

				StringBuilder builder = new StringBuilder();
				StreamWriter sw = new StreamWriter(path);

				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Columns.Count; i++)
					{
						builder.Append(dt.Columns[i].ColumnName);
						if (i < dt.Columns.Count - 1)
						{
							builder.Append(sepChar);
						}
					}
					builder.AppendLine();

					foreach (DataRow row in dt.Rows)
					{
						for (int i = 0; i < dt.Columns.Count; i++)
						{
							builder.Append(row[i].ToString());
							if (i < dt.Columns.Count - 1)
							{
								builder.Append(sepChar);
							}
						}
						builder.AppendLine();
					}
					sw.WriteLine(builder.ToString());
					builder.Clear();
				}
				sw.Close();

				if (File.Exists(path))
				{
					File.Delete(path);
				}

				if (!File.Exists(bingoFile))
				{
					FileInfo bingoLog = new FileInfo(bingoFile);
					sw = bingoLog.CreateText();

					sw.WriteLine(DateTime.Today.ToString() + "");
					sw.Close();
				}
			}
		}*/

		private void Main_Load(object sender, EventArgs e)
		{
			ProcessExtract();
		}

		private void logTextBox(string message)
		{
			logTxtBox.Invoke((Action)delegate
			{
				logTxtBox.Text = message + "\r\n" + logTxtBox.Text;
			});
		}

		private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
		{

		}
	}
}
