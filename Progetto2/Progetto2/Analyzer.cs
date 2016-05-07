using System;
using System.Threading;
using System.IO;
using System.Collections;

namespace Progetto2
{
    // Analizzatore dei dati.
    // E' il cervello dell'applicazione in quanto contiene i metodi per l'interpretazione
    // dei dati acquisiti.
    class Analyzer
    {
        private Container campioni;  //struttura condivisa
        private string id;          
        private const int NMAX = 500;   //rappresento massimo 500 campioni alla volta
        private int freq, TEMPO = 0, start, N = NMAX;
        private bool stop = false, fine; // flag utili per la sincronizzazione dell'analisi
        StreamWriter sw;
        private float[,,] sampwin; // finestra d'analis i campioni N, sensori (5), dati (13: 3 acc, 3 gyr, 3 magn, 4 quaternioni)

        // Utili per l'analisi nei Log della finestra.
        private bool ultimoStaz = false; 
        private float ultimoPos = -1;
        private float ultimaGir = 0;
        private float ultimaGirFiss = 0;
        private double ultimoPosTemp = 0;
        private float ultimoRoll = 0;
        private float ultimoPitch = 0;
        private float ultimoYaw = 0;
        private int end;

        // Utili per il riconoscimento di una girata.
        String tempoInizio = "00:00:00";
        String girata;
        String segno = "£";

        ManualResetEvent manage; // gestione eventi per mettere in pausa o riprende l'esecuzione dell'analisi

        public Analyzer(Container campioni, ManualResetEvent manage)
        {
            this.campioni = campioni;
            sampwin = new float[N, 5, 13];
            this.manage=manage;

            // Ripulisco il grafico ogni volta che incomincio un'analisi
            try
            {
                Program.myForm.clear();
            } catch(Exception)
            {
                Console.WriteLine("Già pulito"); // Eccezione dovuta alla fretta di connettersi
            }
        }

