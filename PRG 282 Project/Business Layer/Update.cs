using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PRG_282_Project.Business_Layer; // Superhero, FormatHandler
using PRG_282_Project.Data_Layer;     // FileHandler

namespace PRG_282_Project.Classes
{
    internal class Update
    {
        public class SuperheroUpdateManager
        {
            private readonly string DataPath = AppConfig.SuperheroesFilePath;

            public List<Superhero> LoadAllHeroes()
            {
                var fmt = new FormatHandler();
                return fmt.FormatData();
            }

            public Superhero FindHeroById(string heroId)
            {
                if (string.IsNullOrWhiteSpace(heroId))
                    return null;

                return LoadAllHeroes().FirstOrDefault(h => h.heroID == heroId);
            }

            public bool UpdateHero(Superhero updatedHero)
            {
                if (updatedHero == null || string.IsNullOrWhiteSpace(updatedHero.heroID))
                    return false;

                var heroes = LoadAllHeroes();
                int idx = heroes.FindIndex(h => h.heroID == updatedHero.heroID);
                if (idx < 0)
                    return false;

                heroes[idx] = updatedHero;

                var csvLines = heroes.Select(h =>
                {
                    int score = int.TryParse(h.score, out int s) ? s : 0;
                    string rank = Add.CalculateRank(score);
                    string threatLevel = Add.CalculateThreatLevel(rank);
                    return $"{h.heroID},{h.name},{h.age},{h.superpower},{h.score},{rank},{threatLevel}";
                });

                File.WriteAllLines(DataPath, csvLines);

                var fh = new FileHandler();
                fh.WriteToFile();

                return true;
            }
        }
    }
}
