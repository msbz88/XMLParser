﻿using System;
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
            if (!Directory.Exists(InputDirectory)) {
                Console.WriteLine("Could not find the specified folder.");
                Console.WriteLine("----------------------------------------------------------------");
                Console.WriteLine("Task failed.");
                Console.ReadKey();
                return;
            }
            string[] files = Directory.GetFiles(InputDirectory);
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------------------------");
            if (files.Any()) {
                ResultDirectory = InputDirectory + "\\Result\\";
                Directory.CreateDirectory(ResultDirectory);
            } else {
                Console.WriteLine("No files found in the specified folder.");
            }
            foreach (var item in files) {
                var ext = Path.GetExtension(item);
                if (ext == ".txt" || ext == ".xml") {
                    try {
                        ParseXML(item);
                        Console.WriteLine(Path.GetFileName(item) + " - done");
                    } catch (Exception) {
                        Console.WriteLine(Path.GetFileName(item) + " - error");
                    }
                } else {
                    Console.WriteLine(Path.GetFileName(item) + " - extension not supported");
                }
            }
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine("Task complited.");
            Console.ReadKey();
        }

        private static void ParseXML(string path) {
            var fileContent = File.ReadAllText(path);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(fileContent);
            string subDataType = xmlDoc.SelectSingleNode("ScdImportData/MetaData/SubDataType").InnerText;
            var nodes = xmlDoc.SelectNodes("ScdImportData/ScdData/" + subDataType);
            List<string> headers = new List<string>();
            List<string> data = new List<string>();
            List<string> result = new List<string>();
            if (nodes.Count > 0) {
                var firstNode = nodes[0];
                if (firstNode.HasChildNodes) {
                    for (int i = 0; i < firstNode.ChildNodes.Count; i++) {
                        headers.Add(firstNode.ChildNodes[i].Name);
                    }
                    result.Add(string.Join(";", headers));
                }
                foreach (XmlNode node in nodes) {
                    data.Clear();
                    if (node.HasChildNodes) {
                        for (int i = 0; i < node.ChildNodes.Count; i++) {
                            data.Add(node.ChildNodes[i].InnerText);
                        }
                        result.Add(string.Join(";", data));
                    }
                }
                File.WriteAllLines(ResultDirectory + Path.GetFileNameWithoutExtension(path) + ".txt", result);
            }
        }
    }
}