        // metodo eseguito dal thread di analisi
        public void inizia()
        {
            // L'accesso ai campioni, essendo condivisi da più thread, deve essere sincronizzato.
            lock (campioni)
            {
                // rimane in attesa di leggere la frequenza
                while (Container.freq == 0)
                {
                    Monitor.Wait(campioni);
                }
                this.freq = Container.freq;
                this.id = Container.id;
                Monitor.Pulse(campioni);
            }
            // Finchè ho campioni da analizzare
            while (stop == false)
            {

                lock (campioni)
                {
                    // rimane in attesa di avere il numero di campioni necessario
                    // se ricevo l'ultimo campione proseguo
                    while (campioni.campioni.Count < NMAX && campioni.fine == false)
                    {
                        Monitor.Wait(campioni);
                    }
                    fine = campioni.fine;

                    // controllo se ho ricevuto l'ultimo campione dello streaming di dati
                    // imposto N uguale al nuovo numero di campioni della finestra
                    if (fine == true)
                    {
                        if (campioni.campioni.Count <= NMAX)
                        {
                            N = campioni.campioni.Count;
                        }
                    }
                    // se inizio la trasmissione di dati comincio a scrivere
                    if (campioni.start == 1)
                    {
                        sw = new StreamWriter(id + DateTime.Now.ToString("_d-M-yyyy_HH.mm.ss_") + freq.ToString() + "Hz.csv");

                        // vuol dire che ho ricevuto dei dati
                        campioni.start = 2;
                    }
                    // inizio procedura di salvataggio dei dati nella finestra d'analisi
                    for (int t = 0; t < N; t++)
                    {
                        for (int s = 0; s < 5; s++)
                        {
                            for (int i = 0; i < 13; i++)
                            {
                                float[,] temp = campioni.leggi(t);
                                sampwin[t, s, i] = temp[i, s];
                            }
                        }
                    }


                    if (N == NMAX)
                    {
                        campioni.swap(N / 2);
                    }
                    else
                    {
                        campioni.swap(N);
                    }
                    start = campioni.start;
                    Monitor.Pulse(campioni);
                }
                if (start != 0)
                {
                    // analisi vera e propria:

                    // aggiorna il csv aggiungendo i dati.
                    aggiornaCSV();
                    // imposta titoli dei pannelli dei grafici
                    Program.myForm.setTitlesGraph(new string[] { "Accelerometro", "Giroscopio", "Magnetometro","Posizionamento"});
                    // salvo le scelte dell'utente su smooth e sensori
                    int valoreSmooth = Program.myForm.getValoreSmooth();
                    int sensoreModuloAcc = Program.myForm.getSensoreAccellerometro();
                    int sensoreModuloGyr = Program.myForm.getSensoreGiroscopio();
                    int sensoreMagn = Program.myForm.getSensoreMagnetometro();
                    // in questa lista inserisco i dati da passare al grafo per disegnarli
                    ArrayList ar = new ArrayList();
                    // se ho uno smooth diverso da 0 allora filtro i dati prima di disegnarli.
                    if (valoreSmooth != 0)
                    {
                        ar.Add(smooth(valoreSmooth, moduloAcc(sensoreModuloAcc)));
                        ar.Add(smooth(valoreSmooth, moduloGyr(sensoreModuloGyr)));
                        ar.Add(smooth(valoreSmooth, orientaMag(sensoreMagn)));
                    }
                    else
                    {
                        ar.Add(moduloAcc(sensoreModuloAcc));
                        ar.Add(moduloGyr(sensoreModuloGyr));
                        ar.Add(orientaMag(sensoreMagn));
                    }
                    
                    ar.Add(laySitStand());

                    // creo un thread per ogni finestra di Log presente sulla form
                    Thread printStaz = new Thread(new ParameterizedThreadStart(stampaStazionamento));
                    printStaz.Start(new object[] { riconosciStazionamento() });

                    Thread printPos = new Thread(new ParameterizedThreadStart(stampaPosizionamento));
                    printPos.Start(new object[] { laySitStand() });

                    Thread printGir = new Thread(new ParameterizedThreadStart(stampaOrientamento));
                    printGir.Start(new object[] { orientamento() });

                    Thread printEulero = new Thread(new ParameterizedThreadStart(stampaEulero));
                    printEulero.Start(new object[] { angoliEulero() });

                    object[] array = ar.ToArray();

                    manage.WaitOne(); // si ferma se è stato messo in pausa

                    // metodo d'appoggio per l'aggiornamento dei grafici
                    // parto a disegnare dall'istante '0'
                    aggiornaGrafici(TEMPO, array);

                    if (N == NMAX)
                    {
                        TEMPO = TEMPO + N / 2;
                    }
                    else
                    {
                        TEMPO = TEMPO + N;
                    }

                    // ho finito di analizzare questi campioni.
                    if (campioni.campioni.Count == 0)
                    {
                        sw.Close();
                        stop = true;
                        campioni.start = 0;
                        campioni.fine = false;
                        Container.id = null;
                        Container.freq = 0;
                    }
                }
                else
                {
                    stop = true;
                }
            }
            stampaUltimo();
            // stampo su una finestra di avviso la fine dell'analisi
            Program.myForm.stampaWarning("Analisi dei dati terminata");
            
        }

        // Procedura per inviare i dati ai grafici.
        private void aggiornaGrafici(int tempo, object[] dati)
        {
            int numGraph = dati.Length;
            object[] graphData = new object[numGraph];
            int lung = 0;
            if (N < NMAX) // minore di 500: passo l'array con i campioni della dimensione N 
            {
                graphData = dati;
                lung = N;
            }
            else // passo l'array con i campioni con la dimensione N/2 + 1
            {
                for (int x = 0; x < numGraph; x++)
                {
                    float[] da = (float[])dati[x];
                    float[] a = new float[(N / 2) + 1];
                    Array.Copy(da, a, (N / 2) + 1);
                    graphData[x] = a;
                }
                lung = (N / 2) + 1;
            }

            float time = tempo / 50;

            for (int i = 0; i < lung - 5; i = i + 5)
            {

                object[] subGraphData = new object[numGraph];

                for (int x = 0; x < numGraph; x++)
                {
                    float[] da = (float[])graphData[x];
                    float[] a = new float[6];
                    Array.Copy(da, i, a, 0, 6);
                    subGraphData[x] = a;
                }
                manage.WaitOne(); // si ferma se è stato messo in pausa
                Program.myForm.writeChart(subGraphData, time);
                time = time + (float)(0.02 * 5);
                // attendi 0,02 s
                Thread.Sleep(20);
            }
        }

