using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _05_2
{   
    
    internal class Program
    {
        static int osszeg = 0;
        static int N = 500000;
        static int[] a = new int[N];
        static void OsszegSzalElejeThreadPool(Object o)
        {
            CountdownEvent countdownEvent = (CountdownEvent)o;
            for (int i = 0; i < N / 2; i++)
                Interlocked.Add(ref osszeg, a[i]);
            countdownEvent.Signal();
        }
        static void OsszegSzalVegeThreadPool(Object o)
        {
            CountdownEvent countdownEvent = (CountdownEvent)o;
            for (int i = N / 2; i < N; i++)
                Interlocked.Add(ref osszeg, a[i]);
            countdownEvent.Signal();
        }
        static void Main(string[] args)
        {
            // https://aries.ektf.hu/~ksanyi/SOP/feladatok.txt 3.feladat
            CountdownEvent countdownEvent = new CountdownEvent(2);
            osszeg = 0;
            ThreadPool.QueueUserWorkItem(OsszegSzalElejeThreadPool)
        }
    }
}
