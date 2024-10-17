using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            labda l = new labda(10, 5, +1, +1); //ha kettesével megy, akkor nem jó 80,25, ha páratlanról indul...
            labda l2 = new labda(30, 2, -1, +1);
            labda l3 = new labda(70, 10, -1, -1);


            CancellationTokenSource tokensource = new CancellationTokenSource();
            //Task t4 = Task.Run(() => { l.mozog(tokensource.Token); }); így is jó
            /*Task task4 = new Task(() =>
            {
                l.mozog(tokensource.Token);
            });*/
            Task t1 = Task.Run(() => l.mozog(tokensource.Token));
            Task t2 = Task.Run(() => l2.mozog(tokensource.Token));
            Task t3 = Task.Run(() => l3.mozog(tokensource.Token));

            Console.WriteLine("Press Enter to stop");
            var consoleKey = Console.ReadKey();
            while (consoleKey.Key != ConsoleKey.Enter)
                consoleKey = Console.ReadKey();
            tokensource.Cancel();
        }
    }
    class labda
    {
        public int currposX, currposY, intx, inty;
        public labda(int currposX, int currposY, int intx, int inty)
        {
            this.currposX = currposX; this.currposY = currposY; this.intx = intx; this.inty = inty;
        }

        public void mozog(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }

                lock (typeof(Program))
                {
                    Console.SetCursorPosition(currposX, currposY);
                    Console.Write(' ');
                }
                if (currposX < 80 && currposX > 0 && currposY < 25 && currposY > 0)
                {
                    currposX += intx; currposY += inty;
                }
                if (currposX == 0 || currposX == 80)
                {
                    intx *= -1;
                    currposX += intx;

                }
                if (currposY == 0 || currposY == 25)
                {
                    inty *= -1;
                    currposY += inty;
                }

                lock (typeof(Program))
                {
                    Console.SetCursorPosition(currposX, currposY);
                    Console.Write('O');
                }
                Thread.Sleep(100);

            }

        }
    }
}
