using System.Drawing;
using System.Windows.Forms;

namespace Progetto2
{
    // Gestisce la finestra di visualizzazione:
    // - posizionamento dei grafi 
    // - menu di Log, smooth e sensori
    // - controllo dell'analisi (inizia, ferma, continua e ricomincia)
    // ATTENZIONE: solo la grafica.
    partial class MyForm
    {
        private ZedGraph.ZedGraphControl zedGraphControl;
        private Label lblStatusLabel;
        private Label lblStatusLabel2;
        private MenuStrip menuStrip;
        private ToolStripMenuItem editMenuItem;
        private ToolStripMenuItem aboutMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem stopMenuItem;
        private ToolStripMenuItem restartMenuItem;
        private ToolStripMenuItem pauseMenuItem;
        private ToolStripMenuItem resumeMenuItem;
        private ToolStripSeparator separator1EditMenuItem;
        private ToolStripSeparator separator2EditMenuItem;
        private Panel panel;
        private Panel panel2;
        private TabControl tabControl1;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Label lblStazionamento;
        private RichTextBox rtbStazLog;
        private GroupBox groupGyr;
        private RadioButton g4;
        private RadioButton g3;
        private RadioButton g2;
        private RadioButton g1;
        private RadioButton g0;
        private GroupBox groupAcc;
        private RadioButton a4;
        private RadioButton a3;
        private RadioButton a2;
        private RadioButton a1;
        private RadioButton a0;
        private GroupBox groupMagn;
        private RadioButton m4;
        private RadioButton m3;
        private RadioButton m2;
        private RadioButton m1;
        private RadioButton m0;
        private GroupBox groupBox3;
        private Label label1;
        private CheckBox chkSmooth;
        private TextBox txtValoreSmooth;
        private ToolStripButton pausaToolBar;
        private ToolStripButton resumeToolBar;
        private ToolStripSeparator separator1ToolBar;
        private ToolStripButton restartToolBar;
        private ToolStripButton stopToolBar;
        private ToolStripSeparator separator2ToolBar;
        private ToolStripButton clearToolBar;
        private ToolStrip toolBar;
        private Label lblPosizionamento;
        private RichTextBox rtbPosLog;
        private Label lblGirata;
        private RichTextBox rtbGirataLog;
        private Label lblEulero;
        private RichTextBox rtbEuleroLog;
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Primo metodo chiamato nella creazione di MyForm
        private void InitializeComponent()
        {
            
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MyForm));
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.lblStatusLabel2 = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1EditMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.restartMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.separator2EditMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblPosizionamento = new System.Windows.Forms.Label();
            this.rtbPosLog = new System.Windows.Forms.RichTextBox();
            this.lblStazionamento = new System.Windows.Forms.Label();
            this.rtbStazLog = new System.Windows.Forms.RichTextBox();
            this.lblEulero = new System.Windows.Forms.Label();
            this.rtbEuleroLog = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.groupMagn = new System.Windows.Forms.GroupBox();
            this.m4 = new System.Windows.Forms.RadioButton();
            this.m3 = new System.Windows.Forms.RadioButton();
            this.m2 = new System.Windows.Forms.RadioButton();
            this.m1 = new System.Windows.Forms.RadioButton();
            this.m0 = new System.Windows.Forms.RadioButton();
            this.groupGyr = new System.Windows.Forms.GroupBox();
            this.g4 = new System.Windows.Forms.RadioButton();
            this.g3 = new System.Windows.Forms.RadioButton();
            this.g2 = new System.Windows.Forms.RadioButton();
            this.g1 = new System.Windows.Forms.RadioButton();
            this.g0 = new System.Windows.Forms.RadioButton();
            this.groupAcc = new System.Windows.Forms.GroupBox();
            this.a4 = new System.Windows.Forms.RadioButton();
            this.a3 = new System.Windows.Forms.RadioButton();
            this.a2 = new System.Windows.Forms.RadioButton();
            this.a1 = new System.Windows.Forms.RadioButton();
            this.a0 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSmooth = new System.Windows.Forms.CheckBox();
            this.txtValoreSmooth = new System.Windows.Forms.TextBox();
            this.pausaToolBar = new System.Windows.Forms.ToolStripButton();
            this.resumeToolBar = new System.Windows.Forms.ToolStripButton();
            this.separator1ToolBar = new System.Windows.Forms.ToolStripSeparator();
            this.restartToolBar = new System.Windows.Forms.ToolStripButton();
            this.stopToolBar = new System.Windows.Forms.ToolStripButton();
            this.separator2ToolBar = new System.Windows.Forms.ToolStripSeparator();
            this.clearToolBar = new System.Windows.Forms.ToolStripButton();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.rtbGirataLog = new System.Windows.Forms.RichTextBox();
            this.lblGirata = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.panel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupMagn.SuspendLayout();
            this.groupGyr.SuspendLayout();
            this.groupAcc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.SuspendLayout();

