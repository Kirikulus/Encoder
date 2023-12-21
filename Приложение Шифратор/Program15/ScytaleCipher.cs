using System;
using System.Text;

namespace Program15
{
    public class ScytaleCipher : ICipher
    {
        private Kripto _kripto;

        public ScytaleCipher(Kripto kripto)
        {
            _kripto = kripto;
        }

        public void Encrypt()
        {
            string text = _kripto.Message;
            int diameter = Convert.ToInt32(_kripto.Keyword);

            StringBuilder result = new StringBuilder(text.Length);

            for (int i = 0; i < diameter; i++)
            {
                for (int j = i; j < text.Length; j += diameter)
                {
                    result.Append(text[j]);
                }
            }

            _kripto.UpdateResult(result.ToString());
        }

        public void Decrypt()
        {
            string encryptedText = _kripto.Message;
            int diameter = Convert.ToInt32(_kripto.Keyword);

            int length = encryptedText.Length;
            int height = (length + diameter - 1) / diameter;

            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < height; i++)
            {
                for (int j = i; j < length; j += height)
                {
                    result.Append(encryptedText[j]);
                }
            }

            _kripto.UpdateResult(result.ToString());
        }
    }
}