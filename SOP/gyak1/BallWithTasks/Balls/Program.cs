internal class Program
{
    

    private static void Main(string[] args)
    {
        Balls b1 = new Balls(10, 10, -1, +1);
        Balls b2 = new Balls(50, 10, +1, +1);
        Balls b3 = new Balls(10, 20, +1, -1);

        CancellationTokenSource tokensource = new CancellationTokenSource();
        Task task4 = new Task(() =>
        {
            b1.Move(tokensource.Token);
        });

        Task task5 = new Task(() =>
        {
            b2.Move(tokensource.Token);
        });

        Task task6 = new Task(() =>
        {
            b3.Move(tokensource.Token);
        });

        task4.Start(); task5.Start(); task6.Start();
        //Task t1 = Task.Run(() => b1.Move(tokensource.Token));
        //Task t2 = Task.Run(() => b2.Move(tokensource.Token));
        //Task t3 = Task.Run(() => b3.Move(tokensource.Token));
        Console.WriteLine("Press Enter to stop");
        
        var consoleKey = Console.ReadKey(); 
        while (consoleKey.Key != ConsoleKey.Enter)
            consoleKey = Console.ReadKey();
        tokensource.Cancel();

    }
}

class Balls
{
    public int CurrentPosX;
    public int CurrentPosY;
    public int DirectionX;
    public int DirectionY;

    public Balls(int cx, int cy, int dx, int dy)
    {
        this.CurrentPosX = cx;
        this.CurrentPosY = cy;
        this.DirectionX = dx;
        this.DirectionY = dy;
    }

    public void Move(CancellationToken token)
    {
        
        while (true)
        {
            
            if (token.IsCancellationRequested)
                 break;
            
            lock (typeof(Program))
            {
                Console.SetCursorPosition(this.CurrentPosX, this.CurrentPosY);
                Console.Write(' ');
            }
            if (CurrentPosX >0 || CurrentPosX < 80)
                this.CurrentPosX += this.DirectionX;
            if(CurrentPosY >0 || CurrentPosY <25)
                this.CurrentPosY += this.DirectionY;
            if (CurrentPosX == 0 || CurrentPosX == 80)
            {
                this.DirectionX *= -1;
                this.CurrentPosX += this.DirectionX;
            }
            if (CurrentPosY == 0 || CurrentPosY == 25)
            {
                this.DirectionY *= -1;
                this.CurrentPosY += this.DirectionY;
            }
            lock (typeof(Program))
            {
                Console.SetCursorPosition(this.CurrentPosX, this.CurrentPosY);
                Console.Write('O');
            }
            Thread.Sleep(200);
        }
       
        
       
    }

}