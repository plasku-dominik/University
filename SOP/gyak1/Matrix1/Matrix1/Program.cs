internal class Program
{
    const int N = 25; const int M = 80;
    static char[,] matrix = new char[N, M];
    const int DB = 2000;

    static void GenX()
    { 
        Random rn= new Random();
        for (int i = 0; i < DB; i++)
        {
            int sor = rn.Next(0, 25); int oszlop = rn.Next(0, 80);
            if (Interlocked.Equals(matrix[sor, oszlop], 'O'))
                ;
            else
                lock (matrix)
                {
                    matrix[sor, oszlop] = 'X';
                }
        }
    }

    static void GenO()
    {
        Random rn = new Random();
        for (int i = 0; i < DB; i++)
        {
            int sor=rn.Next(0, 25); int oszlop = rn.Next(0, 80);
            if (Interlocked.Equals(matrix[sor, oszlop], 'X'))
                ;
            else
                lock (matrix)
                {
                    matrix[sor, oszlop] = 'O';
                }
        }
    }
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}