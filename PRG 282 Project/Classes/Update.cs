using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PRG_282_Project.Classes
{
    internal class Update
    {
        public class Superhero
        {
            public string HeroID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public string Superpower { get; set; }
            public int ExamScore { get; set; }
            public string Rank { get; set; }
            public string ThreatLevel { get; set; }

            public static string GetRank(int score)
            {
                if (score >= 90) return "S";
                else if (score >= 75) return "A";
                else if (score >= 60) return "B";
                else if (score >= 40) return "C";
                else return "D";
            }

            public static string GetThreatLevel(string rank)
            {
                return rank switch
                {
                    "S" => "Planetary",
                    "A" => "National",
                    "B" => "City",
                    "C" => "Street",
                    "D" => "Training",
                    _ => "Unknown"
                };
            }

            public static Superhero FromCsv(string line)
            {
                var parts = line.Split(',');

                return new Superhero
                {
                    HeroID = parts[0],
                    Name = parts[1],
                    Age = int.Parse(parts[2]),
                    Superpower = parts[3],
                    ExamScore = int.Parse(parts[4]),
                    Rank = parts[5],
                    ThreatLevel = parts[6]
                };
            }

            public string ToCsv()
            {
                return $"{HeroID},{Name},{Age},{Superpower},{ExamScore},{Rank},{ThreatLevel}";
            }
        }

        public class SuperheroUpdateManager
        {
            private string filePath = "superheroes.txt";

            public List<Superhero> LoadAllHeroes()
            {
                if (!File.Exists(filePath))
                    return new List<Superhero>();

                return File.ReadAllLines(filePath)
                           .Where(line => !string.IsNullOrWhiteSpace(line))
                           .Select(Superhero.FromCsv)
                           .ToList();
            }

            public Superhero FindHeroById(string heroId)
            {
                var heroes = LoadAllHeroes();
                return heroes.FirstOrDefault(h => h.HeroID == heroId);
            }

            public bool UpdateHero(Superhero updatedHero)
            {
                var heroes = LoadAllHeroes();
                var index = heroes.FindIndex(h => h.HeroID == updatedHero.HeroID);

                if (index == -1)
                    return false;

                // Recalculate rank and threat level
                updatedHero.Rank = Superhero.GetRank(updatedHero.ExamScore);
                updatedHero.ThreatLevel = Superhero.GetThreatLevel(updatedHero.Rank);

                // Replace old record
                heroes[index] = updatedHero;

                // Save back to file
                File.WriteAllLines(filePath, heroes.Select(h => h.ToCsv()));
                return true;
            }
        }
    }
}
