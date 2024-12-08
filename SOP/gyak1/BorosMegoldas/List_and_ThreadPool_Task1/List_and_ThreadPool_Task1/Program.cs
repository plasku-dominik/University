using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace Program
{
    class Supervisor
    {
        static List<string> buffer = new List<string>();
        const int maxSize = 50;
        static bool ProducersStopped = false;
        static int NumberOfProducers = 0;
        static bool Consumerstopped = false;
        static int NumberOfConsumers = 0;
        

        public static void ConsumerStart()
        {
            Interlocked.Increment(ref NumberOfConsumers);
        }
        public static void ProducerStart()
        {
            Interlocked.Increment(ref NumberOfProducers);
        }
        public static void ConsumerStop()
        {
            Interlocked.Decrement(ref NumberOfConsumers);
            if (NumberOfConsumers <= 0)
            {
                Consumerstopped = true;
                lock (buffer)
                {
                    Monitor.PulseAll(buffer);
                }
            }
        }

        public static void ProducerStop()
        {
            Interlocked.Decrement(ref NumberOfProducers);
            if (NumberOfProducers <= 0)
            {
                ProducersStopped = true;
                lock (buffer)
                {
                    Monitor.PulseAll(buffer);
                }
            }
        }

        public static void AddItem(string item)
        {
            lock (buffer)
            {
                while (buffer.Count >= maxSize)
                    if (Consumerstopped)
                        throw new Exception("No more consumer");
                    else
                        Monitor.Wait(buffer);
                buffer.Add(item);
                Monitor.PulseAll(buffer);
            }
        }

        public static string Removeitem(string WinType)
        {
            string item = null;
            lock (buffer)
            {
                while (!buffer.Contains(WinType))
                    if (ProducersStopped)
                        throw new Exception("No more producer");
                    else
                        Monitor.Wait(buffer);

                item = buffer[0];
                buffer.RemoveAt(0);
                Monitor.PulseAll(buffer);
            }
            return item;
        }
    }

    class Producer
    {
        string Wine_type;
        ConsoleColor colour;
        int Quantity;
        int Worktime;
        public Producer(string WineType,ConsoleColor colour, int quantity , int worktime)
        {
            this.colour = colour;
            this.Wine_type= WineType;
            this.Worktime= worktime;
            this.Quantity = quantity;
        }
        public void Work()
        {
            Supervisor.ProducerStart();
            for (int i = 0; i < Quantity; i++)
            {
               Thread.Sleep(Worktime);
                    try
                    {
                        Supervisor.AddItem(Wine_type);
                        Console.WriteLine($"{Wine_type} Bottle has been added.");
                    }
                    catch { Console.WriteLine("No more consumer"); break; }
            }
            Supervisor.ProducerStop();
        }

    }
    class Consumer
    {
        string Wine_type;
        ConsoleColor colour;
        public Consumer(string WineType, ConsoleColor colour)
        {
            this.colour = colour;
            this.Wine_type = WineType;

        }
        public void Consume(object o)
        {
            CountdownEvent countdown = (CountdownEvent)o;
            Supervisor.ConsumerStart();
            while (true)
            {
                try
                {
                   
                    lock (typeof(Program))
                    {
                        string item = Supervisor.Removeitem(this.Wine_type);
                        Console.ForegroundColor = colour;
                        Console.WriteLine($"{item} wine has been removed.");
                        Console.ResetColor();
                    }

                }
                catch { Console.WriteLine("No more producer"); break; }
            }
            Supervisor.ConsumerStop();
            countdown.Signal();
        }

    }

    internal class Program
    {
        private static void Main(string[] args)
        {

            List<ConsoleColor> Randomcolors = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Yellow };
            Random random = new Random();

            CountdownEvent countdown= new CountdownEvent(3);
            Producer p1 = new Producer("red", Randomcolors[random.Next(0,3)], 10, 2000);  //the same!!!
            Producer p2 = new Producer("white", Randomcolors[random.Next(0,3)], 10, 1000);
            Producer p3 = new Producer("red", Randomcolors[random.Next(0,3)], 10, 2000);

            Consumer c1 = new Consumer("red", ConsoleColor.Red);
            Consumer c2 = new Consumer("white", ConsoleColor.Yellow);
            Consumer c3 = new Consumer("white", ConsoleColor.Yellow);

            ThreadPool.QueueUserWorkItem(start => p1.Work());
            ThreadPool.QueueUserWorkItem(start => p2.Work());
            ThreadPool.QueueUserWorkItem(start => p3.Work());

            ThreadPool.QueueUserWorkItem(c1.Consume, countdown);
            ThreadPool.QueueUserWorkItem(c2.Consume, countdown);
            ThreadPool.QueueUserWorkItem(c3.Consume, countdown);
            countdown.Wait();
            Console.ReadKey();
        }
    }
}
