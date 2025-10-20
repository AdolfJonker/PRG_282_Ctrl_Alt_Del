using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using PRG_282_Project;
using PRG_282_Project.Data_Layer;
using PRG_282_Project.Business_Layer;

namespace PRG_282_Project
{
    public partial class Form1 : Form
    {
        private readonly BusinessLogic businessLogic;

        public Form1()
        {
            InitializeComponent();
            businessLogic = new BusinessLogic();
            LoadSuperheroes();

        }
        private void LoadSuperheroes()
        {
            try
            {
                var superheroes = businessLogic.GetAllSuperheroes();
                dgvSuperheroes.Rows.Clear();
                foreach (var hero in superheroes)
                {
                    dgvSuperheroes.Rows.Add(hero.HeroID, hero.Name, hero.Age, hero.Superpower, hero.ExamScore, hero.Rank, hero.ThreatLevel);
                }
                if (dgvSuperheroes.SelectedRows.Count > 0)
                {
                    UpdateFieldsFromSelectedRow();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateFieldsFromSelectedRow()
        {
            if (dgvSuperheroes.SelectedRows.Count > 0)
            {
                var row = dgvSuperheroes.SelectedRows[0];
                txtHeroID.Text = row.Cells["colHeroID"].Value?.ToString() ?? "";
                txtName.Text = row.Cells["colName"].Value?.ToString() ?? "";
                txtAge.Text = row.Cells["colAge"].Value?.ToString() ?? "";
                txtSuperpower.Text = row.Cells["colSuperpower"].Value?.ToString() ?? "";
                txtScore.Text = row.Cells["colExamScore"].Value?.ToString() ?? "";
                txtRank.Text = row.Cells["colRank"].Value?.ToString() ?? "";
                txtThreatLevel.Text = row.Cells["colThreatLevel"].Value?.ToString() ?? "";
            }
        }

        private void btnAddHero_Click_1(object sender, EventArgs e)
        {
            try
            {
                var hero = new Superhero
                {
                    HeroID = txtHeroID.Text,
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Superpower = txtSuperpower.Text,
                    ExamScore = double.Parse(txtScore.Text)
                };
                businessLogic.AddSuperhero(hero);
                LoadSuperheroes();
                ClearFields();
                MessageBox.Show("Superhero added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            LoadSuperheroes();
        }

        private void btnUpdateHero_Click(object sender, EventArgs e)
        {
            try
            {
                var hero = new Superhero
                {
                    HeroID = txtHeroID.Text,
                    Name = txtName.Text,
                    Age = int.Parse(txtAge.Text),
                    Superpower = txtSuperpower.Text,
                    ExamScore = double.Parse(txtScore.Text)
                };
                businessLogic.UpdateSuperhero(hero);
                LoadSuperheroes();
                MessageBox.Show("Superhero updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dgvSuperheroes.SelectedRows.Count > 0)
            {
                string heroID = dgvSuperheroes.SelectedRows[0].Cells["colHeroID"].Value.ToString();
                if (MessageBox.Show($"Are you sure you want to delete Hero ID: {heroID}?", "Confirm Delete",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        businessLogic.DeleteSuperhero(heroID);
                        LoadSuperheroes();
                        ClearFields();
                        MessageBox.Show("Superhero deleted successfully.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a superhero to delete.");
            }
        }

        private void btnSummaryReport_Click(object sender, EventArgs e)
        {
            try
            {
                var (total, avgAge, avgScore, sCount, aCount, bCount, cCount) = businessLogic.GenerateSummary();
                textBox7.Text = total.ToString();
                textBox9.Text = avgAge.ToString("F2");
                textBox10.Text = avgScore.ToString("F2"); 
                textBox11.Text = sCount.ToString(); 
                textBox12.Text = aCount.ToString();
                textBox13.Text = bCount.ToString();
                textBox14.Text = cCount.ToString(); 
                businessLogic.SaveSummary(total, avgAge, avgScore, sCount, aCount, bCount, cCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtHeroID.Clear();
            txtName.Clear();
            txtAge.Clear();
            txtSuperpower.Clear();
            txtScore.Clear();
            txtRank.Clear();
            txtThreatLevel.Clear();
        }
    }
}
    
