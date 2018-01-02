using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapy
{
    struct commition
    {
        public string exchangeid{ get; set; }
        public string instrumentid{ get; set; }
        public string openratiobymoney{ get; set; }
        public string openratiobyvolume{ get; set; }
        public string closeratiobymoney{ get; set; }
        public string closeratiobyvolume{ get; set; }
        public string closetodayratiobymoney{ get; set; }
        public string closetodayratiobyvolume{ get; set; }
    }
    class dhqh_commition
    {
        public int run()
        {
            web_request wreq = new web_request();
            string json = wreq.download_json("dhjr.kiiik.com", "https://dhjr.kiiik.com/html/table.html?from=singlemessage&isappinstalled=1",
                "https://dhjr.kiiik.com/public/getBmsCurrexchcommrateInfo.do");
            if (json.Length > 0)
            {
                dhqh_commition commis = JObject.Parse(json).ToObject<dhqh_commition>();
                List<string> list = new List<string>();
                foreach (commition comm in commis.bmsCurrexchcommrateInfo)
                {
                    string line = comm.instrumentid + ",";
                    if (Double.Parse(comm.openratiobyvolume) > 0.1)
                        line += comm.openratiobyvolume + ",";
                    else
                        line += comm.openratiobymoney + ",";
                    if (Double.Parse(comm.closeratiobyvolume) > 0.1)
                        line += comm.closeratiobyvolume + ",";
                    else
                        line += comm.closeratiobymoney + ",";
                    if (Double.Parse(comm.closetodayratiobyvolume) > 0.1)
                        line += comm.closetodayratiobyvolume + ",";
                    else
                        line += comm.closetodayratiobymoney + ",";
                    list.Add(line);
                }
                string file_name = "./all_symbol_commission.csv";
                using (StreamWriter sw = new StreamWriter(file_name))
                {
                    sw.WriteLine("品种,开仓,平仓,平今仓");
                    foreach (string line in list)
                        sw.WriteLine(line);
                }
                // 备份
                if (File.Exists(file_name))
                {
                    string folder = "./commission";
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string new_name = folder + "/" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                    File.Copy(file_name, new_name);
                }
            }

            return 0;
        }
        public string resultStatus { get; set; }
        public List<commition> bmsCurrexchcommrateInfo;
        public string returnTime { get; set; }
    }
}
