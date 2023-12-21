using Program15;
using System.Linq;
using System.Text;

public class CaesarCipher : ICipher
{
    private Kripto _kripto;

    public CaesarCipher(Kripto kripto)
    {
        _kripto = kripto;
    }

    public void Encrypt()
    {
        int key;
        if (!int.TryParse(_kripto.Keyword, out key))
        {
            return;
        }

        StringBuilder result = new StringBuilder();

        foreach (char symbol in _kripto.Message)
        {
            char upperSymbol = char.ToUpper(symbol);
            string alphabet = Kripto.EnglishAlphabet.Contains(upperSymbol) ? Kripto.EnglishAlphabet : Kripto.RussianAlphabet;
            key = key % alphabet.Length;
            if (key < 0)
            {
                key += alphabet.Length;
            }
            if (alphabet.Contains(upperSymbol))
            {
                int offset = alphabet.IndexOf(upperSymbol);
                char newChar = alphabet[(offset + key) % alphabet.Length];
                result.Append(char.IsUpper(symbol) ? newChar : char.ToLower(newChar));
            }
            else
            {
                result.Append(symbol);
            }
        }

        _kripto.UpdateResult(result.ToString());
    }
    public void Decrypt()
    {
        int key;
        if (!int.TryParse(_kripto.Keyword, out key))
        {
            return;
        }

        StringBuilder result = new StringBuilder();

        foreach (char symbol in _kripto.Message)
        {
            char upperSymbol = char.ToUpper(symbol);
            string alphabet = Kripto.EnglishAlphabet.Contains(upperSymbol) ? Kripto.EnglishAlphabet : Kripto.RussianAlphabet;
            key = key % alphabet.Length;
            if (key < 0)
            {
                key += alphabet.Length;
            }
            if (alphabet.Contains(upperSymbol))
            {
                int offset = alphabet.IndexOf(upperSymbol);
                char newChar = alphabet[(offset - key + alphabet.Length * 100) % alphabet.Length];
                result.Append(char.IsUpper(symbol) ? newChar : char.ToLower(newChar));
            }
            else
            {
                result.Append(symbol);
            }
        }

        _kripto.UpdateResult(result.ToString());
    }
}