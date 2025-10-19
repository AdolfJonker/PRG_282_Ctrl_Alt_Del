using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using PRG_282_Project.Presentation_Layer;
using PRG_282_Project.Data_Layer;
using PRG_282_Project.Business_Layer;
using PRG_282_Project.Classes;

namespace PRG_282_Project
{
    public partial class Form1 : Form
    {
        private BindingSource bindingSource;

        public Form1()
        {
            InitializeComponent();
            bindingSource = new BindingSource();
            dgvSuperheroes.DataSource = bindingSource;
            dgvSuperheroes.SelectionChanged += dgvSuperheroes_SelectionChanged; // Add selection event
        }

        private void dgvSuperheroes_SelectionChanged(object sender, EventArgs e)
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
                Add.AddNewHero(
                    txtHeroID.Text,
                    txtName.Text,
                    txtAge.Text,
                    txtSuperpower.Text,
                    txtScore.Text
                );

                MessageBox.Show("Superhero added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                btnViewAll_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtHeroID.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            txtSuperpower.Text = "";
            txtScore.Text = "";
            txtRank.Text = "";
            txtThreatLevel.Text = "";
        }

        private void btnViewAll_Click(object sender, EventArgs e)
        {
            try
            {
                var fmt = new FormatHandler();
                var heroes = fmt.FormatData();
                bindingSource.DataSource = heroes;

                // Fix DataPropertyName to match Superhero properties
                colHeroID.DataPropertyName = "heroID";
                colName.DataPropertyName = "name";
                colAge.DataPropertyName = "age";
                colSuperpower.DataPropertyName = "superpower";
                colExamScore.DataPropertyName = "score";
                colRank.DataPropertyName = "rank";
                colThreatLevel.DataPropertyName = "threatLevel";

                if (heroes.Count == 0)
                {
                    MessageBox.Show("No superheroes found in the data file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dgvSuperheroes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying superheroes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateHero_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHeroID.Text))
            {
                MessageBox.Show("Please select a hero (HeroID is required).",
                    "Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var updated = new Superhero(
                heroID: txtHeroID.Text.Trim(),
                name: txtName.Text.Trim(),
                age: txtAge.Text.Trim(),
                superpower: txtSuperpower.Text.Trim(),
                score: txtScore.Text.Trim(),
                rank: txtRank.Text.Trim(),
                threatLevel: txtThreatLevel.Text.Trim()
            );

            try
            {
                var mgr = new Update.SuperheroUpdateManager();
                bool ok = mgr.UpdateHero(updated);

                if (!ok)
                {
                    MessageBox.Show("Update failed: HeroID not found in file.",
                        "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                btnViewAll_Click(null, EventArgs.Empty);
                foreach (DataGridViewRow row in dgvSuperheroes.Rows)
                {
                    if (row.IsNewRow) continue;
                    if (string.Equals(row.Cells["colHeroID"].Value?.ToString(), updated.heroID, StringComparison.OrdinalIgnoreCase))
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
                string heroID = dgvSuperheroes.SelectedRows[0].Cells["colHeroID"].Value.ToString();

                bool deleted = Delete.DeleteByHeroID(heroID);

                if (deleted)
                {
                    MessageBox.Show("Superhero deleted successfully.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnViewAll_Click(null, null);
                    ClearFields();
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
                var summary = new Summary();
                var result = summary.Generate();

                // Populate summary textboxes
                textBox7.Text = result.TotalHeroes.ToString();
                textBox9.Text = result.AverageAge.ToString("0.00");
                textBox10.Text = result.AverageExamScore.ToString("0.00");
                textBox11.Text = result.CountS.ToString();
                textBox12.Text = result.CountA.ToString();
                textBox13.Text = result.CountB.ToString();
                textBox14.Text = result.CountC.ToString();

                var preview =
$@"Summary generated and saved to {AppConfig.SummaryFilePath}

Total heroes    : {result.TotalHeroes}
Average age     : {result.AverageAge:0.00}
Average score   : {result.AverageExamScore:0.00}
Ranks ->  S:{result.CountS}  A:{result.CountA}  B:{result.CountB}  C:{result.CountC}";

                MessageBox.Show(preview, "Summary Report", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to generate summary: " + ex.Message,
                    "Summary Report", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}