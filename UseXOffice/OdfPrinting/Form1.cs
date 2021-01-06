using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

public class Form1 : System.Windows.Forms.Form
{
	private System.ComponentModel.Container components = null;

	public Form1()
	{
		InitializeComponent();
	}

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	protected override void Dispose( bool disposing )
	{
		if( disposing )
		{
			if (components != null) 
			{
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
	private void InitializeComponent()
	{
		this.bLoad = new System.Windows.Forms.Button();
		this.bPrint = new System.Windows.Forms.Button();
		this.bOpen = new System.Windows.Forms.Button();
		this.bSave = new System.Windows.Forms.Button();
		this.tabControl = new System.Windows.Forms.TabControl();
		this.tabOdt = new System.Windows.Forms.TabPage();
		this.tabOds = new System.Windows.Forms.TabPage();
		this.bOpenOds = new System.Windows.Forms.Button();
		this.bPrintOds = new System.Windows.Forms.Button();
		this.bLoadOds = new System.Windows.Forms.Button();
		this.bSaveOds = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tcTables = new System.Windows.Forms.TabControl();
		this.bCopyLastRow = new System.Windows.Forms.Button();
		this.tabSource1 = new System.Windows.Forms.TabPage();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.tabSource2 = new System.Windows.Forms.TabPage();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.tabControl.SuspendLayout();
		this.tabOdt.SuspendLayout();
		this.tabOds.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabSource1.SuspendLayout();
		this.tabSource2.SuspendLayout();
		this.SuspendLayout();
		// 
		// bLoad
		// 
		this.bLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bLoad.Location = new System.Drawing.Point(624, 168);
		this.bLoad.Name = "bLoad";
		this.bLoad.TabIndex = 0;
		this.bLoad.Text = "Load";
		this.bLoad.Click += new System.EventHandler(this.bLoad_Click);
		// 
		// bPrint
		// 
		this.bPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bPrint.Enabled = false;
		this.bPrint.Location = new System.Drawing.Point(624, 200);
		this.bPrint.Name = "bPrint";
		this.bPrint.TabIndex = 1;
		this.bPrint.Text = "Print";
		this.bPrint.Click += new System.EventHandler(this.bPrint_Click);
		// 
		// bOpen
		// 
		this.bOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bOpen.Enabled = false;
		this.bOpen.Location = new System.Drawing.Point(624, 232);
		this.bOpen.Name = "bOpen";
		this.bOpen.Size = new System.Drawing.Size(75, 40);
		this.bOpen.TabIndex = 2;
		this.bOpen.Text = "Open in OpenOffice";
		this.bOpen.Click += new System.EventHandler(this.bOpen_Click);
		// 
		// bSave
		// 
		this.bSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bSave.Enabled = false;
		this.bSave.Location = new System.Drawing.Point(624, 286);
		this.bSave.Name = "bSave";
		this.bSave.Size = new System.Drawing.Size(75, 40);
		this.bSave.TabIndex = 3;
		this.bSave.Text = "Save to\r\nc:\\test.odt";
		this.bSave.Click += new System.EventHandler(this.bSave_Click);
		// 
		// tabControl
		// 
		this.tabControl.Controls.Add(this.tabOdt);
		this.tabControl.Controls.Add(this.tabOds);
		this.tabControl.Controls.Add(this.tabSource1);
		this.tabControl.Controls.Add(this.tabSource2);
		this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl.Location = new System.Drawing.Point(0, 0);
		this.tabControl.Name = "tabControl";
		this.tabControl.SelectedIndex = 0;
		this.tabControl.Size = new System.Drawing.Size(720, 358);
		this.tabControl.TabIndex = 4;
		// 
		// tabOdt
		// 
		this.tabOdt.Controls.Add(this.bOpen);
		this.tabOdt.Controls.Add(this.bPrint);
		this.tabOdt.Controls.Add(this.bLoad);
		this.tabOdt.Controls.Add(this.bSave);
		this.tabOdt.Location = new System.Drawing.Point(4, 22);
		this.tabOdt.Name = "tabOdt";
		this.tabOdt.Size = new System.Drawing.Size(712, 332);
		this.tabOdt.TabIndex = 0;
		this.tabOdt.Text = "Writer";
		// 
		// tabOds
		// 
		this.tabOds.Controls.Add(this.tcTables);
		this.tabOds.Controls.Add(this.panel1);
		this.tabOds.Location = new System.Drawing.Point(4, 22);
		this.tabOds.Name = "tabOds";
		this.tabOds.Size = new System.Drawing.Size(712, 332);
		this.tabOds.TabIndex = 1;
		this.tabOds.Text = "Spreadsheet";
		// 
		// bOpenOds
		// 
		this.bOpenOds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bOpenOds.Enabled = false;
		this.bOpenOds.Location = new System.Drawing.Point(16, 236);
		this.bOpenOds.Name = "bOpenOds";
		this.bOpenOds.Size = new System.Drawing.Size(75, 40);
		this.bOpenOds.TabIndex = 6;
		this.bOpenOds.Text = "Open in OpenOffice";
		this.bOpenOds.Click += new System.EventHandler(this.bOpenOds_Click);
		// 
		// bPrintOds
		// 
		this.bPrintOds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bPrintOds.Enabled = false;
		this.bPrintOds.Location = new System.Drawing.Point(16, 204);
		this.bPrintOds.Name = "bPrintOds";
		this.bPrintOds.TabIndex = 5;
		this.bPrintOds.Text = "Print";
		this.bPrintOds.Click += new System.EventHandler(this.bPrintOds_Click);
		// 
		// bLoadOds
		// 
		this.bLoadOds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bLoadOds.Location = new System.Drawing.Point(16, 172);
		this.bLoadOds.Name = "bLoadOds";
		this.bLoadOds.TabIndex = 4;
		this.bLoadOds.Text = "Load";
		this.bLoadOds.Click += new System.EventHandler(this.bLoadOds_Click);
		// 
		// bSaveOds
		// 
		this.bSaveOds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.bSaveOds.Enabled = false;
		this.bSaveOds.Location = new System.Drawing.Point(16, 284);
		this.bSaveOds.Name = "bSaveOds";
		this.bSaveOds.Size = new System.Drawing.Size(75, 40);
		this.bSaveOds.TabIndex = 7;
		this.bSaveOds.Text = "Save to\r\nc:\\test.ods";
		this.bSaveOds.Click += new System.EventHandler(this.bSaveOds_Click);
		// 
		// panel1
		// 
		this.panel1.Controls.Add(this.bCopyLastRow);
		this.panel1.Controls.Add(this.bSaveOds);
		this.panel1.Controls.Add(this.bLoadOds);
		this.panel1.Controls.Add(this.bPrintOds);
		this.panel1.Controls.Add(this.bOpenOds);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel1.Location = new System.Drawing.Point(608, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(104, 332);
		this.panel1.TabIndex = 8;
		// 
		// tcTables
		// 
		this.tcTables.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tcTables.Location = new System.Drawing.Point(0, 0);
		this.tcTables.Name = "tcTables";
		this.tcTables.SelectedIndex = 0;
		this.tcTables.Size = new System.Drawing.Size(608, 332);
		this.tcTables.TabIndex = 9;
		// 
		// bCopyLastRow
		// 
		this.bCopyLastRow.Enabled = false;
		this.bCopyLastRow.Location = new System.Drawing.Point(16, 96);
		this.bCopyLastRow.Name = "bCopyLastRow";
		this.bCopyLastRow.Size = new System.Drawing.Size(75, 40);
		this.bCopyLastRow.TabIndex = 8;
		this.bCopyLastRow.Text = "<- Copy last row";
		this.bCopyLastRow.Click += new System.EventHandler(this.bCopyLastRow_Click);
		// 
		// tabSource1
		// 
		this.tabSource1.Controls.Add(this.textBox1);
		this.tabSource1.Location = new System.Drawing.Point(4, 22);
		this.tabSource1.Name = "tabSource1";
		this.tabSource1.Size = new System.Drawing.Size(712, 332);
		this.tabSource1.TabIndex = 2;
		this.tabSource1.Text = "Writer sample";
		// 
		// textBox1
		// 
		this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(238)));
		this.textBox1.Location = new System.Drawing.Point(0, 0);
		this.textBox1.Multiline = true;
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(712, 332);
		this.textBox1.TabIndex = 0;
		this.textBox1.Text = @"// Writer example
Odt odt = new Odt(""sample.odt"");

odt.Inputs[""name""] = ""Harry Potter"";
odt.Inputs[""address""] = ""The cupboard under the stairs, 4 Privet Drive Little Whinging, Surrey"";
odt.Inputs[""print_date""] = String.Format(""{0:yyyy-MM-dd}"", DateTime.Now);

odt.Save(""result.odt"");
odt.OpenInOo();";
		// 
		// tabSource2
		// 
		this.tabSource2.Controls.Add(this.textBox2);
		this.tabSource2.Location = new System.Drawing.Point(4, 22);
		this.tabSource2.Name = "tabSource2";
		this.tabSource2.Size = new System.Drawing.Size(712, 332);
		this.tabSource2.TabIndex = 3;
		this.tabSource2.Text = "Spreadsheet sample";
		// 
		// textBox2
		// 
		this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.textBox2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(238)));
		this.textBox2.Location = new System.Drawing.Point(0, 0);
		this.textBox2.Multiline = true;
		this.textBox2.Name = "textBox2";
		this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
		this.textBox2.Size = new System.Drawing.Size(712, 332);
		this.textBox2.TabIndex = 1;
		this.textBox2.Text = @"// Spreadsheet example
Ods ods = new Ods(""sample.ods"");

// Print all tables
foreach(Ods.OdsTable table in ods.Tables)
{
	Console.WriteLine(table.Name);
}

// Set text in A2 cell
ods.Tables[""Persons""][""A2""].Text = ""Harry"";

// Add ten more rows
Ods.OdsTable persons = ods.Tables[""Persons""]; 
for(int i = 0; i < 10; i++)
{
	persons.Rows[persons.Rows.Count - 1].CopyDown();
	persons.Rows[persons.Rows.Count - 1].Cells[0].Text = ""Person "" + i;
}

// Save and open in office
ods.Save(@""c:\result.ods"");
ods.OpenInOo();";
		// 
		// Form1
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(720, 358);
		this.Controls.Add(this.tabControl);
		this.Name = "Form1";
		this.Text = "Open document format editing & printing demo";
		this.tabControl.ResumeLayout(false);
		this.tabOdt.ResumeLayout(false);
		this.tabOds.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tabSource1.ResumeLayout(false);
		this.tabSource2.ResumeLayout(false);
		this.ResumeLayout(false);

	}
	#endregion

