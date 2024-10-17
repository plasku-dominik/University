using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _03_2
{
    internal class Program
    {
        const int N = 25; const int M = 80;
        static char[,] matrix = new char[80, 25];
        const int DB = 2000;
        static void GenX()
        {
            Random rn = new Random();
            for (int i = 0; i < DB; i++)
            {
                matrix[rn.Next(0, 25), rn.Next(0, 80)] = 'X';
                int sor = rn.Next(0, 25); int oszlop = rn.Next(0, 80);
                if (Interlocked.Equals(matrix[sor, oszlop], 'O')) { }
                else
                {
                    lock (matrix)
                    {
                        matrix[sor, oszlop] = 'X';
                    }
                }
            }
        }

        static void GenO()
        {
            Random rn = new Random();
            for(int i = 0;i < DB; i++)
            {
                int sor = rn.Next(0, 25); int oszlop = rn.Next(0, 80);
                if (matrix[sor, oszlop] == 'X')
                {

                }
                else
                {
                    matrix[sor, oszlop] = 'O';
                }
            }
        }
        static void Main(string[] args)
        {
            // https://aries.ektf.hu/~ksanyi/SOP/feladatok.txt mátrix 9.

        }
    }
}
