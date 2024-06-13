using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_demo
{
    internal class ReadJSON
    {
        public ReadJSON() 
        {
        }
        public static dynamic ReadJsonFile(string fileName)
        {
            //string directoryPath = @"C:\Train Simulator\Data\penilaian";"C:\Train Simulator\Data\penilaian\MRT_2024-01-05_14-25-43.json"
            string directoryPath = @"C:\Train Simulator\Data\penilaian\";
            Directory.CreateDirectory(directoryPath);
            string[] jsonFiles = Directory.GetFiles(directoryPath,"*.json")
                                           .OrderByDescending(f => new FileInfo(f).LastWriteTime)
                                           .ToArray();

            if (jsonFiles.Length > 0)
            {
                string latestJsonFile = jsonFiles[0]; // Get the latest JSON file based on last write time
                dynamic jsonFile = JsonConvert.DeserializeObject(File.ReadAllText(latestJsonFile));
                return jsonFile;
            }
            else
            {
                Console.WriteLine("No JSON files found in the specified directory.");
                return null;
            }
        }
    }
}
