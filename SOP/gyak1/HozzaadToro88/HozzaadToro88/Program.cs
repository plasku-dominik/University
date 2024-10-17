using System.Security.Cryptography;

internal class Program
{
    const int N = 100000;
    static int[] a=new int[N];
    static Random rn= new Random();

    static void Hozzad(Object o)
    { 
        CountdownEvent ce=(CountdownEvent)o;
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
        CountdownEvent ce=( CountdownEvent)o;
       // Thread.Sleep(1000);
        for (int i = 0;i < N; i++)
        {
            int pos=rn.Next(0, N);
            lock (a)
            {
                if (a[pos] != 0)
                    a[pos] = 0;
                

            }
            
        }
        ce.Signal();
    }
    private static void Main(string[] args)
    {
        CountdownEvent countdownEvent = new CountdownEvent(2);
        ThreadPool.QueueUserWorkItem(new WaitCallback(Hozzad), countdownEvent);
        ThreadPool.QueueUserWorkItem(Torol, countdownEvent);
        countdownEvent.Wait(); int db = 0;
        for (int i = 0; i < N; i++)
        {
            if (a[i] != 0)
            {
                Console.Write("{0} ", a[i]);
                db++;
            }
        }
        Console.WriteLine(db);
        Console.ReadKey();
    }
}