            // zedGraphControl
            this.zedGraphControl.BackColor = System.Drawing.Color.AliceBlue;
            this.zedGraphControl.IsShowHScrollBar = true;
            this.zedGraphControl.Location = new System.Drawing.Point(191, 83);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.ScrollGrace = 0D;
            this.zedGraphControl.ScrollMaxX = 0D;
            this.zedGraphControl.ScrollMaxY = 0D;
            this.zedGraphControl.ScrollMaxY2 = 0D;
            this.zedGraphControl.ScrollMinX = 0D;
            this.zedGraphControl.ScrollMinY = 0D;
            this.zedGraphControl.ScrollMinY2 = 0D;
            this.zedGraphControl.Size = new System.Drawing.Size(829, 485);
            this.zedGraphControl.TabIndex = 2;

            // lblStatusLabel
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.BackColor = System.Drawing.Color.AliceBlue;
            this.lblStatusLabel.Location = new System.Drawing.Point(196, 58);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(68, 13);
            this.lblStatusLabel.TabIndex = 3;
            this.lblStatusLabel.Text = "Connessione";

            // lblStatusLabel2
            this.lblStatusLabel2.AutoSize = true;
            this.lblStatusLabel2.BackColor = System.Drawing.Color.AliceBlue;
            this.lblStatusLabel2.Location = new System.Drawing.Point(196, 34);
            this.lblStatusLabel2.Name = "lblStatusLabel2";
            this.lblStatusLabel2.Size = new System.Drawing.Size(68, 13);
            this.lblStatusLabel2.TabIndex = 3;
            this.lblStatusLabel2.Text = "University of Milano - Bicocca                 Mauro Manfredelli 781266                Programmazione e amministrazione di sistema";

