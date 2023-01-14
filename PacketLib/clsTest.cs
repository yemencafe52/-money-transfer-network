//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PacketLib
//{
//    static class clsTest
//    {
//        static void Main()
//        {
//            Packet p = new Packet(CommandType.Command1);
//            CommandType c1 = p.GetCommand();

//            Packet p2 = new Packet(CommandType.Command2, new byte[] { 1, 2, 3 });
//            byte[] buf0 = p2.ToBytes();

//            Packet p3 = new Packet(buf0);

//            byte[] buf1 = p3.GetBuffer();
//        }
//    }
//}
