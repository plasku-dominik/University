class Supervisor
{ 
    static List<int> buffer= new List<int>();
    const int bufferSize = 50;
    static int termelodb = 0;
    static int fogyasztodb = 0;
    static bool termelokLealltak=false;
    static bool fogyasztokLealltak = false;
    public static void TermeloElindul()
    { 
        Interlocked.Increment(ref termelodb);
    }

    public static void TermeloLeall()
    {
        Interlocked.Decrement(ref termelodb);
        if (termelodb == 0)
        {
            termelokLealltak = true;
            lock (buffer)
            {
                Monitor.PulseAll(buffer);
            }
        }
    }

    public static void FogyasztoElindul()
    {
        Interlocked.Increment(ref fogyasztodb);
    }

    public static void FogyasztoLeall()
    { 
        Interlocked.Decrement (ref fogyasztodb);
        if (fogyasztodb == 0)
        {
            fogyasztokLealltak = true;
            lock (buffer)
            {
                Monitor.PulseAll(buffer);
            }
        }
    }

    public static void Berak(int szam)
    {
        lock (buffer)
        { 
            while (buffer.Count>=bufferSize)
            {
                if (fogyasztokLealltak)
                    throw new Exception("A fogyasztók leálltak");
                Monitor.Wait(buffer);
            }
            buffer.Add(szam);
            Monitor.PulseAll(buffer);
        }
        
    }

    public static int Kivesz()
    {
        int temp = 0;
        lock (buffer)
        {
            while (buffer.Count <= 0)
            {
                if (termelokLealltak)
                    throw new Exception("A termelők leállítak");
                Monitor.Wait(buffer);
                
            }
            temp = buffer[0];
            buffer.RemoveAt(0);
            Monitor.PulseAll(buffer);
        }
        return temp;
    }
}

public class Termelo
{
    int from = 0; int until;
    public Termelo(int from, int to)
    {
        this.from = from; this.until = to;
    }

    bool Prime(int primNumber)
    {
        bool pr = true;
        for (int i=2; i<=Math.Sqrt(primNumber) && pr; i++) 
            if (primNumber%i==0) 
                pr = false;
        return pr;
    }
    public void ToDo()
    {
        Supervisor.TermeloElindul();
        for (int i = from; i <= until; i++)
            if (Prime(i))
                try
                {
                    Supervisor.Berak(i);
                }
                catch (Exception e) { Console.WriteLine("A fogyasztók leálltak??"); break; }   
        Supervisor.TermeloLeall();
    }
}

public class Fogyaszto
{
    ConsoleColor color; int db = 0;

    public Fogyaszto(ConsoleColor color)
    {
        this.color = color;
    }

    public void Kivesz()
    {
        Supervisor.FogyasztoElindul();
        while (true)
        {
            try
            {
                int temp=Supervisor.Kivesz();
                lock (typeof(Program))
                {
                    Console.ForegroundColor = this.color;
                    Console.WriteLine("Kivettem: " + temp);
                    
                }
                db++;

            }
            catch { Console.WriteLine("A termelők leálltak, kivettem ennyit: "+db); break; }
        }
        Supervisor.FogyasztoLeall();
    }
}
internal class Program
{
    private static void Main(string[] args)
    {
        Termelo ter1 = new Termelo(2, 10000);
        Termelo ter2 = new Termelo(10001, 20000);
        Termelo ter3 = new Termelo(20001, 30000);
        Termelo ter4 = new Termelo(30001, 40000);
        Fogyaszto f1 = new Fogyaszto(ConsoleColor.Blue);
        Fogyaszto f2 = new Fogyaszto(ConsoleColor.Yellow);
        Thread t1 = new Thread(ter1.ToDo);
        Thread t2 = new Thread(ter2.ToDo);
        Thread t3 = new Thread(ter3.ToDo);
        Thread t4 = new Thread(ter4.ToDo);
        Thread t5 = new Thread(f1.Kivesz);
        Thread t6 = new Thread(f2.Kivesz);
        t1.Start();t2.Start();t3.Start();t4.Start();t5.Start();t6.Start();
    }
}