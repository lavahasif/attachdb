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
        public static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var coTxt = path + @"\co.txt";
            var excelXlsx = path + @"\excel.xlsx";
            CreateInitialfile(coTxt, excelXlsx);


            // string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;


            var readAllText = System.IO.File.ReadAllText(coTxt);
            var readexcel = excelXlsx;
            // var connectionString = new connectionString();
            connectionString.Connections = readAllText;

            var repository = new Repository();
            var service = new Service();
            var readExcel = repository.ReadExcel(readexcel, "xlsx");
            var convertDataTable = service.ConvertDataTable<Ex>(readExcel);
            convertDataTable.ForEach(x =>
            {
                service.AttachDatabase(x.databse, x.mdf_file.ToString(), x.log_file);
                // Console.WriteLine(x.databse);
            });


            // for (var i = readexcel.Length - 1; i >= 0; i--)
            // {
            //     var dt = readExcel.Rows;
            //     var name = dt[i][0].ToString();
            //     Console.WriteLine(name);
            //    
            // }
        }

        private static void CreateInitialfile(string coTxt, string excelXlsx)
        {
            if (!File.Exists(coTxt))
            {
                var capacity = Properties.Resources.co;
                File.WriteAllText(coTxt, capacity);
            }

            if (!File.Exists(excelXlsx))
            {
                var capacity = Properties.Resources.excel;
                File.WriteAllBytes(excelXlsx, capacity);
            }
        }
    }
}