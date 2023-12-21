using System;
using System.Windows.Forms;

namespace Program15
{
    public partial class Form1 : Form
    {
        Kripto kripto = new Kripto();
        ProgramCommander programCommander;
        public Form1()
        {
            InitializeComponent();
            programCommander = new ProgramCommander(kripto, textBox1, textBox2, checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6);
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
        }
        private void button1_Click(object sender, EventArgs e) //шифровать
        {
            kripto.SetKeyword(textBox1.Text);
            kripto.SetMessage(textBox2.Text);

            programCommander.CheckBoxesEncrypt();

            textBox3.Text = kripto.GetResult();
        }
        private void button2_Click(object sender, EventArgs e) //дешифровать
        {
            kripto.SetKeyword(textBox1.Text);
            kripto.SetMessage(textBox2.Text);

            programCommander.CheckBoxesDecrypt();

            textBox3.Text = kripto.GetResult();
        }

        private void button3_Click(object sender, EventArgs e) //справка
        {
            textBox4.Text = programCommander.PrintFile();
        }

        private void button4_Click(object sender, EventArgs e) // выход
        {
            Application.Exit();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Kirikulus");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("В будущих обновлениях будет добавлен английский язык к шифру Слоганом, также будут добавлены современные алгоритмы шифрования ");
        }
        private void checkBox6_CheckedChanged_1(object sender, EventArgs e)
        {
            CheckBox currentCheckBox = (CheckBox)sender;

            if (currentCheckBox.Checked)
            {
                foreach (Control control in Controls)
                {
                    if (control is CheckBox checkBox && checkBox != currentCheckBox)
                    {
                        checkBox.Enabled = false;
                    }
                }
            }
            else
            {
                foreach (Control control in Controls)
                {
                    if (control is CheckBox checkBox)
                    {
                        checkBox.Enabled = true;
                    }
                }
            }
        }
    }
}