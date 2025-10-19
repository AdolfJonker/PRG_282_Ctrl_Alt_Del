using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG_282_Project.Business_Layer;

namespace PRG_282_Project.Data_Layer
{
    internal class FileHandler
    {
        public List<string> ReadFiles()
        {
            List<string> fileLines = new List<string>();
            string filePath = AppConfig.SuperheroesFilePath;

            if (File.Exists(filePath))
            {
                fileLines = File.ReadAllLines(filePath).ToList();
            }
            return fileLines;
        }

        public List<string> WriteToFile()
        {
            List<string> output = new List<string>();
            FormatHandler handler = new FormatHandler();

            foreach (var p in handler.FormatData())
            {
                output.Add(p.ToString());
            }

            File.WriteAllLines(AppConfig.SummaryFilePath, output);
            return output;
        }
    }
}
