using System;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using System.Text.RegularExpressions;

namespace Progetto2
{
    public partial class MyForm : Form
    {
        
        delegate void SetgraphCallback(object[] value, float tempo);
        delegate void SetState(bool connesso, int stato);
        delegate void SetLabels(string[] yLabels);
        delegate void Clear();
        delegate void setStazLabel_(string value);
        delegate void setGirataLabel_(string value);
        delegate void setPosLabel_(string value);
        delegate void setEuleroLabel_(string value);

        private int sensoreAccellerometro = 0;
        private int sensoreGiroscopio = 0;
        private int sensoreMagnetometro = 0;
        private int valoreSmooth;

        public MyForm()
        {
            InitializeComponent();
            this.BackColor = System.Drawing.Color.AliceBlue;
        }
        
        private void MyForm_Load(object sender, EventArgs e)
        {
            //Al caricamento del form lancio una funzione che prende come parametro l'unico grafico che ho
            CreateChart(zedGraphControl);
            if (Screen.PrimaryScreen.Bounds.Width <= 1024 && Screen.PrimaryScreen.Bounds.Height <= 768)
                this.WindowState = FormWindowState.Maximized;
        }

        private void MyForm_Resize(object sender, EventArgs e)
        {
            zedGraphControl.Width = this.Width - 270;
            zedGraphControl.Height = this.Height - 150;
        }
        private void MyForm_FormClosing(object sender, EventArgs e)
        {
            Program.close();
        }
        
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Program.close();
            this.Close();
        }

        private void restartMenuItem_Click(object sender, EventArgs e)
        {
            Program.start();
            setStatus(false, 0);
        }

        private void stopMenuItem_Click(object sender, EventArgs e)
        {
            Program.close();
            setStatus(false, 5);
        }

        private void pauseMenuItem_Click(object sender, EventArgs e)
        {
            Program.pause();
            setStatus(true, 3);
        }

        private void resumeMenuItem_Click(object sender, EventArgs e)
        {
            Program.resume();
            setStatus(true, 4);
        }

        private void clearMenuItem_Click(object sender, EventArgs e)
        {
            clear();
        }
        
        private void pauseToolBar_Click(object sender, EventArgs e)
        {
            Program.pause();
            setStatus(true, 3);
        }

        private void resumeToolBar_Click(object sender, EventArgs e)
        {
            Program.resume();
            setStatus(true, 4);
        }

        private void restartToolBar_Click(object sender, EventArgs e)
        {
            Program.start();
            setStatus(false, 0);
        }

        private void stopToolBar_Click(object sender, EventArgs e)
        {
            Program.close();
            setStatus(false, 5);
        }

        private void clearToolBar_Click(object sender, EventArgs e)
        {
            clear();
        }
        
