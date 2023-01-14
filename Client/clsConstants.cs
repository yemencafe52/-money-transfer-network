using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class Constants
    {
        private static ushort port = 1234;
        private static string ip = "127.0.0.1";

        public static ushort GetPort
        {
            get
            {
                return port;
            }
        }

        public static string GetIP
        {
            get
            {
                return ip;
            }
        }
    }
}
