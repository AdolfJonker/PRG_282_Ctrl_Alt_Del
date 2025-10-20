using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRG_282_Project.Business_Layer;

namespace PRG_282_Project.Data_Layer
{
    public class FileHandler
    {
        private const string SuperheroesFile = "superheroes.txt";
        private const string SummaryFile = "summary.txt";

        
        public List<Superhero> ReadSuperheroes()
        {
            List<Superhero> superheroes = new List<Superhero>();

            if (!File.Exists(SuperheroesFile))
            {
                return superheroes; 
            }

            try
            {
                string[] lines = File.ReadAllLines(SuperheroesFile);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 7)
                    {
                        Superhero hero = new Superhero
                        {
                            HeroID = parts[0].Trim(),
                            Name = parts[1].Trim(),
                            Age = int.Parse(parts[2].Trim()),
                            Superpower = parts[3].Trim(),
                            ExamScore = double.Parse(parts[4].Trim()),
                            Rank = parts[5].Trim(),
                            ThreatLevel = parts[6].Trim()
                        };
                        superheroes.Add(hero);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reading superheroes file: {ex.Message}");
            }

            return superheroes;
        }

       
       
        public void WriteSuperheroes(List<Superhero> superheroes)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SuperheroesFile))
                {
                    foreach (Superhero hero in superheroes)
                    {
                        writer.WriteLine($"{hero.HeroID},{hero.Name},{hero.Age},{hero.Superpower},{hero.ExamScore},{hero.Rank},{hero.ThreatLevel}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to superheroes file: {ex.Message}");
            }
        }

        
        public void WriteSummary(string summary)
        {
            try
            {
                File.WriteAllText(SummaryFile, summary);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to summary file: {ex.Message}");
            }
        }
    }
}
