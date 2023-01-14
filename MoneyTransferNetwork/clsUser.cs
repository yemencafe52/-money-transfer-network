using dbmgrlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransferSystem
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
        internal User(byte user_no)
        {
            try
            {
                AccessDB db = new AccessDB(Constants.ConnectionString);
                string sql = "select user_no,user_username,user_password from tblUsers where user_no=" + user_no;

                if (db.ExcuteQuery(sql))
                {
                    if (db.GetDataReader.Read())
                    {
                        this.user_no = Convert.ToByte(db.GetDataReader["user_no"].ToString());
                        this.user_username = (db.GetDataReader["user_username"].ToString());
                        this.user_password = (db.GetDataReader["user_password"].ToString());
                    }
                }

                db.CloseConnection();
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

        internal static bool Add(User user)
        {
            lock (mylocker2)
            {
                bool res = false;

                AccessDB db = new AccessDB(Constants.ConnectionString);
                string sql = "insert into tblUsers (user_no,user_username,user_password) values(" + user.user_no + ",'" + user.user_username + "','" + user.user_password + "')";

                if (db.ExcuteNonQuery(sql) > 0)
                {
                    res = true;
                }

                return res;

            }
        }


        internal static bool Login(User user)
        {

            lock (mylocker2)
            {
                bool res = false;

                try
                {
                    AccessDB db = new AccessDB(Constants.ConnectionString);
                    string sql = "select user_no,user_username,user_password from tblUsers where user_username = '" + user.Username + "' and user_password = '" + user.Password + "'";

                    if (db.ExcuteQuery(sql))
                    {
                       // activeUser = user;
                        res = true;
                    }

                    db.CloseConnection();

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

                AccessDB db = new AccessDB(Constants.ConnectionString);
                string sql = "update tblUsers set user_password='" + user.user_password + "' where user_no=" + user.user_no;

                if (db.ExcuteNonQuery(sql) > 0)
                {
                    res = true;
                }

                return res;
            }
        }
    }
}
