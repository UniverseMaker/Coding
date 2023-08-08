using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Coding
{
    public partial class Index : Form
    {
        string key = "";
        FormReplace formReplace;
        FormAddString formAddString;
        FormMergeString formMergeString;
        FormSplitString formSplitString;

        public Index()
        {
            InitializeComponent();

            formReplace = new FormReplace(this);
            formAddString = new FormAddString(this);
            formMergeString = new FormMergeString(this);
            formSplitString = new FormSplitString(this);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        #region BASE64
        public static string Base64Encode(string src, System.Text.Encoding enc)
        {
            byte[] arr = enc.GetBytes(src);
            return Convert.ToBase64String(arr);
        }

        public static string Base64Decode(string src, System.Text.Encoding enc)
        {
            string s = src.Trim().Replace(" ", "+");
            if (s.Length % 4 > 0)
                s = s.PadRight(s.Length + 4 - s.Length % 4, '=');

            byte[] arr = System.Convert.FromBase64String(s);
            return enc.GetString(arr);
        }
        #endregion

        #region MD5
        static string MD5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
        #endregion

        #region Menu
        private void upperStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = contents.Text.ToString().ToUpper();
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void lowerStringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = contents.Text.ToString().ToLower();
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }
        
        private void topMOSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost == true)
            {
                this.TopMost = false;
            }
            else
            {
                this.TopMost = true;
            }
        }

        private void gETTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime defaultDate = DateTime.Parse("1970-01-01 00:00:00");
                DateTime koreaDate = DateTime.Parse("1970-01-01 09:00:00");
                DateTime targetDate = DateTime.Parse(contents.Text);
                TimeSpan ts = new TimeSpan(targetDate.Ticks - defaultDate.Ticks);
                TimeSpan ts_k = new TimeSpan(targetDate.Ticks - koreaDate.Ticks);

                contents.Text = "GMT 00:00 : " + ts.TotalSeconds.ToString() + "\r\nGMT +09:00 : " + ts_k.TotalSeconds.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void hELPTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.AppendText("아래 형식대로 입력 후 Get Time Stamp를 하세요\r\n1970-01-01 09:00:00\r\n단, 이곳에 날짜 내용만이 존재 해야 합니다");
        }

        private void nowTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.AppendText(DateTime.Now.ToString());
        }

        private void rETURNTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime defaultDate = DateTime.Parse("1970-01-01 00:00:00");
                DateTime koreaDate = DateTime.Parse("1970-01-01 09:00:00");

                contents.Text = "GMT 00:00 : " + defaultDate.AddSeconds(Convert.ToInt64(contents.Text)) + "\r\nGMT +09:00 : " + koreaDate.AddSeconds(Convert.ToInt64(contents.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }
        #endregion

        #region Plugin
        private void fileMD5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                byte[] md5byte;
                StringBuilder result = new StringBuilder();

                //OpenFIleDialog로 선택 한 파일을
                //FileInfo 클래스로 파일정보을 얻음
                //얻은 파일정보를 이용하여 FileStream 생성
                FileInfo fi = new FileInfo(openFileDialog1.FileName);
                string filepath = fi.FullName;
                FileStream fs = new FileStream(filepath, FileMode.Open);

                //Stream을 md5로 반환하는
                //MD5CryptoServiceProvider 의 ComputeHash 함수 이용
                md5byte = (new MD5CryptoServiceProvider()).ComputeHash(fs);
                fs.Close();

                //byte 값들을 string으로 변환
                for (int i = 0; i < md5byte.Length; i++)
                    result.Append(md5byte[i].ToString("X2"));

                contents.Text = result.ToString();
            }
        }

        private void findTimeStampToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] data = contents.Text.Split(Convert.ToChar("/"));
                Int64 target = Convert.ToInt32(data[0]);
                string md5target = data[1];
                string result = "";
                string temp = "";

                DateTime defaultDate = DateTime.Parse("1970-01-01 00:00:00");
                Int64 nowts = Convert.ToInt64(new TimeSpan(DateTime.Now.Ticks - defaultDate.Ticks).TotalSeconds);
                

                while (target <= nowts)
                {
                    TimeSpan ts = new TimeSpan(target - defaultDate.Ticks);

                    temp = MD5Hash(ts.TotalSeconds.ToString());
                    contents.Text = target + ">" + temp;

                    if (temp == md5target)
                    {
                        result = target.ToString();
                        break;
                    }
                    target++;
                    Application.DoEvents();
                }

                if (result != "")
                {
                    contents.Text = "Found! : " + result;
                }
                else
                {
                    contents.Text = "Not Found";
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void findTimeStampBackTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string md5target = contents.Text;
                string result = "";
                string temp = "";

                DateTime defaultDate = DateTime.Parse("1970-01-01 00:00:00");
                DateTime koreaDate = DateTime.Parse("1970-01-01 09:00:00");
                Int64 nowts = Convert.ToInt64(new TimeSpan(DateTime.Now.Ticks - defaultDate.Ticks).TotalSeconds);
                
                

                while (0 <= nowts)
                {
                    temp = MD5Hash(nowts.ToString());
                    contents.Text = "GMT 00:00 : " + defaultDate.AddSeconds(nowts) + "\r\nGMT +09:00 : " + koreaDate.AddSeconds(nowts) + "\r\n>" + temp;

                    if (temp == md5target)
                    {
                        result = nowts.ToString();
                        break;
                    }
                    nowts--;
                    Application.DoEvents();
                }

                if (result != "")
                {
                    contents.Text = "Found! : " + result;
                }
                else
                {
                    contents.Text = "Not Found";
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void javascriptUnescapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.Text = Microsoft.JScript.GlobalObject.unescape(contents.Text);
        }
        #endregion

        private void urlenc_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = System.Web.HttpUtility.UrlEncode(contents.Text, Encoding.GetEncoding(type.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void urldec_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = System.Web.HttpUtility.UrlDecode(contents.Text, Encoding.GetEncoding(type.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void base64enc_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = Base64Encode(contents.Text, Encoding.GetEncoding(type.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void base64dec_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = Base64Decode(contents.Text, Encoding.GetEncoding(type.Text));
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void md5hash_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = MD5Hash(contents.Text);
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void staticUTF16EncodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                contents.Text = System.Web.HttpUtility.UrlEncode(contents.Text, Encoding.UTF32);
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void contents_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            //{
            //    var clipboard = Clipboard.GetData(DataFormats.Text).ToString();
            //    contents.SelectedText = clipboard;
            //}
            //else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.C)
            //{
            //    Clipboard.SetText(contents.SelectedText, TextDataFormat.Text);
            //}
            //else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            //{
            //    contents.SelectAll();
            //}
        }

        private void contents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                contents.SelectAll();
            }
        }

        private void makeKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (bit.Text == "")
                {
                    MessageBox.Show("BIT LENGTH를 입력하세요");
                    return;
                }

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(Convert.ToInt32(bit.Text));
                string puk = rsa.ToXmlString(false);    //public key
                string prk = rsa.ToXmlString(true);     //private key

                try
                {
                    saveFileDialog1.Filter = "PUK 파일|*.puk";
                    saveFileDialog1.FileName = "공개키 파일명";
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (saveFileDialog1.FileName == "")
                    {
                        MessageBox.Show("파일을 선택하세요");
                        return;
                    }

                    FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                    //this.ID는 파일명입니다. 나머지두개는 상속임. 속성에 관련된것은 MSDN 참고하세열 ㅋ
                    fs2.Close();

                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false);
                    sw.WriteLine(puk);

                    sw.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Error : PUK SAVE " + ee.Message);
                }

                try
                {
                    saveFileDialog1.Filter = "PRK 파일|*.prk";
                    saveFileDialog1.FileName = "개인키 파일명";
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    if (saveFileDialog1.FileName == "")
                    {
                        MessageBox.Show("파일을 선택하세요");
                        return;
                    }

                    FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                    //this.ID는 파일명입니다. 나머지두개는 상속임. 속성에 관련된것은 MSDN 참고하세열 ㅋ
                    fs2.Close();

                    StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, false);
                    sw.WriteLine(prk);

                    sw.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Error : PRK SAVE " + ee.Message);
                }
            }
            catch (Exception me)
            {
                MessageBox.Show("Error : KEY MAKE " + me.Message);
            }
        }

        private void openKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Random rd = new Random();
                string ImportTemp;

                openFileDialog1.Filter = "PUK 파일|*.puk|PRK 파일|*.prk";
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                if (openFileDialog1.FileName == "")
                {
                    MessageBox.Show("파일을 선택하세요");
                    return;
                }

                FileStream fs2 = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Read);
                //this.ID는 파일명입니다. 나머지두개는 상속임. 속성에 관련된것은 MSDN 참고하세열 ㅋ
                StreamReader st = new StreamReader(fs2, System.Text.Encoding.UTF8);
                //단순한 스트림 리더. 스트림리더를 인코딩을 UTF8형식으로 듬.
                st.BaseStream.Seek(0, SeekOrigin.Begin);
                //어디부터 읽을지. 처음부터면 begin

                key = "";
                while (st.Peek() > -1)
                {
                    //한 줄씩 읽어와서 처리..
                    ImportTemp = st.ReadLine();
                    key += ImportTemp;
                }

                st.Close();
                fs2.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error : CONTENTS OPEN " + ee.Message);
            }

        }

        private void encryptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (key == "")
                {
                    MessageBox.Show("키가 입력되지 않았습니다");
                    return;
                }
                if (contents.Text == "")
                {
                    MessageBox.Show("내용을 입력하세요");
                    return;
                }

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(key);

                contents.Text = System.Convert.ToBase64String(rsa.Encrypt(System.Text.Encoding.UTF8.GetBytes(contents.Text), true));
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error : ENCRYPT " + ee.Message);
            }
        }

        private void decryptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (key == "")
                {
                    MessageBox.Show("키가 입력되지 않았습니다");
                    return;
                }
                if (contents.Text == "")
                {
                    MessageBox.Show("내용을 입력하세요");
                    return;
                }

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(key);

                contents.Text = System.Text.Encoding.UTF8.GetString(rsa.Decrypt(System.Convert.FromBase64String(contents.Text), true));
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error : DECRYPT " + ee.Message);
            }
        }

        private void dateTimeTickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime defaultDate = DateTime.Parse("1970-01-01 00:00:00");
                DateTime koreaDate = DateTime.Parse("1970-01-01 09:00:00");
                DateTime targetDate = DateTime.Parse(contents.Text);
                TimeSpan ts = new TimeSpan(targetDate.Ticks - defaultDate.Ticks);
                TimeSpan ts_k = new TimeSpan(targetDate.Ticks - koreaDate.Ticks);

                contents.Text = "Tick : " + targetDate.Ticks.ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show("ERROR : " + ee.Message);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string t = contents.Text;

            contents.Text = System.Text.RegularExpressions.Regex.Unescape(t);

        }

        private void unicodeDecodingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.Text = System.Text.RegularExpressions.Regex.Unescape(contents.Text);
        }

        private void redEyeEngineHttpSendConvertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string backup = contents.Text;
            string[] tt = System.Text.RegularExpressions.Regex.Split(contents.Text, "\r\n");
            string[] tt2 = System.Text.RegularExpressions.Regex.Split(tt[0], " ");

            contents.Clear();
            contents.AppendText("RedEyeEngine.Engine Engine = new RedEyeEngine.Engine();" + "\r\n");
            contents.AppendText("List<string> Header = new List<string>();" + "\r\n\r\n");


            if (tt2[0] == "POST")
            {
                for (int i = 1; i < tt.Count()-1; i++)
                {
                    if (tt[i] != "")
                        contents.AppendText("Header.Add(\"" + tt[i] + "\");" + "\r\n");
                }

                contents.AppendText("\r\nEngine.HttpSend(\"ALL\", \"utf-8\", \"POST\", \"" + tt2[1] + "\", Header, new StringBuilder(\"" + tt[tt.Count()-1] + "\"), PROXY, 0);" + "\r\n");
            }
            else if (tt2[0] == "GET")
            {
                for (int i = 1; i < tt.Count(); i++)
                {
                    if(tt[i] != "")
                        contents.AppendText("Header.Add(\"" + tt[i] + "\");" + "\r\n");
                }

                contents.AppendText("Engine.HttpSend(\"ALL\", \"utf-8\", \"GET\", \"" + tt2[1] + "\", Header, new StringBuilder(\"\"), PROXY, 0);" + "\r\n");
            }
            else
            {
                contents.Text = backup;
                MessageBox.Show("형식오류");
            }
        }

        private void parameterMatchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contents.Text.Substring(0, 1) != "_")
            {
                string[] lines = System.Text.RegularExpressions.Regex.Split(contents.Text, "\r\n");
                contents.Text = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i] != "")
                        contents.AppendText(lines[i] + " = _" + lines[i] + ";\r\n");
                }
            }
            else
            {
                string[] lines = System.Text.RegularExpressions.Regex.Split(contents.Text, "\r\n");
                contents.Text = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i] != "")
                        contents.AppendText(lines[i] + " = " + lines[i].Substring(1) + ";\r\n");
                }
            }
        }

        private void getParameterNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] lines = System.Text.RegularExpressions.Regex.Split(contents.Text, "\r\n");
            contents.Text = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i] != "")
                {
                    string[] line = System.Text.RegularExpressions.Regex.Split(lines[i], "=");
                    contents.AppendText(line[0].TrimEnd() + "\r\n");
                }
            }
        }

        private void 치환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formReplace.Show();
        }

        private void 문자열추가ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAddString.Show();
        }

        private void 문자열병합ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formMergeString.Show();
        }

        private void 문자열분할ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formSplitString.Show();
        }

        private void transformHttpbodyToParameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.Text = contents.Text.Replace("\r\n", "\\r\\n").Replace("\"", "\\\"");
        }

        private void transformParameterToHttpbodyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            contents.Text = contents.Text.Replace("\\r\\n", "\r\n").Replace("\\\"", "\"");
        }
        
    }
}
