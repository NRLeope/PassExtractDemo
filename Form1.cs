using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PassExtractDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        const int mb = 1024*1024*200;

        public void ReadFile(string filePath, long start = 0)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using(fileStream)
            {
                byte[] buffer = new byte[mb];
                fileStream.Seek(start, SeekOrigin.Begin);
                int bitRead = fileStream.Read(buffer, 0, mb);
                while(bitRead > 0)
                {
                    ProcessChunk(buffer, bitRead);
                    bitRead = fileStream.Read(buffer, 0, mb);
                }
            }
            richTextBox1.Enabled = true;
        }

        private void ProcessChunk(byte[] buffer, int bitRead)
        {
            string stringBits = Encoding.ASCII.GetString(buffer).Replace("\x00", string.Empty).Replace("\0", string.Empty).Replace("\n", string.Empty);
            string rValue = Regex.Replace(stringBits, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
            richTextBox1.AppendText(rValue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Search richTextbox text and highlight key word
            string[] words = textBox1.Text.Split(',');
            foreach (string word in words)
            {
                int startIndex = 0;
                while (startIndex < richTextBox1.TextLength)
                {
                    int wordStartIndex = richTextBox1.Find(word, startIndex, RichTextBoxFinds.None);
                    if (wordStartIndex != -1)
                    {
                        richTextBox1.SelectionStart = wordStartIndex;
                        richTextBox1.SelectionLength = word.Length;
                        richTextBox1.SelectionBackColor = Color.SkyBlue;
                    }
                    else
                        break;
                    startIndex += wordStartIndex + word.Length;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe");
            p.StartInfo.Arguments = "\select";
            p.Start();*/
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select File";
            openFileDialog1.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog1.Filter = "All files (*.*)|*.*|Text File (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.ShowDialog();

            if((myStream = openFileDialog1.OpenFile()) != null)
            {
                //Clear Textbox
                if(richTextBox1.Text != null)
                {
                    richTextBox1.Clear();
                }

                //Read File and Process into richTextBox1
                ReadFile(openFileDialog1.FileName);
                /*string fileName = openFileDialog1.FileName;
                string fileText = File.ReadAllText(fileName);
                richTextBox1.Text = fileText;*/
            }

            /*
            if (openFileDialog1.FileName != "")
            { textBox1.Text = openFileDialog1.FileName; }
            else
            { textBox1.Text = "You didn't select the file!"; }*/

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Clear Higjlights
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
        }
    }

}
