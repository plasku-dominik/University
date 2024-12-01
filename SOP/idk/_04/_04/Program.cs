using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _04
{
    internal class Program
    {
        static int N = 100000;
        static int[] a = new int[N];
        static Random rn = new Random();
        static void Hozzaad(Object o)
        {
            CountdownEvent ce = (CountdownEvent)o;
            for (int i = 0; i < N; i++)
            {
                lock (a)
                {
                    a[i] = rn.Next(1, N - 1);
                }
            }
            ce.Signal();
        }
        static void Torol(Object o) 
        {
            CountdownEvent ce = (CountdownEvent)o;
            // Thread.Sleep(1000);
            for (int i = 0; i < N; i++)
            {
                int pos = rn.Next(0, N);
                lock (a)
                {
                    if (a[pos] != 0)
                        a[pos] = 0;                    
                }
            }
            ce.Signal();
        }

        static void Main(string[] args)
        {
            // 8.8 feladat
            CountdownEvent countdownEvent = new CountdownEvent(2);
            ThreadPool.QueueUserWorkItem(new WaitCallback(Hozzaad), countdownEvent);
            ThreadPool.QueueUserWorkItem(Torol, countdownEvent);
            countdownEvent.Wait();
            int db = 0;
            for (int i = 0; i < N; i++)
            {
                if (a[i] != 0)
                {
                    Console.Write("{0} ", a[i]);
                    db++;
                }
            
            }
            Console.WriteLine($"\n{db} maradt");
            Console.ReadKey();

        }
    }
}
