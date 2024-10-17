using System.Drawing;

public class Termelo
{
    ConsoleColor color;
    int db = 0;
    public Termelo(ConsoleColor color, int db)
    {
        this.color = color; this.db = db;
    }

    public void Berak()
    { 
        Random rn = new Random(); int elem = 0;
        for (int i = 0; i < db; i++)
            lock (Program.buffer)
            {
                while (Program.BufferSize <= Program.buffer.Count)
                    Monitor.Wait(Program.buffer);
                elem = rn.Next(10000, 80001);
                Program.buffer.Add(elem); 
                Console.ForegroundColor =this.color;
                Console.WriteLine(elem);
                Monitor.Pulse(Program.buffer);
            }

    }
}

public class Fogyaszto
{
    ConsoleColor color;
    
    public Fogyaszto(ConsoleColor color)
    {
        this.color = color; 
    }

    public void Kivesz()
    {
        int elem = 0;
        for (int i = 0; i < 400; i++)
            lock (Program.buffer)
            {
                while (Program.buffer.Count==0)
                    Monitor.Wait(Program.buffer);
                elem = Program.buffer[0];
                Program.buffer.RemoveAt(0);
                Console.ForegroundColor =this.color;
                Console.WriteLine(elem);
                Monitor.Pulse(Program.buffer);
            }

    }
}

internal class Program
{

    public static List<int> buffer = new List<int>();
    public const int BufferSize = 40;
    const int Elemek = 400;
    
    private static void Main(string[] args)
    {

        Termelo termelo = new Termelo(ConsoleColor.Blue, Elemek);
        Fogyaszto fogy=new Fogyaszto(ConsoleColor.Red);
        Thread t1 = new Thread(termelo.Berak);
        Thread t2 = new Thread(fogy.Kivesz);
        t1.Start(); t2.Start(); t1.Join(); t2.Join();
        Console.WriteLine(buffer.Count());
    }
}