using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TI_Lab3
{
    public partial class MainForm : Form
    {
        private int p;
        private int g;
        private int x;
        private int y;
        private int k;
        private int a;

        private List<int> primitiveRoots = new List<int>();
        private List<int> cipNums = new List<int>();
        private List<int> resNums = new List<int>();
        private List<int> b = new List<int>();

        private FileManager fileManager = new FileManager();

        public MainForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            radioC.BackColor = Color.LightGreen;
            radioD.BackColor = Color.White;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите выйти?", "Выход",
                             MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateButtonStyle(sender as Button, FontStyle.Bold | FontStyle.Underline);
        }

        private void button_MouseLeave(object sender, EventArgs e)
        {
            UpdateButtonStyle(sender as Button, FontStyle.Regular);
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            radioC.BackColor = (sender == radioC) ? Color.LightGreen : Color.White;
            radioD.BackColor = (sender == radioD) ? Color.LightGreen : Color.White;

            textBoxAllG.Visible = radioC.Checked;
            textBoxA.Visible = radioC.Checked;
            textBoxB.Visible = radioC.Checked;
            textBoxG.Visible = radioC.Checked;
            textBoxInputK.Visible = radioC.Checked;
            textBoxY.Visible = radioC.Checked;

            buttonDisG.Visible = radioC.Checked;

            labelAllG.Visible = radioC.Checked;
            labelA.Visible = radioC.Checked;
            labelB.Visible = radioC.Checked;
            labelG.Visible = radioC.Checked;
            labelK.Visible = radioC.Checked;
            labelY.Visible = radioC.Checked;

            ClearAll();
        }

        private void ClearAll()
        {
            textBoxInputK.Clear();
            textBoxG.Clear();
            textBoxAllG.Clear();
            textBoxDataInput.Clear();
            textBoxOutputData.Clear();
            textBoxA.Clear();
            textBoxY.Clear();
            textBoxB.Clear();

            cipNums.Clear();
            resNums.Clear();
            primitiveRoots.Clear();
            b.Clear();

            x = y = k = g = a = 0;
        }

        private void DisplayPrimitiveRoots(int prime)
        {
            primitiveRoots = Elgamal.FindPrimitiveRoots(prime);
            textBoxAllG.Text = string.Join(" ", primitiveRoots);
        }

        private void ProcessEncryption()
        {
            b.Clear();
            a = Elgamal.ModPow(g, k, p);

            var resultBuilder = new StringBuilder();
            var str = new StringBuilder();

            foreach (var num in cipNums)
            {
                int encryptedValue = (Elgamal.ModPow(y, k, p) * num) % p;

                b.Add(encryptedValue);

                resultBuilder.Append(a + " " + encryptedValue + " ");
                str.Append(encryptedValue + " ");

                resNums.Add(a);
                resNums.Add(encryptedValue);
            }

            textBoxOutputData.Text = resultBuilder.ToString();
            textBoxA.Text = a.ToString();
            textBoxB.Text = str.ToString();
            textBoxY.Text = y.ToString();

            str.Clear();
            resultBuilder.Clear();
        }

        private void ProcessDecryption()
        {
            var resultBuilder = new StringBuilder();
            resNums.Clear();

            for (int i = 0; i < cipNums.Count; i += 2)
            {
                int decryptedValue = Elgamal.ComputeModularExpression(cipNums[i + 1], cipNums[i] , x, p);
                resultBuilder.Append(decryptedValue + " ");
                resNums.Add(decryptedValue);
            }

            textBoxOutputData.Text = resultBuilder.ToString();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void buttonRev_Click(object sender, EventArgs e)
        {
            textBoxDataInput.Text = textBoxOutputData.Text;
            textBoxOutputData.Clear();
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            if (radioC.Checked)
            {
                if (!ValidateInputsC()) {
                    ShowError("Заполните все поля!");
                    return; 
                }

                p = int.Parse(Elgamal.CheckForNum(textBoxInputP.Text));
                g = int.Parse(Elgamal.CheckForNum(textBoxG.Text));
                x = int.Parse(Elgamal.CheckForNum(textBoxInputX.Text));
                k = int.Parse(Elgamal.CheckForNum(textBoxInputK.Text));

                if (!Elgamal.IsPrime(p))
                {
                    ShowError($"Число {p} не является простым!");
                    return;
                }

                if (!Elgamal.IsPrimitiveRoot(g, p))
                {
                    ShowError($"Число {g} не является первообразным корнем по модулю {p}!");
                    return;
                }

                if (!Elgamal.CheckForX(x, p))
                {
                    ShowError("Некорректное значение X!");
                    return;
                }
                
                if (!Elgamal.CheckForK(k, p - 1))
                {
                    ShowError("Некорректное значение K!");
                    return;
                }

                y = Elgamal.ModPow(g, x, p);
                textBoxA.Text = y.ToString();

                ProcessEncryption();
            }
            else
            {
                if (!ValidateInputsD())
                {
                    ShowError("Заполните все поля!");
                    return;
                }

                p = int.Parse(Elgamal.CheckForNum(textBoxInputP.Text));
                x = int.Parse(Elgamal.CheckForNum(textBoxInputX.Text));

                if (!Elgamal.IsPrime(p))
                {
                    ShowError($"Число {p} не является простым!");
                    return;
                }

                if (!Elgamal.CheckForX(x, p))
                {
                    ShowError("Некорректное значение X или K!");
                    return;
                }

                ProcessDecryption();
            }
        }

        private void buttonDisG_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputP.Text))) return;

            p = int.Parse(Elgamal.CheckForNum(textBoxInputP.Text));

            if (!Elgamal.IsPrime(p))
            {
                ShowError($"Число {p} не является простым!");
                return;
            }

            DisplayPrimitiveRoots(p);
        }

        private void buttonReadData_Click(object sender, EventArgs e)
        {
            List<int> newNums = radioC.Checked
                ? fileManager.ReadBytesFromFile(openFileDialog)
                : fileManager.ReadDecFile(openFileDialog);

            if (newNums != null && newNums.Count > 0)
            {
                cipNums = newNums;
                textBoxDataInput.Text = string.Join(" ", cipNums);
                textBoxOutputData.Clear();
            }
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            if (radioC.Checked)
                fileManager.SaveToFile(saveFileDialog, resNums);
            else 
                fileManager.SaveBytesToFile(saveFileDialog, resNums);
        }

        private void UpdateButtonStyle(Button button, FontStyle style)
        {
            if (button != null)
            {
                button.Font = new Font(button.Font.FontFamily, 8.25f, style);
            }
        }

        private bool ValidateInputsC()
        {
            return !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputP.Text)) &&
                   !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxG.Text)) &&
                   !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputX.Text)) &&
                   !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputK.Text)) &&
                   !string.IsNullOrEmpty(textBoxDataInput.Text);
        }

        private bool ValidateInputsD()
        {
            return !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputP.Text)) &&
                   !string.IsNullOrEmpty(Elgamal.CheckForNum(textBoxInputX.Text)) &&
                   !string.IsNullOrEmpty(textBoxDataInput.Text);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}