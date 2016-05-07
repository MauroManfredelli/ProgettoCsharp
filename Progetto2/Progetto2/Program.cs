using System;
using System.Windows.Forms;
using System.Threading;

namespace Progetto2
{
    // Classe che contiene il main, entry point del programma
    // Inizializza la finestra e il server.
    // Uso Application come classe di supporto.
    static class Program
    {
        public static MyForm myForm;
        public static int port = 45555;
        private static Thread serverThread;
        private static Server server;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Creo un nuovo server e lancio un thread
            server = new Server(port);
            start();

            myForm = new MyForm();
            myForm.setStatus(false, 0);
            Application.Run(myForm);

        }

        public static void start()
        {
            if (serverThread == null || !serverThread.IsAlive)
            {
                serverThread = new Thread(new ThreadStart(server.Start));
                serverThread.Start();
            }
        }

        public static void close()
        {
            serverThread.Abort();
            server.Stop();
        }

        public static void resume()
        {
            server.Resume();
        }

        public static void pause()
        {
            server.Pause();
        }
    }
}
