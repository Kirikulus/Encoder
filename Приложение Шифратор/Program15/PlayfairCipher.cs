using System.Linq;
using System;
using System.Windows.Forms;

namespace Program15
{
    public class PlayfairCipher : ICipher
    {
        private Kripto _kripto;
        private char[,] matrix;

        public PlayfairCipher(Kripto kripto)
        {
            _kripto = kripto;
        }
        private string DetermineAlphabet(string keyword)
        {
            keyword = keyword.ToUpper();
            return keyword.Any(ch => Kripto.RussianAlphabet.Contains(ch))
                   ? Kripto.RussianAlphabet : Kripto.EnglishAlphabet;
        }

        private char[,] CreateMatrix(string keyword, string alphabet)
        {
            int matrixSize = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));
            char[,] matrix = new char[matrixSize, matrixSize];
            bool[] alphabetUsed = new bool[alphabet.Length];

            keyword = keyword.ToUpper();

            int x = 0, y = 0;

            foreach (char ch in keyword)
            {
                if (alphabet.Contains(ch))
                {
                    int index = alphabet.IndexOf(ch);
                    if (!alphabetUsed[index])
                    {
                        matrix[y, x] = ch;
                        alphabetUsed[index] = true;

                        x++;
                        if (x == matrixSize)
                        {
                            x = 0;
                            y++;
                        }
                    }
                }
            }

            foreach (char ch in alphabet)
            {
                int index = alphabet.IndexOf(ch);
                if (!alphabetUsed[index])
                {
                    matrix[y, x] = ch;
                    alphabetUsed[index] = true;

                    x++;
                    if (x == matrixSize)
                    {
                        x = 0;
                        y++;
                    }
                }
            }

            return matrix;
        }


        private string PreprocessMessage(string message, string alphabet)
        {
            message = message.ToUpper();
            message = alphabet.Contains('Ё') ? message.Replace("Ё", "Е") : message.Replace("J", "I");
            string pairs = "";

            for (int i = 0; i < message.Length; i++)
            {
                if (!char.IsLetter(message[i]) || !alphabet.Contains(message[i])) continue;

                pairs += message[i];

                if (i < message.Length - 1 && message[i] == message[i + 1])
                    pairs += 'X';

                if (pairs.Length % 2 != 0 && i == message.Length - 1)
                    pairs += 'X';
            }

            return pairs;
        }

        private string TransformPair(char a, char b, bool encrypt)
        {
            string alphabet = _kripto.Keyword.Any(ch => Kripto.RussianAlphabet.Contains(ch))
                              ? Kripto.RussianAlphabet : Kripto.EnglishAlphabet;
            matrix = CreateMatrix(_kripto.Keyword, alphabet);
            int rowA, colA, rowB, colB;
            FindPosition(a, out rowA, out colA);
            FindPosition(b, out rowB, out colB);

            if (rowA == rowB)
            {
                colA = WrapAround(colA + (encrypt ? 1 : -1), matrix.GetLength(1));
                colB = WrapAround(colB + (encrypt ? 1 : -1), matrix.GetLength(1));
            }
            else if (colA == colB)
            {
                rowA = WrapAround(rowA + (encrypt ? 1 : -1), matrix.GetLength(0));
                rowB = WrapAround(rowB + (encrypt ? 1 : -1), matrix.GetLength(0));
            }
            else
            {
                int temp = colA;
                colA = colB;
                colB = temp;
            }

            char charA = matrix[rowA, colA];
            char charB = matrix[rowB, colB];

            return charA.ToString() + charB.ToString();
        }

        private void FindPosition(char ch, out int row, out int col)
        {
            row = 0;
            col = 0;

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    if (matrix[y, x] == ch)
                    {
                        row = y;
                        col = x;
                        return;
                    }
                }
            }
        }

        private int WrapAround(int index, int size)
        {
            return (index + size) % size;
        }

        public void Encrypt()
        {
            string alphabet = DetermineAlphabet(_kripto.Keyword);

            string pairs = PreprocessMessage(_kripto.Message, alphabet);

            string result = "";

            for (int i = 0; i < pairs.Length; i += 2)
            {
                string pair = pairs.Substring(i, 2);
                string encryptedPair = TransformPair(pair[0], pair[1], true);
                result += encryptedPair;
            }

            _kripto.UpdateResult(result);
        }

        public void Decrypt()
        {
            string alphabet = DetermineAlphabet(_kripto.Keyword);

            string result = "";

            for (int i = 0; i < _kripto.Message.Length; i += 2)
            {
                string pair = _kripto.Message.Substring(i, 2);
                string decryptedPair = TransformPair(pair[0], pair[1], false);
                result += decryptedPair;
            }

            _kripto.UpdateResult(result);
        }
    }
}