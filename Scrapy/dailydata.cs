using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Scrapy
{
    [XmlRoot("dailydata")]
    public class dailydata
    {
        public string instrumentid { get; set; }
        public string tradingday { get; set; }
        public string openprice { get; set; }
        public string highestprice { get; set; }
        public string lowestprice { get; set; }
        public string closeprice { get; set; }
        public string openinterest { get; set; }
        public string presettlementprice { get; set; }
        public string settlementpriceIF { get; set; }
        public string settlementprice { get; set; }
        public string volume { get; set; }
        public string turnover { get; set; }
        public string productid { get; set; }
        public string delta { get; set; }
        public string segma { get; set; }
        public string expiredate { get; set; }
    }

    [XmlRoot("dailydatas")]
    public class dailydataCollection
    {
        [XmlElement("dailydata")]
        public dailydata[] dailydatas { get; set; }
    }
}