            // menuStrip
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editMenuItem,
            this.aboutMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip.Size = new System.Drawing.Size(1020, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip";
            
            // exitMenuItem
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(94, 22);
            this.exitMenuItem.Text = "Esci";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            
            // editMenuItem
            this.editMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pauseMenuItem,
            this.resumeMenuItem,
            this.separator1EditMenuItem,
            this.restartMenuItem,
            this.stopMenuItem,});
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editMenuItem.Text = "Manage";
           
            // pauseMenuItem
            this.pauseMenuItem.Name = "pauseMenuItem";
            this.pauseMenuItem.Size = new System.Drawing.Size(167, 22);
            this.pauseMenuItem.Text = "Pausa";
            this.pauseMenuItem.Click += new System.EventHandler(this.pauseMenuItem_Click);
            
            // resumeMenuItem
            this.resumeMenuItem.Name = "resumeMenuItem";
            this.resumeMenuItem.Size = new System.Drawing.Size(167, 22);
            this.resumeMenuItem.Text = "Riprendi";
            this.resumeMenuItem.Click += new System.EventHandler(this.resumeMenuItem_Click);
            
            // separator1EditMenuItem
            this.separator1EditMenuItem.Name = "separator1EditMenuItem";
            this.separator1EditMenuItem.Size = new System.Drawing.Size(164, 6);
            
            // restartMenuItem
            this.restartMenuItem.Name = "restartMenuItem";
            this.restartMenuItem.Size = new System.Drawing.Size(167, 22);
            this.restartMenuItem.Text = "Riavvia";
            this.restartMenuItem.Click += new System.EventHandler(this.restartMenuItem_Click);
            
            // stopMenuItem
            this.stopMenuItem.Name = "stopMenuItem";
            this.stopMenuItem.Size = new System.Drawing.Size(167, 22);
            this.stopMenuItem.Text = "Ferma";
            this.stopMenuItem.Click += new System.EventHandler(this.stopMenuItem_Click);
            
            // separator2EditMenuItem
            this.separator2EditMenuItem.Name = "separator2EditMenuItem";
            this.separator2EditMenuItem.Size = new System.Drawing.Size(164, 6);
            
            // panel
            this.panel.BackColor = System.Drawing.Color.AliceBlue;
            this.panel.Controls.Add(this.tabControl1);
            this.panel.Location = new System.Drawing.Point(0, 23);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(190, 1000);
            this.panel.TabIndex = 13;

            //panel1
            this.panel2.BackColor = System.Drawing.Color.AliceBlue;
            this.panel2.Controls.Add(this.tabControl2);
            this.panel2.Location = new System.Drawing.Point(550, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(590, 1500);
            this.panel2.TabIndex = 14;

            // tabControl1
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(187, 520);
            this.tabControl1.TabIndex = 13;

            // tabControl2
            this.tabControl2.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabControl2.Location = new System.Drawing.Point(3, 3);
            this.tabControl2.Multiline = true;
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(587, 1497);
            this.tabControl2.TabIndex = 14;

            // tabPage1
            this.tabPage1.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage1.Controls.Add(this.lblGirata);
            this.tabPage1.Controls.Add(this.lblPosizionamento);
            this.tabPage1.Controls.Add(this.rtbGirataLog);
            this.tabPage1.Controls.Add(this.rtbPosLog);
            this.tabPage1.Controls.Add(this.lblStazionamento);
            this.tabPage1.Controls.Add(this.rtbStazLog);
            this.tabPage1.Controls.Add(this.lblEulero);
            this.tabPage1.Controls.Add(this.rtbEuleroLog);
            this.tabPage1.Location = new System.Drawing.Point(23, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(160, 512);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Log";

            // tabPage4
            this.tabPage4.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage4.Location = new System.Drawing.Point(23, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(160, 512);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Accelerometro";

            // tabPage5
            this.tabPage5.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage5.Location = new System.Drawing.Point(23, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(160, 512);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Giroscopio";

            // tabPage6
            this.tabPage6.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage6.Location = new System.Drawing.Point(23, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(160, 512);
            this.tabPage6.TabIndex = 0;
            this.tabPage6.Text = "Magnetometro";

            // lblPosizionamento
            this.lblPosizionamento.AutoSize = true;
            this.lblPosizionamento.Location = new System.Drawing.Point(2, 125);
            this.lblPosizionamento.Name = "lblPosizionamento";
            this.lblPosizionamento.Size = new System.Drawing.Size(81, 13);
            this.lblPosizionamento.TabIndex = 18;
            this.lblPosizionamento.Text = "Posizionamento";
            
            // rtbPosLog
            this.rtbPosLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rtbPosLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbPosLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbPosLog.Location = new System.Drawing.Point(3, 142);
            this.rtbPosLog.Name = "rtbPosLog";
            this.rtbPosLog.ReadOnly = true;
            this.rtbPosLog.Size = new System.Drawing.Size(154, 101);
            this.rtbPosLog.TabIndex = 17;
            this.rtbPosLog.Text = "";
            
            // lblStazionamento
            this.lblStazionamento.AutoSize = true;
            this.lblStazionamento.Location = new System.Drawing.Point(1, 3);
            this.lblStazionamento.Name = "lblStazionamento";
            this.lblStazionamento.Size = new System.Drawing.Size(77, 13);
            this.lblStazionamento.TabIndex = 14;
            this.lblStazionamento.Text = "Stazionamento";
            
            // rtbStazLog
            this.rtbStazLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rtbStazLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbStazLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbStazLog.Location = new System.Drawing.Point(3, 21);
            this.rtbStazLog.Name = "rtbStazLog";
            this.rtbStazLog.ReadOnly = true;
            this.rtbStazLog.Size = new System.Drawing.Size(154, 101);
            this.rtbStazLog.TabIndex = 13;
            this.rtbStazLog.Text = "";
            
            // tabPage2
            this.tabPage2.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(23, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(160, 512);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Smooth";
            
            // tabPage3
            this.tabPage3.BackColor = System.Drawing.Color.AliceBlue;
            this.tabPage3.Controls.Add(this.groupMagn);
            this.tabPage3.Controls.Add(this.groupGyr);
            this.tabPage3.Controls.Add(this.groupAcc);
            this.tabPage3.Location = new System.Drawing.Point(23, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(160, 512);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Sensors";
           
            // groupMagn
            this.groupMagn.Controls.Add(this.m4);
            this.groupMagn.Controls.Add(this.m3);
            this.groupMagn.Controls.Add(this.m2);
            this.groupMagn.Controls.Add(this.m1);
            this.groupMagn.Controls.Add(this.m0);
            this.groupMagn.Location = new System.Drawing.Point(3, 196);
            this.groupMagn.Name = "groupMagn";
            this.groupMagn.Size = new System.Drawing.Size(154, 89);
            this.groupMagn.TabIndex = 14;
            this.groupMagn.TabStop = false;
            this.groupMagn.Text = "Orientamento Magn";
            
            // m4
            this.m4.AutoSize = true;
            this.m4.Location = new System.Drawing.Point(87, 43);
            this.m4.Name = "m4";
            this.m4.Size = new System.Drawing.Size(61, 17);
            this.m4.TabIndex = 4;
            this.m4.Text = "Sens. 5";
            this.m4.UseVisualStyleBackColor = true;
            
            // m3
            this.m3.AutoSize = true;
            this.m3.Location = new System.Drawing.Point(87, 19);
            this.m3.Name = "m3";
            this.m3.Size = new System.Drawing.Size(61, 17);
            this.m3.TabIndex = 3;
            this.m3.Text = "Sens. 4";
            this.m3.UseVisualStyleBackColor = true;
            
            // m2
            this.m2.AutoSize = true;
            this.m2.Location = new System.Drawing.Point(7, 67);
            this.m2.Name = "m2";
            this.m2.Size = new System.Drawing.Size(61, 17);
            this.m2.TabIndex = 2;
            this.m2.Text = "Sens. 3";
            this.m2.UseVisualStyleBackColor = true;
            
            // m1
            this.m1.AutoSize = true;
            this.m1.Location = new System.Drawing.Point(7, 43);
            this.m1.Name = "m1";
            this.m1.Size = new System.Drawing.Size(61, 17);
            this.m1.TabIndex = 1;
            this.m1.Text = "Sens. 2";
            this.m1.UseVisualStyleBackColor = true;
            
            // m0
            this.m0.AutoSize = true;
            this.m0.Checked = true;
            this.m0.Location = new System.Drawing.Point(7, 19);
            this.m0.Name = "m0";
            this.m0.Size = new System.Drawing.Size(61, 17);
            this.m0.TabIndex = 0;
            this.m0.TabStop = true;
            this.m0.Text = "Sens. 1";
            this.m0.UseVisualStyleBackColor = true;
            
            // groupGyr
            this.groupGyr.Controls.Add(this.g4);
            this.groupGyr.Controls.Add(this.g3);
            this.groupGyr.Controls.Add(this.g2);
            this.groupGyr.Controls.Add(this.g1);
            this.groupGyr.Controls.Add(this.g0);
            this.groupGyr.Location = new System.Drawing.Point(3, 101);
            this.groupGyr.Name = "groupGyr";
            this.groupGyr.Size = new System.Drawing.Size(154, 89);
            this.groupGyr.TabIndex = 14;
            this.groupGyr.TabStop = false;
            this.groupGyr.Text = "Modulo Gyr";
            
            // g4
            this.g4.AutoSize = true;
            this.g4.Location = new System.Drawing.Point(87, 43);
            this.g4.Name = "g4";
            this.g4.Size = new System.Drawing.Size(61, 17);
            this.g4.TabIndex = 4;
            this.g4.Text = "Sens. 5";
            this.g4.UseVisualStyleBackColor = true;
            
            // g3
            this.g3.AutoSize = true;
            this.g3.Location = new System.Drawing.Point(87, 19);
            this.g3.Name = "g3";
            this.g3.Size = new System.Drawing.Size(61, 17);
            this.g3.TabIndex = 3;
            this.g3.Text = "Sens. 4";
            this.g3.UseVisualStyleBackColor = true;
            
            // g2
            this.g2.AutoSize = true;
            this.g2.Location = new System.Drawing.Point(7, 67);
            this.g2.Name = "g2";
            this.g2.Size = new System.Drawing.Size(61, 17);
            this.g2.TabIndex = 2;
            this.g2.Text = "Sens. 3";
            this.g2.UseVisualStyleBackColor = true;
            
            // g1
            this.g1.AutoSize = true;
            this.g1.Location = new System.Drawing.Point(7, 43);
            this.g1.Name = "g1";
            this.g1.Size = new System.Drawing.Size(61, 17);
            this.g1.TabIndex = 1;
            this.g1.Text = "Sens. 2";
            this.g1.UseVisualStyleBackColor = true;
            
            // g0
            this.g0.AutoSize = true;
            this.g0.Checked = true;
            this.g0.Location = new System.Drawing.Point(7, 19);
            this.g0.Name = "g0";
            this.g0.Size = new System.Drawing.Size(61, 17);
            this.g0.TabIndex = 0;
            this.g0.TabStop = true;
            this.g0.Text = "Sens. 1";
            this.g0.UseVisualStyleBackColor = true;
            
            // groupAcc
            this.groupAcc.Controls.Add(this.a4);
            this.groupAcc.Controls.Add(this.a3);
            this.groupAcc.Controls.Add(this.a2);
            this.groupAcc.Controls.Add(this.a1);
            this.groupAcc.Controls.Add(this.a0);
            this.groupAcc.Location = new System.Drawing.Point(3, 6);
            this.groupAcc.Name = "groupAcc";
            this.groupAcc.Size = new System.Drawing.Size(154, 89);
            this.groupAcc.TabIndex = 13;
            this.groupAcc.TabStop = false;
            this.groupAcc.Text = "Modulo Acc";
            
            // a4
            this.a4.AutoSize = true;
            this.a4.Location = new System.Drawing.Point(87, 40);
            this.a4.Name = "a4";
            this.a4.Size = new System.Drawing.Size(61, 17);
            this.a4.TabIndex = 4;
            this.a4.Text = "Sens. 5";
            this.a4.UseVisualStyleBackColor = true;
            
            // a3
            this.a3.AutoSize = true;
            this.a3.Location = new System.Drawing.Point(87, 16);
            this.a3.Name = "a3";
            this.a3.Size = new System.Drawing.Size(61, 17);
            this.a3.TabIndex = 3;
            this.a3.Text = "Sens. 4";
            this.a3.UseVisualStyleBackColor = true;
            
            // a2
            this.a2.AutoSize = true;
            this.a2.Location = new System.Drawing.Point(7, 64);
            this.a2.Name = "a2";
            this.a2.Size = new System.Drawing.Size(61, 17);
            this.a2.TabIndex = 2;
            this.a2.Text = "Sens. 3";
            this.a2.UseVisualStyleBackColor = true;
            
            // a1
            this.a1.AutoSize = true;
            this.a1.Location = new System.Drawing.Point(7, 40);
            this.a1.Name = "a1";
            this.a1.Size = new System.Drawing.Size(61, 17);
            this.a1.TabIndex = 1;
            this.a1.Text = "Sens. 2";
            this.a1.UseVisualStyleBackColor = true;
            
            // a0
            this.a0.AutoSize = true;
            this.a0.Checked = true;
            this.a0.Location = new System.Drawing.Point(7, 16);
            this.a0.Name = "a0";
            this.a0.Size = new System.Drawing.Size(61, 17);
            this.a0.TabIndex = 0;
            this.a0.TabStop = true;
            this.a0.Text = "Sens. 1";
            this.a0.UseVisualStyleBackColor = true;
            
            // groupBox3
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.chkSmooth);
            this.groupBox3.Controls.Add(this.txtValoreSmooth);
            this.groupBox3.Location = new System.Drawing.Point(3, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(154, 46);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Smooth";
            
            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Valore";
            
            // chkSmooth
            this.chkSmooth.AutoSize = true;
            this.chkSmooth.Location = new System.Drawing.Point(6, 19);
            this.chkSmooth.Name = "chkSmooth";
            this.chkSmooth.Size = new System.Drawing.Size(54, 17);
            this.chkSmooth.TabIndex = 6;
            this.chkSmooth.Text = "Abilita";
            this.chkSmooth.UseVisualStyleBackColor = true;
            
            // txtValoreSmooth
            this.txtValoreSmooth.Location = new System.Drawing.Point(114, 17);
            this.txtValoreSmooth.MaxLength = 2;
            this.txtValoreSmooth.Name = "txtValoreSmooth";
            this.txtValoreSmooth.Size = new System.Drawing.Size(25, 20);
            this.txtValoreSmooth.TabIndex = 8;
            this.txtValoreSmooth.Text = "0";
            
            // pausaToolBar
            this.pausaToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.pausaToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pausaToolBar.Name = "pausaToolBar";
            this.pausaToolBar.Size = new System.Drawing.Size(23, 22);
            this.pausaToolBar.Text = "Pause";
            this.pausaToolBar.Click += new System.EventHandler(this.pauseToolBar_Click);
            
            // resumeToolBar
            this.resumeToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.resumeToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resumeToolBar.Name = "resumeToolBar";
            this.resumeToolBar.Size = new System.Drawing.Size(23, 22);
            this.resumeToolBar.Text = "Riprendi";
            this.resumeToolBar.Click += new System.EventHandler(this.resumeToolBar_Click);
            
            // separator1ToolBar
            this.separator1ToolBar.Name = "separator1ToolBar";
            this.separator1ToolBar.Size = new System.Drawing.Size(6, 25);
            
            // restartToolBar
            this.restartToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.restartToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.restartToolBar.Name = "restartToolBar";
            this.restartToolBar.Size = new System.Drawing.Size(23, 22);
            this.restartToolBar.Text = "Riavvia";
            this.restartToolBar.Click += new System.EventHandler(this.restartToolBar_Click);
            
            // stopToolBar
            this.stopToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopToolBar.Name = "stopToolBar";
            this.stopToolBar.Size = new System.Drawing.Size(23, 22);
            this.stopToolBar.Text = "Ferma";
            this.stopToolBar.Click += new System.EventHandler(this.stopToolBar_Click);
            
            // separator2ToolBar
            this.separator2ToolBar.Name = "separator2ToolBar";
            this.separator2ToolBar.Size = new System.Drawing.Size(6, 25);
            
            // clearToolBar
            this.clearToolBar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.clearToolBar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearToolBar.Name = "clearToolBar";
            this.clearToolBar.Size = new System.Drawing.Size(23, 22);
            this.clearToolBar.Text = "Ripulisci il grafico";
            this.clearToolBar.Click += new System.EventHandler(this.clearToolBar_Click);
            
            // toolBar
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pausaToolBar,
            this.resumeToolBar,
            this.separator1ToolBar,
            this.restartToolBar,
            this.stopToolBar,
            this.separator2ToolBar,
            this.clearToolBar});
            this.toolBar.Location = new System.Drawing.Point(0, 24);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(1020, 25);
            this.toolBar.TabIndex = 14;
            this.toolBar.Text = "toolStrip1";
            
            // rtbGirataLog
            this.rtbGirataLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rtbGirataLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbGirataLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbGirataLog.Location = new System.Drawing.Point(3, 265);
            this.rtbGirataLog.Name = "rtbGirataLog";
            this.rtbGirataLog.ReadOnly = true;
            this.rtbGirataLog.Size = new System.Drawing.Size(154, 101);
            this.rtbGirataLog.TabIndex = 17;
            this.rtbGirataLog.Text = "";

            // lblGirata
            this.lblGirata.AutoSize = true;
            this.lblGirata.Location = new System.Drawing.Point(2, 248);
            this.lblGirata.Name = "lblGirata";
            this.lblGirata.Size = new System.Drawing.Size(35, 13);
            this.lblGirata.TabIndex = 18;
            this.lblGirata.Text = "Girata";

            // rtbEuleroLog
            this.rtbEuleroLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.rtbEuleroLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbEuleroLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.rtbEuleroLog.Location = new System.Drawing.Point(3, 388);
            this.rtbEuleroLog.Name = "rtbEuleroLog";
            this.rtbEuleroLog.ReadOnly = true;
            this.rtbEuleroLog.Size = new System.Drawing.Size(154, 101);
            this.rtbEuleroLog.TabIndex = 17;
            this.rtbEuleroLog.Text = "";

            // lblEulero
            this.lblEulero.AutoSize = true;
            this.lblEulero.Location = new System.Drawing.Point(2, 371);
            this.lblEulero.Name = "lblEulero";
            this.lblEulero.Size = new System.Drawing.Size(35, 13);
            this.lblEulero.TabIndex = 18;
            this.lblEulero.Text = "Angoli d'Eulero";

            // MyForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1020, 570);
            this.Controls.Add(this.lblStatusLabel);
            this.Controls.Add(this.lblStatusLabel2);
            //this.Controls.Add(this.toolBar);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.zedGraphControl);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViweData";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MyForm_FormClosing);
            this.Load += new System.EventHandler(this.MyForm_Load);
            this.Resize += new System.EventHandler(this.MyForm_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupGyr.ResumeLayout(false);
            this.groupGyr.PerformLayout();
            this.groupAcc.ResumeLayout(false);
            this.groupAcc.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}

#endregion