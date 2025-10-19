using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace PRG_282_Project.Classes
{
    internal class Display
    {
         private string filePath = "superheroes.txt";

         //METHOD: Reads superheroes from the textfile and returns it as a list
         //Any UI work (DataGridView) is done in the Form Class
         public List<string[]> LoadSuperheroes()
         {
             List<string[]> heroes = new List<string[]>();

             try
             {
                 //Check if the file exists and then read it line by line
                 if (File.Exists(filePath))
                 {
                     using (StreamReader sr = new StreamReader(filePath))
                     {
                         string line;
                         while ((line = sr.ReadLine()) != null)
                         {
                             string[] parts = line.Split(',');
                             heroes.Add(parts);
                         }
                     }
                 }
                 else
                 {
                     Console.WriteLine("File not found:" + filePath);
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
             return heroes;
         }
    }
}
