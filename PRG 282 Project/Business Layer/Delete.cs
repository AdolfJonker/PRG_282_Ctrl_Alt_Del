using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Classes
{
    internal class Delete
    {
        public static bool DeleteByHeroID(string heroID)
        {
            string filePath = "superheroes.txt";
            string tempFile = "superheroes_temp.txt";
            bool found = false;

            try
            {
                using (var sr = new StreamReader(filePath))
                using (var sw = new StreamWriter(tempFile))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var fields = line.Split(',');
                        if (fields.Length > 0 && fields[0] == heroID)
                        {
                            found = true;
                            continue;
                        }
                        sw.WriteLine(line);
                    }
                }

                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting superhero: {ex.Message}");
                return false;
            }

            return found;
        }
    }
}
