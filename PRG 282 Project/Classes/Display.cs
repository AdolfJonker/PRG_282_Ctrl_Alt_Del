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
         string FilePath = "superheroes.txt";
         List<string[]> heroes = new List<string[]>();

 try
 {
    using (StreamReader sr = new StreamReader(filePath))
     {
         string line;

         while((line = sr.ReadLine()) != null)
         {
             string[] parts = line.Split(',');//splitting each line into different fields
             heroes.Add(parts);//add the fields to a list
         }
     }

     dataGridViewHeroes.Columns.Clear();
     dataGridViewHeroes.Rows.Clear();

     //Column Headers
     dataGridViewHeroes.Columns.Add("HeroID", "Hero ID");
     dataGridViewHeroes.Columns.Add("Name", "Name");
     dataGridViewHeroes.Columns.Add("Age", "Age");
     dataGridViewHeroes.Columns.Add("Superpower", "Superpower");
     dataGridViewHeroes.Columns.Add("ExamScore", "Exam Score");
     dataGridViewHeroes.Columns.Add("Rank", "Rank");
     dataGridViewHeroes.Columns.Add("ThreatLevel", "Threat Level");

     foreach (var h in heroes)
     {
         dataGridViewHeroes.Rows.Add(h); //add each hero in a new line each time in DatGridView
     }
 }
 catch (Exception ex) //error handling to ensure an error message shows when something goes wrong
 {
     MessageBox.Show("Error reading file: " + ex.Message);
 }
}
}
