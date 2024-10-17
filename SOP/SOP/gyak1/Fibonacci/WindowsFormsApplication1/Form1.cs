using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Worker fib;
        static public RichTextBox present;
        static string fibo1, fibo2;
        
        public Form1()
        {
            InitializeComponent();
        }
        public Form1(string fib1,string fib2)
        {
            InitializeComponent();
            fibo1 = fib1; fibo2 = fib2;
        }

        public class Worker
        {
         
        
            ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
            ManualResetEvent _pauseEvent = new ManualResetEvent(true);
            Thread _thread;
            Form m2;

            public Worker() {
                m2 = new Form();
                m2.Size = new Size(400, 300);
                present=new RichTextBox();
                present.Size = new Size(350, 250);
                m2.Text = "Fibonacci numbers";
                m2.Show();
                m2.Controls.Add(present);
                
               
            }

            public void Start()
            {
                _thread = new Thread(DoWork);
                _thread.Start();
            }

            public void Pause()
            {
                _pauseEvent.Reset();
            }

            public void Resume()
            {
                _pauseEvent.Set();
            }

            public void Stop()
            {
                // Signal the shutdown event
                _shutdownEvent.Set();

                // Make sure to resume any paused threads
                _pauseEvent.Set();

                // Wait for the thread to exit
                _thread.Join();
            }

            public IEnumerable<int> FibNumbers(int s1, int s2)
            {
                int f1 = s1;
                int f2 = s2;
                yield return f1;
                yield return f2;
                while (true)
                {
                    int tf = f1 + f2;
                    f1 = f2;
                    f2 = tf;
                    yield return f2;
                }
            }

            public void DoWork()
            {
                var fibtmp = FibNumbers(int.Parse(fibo1),int.Parse(fibo2));
                var enumerator = fibtmp.GetEnumerator();
                string str;
                while (true)
                {
                    _pauseEvent.WaitOne(Timeout.Infinite);

                    if (_shutdownEvent.WaitOne(0))
                        break;
                    enumerator.MoveNext();
                    str=enumerator.Current.ToString();
                    present.Invoke((MethodInvoker)delegate() { present.AppendText(str + ' '); });
                    Thread.Sleep(1000);
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            fib = new Worker();
            fib.Start();
            button1.Enabled = false;
            button2.Enabled = true;
            
            button4.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            button2.Enabled = false;
            fib.Pause();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;
            fib.Resume();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            fib.Stop();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (XmlWriter writer = XmlWriter.Create("Fibonacci.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("F");
                foreach (string number in present.Lines)
                {
                    writer.WriteStartElement("Numbers");
                    writer.WriteElementString("F", number);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
