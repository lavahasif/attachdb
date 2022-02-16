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

        public void BackUpDB(string name, string fname)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString.Connections))
                {
                    cn.Open();
                    string cmd = $"BACKUP DATABASE [{name}] TO DISK='" + fname + @"\" + name + ".bak" + "'";
                    using (var command = new SqlCommand(cmd, cn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Restore(string Database, string Filepath)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString.Connections))
                {
                    con.Open();
                    var query = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" +
                                Database.Replace(".bak", "") + "') DROP DATABASE " + Database.Replace(".bak", "") +
                                " RESTORE DATABASE " + Database.Replace(".bak", "") + " FROM DISK = '" + Filepath +
                                @"\" + Database + "'";
                    var command = new SqlCommand(query);
                    command.Connection = con;
                    command.ExecuteNonQuery();
                    // SqlCommand cmd1 =
                    //     new SqlCommand(
                    //         "ALTER DATABASE [" + Database.Replace(@".bak", "") +
                    //         "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE ", con);
                    //
                    // cmd1.ExecuteNonQuery();
                    // SqlCommand cmd2 =
                    //     new SqlCommand(
                    //         "USE MASTER RESTORE DATABASE [" + Database.Replace(@".bak", "") + "] FROM DISK='" +
                    //         Filepath + "\\" + Database + "' WITH REPLACE",
                    //         con);
                    // cmd2.ExecuteNonQuery();
                    // SqlCommand cmd3 =
                    //     new SqlCommand("ALTER DATABASE [" + Database.Replace(@".bak", "") + "] SET MULTI_USER", con);
                    // cmd3.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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