	private System.Windows.Forms.Button bLoad;
	private System.Windows.Forms.Button bOpen;
	private System.Windows.Forms.Button bSave;
	private System.Windows.Forms.Button bPrint;

	private Odt odt;
	private Ods ods;
	private Label[] keys;
	private System.Windows.Forms.TabControl tabControl;
	private System.Windows.Forms.TabPage tabOdt;
	private System.Windows.Forms.Button bOpenOds;
	private System.Windows.Forms.Button bPrintOds;
	private System.Windows.Forms.Button bLoadOds;
	private System.Windows.Forms.Button bSaveOds;
	private System.Windows.Forms.Panel panel1;
	private System.Windows.Forms.TabPage tabOds;
	private System.Windows.Forms.TabControl tcTables;
	private System.Windows.Forms.Button bCopyLastRow;
	private System.Windows.Forms.TextBox textBox1;
	private System.Windows.Forms.TabPage tabSource1;
	private System.Windows.Forms.TabPage tabSource2;
	private System.Windows.Forms.TextBox textBox2;
	private TextBox[] values;

	[STAThread]
	static void Main() 
	{
		#region Examples
//
//		// Writer example
//		Odt odt = new Odt("sample.odt");
//
//		odt.Inputs["name"] = "Harry Potter";
//		odt.Inputs["address"] = "The cupboard under the stairs, 4 Privet Drive Little Whinging, Surrey";
//		odt.Inputs["print_date"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
//
//		odt.Save(@"c:\result.odt");
//		odt.Print();
//
//		// Spreadsheet example
//		Ods ods = new Ods("sample.ods");
//
//		// Print all tables
//		foreach(Ods.OdsTable table in ods.Tables)
//		{
//			Console.WriteLine(table.Name);
//		}
//
//		// Set text in A2 cell
//		ods.Tables["Persons"]["A2"].Text = "Harry";
//
//		// Add ten more rows
//		Ods.OdsTable persons = ods.Tables["Persons"]; 
//		for(int i = 0; i < 10; i++)
//		{
//			persons.Rows[persons.Rows.Count - 1].CopyDown();
//			persons.Rows[persons.Rows.Count - 1].Cells[0].Text = "Person " + i;
//		}
//
//		// Save and open in office
//		ods.Save(@"c:\result.ods");
//		ods.OpenInOo();
//
		#endregion

        //Ods ods = new Ods("c:\\test1.ods");
        //Ods.OdsCell cell = ods.Tables["1"]["AE42"];
        //for(int i = 0; i < cell.Column; i++)
        //{
        //    ods.Tables["1"][cell.Row, i].Value = i.ToString();
        //    ods.Tables["1"][cell.Row+1, i].Value = i.ToString();
        //}
        //ods.OpenInOo("c:\\result.ods");

		Application.Run(new Form1());
	}

