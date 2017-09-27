using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Data;
using System.Xml.Serialization;
namespace Scrapy
{
    class web_request
    {
        public void download_dce()
        {
            foreach (DateTime dt in lst_request_date_)
            {
                try
                {
                    int y = dt.Year;
                    int m = dt.Month;
                    int d = dt.Day;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.dce.com.cn/publicweb/quotesdata/exportDayQuotesChData.html");
                    request.Referer = "http://www.dce.com.cn";
                    request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
                    request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
                    request.Headers["Cache-Control"] = "no-cache";
                    request.KeepAlive = true;
                    //上面的http头看情况而定，但是下面俩必须加  
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";

                    string postString = "dayQuotes.variety=all&dayQuotes.trade_type=0&year=";
                    postString += string.Format("{0:D4}", y) + "&month=";
                    postString += string.Format("{0}", m - 1) + "&day=";
                    postString += string.Format("{0:D2}", d);
                    postString += "&exportFlag=txt";//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
                    byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式
                    request.ContentLength = postData.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postData, 0, postData.Length);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    string retString = streamReader.ReadToEnd();
                    string file = string.Format("data/mds.dce.{0:D4}.{1:D2}.{2:D2}.dat", y, m, d);
                    if (retString.Length > 1000)
                    {
                        StreamWriter streamWriter = new StreamWriter(file, false, Encoding.UTF8);
                        streamWriter.Write(retString);
                        streamWriter.Close();
                    }
                    streamReader.Close();
                    responseStream.Close();
                    response.Close();

                    string[] split = retString.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    int rowcount = 0;
                    string datetime = string.Format("{0:D4}/{1:D2}/{2:D2} 00:00:00", y, m, d);
                    StreamWriter csv = new StreamWriter("dce.csv", true, Encoding.Default);
                    foreach (string line in split)
                    {
                        if (++rowcount > 1)
                        {
                            string[] items = line.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                            if (items[0].Trim().IndexOf("小计") == -1 && items[0].Trim().IndexOf("总计") == -1)
                            {
				                if (items[0].Trim()=="豆一") 
                                    items[0]="a";
                                else if (items[0].Trim() == "豆二")
                                    items[0]="b";
                                else if (items[0].Trim() == "胶合板")
                                    items[0]="bb";
                                else if (items[0].Trim() == "玉米")
                                    items[0]="c";
                                else if (items[0].Trim() == "玉米淀粉")
                                    items[0]="cs";
                                else if (items[0].Trim() == "纤维板")
                                    items[0]="fb";
                                else if (items[0].Trim() == "铁矿石")
                                    items[0]="i";
                                else if (items[0].Trim() == "焦炭")
                                    items[0]="j";
                                else if (items[0].Trim() == "鸡蛋")
                                    items[0]="jd";
                                else if (items[0].Trim() == "焦煤")
                                    items[0]="jm";
                                else if (items[0].Trim() == "聚乙烯")
                                    items[0]="l";
                                else if (items[0].Trim() == "豆粕")
                                    items[0]="m";
                                else if (items[0].Trim() == "棕榈油")
                                    items[0]="p";
                                else if (items[0].Trim() == "聚丙烯")
                                    items[0]="pp";
                                else if (items[0].Trim() == "聚氯乙烯")
                                    items[0]="v";
                                else if (items[0].Trim() == "豆油")
                                    items[0]="y";

                                Bar bar = new Bar();
                                bar.symbol = "dce_" + items[0] + items[1].Trim();
                                bar.time = datetime;
                                bar.open = Double.Parse(items[2].Trim().Replace(",", ""));
                                bar.high = Double.Parse(items[3].Trim().Replace(",", ""));
                                bar.low = Double.Parse(items[4].Trim().Replace(",", ""));
                                bar.close = Double.Parse(items[5].Trim().Replace(",", ""));
                                bar.volume = Double.Parse(items[10].Trim().Replace(",", ""));
                                bar.posistion = Double.Parse(items[11].Trim().Replace(",", ""));
                                bar.check_price();
                                if (bar.valid())
                                    csv.WriteLine(bar.to_string());
                            }
                        }
                    }
                    csv.Close();

                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(0, ex.Message);
                }
            }
            lst_request_date_.Clear();
            if (handler_ != null)
                handler_.status_changed(1, "finished...");
        }
        public void download_czce()
        {
            foreach (DateTime dt in lst_request_date_)
            {
                try
                {
                    int y = dt.Year;
                    int m = dt.Month;
                    int d = dt.Day;
                    string url = "http://www.czce.com.cn/portal/DFSStaticFiles/Future/" + string.Format("{0:D4}/", y) + string.Format("{0:D4}{1:D2}{2:D2}", y, m, d) + "/FutureDataDaily.txt";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Referer = "http://www.czce.com.cn";
                    request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
                    request.Headers["Accept-Language"] = "zh-CN";
                    request.Headers["Accept-Encoding"] = "gzip,deflate";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
                    request.KeepAlive = true;
                    request.Method = "GET";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
                    string retString = streamReader.ReadToEnd();
                    string file = string.Format("data/mds.czce.{0:D4}.{1:D2}.{2:D2}.dat", y, m, d);
                    if (retString.Length > 1000)
                    {
                        StreamWriter streamWriter = new StreamWriter(file, false, Encoding.Default);
                        streamWriter.Write(retString);
                        streamWriter.Close();
                    }
                    streamReader.Close();
                    responseStream.Close();
                    response.Close();

                    string[] split = retString.Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
                    int rowcount = 0;
                    string datetime = string.Format("{0:D4}/{1:D2}/{2:D2} 00:00:00", y, m, d);
                    StreamWriter csv = new StreamWriter("czce.csv", true, Encoding.Default);
                    foreach (string line in split)
                    {
                        if (++rowcount > 2)
                        {
                            string[] items = line.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                            if (items[0].Trim().IndexOf("小计") == -1 && items[0].Trim().IndexOf("总计") == -1)
                            {
                                Bar bar = new Bar();
                                bar.symbol = "czce_" + items[0].Trim();
                                bar.time = datetime;
                                bar.open = Double.Parse(items[2].Trim().Replace(",", ""));
                                bar.high = Double.Parse(items[3].Trim().Replace(",", ""));
                                bar.low = Double.Parse(items[4].Trim().Replace(",", ""));
                                bar.close = Double.Parse(items[5].Trim().Replace(",", ""));
                                bar.volume = Double.Parse(items[9].Trim().Replace(",", ""));
                                bar.posistion = Double.Parse(items[10].Trim().Replace(",", ""));
                                bar.check_price();
                                if (bar.valid())
                                    csv.WriteLine(bar.to_string());
                            }
                        }
                    }
                    csv.Close();
                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(0, ex.Message);
                }
            }
            lst_request_date_.Clear();
            if (handler_ != null)
                handler_.status_changed(1, "finished..."); 
        }
        public void download_shfe()
        {
            foreach (DateTime dt in lst_request_date_)
            {
                try
                {
                    int y = dt.Year;
                    int m = dt.Month;
                    int d = dt.Day;
                    string url = "http://www.shfe.com.cn/data/dailydata/kx/kx" + string.Format("{0:D4}{1:D2}{2:D2}", y, m, d) + ".dat";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Referer = "http://www.shfe.com.cn/statements/dataview.html?paramid=kx";
                    request.Accept = "*/*";
                    request.Headers["Accept-Language"] = "zh-CN";
                    request.Headers["Accept-Encoding"] = "gzip,deflate";
                    request.Headers["Cache-Control"] = "no-cache";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
                    request.KeepAlive = true;
                    request.Method = "GET";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    string retString = streamReader.ReadToEnd();
                    string file = string.Format("data/mds.shfe.{0:D4}.{1:D2}.{2:D2}.dat", y, m, d);
                    if (retString.Length > 1000)
                    {
                        StreamWriter streamWriter = new StreamWriter(file, false, Encoding.UTF8);
                        streamWriter.Write(retString);
                        streamWriter.Close();
                    }
                    streamReader.Close();
                    responseStream.Close();
                    response.Close();

                    retString = retString.Replace("\"PRESETTLEMENTPRICE\":\"\"", "\"PRESETTLEMENTPRICE\":0");
                    retString = retString.Replace("\"OPENPRICE\":\"\"", "\"OPENPRICE\":0");
                    retString = retString.Replace("\"HIGHESTPRICE\":\"\"", "\"HIGHESTPRICE\":0");
                    retString = retString.Replace("\"LOWESTPRICE\":\"\"", "\"LOWESTPRICE\":0");
                    retString = retString.Replace("\"CLOSEPRICE\":\"\"", "\"CLOSEPRICE\":0");
                    retString = retString.Replace("\"SETTLEMENTPRICE\":\"\"", "\"SETTLEMENTPRICE\":0");
                    retString = retString.Replace("\"ZD1_CHG\":\"\"", "\"ZD1_CHG\":0");
                    retString = retString.Replace("\"ZD2_CHG\":\"\"", "\"ZD2_CHG\":0");
                    retString = retString.Replace("\"OPENINTEREST\":\"\"", "\"OPENINTEREST\":0");
                    retString = retString.Replace("\"OPENINTERESTCHG\":\"\"", "\"OPENINTERESTCHG\":0");
                    retString = retString.Replace("\"AVGPRICE\":\"\"", "\"AVGPRICE\":0");
                    int index = retString.IndexOf("o_curproduct");
                    retString = retString.Substring(0, index - 2);
                    retString += "}";
                    DataSet dataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<DataSet>(retString);
                    DataTable dataTable = dataSet.Tables["o_curinstrument"];
                    StreamWriter csv = new StreamWriter("shfe.csv", true, Encoding.UTF8);
                    string datetime = string.Format("{0:D4}/{1:D2}/{2:D2} 00:00:00", y, m, d);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string product = row["PRODUCTID"].ToString();
                        int indexp = product.IndexOf("_");
                        if (indexp != -1 && row["DELIVERYMONTH"].ToString().IndexOf("小计") == -1)
                        {
                            Bar bar = new Bar();
                            bar.symbol = "shfe_" + product.Substring(0, indexp) + row["DELIVERYMONTH"].ToString();
                            bar.time = datetime;
                            bar.open = Double.Parse(row["OPENPRICE"].ToString().Trim());
                            bar.high = Double.Parse(row["HIGHESTPRICE"].ToString().Trim());
                            bar.low = Double.Parse(row["LOWESTPRICE"].ToString().Trim());
                            bar.close = Double.Parse(row["CLOSEPRICE"].ToString().Trim());
                            bar.volume = Double.Parse(row["VOLUME"].ToString().Trim());
                            bar.posistion = Double.Parse(row["OPENINTEREST"].ToString().Trim());
                            bar.check_price();
                            if (bar.valid())
                                csv.WriteLine(bar.to_string());
                        }
                        else
                        {
                            Console.WriteLine(product);
                        }
                    }
                    csv.Close();
                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(0, ex.Message);
                }
            }
            lst_request_date_.Clear();
            if (handler_ != null)
                handler_.status_changed(1, "finished...");
        }
        public void download_cffex()
        {
            foreach (DateTime dt in lst_request_date_)
            {
                try
                {
                    int y = dt.Year;
                    int m = dt.Month;
                    int d = dt.Day;
                    string url = "http://www.cffex.com.cn/sj/hqsj/rtj/" + string.Format("{0:D4}{1:D2}/{2:D2}", y, m, d) + "/index.xml";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Referer = "www.cffex.com.cn";
                    request.Accept = "text/html, application/xhtml+xml, image/jxr, */*";
                    request.Headers["Accept-Language"] = "zh-CN";
                    request.Headers["Accept-Encoding"] = "gzip,deflate";
                    request.Headers["Cache-Control"] = "no-cache";
                    request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
                    request.KeepAlive = true;
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
                    if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
                    {
                        responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                    }

                    StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    string retString = streamReader.ReadToEnd();
                    string file = string.Format("data/mds.cffex.{0:D4}.{1:D2}.{2:D2}.dat", y, m, d);
                    if (retString.Length > 1000)
                    {
                        StreamWriter streamWriter = new StreamWriter(file, false, Encoding.UTF8);
                        streamWriter.Write(retString);
                        streamWriter.Close();
                    }
                    streamReader.Close();
                    responseStream.Close();
                    response.Close();

                    XmlSerializer serializer = new XmlSerializer(typeof(dailydataCollection));
                    dailydataCollection colection = (dailydataCollection)serializer.Deserialize(new StringReader(retString));
                    //serializer.Serialize(Console.Out, colection);
                    string datetime = string.Format("{0:D4}/{1:D2}/{2:D2} 00:00:00", y, m, d);
                    StreamWriter csv = new StreamWriter(string.Format("cffex.csv", y, m, d), true, Encoding.UTF8);
                    for (int i = 0; i < colection.dailydatas.Length; i++)
                    {
                        Bar bar = new Bar();
                        bar.symbol = "cffex_" + colection.dailydatas[i].instrumentid.Trim();
                        bar.time = datetime;
                        bar.open = Double.Parse(colection.dailydatas[i].openprice.Trim() == "" ? "0" : colection.dailydatas[i].openprice.Trim());
                        bar.high = Double.Parse(colection.dailydatas[i].highestprice.Trim() == "" ? "0" : colection.dailydatas[i].highestprice.Trim());
                        bar.low = Double.Parse(colection.dailydatas[i].lowestprice.Trim() == "" ? "0" : colection.dailydatas[i].lowestprice.Trim());
                        bar.close = Double.Parse(colection.dailydatas[i].closeprice.Trim() == "" ? "0" : colection.dailydatas[i].closeprice.Trim());
                        bar.volume = Double.Parse(colection.dailydatas[i].volume.Trim() == "" ? "0" : colection.dailydatas[i].volume.Trim());
                        bar.posistion = Double.Parse(colection.dailydatas[i].openinterest.Trim() == "" ? "0" : colection.dailydatas[i].openinterest.Trim());
                        bar.check_price();
                        if (bar.valid())
                            csv.WriteLine(bar.to_string());
                    }
                    csv.Close();
                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.InvalidOperationException ioe)
                {
                    if (handler_ != null)
                        handler_.status_changed(0, ioe.Message);
                }
            }
            lst_request_date_.Clear();
            if (handler_ != null)
                handler_.status_changed(1, "finished...");
        }

        public string download_json(string host, string refere, string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Host = host;
            request.Referer = refere;
            request.Accept = "application/json, text/plain, */*";
            request.Headers["Accept-Language"] = "zh-CN";
            request.Headers["Accept-Encoding"] = "gzip, deflate, br";
            request.Headers["Cache-Control"] = "no-cache";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063";
            request.KeepAlive = true;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            //如果http头中接受gzip的话，这里就要判断是否为有压缩，有的话，直接解压缩即可  
            if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip"))
            {
                responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
            }

            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
            string retString = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            response.Close();

            return retString;
        }

        public List<DateTime> lst_request_date_ = new List<DateTime>();
        public Main handler_ = null;
    }
}
