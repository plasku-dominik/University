using System.Threading;

internal class Program
{
    static int numberOfOddNumbers = 0;
    static int numberOfEvenNumbers = 0;
    const int N = 10000;
    static int[] numbers =new int[N];
    static int also = 0; static int felso = N - 1;

    static void CalculateLow()
    {
        while (also<=felso)
        { 
            if (numbers[also] % 2 == 0)
                Interlocked.Increment(ref numberOfEvenNumbers);
            else
                Interlocked.Increment(ref numberOfOddNumbers);
            // Console.WriteLine(i);
            also++;

        }
    
    }

    static void CalculateUp()
    {
        while (felso>=also)
        { 
        
            if (numbers[felso] % 2 == 0)
                Interlocked.Increment(ref numberOfEvenNumbers);
            else
                Interlocked.Increment(ref numberOfOddNumbers);
            felso--;
        }

    }
    private static void Main(string[] args)
    {
        Random rn = new Random();

        for (int i = 0; i < N; i++) {
            numbers[i] = rn.Next(1,1000);
        }
        Thread t1 = new Thread(CalculateLow); t1.IsBackground = true;
        t1.Start();
        Thread t2 = new Thread(CalculateUp); //t2.Start();
        t2.IsBackground = true; t2.Start();
        Console.WriteLine(t1.IsAlive);
         t1.Join();
        Console.WriteLine(t1.IsAlive);
         
        t2.Join();
        Console.WriteLine($"Odd numbers {numberOfOddNumbers}, even numbers: {numberOfEvenNumbers}");
        //Console.ReadKey();
    }
}
// 2 threads and two methods. numberofodd, numberofeven. Array with
//N=100000,Calculete the number of odd and even numbers. How many
//do we have?