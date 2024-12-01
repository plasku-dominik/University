using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Configuration;
using DVD_szerver;
using System.IO;
using System.ComponentModel.Design;

namespace DVD_szerver
{
    class DVD_film
    {
        string id, title, user;
        bool rented;
        public string User
        {
            get { return user; }
            set { user = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Rented
        {
            get { return rented; }
            set { rented = value; }
        }

        public DVD_film(string id, string title, bool rented)
        {
            this.id = id; this.Title = title; this.User = string.Empty;
            this.Rented = rented;
            
        }
    }


    class User
        {
        public string name, password;
        public User(string name, string password)
        {
            this.name = name;
            this.password = password;
        }        

        }

    class Szerver
    {
        StreamReader input;
        StreamWriter output;
        string login_user = string.Empty;
        public Szerver(TcpClient c)
        {
            this.input = new StreamReader(c.GetStream(), Encoding.UTF8);
            this.output = new StreamWriter(c.GetStream(), Encoding.UTF8);
        }

        public void StartKomm()
        {
            output.WriteLine("DVD rental --- Available commands");
            output.Flush();
            bool ok = true;
            while (ok)
            {
                string parancs = string.Empty;
                try
                {
                    string commands = input.ReadLine();
                    string[] data = commands.Split('|');
                    parancs = data[0].ToUpper();
                    switch (parancs)
                    {
                        case "HELP": Help(); break;
                        case "LOGIN": Login(data[1], data[2]); break;
                        case "LOGOUT": Logout(); break;
                        case "ADDUSER": Adduser(data[1], data[2]); break;
                        case "USERDEL": Userdel(data[1]); break;
                        case "USERLIST": Userlist(); break;
                        case "ONLINEUSERLIST": Onlineuserlist(); break;
                        case "LIST": Lista(); break;
                        case "RENT": rental(data[1]); break;
                        case "BACK": Back(data[1]); break;
                        case "EXIT": output.WriteLine("The cliend disconnected"); ok = false; break;
                        default: output.WriteLine("ERR|Invalid command"); break;
                    }
                }
                catch (Exception e)
                {
                    output.WriteLine("ERR|{0}", e.Message);
                }
                output.Flush();
            }
            Console.WriteLine("The client disconnected({0}) .", login_user);
        }

        void Userlist()
        {
            if (this.login_user != "admin")
            {
                output.WriteLine("ERR|Please log in as Amin");
            }
            else
            {
                output.WriteLine("OK*");
                output.WriteLine("User".PadLeft(30) + "Password".PadLeft(15));
                lock (Program.Users)
                {
                    foreach (User user in Program.Users)
                    {
                        output.WriteLine(user.name.PadLeft(30) + user.password.PadLeft(15));
                    }
                }
                output.WriteLine("OK!");
            }
        }

        void Onlineuserlist()
        {
            output.WriteLine("OK*");
            output.WriteLine("Aktív users:".PadLeft(30));
            lock (Program.ActiveUsers)
            {
                foreach (String name in Program.ActiveUsers)
                {
                    output.WriteLine(name.PadLeft(30));
                }
            }
            output.WriteLine("OK!");
        }

        void rental(string id)
        {
            if (login_user != String.Empty)
            {
                lock (Program.Films)
                {
                    int j = 0;
                    for (j = 0; j < Program.Films.Count && Program.Films[j].Id != id; j++) ;
                    if (j >= Program.Films.Count)
                    {
                        output.WriteLine("ERR|Missing DVD");
                    }
                    else
                        if (Program.Films[j].User == String.Empty)
                    {
                        Program.Films[j].User = login_user;
                        Program.Films[j].Rented= true;
                        output.WriteLine("OK|Rented");
                    }
                    else
                    {
                        output.WriteLine("ERR|This film has been rented");
                    }
                }
            }
            else
            {
                output.WriteLine("ERR|Log in to rent!");
            }

        }

        void Back(string id)
        {
            if (login_user != String.Empty)
            {
                lock (Program.Films)
                {
                    int j = 0;
                    for (j = 0; j < Program.Films.Count && Program.Films[j].Id != id; j++) ;
                    if (j >= Program.Films.Count)
                    {
                        output.WriteLine("ERR|Missing DVD");
                    }
                    else
                        if (Program.Films[j].User == login_user)
                    {
                        Program.Films[j].User = String.Empty;
                        Program.Films[j].Rented = false;
                        output.WriteLine("OK|Back is okay");
                    }
                    else
                    {
                        output.WriteLine("ERR|Rented by other user!");
                    }
                }
            }
            else
            {
                output.WriteLine("ERR|Log in for taking back");
            }
        }
        void Logout()
        {
            if (login_user != String.Empty)
            {
                login_user = String.Empty;
                output.WriteLine("OK|Logged out succesfully");
                Program.ActiveUsers.Remove(login_user);
            }
            else
            {
                output.WriteLine("ERR|Nobody logged in");
            }

        }

        void Help()
        {
            output.WriteLine("OK*");
            output.WriteLine("LIST : List of films");
            output.WriteLine("LOGIN|<name>|<pass>: To log in");
            output.WriteLine("LOGOUT: To log out");
            output.WriteLine("RENT|<id>: Rent a DVD by id");
            output.WriteLine("BACK|<id>: Take the DVD back");
            output.WriteLine("ADDUSER|<acc>|<pass>: Add new user - only by admin");
            output.WriteLine("USERDEL|<acc>: Delete a user - only by admin");
            output.WriteLine("USERLIST: List of users - only by admin");
            output.WriteLine("ONLINEUSERLIST: Active users");
            output.WriteLine("EXIT: To disconnect");
            output.WriteLine("OK!");

        }
        void Login(string name, string password)
        {
            lock (Program.Users)
            {
                foreach (User user in Program.Users)
                {
                    if (user.name == name && user.password == password)
                    {
                        login_user = name;
                        output.WriteLine("OK|You have logged in");
                        Program.ActiveUsers.Add(login_user);
                        break;
                    }

                }
            }
            if (login_user != name)
            {
                output.WriteLine("ERR|Incorrect name or password");
            }
        }
        void Adduser(string name, string password)
        {
            if (this.login_user != "admin")
            {
                output.WriteLine("ERR|Log in as admin");
            }
            else
            {
                Boolean missing = true;
                lock (Program.Users)
                {
                    foreach (User user in Program.Users)
                    {
                        if (user.name == name)
                        {
                            output.WriteLine("ERR|Existing user");
                            missing = false;
                            break;
                        }
                    }
                    if (missing)
                    {
                        User newUser = new User(name, password);
                        Program.Users.Add(newUser);
                        output.WriteLine("OK|Saved data");
                    }
                }
            }
        }

        void Userdel(string name)
        {
            if (this.login_user != "admin")
            {
                output.WriteLine("ERR|Log in as admin");
            }
            else
            {
                bool missing = true;
                lock (Program.Users)
                {
                    foreach (User user in Program.Users)
                    {
                        if (user.name == name)
                        {
                            Program.Users.Remove(user);
                            output.WriteLine("OK|The user has been deleted.");
                            missing = false;
                            break;
                        }
                    }
                    if (missing)
                    {
                        output.WriteLine("ERR|Existing user");
                    }
                }
            }
        }
        void Lista()
        {
            output.WriteLine("OK*");
            output.WriteLine("ID".PadRight(12) + "Film title: ".PadRight(15)+ "Film rented: ");
            foreach (DVD_film film in Program.Films)
                output.WriteLine(film.Id.PadRight(12) + film.Title.PadRight(15)+film.Rented);
            output.WriteLine("OK!");
        }
    }
 

    class Program
    {
    public static List<DVD_film> Films = new List<DVD_film>();
    public static List<User> Users = new List<User>();
    public static List<string> ActiveUsers = new List<string>();
    static void ReadingData()
    {
        string[] films = File.ReadAllLines("lista.txt", Encoding.Default);
        foreach (string sor in films)
	    {
            string[] data = sor.Split('|');
            DVD_film ujDVD = new DVD_film(data[0], data[1], false);
            Films.Add(ujDVD);
        }
        string[] users = File.ReadAllLines("users.txt", Encoding.Default);
        foreach (string sor in users)
        {
            string[] data = sor.Split('|');
            User newUser = new User(data[0], data[1]);
            Users.Add(newUser);
        }
    }
    


    static void Main(string[] args)
        {
        //string ip = ConfigurationManager.AppSettings["IP"];
        IPAddress ip = IPAddress.Parse("127.0.0.1");
            //int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            ReadingData();
            TcpListener listener = new TcpListener(ip, 44444);
            listener.Start();
            bool vege = false;
            while (!vege)
            {
                Console.WriteLine("The server has been startad");
                TcpClient kliens = listener.AcceptTcpClient();
                Szerver rental = new Szerver(kliens);
                Thread th = new Thread(rental.StartKomm);
                th.Start();
            }
        }
    }
}
