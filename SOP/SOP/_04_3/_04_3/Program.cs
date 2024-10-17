using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _04_3
{
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
            for (int i = 2; i <= Math.Sqrt(primNumber) && pr; i++)
                if (primNumber % i == 0)
                    pr = false;
            return pr;
        }
        public void ToDo()
        {
            Supervisor.TermeloElindul();
            for (int i = from; i <= until; i++)
            {
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
                        int temp = Supervisor.Kivesz();
                        lock (typeof(Program))
                        {
                            Console.ForegroundColor = this.color;
                            Console.WriteLine("Kivttem: " + temp);
                        }
                    }
                    catch { Console.WriteLine("A termelők leálltak:"+db); break; }

                }

                Supervisor.FogyasztoLeall();
            }
        }
        class Supervisor
        {
            static List<int> buffer = new List<int>();
            const int bufferSize = 100;
            static int termeloDB = 0;
            static int fogyasztoDB = 0;
            static bool termelokLealltak = false;
            static bool fogyasztokLealltak = false;

            public static void TermeloElindul()
            {
                Interlocked.Increment(ref termeloDB);
            }
            public static void TermeloLeall()
            {
                Interlocked.Decrement(ref termeloDB);
                if (termeloDB == 0)
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
                Interlocked.Increment(ref fogyasztoDB);
            }
            public static void FogyasztoLeall()
            {
                Interlocked.Decrement(ref fogyasztoDB);
                if (fogyasztoDB == 0)
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
                    while (buffer.Count >= bufferSize)
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
                            throw new Exception("A termelők leálltak");
                        temp = buffer[0];
                        buffer.RemoveAt(0);
                        Monitor.PulseAll(buffer);
                    }
                }
                return temp;
            }
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                // Termelo, fogyaszto
                Termelo ter1 = new Termelo(2, 1000);
                Termelo ter2 = new Termelo(10001, 20000);
                Termelo ter3 = new Termelo(20001, 30000);
                Termelo ter4 = new Termelo(30001, 40000);
                Termelo ter5 = new Termelo(40001, 50000);
                Termelo ter6 = new Termelo(50001, 60000);
                Fogyaszto fogyaszto1 = new Fogyaszto(ConsoleColor.Yellow);
                Fogyaszto fogyaszto2 = new Fogyaszto(ConsoleColor.Blue);
                Thread t1 = new Thread(ter1.ToDo); t1.Start();
                Thread t2 = new Thread(ter2.ToDo); t2.Start();
                Thread t3 = new Thread(ter3.ToDo); t3.Start();
                Thread t4 = new Thread(ter4.ToDo); t4.Start();
                Thread t5 = new Thread(ter5.ToDo); t5.Start();
                Thread t6 = new Thread(ter6.ToDo); t6.Start();

                Console.ReadKey();
            }
        }
    }
}
