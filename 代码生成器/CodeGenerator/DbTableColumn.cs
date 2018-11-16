using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DbTableColumn
    {
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }
        public string isNullAble { get; set; }
        public string isPrimary { get; set; }
        public string isKey { get; set; }
        public string description { get; set; }
        public string showName 
        {
            get
            {
                if (description == "")
                {
                    return name;
                }
                return description;
            }
        }
    }
}