        private void txtValoreSmooth_LostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int valore;
            if (Int32.TryParse(tb.Text, out valore) && valore > 0)
            {
                valoreSmooth = valore;
            }
            else
            {
                MessageBox.Show("Il valore inserito non è valido");
                tb.Text = "0";
            }
        }

        private void rtbStazLog_TextChanged(object sender, EventArgs e)
        {
        }

        private void rtbPosLog_TextChanged(object sender, EventArgs e)
        {
        }
        private void changeAccSens(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                String nomeSensore = rb.Name;
                Match match = Regex.Match(nomeSensore, "[0-9]*$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                Int32.TryParse(match.ToString(), out sensoreAccellerometro);
            }
        }

        private void changeGyrSens(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                String nomeSensore = rb.Name;
                Match match = Regex.Match(nomeSensore, "[0-9]*$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                Int32.TryParse(match.ToString(), out sensoreGiroscopio);
            }
        }

        private void changeMagnSens(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                String nomeSensore = rb.Name;
                Match match = Regex.Match(nomeSensore, "[0-9]*$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                Int32.TryParse(match.ToString(), out sensoreMagnetometro);
            }
        }
        
        public void CreateChart(ZedGraphControl zgc)
        {

            // First, clear out any old GraphPane's from the MasterPane collection
            MasterPane master = zgc.MasterPane;
            master.PaneList.Clear();
            master.Border.IsVisible = false;
            master.Fill.IsVisible = false;
            master.Title.IsVisible = false;
            master.Margin.All = 0;
            master.InnerPaneGap = 0;
            master.Fill.Color = Color.LightSkyBlue;

            ColorSymbolRotator rotator = new ColorSymbolRotator();
            for (int j = 0; j < 4; j++)
            {
                GraphPane pane1 = new GraphPane(new Rectangle(), "", "Time (sec)", "");
                // Sfondo pannello grafico
                pane1.Fill.IsVisible = true;

                // Sfondo singolo grafico
                pane1.Chart.Fill = new Fill(Color.LightBlue, Color.SteelBlue, 45.0F);
                //pane1.Chart.Fill = new Fill(Color.White, Color.SteelBlue, 45.0F);
                // Impedisce il resize del font
                pane1.IsFontsScaled = false;
                // Nasconde la scala e il titol XAxis
                pane1.XAxis.Title.IsVisible = false;
                pane1.XAxis.Scale.IsVisible = false;
                // Nasconde la legenda, il bordo, e il titolo del grafico
                pane1.Legend.IsVisible = false;
                pane1.Border.IsVisible = false;
                pane1.Title.IsVisible = false;
                // Nasconde la linea dello zero
                pane1.YAxis.MajorGrid.IsZeroLine = false;
                // Impedisce che le "tacchette" dei valori delle x non escano fuori dal grafico
                pane1.XAxis.MajorTic.IsOutside = false;
                pane1.XAxis.MinorTic.IsOutside = false;
                // Imposta margini
                pane1.Margin.Bottom = 1;
                // Margine alto del primo grafico
                if (j == 0)
                    pane1.Margin.Top = 10;
                // Mostra il titolo, e la scala XAxis nell'ultimo grafico
                if (j == 3)
                {
                    pane1.XAxis.Title.IsVisible = true;
                    pane1.XAxis.Scale.IsVisible = true;
                    pane1.YAxis.MinorTic.IsAllTics = false;
                }
                // Nasconde ultimo valore della scala YAxis dal secondo grafico
                if (j > 0)
                {
                    pane1.YAxis.Scale.IsSkipLastLabel = true;
                    pane1.Margin.Top = 1;
                }

                // Scala asse x iniziale
                pane1.XAxis.Scale.Min = -2.5d;
                pane1.XAxis.Scale.Max = 2.5d;

                // Le "tacchette" non sono presenti nei lati opposti
                pane1.XAxis.MinorTic.IsOpposite = false;
                pane1.YAxis.MinorTic.IsOpposite = false;
                pane1.XAxis.MajorTic.IsOpposite = false;
                pane1.YAxis.MajorTic.IsOpposite = false;

                // Spazio a sinistra e a destra del margine per allineare i grafici
                pane1.YAxis.MinSpace = 60;
                pane1.Y2Axis.MinSpace = 10;
                
                master.PaneList.Add(pane1);
            }
            master[0].YAxis.Scale.Min = 0;
            master[0].YAxis.Scale.Max = 20;
            master[1].YAxis.Scale.Min = 0;
            master[1].YAxis.Scale.Max = 5;
            master[2].YAxis.Scale.Min = -5;
            master[2].YAxis.Scale.Max = 5;
            master[3].YAxis.Scale.Max = 3.5;
            master[3].YAxis.Scale.BaseTic = -0.5;
            // Refigure the axis ranges for the GraphPanes
            zgc.AxisChange();

            // Layout the GraphPanes using a default Pane Layout
            using (Graphics g = this.CreateGraphics())
            {

                master.SetLayout(g, PaneLayout.SingleColumn);

                master.AxisChange(g);

                // Synchronize the Axes

                zgc.IsAutoScrollRange = true;
                zgc.IsShowHScrollBar = true;
                zgc.IsSynchronizeXAxes = true;
                zgc.Width = this.Width - 270;
                zgc.Height = this.Height - 150;
                g.Dispose();

            }

        }

        //Funzione che preso in ingresso un array di float che rappresenta le ordinate che voglio rappresentare scrive il grafico
        public void writeChart(object[] obj, float tempo)
        {

            //Se la funzione writeChart è chiamata dall'esterno di form (ad esempio da un thread tipo l'analyzer)
            if (this.zedGraphControl.InvokeRequired)
            {
                this.zedGraphControl.Invoke(new SetgraphCallback(writeChart), obj, tempo);
            }
            else  //altrimenti se la chiamata arriva dalla classe form o è frutto dell'invoke
            {

                float[] primo = (float[])obj[0];
                int lung = primo.Length;
                float[] tempi = new float[obj.Length];
                for (int x = 0; x < obj.Length; x++)
                    tempi[x] = tempo;
                PointPairList[] list = new PointPairList[obj.Length];

                for (int x = 0; x < obj.Length; x++)
                    list[x] = new PointPairList();

                for (int j = 0; j < lung; j++)
                {
                    for (int x = 0; x < obj.Length; x++)
                    {
                        float[] value = (float[])obj[x];
                        list[x].Add(tempi[x], value[j]);
                        tempi[x] = tempi[x] + 0.02F;
                    }
                }
                for (int x = 0; x < obj.Length; x++)
                {
                    try
                    {
                        LineItem curve = zedGraphControl.MasterPane[x].AddCurve("", list[x], Color.Black, SymbolType.None);
                        curve.Line.Width = 3F;
                        zedGraphControl.MasterPane[x].XAxis.Scale.Min = tempi[x] - 5;
                        zedGraphControl.MasterPane[x].XAxis.Scale.Max = tempi[x];
                    }
                    catch (Exception)
                    {
                        
                    }
                }
                zedGraphControl.AxisChange();
                Refresh();      

            }
        }

        public void clear()
        {
            if (this.zedGraphControl.InvokeRequired)
            {
                Clear d = new Clear(clear);
                this.zedGraphControl.Invoke(d);
            }
            else
            {
                for (int x = 0; x < 4; x++)
                {
                    zedGraphControl.MasterPane[x].CurveList.Clear();
                    zedGraphControl.MasterPane[x].XAxis.Scale.Min = -2.5d;
                    zedGraphControl.MasterPane[x].XAxis.Scale.Max = 2.5;
                }
                Refresh();
                zedGraphControl.Invalidate();
                rtbStazLog.Clear();
                rtbPosLog.Clear();
                rtbGirataLog.Clear();
                rtbEuleroLog.Clear();
            }
        }

        public void setTitlesGraph(string[] yLabels)
        {
            if (this.InvokeRequired)
            {
                SetLabels d = new SetLabels(setTitlesGraph);
                this.Invoke(d, new object[] { yLabels });
            }
            else
            {
                for (int x = 0; x < yLabels.Length; x++)
                {
                    zedGraphControl.MasterPane[x].YAxis.Title.Text = yLabels[x];
                }
            }
        }
        
        public void setStatus(bool connesso, int stato)
        {
            if (this.lblStatusLabel.InvokeRequired)
            {
                SetState d = new SetState(setStatus);
                this.lblStatusLabel.Invoke(d, new object[] { connesso, stato });
            }
            else
            {
                if (connesso == true)
                {
                    switch (stato)
                    {
                        case 1:
                            lblStatusLabel.Text = "Connesso: " + Progetto2.Container.id; ;
                            break;

                        case 2:
                            lblStatusLabel.Text = "Analisi dei dati in corso";
                            break;

                        case 3:
                            lblStatusLabel.Text = "Pausa, selezionare \"Riprendi\" per continuare";
                            break;

                        case 4:
                            lblStatusLabel.Text = "Elaborazione dei dati in corso";
                            break;
                    }
                }
                else if (stato == 5)
                {
                    lblStatusLabel.Text = "Esecuzione terminata. Per eseguire un'altra analisi selezionare   Manage -> Riavvia   o chiudere il programma.";
                }
                else
                {
                    lblStatusLabel.Text = "In attesa di connessione  -  è consigliata la scelta dei sensori e dello smooth prima dell'avvio e non durante l'esecuzione.";
                }
            }
        }

        public void setStazLabel(string value)
        {
            if (this.rtbStazLog.InvokeRequired)
            {
                setStazLabel_ d = new setStazLabel_(setStazLabel);
                this.rtbStazLog.Invoke(d, value);
            }
            else
            {
                rtbStazLog.Text = rtbStazLog.Text + " " + value;
                rtbPosLog.SelectionStart = rtbPosLog.Text.Length;
                rtbPosLog.ScrollToCaret();

                rtbGirataLog.SelectionStart = rtbGirataLog.Text.Length;
                rtbGirataLog.ScrollToCaret();
            }
        }

        public void setPosLabel(string value)
        {
            if (this.rtbPosLog.InvokeRequired)
            {
                setPosLabel_ d = new setPosLabel_(setPosLabel);
                this.rtbPosLog.Invoke(d, value);
            }
            else
            {
                rtbPosLog.Text = rtbPosLog.Text + " " + value;
                rtbPosLog.SelectionStart = rtbPosLog.Text.Length;
                rtbPosLog.ScrollToCaret();

                rtbGirataLog.SelectionStart = rtbGirataLog.Text.Length;
                rtbGirataLog.ScrollToCaret();
            }
        }

        public void setGirataLabel(string value)
        {
            if (this.rtbGirataLog.InvokeRequired)
            {
                setGirataLabel_ d = new setGirataLabel_(setGirataLabel);
                this.rtbGirataLog.Invoke(d, value);
            }
            else
            {
                rtbGirataLog.Text = rtbGirataLog.Text + " " + value;
                rtbGirataLog.SelectionStart = rtbGirataLog.Text.Length;
                rtbGirataLog.ScrollToCaret();
            }
        }

        public void setEuleroLabel(string value)
        {
            if (this.rtbEuleroLog.InvokeRequired)
            {
                setEuleroLabel_ d = new setEuleroLabel_(setEuleroLabel);
                this.rtbEuleroLog.Invoke(d, value);
            }
            else
            {
                rtbEuleroLog.Text = value;
                rtbEuleroLog.SelectionStart = rtbEuleroLog.Text.Length;
                rtbEuleroLog.ScrollToCaret();
            }
        }

        public int getSensoreAccellerometro()
        {
            if (a0.Checked)
                sensoreAccellerometro = 0;
            else if (a1.Checked)
                sensoreAccellerometro = 1;
            else if (a2.Checked)
                sensoreAccellerometro = 2;
            else if (a3.Checked)
                sensoreAccellerometro = 3;
            else
                sensoreAccellerometro = 4;

            return sensoreAccellerometro;
        }

        public int getSensoreGiroscopio()
        {
            if (g0.Checked)
                sensoreGiroscopio = 0;
            else if (g1.Checked)
                sensoreGiroscopio = 1;
            else if (g2.Checked)
                sensoreGiroscopio = 2;
            else if (g3.Checked)
                sensoreGiroscopio = 3;
            else
                sensoreGiroscopio = 4;

            return sensoreGiroscopio;
        }

        public int getSensoreMagnetometro()
        {
            if (m0.Checked)
                sensoreMagnetometro = 0;
            else if (m1.Checked)
                sensoreMagnetometro = 1;
            else if (m2.Checked)
                sensoreMagnetometro = 2;
            else if (m3.Checked)
                sensoreMagnetometro = 3;
            else
                sensoreMagnetometro = 4;

            return sensoreMagnetometro;
        }

        public int getValoreSmooth()
        {
            if (chkSmooth.Checked)
            {
                int smoothAttuale;
                Int32.TryParse(txtValoreSmooth.Text, out smoothAttuale);
                valoreSmooth = smoothAttuale;
                return smoothAttuale;
            }
            else
                return 0;

        }

        public void stampaInfo(String message)
        {
            string caption = "Information";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Information;
            MessageBox.Show(message, caption, buttons, icons);
        }

        public void stampaWarning(String message)
        {
            string caption = "Press OK to restart";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBoxIcon icons = MessageBoxIcon.Warning;
            MessageBox.Show(message, caption, buttons, icons);
        }
    }
}