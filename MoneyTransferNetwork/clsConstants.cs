using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoneyTransferSystem
{
    internal static class Constants
    {
        private static string dbPath = Application.StartupPath + "\\Database\\db.accdb";
        private static string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dbPath + ";Persist Security Info=False;ole db services=-1";

        internal static string DbPath
        {
            get
            {
                return dbPath;
            }
        }

        internal static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
    }
}
