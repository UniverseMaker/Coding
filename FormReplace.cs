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
    public partial class FormReplace : Form
    {
        Index MainForm;

        public FormReplace(Index _MainForm)
        {
            InitializeComponent();

            MainForm = _MainForm;
        }

        private void FormReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            string target = textBox1.Text;
            string result = textBox2.Text;

            target = target.Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");
            result = result.Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\t", "\t");

            MainForm.contents.Text = MainForm.contents.Text.Replace(target, result);
        }


    }
}
