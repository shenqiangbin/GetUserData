using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetUserData
{
    class Program
    {
        static void Main(string[] args)
        {
            //bool result = RegexHelper.IsTel("152011069262");
            new HandleTxt().Exe();
            //for (int i = 341; i < 32759; i++)
            //{
            //    try
            //    {
            //        Console.WriteLine("page" + i);
            //        string sql = "Sqlstr=select+*+from+worker&page=";
            //        sql = "Sqlstr=select+*+from+patient&page=";
            //        GetPage(sql, i);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("error");
            //        File.AppendAllText("c:/info.txt", ex.Message + ex.StackTrace);
            //    }
            //}
        }

        //Array arr = serializer.Deserialize(new JsonTextReader(sr)) as Array;


        //var list = JsonHelper.DeserializeJsonToList<Model>(contentResult);



        static void GetPage(string sql, int page)
        {
            string content = sql + page;
            string url = "http://tcrm.qiezzi.com/CRMManage/SysConfig/GetResult";

            string contentResult = NetHelper.Post(url, content);

            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(contentResult);
            var obj = serializer.Deserialize(new JsonTextReader(sr));

            var objItem = ((Newtonsoft.Json.Linq.JContainer)obj)[0];

            foreach (var item in objItem)
            {
                var s = item.ToString();
                var list = JsonHelper.DeserializeJsonToList<Model>(s);

                //string workername = list.First(m => m.Key == "WorkerName").Value;
                //string tel = list.First(m => m.Key == "Tel").Value;

                string workername = list.First(m => m.Key == "PatientName").Value;
                string tel = list.First(m => m.Key == "Tel").Value??"";
                string phone = list.First(m => m.Key == "Phone").Value??"";
                tel = tel + "," + phone;

                string path = "c:/info3.txt";
                if (!File.Exists(path))
                    File.Create(path);

                File.AppendAllText(path, string.Format("{0},{1}\r\n", workername, tel));
            }
        }
    }



    public class Model
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
