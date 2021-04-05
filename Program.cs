using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace LabOne
{
    public class Program
    {
        static void Main(string[] args)
        {
            Payment payment = new Payment();
            foreach (var item in payment.PaymentNums)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
