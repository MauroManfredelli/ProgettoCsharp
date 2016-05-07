using System;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;

namespace Progetto2
{
    // Classe di ricezione
    // Si basa sul materiale fornito a lezione e sul tema d'esame.
    // Nei dati forniti i sensori sono al massimo 5, ma la costante ho preferito
    // impostarla a 10.
    class Receiver
    {
        private int byteToRead;

        private Container campioni;
        private readonly int maxSensori=10;
        private int numSensori;
        private byte[] pacchetto;

        public Receiver(Container campioni)
        {
            this.campioni= campioni;
        }

        // Unico metodo: lettura dei dati.
        public void leggiDati(object obj)
        {
            Socket socket = (Socket)obj;
            using (Stream stream = new NetworkStream(socket))
            using (BinaryReader bin = new BinaryReader(stream))
            {
                try
                {
                    byte[] len = new byte[2];
                    byte[] tem = new byte[3];

                    byte[] id = bin.ReadBytes(10); //bloccante
                    byte[] freq = bin.ReadBytes(4);
                    string s = System.Text.ASCIIEncoding.ASCII.GetString(id);
                    int f = BitConverter.ToInt32(freq, 0);
                    Console.WriteLine(f);

                    lock (campioni)
                    {
                        Container.id = s;
                        Container.freq = f;
                        Monitor.Pulse(campioni);
                    }
                    
                    Program.myForm.setStatus(true, 1);

                    while (!(tem[0] == 0xFF && tem[1] == 0x32)) // cerca la sequenza FF-32
                    {
                        tem[0] = tem[1];
                        tem[1] = tem[2];
                        byte[] read = bin.ReadBytes(1);
                        tem[2] = read[0];
                    }
                    if (tem[2] != 0xFF) // modalità normale
                    {
                        byteToRead = tem[2]; // byte da leggere
                    }
                    else  // modalità extended-length
                    {
                        len = new byte[2];
                        len = bin.ReadBytes(2);
                        byteToRead = (len[0] * 256) + len[1]; // byte da leggere
                    }

                    byte[] data = new byte[byteToRead + 1];
                    data = bin.ReadBytes(byteToRead + 1); // lettura dei dati

                    if (tem[2] != 0xFF)
                    {
                        pacchetto = new byte[byteToRead + 4]; // creazione pacchetto
                    }
                    else
                    {
                        pacchetto = new byte[byteToRead + 6];
                    }

                    numSensori = (byteToRead - 2) / 52; // calcolo del numero di sensori
                    pacchetto[0] = 0xFF; // copia dei primi elementi
                    pacchetto[1] = 0x32;
                    pacchetto[2] = tem[2];

                    if (tem[2] != 0xFF)
                    {
                        data.CopyTo(pacchetto, 3); // copia dei dati
                    }
                    else
                    {
                        pacchetto[3] = len[0];
                        pacchetto[4] = len[1];
                        data.CopyTo(pacchetto, 5); // copia dei dati
                    }


                    List<List<float>> array = new List<List<float>>(); // salvataggio dati


                    int[] t = new int[maxSensori];

                    for (int x = 0; x < numSensori; x++)
                    {
                        array.Add(new List<float>()); // una lista per ogni sensore
                        t[x] = 5 + (52 * x);
                    }
                    if (campioni.start == 0)
                        Program.myForm.setStatus(true, 2);
                    while (true)
                    {
                        for (int i = 0; i < numSensori; i++)
                        {
                            byte[] temp = new byte[4];
                            for (int tr = 0; tr < 13; tr++)// 13 campi, 3 * 3 + 4
                            {
                                if (numSensori < 5)
                                {
                                    temp[0] = pacchetto[t[i] + 3]; // lettura inversa
                                    temp[1] = pacchetto[t[i] + 2];
                                    temp[2] = pacchetto[t[i] + 1];
                                    temp[3] = pacchetto[t[i]];
                                }
                                else
                                {
                                    temp[0] = pacchetto[t[i] + 5];
                                    temp[1] = pacchetto[t[i] + 4];
                                    temp[2] = pacchetto[t[i] + 3];
                                    temp[3] = pacchetto[t[i] + 2];
                                }

                                float valore = BitConverter.ToSingle(temp, 0);
                                array[i].Add(valore); // memorizzazione
                                
                                t[i] += 4;
                            }
                        }
                        
                        for (int x = 0; x < numSensori; x++)
                        {
                            t[x] = 5 + (52 * x);
                        }
                        lock (campioni)
                        {
                            campioni.scrivi(array);
                            if (campioni.campioni.Count == 500)
                            {
                                // se è la prima volta che passo qua la metto a true
                                if (campioni.start == 0)
                                {
                                    campioni.start = 1;
                                }
                                // ho scritto 500 campioni quindi sveglio l'analizzatore
                                Monitor.Pulse(campioni);
                            }
                        }

                        // questo ciclo era la stampa di prova;
                        // di esso mantengo la swap.
                        for (int j = 0; j < numSensori; j++)
                        {
                            for (int tr = 0; tr < 13; tr++)
                            {
                                // esempio output su console
                                //Console.Write(array[j][tr] + "; ");
                            }
                            //Console.WriteLine();
                            array[j].RemoveRange(0, 13); // cancellazione dati
                        }

                        //Console.WriteLine();

                        if (numSensori < 5) // lettura pacchetto seguente
                        {
                            pacchetto = bin.ReadBytes(byteToRead + 4);
                        }
                        else
                        {
                            pacchetto = bin.ReadBytes(byteToRead + 6);
                        }

                        lock (campioni)
                        {
                            campioni.fine = true;
                            // se è la prima volta che passo qua la metto a true
                            // 0 nessun invio
                            // 1 invio primo campione alla finestra, csv non ancora creato
                            // 2 ricevuto primo campione + creato csv
                            if (campioni.start == 0 && campioni.campioni.Count != 0)
                            {
                                campioni.start = 1;
                            }
                            Monitor.Pulse(campioni);
                        }
                    }

                }
                catch (Exception)
                {
                    Program.myForm.stampaInfo("Lettura dei dati: operazione conclusa");
                }
            }
        }
    }
}
