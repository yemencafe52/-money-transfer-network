using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dbmgrlib;

namespace MoneyTransferSystem
{
    internal class Point
    {
        private byte point_no;
        private string point_name;
        //========================
        private static byte currentPoint = 1;

        internal static byte GetCurrentPoint
        {
            get
            {
                return currentPoint;
            }
        }

        internal Point(
            byte point_no,
            string point_name
        )
        {
            this.point_no = point_no;
            this.point_name = point_name;
        }

        internal Point(byte number)
        {
            try
            {
                AccessDB db = new AccessDB(Constants.ConnectionString);
                string sql = "select point_no,point_name from tblPoints where point_no=" + number;

                if(db.ExcuteQuery(sql))
                {
                    if(db.GetDataReader.Read())
                    {
                        this.point_no = Convert.ToByte(db.GetDataReader["point_no"].ToString());
                        this.point_name = (db.GetDataReader["point_no"].ToString());
                    }
                }

                db.CloseConnection();
            }
            catch
            {
                this.point_no = 0;
            }

        }

        internal byte GetNumber
        {
            get
            {
                return this.point_no;
            }
        }

        internal string GetName
        {
            get
            {
                return this.point_name;
            }
        }

        internal static bool Add(Point point)
        {
            bool res = false;

            try
            {
                AccessDB db = new AccessDB(Constants.ConnectionString);
                string sql = "insert into tblPoints (point_no,point_name) values("+ point.point_no +",'"+ point.point_name +"')";

                if(db.ExcuteNonQuery(sql) > 0)
                {
                    res = true;
                }
            }
            catch
            {

            }

            return res;
        }

        internal static List<Point> GetALL()
        {
            List<Point> res = new List<Point>();
            return res;
        }
    }
}
