using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbmgrlib
{
    static class Test
    {
        static void Main()
        {
            Test0();
        }

        static bool Test0()
        {
            bool res = false;
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Application.StartupPath + "\\database\\db.accdb" + ";Persist Security Info=False;";

            AccessDB db = new AccessDB(connectionString);

            string sql = "insert into tblLog (l_no,l_date) values(1,'"+DateTime.Now.ToString()+"')";

            if (db.ExcuteNonQuery(sql) > 0)
            {
                sql = "select l_no from tblLog";
                if (db.ExcuteQuery(sql))
                {
                    res = true;
                }

                db.CloseConnection();
            }

            return res;
        }
    }
}
