using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetUserData.dao;
using System.IO;
using MySql.Data.MySqlClient;

namespace GetUserData
{
    public class HandleTxt
    {
        public void Exe()
        {
            InitTable();
            //HandleInfo2TextFile();
            HandleInfo3TextFile();
        }

        private void InitTable()
        {
            string sql = @"
CREATE TABLE IF NOT EXISTS `User` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` nvarchar(64) NULL,  
  `Tel` varchar(24) NULL,  
  `CreateTime` datetime NULL,    
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
";

            int effectRows = MySqlRepository.Execute(sql);
        }

        private HandleInfoTextFileResult HandleInfo2TextFile()
        {
            HandleInfoTextFileResult result = new HandleInfoTextFileResult();
            result.Success = true;

            try
            {
                TryHandle(result);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message + ex.StackTrace;
            }

            return result;
        }

        private void TryHandle(HandleInfoTextFileResult result)
        {
            string content = File.ReadAllText("C:/info2.txt");
            if (string.IsNullOrEmpty(content))
            {
                result.Success = false;
                result.Msg = "没有内容";
            }

            string[] records = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            result.TotalRecords = records.Count();

            foreach (var record in records)
            {
                HandRecord(record);
            }
        }

        private void HandRecord(string record)
        {
            string[] userInfos = record.Split(new char[] { ',' });

            if (userInfos.Count() == 0)
                return;

            string userName = "";
            string tel = "";

            if (userInfos.Count() == 1)
            {
                string str = userInfos[0];
                if (RegexHelper.IsTel(str))
                    tel = str;
                else
                    userName = str;
            }

            if (userInfos.Count() > 1)
            {
                userName = userInfos[0];
                tel = userInfos[1];
            }

            if (RegexHelper.IsTel(tel))
                InsertToDb(userName, tel);
        }

        private void InsertToDb(string userName, string tel)
        {
            string sql = "insert user(UserName,Tel,CreateTime) values(@UserName,@Tel,@CreateTime)";

            Array arr = new[] {
                new MySqlParameter("UserName", userName),
                new MySqlParameter("Tel", tel),
                new MySqlParameter("CreateTime", DateTime.Now.ToString())
            };

            int effectRows = MySqlRepository.Execute(sql, arr);
        }

        private HandleInfoTextFileResult HandleInfo3TextFile()
        {
            HandleInfoTextFileResult result = new HandleInfoTextFileResult();
            result.Success = true;

            try
            {
                TryHandle3Text(result);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message + ex.StackTrace;
            }

            return result;
        }

        private void TryHandle3Text(HandleInfoTextFileResult result)
        {
            string content = File.ReadAllText("C:/info3.txt");
            if (string.IsNullOrEmpty(content))
            {
                result.Success = false;
                result.Msg = "没有内容";
            }

            string[] records = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            result.TotalRecords = records.Count();

            foreach (var record in records)
            {
                HandRecord3(record);
            }
        }
        private void HandRecord3(string record)
        {
            string[] userInfos = record.Split(new char[] { ',' });

            if (userInfos.Count() == 0)
                return;

            string userName = "";
            string tel1 = "";
            string tel2 = "";

            if (userInfos.Count() == 3)
            {
                userName = userInfos[0];
                tel1 = userInfos[1];
                tel2 = userInfos[2];
            }

            if (RegexHelper.IsTel(tel1))
                InsertToDb(userName, tel1);

            if (RegexHelper.IsTel(tel2))
                InsertToDb(userName, tel2);
        }

    }


    public class HandleInfoTextFileResult
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public int TotalRecords { get; set; }

    }
}
