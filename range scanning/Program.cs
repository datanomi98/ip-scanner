using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace range_scanning
{
    class Program
    {
        public static string ipagain;
        public static int ipcount = 1;
        public static long ipLong;

        private const string initVector = "pemgail9uzpgzl88";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 64;
        //Encrypt the sent message.
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
        public static void sendAsyncPingPacket(string hostToPing)
        {
            try
            {
                
                
                int timeout = 3000;
                AutoResetEvent waiter = new AutoResetEvent(false);
                Ping pingPacket = new Ping();
                //ping completion event reaised
                pingPacket.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

                string data2 = "If someone can crack this contact me: datanomi@protonmail.com";
                string dataen = EncryptString(data2, "ez");
               
                byte[] data = Encoding.ASCII.GetBytes(dataen);
                PingOptions pingOptions = new PingOptions(64, true);
                Console.WriteLine("Time to live: {0}", pingOptions.Ttl);
                //Console.WriteLine("Don't fragment: {0}", pingOptions.DontFragment);
                //send ping
               
                pingPacket.SendAsync(hostToPing, timeout, data, pingOptions, waiter);
                Console.WriteLine("ip: " + hostToPing);
                ipagain = hostToPing;


                //do something useful
                waiter.WaitOne();
                //Console.WriteLine("Ping RoundTrip returned, Do something useful here...");
               


                // Console.ReadLine();
            }
            catch (PingException pe)
            {
                Console.WriteLine("INVALID IP ADDRESS FOUND");
                Console.WriteLine(pe.Message);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceptin " + ex.Message);
                Console.ReadLine();
            }

        }
        private static void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    Console.WriteLine("Ping canceled.");
                    Console.ReadLine();

                    // Let the main thread resume.
                    // UserToken is the AutoResetEvent object that the main thread
                    // is waiting for.
                    ((AutoResetEvent)e.UserState).Set();
                }

                // If an error occurred, display the exception to the user.
                if (e.Error != null)
                {
                    Console.WriteLine("Ping failed>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ");
                    //this will print exception
                    //Console.WriteLine (e.Error.ToString ());
                    Console.ReadLine();
                    // Let the main thread resume.
                    ((AutoResetEvent)e.UserState).Set();
                }

                PingReply reply = e.Reply;

                DisplayReply(reply);

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }
            catch (PingException pe)
            {
                Console.WriteLine("INVALID IP ADDRESS");
                Console.WriteLine(pe.Message);
            }
            catch (Exception ex)
            {
                Console.ReadLine();
                Console.WriteLine("Exception " + ex.Message);
            }
        }

        public static void DisplayReply(PingReply reply)
        {
            if (reply == null)

                return;

            Console.WriteLine("ping status: {0}", reply.Status);
          
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                //Console.WriteLine ("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                Console.WriteLine("\n");
            }
            else

            {

                while (ipcount < 4)
                {
                    long ipag = Program.ToInt(ipagain);

                    string address = Program.ToAddr(ipag);
                    Console.WriteLine("ip:" + address);

                    Console.WriteLine(ipcount);
                    ipcount++;
                    if (ipcount<= 4)
                    {
                        Console.WriteLine("\n");
                        sendAsyncPingPacket(address);
                    }
                    else if(reply.Status == IPStatus.Success)
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
            long from = Program.ToInt(ipFrom);
            long to = Program.ToInt(ipTo);

          ipLong = Program.ToInt(ipFrom);
            while (from < to)
            {
                ipcount = 1;
                scan();
             
            }
           

        }
        private static void scan()
        {
            //Console.WriteLine(ipLong);
            Console.WriteLine("\n");
            string address = Program.ToAddr(ipLong);
            Program.sendAsyncPingPacket(address);
            ipLong++;
        }
        static void Main(string[] args)
        {

            try
            {
                Program.getDeviceList();
                Program.sendAsyncPingPacket("91.153.139.20");
                Program.scanLiveHosts("91.153.139.10", "91.153.139.60");



            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
                Console.ReadLine();
            }
            catch (NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
        private static void getDeviceList()
        {

        }
       
    }
}

