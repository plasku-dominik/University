using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


// Megszámolja egy vektorban a páros és páratlan számokat! Két metódus kell,
// az egyik elejéről, a másik a közepétől. Közös parosdb, paratlandb változókban számolják.
namespace elso
{
    internal class Program
    {
        const int N = 1000000;
        static int[] a = new int[N];
        [ThreadStatic]
        static int szum = 0;

        static void Felso(ref int sum)
        {
            for (int i = 0; i < N / 2; i++)
            {
                  szum += a[i]; // szum=szum+a[i]  10+11
                  //Interlocked.Add(ref szum, a[i]);
            }
            sum = szum;
            //Console.WriteLine(szum);

        }

        static void Also(ref int sum)
        {
            for (int i = N / 2; i < N; i++)
            {
                  szum += a[i];  //szum=20+1
                  //Interlocked.Add(ref szum, a[i]);

            }
            sum = szum;
        }

        static void Main(string[] args)
        {
            
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = 1;
            }
           // Felso();Also();
           // Console.WriteLine(szum);
            int szum1 = 0;
            int szum2 = 0;
            Thread t1=new Thread(()=>Also(ref szum1));
            Thread t2 = new Thread(()=>Felso(ref szum2));
            t1.Start();t2.Start();t1.Join();t2.Join();
            Console.WriteLine(szum1+szum2);
            Console.ReadKey();


        }
    }
}
