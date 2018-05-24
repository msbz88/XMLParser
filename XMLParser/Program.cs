using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

namespace XMLParser {
    class Program {
        private static void PrintValues(DataSet dataSet, string label) {
            Console.WriteLine("\n" + label);
            foreach (DataTable table in dataSet.Tables) {
                Console.WriteLine("TableName: " + table.TableName);
                foreach (DataRow row in table.Rows) {
                    foreach (DataColumn column in table.Columns) {
                        Console.Write("\table " + row[column]);
                    }
                    Console.WriteLine();
                }
            }
        }

        public static void WriteDataToFile(DataTable dataTable, string filePath, string delimiter) {
            int i = 0;
            StreamWriter sw = new StreamWriter(filePath, false);
            for (i = 0; i < dataTable.Columns.Count - 1; i++) {
                sw.Write(dataTable.Columns[i].ColumnName + delimiter);
            }
            sw.Write(dataTable.Columns[i].ColumnName);
            sw.WriteLine();
            foreach (DataRow row in dataTable.Rows) {
                object[] itemsArray = row.ItemArray;
                for (i = 0; i < itemsArray.Length - 1; i++) {
                    sw.Write(itemsArray[i].ToString() + delimiter);
                }
                sw.Write(itemsArray[i].ToString());
                sw.WriteLine();
            }
            sw.Close();
        }

        public static void WriteDataSetToFile(DataSet ds, string filePath, string delimiter) {
            StreamWriter sw = new StreamWriter(filePath, false);
            foreach (DataTable table in ds.Tables) {
                for (int i = 0; i < table.Columns.Count; ++i)
                    sw.Write(delimiter + table.Columns[i].ColumnName.Substring(0, Math.Min(6, table.Columns[i].ColumnName.Length)));
                sw.WriteLine();
                foreach (var row in table.AsEnumerable()) {
                    for (int i = 0; i < table.Columns.Count; ++i) {
                        sw.Write(delimiter + row[i]);
                    }
                    sw.WriteLine();
                }
            }
            sw.Close();
        }

        static void Main(string[] args) {
            string inputFile = @"C:\Users\msbz\Desktop\xml_PSP\[20180315_TRANSACTION_Publish.xml";
            string saveFile = @"C:\Users\msbz\Desktop\xml_PSP\20180315_TRANSACTION_Publish_res_";

            using (StreamReader sr = new StreamReader(inputFile)) {
                XElement booksFromFile = XElement.Load(sr);
                Console.WriteLine(booksFromFile);
            }




            /*
            DataSet ds = new DataSet();
            using (StreamReader sr = new StreamReader(inputFile)) {
                XmlReader xmlFile = XmlReader.Create(sr);
                ds.ReadXml(xmlFile);
            }
            
            Console.WriteLine("Parse completed");


            //PrintValues(ds, "Merged With table.");
            //WriteDataToFile(ds.Tables[0], saveFile + i + ".txt", "\t");
            int i = 0;
            foreach (DataTable item in ds.Tables) {
                WriteDataToFile(item, saveFile + ++i + ".txt", "\t");
                Console.WriteLine($"Columnns: {ds.Tables[0].Columns.Count}\nRows: { ds.Tables[0].Rows.Count}");
                Console.WriteLine("Data saved to file");
            }
            
            //WriteDataSetToFile(ds, saveFile + ".txt", "\t");
            */
        }
    }
}

