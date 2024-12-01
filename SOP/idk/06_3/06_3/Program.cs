using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _06_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // https://aries.ektf.hu/~ksanyi/SOP/feladatok.txt 2) Matematikai szerver | 4 funkció

            string ip="127.0.0.1"; int port = 12345;
            IPAddress ipaddress = IPAddress.Parse(ip);
            TcpListener listener = new TcpListener(ipaddress, port);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            StreamReader r = new StreamReader(client.GetStream(), Encoding.UTF8);
            StreamWriter w = new StreamWriter(client.GetStream(), Encoding.UTF8);
            w.WriteLine("Matek szerver vagyok!!!");
            w.Flush();
            string answer = "";
            while (answer.ToLower() != "bye")
            {
                answer = r.ReadLine();
                string[] temp = answer.ToUpper().Split('|');
                switch (temp[0])
                {
                    case "ADD":
                        int ered = int.Parse(temp[1]) + int.Parse(temp[2]);
                        w.WriteLine(ered);
                        break;
                    case "PRIMEK":
                        Primek(int.Parse(temp[1]));
                        break;
                    case "FIBO":
                        int fib = Fibo(int.Parse(temp[1]));
                        w.WriteLine("fib");
                        break;
                    case "BYE":
                        answer = "bye";
                        break;
                    default:
                        w.WriteLine("Ismeretlen parancs");
                        break;
                }
            }
            Console.WriteLine("A szerver nem megy tovább...");
            w.Close(); r.Close(); // le kell zárni
        }
        static void Primek(int meddig)
        {

        }
        static int Fibo(int meddig)
        {
            return 0;
        }
    }
}
