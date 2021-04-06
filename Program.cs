using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace LabOne
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            DateTime dt;
            string selectFromDate;
            SqlManager db = new SqlManager();

            while (true)
            {
                try
                {
                    Console.WriteLine("Would you like to select from specific date? Yes, No");
                    Console.WriteLine("To see all statements type 1..");
                    Console.WriteLine("Or would you like to exit the program? type exit..");
                    selectFromDate = Console.ReadLine();
                    //selectFromDate.ToLower();

                    if(selectFromDate.ToLower() == "yes")
                    {
                        Console.WriteLine("Enter from what date you want to get results, date format: yyyy-MM-dd:");
                        string enterDateString = Console.ReadLine();
                        dt = DateTime.ParseExact(enterDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var listPayments = db.GetAllPayments(selectFromDate, dt);
                        foreach (var item in listPayments)
                        {
                            Console.WriteLine(@"{0} --- {1} --- {2} --- {3} --- {4} --- {5},",
                                                item.PaymentNumber, item.MemberNo, item.PaymentDate,
                                                item.PaymentAmnt, item.StatementNo, item.PaymentCode);
                        }
                    }
                    else if (selectFromDate.ToLower() == "no")
                    {
                        Console.WriteLine("Printing all data in table:");
                        var listPayments = db.GetAllPayments(selectFromDate, DateTime.MinValue);
                        foreach (var item in listPayments)
                        {
                            Console.WriteLine(@"{0} --- {1} --- {2} --- {3} --- {4} --- {5},",
                                                item.PaymentNumber, item.MemberNo, item.PaymentDate,
                                                item.PaymentAmnt, item.StatementNo, item.PaymentCode);
                        }
                    }
                    else if(selectFromDate.ToLower() == "1")
                    {
                        Console.WriteLine("Printing all statements from table:");
                        var listStatemetns = db.GetAllStatements();
                        foreach (var item in listStatemetns)
                        {
                            Console.WriteLine(@"{0} --- {1} --- {2} --- {3} --- {4} --- {5},",
                                                item.StatementNo, item.MemberNo, item.StatementDate,
                                                item.DueDate, item.StatementAmnt, item.StatementCode);
                        }
                    }
                    else if (selectFromDate.ToLower() == "exit")
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        throw new InvalidInputException("You need to enter yes/no.. Invalid input");
                    }
                    Console.ReadKey();
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("Not valid date format...");
                }
                catch (InvalidInputException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
            
            
            
        }

        private static Exception InvalidInputException(string v)
        {
            throw new NotImplementedException();
        }
    }
}
