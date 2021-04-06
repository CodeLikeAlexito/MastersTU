using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LabOne
{
    public class SqlManager
    {
        IDbConnection connection;
        private Int32 LastSTatementId = -1;

        public SqlManager()
        {
            connection = new SqlConnection(Properties.Settings.Default.DbConnect);
            //GetAllPayments();
        }

        // 1) print only payment number
        // 2) print from entered date
        public List<Payment> GetAllPayments(string selectFromDate, DateTime date)
        {
            List<Payment> payments = new List<Payment>();
            string sqlquery;

            if (String.IsNullOrEmpty(selectFromDate) || String.IsNullOrWhiteSpace(selectFromDate))
            {
                sqlquery = @"SELECT * FROM payment ";
            }
            else
            {
                sqlquery = @"select * from [dbo].[payment] where payment_dt > " + date.ToString("yyyy-MM-dd");
            }
            
            IDbCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();
            command.CommandText = sqlquery;
            IDataReader reader = command.ExecuteReader();
            bool notEndOfResult;
            notEndOfResult = reader.Read();
            while (notEndOfResult)
            {
                Payment payment = new Payment()
                {
                    PaymentNumber = Convert.ToInt32(reader["payment_no"]),
                    MemberNo = Convert.ToInt32(reader["member_no"]),
                    PaymentDate = Convert.ToDateTime(reader["payment_dt"]),
                    PaymentAmnt = Convert.ToDecimal(reader["payment_amt"]),
                    StatementNo = Convert.ToInt32(reader["statement_no"]),
                    PaymentCode = reader["payment_code"].ToString()
                };

                payments.Add(payment);
                notEndOfResult = reader.Read();
            }
            connection.Close();
            return payments;
        }


        public List<Statement> GetAllStatements()
        {
            List<Statement> statements = new List<Statement>();
            string sqlquery = @"SELECT * FROM statement "; ;

            IDbCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();
            command.CommandText = sqlquery;
            IDataReader reader = command.ExecuteReader();
            bool notEndOfResult;
            notEndOfResult = reader.Read();
            while (notEndOfResult)
            {
                Statement statement = new Statement()
                {
                    StatementNo = Convert.ToInt32(reader["statement_no"]),  
                    MemberNo = Convert.ToInt32(reader["member_no"]),
                    StatementDate = Convert.ToDateTime(reader["statement_dt"]),
                    DueDate = Convert.ToDateTime(reader["due_dt"]),
                    StatementAmnt = Convert.ToDecimal(reader["statement_amt"]),
                    StatementCode = reader["statement_code"].ToString()
                };

                statements.Add(statement);
                notEndOfResult = reader.Read();
            }
            connection.Close();
            return statements;
        }

        public void AddNewTransaction()
        {
            string sqlquery =
                                @"INSERT INTO statement
                                ([member_no]
                                ,[statement_dt]
                                ,[due_dt]
                                ,[statement_amt]
                                ,[statement_code])
                                VALUES
                                (10001
                                ,CURRENT_TIMESTAMP
                                ,CURRENT_TIMESTAMP
                                ,1500.00
                                ,0)";

            IDbCommand command = new SqlCommand(sqlquery);
            command.Connection = connection;
            connection.Open();
            command.ExecuteNonQuery();
            command.CommandText = "SELECT MAX(statement.statement_no) FROM statement; ";
            IDataReader reader = command.ExecuteReader();
            reader.Read();
            LastSTatementId = reader.GetInt32(0);
        }

        public void PaymentWithoutTransaction()
        {
            if (LastSTatementId < 0)
                return;
            int ErrorVar = 0;
            Double PaySum = 500.0;
            Double PayedTotal = 0.0;
            Double ToBePayed = 0.0;
            string FirstQuery =
                                @"INSERT INTO payment
                                ([member_no]
                                ,[payment_dt]
                                ,[payment_amt]
                                ,[statement_no]
                                ,[payment_code])
                                VALUES
                                (10001
                                ,CURRENT_TIMESTAMP
                                ," + PaySum + " ," + LastSTatementId + ",0)";
            string SecondQuery =
                                @"SELECT SUM(payment.payment_amt)
                                FROM payment
                                WHERE payment.statement_no = " + LastSTatementId;
            int Code = 0;
            string ThirdQuery =
                                @"SELECT statement.statement_amt
                                FROM statement
                                WHERE statement.statement_no = " + LastSTatementId;
            string FourthQuery =
                                @"UPDATE statement
                                SET statement_code = {0}" +
                                "WHERE statement_no = " + LastSTatementId;

            IDbCommand command = new SqlCommand(FirstQuery);
            command.Connection = connection;
            connection.Open();
            try
            {
                command.ExecuteNonQuery();
                int a = 10 / ErrorVar; // Генерираме Exeption
                command.CommandText = SecondQuery;
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                PayedTotal = reader.GetDouble(0);
                command.CommandText = ThirdQuery;
                reader = command.ExecuteReader();
                reader.Read();
                ToBePayed = reader.GetDouble(0);
                if (PayedTotal == 0.0)
                { Code = 0; }
                else if (PayedTotal < ToBePayed)
                { Code = 1; }
                else { Code = 2; }
                command.CommandText = String.Format(FourthQuery, Code);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Грешка без rollback");
            }
        }

        public void TransactionPayment()
        {
            Double PaySum = 500.0;
            string FirstQuery =
                                @"INSERT INTO payment
                                ([member_no]
                                ,[payment_dt]
                                ,[payment_amt]
                                ,[statement_no]
                                ,[payment_code])
                                VALUES
                                (10001
                                ,CURRENT_TIMESTAMP
                                ," + PaySum + " ," + LastSTatementId + ",0)";

            IDbCommand command = new SqlCommand(FirstQuery);
            command.Connection = connection;
            connection.Open();

            try
            {
                IDbTransaction transact;
                transact = connection.BeginTransaction();
                command.Transaction = transact;

                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                command.Transaction.Rollback();
                Console.WriteLine(ex.Message, "Грешка, но пък правим rollback");
            }
        }
    }
}