	// Display textboxes for all input fields in document
	private void OdtCreateInputControls()
	{
		Odt.OdtDocFields inputs = odt.Inputs;

		keys = new Label[inputs.Count];
		values = new TextBox[inputs.Count];

		int i = 0;
		foreach(string key in inputs.FieldNames)
		{
			Label l = keys[i] = new Label();
			l.TextAlign = ContentAlignment.MiddleRight;
			l.Top = i * 20 + 8;
			l.Left = 32;
			l.Width = 128;			
			l.Text = key;				// key is name of input field in OO document

			TextBox tb = values[i] = new TextBox();
			tb.Top = l.Top;
			tb.Left = l.Left + l.Width + 16;
			tb.Width = 256;
			tb.Text = inputs[key];		// default value - user will be able to alter it

			if(key == "print_date")		// default value for print_date field
			{
				tb.Text = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
			}

			tabOdt.Controls.Add(l);
			tabOdt.Controls.Add(tb);

			i++;
		}
	}

	// Write new values to input fields
	private void OdtWriteInputs()
	{
		Odt.OdtDocFields inputs = odt.Inputs;
		for(int i = 0; i < keys.Length; i++)
		{
			inputs[keys[i].Text] = values[i].Text;
		}
	}

	private void bLoad_Click(object sender, System.EventArgs e)
	{
		// Show dialog to let user select the template document
		OpenFileDialog dlg = new OpenFileDialog();
		dlg.Filter = "OpenOffice document (*.odt)|*.odt";
		dlg.FileName = Path.Combine(Application.StartupPath, "sample.odt");
		if(dlg.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		// Constructor
		odt = new Odt(dlg.FileName);

		// Create controls for displaying document fields
		OdtCreateInputControls();
		bSave.Enabled = bOpen.Enabled = bPrint.Enabled = true;
	}

	private void bPrint_Click(object sender, System.EventArgs e)
	{
		OdtWriteInputs();
		odt.Print();
	}

	private void bSave_Click(object sender, System.EventArgs e)
	{
		OdtWriteInputs();
		odt.Save("c:\\test.odt");
	}

	private void bOpen_Click(object sender, System.EventArgs e)
	{
		OdtWriteInputs();
		odt.OpenInOo();
	}

	public class TextBoxCell : TextBox
	{
		public Ods.OdsCell Cell;

		public TextBoxCell(Ods.OdsCell cell)
		{
			this.Cell = cell;
			this.Top = cell.Row * 32;
			this.Left = cell.Column * 96;
			this.Width = 88;
			this.Text = cell.Value;
		}
	}

	// Display tables, rows and cells in spreadsheet
	private void OdsCreateInputControls()
	{
		tcTables.TabPages.Clear();
		foreach(Ods.OdsTable table in ods.Tables)
		{
			TabPage tab = new TabPage(table.Name);
			tcTables.TabPages.Add(tab);

			for(int i = 0; i < table.Rows.Count; i++)
			{
				Ods.OdsRow row = table.Rows[i];
				for(int j = 0; j < row.Cells.Count; j++)
				{
					Ods.OdsCell cell = row.Cells[j];
					tab.Controls.Add(new TextBoxCell(cell));
				}
			}
		}
	}

	// Write new values to input fields
	private void OdsWriteInputs()
	{
		foreach(TabPage tab in tcTables.TabPages)
		{
			foreach(TextBoxCell tb in tab.Controls)
			{
				Ods.OdsCell cell = tb.Cell;
				cell.Value = tb.Text;
			}
		}
	}

	private void bLoadOds_Click(object sender, System.EventArgs e)
	{
		// Show dialog to let user select the template document
		OpenFileDialog dlg = new OpenFileDialog();
		dlg.Filter = "OpenOffice document (*.ods)|*.ods";
		dlg.FileName = Path.Combine(Application.StartupPath, "sample.ods");
		if(dlg.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		// Constructor
		ods = new Ods(dlg.FileName);

		// Create controls for displaying document cells
		OdsCreateInputControls();
		bSaveOds.Enabled = bOpenOds.Enabled = bPrintOds.Enabled = bCopyLastRow.Enabled = true;
	}

	private void bPrintOds_Click(object sender, System.EventArgs e)
	{
		OdsWriteInputs();
		ods.Print();
	}

	private void bOpenOds_Click(object sender, System.EventArgs e)
	{
		OdsWriteInputs();
		ods.OpenInOo();
	}

	private void bSaveOds_Click(object sender, System.EventArgs e)
	{
		OdsWriteInputs();
		ods.Save("c:\\test.ods");
	}

	private void bCopyLastRow_Click(object sender, System.EventArgs e)
	{
		Ods.OdsTable table = ods.Tables[tcTables.SelectedTab.Text];
		int rowIndex = table.Rows.Count - 1;
		if(rowIndex < 0)
		{
			MessageBox.Show("Table should have at least one row");
			return;
		}
		table.Rows[rowIndex].CopyDown();
		OdsWriteInputs();
		OdsCreateInputControls();
	}
}
