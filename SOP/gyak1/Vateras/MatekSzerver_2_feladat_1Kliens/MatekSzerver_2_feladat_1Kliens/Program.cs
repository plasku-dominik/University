using System.ComponentModel.Design;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

public class Product
{
    string code;
    public string Code { set; get; }

    string name;
    public string Name { set; get; }

    int price;

    public int Price { set; get; }

    string user;
    public string User { set; get; }

    public Product(string p1, string p2, int p3, string p4)
    {
        Code = p1; Name = p2; Price = p3; User = p4;
    }


}
public class ClientComm
{
    StreamReader r;

    StreamWriter w;
    string user = null;
    static List<Product> products = new List<Product>();
    public ClientComm(TcpClient client)
    {

       r=  new StreamReader(client.GetStream(), Encoding.UTF8);
       w = new StreamWriter(client.GetStream(), Encoding.UTF8);
    }

    public void Communication()
    {
        w.WriteLine("Vatera szerver vagyok");
        w.Flush();
        string answer = ""; bool end = false;
        while (!end)
        {
            try
            {
                answer = r.ReadLine();
                string[] temp = answer.ToUpper().Split('|');
                switch (temp[0])
                {
                    case "LOGIN": Login(temp[1], temp[2]); break;
                    case "ADD":  Add(temp[1], temp[2], int.Parse(temp[3])); break;
                    case "LIST": List();
                         break;
                    case "BID": Bid(temp[1], int.Parse(temp[2]));break;
                    case "HELP": w.WriteLine("LOGIN|<name>|<passwd>; ADD|<code>|<name>|<price>; ");  break;
                    case "BYE": answer = "bye"; end = true; w.WriteLine(answer); break;
                    default: w.WriteLine("Ismeretlen parancs"); break;
                }
                
            }
            catch (Exception e) { Console.WriteLine("Hiba"); w.WriteLine("Hiba"); }
            w.Flush();
        }
        w.Close(); r.Close();
    }

    void Login(string p1, string p2)
    {
        if (Program.users.Contains(p1 + "|" + p2))
        {
            w.WriteLine("OK|Logged in");
            this.user = p1;
        }
        else
            w.WriteLine("ERR|Invalid user or password");
    }

    void List()
    {
        w.WriteLine("OK!");
        lock (products)
        {
            foreach (Product items in products)
                w.WriteLine($"{items.Code}, {items.Name}, {items.Price}, {items.User}");
        }
        w.WriteLine("OK*");
    }
    void Add(string code, string name, int price)
    {
        if (user == null)
            w.WriteLine("ERR|Not logged in");
        else
        {
            lock (products)
            {
                bool vane = false;
                for (int i = 0; i < products.Count && !vane; i++)
                    if (products[i].Code == code)
                        vane = true;
                if (vane)
                    w.WriteLine("Err|Már van ilyen!");
                else
                {
                    products.Add(new Product(code, name, price, this.user));
                    w.WriteLine("OK, hozzáadtam");
                }
            }
        }
    }

    void Bid(string code, int price)
    {
        if(user == null)
            w.WriteLine("ERR|Not logged in");
        else
        {
            lock (products)
            {
                var elem = products.Find(item => item.Code == code);
                if (elem!=null)
                {
                    if (elem.Price <= price)
                    {
                        elem.Price = price;
                        products.Remove(elem);
                        elem.User = this.user;
                        products.Add(elem);
                        w.WriteLine("Módosítva!");
                    }
                    else
                        w.WriteLine("ERR|Alacsony ár");
                }
                else
                    w.WriteLine("Err|Nincs ilyen termék!");
            }
        }



    


}


    internal class Program
    {
        public static List<string> users = new List<string>();
        private static void Main(string[] args)
        {

            //StreamReader be = new StreamReader("users.txt");
            users = File.ReadAllLines("users.txt").ToList();
            string ip = "127.0.0.1"; int port = 12345;
            IPAddress ipaddress = IPAddress.Parse(ip);
            TcpListener listener = new TcpListener(ipaddress, port);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ClientComm c1 = new ClientComm(client);
                Thread t = new Thread(c1.Communication);
                t.Start();
            }
        }

    }   
}
