using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapy
{
    public class Instrument
    {
        public string PRODUCTID { get; set; }
        public string PRODUCTNAME { get; set; }
        public string DELIVERYMONTH { get; set; }
        public string OPENPRICE { get; set; }
        public string HIGHESTPRICE { get; set; }
        public string LOWESTPRICE { get; set; }
        public string CLOSEPRICE { get; set; }
        public string VOLUME { get; set; }
        public string OPENINTEREST { get; set; }
    }
}
