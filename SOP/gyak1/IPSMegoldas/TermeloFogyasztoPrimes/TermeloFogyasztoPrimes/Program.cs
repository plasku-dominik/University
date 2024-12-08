using System;
using System.Drawing;
using System.Threading.Tasks.Dataflow;

class Supervisor
{ 
    static List<string> buffer= new List<string>();
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

    public static void Berak(string ip)
    {
        lock (buffer)
        { 
            while (buffer.Count>=bufferSize)
            {
                if (fogyasztokLealltak)
                    throw new Exception("A fogyasztók leálltak");
                Monitor.Wait(buffer);
            }
            buffer.Add(ip);
            Monitor.PulseAll(buffer);
        }
        
    }

    public static string Kivesz(char IpType)
    {
       string ipAddress = null;
        lock (buffer)
        {
           // while (buffer.Count <= 0) //Nem túl jó, de most mind1..
            while (!buffer.Exists(ip=>ip.StartsWith(IpType.ToString())))
            {
                if (termelokLealltak)
                    throw new Exception("A termelők leálltak");
                Monitor.Wait(buffer);

            }
            ipAddress = buffer.Find(ip => ip.StartsWith(IpType.ToString()));
            if (ipAddress != null)
            {
                buffer.Remove(ipAddress);
            }
            Monitor.PulseAll(buffer);

        }
        return ipAddress;
        
    }
}

public class Termelo
{
    char IPType = ' ' ; ConsoleColor color = ConsoleColor.Green; int Amount = 0; int WorkTime = 0;
    Random rn=new Random();
    public Termelo(char p1, ConsoleColor p2, int p3, int p4)
    {
        IPType = p1; color = p2; Amount = p3; WorkTime = p4;   
    }

    string Generate(Char type)
    {
        string ip = "";
        if (type == 'A')
            ip = "A"+rn.Next(1, 127).ToString()+"."+rn.Next(0,255).ToString()+"."+rn.Next(0,255).ToString();
        else
            if (type=='B')
              ip = "B"+rn.Next(128, 192).ToString() +"."+ rn.Next(0, 255).ToString() + "."+rn.Next(0, 255).ToString() + "." + rn.Next(0, 255).ToString();

            else
              if (type=='C')
                 ip = "C"+rn.Next(192, 224).ToString() +"."+ rn.Next(0, 255).ToString() +"."+ rn.Next(0, 255).ToString() + "." + rn.Next(0, 255).ToString();
        return ip;

        /*Vagy, hogy profibb legyen! 
        int firstOctet = IPType == 'A' ? random.Next(1, 127) :
                             IPType == 'B' ? random.Next(128, 192) : random.Next(192, 224);
        return $"{firstOctet}.{random.Next(0, 256)}.{random.Next(0, 256)}.{random.Next(0, 256)}";
        */
    }
    public void ToDo(Object o)
    {
        CountdownEvent c=(CountdownEvent)o;
        Supervisor.TermeloElindul();
        for (int i = 1; i <= Amount; i++)
        {
            string ip = Generate(IPType);Thread.Sleep(WorkTime);
            try
            {
                Supervisor.Berak(ip);
                lock (typeof(Program))
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine("Added:"+ip);
                }
            }
            catch (Exception e) { Console.WriteLine("A fogyasztók leálltak??"); break; }
        }
        Supervisor.TermeloLeall();
        c.Signal();
    }
}

public class Fogyaszto
{
    char IPType = ' '; ConsoleColor color = ConsoleColor.Green;

    public Fogyaszto(char ip, ConsoleColor color)
    {
        this.color = color;
        IPType = ip;
    }

    public void Kivesz(Object o)
    {
        CountdownEvent c = (CountdownEvent)o;
        Supervisor.FogyasztoElindul(); int db = 0;
        while (true)
        {
            try
            {
                string temp=Supervisor.Kivesz(IPType);
              
                lock (typeof(Program))
                {
                    Console.ForegroundColor = this.color;
                    Console.WriteLine("Removed: " + temp);
                    
                }
                db++;

            }
            catch { Console.WriteLine("A termelők leálltak, kivettem ennyit: "+db); break; }
        }
        Supervisor.FogyasztoLeall();
        c.Signal();
    }
}
internal class Program
{
    private static void Main(string[] args)
    {
        Termelo ter1 = new Termelo('A',ConsoleColor.Red,10,10);
        Termelo ter2 = new Termelo('B', ConsoleColor.Green, 20, 10);
        Termelo ter3 = new Termelo('C', ConsoleColor.Blue, 30, 10);
       
        Fogyaszto f1 = new Fogyaszto('A', ConsoleColor.Red);
        Fogyaszto f2 = new Fogyaszto('B', ConsoleColor.Green);
        Fogyaszto f3 = new Fogyaszto('C', ConsoleColor.Blue);
        CountdownEvent countdown = new CountdownEvent(6);
        ThreadPool.QueueUserWorkItem(ter1.ToDo,countdown);
        ThreadPool.QueueUserWorkItem(ter2.ToDo, countdown);
        ThreadPool.QueueUserWorkItem(ter3.ToDo, countdown);
        ThreadPool.QueueUserWorkItem(f1.Kivesz, countdown);
        ThreadPool.QueueUserWorkItem(f2.Kivesz, countdown);
        ThreadPool.QueueUserWorkItem(f3.Kivesz, countdown);
        countdown.Wait();


        /*
        Thread t1 = new Thread(ter1.ToDo);
        Thread t2 = new Thread(ter2.ToDo);
        Thread t3 = new Thread(ter3.ToDo);
        Thread t5 = new Thread(f1.Kivesz);
        Thread t6 = new Thread(f2.Kivesz);
        Thread t7 = new Thread(f3.Kivesz);
        t1.Start();t2.Start();t3.Start();;t5.Start();t6.Start();t7.Start();*/
        Console.ReadKey();
    }
}