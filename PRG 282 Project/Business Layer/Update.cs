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
        // Manager that loads via FormatHandler (data handler)
        // and saves back to superheroes.txt, then refreshes summary via FileHandler.
        public class SuperheroUpdateManager
        {
            private const string DataPath = "superheroes.txt";


            /// Loads all heroes using the existing FormatHandler (data handler).
            public List<Superhero> LoadAllHeroes()
            {
                var fmt = new FormatHandler();
                return fmt.FormatData(); // returns List<Superhero>
            }


            /// Finds a hero by ID (matches Business_Layer.Superhero.heroID).
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

                // Load (via data handler)
                var fmt = new FormatHandler();
                var heroes = fmt.FormatData();

                // Find by ID (case-sensitive to match file content)
                int idx = heroes.FindIndex(h => h.heroID == updatedHero.heroID);
                if (idx < 0)
                    return false;

                // Replace in-memory
                heroes[idx] = updatedHero;

                // Persist back to the source file (CSV lines).
                // We keep the exact column order your FormatHandler expects: id,name,age,superpower,score
                var csvLines = heroes.Select(h =>
                    $"{h.heroID},{h.name},{h.age},{h.superpower},{h.score}");

                File.WriteAllLines(DataPath, csvLines);

                // regenerate your summary.txt using your FileHandler
                // (this uses your existing pipeline and respects your handlers).
                var fh = new FileHandler();
                fh.WriteToFile();

                return true;
            }
        }
    }
}
