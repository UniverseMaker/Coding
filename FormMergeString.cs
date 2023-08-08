using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Coding
{
    public partial class FormMergeString : Form
    {
        Index MainForm;
        
        public FormMergeString(Index _MainForm)
        {
            InitializeComponent();

            MainForm = _MainForm;
        }

        private void FormMergeString_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void linematching_Click(object sender, EventArgs e)
        {
            string[] lines1 = System.Text.RegularExpressions.Regex.Split(textBox1.Text, "\r\n");
            string[] lines2 = System.Text.RegularExpressions.Regex.Split(textBox2.Text, "\r\n");

            textBox3.Text = "";
            for (int i = 0; i < lines1.Count(); i++)
            {
                if (i >= lines2.Count())
                    break;

                textBox3.AppendText(lines1[i] + lines2[i] + "\r\n");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                textBox1.SelectAll();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                textBox2.SelectAll();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                textBox3.SelectAll();
            }
        }
    }
}
