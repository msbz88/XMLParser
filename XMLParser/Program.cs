using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace XMLParser {
    class Program {
        static string InputDirectory = "";
        static string ResultDirectory = "";

        static void Main(string[] args) {
            Console.WriteLine("Please specify the path to the folder with xml files:");
            InputDirectory = Console.ReadLine();
            if (!InputDirectory.Contains("\\")) {
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("Task failed.");
                Console.ReadKey();
                return;
            }
            ResultDirectory = InputDirectory + "\\Result\\";
            Directory.CreateDirectory(ResultDirectory);
            string[] files = Directory.GetFiles(InputDirectory);
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------");
            foreach (var item in files) {
                var ext = Path.GetExtension(item);
                if (ext == ".txt" || ext == ".xml") {
                    try {
                        ParseXML(item);
                        Console.WriteLine(Path.GetFileName(item) + " - done");
                    } catch (Exception) {
                        Console.WriteLine(Path.GetFileName(item) + " - error");
                    }
                }
            }
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Task complited.");
            Console.ReadKey();
        }

        private static void ParseXML(string path) {
            XmlDocument doc = new XmlDocument();
            var xml = File.ReadAllText(path);
            doc.LoadXml(xml);
            string subDataType = doc.SelectSingleNode("ScdImportData/MetaData/SubDataType").InnerText;
            XmlNode root = doc.SelectSingleNode("ScdImportData/ScdData/" + subDataType);
            List<string> headers = new List<string>();
            List<string> data = new List<string>();
            List<string> result = new List<string>();
            if (root.HasChildNodes) {
                for (int i = 0; i < root.ChildNodes.Count; i++) {
                    headers.Add(root.ChildNodes[i].Name);
                    data.Add(root.ChildNodes[i].InnerText);
                }
                result.Add(string.Join(";", headers));
                result.Add(string.Join(";", data));
            }
            File.WriteAllLines(ResultDirectory + Path.GetFileNameWithoutExtension(path) + ".txt", result);
        }
    }
}

