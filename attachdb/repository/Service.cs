using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace attachdb.repository
{
    public class Service
    {
        public void AttachDatabase(String name, String mdf, String ldf)
        {
            try
            {
                // var connectionString = new connectionString();
                SqlConnection con = new SqlConnection(connectionString.Connections);
                con.Open();

                // SqlDataAdapter da = new SqlDataAdapter("select name from sys.databases", con);
                // DataTable dt = new DataTable();
                // da.Fill(dt);
                // var list = new List<string>();
                // for (var i = 0; i < dt.Rows.Count; i++)
                // {
                //     var o = dt.Rows[i][0].ToString();
                //     list.Add(o);
                // }


                // if (!list.Contains(name))
                // {
                Console.WriteLine(name);
                // setStored(name, mdf, con);
                setStext(name, mdf, ldf, con);
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void setStored(string name, string mdf, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("sp_attach_db");
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@dbname", name);
            cmd.Parameters.AddWithValue("@filename1", mdf);
            cmd.ExecuteNonQuery();
        }

        private static void setStext(string name, string mdf, string ldf, SqlConnection con)
        {
            var sql =
                $@" CREATE DATABASE {name}  ON   (   FILENAME = '{mdf}') LOG ON  (    FILENAME = '{ldf}') FOR ATTACH; ";
            SqlCommand cmd = new SqlCommand(sql);
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;

            cmd.ExecuteNonQuery();
        }

        public List<Ex> ConvertDataTable<T>(DataTable dt)
        {
            List<Ex> data = new List<Ex>();
            foreach (DataRow row in dt.Rows)
            {
                // T item = GetItem<T>(row);
                data.Add(new Ex()
                {
                    databse = row.ItemArray[0].ToString(),
                    mdf_file = row.ItemArray[1].ToString(),
                    log_file = row.ItemArray[2].ToString(),
                });
            }

            return data;
        }
    }
}

public class Ex
{
    public string databse { get; set; }
    public string mdf_file { get; set; }
    public string log_file { get; set; }
}

class connectionString
{
    public static String Connections { get; set; } = "";
}