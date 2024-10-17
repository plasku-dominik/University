using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _06_3_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1",12345);
            StreamReader r = new StreamReader(client.GetStream(), Encoding.UTF8);
            StreamWriter w = new StreamWriter(client.GetStream(), Encoding.UTF8);
            string uzenet = r.ReadLine();
            Console.WriteLine(uzenet);
            while (uzenet != "bye")
            {
                Console.WriteLine("Mit küldjek a szervernek?");
                uzenet = Console.ReadLine();
                w.WriteLine(uzenet); w.Flush();
                uzenet = r.ReadLine();
                Console.WriteLine("A szerver válasza:" + uzenet);
            }
            Console.ReadKey();
        }
    }
}
