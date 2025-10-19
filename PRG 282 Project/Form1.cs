using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRG_282_Project.Presentation_Layer;
using PRG_282_Project.Data_Layer;

namespace PRG_282_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAddHero_Click_1(object sender, EventArgs e){}

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            Display display = new Display();
             List<string[]> heroes = display.LoadSuperheroes();

             //clear datagrid
             dgvSuperheroes.Columns.Clear();
             dgvSuperheroes.Rows.Clear();

             //add in the headers
             dgvSuperheroes.Columns.Add("HeroID", "Hero ID");
             dgvSuperheroes.Columns.Add("Name", "Name");
             dgvSuperheroes.Columns.Add("Age", "Age");
             dgvSuperheroes.Columns.Add("Superpower", "Superpower");
             dgvSuperheroes.Columns.Add("ExamScore", "Exam Score");
             dgvSuperheroes.Columns.Add("Rank", "Rank");
             dgvSuperheroes.Columns.Add("ThreatLevel", "Threat Level");

             //adding each hero record from the list into the datagridview
             foreach (var hero in heroes)
             {
                 dgvSuperheroes.Rows.Add(hero);
             }
             //make columns fit into the grid
             dgvSuperheroes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnUpdateHero_Click(object sender, EventArgs e){ }

        private void btnDeleteHero_Click(object sender, EventArgs e){ }

        private void btnSummaryReport_Click(object sender, EventArgs e) { }

        //private void LoadSuperheroes()
        //{
        //    string filePath = "superheroes.txt";

        //    if (!File.Exists(filePath))
        //    {
        //        MessageBox.Show("No data file found yet.");
        //        return;
        //    }

        //    dgvSuperheroes.Rows.Clear(); // Clear old data

        //    string[] lines = File.ReadAllLines(filePath);
        //    foreach (string line in lines)
        //    {
        //        string[] parts = line.Split(',');
        //        if (parts.Length == 7)
        //        {
        //            dgvSuperheroes.Rows.Add(parts[0], parts[1], parts[2],
        //                                    parts[3], parts[4], parts[5], parts[6]);
        //        }
        //    }
        //}
    }
 }


