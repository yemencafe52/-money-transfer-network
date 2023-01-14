using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using PacketLib;

namespace mynetworklib
{
    public class SocketCommander
    {
        private ushort port;
        private string ip;

        public SocketCommander(ushort port,string ip)
        {
            this.port = port;
            this.ip = ip;
        }

        public bool GetObject(Packet p,ref byte[] res)
        {
            bool res0 = false;

            try
            {
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    s.Connect(this.ip, this.port);

                 //   s.SendTimeout = 30000;
                 //   s.ReceiveTimeout = 30000;
                    if (s.Send(p.ToBytes()) > 0)
                    {
                        byte[] buffer = new byte[1024 * 4];
                        
                       
                        int len = s.Receive(buffer);

                        if (len > 0)
                        {
                            res = new byte[len-1];
                            Array.Copy(buffer, 1, res, 0, len-1);
                            res0 = true;

                        }
                    }

                    s.Close();
                }
                catch
                { 
                }
            }
            catch
            {
            }

            return res0;
        }

    }
}
