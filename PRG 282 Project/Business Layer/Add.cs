using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace PRG_282_Project.Classes
{
    internal class Add
    {
        public static void AddNewHero(string heroID, string name, string age, string superpower, string score)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(heroID) ||
                    string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(age) ||
                    string.IsNullOrWhiteSpace(superpower) ||
                    string.IsNullOrWhiteSpace(score))
                {
                    throw new ArgumentException("Please fill in all fields.");
                }
                if (!int.TryParse(age, out int ageValue) || ageValue <= 0)
                {
                    throw new ArgumentException("You cannot be a negative age.");
                }
                if (!int.TryParse(score, out int scoreValue) || scoreValue < 0 || scoreValue > 100)
                {
                    throw new ArgumentException("Score must be a number between 0 and 100.");
                }
                string rank = CalculateRank(scoreValue);
                string threatLevel = CalculateRank(rank);

                string heroData = $"{heroID},{name},{ageValue},{superpower},{scoreValue},{rank},{threatLevel}";

                string filePath = "superheroes.txt";
                File.AppendAllText(filePath, heroData + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding superhero:");
            }
        }

        //Rank Calculator
        static string CalculateRank(int score)
        {
            if (score >= 81) return "S-Rank";
            if (score >= 61) return "A-Rank";
            if (score >= 41) return "B-Rank";
            return "C-Rank";
        }

        //Threat Calculator
        private static string CalculateRank(string rank)
        {
            switch (rank)
            {
                case "S-Rank": return "Finals Week";
                case "A-Rank": return "Midterm Madness";
                case "B-Rank": return "Group Project Gone Wrong";
                default: return "Pop Quiz";
            }
        }
    }
}

