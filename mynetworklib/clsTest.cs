using PacketLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mynetworklib
{
    static class clsTest
    {
        static void Main()
        {
            Server s = new Server(1234);
            s.Start();
            SocketCommander sc = new SocketCommander(1234, "127.0.0.1");

            byte[] buffer = new byte[0];

            Packet p = new Packet(CommandType.Command1);

            bool res  = sc.GetObject(p,ref buffer);
            s.Stop();


        }
    }
}
