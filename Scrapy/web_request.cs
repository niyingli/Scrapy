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
                    string out_file = string.Format("data/mds.dce.{0:D4}.{1:D2}.{2:D2}.dat", y, m, d);
                    if (retString.Length > 1000)
                    {
                        StreamWriter streamWriter = new StreamWriter(out_file, false, Encoding.UTF8);
                        streamWriter.Write(retString);
                        streamWriter.Close();
                    }
                    streamReader.Close();
                    responseStream.Close();

                    if (handler_ != null)
                        handler_.status_changed(0, out_file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(-1, ex.Message);
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

                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(-1, ex.Message);
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
                    StreamWriter outok = new StreamWriter(string.Format("data/mds.shfe.{0:D4}.{1:D2}.{2:D2}.csv", y, m, d), false, Encoding.UTF8);
                    string line="";
                    string datetime = string.Format("{0:D4}/{1:D2}/{2:D2} 00:00:00", y, m, d);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string product = row["PRODUCTID"].ToString();
                        int indexp = product.IndexOf("_");
                        if (indexp != -1)
                        {
                            line = "shfe_" + product.Substring(0, indexp) +
                                row["DELIVERYMONTH"].ToString() + "," +
                                datetime + "," +
                                row["OPENPRICE"].ToString() + "," +
                                row["HIGHESTPRICE"].ToString() + "," +
                                row["LOWESTPRICE"].ToString() + "," +
                                row["CLOSEPRICE"].ToString() + "," +
                                row["VOLUME"].ToString() + "," +
                                row["OPENINTEREST"].ToString() + "," +
                                "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
                            if (row["DELIVERYMONTH"].ToString() != "小计")
                            {
                                outok.WriteLine(line);
                            }
                        }
                        else
                        {
                            Console.WriteLine(product);
                        }
                    }

                    outok.Close();
                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(-1, ex.Message);
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

                    if (handler_ != null)
                        handler_.status_changed(0, file);

                    Thread.Sleep(1000);
                }
                catch (System.Net.WebException ex)
                {
                    if (handler_ != null)
                        handler_.status_changed(-1, ex.Message);
                }
            }
            lst_request_date_.Clear();
            if (handler_ != null)
                handler_.status_changed(1, "finished...");
        }


        public List<DateTime> lst_request_date_ = new List<DateTime>();
        public Main handler_ = null;
    }
}
