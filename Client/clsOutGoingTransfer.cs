
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mynetworklib;
using PacketLib;

namespace Client
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

        internal static bool Add(ref OutGoingTransfer ogt)
        {
            lock (mylocker)
            {
                bool res = false;

                try
                {
                    SocketCommander sc = new SocketCommander(Constants.GetPort, Constants.GetIP);
                    Packet p = new Packet(CommandType.Command3, ogt.ToBytes());

                    byte[] buffer = new byte[1024 * 4];

                    if (sc.GetObject(p, ref buffer))
                    {
                        ogt = new OutGoingTransfer(Encoding.UTF8.GetString(buffer));
                        Event.Add(new Event("تم ارسال حوالة رقم " + ogt.ogt_no + " بنجاح ", DateTime.Now));
                        
                        res = true;
                    }
                    else
                    {
                        Event.Add(new Event("تعذر ارسال حوالة ", DateTime.Now));
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

                long res = 0;
                try
                {
                    SocketCommander sc = new SocketCommander(Constants.GetPort, Constants.GetIP);
                    Packet p = new Packet(CommandType.Command2);

                    byte[] buffer = new byte[1024 * 4];

                    if (sc.GetObject(p, ref buffer))
                    {
                        string temp = Encoding.UTF8.GetString(buffer);
                        res = long.Parse(temp);
                    }
                }
                catch
                {

                }

                return res;
            }
        }

        internal static bool Query(long number, ref OutGoingTransfer ogt)
        {
            lock (mylocker)
            {
                bool res = false;

                try
                {
                    SocketCommander sc = new SocketCommander(Constants.GetPort, Constants.GetIP);
                    Packet p = new Packet(CommandType.Command4,Encoding.UTF8.GetBytes(number.ToString()));

                    byte[] buffer = new byte[1024 * 4];

                    if (sc.GetObject(p, ref buffer))
                    {
                        Event.Add(new Event("تم استعلام عن حوالة رقم " + number + " بنجاح ", DateTime.Now));
                        string temp = Encoding.UTF8.GetString(buffer);
                        ogt = new OutGoingTransfer(temp);
                        res = true;
                    }
                    else
                    {
                        Event.Add(new Event("تم استعلام عن حوالة رقم " + number + " ولم يتم العثور عليها ", DateTime.Now));
                    }
                }
                catch
                {

                }

               
                return res;
            }
        }

    }
}
