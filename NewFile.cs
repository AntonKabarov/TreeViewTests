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

namespace TreeViewTests
{
    public partial class NewFile : Form
    {   
        
        
        private string pathsfile;

        private Form1 form1;
        public NewFile(string Pathsfile, Form1 form1)
        {
            InitializeComponent();
            this.pathsfile = Pathsfile;
            this.form1 = form1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FileNameNew = textBox1.Text;

            string FilePathNew = Path.Combine(pathsfile, FileNameNew);
            
            
            if (!IsValidFileName(FileNameNew))
            {
                MessageBox.Show("Имя файла не может содержать ни одного из следующих символов:\r\n" + "\t\\/:*?\"<>|", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (File.Exists(FilePathNew))
            {
                MessageBox.Show("В текущем пути есть файл с таким же именем!","Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (File.Create(FilePathNew)) 
                           
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private bool IsValidFileName(string fileName)
        {
            bool isValid = true;

            string errChar = "\\/:*?\"<>|";

            for (int i = 0; i < errChar.Length; i++)
            {
                if (fileName.Contains(errChar[i].ToString()))
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }

     
    }
}
