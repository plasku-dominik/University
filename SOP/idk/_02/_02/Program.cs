using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _02
{
    internal class Program
    {
        public static ManualResetEvent mre = new ManualResetEvent(false);
        public static ManualResetEvent shutdown = new ManualResetEvent(true);
        static void Main(string[] args)
        {
            Ball b1 = new Ball(10, 5, 1, 1);
            Thread t1 = new Thread(b1.Move);
            t1.Start();
            Ball b2 = new Ball(30, 2, -1, +1);
            Thread t2 = new Thread(b2.Move);
            t2.Start();
            Ball b3 = new Ball(70, 10, -1, -1);
            Thread t3 = new Thread(b3.Move);
            t3.Start();

            Console.WriteLine("Megállít?");
            Console.ReadLine();
            mre.Set();
            Console.ReadLine();
            mre.Reset();
            Console.WriteLine("Mostmár megállítom");
            mre.Set();
            Console.ReadLine();
            shutdown.Reset();
            Console.ReadLine();
            

            while (true)
            {
                if ((b1.currX == b2.currX && b1.currY == b2.currY) ||
                        (b2.currX == b3.currX && b2.currY == b3.currY) ||
                        (b1.currX == b3.currX && b1.currY == b3.currY))
                {
                    t1.Interrupt();
                    t2.Interrupt();
                    t3.Interrupt();
                }
            }
        }

        class Ball
        {
            public int currX, currY, dirX, dirY;

            public Ball(int currX, int currY, int dirX, int dirY)
            {
                this.currX = currX;
                this.currY = currY;
                this.dirX = dirX;
                this.dirY = dirY;
            }

            public void Move()
            {
                while (true)
                {
                    Program.mre.WaitOne(Timeout.Infinite);
                    Thread.Sleep(100);
                    lock (typeof(Program)) // referencia a Program class-re, azt lockolja
                                           // azért működik, mivel más thread-ek is ezt a lockot várják
                    {
                        Console.SetCursorPosition(currX, currY);
                        Console.Write(" ");
                    }
                    this.currX += this.dirX;
                    this.currY += this.dirY;
                    if (this.currX <= 0 || this.currX >= 80)
                    {
                        this.dirX = -this.dirX;
                    }
                    else if (this.currY <= 0 || this.currY >= 40)
                    {
                        this.dirY = -this.dirY;
                    }
                    lock (typeof(Program))
                    {
                        Console.SetCursorPosition(currX, currY);
                        Console.Write("*");
                    }
                }
            }
        }
    }
}
