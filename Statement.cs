using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabOne
{
    public class Statement
    {
        public int? StatementNo { get; set; }
        public int? MemberNo { get; set; }
        public DateTime? StatementDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? StatementAmnt { get; set; }
        public string StatementCode { get; set; }
    }
}
