using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GetUserData
{
    public class NetHelper
    {
        public static string Post(string url,string content)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                //获得用户名密码的Base64编码
                //string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, password)));
                //添加Authorization到HTTP头
                //request.Headers.Add("Authorization", "Basic " + code);
                //VFNJTkdIVUE6dHNpbmdodWFfbWRz
                //string body = xml;
                request.Headers.Add("Cookie", "UM_distinctid=160bc8a074a112-00a9b728f451af-5a442916-1fa400-160bc8a074b639; CNZZDATA1254157783=1598788597-1514991676-http%253A%252F%252Ftcrm.qiezzi.com%252F%7C1515078349; .ASPXAUTH=37D3F3478B8D007E6957A0BE186D1E3DEFF03A74629BCFFFFCC691D0FF78F548ED9276FEED188C5571D0E548BB92D142FCEDC53B2F274F9A69485B83DF833DAD6C3B0835CAEE6299B2FB6F4A090593D811175975606AA7FC72CFB0267560DC5D62755ABFAFC958CD782A93893E96126D1315B8DA2F53BCE6C3764D50B2589F87B4C261C9");

                byte[] bytes = Encoding.UTF8.GetBytes(content);
                request.GetRequestStream().Write(bytes, 0, bytes.Length);

                WebResponse response = request.GetResponse();
                var stream = response.GetResponseStream();
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    var resultContent = sr.ReadToEnd();
                    //if (content.Contains("OK"))
                    //return true;
                    return resultContent;
                }
                //return false;               
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                //Logger.Trace(string.Format("RegisterMetadata:{0},{1},{2} ", userName, password, url) + response.StatusCode.ToString() + "\r\n" + xml);
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();
                            //Logger.Trace(text);
                        }
                    }
                }
                //return false;
            }
            return "";
        }
    }
}
