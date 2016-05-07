using System;
using System.Collections;
using System.Collections.Generic;

namespace Progetto2
{
    // Classe condivisa: contiene i campioni che il ricevitores scrive e l'analizzatore legge.
    class Container
    {
        public static int freq; // statica così la si può leggere dal form senza usare variabili interne
        public static string id; // statica così la si può leggere dal form senza usare variabili interne
        public ArrayList campioni;

        public bool fine = false;
        // START
        // 0 nessun invio
        // 1 invio primo campione alla finestra csv non ancora creato
        // 2 ricevuto primo campione + creato csv
        public int start = 0;
        public Container()
        {
            campioni = new ArrayList();
        }
        
        public float[,] leggi(int index)
        {
            float[,] letto = (float[,])campioni[index];
            return letto;
        }
        
        public void swap(int num)
        {
            campioni.RemoveRange(0, num);
        }

        internal void scrivi(List<List<float>> list)
        {
            float[,] matrice = new float[13, 5];
            for (int i = 0; i < 5; i++)
            {
                List<float> tmp = list[i];
                for (int j = 0; j < 13; j++)
                {
                    matrice[j, i] = tmp[j];
                }
            }
            campioni.Add(matrice);
        }
    }
}
