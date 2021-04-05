using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LabOne
{
    public class Payment
    {

        public List<int> PaymentNums { get; set; }

        public Payment()
        {
            PaymentNums = new List<int>();
            //FillPaymentNums();
            ReadFromDate(DateTime.Now);
        }

        private void FillPaymentNums()
        {
            using (IDbConnection connection = new SqlConnection(Properties.Settings.Default.DbConnect))
            {
                string sqlquery =
                @"SELECT payment_no FROM payment ";
                IDbCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = sqlquery;
                IDataReader reader = command.ExecuteReader();
                bool notEndOfResult;
                notEndOfResult = reader.Read();
                while (notEndOfResult)
                {
                    int s = reader.GetInt32(0);//reader.GetString(0);
                    PaymentNums.Add(s);
                    notEndOfResult = reader.Read();
                }
            }
        }

        // Read rows based on eneterd date
        private void ReadFromDate(DateTime date)
        {
            using (IDbConnection connection = new SqlConnection(Properties.Settings.Default.DbConnect))
            {
                string sqlquery = @"select * from [dbo].[payment] where payment_dt > " + date;
                IDbCommand command = new SqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = sqlquery;
                IDataReader reader = command.ExecuteReader();
                bool notEndOfResult;
                notEndOfResult = reader.Read();
                while (notEndOfResult)
                {
                    int s = reader.GetInt32(0);//reader.GetString(0);
                    PaymentNums.Add(s);
                    notEndOfResult = reader.Read();
                }
            }
        }

    }
}
