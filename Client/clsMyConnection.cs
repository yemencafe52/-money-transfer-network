using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using mynetworklib;

namespace Client
{
    public static class MyConnection
    {
        private static bool isRunning = false;
        private static bool isConnected = false;
        private static int ping = -1;
        //===========================
        private static Thread th0 = null;
        private static int ticker = 0;

        public static bool Start()
        {
            bool res = false;

            try
            {
                if(th0 != null)
                {
                    return res;
                }

                th0 = new Thread(Monitor);
                th0.Start();
                isRunning = true;
                res = true;

            }
            catch
            {

            }

            return res;
        }

        public static bool Stop()
        {
            bool res = false;
            try
            {
                if(th0 == null)
                {
                    return res;
                }

                isRunning = false;
                th0.Join();
                isConnected = false;
                ping = -1;
                res = true;
                
            }
            catch
            {

            }
            return res;
        }

        public static void Monitor()
        {
            while (isRunning)
            {
                SocketCommander sc = new SocketCommander(Constants.GetPort, Constants.GetIP);

                byte[] buffer = new byte[1024*4];
                ticker = Environment.TickCount;

                if (sc.GetObject(new PacketLib.Packet(PacketLib.CommandType.Command1),ref buffer))
                {
                    if (isConnected == false)
                    {
                        Event.Add(new Event("تم الاتصال", DateTime.Now));
                    }

                    ping = Environment.TickCount - ticker;
                    isConnected = true;

                }
                else
                {
                    Event.Add(new Event("الشبكة غير متوفرة ", DateTime.Now));
                    isConnected = false;
                    ping =-1;
                }

                Thread.Sleep(3000);
            }
        }

        public static bool IsConnected
        {
            get
            {
                return isConnected;
            }
        }

        public static bool IsRunning
        {
            get
            {
                return isRunning;
            }
        }

        public static int PING
        {
            get
            {
                return ping;
            }
        }
    }


}
