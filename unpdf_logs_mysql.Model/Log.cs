using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unpdf_logs_mysql.Model
{
    public class Logs
    {
        public long Id { get; set; }
        public long Doc_id { get; set; }
        public string? User { get; set; }
        public string? Description { get; set; }
    }
}