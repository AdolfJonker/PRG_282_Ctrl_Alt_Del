using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Data_Layer
{
    internal class FileHandler
    {
         public List<string> ReadFiles()
 {
     List<string> fileLines = new List<string>();

     fileLines = File.ReadAllLines("superheroes.txt").ToList();

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

     File.WriteAllLines("summary.txt", output);

     return output;
 }
    }
}
