using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapy
{
    public class Bar
    {
        public Bar()
        {
            this.symbol = "";
            this.time = "";
            this.open = 0;
            this.high = 0;
            this.low = 0;
            this.close = 0;
            this.volume = 0;
            this.posistion = 0;
        }
        public string symbol { get; set; }
        public string time { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
        public double posistion { get; set; }

        public void check_price()
        {
            if (this.open == 0)
                this.open = this.close;
            if (this.high == 0)
                this.high = this.close;
            if (this.low == 0)
                this.low = this.close;

        }
        public string to_string()
        {
            return this.symbol + "," + this.time + "," + this.open + "," + this.high + "," + this.low + "," + this.close + "," + this.volume + "," + this.posistion +
                ",0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
        }
        public bool valid()
        {
            if (this.symbol == "")
                return false;
            if (this.close == 0)
                return false;

            return true;
        }
    }
}
