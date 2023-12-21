namespace Program15
{
    public class Kripto
    {
        private string _keyword;
        private string _message;
        private string _result;

        public const string EnglishAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        public const string RussianAlphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфчцчшщъыьэюя";
        public void SetKeyword(string keyword)
        {
            _keyword = keyword;
            _result = "";
        }

        public void SetMessage(string message)
        {
            _message = message;
            _result = ""; 
        }
        public string GetResult()
        {
            return _result;
        }
        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; _result = ""; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; _result = ""; }
        }

        public string Result
        {
            get { return _result; }
        }
        public void UpdateResult(string newResult)
        {
            _result = newResult;
        }
    }
}