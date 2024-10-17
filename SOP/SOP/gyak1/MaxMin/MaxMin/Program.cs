using System.Diagnostics;

internal class Program
{
    static int max = int.MinValue; static int min = int.MaxValue;
    static int maxDb = 0; static int minDb =0;
    static List<int> list = new List<int>();
    static void MaxMin1()
    {
        for (int i = 0; i < list.Count / 2; i++)
        {
            lock (list)
            {
                if (list[i] > max)
                {
                    max = list[i]; maxDb = 1;
                }
                else
                if (list[i] == max)
                    maxDb++;
                else
                  if (list[i] < min)
                {
                    min = list[i]; minDb = 1;
                }
                else
                       if (list[i] == min)
                    minDb++;
            }
        }

    
    }
    static void MaxMin2()
    {
        for (int i = list.Count / 2; i < list.Count; i++)
        {
            lock (list)
            {

                if (list[i] > max)
                {
                    max = list[i]; maxDb = 1;
                }
                else
                   if (list[i] == max)
                    maxDb++;
                else
                   if (list[i] < min)
                {
                    min = list[i]; minDb = 1;
                }
                else
                     if (list[i] == min)
                    minDb++;
            }
        }
    }

        static void MaxMin3()
        {
            for (int i = 0; i < list.Count; i++)
            {
                lock (list)
                {

                    if (list[i] > max)
                    {
                        max = list[i]; maxDb = 1;
                    }
                    else
                       if (list[i] == max)
                        maxDb++;
                    else
                       if (list[i] < min)
                    {
                        min = list[i]; minDb = 1;
                    }
                    else
                         if (list[i] == min)
                        minDb++;
                }
            }
        }

    static void Min1(ref int min, ref int mindb)
    { 
        int minl=int.MaxValue; int mindbl = 0;
        for (int i = 0; i < list.Count; i++)
            if (minl > list[i])
            {
                minl = list[i]; mindbl = 1;

            }
            else
                if (minl == list[i])
                  mindbl++;
        min=minl;mindb= mindbl;
    }
    static void Max(ref int max, ref int maxdb)
    {
        int maxl = int.MinValue; int maxdbl = 0;
        for (int i = 0; i < list.Count; i++)
            if (maxl < list[i])
            {
                maxl = list[i]; maxdbl = 1;

            }
            else
                if (maxl == list[i])
                  maxdbl++;
        max= maxl; maxdb= maxdbl;
    }



    private static void Main(string[] args)
    {
        const int N = 1000;
        Random rn = new Random();
        for (int i = 0; i < N; i++)
            list.Add(rn.Next(1, 1212122));
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Thread t1 = new Thread(MaxMin1);t1.Start();
        Thread t2 = new Thread(MaxMin2); t2.Start();

        t1.Join();t2.Join();
        Console.WriteLine($"A legn: {max}, db: {maxDb}, a legk: {min}, db: {minDb}");
        sw.Stop();
        Console.WriteLine("Az eltelt idő: {0}", sw.ElapsedTicks);
        max = int.MinValue;min = int.MaxValue;
        maxDb = 0; minDb = 0;
        sw.Reset();
        sw.Start();
        MaxMin3();
        Console.WriteLine($"A legn: {max}, db: {maxDb}, a legk: {min}, db: {minDb}");
        sw.Stop();
        Console.WriteLine("Az eltelt idő: {0}", sw.ElapsedTicks);
        sw.Reset();
        sw.Start(); 
        t1= new Thread(()=>Min1(ref min, ref minDb));t1.Start();
        t2 = new Thread(() => Max(ref max, ref maxDb)); t2.Start();
        t1.Join(); t2.Join();
        Console.WriteLine($"A legn: {max}, db: {maxDb}, a legk: {min}, db: {minDb}");
        sw.Stop();
        Console.WriteLine("Az eltelt idő: {0}", sw.ElapsedTicks);
        sw.Reset();
        sw.Start();
        Min1(ref min, ref minDb);
        Max(ref max, ref maxDb);
        Console.WriteLine($"A legn: {max}, db: {maxDb}, a legk: {min}, db: {minDb}");
        sw.Stop();
        Console.WriteLine("Az eltelt idő: {0}", sw.ElapsedTicks);
        Console.ReadKey();

    }

}