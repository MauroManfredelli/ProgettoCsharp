using System;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Windows.Forms;

namespace Progetto2
{
    // Si mette in ascolto sulla porta 45555
    // Crea due thread, uno per leggere i dati e l'altro per analizzarli.
    // Entrambi condividono il container con i campioni.
    class Server
    {
        private readonly int port;
        TcpListener listener;
        Socket socket;
        Thread receiver, analyzer;
        ManualResetEvent manage; // gestione eventi per mettere in pausa o riprende l'esecuzione dell'analyzer
        public Server(int port)
        {
            this.port = port;
            this.manage = new ManualResetEvent(true);
        }

        public void Start()
        {
            try
            {
                listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
                while (true)
                {
                    listener.Start(0);
                    //accetto un nuovo socket;
                    socket = listener.AcceptSocket();

                    Container campioni = new Container();

                    //creo un thread per ricevere i dati
                    Receiver ricevi = new Receiver(campioni);
                    receiver = new Thread(new ParameterizedThreadStart(ricevi.leggiDati));
                    receiver.Start(socket);
                    //creo un thread per l'analisi dei dati
                    Analyzer analizza = new Analyzer(campioni, manage);
                    analyzer = new Thread(new ThreadStart(analizza.inizia));
                    analyzer.Start();

                    // attende che riceve finisca
                    receiver.Join();
                    listener.Stop();
                    // attende che analizza finisca
                    analyzer.Join();

                    // Imposto stato sul form
                    Program.myForm.setStatus(false, 0);

                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Il server è già avviato", "ERRORE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Stop()
        {
            if (socket != null)
            {
                receiver.Abort();
                analyzer.Abort();
                socket.Close();
            }
            listener.Stop();
        }

        public void Pause()
        {
            manage.Reset();
        }

        public void Resume()
        {
            manage.Set();
        }
    }
}
