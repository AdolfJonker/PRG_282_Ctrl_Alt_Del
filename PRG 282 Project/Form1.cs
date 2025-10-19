using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRG_282_Project.Presentation_Layer;
using PRG_282_Project.Data_Layer;
using PRG_282_Project.Business_Layer;

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

        private void btnUpdateHero_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHeroID.Text))
            {
                MessageBox.Show("Please select a hero (HeroID is required).",
                    "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use the Superhero class fields/properties directly
            var updated = new PRG_282_Project.Business_Layer.Superhero(
                heroID: txtHeroID.Text.Trim(),
                name: txtName.Text.Trim(),
                age: txtAge.Text.Trim(),
                superpower: txtSuperpower.Text.Trim(),
                score: txtScore.Text.Trim()
            );

            try
            {
                var mgr = new PRG_282_Project.Classes.Update.SuperheroUpdateManager();
                bool ok = mgr.UpdateHero(updated);

                if (!ok)
                {
                    MessageBox.Show("Update failed: HeroID not found in file.",
                        "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Refresh view and reselect
                btnViewAll_Click(null, EventArgs.Empty);
                foreach (DataGridViewRow row in dgvSuperheroes.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (string.Equals(row.Cells[0]?.Value?.ToString(), updated.heroID, StringComparison.OrdinalIgnoreCase))
                    {
                        row.Selected = true;
                        dgvSuperheroes.CurrentCell = row.Cells[0];
                        dgvSuperheroes.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }

                MessageBox.Show("Superhero updated successfully.",
                    "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating: " + ex.Message,
                    "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeleteHero_Click(object sender, EventArgs e)
        {
            if (dgvSuperheroes.SelectedRows.Count > 0)
            {
                // Assuming HeroID is in the first column
                string heroID = dgvSuperheroes.SelectedRows[0].Cells[0].Value.ToString();

                bool deleted = PRG_282_Project.Classes.Delete.DeleteByHeroID(heroID);

                if (deleted)
                {
                    MessageBox.Show("Superhero deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnViewAll_Click(null, null); // Refresh the grid
                }
                else
                {
                    MessageBox.Show("Superhero not found.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a superhero to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSummaryReport_Click(object sender, EventArgs e) 
        {
            try
            {
                var summary = new PRG_282_Project.Classes.Summary(); // writes summary.txt by default
                var result = summary.Generate();                    // compute + persist

                var preview =
               $@"Summary generated and saved to summary.txt

                 Total heroes    : {result.TotalHeroes}
                 Average age     : {result.AverageAge:0.00}
                 Average score   : {result.AverageExamScore:0.00}
                 Ranks ->  S:{result.CountS}  A:{result.CountA}  B:{result.CountB}  C:{result.CountC}";

                  MessageBox.Show(preview, "Summary Report",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate summary: " + ex.Message,
                    "Summary Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

     
    }
 }


