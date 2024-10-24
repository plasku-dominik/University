using System.Net.Sockets;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        TcpClient client= new TcpClient("127.0.0.1",12345);
        StreamReader r = new StreamReader(client.GetStream(), Encoding.UTF8);
        StreamWriter w = new StreamWriter(client.GetStream(), Encoding.UTF8);
        string uzenet = r.ReadLine();
        Console.WriteLine(uzenet);
        while (uzenet != "bye")
        {
            Console.WriteLine("Mit küldjek a szervernek?");
            uzenet= Console.ReadLine();
            w.WriteLine(uzenet);w.Flush();
            uzenet = r.ReadLine();
            Console.WriteLine("A szerver válasza:" + uzenet);
        }
        Console.ReadKey();
    }
}