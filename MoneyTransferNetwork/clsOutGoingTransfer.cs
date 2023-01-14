using dbmgrlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransferSystem
{
    internal class OutGoingTransfer
    {
        private long ogt_no;
        private byte from_point;
        private byte to_point;
        private byte user_no;
        private double ogt_amount;
        private string ogt_sendername;
        private string ogt_reciverrname;
        private DateTime ogt_date;

        //========================
        private static long ogt_number = 0;
        private static object mylocker = new object();

        internal OutGoingTransfer(
                 long ogt_no,
                 byte from_point,
                 byte to_point,
                 byte user_no,
                 double ogt_amount,
                 string ogt_sendername,
                 string ogt_reciverrname,
                 DateTime ogt_date

        )
        {
            this.ogt_no = ogt_no;
            this.from_point = from_point;
            this.to_point = to_point;
            this.user_no = user_no;
            this.ogt_amount = ogt_amount;
            this.ogt_sendername = ogt_sendername;
            this.ogt_reciverrname = ogt_reciverrname;
            this.ogt_date = ogt_date;
        }


        //internal OutGoingTransfer(string data)
        //{  
        //    string[] info = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //    this.ogt_no = Convert.ToByte(info[0]);
        //    this.from_point = Convert.ToByte(info[1]);
        //    this.to_point = Convert.ToByte(info[2]);
        //    this.user_no = Convert.ToByte(info[3]);
        //    this.ogt_amount = Convert.ToByte(info[4]);
        //    this.ogt_sendername = (info[5]);
        //    this.ogt_reciverrname = (info[6]);
        //    this.ogt_date = Convert.ToDateTime(info[7]);
           
        //}


        internal long Number
        {
            get
            {
                return this.ogt_no;
            }

        }

        internal byte FromPoint
        {
            get
            {
                return this.from_point;
            }

        }

        internal byte ToPoint
        {
            get
            {
                return this.to_point;
            }

        }

        internal byte UserInfo
        {
            get
            {
                return this.user_no;
            }

        }


        internal double Amount
        {
            get
            {
                return this.ogt_amount;
            }

        }

        internal string SenderName
        {
            get
            {
                return this.ogt_sendername;
            }

        }

        internal string ReciverName
        {
            get
            {
                return this.ogt_reciverrname;
            }

        }

        internal DateTime Date
        {
            get
            {
                return this.ogt_date;
            }

        }


        internal byte[] ToBytes()
        {
            //byte[] res = new byte[0];

            StringBuilder sb = new StringBuilder();
            sb.Append(this.ogt_no.ToString());
            sb.Append(";");
            sb.Append(this.from_point.ToString());
            sb.Append(";");
            sb.Append(this.to_point.ToString());
            sb.Append(";");
            sb.Append(this.user_no.ToString());
            sb.Append(";");
            sb.Append(this.ogt_amount.ToString());
            sb.Append(";");
            sb.Append(this.ogt_sendername.ToString());
            sb.Append(";");
            sb.Append(this.ogt_reciverrname.ToString());
            sb.Append(";");
            sb.Append(this.ogt_date.ToString());

            return Encoding.UTF8.GetBytes(sb.ToString());
                
        }

        internal OutGoingTransfer(string data)
        {

            try
            {
                string[] info = data.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                this.ogt_no = Convert.ToInt64(info[0]);
                this.from_point = Convert.ToByte(info[1]);
                this.to_point = Convert.ToByte(info[2]);
                this.user_no = Convert.ToByte(info[3]);
                this.ogt_amount = Convert.ToDouble(info[4]);
                //==========================================
                this.ogt_sendername = (info[5]);
                this.ogt_reciverrname = (info[6]);
                this.ogt_date = Convert.ToDateTime((info[7]));
            }
            catch
            {
                this.ogt_no = 0;
            }
        }

        internal static bool Add(OutGoingTransfer ogt)
        {
            lock (mylocker)
            {
                bool res = false;

                try
                {
                    AccessDB db = new AccessDB(Constants.ConnectionString);
                    string sql = "insert into tblOutGoingTransfer (ogt_no,from_point,to_point,user_no,ogt_amount,ogt_sendname,ogt_recivername,ogt_date) values (" + ogt.ogt_no + "," + ogt.from_point + "," + ogt.to_point + "," + ogt.user_no + "," + ogt.ogt_amount + ",'"+ ogt.ogt_sendername +"','"+ ogt.ogt_reciverrname +"','"+ ogt.ogt_date +"')";

                    if (db.ExcuteNonQuery(sql) > 0)
                    {
                        res = true;
                    }
                }
                catch
                {

                }
                return res;
            }
        }

        internal static long GenerateNewOGTNumber()
        {
            lock (mylocker)
            {
                if (ogt_number > 0)
                {
                    return ++ogt_number;
                }

                try
                {
                    AccessDB db = new AccessDB(Constants.ConnectionString);
                    string sql = "select max(ogt_no) as res from tblOutGoingTransfer";

                    if (db.ExcuteQuery(sql))
                    {
                        if (db.GetDataReader.Read())
                        {
                            string r1 = (db.GetDataReader["res"].ToString());

                            if (long.TryParse(r1, out ogt_number))
                            {
                                ++ogt_number;
                            }
                            else
                            {
                                ogt_number = 10000;
                            }
                        }
                    }

                    db.CloseConnection();
                }
                catch
                {

                }

                return ogt_number;
            }
        }

        internal static bool Query(long number,ref OutGoingTransfer ogt)
        {
            lock (mylocker)
            {

                bool res = false;

                try
                {
                    AccessDB db = new AccessDB(Constants.ConnectionString);
                    string sql = "select ogt_no,from_point,to_point,user_no,ogt_amount,ogt_sendname,ogt_recivername,ogt_date from tblOutGoingTransfer where ogt_no=" + number;

                    if (db.ExcuteQuery(sql))
                    {
                        if (db.GetDataReader.Read())
                        {
                            ogt.ogt_no = Convert.ToInt64(db.GetDataReader["ogt_no"].ToString());
                            ogt.from_point = Convert.ToByte(db.GetDataReader["from_point"].ToString());
                            ogt.to_point = Convert.ToByte(db.GetDataReader["to_point"].ToString());
                            ogt.user_no = Convert.ToByte(db.GetDataReader["user_no"].ToString());
                            ogt.ogt_amount = Convert.ToDouble(db.GetDataReader["ogt_amount"].ToString());
                            ogt.ogt_sendername = (db.GetDataReader["ogt_sendname"].ToString());
                            ogt.ogt_reciverrname = (db.GetDataReader["ogt_recivername"].ToString());
                            ogt.ogt_date = Convert.ToDateTime(db.GetDataReader["ogt_date"].ToString());
                            res = true;
                        }
                    }

                    db.CloseConnection();
                }
                catch
                {

                }
                return res;
            }
        }

    }
}
