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
    public class Server
    {
        private Socket s;
        private ushort port;
        private bool is_running = false;

        public Server(ushort port)
        {
            this.port = port;
        }

        public bool Start()
        {
            bool res = false;

            try
            {
                if(this.is_running)
                {
                    return res;
                }

                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), this.port));
                s.Listen(0);
                this.s.BeginAccept(new AsyncCallback(OnAccept), null);
                is_running = true;
                res = true;

            }
            catch
            {
                if(s != null)
                {
                    s.Close();
                }

                s = null;
            }


            return res;
        }

        public bool Stop()
        {
            bool res = false;

            try
            {
                if(is_running)
                {
                    s.Close();
                    res = true;
                }
            }
            catch
            {

            }
            return res;
        }

        public void OnAccept(IAsyncResult ar)
        {
            Socket c = null;

            try
            {
                c = this.s.EndAccept(ar);
            }
            catch
            {
                this.s.Close();
                this.is_running = false;
                return;
            }


            byte[] buffer = new byte[1024 * 4];

           
            try
            {
                int len = c.Receive(buffer);

                if(len > 0)
                {
                    byte[] temp = new byte[len];
                    Array.Copy(buffer, 0, temp, 0, len);

                    Packet packet = new Packet(temp);

                    switch(packet.GetCommand())
                    {
                        case CommandType.Command1:
                            {
                                c.Send(new Packet(CommandType.Command1, new byte[] { 1, 2, 3 }).ToBytes());
                                break;
                            }
                    }

                    
                }

                c.Close();
            }
            catch
            {
            }

            this.s.BeginAccept(new AsyncCallback(OnAccept), null);

        }
    }
}
