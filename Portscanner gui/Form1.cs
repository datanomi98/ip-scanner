using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;






namespace Portscanner_gui
{

    public partial class Form1 : Form
    {
        public static string ipagain;
        public static int ipcount = 1;
        public static long ipLong;

        public Form1()
        {
            InitializeComponent();
        }
        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 64;
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 2);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            int i = 0;
            while (i < 4)
            {
                i++;
                string testi = i.ToString();
                //DataGridView ip = new DataGridView();
                ip.ColumnCount = 1;
                ip.Columns[0].Name = "IP";

                DataGridViewRow row = (DataGridViewRow)ip.Rows[0].Clone();

                ip.Rows.Add(testi);
                
                
                
            }
        }
        public static void sendAsyncPingPacket(string hostToPing)
        {
            try
            {


                int timeout = 3000;
              
                Ping pingPacket = new Ping();
                //ping completion event reaised
                pingPacket.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                string data2 = "If someone can crack this contact me: datanomi@protonmail.com";
                string dataen = EncryptString(data2, "ez");
               

                byte[] data = Encoding.ASCII.GetBytes(dataen);
                PingOptions pingOptions = new PingOptions(64, true);
               
               
                //send ping
                
                pingPacket.SendAsync(hostToPing, timeout, data, pingOptions);
              
                ipagain = hostToPing;


                //do something useful
            

               


               
            }
            catch (PingException pe)
            {
                MessageBox.Show(pe.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                   

                    // Let the main thread resume.
                    // UserToken is the AutoResetEvent object that the main thread
                    // is waiting for.
                   // ((AutoResetEvent)e.UserState).Set();
                }

                // If an error occurred, display the exception to the user.
                if (e.Error != null)
                {
                   
                    // Let the main thread resume.
                    //((AutoResetEvent)e.UserState).Set();
                }

                PingReply reply = e.Reply;

                DisplayReply(reply);

                // Let the main thread resume.
                //((AutoResetEvent)e.UserState).Set();
            }
            catch (PingException pe)
            {
                MessageBox.Show(pe.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)

                return;

            

            if (reply.Status == IPStatus.Success)
            {
                MessageBox.Show("ping succesfull");
            }
            else

            {

                while (ipcount < 4)
                {
                    long ipag = ToInt(ipagain);

                    string address = ToAddr(ipag);



                    ipcount++;
                    if (ipcount <= 4)
                    {

                        sendAsyncPingPacket(address);
                    }
                    else if (reply.Status == IPStatus.Success)
                    {
                        scan();
                    }
                    else
                    {

                        scan();

                    }

                }

                }
            }
        private static long ToInt(string addr)
        {

            return (long)(uint)System.Net.IPAddress.NetworkToHostOrder(
              (int)System.Net.IPAddress.Parse(addr).Address);

        }

        private static string ToAddr(long address)
        {
            return System.Net.IPAddress.Parse(address.ToString()).ToString();
        }

        static int temp = 0;
        private static IPAddress ipAddress;
        private static byte[] bytes;

        private static void scanLiveHosts(string ipFrom, string ipTo)
        {
            long from = ToInt(ipFrom);
            long to = ToInt(ipTo);

            ipLong = ToInt(ipFrom);
            while (from < to)
            {
                ipcount = 1;
                scan();

            }


        }
        private static void scan()
        {
            
            
            string address = ToAddr(ipLong);
            sendAsyncPingPacket(address);
            ipLong++;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            


                //sendAsyncPingPacket("91.153.139.20");

                scanLiveHosts("91.153.139.10", "91.153.139.12");

            
            


        }
    }
}

    






