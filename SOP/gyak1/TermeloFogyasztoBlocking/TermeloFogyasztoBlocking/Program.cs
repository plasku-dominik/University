using System.Collections.Concurrent;

internal class Program
{
    static int bufferSize=50;
    static BlockingCollection<int> buffer = new BlockingCollection<int>(bufferSize);

    private static void Main(string[] args)
    {
        Task t1 = Task.Run(()=>Termel(2, 10000));
        Task t2 = Task.Run(() => Termel(10001, 20000));
        Task t3 = Task.Run(() => Termel(20001, 30000));
        Task t4 = Task.Run(() => Termel(30001, 40000));
        Task t5 = Task.Run(() => Fogyaszt(ConsoleColor.Blue));
        Task t6 = Task.Run(() => Fogyaszt(ConsoleColor.Yellow));
        Task.WaitAll(t1, t2, t3, t4);
        buffer.CompleteAdding();
        Task.WaitAll(t5,t6);
        Console.WriteLine("Most van vége!!!");
    }

    static bool Prime(int primNumber)
    {
        bool pr = true;
        for (int i = 2; i <= Math.Sqrt(primNumber) && pr; i++)
            if (primNumber % i == 0)
                pr = false;
        return pr;
    }
    static void Termel(int from, int to)
    {
        for (int i = from; i <= to; i++)
            if (Prime(i))
                buffer.Add(i);
    }
    static void Fogyaszt(ConsoleColor color)
    {
        int temp = 0; int db = 0;
        while (!buffer.IsCompleted)
        {
            temp = buffer.Take();
            lock (typeof(Program))
            {
                Console.ForegroundColor = color;
                Console.WriteLine("Kivettem: " + temp);
            }
            db++;
        }
        Console.WriteLine("Összesen:" + db);
    }
 }