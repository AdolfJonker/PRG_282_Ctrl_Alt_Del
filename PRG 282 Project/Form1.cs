using System;
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
                private void btnAddHero_Click(object sender, EventArgs e)
{
    try
    {
        // Call logic class
        AddHero.AddNewHero(
            txtHeroID.Text,
            txtName.Text,
            txtAge.Text,
            txtSuperpower.Text,
            txtScore.Text
        );

        MessageBox.Show("Superhero added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ClearFields();
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

// 🧹 Clear inputs
private void ClearFields()
{
    txtHeroID.Clear();
    txtName.Clear();
    txtAge.Clear();
    txtSuperpower.Clear();
    txtExamScore.Clear();
}
        }
    }
}
