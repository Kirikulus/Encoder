using System;
using System.Text;

namespace Program15
{
    public class HillCipher : ICipher
    {
        private Kripto _kripto;
        private int[,] keyMatrix;

        public HillCipher(Kripto kripto)
        {
            _kripto = kripto;
        }
        public void SetKey(string key)
        {
            keyMatrix = ConvertKeyToMatrix(key);
        }

        private int[,] ConvertKeyToMatrix(string key)
        {
            if (key.Length != 4)
            {
                throw new ArgumentException("Ключ должен содержать ровно 4 символа.");
            }

            int[,] matrix = new int[2, 2];
            for (int i = 0; i < key.Length; i++)
            {
                matrix[i / 2, i % 2] = key[i] - '0'; 
            }
            return matrix;
        }

        public void Encrypt()
        {
            if (keyMatrix == null)
            {
                throw new InvalidOperationException("Ключ не установлен.");
            }
            string plainText = _kripto.Message.ToUpper().Replace(" ", "");
            if (plainText.Length % 2 != 0)
                plainText += "X";

            StringBuilder encryptedText = new StringBuilder();

            for (int i = 0; i < plainText.Length; i += 2)
            {
                int[] charVector = { plainText[i] - 'A', plainText[i + 1] - 'A' };
                int[] encryptedVector = MultiplyMatrixVector(keyMatrix, charVector);
                encryptedText.Append((char)('A' + encryptedVector[0]));
                encryptedText.Append((char)('A' + encryptedVector[1]));
            }
            _kripto.UpdateResult(encryptedText.ToString());
        }

        public void Decrypt()
        {
            string cipherText = _kripto.Message.ToUpper().Replace(" ", "");
            StringBuilder decryptedText = new StringBuilder();
            int[,] inverseMatrix = FindInverseMatrix(keyMatrix);

            for (int i = 0; i < cipherText.Length; i += 2)
            {
                int[] charVector = { cipherText[i] - 'A', cipherText[i + 1] - 'A' };
                int[] decryptedVector = MultiplyMatrixVector(inverseMatrix, charVector);
                decryptedText.Append((char)('A' + decryptedVector[0]));
                decryptedText.Append((char)('A' + decryptedVector[1]));
            }
            _kripto.UpdateResult(decryptedText.ToString().TrimEnd('X'));
        }

        private int[,] FindInverseMatrix(int[,] matrix)
        {
            int a = matrix[0, 0];
            int b = matrix[0, 1];
            int c = matrix[1, 0];
            int d = matrix[1, 1];
            int determinant = a * d - b * c;
            int invDet = ModInverse(determinant, 26);

            return new int[,] {
                { invDet * d % 26, -invDet * b % 26 },
                { -invDet * c % 26, invDet * a % 26 }
            };
        }

        private int ModInverse(int a, int m)
        {
            a %= m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }
            return 1;
        }

        private int[] MultiplyMatrixVector(int[,] matrix, int[] vector)
        {
            int[] result = new int[2];
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    result[i] += matrix[i, j] * vector[j];
                }
                result[i] = (result[i] % 26 + 26) % 26;
            }
            return result;
        }
    }
}