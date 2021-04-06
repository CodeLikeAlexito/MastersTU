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
        public int? PaymentNumber { get; set; }
        public int? MemberNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmnt { get; set; }
        public int? StatementNo { get; set; }
        public string PaymentCode { get; set; }

    }
}
