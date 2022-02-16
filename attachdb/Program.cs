using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using attachdb.repository;

namespace attachdb
{
    internal class Program
    {
        private static void CreateInitialfile(string coTxt, string excelXlsx)
        {
            if (!File.Exists(coTxt))
            {
                var capacity = Properties.Resources.co;
                File.WriteAllText(coTxt, capacity);
            }


            if (!File.Exists(excelXlsx))
            {
                byte[] capacity = Properties.Resources.excel;
                File.WriteAllBytes(excelXlsx, capacity);
            }
        }

        public static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var coTxt = path + @"\co.txt";
            var dbTxt = path + @"\db.txt";
            var excelXlsx = path + @"\excel.xlsx";
            CreateInitialfile(coTxt, excelXlsx);
            var filepath = path + @"\db";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            if (!File.Exists(dbTxt))
            {
                File.WriteAllText(dbTxt, "");
            }


            // string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;


            var readAllText = System.IO.File.ReadAllText(coTxt);
            var readexcel = excelXlsx;
            // var connectionString = new connectionString();
            connectionString.Connections = readAllText;

            var repository = new Repository();
            var service = new Service();
            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");
            Console.WriteLine("1 . Attach Database");
            Console.WriteLine("2 . Restore Database");
            Console.WriteLine("3 . Backup Database");
            Console.WriteLine("===============================================");
            Console.WriteLine("===============================================");
            var readLine = Console.ReadLine();
            if (readLine == "2")
            {
                DirectoryInfo d = new DirectoryInfo(filepath); //Assuming Test is your Folder

                FileInfo[] Files = d.GetFiles(); //Getting Text files
                string str = "";

                foreach (FileInfo file in Files)
                {
                    service.Restore(file.Name, filepath);
                }
            }
            else if (readLine == "3")
            {
                var allText = File.ReadAllText(dbTxt);
                var strings = allText.Split(',');
                foreach (var s in strings)
                {
                    service.BackUpDB(s, filepath);
                }
            }
            else
            {
                AttachDatabase(repository, readexcel, service);
            }


            // for (var i = readexcel.Length - 1; i >= 0; i--)
            // {
            //     var dt = readExcel.Rows;
            //     var name = dt[i][0].ToString();
            //     Console.WriteLine(name);
            //    
            // }
        }

        private static void AttachDatabase(Repository repository, string readexcel, Service service)
        {
            var readExcel = repository.ReadExcel(readexcel, "xlsx");
            var convertDataTable = service.ConvertDataTable<Ex>(readExcel);
            convertDataTable.ForEach(x =>
            {
                service.AttachDatabase(x.databse, x.mdf_file.ToString(), x.log_file);
                // Console.WriteLine(x.databse);
            });
        }
    }
}