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
        public List<string[]> LoadSuperheroes()
        {
            List<string[]> heroes = new List<string[]>();
            string filePath = AppConfig.SuperheroesFilePath;

            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"Superhero data file not found: {filePath}");
                }

                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split(',');
                        if (parts.Length >= 7)
                        {
                            heroes.Add(parts);
                        }
                        else
                        {
                            MessageBox.Show($"Skipping malformed line in file: {line}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading superheroes: {ex.Message}", ex);
            }
            return heroes;
        }
    }
}
