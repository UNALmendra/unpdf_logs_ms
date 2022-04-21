using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unpdf_logs_mysql.Data
{
    public class Mysqlconfig
    {
        public Mysqlconfig(string connectionString) => ConnectionString = connectionString;
        public string ConnectionString { get; set; }
    }
}
