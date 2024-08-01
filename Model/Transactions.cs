using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIService.Model
{
    public class Transactions
    {
        public List<DataObjects> data {  get; set; }
        public int total { get; set; }
        public int limit { get; set; }
        public int skip { get; set; }
    }

}
