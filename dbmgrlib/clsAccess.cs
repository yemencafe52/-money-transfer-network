using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace dbmgrlib
{
    public class AccessDB
    {
        private OleDbConnection con;
        private OleDbCommand cmd;
        private OleDbDataReader dr;
        private string connectionString = "";

        public AccessDB(string cn)
        {
            this.connectionString = cn;
        }

        public int ExcuteNonQuery(string sql)
        {
            int res = 0;

            try
            {
                this.con = new OleDbConnection(this.connectionString);
                this.con.Open();
                this.cmd = new OleDbCommand(sql, this.con);
                res = this.cmd.ExecuteNonQuery();
                this.con.Close();
            }
            catch
            {

            }

            return res;
        }

        public bool ExcuteQuery(string sql)
        {
            bool res = false;

            try
            {
                this.con = new OleDbConnection(this.connectionString);
                this.con.Open();
                this.cmd = new OleDbCommand(sql, this.con);
                dr = this.cmd.ExecuteReader();
                
                if(dr.HasRows)
                {
                    res = true;
                }

            }
            catch
            {

            }

            return res;
        }

        public OleDbDataReader GetDataReader
        {
            get
            {
                return this.dr;
            }
        }

        public void CloseConnection()
        {
            if(this.con != null)
            {
                if(this.con.State != System.Data.ConnectionState.Closed)
                {
                    try
                    {
                        this.con.Close();
                    }
                    catch
                    {

                    }
                }
            }

            GC.Collect();
        }

        ~AccessDB()
        {
            CloseConnection();
        }

    }
}
