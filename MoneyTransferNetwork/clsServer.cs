using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using PacketLib;

namespace MoneyTransferSystem
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

        public bool IsRunning
        {
            get
            {
                return this.is_running;
            }
        }

        public bool Start()
        {
            bool res = false;

            try
            {
                if (this.is_running)
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
                if (s != null)
                {
                    s.Close();
                }

                s = null;
                is_running = false;
            }


            return res;
        }

        public bool Stop()
        {
            bool res = false;

            try
            {
                if (is_running)
                {
                    s.Close();
                    is_running = false;
                    res = true;
                }
            }
            catch
            {

            }
            return res;
        }

        object o = new Object();



        public void OnAccept(IAsyncResult ar)
        {

            lock (o)

            {
                // Socket c = null;


                try
                {
                    Socket c = this.s.EndAccept(ar);
                    this.s.BeginAccept(new AsyncCallback(OnAccept), null);
                    RequestProcess rp = new RequestProcess(c);

                }
                catch
                {
                    this.s.Close();
                    this.is_running = false;
                    return;
                }
            }

          

           // this.s.BeginAccept(new AsyncCallback(OnAccept), null);

        }
    }

    internal class RequestProcess
    {
        internal RequestProcess(Socket c)
        {
            byte[] buffer = new byte[1024 * 4];


            try
            {
                int len = c.Receive(buffer);

                if (len > 0)
                {
                    byte[] temp = new byte[len];
                    Array.Copy(buffer, 0, temp, 0, len);

                    Packet packet = new Packet(temp);

                    switch (packet.GetCommand())
                    {
                        case CommandType.Command1:
                            {
                                c.Send(new Packet(CommandType.Command1, new byte[] { 1, 2, 3 }).ToBytes());
                                break;
                            }
                        case CommandType.Command2:
                            {
                                //long num = OutGoingTransfer.GenerateNewOGTNumber();
                                //c.Send(new Packet(CommandType.Command2,Encoding.UTF8.GetBytes(num.ToString())).ToBytes());
                                break;
                            }
                        case CommandType.Command3:
                            {
                                string t = Encoding.UTF8.GetString(packet.GetBuffer());

                                OutGoingTransfer ogt = new OutGoingTransfer(t);

                                long nid = OutGoingTransfer.GenerateNewOGTNumber();
                                OutGoingTransfer ogt2 = new OutGoingTransfer(nid, ogt.FromPoint, ogt.ToPoint, ogt.UserInfo, ogt.Amount, ogt.SenderName, ogt.ReciverName, ogt.Date);

                                if (OutGoingTransfer.Add(ogt2))
                                {
                                    Event.Add(new Event("New Transction added, NUM=" + ogt2.Number, DateTime.Now));
                                    c.Send(new Packet(CommandType.Command3, ogt2.ToBytes()).ToBytes());
                                }
                                break;
                            }
                        case CommandType.Command4:
                            {
                                string t = Encoding.UTF8.GetString(packet.GetBuffer());
                                long num = long.Parse(t);

                                OutGoingTransfer ogt = new OutGoingTransfer(0, 0, 0, 0, 0, "", "", new DateTime());

                                if (OutGoingTransfer.Query(num, ref ogt))
                                {
                                    Event.Add(new Event("Query,Transction found, NUM=" + ogt.Number, DateTime.Now));
                                    c.Send(new Packet(CommandType.Command4, ogt.ToBytes()).ToBytes());
                                }
                                else
                                {
                                    Event.Add(new Event("Query,Transction not found, NUM=" + ogt.Number, DateTime.Now));
                                }

                                break;
                            }
                        case CommandType.Command5:
                            {
                                string t = Encoding.UTF8.GetString(packet.GetBuffer());
                                User user = new User(t);

                                if (User.Login(user))
                                {
                                    Event.Add(new Event("SUCCESS LOGIN,USER=" + user.Username, DateTime.Now));
                                    c.Send(new Packet(CommandType.Command5, user.ToBytes()).ToBytes());
                                }
                                else
                                {
                                    Event.Add(new Event("FIELD LOGIN,USER=" + user.Username, DateTime.Now));
                                }

                                break;
                            }
                        case CommandType.Command6:
                            {
                                // c.Send(new Packet(CommandType.Command1, new byte[] { 1, 2, 3 }).ToBytes());
                                break;
                            }
                        case CommandType.Command7:
                            {
                                //   c.Send(new Packet(CommandType.Command1, new byte[] { 1, 2, 3 }).ToBytes());
                                break;
                            }

                    }


                }

                c.Close();
            }
            catch
            {
            }
        }
    }
}
