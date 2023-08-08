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
    public partial class FormAddString : Form
    {
        Index MainForm;

        public FormAddString(Index _MainForm)
        {
            InitializeComponent();

            MainForm = _MainForm;
        }

        private void FormAddString_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string[] lines = System.Text.RegularExpressions.Regex.Split(MainForm.contents.Text, "\r\n");
                MainForm.contents.Text = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i] != "" || (lines[i] == "" && checkBox1.Checked == true))
                    {
                        if(textBox1.Text == "first")
                            MainForm.contents.AppendText(textBox3.Text + lines[i] + "\r\n");
                        else if(textBox1.Text == "last")
                            MainForm.contents.AppendText(lines[i] + textBox3.Text + "\r\n");
                        else if (textBox1.Text == "center")
                            MainForm.contents.AppendText(lines[i] + "\r\n");
                        else if (textBox1.Text == "condition")
                            MainForm.contents.AppendText(lines[i] + "\r\n");
                    }
                    else
                    {
                        MainForm.contents.AppendText("\r\n");
                    }
                }
            }
            else
            {
                MessageBox.Show("위치: first, last, center, condition");
            }
        }
    }
}
