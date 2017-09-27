using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace B2MAWS
{
    class CSVParser
    {
        static string sqlscript;

        public static void GetScript()
        {
            DownloadScripts c = new DownloadScripts();
            sqlscript = DownloadScripts.GetObject();
            //RunSqlScript(sqlscript); 
            CreateCSV(sqlscript);
        }
        public static void RunSqlScript(string s)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(s, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable schema = rdr.GetSchemaTable();
                foreach (DataRow row in schema.Rows)
                {
                    foreach (DataColumn col in schema.Columns)
                        Console.WriteLine(col.ColumnName + " = " + row[col]);
                }
                rdr.Close(); con.Close();
            }
        }
        public static void CreateCSV(string s)
        {
            string destinationFile= "csv1.csv";
            string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            using (var command = new SqlCommand(s, con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                using (var outFile = File.CreateText(destinationFile))
                {
                    string[] columnNames = GetColumnNames(reader).ToArray();
                    int numFields = columnNames.Length;
                    outFile.WriteLine(string.Join("     ", columnNames));
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string[] columnValues =
                                Enumerable.Range(0, numFields)
                                          .Select(i => reader.GetValue(i).ToString())
                                          .Select(field => string.Concat("\"", field.Replace("\"", "\"\""), "\""))
                                          .ToArray();
                            outFile.WriteLine(string.Join("     ", columnValues));
                        }
                    }
                }
            }

        }
        private static IEnumerable<string> GetColumnNames(IDataReader reader)
        {
            foreach (DataRow row in reader.GetSchemaTable().Rows)
            {
                yield return (string)row["ColumnName"];
            }
        }
    }
}
