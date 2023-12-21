using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System;

namespace Program15
{
    internal class ProgramCommander : Form
    {
        Kripto kripto;
        CaesarCipher caesarCipher;
        SloganCipher sloganCipher;
        HillCipher hillCipher;
        GridPermutationCipher gridPermutationCipher;
        ScytaleCipher scytaleCipher;
        PlayfairCipher playfairCipher;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private CheckBox checkBox6;
        private TextBox textBox1;
        private TextBox textBox2;

        public ProgramCommander(Kripto kripto,TextBox textBox1, TextBox textBox2, CheckBox checkBox1, CheckBox checkBox2,CheckBox checkBox3, CheckBox checkBox4, CheckBox checkBox5, CheckBox checkBox6)
        {
            caesarCipher = new CaesarCipher(kripto);
            sloganCipher = new SloganCipher(kripto);
            gridPermutationCipher = new GridPermutationCipher(kripto);
            scytaleCipher = new ScytaleCipher(kripto);
            playfairCipher = new PlayfairCipher(kripto);
            hillCipher = new HillCipher(kripto);
            this.checkBox1 = checkBox1;
            this.checkBox2 = checkBox2;
            this.checkBox3 = checkBox3;
            this.checkBox4 = checkBox4;
            this.checkBox5 = checkBox5;
            this.checkBox6 = checkBox6;
            this.textBox1 = textBox1;
            this.textBox2 = textBox2;
            this.kripto = kripto;
        }
        public enum OperationType
        {
            Encrypt,
            Decrypt
        }
        public string ReadFile(string path)
        {
            string file = File.ReadAllText(path);
            return file;
        }
        public string PrintFile()
        {
            string file = ReadFile("help.txt");
            return file;
        }
        private bool IsNumericKey(string key)
        {
            return int.TryParse(key, out _);
        }

        private bool IsAlphabeticKey(string key)
        {
            return key.All(char.IsLetter);
        }
        private bool IsEnglish(string text)
        {
            return text.All(c => Kripto.EnglishAlphabet.Contains(char.ToUpper(c)) || char.IsWhiteSpace(c));
        }

        private bool IsRussian(string text)
        {
            return text.All(c => Kripto.RussianAlphabet.Contains(char.ToUpper(c)) || char.IsWhiteSpace(c));
        }
        public void PerformOperation(OperationType operationType)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                ShowError("Пожалуйста, введите ключ.");
                return;
            }

            // Словарь для связывания checkBox с проверками и операциями
            var cipherActions = new Dictionary<CheckBox, Action>
            {
                { checkBox1, () => ProcessCipher(caesarCipher, IsNumericKey, operationType, false) }, // Цезарь
                { checkBox2, () => ProcessCipher(sloganCipher, IsAlphabeticKey, operationType) },    // Слоган
                { checkBox3, () => ProcessCipher(playfairCipher, IsAlphabeticKey, operationType) },  // Плейфер
                { checkBox4, () => ProcessCipher(hillCipher, IsValidHillCipherKey, operationType, false) }, //Хилла
                { checkBox5, () => ProcessCipher(scytaleCipher, IsNumericKey, operationType, false) }, // Скиталы
                { checkBox6, () => ProcessCipher(gridPermutationCipher, IsAlphabeticKey, operationType, true, operationType == OperationType.Decrypt) }, // Горбунов
            };


            foreach (var kvp in cipherActions)
            {
                if (kvp.Key.Checked)
                {
                    kvp.Value.Invoke();
                    return;
                }
            }
        }
        private bool IsValidHillCipherKey(string key)
        {
            if (key.Length == 4 && key.All(char.IsDigit))
            {
                return true;
            }
            return false;
        }
        private bool IsValidDiameter(string key)
        {
            if (int.TryParse(key, out int diameter) && diameter > 0)
            {
                return true;
            }
            ShowError("Диаметр должен быть положительным числом.");
            return false;
        }

        private void ProcessCipher(ICipher cipher, Func<string, bool> keyValidation, OperationType operationType, bool checkLanguage = true, bool isNumericCipher = false)
        {
            if (!keyValidation(textBox1.Text))
            {
                ShowError("Неверный формат ключа для выбранного шифра.");
                return;
            }
            if (checkLanguage && !isNumericCipher && !IsMatchingLanguage(textBox1.Text, kripto.Message))
            {
                ShowError("Язык ключа и сообщения должен совпадать.");
                return;
            }
            if (cipher is ScytaleCipher && !IsValidDiameter(textBox1.Text))
            {
                return; 
            }
            if (cipher is HillCipher hillCipher)
            {
                hillCipher.SetKey(textBox1.Text);
            }
            if (operationType == OperationType.Encrypt)
                cipher.Encrypt();
            else
                cipher.Decrypt();
        }

        private bool IsMatchingLanguage(string key, string message)
        {
            return (IsEnglish(key) && IsEnglish(message)) || (IsRussian(key) && IsRussian(message));
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public void CheckBoxesEncrypt()
        {
            PerformOperation(OperationType.Encrypt);
        }
    
        public void CheckBoxesDecrypt()
        {
            PerformOperation(OperationType.Decrypt);
        }
    }
}