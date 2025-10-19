using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PRG_282_Project.Business_Layer; // Superhero
using PRG_282_Project.Data_Layer;     // FormatHandler, FileHandler

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


    // Reads via FormatHandler, uses stored Rank from file (via FileHandler),
    // computes summary, writes summary.txt, returns values for UI.

    public sealed class Summary
    {
        private readonly string _outPath;

        public Summary(string outPath = "summary.txt")
        {
            _outPath = outPath;
        }

        public SummaryResult Generate()
        {
            // Load heroes as objects (id/name/age/superpower/score)
            var heroes = LoadHeroes();

            // Build a map of HeroID -> stored Rank from the raw file lines
            var rankById = LoadStoredRanks();

            // Compute stats using stored ranks
            var result = Compute(heroes, rankById);

            // Persist human-readable summary
            Save(result, _outPath);

            return result;
        }

        // -------------------------
        // Internals
        // -------------------------

        private static List<Superhero> LoadHeroes()
        {
            var fmt = new FormatHandler();
            return fmt.FormatData() ?? new List<Superhero>();
        }

        /// <summary>
        /// Reads raw lines via FileHandler and extracts stored Rank.
        /// Expected formats (both supported):
        ///   [0]Id,[1]Name,[2]Age,[3]Superpower,[4]Score,[5]Rank
        ///   or legacy with extra columns (Rank still at index 5).
        /// If rank is missing, we do NOT derive; we simply skip counting it (treated as 'C' below).
        /// </summary>
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
                    var rank = NormalizeRank(parts[5]); // trust stored, just normalize
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
                // Age
                if (int.TryParse(h.age?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var ageVal))
                    ages.Add(ageVal);

                // Score
                if (int.TryParse(h.score?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var scoreVal))
                    scores.Add(scoreVal);

                // Stored rank (default to C only if missing/empty)
                string rank = null;
                if (!string.IsNullOrWhiteSpace(h.heroID))
                    rankById.TryGetValue(h.heroID, out rank);

                rank = NormalizeRank(rank);

                switch (rank)
                {
                    case "S": countS++; break;
                    case "A": countA++; break;
                    case "B": countB++; break;
                    default: countC++; break; // includes null/empty -> treated as C
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

        /// <summary>
        /// Normalizes stored rank strings to "S", "A", "B", or "C".
        /// Accepts values like "S", "S-Rank", "rank a", etc.
        /// </summary>
        private static string NormalizeRank(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return "C";
            var t = raw.Trim().ToUpperInvariant();
            var c = t[0];
            return (c == 'S' || c == 'A' || c == 'B' || c == 'C') ? c.ToString() : "C";
        }
    }
}

