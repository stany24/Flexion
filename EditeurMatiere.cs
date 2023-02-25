﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flexion
{
    public partial class EditeurMatiere : Form
    {
        public List<Matiere> ListMatieres;
        public Form1 Main;
        public EditeurMatiere(List<Matiere> listmatiere, Form1 main)
        {
            InitializeComponent();
            Main = main;
            ListMatieres = listmatiere;
            cbxMatieres.DataSource = ListMatieres;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Matiere selected = cbxMatieres.SelectedItem as Matiere;
            if (selected == null){ return; }
            tbxNomMatiere.Text = selected.GetNom();
            nudE.Value = (decimal)selected.GetE();
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            Matiere selected = cbxMatieres.SelectedItem as Matiere;
            selected.SetNom(tbxNomMatiere.Text);
            selected.SetE((double)nudE.Value);
            cbxMatieres.DataSource = null;
            cbxMatieres.DataSource = ListMatieres;
        }

        private void EditeurMatiere_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.ListMatieres = ListMatieres;
            Main.Show();
        }
    }
}
