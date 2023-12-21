namespace Program15
{
    public class SloganCipher : ICipher
    {
        private Kripto _kripto;

        public SloganCipher(Kripto kripto)
        {
            _kripto = kripto;
        }

        public void Encrypt()
        {
            string keywordUpper = _kripto.Keyword.ToUpper();
            string result = "";

            for (int i = 0; i < _kripto.Message.Length; i++)
            {
                char plainChar = _kripto.Message[i];
                char keywordChar = keywordUpper[i % keywordUpper.Length];

                if (char.IsLetter(plainChar))
                {
                    bool isUpper = char.IsUpper(plainChar);
                    int shift = (keywordChar - 'A') % 26;
                    char shiftedChar = ShiftChar(plainChar, shift, isUpper);
                    result += shiftedChar;
                }
                else
                {
                    result += plainChar;
                }
            }
            _kripto.UpdateResult(result);
        }

        public void Decrypt()
        {
            string keywordUpper = _kripto.Keyword.ToUpper();
            string result = "";

            for (int i = 0; i < _kripto.Message.Length; i++)
            {
                char encryptedChar = _kripto.Message[i];
                char keywordChar = keywordUpper[i % keywordUpper.Length];

                if (char.IsLetter(encryptedChar))
                {
                    result += ShiftCharacter(encryptedChar, keywordChar, false);
                }
                else
                {
                    result += encryptedChar;
                }
            }
            _kripto.UpdateResult(result);
        }

        private char ShiftCharacter(char inputChar, char keywordChar, bool encrypt)
        {
            bool isUpper = char.IsUpper(inputChar);
            int alphabetSize = (inputChar >= 'А' && inputChar <= 'Я') || inputChar == 'Ё' ? 33 : 26;
            char baseLetter = isUpper ? (alphabetSize == 33 ? 'А' : 'A') : (alphabetSize == 33 ? 'а' : 'a');
            int shift = GetShiftAmount(keywordChar, isUpper, alphabetSize);

            if (!encrypt)
            {
                shift = alphabetSize - shift;
            }

            if (inputChar == 'ё') return encrypt ? 'ё' : 'ё';
            if (inputChar == 'Ё') return encrypt ? 'Ё' : 'Ё';

            return (char)(((inputChar - baseLetter + shift) % alphabetSize) + baseLetter);
        }

        private int GetShiftAmount(char keywordChar, bool isUpper, int alphabetSize)
        {
            char baseLetter = isUpper ? (alphabetSize == 33 ? 'А' : 'A') : (alphabetSize == 33 ? 'а' : 'a');
            return (keywordChar - baseLetter) % alphabetSize;
        }

        private char ShiftChar(char ch, int shift, bool isUpper)
        {
            int alphabetSize = (ch >= 'А' && ch <= 'Я') || ch == 'Ё' ? 33 : 26;
            char baseChar = isUpper ? (alphabetSize == 33 ? 'А' : 'A') : (alphabetSize == 33 ? 'а' : 'a');

            if (ch == 'ё' || ch == 'Ё') return ch;

            return (char)(((ch + shift - baseChar + alphabetSize) % alphabetSize) + baseChar);
        }
    }
}