        // Metodi per la stampa nelle finestre di Log:

        // Log della girata.
        private void stampaOrientamento(object orien)
        {
            float[] or = (float[])((object[])orien)[0];
            int i = 0;
            try
            {
                
                foreach(float o in or)
                {
                    if (segno.Equals("£"))
                    {
                        if (or[1] > or[0])
                        {
                            segno = "+"; //sinistra
                        }
                        else
                        {
                            segno = "-"; //destra
                        }
                        ultimaGirFiss = (float)Math.Round(o, 3);
                        ultimaGir = ultimaGirFiss;
                    }
                    else
                    {
                        if ((((segno.Equals("+")) && (Math.Round(o, 3) > ultimaGir)) || ((segno.Equals("-")) && (Math.Round(o, 3) < ultimaGir)))==false)
                        {
                            if (segno.Equals("+"))
                            {
                                girata = "sinistra";
                                segno = "-";
                            }
                            else
                            {
                                girata = "destra";
                                segno = "+";
                            }
                            double angolo = Math.Abs(Math.Round(o, 3) - ultimaGirFiss);
                            double angoloGradi = toGradi((float)angolo);
                            TimeSpan tempo = TimeSpan.FromSeconds((double)(TEMPO + i) / 50);
                            if (angoloGradi >= 10) // la stampo solo se è stata una girata significativa
                            {
                                Program.myForm.setGirataLabel(tempoInizio + " - " + tempo.ToString(@"mm\:ss\,fff") + "\n" + girata + " di " + angoloGradi + "\n");
                            }
                            tempoInizio = tempo.ToString(@"mm\:ss\,fff");
                            ultimaGirFiss = (float)Math.Round(o, 3);
                            //Console.WriteLine("girata fissa: "+ultimaGirFiss);
                        }
                        ultimaGir= (float)Math.Round(o, 3);
                    }
                    //Console.WriteLine(o);
                    Thread.Sleep(20); // attendo 0,02s per sincronizzare ogni campione con il tempo reale
                    i++;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> Exception, metodo:");
                Console.WriteLine("private void stampaOrientamento(object orien)");
            }
            end = i;
        }

        // Metodo ausiliario della sgtampa sopra, per stampare anche l'ultima girata che altrimenti
        // non viene presa.
        private void stampaUltimo()
        {
            double angolo = Math.Abs(ultimaGir - ultimaGirFiss);
            double angoloGradi = toGradi((float)angolo);
            TimeSpan tempo = TimeSpan.FromSeconds((double)(TEMPO + end) / 50);
            if (segno.Equals("+"))
            {
                girata = "sinistra";
            }
            else
            {
                girata = "destra";
            }
            if (angoloGradi >= 10) // la stampo solo se è stata una girata significativa
            {
                Program.myForm.setGirataLabel(tempoInizio + " - " + tempo.ToString(@"mm\:ss\,fff") + "\n" + girata + " di " + angoloGradi + "\n");
            }
        }

        // Log degli angoli d'Eulero (non particolarmente esplicativo)
        private void stampaEulero(object angoliEu)
        {
            float[][] angoli = (float[][])((object[])angoliEu)[0];
            int i = 0;
            foreach(float[] valori in angoli)
            {
                if ((i%100) == 0)
                {
                    Program.myForm.setEuleroLabel("Roll: " + valori[0] + "\nPitch: " + valori[1] + "\nYaw: " + valori[2]);
                    ultimoRoll = valori[0];
                    ultimoPitch = valori[1];
                    ultimoYaw = valori[2];
                }
                i++;
                Thread.Sleep(20); // attendo 0,02s per sincronizzare ogni campione con il tempo reale
            }
        }

        // Log per lo stazionamento
        private void stampaStazionamento(object sta)
        {
            bool[] staz = (bool[])((object[])sta)[0];
            int i = 0;
            try
            {
                foreach(bool s in staz)
                {
                    if (s!= ultimoStaz)
                    {
                        TimeSpan tempo = TimeSpan.FromSeconds((double)(TEMPO + i) / 50);
                        if (s)
                            Program.myForm.setStazLabel(tempo.ToString(@"mm\:ss\,fff") + " - Soggetto fermo\n");
                        else
                            Program.myForm.setStazLabel(tempo.ToString(@"mm\:ss\,fff") + " - Soggetto non fermo\n");
                        ultimoStaz = s;
                    }
                    Thread.Sleep(20); // attendo 0,02s per sincronizzare ogni campione con il tempo reale
                    i++;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Eccezione generata -> Exception, metodo:");
                Console.WriteLine("private void stampaStazionamento(object sta)");
            }
        }

        // Log per il posizionamento
        private void stampaPosizionamento(object pos)
        {
            float[] posz = (float[])((object[])pos)[0];
            int i = 0;
            try
            {
                foreach(float p in posz)
                {
                    if (p != ultimoPos)
                    {
                        TimeSpan tempo = TimeSpan.FromSeconds((double)(TEMPO + i) / 50);
                        double tempoAtt = tempo.TotalMilliseconds;
                        if ((tempoAtt - ultimoPosTemp < 100) && ((ultimoPos == 2) || (ultimoPos == 1)) && ((int)p == 0))
                        {
                            Program.myForm.setPosLabel(tempo.ToString(@"mm\:ss\,fff") + " - E' caduto\n");
                        }
                        switch ((int)p)
                        {
                            case 0:
                                Program.myForm.setPosLabel(tempo.ToString(@"mm\:ss\,fff") + " - Sdraiato\n");
                                break;
                            case 1:
                                Program.myForm.setPosLabel(tempo.ToString(@"mm\:ss\,fff") + " - Sdraiato/seduto\n");
                                break;
                            case 2:
                                Program.myForm.setPosLabel(tempo.ToString(@"mm\:ss\,fff") + " - Seduto\n");
                                break;
                            case 3:
                                Program.myForm.setPosLabel(tempo.ToString(@"mm\:ss\,fff") + " - In piedi\n");
                                break;
                        }
                        ultimoPos = p;
                        ultimoPosTemp = tempoAtt;
                    }
                    Thread.Sleep(20); // attendo 0,02s per sincronizzare ogni campione con il tempo reale
                    i++;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Eccezione generata -> Exception, metodo:");
                Console.WriteLine("private void stampaPosizionamento(object pos)");
            }
        }

        // Scrittura su *file*.csv
        // utilizza lo StreamWriter per scrivere su file...
        private void aggiornaCSV()
        {
            try
            {
                for (int t = 0; t < N; t++)
                {
                    for (int s = 0; s < 5; s++)
                    {
                        if (sampwin[t,s,0] != 0) // per evitare la scrittura di dati errati e non gestire eccezioni
                                                 // del tipo IndexOutOfBound
                        {
                            for (int i = 0; i < 13; i++)
                            {
                                sw.Write(sampwin[t, s, i].ToString());
                                sw.Write(";");
                            }
                            sw.Write(";");
                        }
                    }
                    sw.WriteLine(";");
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private void aggiornaCSV()");
            }
        }

        // Modulo accelerometro
        // Crea l'array di float con i dati da disegnare sul primo grafo.
        private float[] moduloAcc(int s)
        {
            float[] moduloAcc = new float[N];
            try
            {

                for (int t = 0; t < N; t++)
                {
                    moduloAcc[t] = (float)Math.Sqrt(Math.Pow(sampwin[t, s, 0], 2) + Math.Pow(sampwin[t, s, 1], 2) + Math.Pow(sampwin[t, s, 2], 2));
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] moduloAcc(int s)");
            }
            return moduloAcc;
        }

        // Modulo giroscopio
        // Crea l'aaray di valori da disegnare per il secondo grafico
        private float[] moduloGyr(int s)
        {
            float[] moduloGyr = new float[N];
            try
            {
                for (int t = 0; t < N; t++)
                {
                    moduloGyr[t] = (float)Math.Sqrt(Math.Pow(sampwin[t, s, 3], 2) + Math.Pow(sampwin[t, s, 4], 2) + Math.Pow(sampwin[t, s, 5], 2));
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] moduloGyr(int s)");
            }
            return moduloGyr;
        }

        // Orientamento Magnetometro.
        // Crea l'array di dati da disegnare sul terzo grafo
        private float[] orientaMag(int s)
        {
            float[] orientaMag = new float[N];

            try
            {

                for (int t = 0; t < N; t++)
                {
                    orientaMag[t] = (float)Math.Atan(sampwin[t, s, 7] / sampwin[t, s, 8]);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] orientaMag(int s)");
            }
            return orientaMag;
        }

        // Il quarto grafo del laySitStand è gestito da un'unica funzione sia per il disegno che per la stampa.
        // (più in fondo nel codice)

        // Operazione di smoothing
        // se smooth=m e i è la posizione attuale, salva in i il valore dato dalla media
        // degli m valori prima, m valori dopo e il valore stesso.
        private float[] smooth(int k, float[] data)
        {
            float[] smooth = new float[data.Length];

            try
            {
                for (int t = 0; t < data.Length; t++)
                {
                    float totale = 0;
                    int n = 0;
                    for (int i = t - k; i <= t + k; i++)
                    {
                        if (i >= 0 && i < data.Length)
                        {
                            totale += data[i];
                            n++;
                        }
                    }
                    smooth[t] = totale / n;
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] smooth(int k, float[] data)");
            }
            return smooth;
        }

        // Rapporto incrementale
        // Per semplicita l'incremento è sempre 1.
        // E' calcolato tra un valore e il successivo.
        private float[] RIfunc(float[] data)
        {
            float[] RI = new float[data.Length];
            try
            {
                for (int i = 0; i < data.Length - 1; i++)
                {
                    RI[i] = data[i + 1] - data[i];
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] RIfunc(float[] data)");
            }
            return RI;
        }

        // Deviazione standard con media mobile (uso lo smooth per calcolare la media per ogni posizione)
        // e finestra mobile.
        private float[] stDev(int k, float[] data)
        {
            float[] stDev = new float[data.Length];
            float[] media = smooth(k, data);

            try
            {
                for (int t = 0; t < data.Length; t++)
                {
                    float valore;
                    float totale = 0;
                    int n = 0;
                    for (int i = t - k; i <= t + k; i++)
                    {
                        if (i >= 0 && i < data.Length)
                        {
                            valore = data[i] - media[i];
                            totale += (float)Math.Pow(valore, 2);
                            n++;
                        }
                    }
                    stDev[t] = (float)Math.Sqrt(totale / n);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] stDev(int k, float[] data)");
            }
            return stDev;
        }

        // Deviazione standard con media fissa e finestra mobile.
        // stesso ragionamento di prima ma senza funzione smooth per il calcolo della media.
        private float[] stDev(int k, float[] data, float media)
        {
            float[] stDev = new float[data.Length];

            try
            {
                for (int t = 0; t < data.Length; t++)
                {
                    float valore;
                    float totale = 0;
                    int n = 0;
                    for (int i = t - k; i <= t + k; i++)
                    {
                        if (i >= 0 && i < data.Length)
                        {
                            valore = data[i] - media;
                            totale += (float)Math.Pow(valore, 2);
                            n++;
                        }
                    }
                    stDev[t] = (float)Math.Sqrt(totale / n);
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] stDev(int k, float[] data, float media)");
            }
            return stDev;
        }

        // Metodi per il riconoscimento dei dati da stampare in Log.

        // Riconosci stazionamento.
        // Il soggetto può essere fermo o non fermo, struttura si/no, dunque uso un'array di boolean
        // true  -> fermo
        // false -> non fermo
        // Uso l'algoritmo di deviazione standard media mobile finestra mobile.
        private bool[] riconosciStazionamento()
        {
            bool[] stazionamento = new bool[N];
            float[] deviazione = stDev(25, moduloAcc(Program.myForm.getSensoreAccellerometro()), (float) 9.81);
            bool statoPrecedente = true; //parto con il soggetto fermo

            try
            {
                for (int i = 0; i < N; i++)
                {
                    if (deviazione[i] > 0.7 && deviazione[i] < 1.3)
                    {
                        stazionamento[i] = statoPrecedente; //questo if mi serve per filtrare i dati e evitare errori.
                    }
                    else if (deviazione[i] < 1)
                    {
                        stazionamento[i] = true; // "soggetto fermo"
                    }
                    else
                    {
                        stazionamento[i] = false; // "soggetto in movimento"
                    }
                    statoPrecedente = stazionamento[i];
                }
            } catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private bool[] riconosciStazionamento()");
            }
            return stazionamento;
        }

        // Riconosce la girata
        // Filtro i dati disegnati in base alla differenza in valore assoluto: se è piccola non la riporto.
        private float[] orientamento()
        {
            float[] arcotg = orientaMag(Program.myForm.getSensoreMagnetometro()); // dati dal magnetometro, resistuisce l'arcotangente di y / z
            float[] arctan = new float[N]; // dati arcotangente da stampare

            try
            {

                for (int i = 0; i < N; i++)
                {
                    if (i == 0)
                    {
                        arctan[i] = (float) Math.Round(ultimaGir, 3);
                    }
                    else
                    {
                        // controlla il salto con la differenza assoluta tra successore e a attuale
                        if (Math.Abs(arcotg[i] - arcotg[i-1]) < 0.1)
                        {
                            arctan[i] = arctan[i - 1];
                        }
                        else
                        {
                            arctan[i] = arcotg[i];
                        }
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] orientamento()");
            }
            return arctan;
        }

        // Questa funzione viene usata sia per disegnare che per riconoscere la posizione.
        // In questo caso il sensore è sempre lo stesso.
        private float[] laySitStand()
        {                            
            // convenzione:           
            // LAY (sdraiato)   -> 0
            // LAY/SIT          -> 1
            // SIT (seduto)     -> 2
            // STAND (in piedi) -> 3
            float[] posizionamento = new float[N];
            float[] datiDaElaborare = new float[N];

            try
            {
                for (int z = 0; z < N; z++)
                {
                    datiDaElaborare[z] = sampwin[z, 0, 0];
                }


                datiDaElaborare = smooth(10, datiDaElaborare);

                for (int i = 0; i < N; i++)
                {
                        if (datiDaElaborare[i] <= 2.7) 
                        {
                            posizionamento[i] = 0;
                        }
                        else if (datiDaElaborare[i] > 2.7 && datiDaElaborare[i] <= 3.7)
                        {
                            posizionamento[i] = 1;
                        }
                        else if (datiDaElaborare[i] > 3.7 && datiDaElaborare[i] <= 7)
                        {
                            posizionamento[i] = 2;
                        }
                        else
                        {
                            posizionamento[i] = 3;
                        }
                    
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Eccezione generata -> IndexOutOfRangeException, metodo:");
                Console.WriteLine("private float[] laySitStand()");
            }
            return posizionamento;
        }

        // Riconosci angoli d'eulero
        // Rtitorna gli angoli di eulero caclolati per ogni campione (roll, pitch, yaw)
        // in gradi.
        private float[][] angoliEulero()
        {
            float[][] valori = new float[N][]; //3: roll, pitch, yaw
            for(int i = 0; i < N; i++)
            {
                valori[i] = new float[3];
            }
            float roll;
            float pitch;
            float yaw;
            float q0, q1, q2, q3;
            float[] dati = new float[N];
            for (int t=0; t<N; t++)
            {
                    q0 = sampwin[t, 0, 9];
                    q1 = sampwin[t, 0, 10];
                    q2 = sampwin[t, 0, 11];
                    q3 = sampwin[t, 0, 12];
                    roll = (float)Math.Atan((2 * q2 * q3 + 2 * q0 * q1) / (Math.Pow(q0, 2) * 2 + Math.Pow(q3, 2) * 2 - 1));
                    pitch = (float)-Math.Asin(2 * q1 * q3 - q0 * q2);
                    yaw = (float)Math.Atan((2 * q1 * q2 + 2 * q0 * q3) / (Math.Pow(q0, 2) * 2 + Math.Pow(q1, 2) * 2 - 1));
                    valori[t][0] = (float) toGradi(roll);
                    valori[t][1] = (float) toGradi(pitch);
                    valori[t][2] = (float) toGradi(yaw);
            }
            return valori;
        }

        // I gradi sono più facili da comprendere rispetto ai radianti quindi mi creo 
        // questa funzione per passare da radianti a gradi.
        private double toGradi(float radianti)
        {
            return Math.Round((radianti * 180) / Math.PI, 0);
        }
    }
}

