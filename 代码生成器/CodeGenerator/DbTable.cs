using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DbTable
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<DbTableColumn> columns { get; set; }
    }
}
