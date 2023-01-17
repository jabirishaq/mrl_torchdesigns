using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace TorchDesigns.DynamicsGP
{
    public class gpLog
    {
        string _strConnectionString;

        public string ConnectionString
        {
            get
            {
                return _strConnectionString;
            }
            set
            {
                _strConnectionString = value;
            }
        }

        public void WriteLog(Exception ex)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    //string strUpdateLogCommand = "INSERT INTO gp_Log (Message) VALUES('" + ex.Message.Trim() + "')";
                    string strUpdateLogCommand = "INSERT INTO gp_Log (Message, StackTrace) VALUES('" + ex.Message.Replace("'","\'")
                                               + "','" + ex.StackTrace.Trim('\'') + "')";
                    string strDeleteCommand = "DELETE FROM gp_Log WHERE gpLogID < (SELECT MAX(gpLogID) FROM gp_Log) + 1000";
                    SqlCommand UpdateLogCommand = conn.CreateCommand();
                    SqlCommand DeleteLogCommand = conn.CreateCommand();
                    UpdateLogCommand.CommandText = strUpdateLogCommand;
                    DeleteLogCommand.CommandText = strDeleteCommand;

                    conn.Open();
                    UpdateLogCommand.ExecuteNonQuery();
                    DeleteLogCommand.ExecuteNonQuery();
                }
                catch (Exception exc)
                {
                    Exception e = exc;
                    string something = exc.Message;
                }
            }

        }
    }
}
