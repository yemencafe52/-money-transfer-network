using mynetworklib;
using PacketLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class User
    {
        private byte user_no;
        private string user_username;
        private string user_password;
        //===========================
        private static User activeUser = null;
        private static object mylocker2 = new object();

        internal User(
             byte user_no,
             string user_username,
             string user_password
         )
        {
            this.user_no = user_no;
            this.user_username = user_username;
            this.user_password = user_password;
        }

        internal User(byte user_no)
        {
            try
            {
               
            }
            catch
            {

            }
        }

        internal byte Number
        {
            get
            {
                return this.user_no;
            }
        }

        internal string Username
        {
            get
            {
                return this.user_username;
            }
        }

        internal string Password
        {
            get
            {
                return this.user_password;
            }
        }

        internal static User ActiveUser
        {
            get
            {
                return activeUser;
            }
        }

        internal User(string data)
        {
            string[] info = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            this.user_no = Convert.ToByte(info[0]);
            this.user_username = (info[1]);
            this.user_password = (info[2]);

        }
        internal byte[] ToBytes()
        {
            //byte[] res = new byte[0];

            StringBuilder sb = new StringBuilder();
            sb.Append(this.user_no.ToString());
            sb.Append(";");
            sb.Append(this.user_username.ToString());
            sb.Append(";");
            sb.Append(this.user_password.ToString());

            return Encoding.UTF8.GetBytes(sb.ToString());

        }


        internal static bool Login(User user)
        {

            lock (mylocker2)
            {
                bool res = false;

                try
                {
                    SocketCommander sc = new SocketCommander(Constants.GetPort, Constants.GetIP);
                    Packet p = new Packet(CommandType.Command5,user.ToBytes());

                    byte[] buffer = new byte[1024 * 4];

                    if(sc.GetObject(p,ref buffer))
                    {
                        Event.Add(new Event("تسجيل دخول النظام", DateTime.Now));
                        activeUser = user;
                        res = true;
                    }
                    else
                    {
                        Event.Add(new Event("تسجيل دخول فاشل ", DateTime.Now));
                    }

                }
                catch
                {

                }

                return res;
            }
        }

        internal static bool Update(User user)
        {
            lock (mylocker2)
            {
                bool res = false;

                return res;
            }
        }
    }
}
