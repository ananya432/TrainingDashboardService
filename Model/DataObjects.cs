using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBIService.Model
{
    public class DataObjects
    {
        public string _id { get; set; }
        public string TXNID { get; set; }
        public int TXNAMOUNT { get; set; }
        public string ORDERID { get; set; }
        public List<Items> items { get; set; }
        public string userId { get; set; }
        public string userEmail {  get; set; }
        public string userPhone { get; set; }
        public DateTime createdDate { get; set; }
        public string status { get; set; }
        public string channel { get; set; }

    }
}
