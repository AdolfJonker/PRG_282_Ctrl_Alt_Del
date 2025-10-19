using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PRG_282_Project.Business_Layer;
using PRG_282_Project.Data_Layer;

namespace PRG_282_Project.Classes
{
    public sealed class SummaryResult
    {
        public int TotalHeroes { get; set; }
        public double AverageAge { get; set; }
        public double AverageExamScore { get; set; }
        public int CountS { get; set; }
        public int CountA { get; set; }
        public int CountB { get; set; }
        public int CountC { get; set; }

        public override string ToString()
        {
                return
            $@"=== Superhero Summary Report ===
            Generated: {DateTime.Now:yyyy-MM-dd HH:mm}

              Total heroes           : {TotalHeroes}
              Average age            : {AverageAge:0.00}
              Average exam score     : {AverageExamScore:0.00}

            Heroes per rank (stored values):
              S : {CountS}
              A : {CountA}
              B : {CountB}
              C : {CountC}";
                    }
        }

    public sealed class Summary
    {
        private readonly string _outPath;

        public Summary(string outPath = null)
        {
            _outPath = outPath ?? AppConfig.SummaryFilePath;
        }

        public SummaryResult Generate()
        {
            var heroes = LoadHeroes();
            var rankById = LoadStoredRanks();
            var result = Compute(heroes, rankById);
            Save(result, _outPath);
            return result;
        }

        private static List<Superhero> LoadHeroes()
        {
            var fmt = new FormatHandler();
            return fmt.FormatData() ?? new List<Superhero>();
        }

        private static Dictionary<string, string> LoadStoredRanks()
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var fh = new FileHandler();
            foreach (var raw in fh.ReadFiles() ?? new List<string>())
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;
                var parts = raw.Split(',');
                if (parts.Length >= 6)
                {
                    var id = parts[0].Trim();
                    var rank = NormalizeRank(parts[5]);
                    if (!string.IsNullOrEmpty(id))
                        map[id] = rank;
                }
            }
            return map;
        }

        private static SummaryResult Compute(IEnumerable<Superhero> heroes, Dictionary<string, string> rankById)
        {
            var arr = (heroes ?? Enumerable.Empty<Superhero>()).ToArray();
            int total = arr.Length;

            var ages = new List<int>();
            var scores = new List<int>();
            int countS = 0, countA = 0, countB = 0, countC = 0;

            foreach (var h in arr)
            {
                if (int.TryParse(h.age?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var ageVal))
                    ages.Add(ageVal);

                if (int.TryParse(h.score?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var scoreVal))
                    scores.Add(scoreVal);

                string rank = null;
                if (!string.IsNullOrWhiteSpace(h.heroID))
                    rankById.TryGetValue(h.heroID, out rank);

                rank = NormalizeRank(rank);

                switch (rank)
                {
                    case "S": countS++; break;
                    case "A": countA++; break;
                    case "B": countB++; break;
                    default: countC++; break;
                }
            }

            double avgAge = ages.Count == 0 ? 0 : ages.Average();
            double avgScore = scores.Count == 0 ? 0 : scores.Average();

            return new SummaryResult
            {
                TotalHeroes = total,
                AverageAge = Math.Round(avgAge, 2),
                AverageExamScore = Math.Round(avgScore, 2),
                CountS = countS,
                CountA = countA,
                CountB = countB,
                CountC = countC
            };
        }

        private static void Save(SummaryResult s, string outPath)
        {
            var dir = Path.GetDirectoryName(Path.GetFullPath(outPath));
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(outPath, s.ToString());
        }

        private static string NormalizeRank(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "C";
            var t = raw.Trim().ToUpperInvariant();
            var c = t[0];
            return (c == 'S' || c == 'A' || c == 'B' || c == 'C') ? c.ToString() : "C";
        }
    }
}