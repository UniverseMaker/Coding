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
    public partial class FormSplitString : Form
    {
        Index MainForm;
        
        public FormSplitString(Index _MainForm)
        {
            InitializeComponent();

            MainForm = _MainForm;
        }

        private void FormSplitString_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            try
            {
                int errcount = 0;

                string[] lines = System.Text.RegularExpressions.Regex.Split(MainForm.contents.Text, "\r\n");
                MainForm.contents.Text = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    try
                    {
                        if (lines[i] != "")
                        {
                            string[] datas = System.Text.RegularExpressions.Regex.Split(lines[i], textBox1.Text);
                            MainForm.contents.AppendText(datas[Convert.ToInt32(textBox2.Text)] + "\r\n");
                        }
                        else
                        {
                            MainForm.contents.AppendText("\r\n");
                        }
                    }
                    catch
                    {
                        errcount++;
                    }
                }

                if (errcount > 0)
                    MessageBox.Show(errcount.ToString() + "개의 데이터 오류발생");
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }


    }
}
