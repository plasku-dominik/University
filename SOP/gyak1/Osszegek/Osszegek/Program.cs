using System.Buffers;
using System.Diagnostics;

internal class Program
{

    const int N = 100000;
    static int[] a=new int[N];
    static int osszeg = 0;

    static void OsszegNemSzal()
    {
        for (int i = 0; i < N; i++)
            osszeg += a[i];
        
    }

    static void OsszegSzalEleje()
    {
        for (int i = 0; i < N/2; i++)
            Interlocked.Add(ref osszeg, a[i]);
       
    }

    static void OsszegSzalVege()
    {
        for (int i = N/2; i < N; i++)
            Interlocked.Add(ref osszeg, a[i]);
    }

    static void OsszegSzalElejeThreadPool(Object o)
    {
        CountdownEvent countdownEvent =(CountdownEvent) o;
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

    static void OsszegSzalElejeLock()
    {

        for (int i = 0; i < N / 2; i++)
            lock (a)
            {
                osszeg += a[i];
            }
        
    }

    static void OsszegSzalVegeLock()
    {
        for (int i = N / 2; i < N; i++)
            lock (a)
            {
                osszeg += a[i];
            }
    }




    private static void Main(string[] args)
    {
        for (int i = 0; i < N; i++)
            a[i] = 1;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        OsszegNemSzal();
        Console.WriteLine("Alap:"+osszeg);
        stopwatch.Stop(); ;
        Console.WriteLine(stopwatch.ElapsedTicks);
        stopwatch.Reset();
        osszeg = 0;
        stopwatch.Start();
        Thread t1 =new Thread(OsszegSzalEleje); 
        Thread t2 =new Thread(OsszegSzalVege);
        t1.Start();t2.Start(); t1.Join(); t2.Join();
        Console.WriteLine("Dedikált, Interlocked"+osszeg);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedTicks);
        stopwatch.Reset();
        stopwatch.Start();
        CountdownEvent countdownEvent = new CountdownEvent(2);
        osszeg = 0;
        ThreadPool.QueueUserWorkItem(OsszegSzalElejeThreadPool, countdownEvent);
        ThreadPool.QueueUserWorkItem(OsszegSzalVegeThreadPool, countdownEvent);
        countdownEvent.Wait();
        Console.WriteLine("ThreadPool"+osszeg);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedTicks);
        stopwatch.Reset();
        stopwatch.Start();
        osszeg = 0;
        t1 = new Thread(OsszegSzalElejeLock);
        t2 = new Thread(OsszegSzalVegeLock);
        t1.Start(); t2.Start(); t1.Join(); t2.Join();
        Console.WriteLine("Dedikált, Lock"+osszeg);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedTicks);
        osszeg = 0;
        stopwatch.Reset();
        stopwatch.Start();
        Parallel.For(0, N, (i) =>
        {
            Interlocked.Add(ref osszeg, a[i]);
            //Console.Write("{0} ",i);
        });
        Console.WriteLine("Parallel.For"+osszeg);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedTicks);
        osszeg = 0;
        stopwatch.Reset();
        stopwatch.Start();
        Task task = Task.Run(OsszegSzalEleje);
        Task task2 = Task.Run(OsszegSzalVege);
        Task.WaitAll(task, task2);
        Console.WriteLine("Taszk"+osszeg);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.ElapsedTicks);
        Console.ReadLine();
    }
}