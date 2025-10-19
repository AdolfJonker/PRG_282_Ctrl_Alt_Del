using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PRG_282_Project.Classes
{
    /// <summary>
    /// POCO for returning summary values to the UI.
    /// </summary>
    public sealed class SummaryResult
    {
        public int TotalHeroes { get; set; }
        public double AverageAge { get; set; }
        public double AverageExamScore { get; set; }
        public int CountS { get; set; }
        public int CountA { get; set; }
        public int CountB { get; set; }
        public int CountC { get; set; }

        /// Formats the summary for saving to file.
        public override string ToString()
        {
            return
           $@"=== Superhero Summary Report ===
            Generated: {DateTime.Now:yyyy-MM-dd HH:mm}

               Total heroes           : {TotalHeroes}
               Average age            : {AverageAge:0.00}
               Average exam score     : {AverageExamScore:0.00}

            Heroes per rank:
              S: {CountS}
              A: {CountA}
              B: {CountB}
              C: {CountC}";
        }
    }

   
    /// Reads superheroes.txt, computes a summary, writes summary.txt, returns values for UI.
   
    public sealed class Summary
    {
        private readonly string _dataPath;
        private readonly string _outPath;

        /// <param name="dataPath">Defaults to 'superheroes.txt' in app folder.</param>
        /// <param name="outPath">Defaults to 'summary.txt' in app folder.</param>
        public Summary(string dataPath = "superheroes.txt", string outPath = "summary.txt")
        {
            _dataPath = dataPath;
            _outPath = outPath;
        }

      
        /// Main call for your form: compute and persist a summary.
       
        public SummaryResult Generate()
        {
            var heroes = LoadHeroes(_dataPath);
            var result = Compute(heroes);
            Save(result, _outPath);
            return result;
        }

        /// Loads heroes from the data file.
        private static IEnumerable<(string Id, string Name, int Age, int Score, string Rank)> LoadHeroes(string path)
        {
            if (!File.Exists(path))
                return Enumerable.Empty<(string, string, int, int, string)>();

            var list = new List<(string, string, int, int, string)>();

            foreach (var raw in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;

                // Expected format: Id, Name, Age, City, ExamScore, Rank, Description
                var parts = raw.Split(',');
                if (parts.Length < 7) continue;

                // age / score
                if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var age)) continue;
                if (!int.TryParse(parts[4], NumberStyles.Integer, CultureInfo.InvariantCulture, out var score)) continue;

                var rank = NormalizeRank(parts[5]); // "S", "A", "B", "C" from things like "S", "S-Rank", etc.

                list.Add((parts[0].Trim(), parts[1].Trim(), age, score, rank));
            }

            return list;
        }

        /// Computes summary values from the hero list.
        private static SummaryResult Compute(IEnumerable<(string Id, string Name, int Age, int Score, string Rank)> heroes)
        {
            var arr = heroes as (string Id, string Name, int Age, int Score, string Rank)[] ?? heroes.ToArray();
            var total = arr.Length;

            double avgAge = total == 0 ? 0 : arr.Average(h => (double)h.Age);
            double avgScore = total == 0 ? 0 : arr.Average(h => (double)h.Score);

            return new SummaryResult
            {
                TotalHeroes = total,
                AverageAge = Math.Round(avgAge, 2),
                AverageExamScore = Math.Round(avgScore, 2),
                CountS = arr.Count(h => h.Rank == "S"),
                CountA = arr.Count(h => h.Rank == "A"),
                CountB = arr.Count(h => h.Rank == "B"),
                CountC = arr.Count(h => h.Rank == "C"),
            };
        }


        /// Saves the summary to the output file.
        private static void Save(SummaryResult s, string outPath)
        {
            var dir = Path.GetDirectoryName(Path.GetFullPath(outPath));
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(outPath, s.ToString());
        }

        /// Normalizes rank strings to "S", "A", "B", or "C".
        private static string NormalizeRank(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "C";
            var t = raw.Trim().ToUpperInvariant();


            if (t.Length > 0)
            {
                var first = t[0];
                if (first == 'S' || first == 'A' || first == 'B' || first == 'C')
                    return first.ToString();
            }
            return "C";
        }
    }
}
