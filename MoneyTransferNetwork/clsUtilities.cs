using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using dbmgrlib;

namespace MoneyTransferSystem
{
    internal static class Utilities
    {

        internal static bool CheckDataBaseFile()
        {
            bool res = false;

            try
            {
                if(File.Exists(Constants.DbPath))
                {
                    res = true;
                }
            }
            catch
            {

            }

            return res;
        }

        internal static bool TestDb()
        {
            bool res = false;

            try
            {
                if (CheckDataBaseFile())
                {
                    AccessDB db = new AccessDB(Constants.ConnectionString);
                    string sql = "select count(*) from tblUsers";

                    if(db.ExcuteQuery(sql))
                    {
                        res = true;
                    }


                    db.CloseConnection();
                    
                }
            }
            catch
            {

            }
            return res;
        }

        internal static bool InstallDB()
        {
            bool res = false;

            return res;
        }

        internal static bool Backup(string path)
        {
            bool res = false;

            return res;
        }

        internal static bool RestoreDB(string path)
        {
            bool res = false;

            return res;
        }
    }
}
