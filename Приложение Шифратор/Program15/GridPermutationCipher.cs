using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Program15
{
    public class GridPermutationCipher : ICipher
    {
        private Kripto _kripto;

        public GridPermutationCipher(Kripto kripto)
        {
            _kripto = kripto;
        }
        private int GetGridSize(string alphabet)
        {
            if (alphabet == Kripto.EnglishAlphabet)
            {
                return 5; // Сетка для английского алфавита
            }
            else if (alphabet == Kripto.RussianAlphabet)
            {
                return 6; // Сетка для русского алфавита
            }
            throw new InvalidOperationException("Алфавит не поддерживается.");
        }

        public void Encrypt()
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in _kripto.Message)
            {
                char upperSymbol = char.ToUpper(symbol);
                string alphabet = Kripto.EnglishAlphabet.Contains(upperSymbol) ? Kripto.EnglishAlphabet : Kripto.RussianAlphabet;

                var keyGrid = CreateKeyGrid(_kripto.Keyword, alphabet);
                if (keyGrid.ContainsKey(upperSymbol))
                {
                    var (row, col) = keyGrid[upperSymbol];
                    var newChar = keyGrid.FirstOrDefault(x => x.Value == (col, row)).Key;
                    result.Append(char.IsUpper(symbol) ? newChar : char.ToLower(newChar));
                }
                else
                {
                    result.Append(symbol);
                }
            }

            _kripto.UpdateResult(ConvertToNumeric(result.ToString(), Kripto.EnglishAlphabet, Kripto.RussianAlphabet));
        }
        public void Decrypt()
        {
            string keyAlphabet = DetermineAlphabet(_kripto.Keyword);

            var numericValues = _kripto.Message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder decodedMessage = new StringBuilder();

            foreach (var value in numericValues)
            {
                if (int.TryParse(value, out int index))
                {
                    char? decodedChar = ConvertNumericToChar(index, keyAlphabet);
                    if (decodedChar != null)
                    {
                        decodedMessage.Append(decodedChar.Value);
                    }
                }
            }

            StringBuilder result = new StringBuilder();
            foreach (char symbol in decodedMessage.ToString())
            {
                char upperSymbol = char.ToUpper(symbol);
                string symbolAlphabet = Kripto.EnglishAlphabet.Contains(upperSymbol) ? Kripto.EnglishAlphabet : Kripto.RussianAlphabet;

                var keyGrid = CreateKeyGrid(_kripto.Keyword, symbolAlphabet);
                if (keyGrid.ContainsKey(upperSymbol))
                {
                    var (col, row) = keyGrid[upperSymbol];
                    var originalChar = keyGrid.FirstOrDefault(x => x.Value == (row, col)).Key;
                    result.Append(char.IsUpper(symbol) ? originalChar : char.ToLower(originalChar));
                }
                else
                {
                    result.Append(symbol);
                }
            }

            _kripto.UpdateResult(result.ToString());
        }

        private bool IsEnglish(string text)
        {
            return text.All(c => Kripto.EnglishAlphabet.Contains(char.ToUpper(c)) || char.IsWhiteSpace(c));
        }

        private bool IsRussian(string text)
        {
            return text.All(c => Kripto.RussianAlphabet.Contains(char.ToUpper(c)) || char.IsWhiteSpace(c));
        }
        private string DetermineAlphabet(string key)
        {
            bool isEnglish = IsEnglish(key);
            bool isRussian = IsRussian(key);

            if (isEnglish && !isRussian)
                return Kripto.EnglishAlphabet;
            if (isRussian && !isEnglish)
                return Kripto.RussianAlphabet;
            throw new InvalidOperationException("Неопределенный или смешанный язык ключа.");
        }
        private char? ConvertNumericToChar(int index, string alphabet)
        {
            if (index >= 1 && index <= alphabet.Length)
                return alphabet[index - 1];
            return null;
        }
        private string ConvertToNumeric(string input, string englishAlphabet, string russianAlphabet)
        {
            StringBuilder numericResult = new StringBuilder();

            foreach (char c in input)
            {
                int index;
                if (englishAlphabet.Contains(char.ToUpper(c)))
                {
                    index = englishAlphabet.IndexOf(char.ToUpper(c)) + 1;
                }
                else if (russianAlphabet.Contains(char.ToUpper(c)))
                {
                    index = russianAlphabet.IndexOf(char.ToUpper(c)) + 1;
                }
                else
                {
                    numericResult.Append(c); // Для неалфавитных символов
                    continue;
                }

                numericResult.Append(index.ToString() + " ");
            }

            return numericResult.ToString().TrimEnd();
        }
        private Dictionary<char, (int, int)> CreateKeyGrid(string key, string alphabet)
        {
            var gridSize = GetGridSize(alphabet);
            var keyGrid = new Dictionary<char, (int, int)>();
            int index = 0;

            foreach (var c in key.ToUpper().Distinct())
            {
                if (!keyGrid.ContainsKey(c) && alphabet.Contains(c))
                {
                    keyGrid.Add(c, (index / gridSize, index % gridSize));
                    index++;
                }
            }

            foreach (var c in alphabet)
            {
                if (!keyGrid.ContainsKey(c))
                {
                    keyGrid.Add(c, (index / gridSize, index % gridSize));
                    index++;
                }
            }

            return keyGrid;
        }
    }
}