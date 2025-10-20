using System;
using System.Collections.Generic;
using System.Linq;
using PRG_282_Project.Data_Layer;
using PRG_282_Project;

namespace PRG_282_Project.Business_Layer
{
    public class BusinessLogic
    {
        private readonly FileHandler fileHandler;

        public BusinessLogic()
        {
            fileHandler = new FileHandler();
        }

        public string CalculateRank(double score)
        {
            if (score >= 81 && score <= 100) return "S-Rank";
            if (score >= 61 && score <= 80) return "A-Rank";
            if (score >= 41 && score <= 60) return "B-Rank";
            if (score >= 0 && score <= 40) return "C-Rank";
            throw new ArgumentOutOfRangeException("Score must be between 0 and 100.");
        }

        public string CalculateThreatLevel(string rank)
        {
            switch (rank)
            {
                case "S-Rank": return "Finals Week (threat to the entire academy)";
                case "A-Rank": return "Midterm Madness (threat to a department)";
                case "B-Rank": return "Group Project Gone Wrong (threat to a study group)";
                case "C-Rank": return "Pop Quiz (potential threat to an individual student)";
                default: throw new ArgumentException("Invalid rank.");
            }
        }

        public void AddSuperhero(Superhero hero)
        {
            ValidateSuperhero(hero);
            hero.Rank = CalculateRank(hero.ExamScore);
            hero.ThreatLevel = CalculateThreatLevel(hero.Rank);

            List<Superhero> superheroes = fileHandler.ReadSuperheroes();
            if (superheroes.Any(h => h.HeroID == hero.HeroID))
                throw new Exception("Hero ID already exists.");
            superheroes.Add(hero);
            fileHandler.WriteSuperheroes(superheroes);
        }

        public void UpdateSuperhero(Superhero hero)
        {
            ValidateSuperhero(hero);
            hero.Rank = CalculateRank(hero.ExamScore);
            hero.ThreatLevel = CalculateThreatLevel(hero.Rank);

            List<Superhero> superheroes = fileHandler.ReadSuperheroes();
            int index = superheroes.FindIndex(h => h.HeroID == hero.HeroID);
            if (index == -1) throw new Exception("Hero not found.");
            superheroes[index] = hero;
            fileHandler.WriteSuperheroes(superheroes);
        }

        public void DeleteSuperhero(string heroID)
        {
            List<Superhero> superheroes = fileHandler.ReadSuperheroes();
            int removed = superheroes.RemoveAll(h => h.HeroID == heroID);
            if (removed == 0) throw new Exception("Hero not found.");
            fileHandler.WriteSuperheroes(superheroes);
        }

        public List<Superhero> GetAllSuperheroes()
        {
            return fileHandler.ReadSuperheroes();
        }

        public (int TotalHeroes, double AvgAge, double AvgScore, int SCount, int ACount, int BCount, int CCount) GenerateSummary()
        {
            List<Superhero> superheroes = GetAllSuperheroes();
            if (superheroes.Count == 0) return (0, 0, 0, 0, 0, 0, 0);

            return (
                superheroes.Count,
                superheroes.Average(h => h.Age),
                superheroes.Average(h => h.ExamScore),
                superheroes.Count(h => h.Rank == "S-Rank"),
                superheroes.Count(h => h.Rank == "A-Rank"),
                superheroes.Count(h => h.Rank == "B-Rank"),
                superheroes.Count(h => h.Rank == "C-Rank")
            );
        }

        public void SaveSummary(int total, double avgAge, double avgScore, int sCount, int aCount, int bCount, int cCount)
        {
            string summary = $"Total Superheroes: {total}\nAverage Age: {avgAge:F2}\nAverage Exam Score: {avgScore:F2}\nS-Rank Heroes: {sCount}\nA-Rank Heroes: {aCount}\nB-Rank Heroes: {bCount}\nC-Rank Heroes: {cCount}";
            fileHandler.WriteSummary(summary);
        }

        private void ValidateSuperhero(Superhero hero)
        {
            if (string.IsNullOrWhiteSpace(hero.HeroID)) throw new Exception("Hero ID cannot be empty.");
            if (string.IsNullOrWhiteSpace(hero.Name)) throw new Exception("Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(hero.Superpower)) throw new Exception("Superpower cannot be empty.");
            if (hero.Age <= 0) throw new Exception("Age must be a positive integer.");
            if (hero.ExamScore < 0 || hero.ExamScore > 100) throw new Exception("Exam Score must be between 0 and 100.");
        }
    }
}