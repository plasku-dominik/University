using System;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Configuration;

namespace SOP_feladat_kliens
{
    class Program
    {
        //static string ip = ConfigurationManager.AppSettings["IP"];
        //static int port = int.Parse(ConfigurationManager.AppSettings["port"]);
        static void Main(string[] args)
        {
            TcpClient kliens = new TcpClient("127.0.0.1", 44444);
            StreamReader olvas = new StreamReader(kliens.GetStream(), Encoding.UTF8);
            StreamWriter ir = new StreamWriter(kliens.GetStream(), Encoding.UTF8);
            string szerver_valasz= olvas.ReadLine();
            Console.WriteLine(szerver_valasz);
            string parancsok;// = olvas.ReadLine();
            //Console.WriteLine(parancsok);
            string kliens_szoveg = Console.ReadLine();
            ir.WriteLine(kliens_szoveg);
            ir.Flush();
            while (kliens_szoveg.ToUpper() != "EXIT")
            {
                szerver_valasz = olvas.ReadLine();
                //Console.WriteLine(szerver_valasz);
                if (szerver_valasz == "OK*")
                {
                    szerver_valasz = olvas.ReadLine();
                    while (szerver_valasz != "OK!")
                    {
                        Console.WriteLine(szerver_valasz);
                        szerver_valasz = olvas.ReadLine();
                    }
                }
                else
                {
                    string[] sor = szerver_valasz.Split('|');
                    if (sor[0] == "ERR")
                        Console.WriteLine("Hiba: "+ sor[1]);
                    else
                        Console.WriteLine("A művelet eredménye: {0}", sor[1]);
                }
                Console.WriteLine("\nMi a következő parancs?");
                kliens_szoveg = Console.ReadLine();
                ir.WriteLine(kliens_szoveg);
                ir.Flush();
            }
        }
    }
}
