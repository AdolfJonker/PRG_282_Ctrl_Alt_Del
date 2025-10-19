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

namespace PRG_282_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
                //private void btnAddHero_Click(object sender, EventArgs e)
                //{
                //try
                //{
                //    // Call logic class
                //    AddHero.AddNewHero(
                //        txtHeroID.Text,
                //        txtName.Text,
                //        txtAge.Text,
                //        txtSuperpower.Text,
                //        txtScore.Text
                //    );

                //    MessageBox.Show("Superhero added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    ClearFields();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
                //}

                // 🧹 Clear inputs
                //private void ClearFields()
                //{
                //    txtHeroID.Clear();
                //    txtName.Clear();
                //    txtAge.Clear();
                //    txtSuperpower.Clear();
                //    txtExamScore.Clear();
                //}

        private void btnAddHero_Click_1(object sender, EventArgs e){}

        private void btnViewAll_Click(object sender, EventArgs e){}

        private void btnUpdateHero_Click(object sender, EventArgs e){ }

        private void btnDeleteHero_Click(object sender, EventArgs e){ }

        private void btnSummaryReport_Click(object sender, EventArgs e) { }

        private void LoadSuperheroes()
        {
            string filePath = "superheroes.txt";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("No data file found yet.");
                return;
            }

            dgvSuperheroes.Rows.Clear(); // Clear old data

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 7)
                {
                    dgvSuperheroes.Rows.Add(parts[0], parts[1], parts[2],
                                            parts[3], parts[4], parts[5], parts[6]);
                }
            }
        }
    }
 }

